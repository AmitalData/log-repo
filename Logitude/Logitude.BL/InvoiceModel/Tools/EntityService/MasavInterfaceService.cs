using CWXSD;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.CloseTables;
using Logitude.BL.InvoiceModel.CoreBL.Batch;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class MasavInterfaceService
    {
        bool isNewEntity;
        private int tenant;
        public MasavInterface Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private MasavInterfacePM entityPM;
        private IInvoiceContext objectContext;
        private MasavInterfaceRepository entityRepository;
        public MasavInterfaceService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new MasavInterfaceRepository(objectContext);
        }

        public void Create(MasavInterfacePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("MasavInterface", tenant).ToString();
            this.Poco = new MasavInterface(); 
            this.Poco.Id = this.entityPM.Id;
            MasavInterfaceMapping.MapEntity(entityPM, Poco, isNewEntity);
            CreateEvent("CREV", "Masav Interface Created" );
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(MasavInterfacePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleMasavInterface(theEntityPm.Id,entityPM.Tenant);
            if (theEntityPm.StatusCode == MasavInterfaceStatusValues.CancellationInProgress && Poco.StatusCode == MasavInterfaceStatusValues.Transmitted)
               CreateCancelTask();               
            if (theEntityPm.StatusCode == MasavInterfaceStatusValues.InProgress)
                CreateTransmissionTask();
            MasavInterfaceMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }
        public void CreateCancelTask()
        {
            try
            {
                string batchId = new MasavBatchCancelTask(null).CreateQBatchTaskExecution<MasavBatchTransmissionTaskArgs>(
                    new MasavBatchTransmissionTaskArgs()
                    {
                        MasavInterfaceId = entityPM.Id,
                        Tenant = tenant
                    }, tenant, "Masav Batch Cancel Task", false);
                CreateEvent("UPEV", "Masav Cancel Task Created. Batch Id: " + batchId);
            }
            catch (Exception ex)
            {
                this.Poco.StatusCode = MasavInterfaceStatusValues.Failed;
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
                throw ex;
            }
        }
        public void CreateTransmissionTask()
        {
            try
            {             
                string batchId = new MasavBatchTransmissionTask(null).CreateQBatchTaskExecution<MasavBatchTransmissionTaskArgs>(
                    new MasavBatchTransmissionTaskArgs()
                    {
                        MasavInterfaceId = entityPM.Id,
                        Tenant = tenant
                    }, tenant, "Masav Batch Transmission Task", false);
                CreateEvent("UPEV", "Masav Transmission Task Created. Batch Id: " + batchId);

            }
            catch (Exception ex)
            {
                Poco.StatusCode = MasavInterfaceStatusValues.Failed;
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
                throw ex;
            }
        }

        public void Transmit(string masavInterfaceId, int tenant)
        {
            Poco = entityRepository.GetSingleMasavInterface(masavInterfaceId, tenant);

            try
            {
                SecurityUtility.IsWorkerRoleCall = true;

                APPaymentService paymentService = new APPaymentService(ObjectContext, tenant);
                List<APPayment> payments = paymentService.GetAPPaymentsByMasavInterfaceId(Poco.Id, tenant);
                IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
                Accounting.Def.EntityPMs.FullAccountingSettingPM accountingSettings = query.GetFullAccountingSettingByTenant(tenant);
                if (accountingSettings != null)
                {
                    ProcessMasavClearingBankTransfer(payments, accountingSettings);
                    CreateMasavFile(accountingSettings.MasavCode, payments);
                    Poco.StatusCode = MasavInterfaceStatusValues.Transmitted;
                    entityRepository.Update(Poco);
                    entityRepository.SubmitChanges();

                }
            }
            catch (Exception ex)
            {
                CreateEvent("FAID", ex.Message);
                Poco.StatusCode = MasavInterfaceStatusValues.Failed;
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();               
            }
        }
        public void ProcessMasavClearingBankTransfer(List<APPayment> payments,FullAccountingSettingPM accountingSettings)
        {
            var failures = new List<PaymentFailure>();
            int tenant = Poco.Tenant;
            string masavGLAcccountId ="";
            string bankGLAccountId = "";
            string bankGLAccountCurrencyId = "";
            double? bankCurrecyRate = null;
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
            BankAccountPM bankAccount = bankAccountQuery.GetByFirstOrDefault(accountingSettings.MasavBankId, Poco.Tenant);
            var ratesTablesRepository = new RatesTableRepository(tenant);
            var ratesTableQuery = new RatesTableQuery(ratesTablesRepository);

            if (bankAccount != null && !string.IsNullOrEmpty(bankAccount.MasavGLAcccountId))
            {
                masavGLAcccountId = bankAccount.MasavGLAcccountId;
                bankGLAccountId = bankAccount.GLAccountId;
                bankGLAccountCurrencyId = bankAccount.GLAccountCurrencyId;
                if(bankGLAccountCurrencyId != tenantPOCO.CurrencyId)
                    bankCurrecyRate = ratesTableQuery.GetLastRecordByValueDateAndExchangeRateId(tenant, bankGLAccountCurrencyId, tenantPOCO.CurrencyId, TenantServerConfigration.GetCurrentDateTime(tenant), bankGLAccountId) ?? 1;
            }
            foreach (var item in payments)
            {
                try
                {
                    CreateJournal(item, masavGLAcccountId, tenantPOCO, bankGLAccountId, bankGLAccountCurrencyId);
                    UpdateAPPaymentStatus(item, APPaymentStatusValues.Paid);
                }
                catch (Exception ex)
                {
                    failures.Add(new PaymentFailure
                    {
                        PaymentId = item.Id.ToString(),
                        Reason = ex.Message
                    });

                }
                
            }
            if (failures.Any())
            {
                Poco.StatusCode = MasavInterfaceStatusValues.Failed;
                CreateEvent(
                    "FAID",
                    BuildMessage(failures)
                );
            }
        }
        public void CreateJournal(APPayment aPPayment,string masavGLAcccountId, Tenant tenantPOCO, string bankGLAccountId,string bankGLAccountCurrencyId)
        {
            IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            JournalPM journalPM = journalQuery.GetJournalsWithLinesByAccountingEntityIdAndCode(aPPayment.Id, AccountingEntityValues.APPayment, aPPayment.Tenant)?.OrderBy(a => a.CreateDate).FirstOrDefault();
            AddJournalAndJournalLines(journalPM, aPPayment,tenantPOCO,masavGLAcccountId,bankGLAccountId, bankGLAccountCurrencyId);
        }
        private void AddJournalAndJournalLines(JournalPM journalPM, APPayment aPPayment, Tenant tenantPOCO, string masavGLAcccountId,string bankGLAccountId,string bankGLAccountCurrencyId)
        {
            var ratesTablesRepository = new RatesTableRepository(tenant);
            var ratesTableQuery = new RatesTableQuery(ratesTablesRepository);


            if (tenantPOCO.AccountingActivated)
                {
                    JournalPM journal = new JournalPM();
                    journal.Tenant = tenant;                    
                    journal.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.AccountingDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.TypeCode = "0";
                    journal.StatusCode = "6";
                    journal.CreatedByUserId = Poco.CreatedByUserId;
                    journal.AccountingEntityCode = AccountingEntityValues.APPayment;
                    journal.AccountingEntityId = aPPayment.Id;
                    journal.AccountingEntityReference = aPPayment.PaymentNo;
                    journal.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.UpdatedByUserId = Poco.UpdatedByUserId;
                    journal.ApproveDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    journal.ApprovedByUserId = Poco.CreatedByUserId;
                    journal.ChangeSetOp = ChangeSetOperation.Insert;                    
                    List<JournalLinePM> journalLines = (from d in journalPM.JournalLines?.Where(a =>a.CreditAccountId == masavGLAcccountId)
                                                        select new JournalLinePM()
                                                        {
                                                            Tenant = tenant,
                                                            ActionCode = d.ActionCode,
                                                            ActionTypeCodeEnum = d.ActionTypeCodeEnum,
                                                            JournalId = journal.Id,
                                                            DebitAccountId = d.CreditAccountId,
                                                            CreditAccountId = bankGLAccountId,
                                                            Line = d.Line,
                                                            DocumentDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                                            AccountingDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                                            DueDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                                            LocalAmount = d.LocalAmount,
                                                            CurrencyId = d.ActionTypeCodeEnum == JournalActionTypeEnum.Credit ? bankGLAccountCurrencyId : d.CurrencyId,
                                                            ForeignAmount = d.ActionTypeCodeEnum == JournalActionTypeEnum.Credit ? 0  : d.ForeignAmount,
                                                            ExchangeRate = (decimal)d.ExchangeRate,
                                                            Reference1 = d.Reference1,
                                                            Reference2 = d.Reference2,
                                                            Reference3 = d.Reference3,
                                                            Notes = d.Notes,
                                                            ExcludeFromTaxReport = d.ExcludeFromTaxReport,
                                                        }).ToList();


                    journal.JournalLines.AddRange(journalLines);
                    IJournalUpdateServiceExt journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalUpdateServiceExt), "JournalUpdateServiceExt", new ParameterOverride(string.Empty, 1)) as IJournalUpdateServiceExt;
                    AddAccountingEntitieJournal(journal, AccountingEntityJournalActions.APPaymentPaid,Poco.Id);
                    journalUpdate.Update(journal);
                }
            
        }

        private void AddAccountingEntitieJournal(JournalPM entityPM, string action, string ChildEntityId = null)
        {
            IAccountingEntityJournalUpdateServiceExt service = ContainerAccessor.Container.Resolve(typeof(IAccountingEntityJournalUpdateServiceExt), "AccountingEntityJournalUpdateServiceExt", new ParameterOverride(string.Empty, 1)) as IAccountingEntityJournalUpdateServiceExt;
            service.AddAccountingEntitieJournal(entityPM, action, ChildEntityId);
        }

        public void UpdateAPPaymentStatus(APPayment aPPayment,string status)
        {
            APPaymentRepository aPPaymentRepository = new APPaymentRepository(ObjectContext);
            aPPayment.StatusCode = status;
            aPPaymentRepository.Update(aPPayment);
            aPPaymentRepository.SubmitChanges();

        }
        public void Cancel(string masavInterfaceId, int tenant)
        {
            Poco = entityRepository.GetSingleMasavInterface(masavInterfaceId, tenant);
            var failures = new List<PaymentFailure>();

            try
            {
                SecurityUtility.IsWorkerRoleCall = true;

                APPaymentService paymentService = new APPaymentService(this.ObjectContext, this.tenant);
                List<APPayment> payments = paymentService.GetAPPaymentsByMasavInterfaceId(this.Poco.Id, this.tenant);
                IFullAccountingSettingQueryServiceExt query = ContainerAccessor.Container.Resolve(typeof(IFullAccountingSettingQueryServiceExt), "FullAccountingSettingQueryServiceExt", new ParameterOverride("", 1)) as IFullAccountingSettingQueryServiceExt;
                Accounting.Def.EntityPMs.FullAccountingSettingPM accountingSettings = query.GetFullAccountingSettingByTenant(tenant);
                if (accountingSettings != null)
                {
                    foreach (var item in payments)
                    {
                        try
                        {
                            CreateStorno(item);
                            UpdateAPPaymentStatus(item, APPaymentStatusValues.Approved);                           
                        }
                        catch (Exception ex)
                        {
                            failures.Add(new PaymentFailure
                            {
                                PaymentId = item.Id.ToString(),
                                Reason = ex.Message
                            });
                        }


                    }
                }
                if (failures.Any())
                {
                    Poco.StatusCode = MasavInterfaceStatusValues.Failed;

                    CreateEvent(
                        "FAID",
                        BuildMessage(failures)
                    );
                }
                else
                {
                    Poco.StatusCode = MasavInterfaceStatusValues.Cancelled;
                }
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
            }
            catch (Exception ex)
            {
                CreateEvent("FAID", ex.Message);
                this.Poco.StatusCode = MasavInterfaceStatusValues.Failed;
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
                throw ex;
            }
        }
        private string BuildMessage(List<PaymentFailure> failures)
        {
            var sb = new StringBuilder();
            sb.AppendLine("process completed with failures:");

            foreach (var f in failures)
            {
                sb.AppendLine($"PaymentId: {f.PaymentId} | Reason: {f.Reason}");
            }

            return sb.ToString();
        }
        public void CreateEvent(string code , string notes)
        {
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = Poco?.Id,
                ObjectTableName = "MasavInterface",
                Tenant = tenant,
                UserId = Poco?.UpdatedByUserId,
                EventTypeCode = code,
                Notes = notes
            });
        }
        public void CreateStorno(APPayment aPPayment)
        {
          IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
          JournalPM journalPM = journalQuery.GetJournalsWithLinesByAccountingEntityIdAndCode(aPPayment.Id, AccountingEntityValues.APPayment, aPPayment.Tenant)?.OrderByDescending(a =>a.CreateDate).FirstOrDefault();
          var journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalVoidUpdateServiceExt), "JournalVoidUpdateServiceExt", new ParameterOverride(string.Empty, 1)) as IJournalVoidUpdateServiceExt;
          AddAccountingEntitieJournal(journalPM, AccountingEntityJournalActions.APPaymentPaidVoid,Poco.Id);
          journalUpdate.Update(journalPM, new StornoOverrideM()
          {
              AccountingEntityCode = AccountingEntityValues.APPayment,
              AccountingEntityId = aPPayment.Id,
              AccountingEntityReference = aPPayment.PaymentNo
          });
          APPaymentRepository paymentRepository = new APPaymentRepository(ObjectContext);
          aPPayment.MasavInterfaceId = null;
          paymentRepository.Update(aPPayment);
          paymentRepository.SubmitChanges();

        }




        private const int RecordLength = 128;

        public DocumentsFilingPM CreateMasavFile(string masavCode,List<APPayment> payments)
        {
            var sb = new StringBuilder();

            string paymentDate = Poco.PaymentDate.ToString("yyMMdd");
            string creationDate = DateTime.Now.ToString("yyMMdd");
            sb.AppendLine(BuildHeader(
                masavCode,
                paymentDate,
                creationDate                
                ));

            foreach (var p in payments)
            {
                sb.AppendLine(BuildAPPayment(masavCode, p));
            }

            sb.AppendLine(BuildTotal(
                masavCode,
                paymentDate,
                payments));
            DocumentsFilingPM docOut = CreateDocumnetFiling(sb, Poco);
            return docOut;

        }
        private string BuildHeader(string masavCode,string paymentDate,string creationDate)
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
            var line = new StringBuilder();

            line.Append("K");                                  
            line.Append(PadNum(masavCode, 8));           
            line.Append("00");                                 
            line.Append(paymentDate);   
            line.Append("0");                                 
            line.Append("001");                               
            line.Append("0");                                 
            line.Append(creationDate);                       
            line.Append(PadNum(masavCode, 5));        
            line.Append(new string('0', 6));                  
            line.Append(PadText(tenantPM.Company, 30));      
            line.Append(new string(' ', 56));                 
            line.Append("KOT");                             

            return FixLength(line);
        }
        private string BuildAPPayment(string masavCode, APPayment p)
        {
            CardQuery cardQuery = new CardQuery(p.Tenant);
            var vendor = cardQuery.GetSinglePM(p.VendorId, p.Tenant);
            if(vendor == null)
                throw new ApplicationException("GL Account not found for Masav Payment. Payment Id: " + p.Id);
            IGLAccountQueryServiceExt gLAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            var glAccount = gLAccountQuery.GetSingleGLAccountPM(vendor.GLAccountId, p.Tenant);
            if(glAccount == null)
                throw new ApplicationException("GL Account not found for Masav Payment. Payment Id: " + p.Id);
            var line = new StringBuilder();            
            line.Append("1");                                 
            line.Append(PadNum(masavCode, 8));
            line.Append("00");                              
            line.Append("000000");                            
            line.Append(PadNum(vendor.BankCodeId, 2));
            line.Append(PadNum(vendor.BankBranch,3));
            line.Append("0000");
            line.Append(PadNum(vendor.AccountNumber, 9));
            line.Append("0");
            line.Append(PadNum(vendor.VatNumber, 9));     
            line.Append(PadText(vendor.EnglishName, 16));   
            line.Append(PadAmount((decimal)p.AmountInPaymentCurrency, 13));       
            line.Append(glAccount.DisplayNumber.PadLeft(20, '0'));   
            line.Append("00000000");                          
            line.Append("000");                               
            line.Append("006");                               
            line.Append(new string('0', 18));
            line.Append("  ");                                 

           return FixLength(line);
        }
        private string BuildTotal(string masavCode,string paymentDate,List<APPayment> payments)
        {
             var line = new StringBuilder();
             double? totalAmount = payments.Sum(p => p.AmountInPaymentCurrency);
             int count = payments.Count;             
             line.Append("5");
             line.Append(PadNum(masavCode, 8));
             line.Append("00");
             line.Append(paymentDate);
             line.Append("0");
             line.Append("001");
             line.Append(PadAmount((decimal)totalAmount, 15));    
             line.Append(new string('0', 15));
             line.Append(PadNum(count.ToString(), 7));
             line.Append(new string('0', 7));
             line.Append(new string(' ', 63));
             
             return FixLength(line);
        }
        private string PadNum(string value, int length)
      => (value ?? "").PadLeft(length, '0').Substring(0, length);

        private string PadText(string value, int length)
            => (value ?? "").PadLeft(length, ' ').Substring(0, length);

        private string PadAmount(decimal amount, int length)
        {
            var val = ((long)(amount * 100)).ToString();
            return val.PadLeft(length, '0');
        }

        private string FixLength(StringBuilder sb)
        {
            if (sb.Length > RecordLength)
                return sb.ToString(0, RecordLength);

            if (sb.Length < RecordLength)
                sb.Append(' ', RecordLength - sb.Length);

            return sb.ToString();
        }

        private DocumentsFilingPM CreateDocumnetFiling(StringBuilder lines, MasavInterface masavInterface)
        {
            string file = string.Join(Environment.NewLine, lines);
            int tenant = masavInterface.Tenant;
            ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingService docService = new DocumentsFilingService(MyContext, tenant);
            DocumentTypeRepository docTypeReposioty = new DocumentTypeRepository(MyContext);

            ObjectTableRepository tableRep = new ObjectTableRepository(tenant);
            ObjectTable table = tableRep.GetObjectTableByName("MasavInterface", 0, true);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
            ContactQuery contactQuery = new ContactQuery(tenant);

            ContactPM contact = contactQuery.GetSinglePM(masavInterface.CreatedByUserId, tenant);
            if (contact == null)
            {
                contact = contactQuery.GetSinglePM(masavInterface.CreatedByUserId, 0);
            }

            DocumentType docType = docTypeReposioty.GetSingleDocumentTypeByCode("Masav", tenant);
            string error = TranslateTextsClass.Translate("Accounting.O.DocumentTypeNotFound", tenant, !contact.DontShowLocal);
            if (error != null)
            {
                error = error.Replace("X", "Masav");
            }
            if (docType == null)
            {
                throw new ApplicationException(error);
            }

            string _code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
            DocumentsFilingPM document = new DocumentsFilingPM()
            {
                Description = "Masav Text File",
                DocumentTypeId = docType.Id,
                Tenant = tenant,
                DirectionCode = "I",
                EntityId = masavInterface.Id,
                EntityNumber = masavInterface.Id ,
                ObjectTableId = table.Id,
                Code = _code,
                CreatedByUserId = contact.Id,
                OwnerId = contact.Id,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdatedByUserId = contact.Id,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                FileExtension = "txt",
                SecurityId = "100",
                FileName = "Masav",
            };

            byte[] bytearray = Encoding.ASCII.GetBytes(file);
            document.FileData = bytearray;
            docService.Create(document, document.FileData, contact.Id);
            DocumentsFilingQuery queryService = new DocumentsFilingQuery(tenant);
            string code = (Convert.ToInt32(_code) + 1).ToString();
            DocumentsFilingPM docFiling = queryService.GetDocumentsFilingByDocumentCode(code, tenant);
            return docFiling;
        }


    }
    public class PaymentFailure
    {
        public string PaymentId { get; set; }
        public string Reason { get; set; }
    }
}
