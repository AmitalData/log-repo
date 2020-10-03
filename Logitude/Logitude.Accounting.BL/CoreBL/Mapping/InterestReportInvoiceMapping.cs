using Logitude.Accounting.BL.CoreBL.InterestReport;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Mapping
{
    public class InterestReportInvoiceMapping
    {
        public ARInvoicePM MapARInvoice(InterestReportArgs interestReportArgs, InterestReportPM interestReport, TenantPM tenantPM, UserPM userPM, CardPM cardPM)
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
            //aRInvoicePM.InvoiceDate = TenantServerConfigration.GetCurrentDateTime(interestReportArgs.Tenant);
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
            aRInvoicePM.HasInterestFeature = true;
            aRInvoicePM.InvoiceDate = interestReportArgs.InvoiceDate;
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
        public ARInvoiceEntityPM MapARInvoiceEntity(InterestReportArgs interestReportArgs, string ObjectTableId)
        {
            ARInvoiceEntityPM aRInvoiceEntityPM = new ARInvoiceEntityPM();
            aRInvoiceEntityPM.Tenant = interestReportArgs.Tenant;
            aRInvoiceEntityPM.EntityId = interestReportArgs.InterestReportId;
            aRInvoiceEntityPM.EntityReference = interestReportArgs.ReportNumber;
            aRInvoiceEntityPM.ObjectTableId = ObjectTableId;
            return aRInvoiceEntityPM;
        }
        public ARInvoiceLinePM MapARInvoiceLine(InterestReportPM interestReport, TenantPM tenantPM, ChargesTypePM chargesType, VatTypePercentagePM vatTypePercentagePM)
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
            //aRInvoiceLinePM.Description = "Interest For Date " + interestReport.InterestCalculationDate.ToString("dd/MM/yyyy");
            //aRInvoiceLinePM.LocalDescription = "חישוב ריבית לתאריך " + interestReport.InterestCalculationDate.ToString("dd/MM/yyyy");
            aRInvoiceLinePM.Description = "Interest between ";//+ interestReport.InterestReportLinesByDates.First().FromDate.ToString("dd/MM/yyyy") + " and " + interestReport.InterestReportLinesByDates.Last().ToDate.ToString("dd/MM/yyyy");
            aRInvoiceLinePM.LocalDescription = "ריבית לתאריכים ";// + interestReport.InterestReportLinesByDates.First().FromDate.ToString("dd/MM/yyyy") + " עד " + interestReport.InterestReportLinesByDates.Last().ToDate.ToString("dd/MM/yyyy");
            aRInvoiceLinePM.ChargesTypeId = chargesType.Id;
            aRInvoiceLinePM.VatTypeId = chargesType.VatTypeId;
            aRInvoiceLinePM.VatPercentage = vatTypePercentagePM.Percentage;
            aRInvoiceLinePM.GLAccountId = chargesType.ReceivableCreditGLAccountId;
            if (aRInvoiceLinePM.ForiegnCurrencyAmount == null || aRInvoiceLinePM.LocalCurrencyAmount == null || aRInvoiceLinePM.LocalCurrencyAmount == 0)
            {
                aRInvoiceLinePM.ForiegnExchangeRate = 0;
            }
            else
            {
                aRInvoiceLinePM.ForiegnExchangeRate = aRInvoiceLinePM.ForiegnCurrencyAmount / aRInvoiceLinePM.LocalCurrencyAmount;

            }
            aRInvoiceLinePM.LineActionCode = "1";

            return aRInvoiceLinePM;

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
        private ARInvoicePM InitializeDueDate(ARInvoicePM entityPM)// make sure it goes with the new adjustment
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
