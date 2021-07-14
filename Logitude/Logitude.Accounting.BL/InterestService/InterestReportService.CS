using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Logitude.Accounting.BL.InterestEntityQueryServices;
using Logitude.Accounting.BL.InterestEntityQueryServices.InterestFactory;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Accounting.BL.InterestService.HelperClasses;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Server.Tools.QueueService;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.InterestService
{
    public class InterestReportService
    {
        private const string invoiced = "2";
        private const string closedWithoutInvoice = "4";
        
        public InterestTransactionsWithTotal GetAllInterestTransactionByDate(string ReportId, DateTime? InterestCalculationDate, int Tenant, IAccountingContext MyContext=null)
        {
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(Tenant);
            }
            InterestTransactionsWithTotal interestTransactionsWithTotal = new InterestTransactionsWithTotal();

            IQueryable<InterestTransactionList> IQueryable_InterestTransactionLists = GetIQueryableInterestTransactionLists(ReportId, Tenant, MyContext);

            List<InterestTransactionList> interestTransactionLists = null;

            if (InterestCalculationDate != null)
            {
                interestTransactionLists = IQueryable_InterestTransactionLists.Where(a => DbFunctions.TruncateTime(a.InterestValueDate) == DbFunctions.TruncateTime(InterestCalculationDate)).ToList();
            }
            else
            {
                interestTransactionLists = IQueryable_InterestTransactionLists.ToList();

            }
 
            interestTransactionsWithTotal = MappingDataInterestTransaction(interestTransactionLists,Tenant);

            return interestTransactionsWithTotal;
        }

        public InterestReportPM PutConfirmCreateInvoice(InterestReportPM interestReportPM, int Tenant, IAccountingContext MainContext, string ARInvoiceId = null, string InvoiceNumber = null, double? AmountInLocalCurrency = null, string InvoiceEntitiId = null)
        {
            if (MainContext == null)
            {
                MainContext = AccountingContext.GetContext(Tenant);
            }

            if (interestReportPM == null)
            {
                interestReportPM = GetSingleInterestReportPM(MainContext, Tenant, InvoiceEntitiId, AmountInLocalCurrency, ARInvoiceId, InvoiceNumber);
            }

            interestReportPM = CloseInterestReportWithoutInvoice(interestReportPM, Tenant, MainContext);  

            return interestReportPM;
        }

        public InterestLastBatchServicePM GetInterestLastBatchServiceByTenant(int tenant)
        {
            InterestLastBatchServiceQueryService interestLastBatchServiceQueryService = new InterestLastBatchServiceQueryService(tenant);
            InterestLastBatchServicePM InterestLastBatchService = interestLastBatchServiceQueryService.CheckInterestLastBatchServicesByTenant(tenant);

            if (InterestLastBatchService != null && !string.IsNullOrEmpty(InterestLastBatchService.CreateInvoicesBatchId))
            {
                BatchTaskExecutionQueryService batchTaskExecutionQueryService = new BatchTaskExecutionQueryService(tenant);
                BatchTaskExecutionPM batchTaskExecutionPM = batchTaskExecutionQueryService.GetSingle(InterestLastBatchService.CreateInvoicesBatchId, false, false);

                if (batchTaskExecutionPM.StatusCode != "D" && batchTaskExecutionPM.StatusCode != "F")
                {
                    ContactPM contact = GetLoggedContact(tenant);
                    bool showLocals = !contact.DontShowLocal;
                    throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.AnotherBatchInvoiceStillInProgress", tenant, showLocals));
                }
            }

            return InterestLastBatchService;
        }
        public string CheckLastBatchAndCreateInvoiceBatch(InterestReportArguments interestReportArgs, int tenant, string email)// reduce the method lines by moving common things DRY
        {
            string BatchId = null;
            InterestLastBatchServiceQueryService interestLastBatchServiceQueryService = new InterestLastBatchServiceQueryService(tenant);
            InterestLastBatchServicePM InterestLastBatchService = interestLastBatchServiceQueryService.CheckInterestLastBatchServicesByTenant(tenant);
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            InterestLastBatchServiceUpdateService interestLastBatchServiceUpdateService = new InterestLastBatchServiceUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);

            if (InterestLastBatchService != null && !string.IsNullOrEmpty(InterestLastBatchService.CreateInvoicesBatchId))
            {
                BatchTaskExecutionQueryService batchTaskExecutionQueryService = new BatchTaskExecutionQueryService(tenant);
                BatchTaskExecutionPM batchTaskExecutionPM = batchTaskExecutionQueryService.GetSingle(InterestLastBatchService.CreateInvoicesBatchId, false, false);

                if (batchTaskExecutionPM.StatusCode == "D" || batchTaskExecutionPM.StatusCode == "F")
                {
                    BatchId = CreateBatchInvoice(interestReportArgs, tenant, email);
                    InterestLastBatchService.CreateInvoicesBatchId = BatchId;
                    InterestLastBatchService.ChangeSetOp = ChangeSetOperation.Update;
                    interestLastBatchServiceUpdateService.Update(InterestLastBatchService, true);
                }
                else
                {
                    ContactPM contact = GetLoggedContact(tenant);
                    bool showLocals = !contact.DontShowLocal;
                    throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.AnotherBatchInvoiceStillInProgress", tenant, showLocals));
                }
            }
            else if (InterestLastBatchService != null && string.IsNullOrEmpty(InterestLastBatchService.CreateInvoicesBatchId))
            {
                BatchId = CreateBatchInvoice(interestReportArgs, tenant, email);
                InterestLastBatchService.CreateInvoicesBatchId = BatchId;
                InterestLastBatchService.ChangeSetOp = ChangeSetOperation.Update;
                interestLastBatchServiceUpdateService.Update(InterestLastBatchService, true);

            }

            else if (InterestLastBatchService == null)
            {
                BatchId = CreateBatchInvoice(interestReportArgs, tenant, email);
                InterestLastBatchService = new InterestLastBatchServicePM();
                InterestLastBatchService.Tenant = tenant;
                InterestLastBatchService.CreateInvoicesBatchId = BatchId;
                InterestLastBatchService.ChangeSetOp = ChangeSetOperation.Insert;
                interestLastBatchServiceUpdateService.Update(InterestLastBatchService, true);

            }

            return BatchId;

        }
        public int CheckNumberOfInterestReportInvoicingWithoutInvoice(InterestReportArguments interestReportArgs,int tenant,string email)
        {

            InterestLastBatchServiceQueryService interestLastBatchServiceQueryService = new InterestLastBatchServiceQueryService(tenant);
            InterestLastBatchServicePM InterestLastBatchService = interestLastBatchServiceQueryService.CheckInterestLastBatchServicesByTenant(tenant);

            if (InterestLastBatchService != null && !string.IsNullOrEmpty(InterestLastBatchService.CreateInvoicesBatchId))
            {
                BatchTaskExecutionQueryService batchTaskExecutionQueryService = new BatchTaskExecutionQueryService(tenant);
                BatchTaskExecutionPM batchTaskExecutionPM = batchTaskExecutionQueryService.GetSingle(InterestLastBatchService.CreateInvoicesBatchId, false, false);

                if (batchTaskExecutionPM.StatusCode != "D" && batchTaskExecutionPM.StatusCode != "F")
                {
                    ContactPM contact = GetLoggedContact(tenant);
                    bool showLocals = !contact.DontShowLocal;
                    throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.AnotherBatchInvoiceStillInProgress", tenant, showLocals));
                }
            }
            interestReportArgs.Tenant = tenant;
            interestReportArgs.Email = email;

            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            int NumberOfInterestReportsWithoutInvoice = interestReportQueryService.GetInterestReportsBySelectedIds(interestReportArgs).Count();

            return NumberOfInterestReportsWithoutInvoice;
        }
        public string CreateBatchInvoice(InterestReportArguments interestReportArgs, int tenant, string email)// move it with CheckLastBatchAndCreateInvoiceBatch
        {
            string BatchId = null;
            interestReportArgs.Tenant = tenant;
            interestReportArgs.Email = email;
            string BatchName = interestReportArgs.CloseWithoutInvoice ? "Create Close Without Invoice Batch" : "Create Batch Invoice";
            BatchId = CreateBatchTaskExecution(interestReportArgs, BatchName, "Logitude.Accounting.BL.CoreBL.Batch.BatchInterestReportInvoiceService,Logitude.Accounting.BL");

            return BatchId;
        }
        private string CreateBatchTaskExecution(InterestReportArguments args, string Subject, string ClassName)
        {
            // 1- create BTE record
            BatchTaskExecutionPM taskExe;

            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(InterestReportArguments));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();


            taskExe = new BatchTaskExecutionPM()
            {
                Subject = Subject,
                Tenant = args.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = ClassName,
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C",

            };


            IInfrastructureContext MyContext = InfrastructureContext.GetContext(args.Tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), args.Tenant);
            bteUpdateService.Update(taskExe, true);

            // 2- Send to queue
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant",  args.Tenant.ToString() }
                }, args.Tenant);


            return taskExe.Id;
        }
        public void CancelInterestTransactionsByARPayment(string EntityId, int Tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(Tenant);
            InterestTransactionRepository interestTransactionRepository = new InterestTransactionRepository(MyContext);
            List<InterestTransaction> AllEntities = interestTransactionRepository.GetAll(Tenant).Where(s => s.InterestEntityTypeCode == "2" && s.EntityId == EntityId).ToList();
            for (int i = 0; i < AllEntities.Count; i++)
            {
                AllEntities[i].IsCancelled = true;
                interestTransactionRepository.Update(AllEntities[i]);
            }
            MyContext.SaveChanges();
        }

        private InterestTransactionsWithTotal MappingDataInterestTransaction(List<InterestTransactionList> interestTransactionLists,int Tenant)// call it Map.....
        {
            decimal TotalLocalAmount = 0;
            InterestTransactionsWithTotal interestTransactionsWithTotal = new InterestTransactionsWithTotal();
            if (interestTransactionLists != null && interestTransactionLists.Count > 0)
            {
                List<string> JournalNumbers = GetAllJournalNumbersByInterestTransactions(interestTransactionLists, Tenant);
                List<string> CurencyCodes = GetAllCurencyCodesByInterestTransactions(interestTransactionLists, Tenant);
                for (int i = 0; i < interestTransactionLists.Count; i++)
                {

                    interestTransactionLists[i] = FillDataInterestTransaction(interestTransactionLists[i],JournalNumbers, CurencyCodes);

                }
                TotalLocalAmount += interestTransactionLists.Sum(s=>s.LocalAmount);
                interestTransactionsWithTotal = FillInterestTransactionsWithTotal(interestTransactionLists, TotalLocalAmount);

            }
            return interestTransactionsWithTotal;

        }

        private List<string> GetAllJournalNumbersByInterestTransactions(List<InterestTransactionList> interestTransactionLists, int Tenant)
        {
            JournalQueryService journalQueryService = new JournalQueryService(Tenant);
            List<string>  JournalNumbers = journalQueryService.GetJournalNumbersByTransactionsList(interestTransactionLists, Tenant);

            return JournalNumbers; ;
        }

        private List<string> GetAllCurencyCodesByInterestTransactions(List<InterestTransactionList> interestTransactionLists, int Tenant)
        {
            CurrencyQuery currencyQuery = new CurrencyQuery(Tenant);
            List<string> CurencyCodes = currencyQuery.GetCurencyCodesByTransactionsList(interestTransactionLists, Tenant);
            return CurencyCodes; 
        }

        private InterestTransactionsWithTotal FillInterestTransactionsWithTotal(List<InterestTransactionList> interestTransactionLists, decimal TotalLocalAmount)
        {
            InterestTransactionsWithTotal interestTransactionsWithTotal = new InterestTransactionsWithTotal();
            interestTransactionsWithTotal.interestTransactionLists = interestTransactionLists;
            decimal? TotalLocal = MethodHelper.Round(TotalLocalAmount, 2);
            if (TotalLocal == null)
            {
                interestTransactionsWithTotal.TotalLocalAmount = 0;
            }
            else
            {
                interestTransactionsWithTotal.TotalLocalAmount = (decimal)MethodHelper.Round(TotalLocalAmount, 2);
            }
            return interestTransactionsWithTotal;

        }

        private InterestTransactionList FillDataInterestTransaction(InterestTransactionList interestTransactionLists, List<string> JournalNumbers, List<string> CurencyCodes)
        {
            InterestEntityResult entityResult = GetInterestEntityResultByInterestTransaction(interestTransactionLists);
         
            interestTransactionLists.JournalNumber = GetSingleJournalnumberByInterestTransaction(entityResult, interestTransactionLists, JournalNumbers);
            interestTransactionLists.InterestEntityNumber = entityResult.EntityNumber;
            interestTransactionLists.InterestEntityType = entityResult.EntityType;
            interestTransactionLists.InterestEntityIconCode = entityResult.EntityTypeCode;
            interestTransactionLists.JournalId = entityResult.JournalId;
            interestTransactionLists.AccountEntityCode = entityResult.AccountCode;
            interestTransactionLists.CurrencyCode = GetSingleCurrencyCodeByInterestTransaction(interestTransactionLists, CurencyCodes);
  
            return interestTransactionLists;
        }

        private InterestEntityResult GetInterestEntityResultByInterestTransaction(InterestTransactionList interestTransactionLists)
        {
            IInterestEntityQueryService interestEntityQueryService = InterestQueryFactory.CreateInstance(interestTransactionLists.InterestEntityTypeCode);
            InterestEntityResult entityResult = interestEntityQueryService.GetInterestEntity(interestTransactionLists);
            return entityResult;
        }

        private string GetSingleCurrencyCodeByInterestTransaction( InterestTransactionList interestTransactionLists,List<string> CurencyCodes)
        {
            if (interestTransactionLists.CurrencyId != null)
            {
                interestTransactionLists.CurrencyCode = (from a in CurencyCodes
                                                         where interestTransactionLists.CurrencyId == a.Split(',')[0]
                                                           select a.Split(',')[1]).FirstOrDefault();
            }

            return interestTransactionLists.CurrencyCode;
        }

        private string GetSingleJournalnumberByInterestTransaction(InterestEntityResult entityResult,InterestTransactionList interestTransactionLists, List<string> JournalNumbers)
        {
            if (entityResult.JournalNumber == null)
            {
                interestTransactionLists.JournalNumber = (from a in JournalNumbers
                                                          where interestTransactionLists.InterestEntityTypeCode == a.Split(',')[2]
                                                                 && interestTransactionLists.EntityId == a.Split(',')[1]
                                                          select a.Split(',')[0]).FirstOrDefault();
            }
            else
            {
                interestTransactionLists.JournalNumber = entityResult.JournalNumber;
            }

            return interestTransactionLists.JournalNumber;
        }
   
        private InterestReportPM CloseInterestReportWithoutInvoice(InterestReportPM interestReportPM, int Tenant, IAccountingContext MainContext)
        {
            ContactQuery contactQuery = new ContactQuery(Tenant);
            ContactPM contact = GetLoggedContact(Tenant) ?? new ContactPM();
            bool showLocals = !contact.DontShowLocal;


            InterestReportUpdateService InterestReportUpdate = new InterestReportUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);

            //interestReportPM.CloseBalance = 0;
            //if (interestReportPM.OpenBalance != null)
            //{
            //    interestReportPM.CloseBalance += interestReportPM.OpenBalance;
            //}
            //if (interestReportPM.TotalAmount != null)
            //{
            //    interestReportPM.CloseBalance += interestReportPM.TotalAmount;
            //}
            if (interestReportPM.InterestReportStatusCode != invoiced)
            {
                interestReportPM.InterestReportStatusCode = closedWithoutInvoice;
            }
            if (interestReportPM.InterestReportStatusCode != null)
            {
                InterestReportStatuseRepository interestReportStatuseRepository = new InterestReportStatuseRepository(Tenant);
                InterestReportStatuse interestReportStatuse = interestReportStatuseRepository.GetSingle(interestReportPM.InterestReportStatusCode);
                interestReportPM.InterestReportStatusName = showLocals ? interestReportStatuse.LocalName : interestReportStatuse.EnglishName;
                interestReportPM.InterestReportStatusLocalName = showLocals ? interestReportStatuse.LocalName : interestReportStatuse.EnglishName;
            }

            interestReportPM.ChangeSetOp = ChangeSetOperation.Update;
            InterestReportUpdate.Update(interestReportPM, true);
            UpdateInterestTransactionIsClosedAndMapingInterestReportId(interestReportPM, Tenant, MainContext);

            return interestReportPM;
        }

        private InterestReportPM GetSingleInterestReportPM(IAccountingContext MainContext ,int Tenant, string InvoiceEntitiId, double? AmountInLocalCurrency, string ARInvoiceId , string InvoiceNumber)
        {
            InterestReportRepository interestReportRepository = new InterestReportRepository(MainContext);
            InterestReport interestReport = interestReportRepository.GetSingle(InvoiceEntitiId, Tenant);
            InterestReportPM interestReportPM = new InterestReportPM()
            {
                Id = interestReport.Id,
                InterestReportStatusCode = "2",
                ARinvoiceId = ARInvoiceId,
                ARInvoiceNumber = InvoiceNumber,
                OpenBalance = interestReport.OpenBalance,
                CloseBalance = interestReport.CloseBalance,
                TotalAmount = interestReport.TotalAmount,
                UpdatedByUserId = interestReport.UpdatedByUserId,
                UpdateDateTime = interestReport.UpdateDateTime,
                Tenant = interestReport.Tenant,
                CalCreditAllotmentCommission = interestReport.CalCreditAllotmentCommission,
                CreateDateTime = interestReport.CreateDateTime,
                CreatedByUserId = interestReport.CreatedByUserId,
                CustomerId = interestReport.CustomerId,
                GLAccountId = interestReport.GLAccountId,
                GLAccountInterestCreditLimit = interestReport.GLAccountInterestCreditLimit,
                ReportNumber = interestReport.ReportNumber,
                InterestCalculationDate = interestReport.InterestCalculationDate,
                SearchFields = interestReport.SearchFields,
                InvoiceAmount = (decimal?)AmountInLocalCurrency,

            };

            return interestReportPM;
        }

        private void UpdateInterestTransactionIsClosedAndMapingInterestReportId(InterestReportPM interestReportPM, int Tenant, IAccountingContext MainContext)
        {
            InterestTransactionUpdateService InterestTransactionUpdate = new InterestTransactionUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);

            List<InterestTransactionPM> interestTransactionPMs = GetAllInterestTransactionForReport(interestReportPM.Id, Tenant, MainContext).ToList();

            for (int i = 0; i < interestTransactionPMs.Count; i++)
            {
                interestTransactionPMs[i].IsClosed = true;
                interestTransactionPMs[i].InterestReportId = interestReportPM.Id;
                interestTransactionPMs[i].ChangeSetOp = ChangeSetOperation.Update;
                InterestTransactionUpdate.Update(interestTransactionPMs[i], true);
            }
        }

        private IQueryable<InterestTransactionPM> GetAllInterestTransactionForReport( string InterestReportId,int Tenant, IAccountingContext MyContext)
        {
            IQueryable<InterestTransactionPM> interestTransactionLists = (from a in MyContext.InterestReportLines
                                                                          where a.Tenant == Tenant && a.InterestReportId == InterestReportId
                                                                          select new InterestTransactionPM()
                                                                          {
                                                                              Id =a.InterestTransaction.Id,

                                                                              Tenant = a.Tenant,

                                                                              CurrencyId = a.InterestTransaction.CurrencyId,

                                                                              InterestEntityTypeCode = a.InterestTransaction.InterestEntityTypeCode,

                                                                              EntityId = a.InterestTransaction.EntityId,

                                                                              LocalAmount = a.InterestTransaction.LocalAmount,

                                                                              ForeignAmount = a.InterestTransaction.ForeignAmount,

                                                                              InterestValueDate = a.InterestTransaction.InterestValueDate,

                                                                              InterestReportId = a.InterestTransaction.InterestReportId,

                                                                              IsClosed = a.InterestTransaction.IsClosed,

                                                                              GLAccountId = a.InterestTransaction.GLAccountId,

                                                                              CreateDateTime = a.InterestTransaction.CreateDateTime,

                                                                              UpdateDateTime = a.InterestTransaction.UpdateDateTime,
                                                                        
                                                                              OriginalEntityLineNumber = a.InterestTransaction.OriginalEntityLineNumber,

                                                                              SearchFields = a.InterestTransaction.SearchFields,
 
                                                                          });
            return interestTransactionLists;

        }

        public IQueryable<InterestTransactionList> GetIQueryableInterestTransactionLists(string ReportId,int Tenant, IAccountingContext MyContext=null)
        {
            if (MyContext==null)
            {
                MyContext = AccountingContext.GetContext(Tenant);
            }

            IQueryable<InterestTransactionList> interestTransactionLists = (from a in MyContext.InterestReportLines.Include("InterestTransaction").Include("InterestTransaction.GLAccount")
                                                                            where a.Tenant == Tenant  &&a.InterestReportId == ReportId && a.InterestTransaction.GLAccountId !=null && a.InterestTransaction.GLAccount.ActiveForInterest ==true   && a.InterestTransaction.IsCancelled == false
                                                                            select new InterestTransactionList()
                                                                            {
                                                                                Tenant = a.Tenant,

                                                                                CurrencyId = a.InterestTransaction.CurrencyId,

                                                                                InterestEntityTypeCode = a.InterestTransaction.InterestEntityTypeCode,

                                                                                EntityId = a.InterestTransaction.EntityId,

                                                                                LocalAmount = a.InterestTransaction.LocalAmount,

                                                                                ForeignAmount = a.InterestTransaction.ForeignAmount,

                                                                                InterestValueDate = a.InterestTransaction.InterestValueDate,

                                                                            });
            return interestTransactionLists;

        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }





    }


    public class InterestTransactionsWithTotal
    {
        public List<InterestTransactionList> interestTransactionLists { get; set; }
        public decimal TotalLocalAmount { get; set; }

    }
}