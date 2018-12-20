using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL
{
   public class OpenFormatReportService
    {

        public static DocumentsFilingPM CreateBKMVDATAFile(string openFormatReportId, int tenant)
        {
            OpenFormatReportQueryService openFormatReportQueryService = new OpenFormatReportQueryService(tenant);
            OpenFormatReportPM openFormatReportPM = openFormatReportQueryService.GetSingle(openFormatReportId, false, false);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
            LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(tenant);
            List<B100Data> b100Data = ledgerTransactionQueryService.GetTransactionsByDate(openFormatReportPM.DateTypeCode, openFormatReportPM.FromDate, openFormatReportPM.ToDate, tenant);
            UserQuery userQuery = new UserQuery(tenant);
            CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            List<B110Data> b110Data = gLAccountQueryService.GetB110sForGLAccounts(tenant);
            ComputingPartnerTranslationHelper computingPartnerTranslationHelper = new ComputingPartnerTranslationHelper(tenant);

            List<string> linesArray = new List<string>();



            StringBuilder myStringBuilder = new StringBuilder();

            // A100
            myStringBuilder.Append("A100");
            myStringBuilder.Append("000000001");
            myStringBuilder.Append(tenantPM.VatNumber);
            if (tenantPM.VatNumber != null)
            {
                if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber.Substring(0, 9); }
                myStringBuilder.Append(tenantPM.VatNumber.PadLeft(9, '0'));
            }

            if (openFormatReportPM.ReportNumber != null)
            {
                if (openFormatReportPM.ReportNumber.Length > 15) { openFormatReportPM.ReportNumber.Substring(0, 15); }
                myStringBuilder.Append(openFormatReportPM.ReportNumber.PadLeft(15, '0'));
            }
            myStringBuilder.Append("&OF1.31&");
            myStringBuilder.Append(' ', 50);
            myStringBuilder.Append('\n');
            //B100
            int counter = 1;
            foreach (B100Data item in b100Data)
            {
                counter++;
                myStringBuilder.Append("B100");
                if (counter.ToString().Length > 9)
                {
                    counter.ToString().Substring(0, 9);
                    myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                }

                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber.Substring(0, 9); }
                    myStringBuilder.Append(tenantPM.VatNumber.PadLeft(9, '0'));
                }

                if (item.JournalNumber != null)
                {
                    if (item.JournalNumber.Length > 10) { item.JournalNumber.Substring(0, 10); }
                    myStringBuilder.Append(item.JournalNumber.PadLeft(10, '0'));
                }


                if (item.JournalLineNumber.ToString().Length > 9) { item.JournalLineNumber.ToString().Substring(0, 9); }
                myStringBuilder.Append(item.JournalLineNumber.ToString().PadLeft(9, '0'));

                myStringBuilder.Append(' ', 8);
                myStringBuilder.Append(' ', 15);

                if (item.AccountingEntityReference != null)
                {
                    if (item.AccountingEntityReference.Length > 20) { item.AccountingEntityReference.Substring(0, 20); }
                    myStringBuilder.Append(item.AccountingEntityReference.PadLeft(20, '0'));
                }

             
                var entityPartnerCode = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslation(item.AccountingEntityCode, "ACC", "AccountingEntity");

                if (entityPartnerCode != null)
                {
                    if (entityPartnerCode.Length > 3) { entityPartnerCode.Substring(0, 3); }
                    myStringBuilder.Append(entityPartnerCode.PadLeft(3, '0'));
                }
                else
                {
                    myStringBuilder.Append("000");

                }


                if (item.Reference2 != null)
                {
                    if (item.Reference2.Length > 20) { item.Reference2.Substring(0, 20); }
                    myStringBuilder.Append(item.Reference2.PadLeft(20, '0'));
                }

                myStringBuilder.Append("000");
                if (item.Notes != null)
                {
                    if (item.Notes.Length > 50) { item.Notes.Substring(0, 50); }
                    myStringBuilder.Append(item.Notes.PadLeft(50, '0'));
                }

                var DocumentDate = String.Format("{0:ddMMyyyy}", item.DocumentDate);
                var AccountingDate = String.Format("{0:ddMMyyyy}", item.AccountingDate);
                var CreateDate = String.Format("{0:ddMMyyyy}", item.CreateDate);

                if (AccountingDate.Length > 8) { AccountingDate.Substring(0, 8); }
                myStringBuilder.Append(AccountingDate.PadLeft(8, '0'));

                if (DocumentDate.Length > 8) { DocumentDate.Substring(0, 8); }
                myStringBuilder.Append(DocumentDate.PadLeft(8, '0'));

                if (item.GLAccountDisplayNumber != null)
                {
                    if (item.GLAccountDisplayNumber.Length > 15) { item.GLAccountDisplayNumber.Substring(0, 15); }
                    myStringBuilder.Append(item.GLAccountDisplayNumber.PadLeft(15, '0'));
                }

                if (item.LocalAmountDebit != null)
                {
                    var LocalAmountDebit = "+" + item.LocalAmountDebit;

                    if (LocalAmountDebit.Length > 15) { LocalAmountDebit.Substring(0, 15); }
                    myStringBuilder.Append(LocalAmountDebit.PadLeft(15, '0'));
                }
                else
                {
                    var LocalAmountCredit = "-" + item.LocalAmountCredit;
                    if (LocalAmountCredit.Length > 15) { LocalAmountCredit.Substring(0, 15); }
                    myStringBuilder.Append(LocalAmountCredit.PadLeft(15, '0'));
                }


                if (item.ForeignAmountDebit != null)
                {
                    var ForeignAmountDebit = "+" + item.ForeignAmountDebit;

                    if (ForeignAmountDebit.Length > 15) { ForeignAmountDebit.Substring(0, 15); }
                    myStringBuilder.Append(ForeignAmountDebit.PadLeft(15, '0'));
                }
                else
                {
                    var ForignAmountCredit = "-" + item.ForeignAmountCredit;
                    if (ForignAmountCredit.Length > 15) { ForignAmountCredit.Substring(0, 15); }
                    myStringBuilder.Append(ForignAmountCredit.PadLeft(15, '0'));
                }


                myStringBuilder.Append(' ', 12);
                myStringBuilder.Append(' ', 10);
                myStringBuilder.Append(' ', 10);
                myStringBuilder.Append(' ', 7);

                if (CreateDate.Length > 8) { CreateDate.Substring(0, 8); }
                myStringBuilder.Append(CreateDate.PadLeft(8, '0'));

                UserPM user = userQuery.GetSinglePM(item.CreatedByUser, tenant);
                if (user != null)
                {
                    if (user.LocalName.Length > 8) { user.LocalName.Substring(0, 9); }
                    myStringBuilder.Append(user.LocalName.PadLeft(9, '0'));
                }
                myStringBuilder.Append(' ', 25);


                CurrencyPM currency = currencyQuery.GetSinglePM(item.CurrencyId, tenant);



                var partnerCode = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslation(currency.Code, "Cust", "Currency");

                if (partnerCode != null)
                {
                    if (partnerCode.Length > 3) { partnerCode.Substring(0, 3); }
                    myStringBuilder.Append(partnerCode.PadLeft(3, '0'));
                }

                myStringBuilder.Append('\n');
            }

            //B110

            var trailReportParam = new TrailReportParam()
            {
                Tenant = tenant,
                //    MyRevenueExpenseReportLevel = ReportLevel.,
                ToDate = (DateTime)openFormatReportPM.ToDate,
                FromDate = (DateTime)openFormatReportPM.FromDate,
                CurrenciesDetailed = false,
                DetailedControlVendors = true,
                DetailedControlClients = true,
                Category1 = null,
                Category5 = null,
                Suppress_DoNotShowCardWithoutActivity = false,
                IsRevenueExpenseReport = false,
                MyTrailReportLevel = ReportLevel.GLAccount,

            };


            var typeservice = TrailReportFactory.CreateNew(trailReportParam);
            var res1 = typeservice.Execute();
            typeservice.Dispose();
            var result = res1.Where(d=> d.GLAccountId !=null).ToDictionary(x => x.GLAccountId, x => x);

            foreach (B110Data item in b110Data)
            {
                counter++;

                myStringBuilder.Append("B110");

                if (counter.ToString().Length > 9)
                {
                    counter.ToString().Substring(0, 9);
                    myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                }
                else
                {
                    myStringBuilder.Append(counter.ToString().PadLeft(9, '0'));
                }
                if (tenantPM.VatNumber != null)
                {
                    if (tenantPM.VatNumber.Length > 9) { tenantPM.VatNumber.Substring(0, 9); }
                    myStringBuilder.Append(tenantPM.VatNumber.PadLeft(9, '0'));
                }

                if (item.DisplayNumber != null)
                {
                    if (item.DisplayNumber.Length > 15) { item.DisplayNumber.Substring(0, 15); }
                    myStringBuilder.Append(item.DisplayNumber.PadLeft(15, '0'));
                }

                if (item.LocalName != null)
                {
                    if (item.DisplayNumber.Length > 50) { item.LocalName.Substring(0, 50); }
                    myStringBuilder.Append(item.LocalName.PadLeft(50, '0'));
                }
                else
                {
                    if (item.EnglishName.Length > 50) { item.EnglishName.Substring(0, 50); }
                    myStringBuilder.Append(item.EnglishName.PadLeft(50, '0'));
                }
                if (item.ChartOfAccountsCode != null)
                {
                    if (item.ChartOfAccountsCode.Length > 15) { item.ChartOfAccountsCode.Substring(0, 15); }
                    myStringBuilder.Append(item.ChartOfAccountsCode.PadLeft(15, '0'));
                }

                if (item.ChartOfAccountsName != null)
                {
                    if (item.ChartOfAccountsName.Length > 30) { item.ChartOfAccountsName.Substring(0, 30); }
                    myStringBuilder.Append(item.ChartOfAccountsName.PadLeft(30, '0'));
                }

                if (item.AccountTypeCode == "2" || item.AccountTypeCode == "3")
                {
                    string address = null;
                    var billingaddress = b110Data.Where(d => d.GLAccountId == item.GLAccountId && item.AddressType == "B").FirstOrDefault();
                    if(billingaddress != null)
                    {
                        address = billingaddress.Address1;
                    }
                    if (address == null)
                    {
                       var  mainaddress = b110Data.Where(d => d.GLAccountId == item.GLAccountId && item.AddressType == "M").FirstOrDefault();
                        address = mainaddress.Address1;
                        item.Address1 = address;
                    }
                    if (item.Address1 != null)
                    {
                        if (item.Address1.Length > 50) { item.Address1.Substring(0, 50); }
                        myStringBuilder.Append(item.Address1.PadLeft(50, '0'));
                        if (address.Length > 51)
                        {
                            item.Address2 = address.Substring(51, 60);
                            myStringBuilder.Append(item.Address1.PadLeft(10, '0'));
                        }
                    }

                    if (item.City != null)
                    {
                        if (item.City.Length > 30) { item.City.Substring(0, 30); }
                        myStringBuilder.Append(item.City.PadLeft(30, '0'));
                    }
                    if (item.ZipCode != null)
                    {
                        if (item.ZipCode.Length > 8) { item.ZipCode.Substring(0, 8); }
                        myStringBuilder.Append(item.ZipCode.PadLeft(8, '0'));
                    }

                    if (item.CountryName != null)
                    {
                        if (item.CountryName.Length > 30) { item.CountryName.Substring(0, 30); }
                        myStringBuilder.Append(item.CountryName.PadLeft(30, '0'));
                    }
                    if (item.CountryCode != null)
                    {
                        var partnerCode = computingPartnerTranslationHelper.GetComputingPartnerCodeTranslation(item.CountryCode, "Cust", "Country");

                        if (partnerCode != null)
                        {
                            if (partnerCode.Length > 15) { partnerCode.Substring(0, 15); }
                            myStringBuilder.Append(partnerCode.PadLeft(15, '0'));
                        }

                     
                    }

                }
                else
                {
                    myStringBuilder.Append(' ', 60);
                }

                myStringBuilder.Append(' ', 15);
                TrailReportM trailReportM = null;
                if (result.ContainsKey(item.GLAccountId))
                {
                    trailReportM =  result[item.GLAccountId];
                }
                if (trailReportM != null)
                {
                    item.OpeningBalance = trailReportM.LocalOpenBalance;
                    item.TotalDebit = trailReportM.LocalDebit;
                    item.TotalCredit = trailReportM.LocalCredit;
                    if (item.OpeningBalance != null)
                    {
                        if (item.OpeningBalance.ToString().Length > 15) { item.OpeningBalance.ToString().Substring(0, 15); }
                        myStringBuilder.Append(item.OpeningBalance.ToString().PadLeft(15, '0'));
                    }
                    if (item.TotalDebit != null)
                    {
                        if (item.TotalDebit.ToString().Length > 15) { item.TotalDebit.ToString().Substring(0, 15); }
                        myStringBuilder.Append(item.TotalDebit.ToString().PadLeft(15, '0'));
                    }
                    if (item.TotalCredit != null)
                    {
                        if (item.TotalCredit.ToString().Length > 15) { item.TotalCredit.ToString().Substring(0, 15); }
                        myStringBuilder.Append(item.TotalCredit.ToString().PadLeft(15, '0'));
                    }
                }
                myStringBuilder.Append("0000");
                if (item.VatNumber != null)
                {
                    if (item.VatNumber.Length > 9) { item.VatNumber.Substring(0, 9); }
                    myStringBuilder.Append(item.VatNumber.PadLeft(9, '0'));
                }
                myStringBuilder.Append(' ', 7);

                if(item.IsMultiCurrency== false && item.CurrecnyId != tenantPM.CurrencyId)
                {
                    item.OpeningBalanceInForegnCurrency = trailReportM != null? trailReportM.ForeignOpenBalance:null;
                    if (item.OpeningBalanceInForegnCurrency != null)
                    {
                        if (item.OpeningBalanceInForegnCurrency.ToString().Length > 15) { item.OpeningBalanceInForegnCurrency.ToString().Substring(0, 15); }
                        myStringBuilder.Append(item.OpeningBalanceInForegnCurrency.ToString().PadLeft(15, '0'));
                    }
                    if (item.CurrencyCode != null)
                    {
                        if (item.CurrencyCode.Length> 3) { item.CurrencyCode.Substring(0, 3); }
                        myStringBuilder.Append(item.CurrencyCode.PadLeft(3, '0'));
                    }
                }
                else
                {
                    myStringBuilder.Append(' ', 18);
                }

                myStringBuilder.Append(' ', 16);
                myStringBuilder.Append('\n');
            }

            

            DocumentsFilingPM docOut = CreateDocumnetFiling(myStringBuilder, openFormatReportPM);

            return docOut;



        }

        public static BatchTaskExecutionPM CreateBKMVDATAFileInBatch(string taxReportId, int tenant)
        {
            BatchTaskExecutionPM taskExe;
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                // 1- create BTE record
                PNCFileArgs args = new PNCFileArgs() { ReportId = taxReportId, Tenant = tenant };
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(PNCFileArgs));
                serializer.Serialize(stringwriter, args);
                string xmlParameters = stringwriter.ToString();

                taskExe = new BatchTaskExecutionPM()
                {
                    Subject = "Create a flat file for Open Format Report",
                    Tenant = tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchOpenFormatReportService,Logitude.Accounting.BL",
                    CreateDate = DateTime.Now,
                    PrametersXml = xmlParameters,
                    StatusCode = "C",

                };


                IInfrastructureContext MyContext = InfrastructureContext.GetContext(tenant);
                BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                bteUpdateService.Update(taskExe, true);

                // 2- Send to queue
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
                queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", tenant.ToString() }
                });
                scope.Complete();
            }

            return taskExe;

        }


        private static DocumentsFilingPM CreateDocumnetFiling(StringBuilder lines, OpenFormatReportPM openFormatReport, bool isFromWR = false)
        {
            // prepare file string
            string file = string.Join(Environment.NewLine, lines);

            // create document
            int tenant = openFormatReport.Tenant;
            ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingService docService = new DocumentsFilingService(MyContext, tenant);
            DocumentTypeRepository docTypeReposioty = new DocumentTypeRepository(MyContext);

            ObjectTableRepository tableRep = new ObjectTableRepository(tenant);
            ObjectTable table = tableRep.GetObjectTableByName("OpenFormatReport", 0, true);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);

            // user
            User loggedUser = GetLoggedUser(tenant);
            DocumentType docType = docTypeReposioty.GetSingleDocumentTypeByCode("BKMV", tenant);

            string _code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
           // string name = "A856." + tenantPM.VatNumber + "." + taxDeductionReport.TaxYear.ToString().Substring(1, 3);
            DocumentsFilingPM document = new DocumentsFilingPM()
            {
                Description = "BKMVDATA Text File",
                DocumentTypeId = docType.Id,
                Tenant = tenant,
                DirectionCode = "I",
                EntityId = openFormatReport.Id,
                EntityNumber = openFormatReport.ReportNumber != null ? openFormatReport.ReportNumber.ToString() : null,
                ObjectTableId = table.Id,
                Code = _code,
                CreatedByUserId = loggedUser.Id,
                OwnerId = loggedUser.Id,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdatedByUserId = loggedUser.Id,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                FileExtension = "txt",
                SecurityId = "100",
                FileName = _code,
            };

            byte[] bytearray = Encoding.Unicode.GetBytes(file);
            document.FileData = bytearray;
            docService.Create(document, document.FileData, loggedUser.Id);


            //get document out
            DocumentsFilingQuery queryService = new DocumentsFilingQuery(tenant);
            string code = (Convert.ToInt32(_code) + 1).ToString();
            DocumentsFilingPM docFiling = queryService.GetDocumentsFilingByDocumentCode(code, tenant);


            return docFiling;
        }

        private static Contact GetLoggedContact(int tenant)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact;
            if (HttpContext.Current != null)
            {
                string email = HttpContext.Current.User.Identity.Name;
                loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            }
            else
            {
                string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
                loggedContact = contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);

            }
            return loggedContact;
        }

        private static User GetLoggedUser(int tenant)
        {
            UserRepository userRepository = new UserRepository(tenant);
            User loggedContact;
            if (HttpContext.Current != null)
            {
                string email = HttpContext.Current.User.Identity.Name;
                loggedContact = userRepository.GetSingleUserByCodeOrEmail(null, email, tenant, true);
            }
            else
            {
                string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
                loggedContact = userRepository.GetSingleUserByCodeOrEmail(null, "system@tenant" + tenant + ".com", tenant, true);

            }
            return loggedContact;
        }

    }
}
