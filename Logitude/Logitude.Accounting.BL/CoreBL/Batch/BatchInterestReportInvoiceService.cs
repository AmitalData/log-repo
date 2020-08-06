using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.InterestReport;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.InterestService;
using Logitude.Accounting.BL.InterestService.HelperClasses;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.Resolvers;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    public class BatchInterestReportInvoiceService : BatchTaskExecutionsService
    {
       private InterestReportQueryService interestReportQueryService;
        private UserPM userPM;
        private BatchTaskExecutionPM BatchTaskExecution;
        public BatchInterestReportInvoiceService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
            BatchTaskExecution = batchTaskExecution ;
 
        }
 
        public override void RunCode()
        {
            try
            {
                InterestReportArguments interestReportArgs = GetInterestReportArgs();
                interestReportQueryService = new InterestReportQueryService(interestReportArgs.Tenant);
                UserQuery userQuery = new UserQuery(interestReportArgs.Tenant);
                userPM = userQuery.GetSinglePMByEmail(interestReportArgs.Email, interestReportArgs.Tenant);
                if (userPM==null)
                {
                    userPM = userQuery.GetSinglePMByEmail(interestReportArgs.Email, 0);
                }
                CreateBatchesInvoice(interestReportArgs);
            }
            catch (Exception e)
            {
                throw new Exception("\n" + e.Message + "\n" + e.StackTrace);
            }
        }


        private void CheckAndUpdateInterestLastBatchByTenant(InterestReportArguments interestReportArguments)
        {
            InterestLastBatchServiceQueryService interestLastBatchServiceQueryService = new InterestLastBatchServiceQueryService(interestReportArguments.Tenant);
            InterestLastBatchServicePM interestLastBatchServicePM = interestLastBatchServiceQueryService.CheckInterestLastBatchServicesByTenant(interestReportArguments.Tenant);
            IAccountingContext MyContext = AccountingContext.GetContext(interestReportArguments.Tenant);
            InterestLastBatchServiceUpdateService interestLastBatchServiceUpdateService = new InterestLastBatchServiceUpdateService(MyContext, new Dictionary<string, IContext>(), interestReportArguments.Tenant);

            if (interestLastBatchServicePM == null)
            {
                interestLastBatchServicePM = new InterestLastBatchServicePM();
                interestLastBatchServicePM.Tenant = interestReportArguments.Tenant;
                interestLastBatchServicePM.CreateInvoicesBatchId = BatchTaskExecution.Id;
                interestLastBatchServicePM.ChangeSetOp = ChangeSetOperation.Insert;
                interestLastBatchServiceUpdateService.Update(interestLastBatchServicePM, true);
            }
            else
            {
                interestLastBatchServicePM.CreateInvoicesBatchId = BatchTaskExecution.Id;
                interestLastBatchServicePM.ChangeSetOp = ChangeSetOperation.Update;
                interestLastBatchServiceUpdateService.Update(interestLastBatchServicePM, true);

            }

        }
        private void CreateBatchesInvoice(InterestReportArguments interestReportArguments)
        {
            InterestReportArgs args = new InterestReportArgs();
            args.Tenant = interestReportArguments.Tenant;
            args.Email = interestReportArguments.Email;
            if (interestReportArguments.AllSelected)
            {
                List<InterestReportPM> interestReports = interestReportQueryService.GetNotInvoicedInterestReportsByDates(interestReportArguments.FromDate, interestReportArguments.ToDate, interestReportArguments.Tenant);

                if (interestReportArguments.ExcludedIds != null)
                {
                    interestReports = (from a in interestReports
                                       where !interestReportArguments.ExcludedIds.Contains(a.Id)
                                       select a).ToList();
                }
              
                foreach (InterestReportPM report in interestReports)
                {
                    args.ReportNumber = report.ReportNumber;
                    args.InterestReportId = report.Id;
                    args.Tenant = interestReportArguments.Tenant;
                    if (report.InterestReportStatusCode=="1" || report.InterestReportStatusCode =="9")
                    {
                        UpdateInterestReportsStatues(report, interestReportArguments.Tenant, "8");
                        UpdateAndCreateInvoiceForInterestReport(args, report);
                    }
                   
                }
            }
            else
            {
                foreach (string ReportId in interestReportArguments.SelectedIds)
                {
                    InterestReportPM  interestReport = interestReportQueryService.GetSingle(ReportId, false,true);
                    args.ReportNumber = interestReport.ReportNumber;
                    args.InterestReportId = interestReport.Id;
                    args.Tenant = interestReportArguments.Tenant;
                    if (interestReport.InterestReportStatusCode == "1" || interestReport.InterestReportStatusCode == "9")
                    {
                        UpdateInterestReportsStatues(interestReport, interestReportArguments.Tenant, "8");
                        UpdateAndCreateInvoiceForInterestReport(args, interestReport);
                    }
              

                }
            }
           
        }
        private void UpdateAndCreateInvoiceForInterestReport(InterestReportArgs interestReportArgs, InterestReportPM interestReport)
        {
            try
            {
                interestReport = interestReportQueryService.GetSingle(interestReportArgs.InterestReportId, false, true);
                CreateInvoiceForInterestReport(interestReportArgs, interestReport);
            }

            catch (Exception e)
            {
                BatchTaskExecution.ErrorLog += "\n"+"Report # "+interestReport.ReportNumber + " " + e.Message;
                UpdateInterestReportsStatues(interestReport, interestReportArgs.Tenant, "9",null, e.Message);
            }

          
        }
        private InterestReportArguments GetInterestReportArgs()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(InterestReportArguments));
            InterestReportArguments interestReportArgs = serializer.Deserialize(stringReader) as InterestReportArguments;
            return interestReportArgs;
        }
        private void CreateInvoiceForInterestReport(InterestReportArgs interestReportArgs, InterestReportPM interestReport)
        {
            if (interestReport.TotalAmount == null || interestReport.TotalAmount <= interestReport.GLAccountMinimumInterest)
            {
                IAccountingContext iAccountingContext = AccountingContext.GetContext(interestReportArgs.Tenant);
                InterestReportService interestTransactionQuery = new InterestReportService();
                interestReport = interestTransactionQuery.PutConfirmCreateInvoice(interestReport, interestReportArgs.Tenant, iAccountingContext);
            }
            else
            {
                ARInvoicePM aRInvoicePM = FullMapInvoice(interestReportArgs, interestReport);
                CheckVatNumber(interestReportArgs.Tenant, aRInvoicePM);
                IInvoiceContext invoiceContext = InvoiceContext.GetContext(interestReportArgs.Tenant);
                ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, interestReportArgs.Tenant);
                invoiceService.Create(aRInvoicePM);
                UpdateInterestReportsStatues(interestReport, interestReportArgs.Tenant, "2", aRInvoicePM);
            }
        }
        private void CheckVatNumber(int Tenant, ARInvoicePM aRInvoicePM)
        {
            bool isFieldRequired = false;
            AccountingSettingRepository accountingSettingsRepository = new AccountingSettingRepository(Tenant);
            AccountingSetting iAccountingSetting = accountingSettingsRepository.GetSingleAccountSetting(Tenant);

            if (iAccountingSetting.IsVatNumberMandatoryInAR)
            {
                if (string.IsNullOrEmpty(aRInvoicePM.VatNumber))
                {
                    isFieldRequired = true;
                }
                if (isFieldRequired== true)
                {
                    ContactPM contactLocal = GetLoggedContact(Tenant);
                    bool showLocals = !contactLocal.DontShowLocal;
                    string ErrorMessage = TextCodesTranslator.TranslateText("General.M.FieldIsRequired", Tenant, showLocals);
                    ErrorMessage = ErrorMessage.Replace("%FieldName", TextCodesTranslator.TranslateText("ARInvoice.F.VatNumber", Tenant, showLocals));
                    throw new Exception(ErrorMessage);
                }
             }
        }
        private void UpdateInterestReportsStatues(InterestReportPM interestReportPM, int Tenant, string Statues, ARInvoicePM aRInvoicePM = null, string InvoiceFailureReason=null)
        {
            var accountingContext = AccountingContext.GetContext(Tenant);

            interestReportPM.InterestReportStatusCode = Statues;
            interestReportPM.UpdatedByUserId = userPM.Id;
            interestReportPM.UpdateDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
            if (InvoiceFailureReason!=null)
            {
                interestReportPM.InvoiceFailureReason = InvoiceFailureReason;

            }
            if (aRInvoicePM != null)
            {
                interestReportPM.ARinvoiceId = aRInvoicePM.Id;
                interestReportPM.InvoiceAmount = (decimal?)aRInvoicePM.AmountInLocalCurrency;
            }
            interestReportPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            InterestReportUpdateService service = new InterestReportUpdateService(accountingContext, new Dictionary<string, IContext>(), Tenant);
            service.Update(interestReportPM, true);

        }
        private ARInvoicePM FullMapInvoice(InterestReportArgs interestReportArgs, InterestReportPM interestReport)
        {
            CardQuery cardQueryService = new CardQuery(interestReportArgs.Tenant);
            TenantQuery tenantQuery = new TenantQuery(interestReportArgs.Tenant);
            ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(interestReportArgs.Tenant);
            VatTypePercentageQuery vatTypePercentageQuery = new VatTypePercentageQuery(interestReportArgs.Tenant);
            ObjectTableQuery objectTableQuery = new ObjectTableQuery(interestReportArgs.Tenant);
            CardPM cardPM = cardQueryService.GetSinglePM(interestReport.CustomerId, interestReportArgs.Tenant);
            ChargesTypePM chargesType = chargesTypeQuery.GetSinglePMByCode("INT", interestReportArgs.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(interestReportArgs.Tenant);
            string email = "system@tenant" + interestReportArgs.Tenant.ToString() + ".com";
            AuthenticationUtil.AuthenticatedUserEmail = email;
            string ObjectTableId = objectTableQuery.GetObjectTableIdByName("InterestReport");
            VatTypePercentagePM vatTypePercentagePM = vatTypePercentageQuery.GetVatTypePercentagesForVatType(interestReportArgs.Tenant, chargesType.VatTypeId).ToList()[0];

            ARInvoicePM aRInvoicePM = MappingARInvoice(interestReportArgs, interestReport, tenantPM, userPM, cardPM);
            ARInvoiceEntityPM aRInvoiceEntityPM = MappingARInvoiceEntity(interestReportArgs, ObjectTableId);
            ARInvoiceLinePM aRInvoiceLinePM = MappingARInvoiceLine(interestReport, tenantPM, chargesType, vatTypePercentagePM);

            aRInvoicePM.InvoiceEntities.Add(aRInvoiceEntityPM);
            aRInvoicePM.InvoiceLines.Add(aRInvoiceLinePM);

            return aRInvoicePM;

        }
        private ARInvoicePM MappingARInvoice(InterestReportArgs interestReportArgs, InterestReportPM interestReport, TenantPM tenantPM, UserPM userPM, CardPM cardPM)
        {
            ARInvoicePM aRInvoicePM = new ARInvoicePM();
            aRInvoicePM.ARInvoiceTypeCode = "IT";
            aRInvoicePM.BillToGLAccountId = interestReport.GLAccountId;
            aRInvoicePM.BillToId = interestReport.CustomerId;
            aRInvoicePM.Tenant = interestReportArgs.Tenant;
            aRInvoicePM.BillToPartnerTypeId = "CS";
            aRInvoicePM.AmountInLocalCurrency = (double?)interestReport.TotalAmount;
            aRInvoicePM.LocalCurrencyId = tenantPM.CurrencyId;
            aRInvoicePM.InvoiceCurrencyId = tenantPM.CurrencyId;
            aRInvoicePM.ProfitCurrencyId = tenantPM.ProfitCurrencyId;
            aRInvoicePM.ProfitCurrencyCode = tenantPM.ProfitCurrencyCode;
            aRInvoicePM.InvoiceCurrencyCode = tenantPM.CurrencyCode;
            aRInvoicePM.AmountInInvoiceCurrency = (double?)interestReport.TotalAmount;
            aRInvoicePM.AmountInProfitCurrency = (double?)interestReport.TotalAmount;
            aRInvoicePM.BranchId = userPM.BranchId;
            aRInvoicePM.InvoiceDate = TenantServerConfigration.GetCurrentDateTime(interestReportArgs.Tenant);
            aRInvoicePM.CreateDate = TenantServerConfigration.GetCurrentDateTime(interestReportArgs.Tenant);
            aRInvoicePM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(interestReportArgs.Tenant);
            aRInvoicePM.IssuedByUserId = userPM.Id;
            aRInvoicePM.CreatedByUserId = userPM.Id;
            aRInvoicePM.UpdatedByUserId = userPM.Id;
            aRInvoicePM.MainEntityId = null;
            aRInvoicePM.HouseNumber = null;
            aRInvoicePM.MainEntityReference = null;
            aRInvoicePM.MasterNumber = null;
            aRInvoicePM.Description = null;
            aRInvoicePM.IsGeneralInvoice = true;
            aRInvoicePM.IsFullAccounting = true;
            aRInvoicePM.SetApproved = true;

            if (!string.IsNullOrEmpty(cardPM.SATPaymentMethodCode))
            {
                aRInvoicePM.SATPaymentMethodCode = cardPM.SATPaymentMethodCode;
            }
            if (!string.IsNullOrEmpty(cardPM.InvoiceCurrencyId))
            {
                aRInvoicePM.InvoiceCurrencyId = cardPM.InvoiceCurrencyId;
            }
            if (!string.IsNullOrEmpty(cardPM.PaymentTermId))
            {
                aRInvoicePM.PaymentTermId = cardPM.PaymentTermId;
            }

            if (!string.IsNullOrEmpty(cardPM.VatNumber))
            {
                aRInvoicePM.VatNumber = cardPM.VatNumber;
            }
            if (!string.IsNullOrEmpty(cardPM.BillingAddressId))
            {
                aRInvoicePM.BillToAddressId = cardPM.BillingAddressId;
            }

            else if (!string.IsNullOrEmpty(cardPM.MainAddressId))
            {
                aRInvoicePM.BillToAddressId = cardPM.MainAddressId;
            }

            aRInvoicePM = InitializeDueDate(aRInvoicePM);
            aRInvoicePM = SetCurrencyRateData(aRInvoicePM, tenantPM);

            return aRInvoicePM;
        }
        private ARInvoicePM SetCurrencyRateData(ARInvoicePM aRInvoicePM, TenantPM tenantPM)
        {
            double? myRate = null;
            DateTime? myRateDate = null;

            if (!string.IsNullOrEmpty(aRInvoicePM.InvoiceCurrencyId))
            {
                if (aRInvoicePM.InvoiceCurrencyId == tenantPM.CurrencyId)
                {
                    myRate = 1;
                }

                else
                {
                    List<LastRate> lastRates = GetCurrenciesExchangeRateByValueDate(tenantPM.Id, tenantPM.CurrencyId, aRInvoicePM.InvoiceDate);

                    LastRate SinglelastRate = lastRates.Where(s => s.ForeignCurrencyId == aRInvoicePM.InvoiceCurrencyId).ToList()[0];
                    if (SinglelastRate != null)
                    {
                        myRate = SinglelastRate.Rate;
                        myRateDate = SinglelastRate.ValueDate;
                    }
                }
            }

            aRInvoicePM.InvoiceCurrencyExchangeRate = myRate;
            aRInvoicePM.ProfitCurrencyExchangeRate = myRate;
            aRInvoicePM.ExchangeRateDate = myRateDate;

            return aRInvoicePM;
        }
        private ARInvoiceEntityPM MappingARInvoiceEntity(InterestReportArgs interestReportArgs, string ObjectTableId)
        {
            ARInvoiceEntityPM aRInvoiceEntityPM = new ARInvoiceEntityPM();
            aRInvoiceEntityPM.Tenant = interestReportArgs.Tenant;
            aRInvoiceEntityPM.EntityId = interestReportArgs.InterestReportId;
            aRInvoiceEntityPM.EntityReference = interestReportArgs.ReportNumber;
            aRInvoiceEntityPM.ObjectTableId = ObjectTableId;
            return aRInvoiceEntityPM;
        }
        private ARInvoiceLinePM MappingARInvoiceLine(InterestReportPM interestReport, TenantPM tenantPM, ChargesTypePM chargesType, VatTypePercentagePM vatTypePercentagePM)
        {
            ARInvoiceLinePM aRInvoiceLinePM = new ARInvoiceLinePM();
            aRInvoiceLinePM.Tenant = tenantPM.Id;
            aRInvoiceLinePM.InvoiceLocalCurrencyCode = tenantPM.CurrencyCode;
            aRInvoiceLinePM.ForiegnCurrencyCode = tenantPM.CurrencyCode;
            aRInvoiceLinePM.InvoiceCurrencyCode = tenantPM.CurrencyCode;
            aRInvoiceLinePM.ForiegnCurrencyId = tenantPM.CurrencyId;
            if (interestReport.TotalAmount == null)
            {
                aRInvoiceLinePM.UnitPrice = 0;
                aRInvoiceLinePM.ForiegnCurrencyAmount = 0;
                aRInvoiceLinePM.InvoiceCurrencyAmount = 0;
                aRInvoiceLinePM.ProfitCurrencyAmount = 0;
                aRInvoiceLinePM.LocalCurrencyAmount = 0;
            }
            else
            {
                aRInvoiceLinePM.UnitPrice = (double?)interestReport.TotalAmount;
                aRInvoiceLinePM.ForiegnCurrencyAmount = (double?)interestReport.TotalAmount;
                aRInvoiceLinePM.InvoiceCurrencyAmount = (double?)interestReport.TotalAmount;
                aRInvoiceLinePM.ProfitCurrencyAmount = (double?)interestReport.TotalAmount;
                aRInvoiceLinePM.LocalCurrencyAmount = (double?)interestReport.TotalAmount;
            }
            aRInvoiceLinePM.Quantity = 1;
            aRInvoiceLinePM.Description = "Interest For Date " + interestReport.InterestCalculationDate.ToString("dd/MM/yyyy");
            aRInvoiceLinePM.LocalDescription = "חישוב ריבית לתאריך " + interestReport.InterestCalculationDate.ToString("dd/MM/yyyy");
            aRInvoiceLinePM.ChargesTypeId = chargesType.Id;
            aRInvoiceLinePM.VatTypeId = chargesType.VatTypeId;
            aRInvoiceLinePM.VatPercentage = vatTypePercentagePM.Percentage;
            aRInvoiceLinePM.GLAccountId = interestReport.GLAccountId;
            if (aRInvoiceLinePM.ForiegnCurrencyAmount == null || aRInvoiceLinePM.LocalCurrencyAmount == null || aRInvoiceLinePM.LocalCurrencyAmount == 0)
            {
                aRInvoiceLinePM.ForiegnExchangeRate = 0;
            }
            else
            {
                aRInvoiceLinePM.ForiegnExchangeRate = aRInvoiceLinePM.ForiegnCurrencyAmount / aRInvoiceLinePM.LocalCurrencyAmount;

            }


            return aRInvoiceLinePM;

        }
        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }
        private List<LastRate> GetCurrenciesExchangeRateByValueDate(int tenant, string baseCurrencyId, DateTime? date)
        {

            List<LastRate> resultList = new List<LastRate>();

            if (string.IsNullOrEmpty(baseCurrencyId))
            {
                string msg = TranslateTextsClass.Translate("General.M.AccountingCurrencyIsNotSet", tenant);
                throw new ApplicationException(msg);
            }

            RatesTableRepository ratesTablesRepository = new RatesTableRepository(tenant);
            RatesTableQuery ratesTableQuery = new RatesTableQuery(ratesTablesRepository);
            CurrencyRepository currencyRepository = new CurrencyRepository(tenant);
            Currency baseCurrency = currencyRepository.GetCurrencies(tenant).Where(r => r.Id == baseCurrencyId).FirstOrDefault();
            List<Currency> foreignCurrencies = currencyRepository.GetCurrencies(tenant).Where(c => c.Id != baseCurrencyId).ToList();

            foreach (Currency currency in foreignCurrencies)
            {
                LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, currency.Id, baseCurrencyId, date);
                if (lastRate != null)
                {
                    lastRate.BaseCurrencyId = baseCurrencyId;
                    lastRate.BaseCurrencyCode = baseCurrency.Code;
                    resultList.Add(lastRate);
                }
                else
                {
                    LastRate newLastRate = new LastRate()
                    {
                        Id = IdCounter.GetNumber("LastRate", tenant).ToString(),
                        Tenant = tenant,
                        ForeignCurrencyId = currency.Id,
                        ForeignCurrencyCode = currency.Code,
                        ForeignCurrencyName = currency.EnglishName,
                        BaseCurrencyId = baseCurrency.Id,
                        BaseCurrencyCode = baseCurrency.Code,
                        HistoryCount = 0,
                        Rate = null,
                    };
                    resultList.Add(newLastRate);
                }
            }
            return resultList;
        }
        private ARInvoicePM InitializeDueDate(ARInvoicePM entityPM)
        {
            if (entityPM.DueDate == null)
            {
                if (string.IsNullOrEmpty(entityPM.PaymentTermId))
                {
                    entityPM.DueDate = entityPM.InvoiceDate;
                }

                else
                {
                    PaymentTermRepository paymentTermRepository = new PaymentTermRepository(entityPM.Tenant);
                    PaymentTerm myPaymentTerm = paymentTermRepository.GetSinglePaymentTerm(entityPM.PaymentTermId, entityPM.Tenant);

                    if (myPaymentTerm != null)
                    {
                        if (myPaymentTerm.IsManuallySet)
                        {
                            entityPM.DueDate = null;
                        }

                        else
                        {
                            DateTime? myComparativeDate = null;

                            if (entityPM.IsConsolidationInvoice)
                            {
                                myComparativeDate = entityPM.InvoiceDate;
                            }

                            else
                            {
                                if (myPaymentTerm.FromDateTypeCode == "SHI")
                                {
                                    myComparativeDate = entityPM.OperationalDate;

                                    if (myComparativeDate == null)
                                    {
                                        myComparativeDate = entityPM.InvoiceDate;
                                    }
                                }

                                else
                                {
                                    myComparativeDate = entityPM.InvoiceDate;
                                }
                            }

                            if (myComparativeDate != null)
                            {
                                if (myPaymentTerm.CurrentMonth)
                                {
                                    myComparativeDate = myComparativeDate.Value.AddMonths(1);

                                    int dateYear = myComparativeDate.Value.Year;
                                    int dateMonth = myComparativeDate.Value.Month;
                                    int dateDay = myComparativeDate.Value.Day;
                                    int dateHour = myComparativeDate.Value.Hour;
                                    int dateMinute = myComparativeDate.Value.Minute;
                                    int dateSecond = myComparativeDate.Value.Second;

                                    myComparativeDate = new DateTime(dateYear, dateMonth, 1, dateHour, dateMinute, dateSecond);
                                }

                                DateTime? date = myComparativeDate.Value.AddDays(Convert.ToDouble(myPaymentTerm.Days));

                                if (entityPM.DueDate != date)
                                {
                                    entityPM.DueDate = date;
                                }
                            }
                        }
                    }
                }
            }

            if (entityPM.DueDate != null)
            {
                entityPM.DueDate = entityPM.DueDate.Value.Date;
            }

            return entityPM;
        }
    }
}
