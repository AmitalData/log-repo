using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Card = Simplog.Data.CommonDataModel.EntityPOCOs.Card;
using Currency = Simplog.Data.CommonDataModel.EntityPOCOs.Currency;
using PaymentTerm = Logitude.BL.CommonDataModel.APIDataContract.ApiV1.PaymentTerm;
using User = Simplog.Data.CommonDataModel.EntityPOCOs.User;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
    public partial class APInvoiceQueryService
    {
        public APInvoicePM APInvoiceCustomDataMappingAndValidating(APInvoice MyEntity, int tenant, string ComputingPartnerCode = "")
        {
            try
            {
                ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(commonDataContext);
                UserRepository userRepository = new UserRepository(commonDataContext);
                TenantRepository tenantRepository = new TenantRepository(commonDataContext);

                AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountingSetting(tenant);
                Tenant myTenant = tenantRepository.GetSingleTenant(tenant);
                User myUser = userRepository.GetSingleUserByEmail("system@tenant" + tenant + ".com", tenant, false);

                APInvoicePM temp = APInvoiceDataMappingAndValidatin(MyEntity, tenant, ComputingPartnerCode);
                temp.Tenant = tenant;
                temp.StatusCode = "WA";
                temp.CreatedByUserId = myUser.Id;
                temp.UpdatedByUserId = myUser.Id;
                temp.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                temp.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                temp.SubTotalInLocalCurrency = 0;
                temp.SubTotalInInvoiceCurrency = 0;

                if (string.IsNullOrEmpty(temp.BranchId))
                {
                    temp.BranchId = myUser.BranchId;
                }

                if (string.IsNullOrEmpty(temp.PaymentTermId))
                {
                    temp.PaymentTermId = myTenant.PaymentTermId;
                }

                if (string.IsNullOrEmpty(temp.LocalCurrencyId))
                {
                    temp.LocalCurrencyId = myTenant.CurrencyId;
                }

                if (string.IsNullOrEmpty(temp.VendorId))
                {
                    throw new ApplicationException("Vendor is required");
                }
                else
                {
                    CardRepository cardRepository = new CardRepository(commonDataContext);
                    Card vendor = cardRepository.GetSingleCard(temp.VendorId, tenant);
                    if (vendor != null)
                    {
                        if (string.IsNullOrEmpty(temp.VATNumber))
                        {
                            temp.VATNumber = vendor.VatNumber;
                        }

                        if (string.IsNullOrEmpty(temp.InvoiceCurrencyId))
                        {
                            temp.InvoiceCurrencyId = vendor.InvoiceCurrencyId;
                        }

                        if (string.IsNullOrEmpty(temp.PaymentTermId))
                        {
                            temp.PaymentTermId = vendor.PaymentTermId;
                        }
                    }
                }

                if (string.IsNullOrEmpty(temp.InvoiceNumber))
                {
                    throw new ApplicationException("Invoice Number is required");
                }

                if(temp.AmountInInvoiceCurrency == null || temp.AmountInInvoiceCurrency == 0)
                {
                    throw new ApplicationException("Invoice Amount is required");
                }
                else
                {
                    temp.InvoiceExpectedAmount = temp.AmountInInvoiceCurrency;
                }

                if (string.IsNullOrEmpty(temp.InvoiceCurrencyId))
                {
                    throw new ApplicationException("Invoice Currency is required");
                }

                else
                {
                    if(temp.InvoiceCurrencyId == temp.LocalCurrencyId)
                    {
                        if (temp.InvoiceCurrencyExchangeRate != null && temp.InvoiceCurrencyExchangeRate != 0)
                        {
                            if(temp.InvoiceCurrencyExchangeRate != 1)
                            {
                                throw new ApplicationException("Invoice Currency Exchange Rate should be 1 when Invoice Currency same as Local Currency");
                            }
                        }
                    }
                }

                if (accountingSetting != null && accountingSetting.IsVatNumberMandatoryInAP)
                {
                    if (string.IsNullOrEmpty(temp.VATNumber))
                    {
                        throw new ApplicationException("VAT Number is required");
                    }
                }

                if (string.IsNullOrEmpty(temp.PaymentTermId))
                {
                    throw new ApplicationException("Payment Term is required");
                }

                this.SetCurrencyRateData(temp);

                if (temp.InvoiceCurrencyExchangeRate == null || temp.InvoiceCurrencyExchangeRate == 0)
                {
                    throw new ApplicationException("Invoice Currency Exchange Rate is required");
                }

                if(temp.InvoiceDate != null)
                {
                    this.ComputeAPInvoiceDueDate(temp);
                }

                else
                {
                    throw new ApplicationException("Invoice Date is required");
                }
                
                if (!string.IsNullOrEmpty(temp.MainEntityReference))
                {
                    ShipmentQuery shipmentRepository = new ShipmentQuery(tenant);
                    ShipmentPM shipment = shipmentRepository.GetSinglePMByShipmentNumber(temp.MainEntityReference, tenant, false);
                    if(shipment != null)
                    {
                        if(shipment.IsAccountingClosed)
                        {
                            throw new ApplicationException("The Shipment is Accounting Closed");
                        }

                        temp.MainEntityId = shipment.Id;
                        temp.HouseNumber = shipment.House;
                        temp.MasterNumber = shipment.LongMaster;
                        temp.ProfitCurrencyId = shipment.ProfitCurrencyId;
                        //temp.ProfitCurrencyExchangeRate = this.GetCurrencyRate(this.EntityPM.ProfitCurrencyId);
                        temp.OperationalDate = shipment.OperationalDate;

                        switch (shipment.DirectionId)
                        {
                            case "E": { temp.Description = "Export to " + shipment.MainCarriageFinalDestinationPortCode; break; }
                            case "I": { temp.Description = "Import from " + shipment.MainCarriageFromPortCode; break; }
                            case "D": { temp.Description = "Ship to " + shipment.ToPartnerCity; break; }
                        }
                        
                        if (string.IsNullOrEmpty(temp.BranchId))
                        {
                            temp.BranchId = shipment.BranchId;
                        }
                    }

                    else
                    {
                        throw new ApplicationException("No Shipment Found");
                    }
                }

                foreach (APInvoiceLinePM line in temp.InvoiceLines)
                {
                    if(string.IsNullOrEmpty(line.ForiegnCurrencyId))
                    {
                        line.ForiegnCurrencyId = temp.InvoiceCurrencyId;
                    }

                    else
                    {
                        if(line.ForiegnCurrencyId != temp.InvoiceCurrencyId)
                        {
                            throw new ApplicationException("Line Currency is Different than Invoice Currency");
                        }
                    }

                    line.EntityReference = temp.MainEntityReference;
                    line.EntityId = temp.MainEntityId;
                }

                return temp;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SetCurrencyRateData(APInvoicePM invoice)
        {
            if (!string.IsNullOrEmpty(invoice.InvoiceCurrencyId))
            {
                if (invoice.InvoiceCurrencyId == invoice.LocalCurrencyId)
                {
                    invoice.InvoiceCurrencyExchangeRate = 1;
                }

                else
                {
                    DateTime? loadingDate = invoice.InvoiceDate;
                    if (loadingDate == null)
                    {
                        loadingDate = TenantServerConfigration.GetCurrentDateTime(invoice.Tenant);
                    }
                    
                    LastRate myRate = this.GetCurrencysExchangeRate(invoice.Tenant, invoice.LocalCurrencyId, invoice.InvoiceCurrencyId, loadingDate.Value);
                    if (myRate != null)
                    {
                        invoice.InvoiceCurrencyExchangeRate = myRate.Rate;
                        invoice.ExchangeRateDate = myRate.ValueDate;
                    }
                }
            }
        }
        private LastRate GetCurrencysExchangeRate(int tenant, string localCurrencyId, string invoiceCurrencyId, DateTime rateDate)
        {
            LastRate result = null;
            
            RatesTableQuery ratesTableQuery = new RatesTableQuery(tenant);
            CurrencyRepository currencyRepository = new CurrencyRepository(tenant);

            LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, invoiceCurrencyId, localCurrencyId, rateDate);
            if (lastRate != null)
            {
                result = lastRate;
            }

            return result;
        }
        public void ComputeAPInvoiceDueDate(APInvoicePM invoice)
        {
            if (string.IsNullOrEmpty(invoice.PaymentTermId))
            {
                invoice.DueDate = invoice.InvoiceDate;
            }

            else
            {
                PaymentTermRepository paymentTermRepository = new PaymentTermRepository(invoice.Tenant);
                Simplog.Data.CommonDataModel.EntityPOCOs.PaymentTerm paymentTerm = paymentTermRepository.GetSinglePaymentTerm(invoice.PaymentTermId, invoice.Tenant);
                if (paymentTerm != null)
                {
                    if (paymentTerm.IsManuallySet)
                    {
                        invoice.DueDate = null;
                    }

                    else if (paymentTerm.Days == 0)
                    {
                        DateTime? myComparativeDate = null;

                        if (invoice.IsMultipleEntities)
                        {
                            myComparativeDate = invoice.InvoiceDate;
                        }

                        else
                        {
                            if (paymentTerm.FromDateTypeCode == "SHI")
                            {
                                myComparativeDate = invoice.OperationalDate;

                                if (myComparativeDate == null)
                                {
                                    myComparativeDate = invoice.InvoiceDate;
                                }
                            }

                            else
                            {
                                myComparativeDate = invoice.InvoiceDate;
                            }
                        }

                        if (invoice.DueDate != myComparativeDate)
                        {
                            invoice.DueDate = myComparativeDate;
                        }
                    }

                    else
                    {
                        DateTime? myComparativeDate = null;

                        if (invoice.IsMultipleEntities)
                        {
                            myComparativeDate = invoice.InvoiceDate;
                        }

                        else
                        {
                            if (paymentTerm.FromDateTypeCode == "SHI")
                            {
                                myComparativeDate = invoice.OperationalDate;

                                if (myComparativeDate == null)
                                {
                                    myComparativeDate = invoice.InvoiceDate;
                                }
                            }

                            else
                            {
                                myComparativeDate = invoice.InvoiceDate;
                            }
                        }

                        if (myComparativeDate != null)
                        {
                            int dateYear = myComparativeDate.Value.Year;
                            int dateMonth = myComparativeDate.Value.Month + 1;
                            int dateDay = myComparativeDate.Value.Day;

                            if (paymentTerm.CurrentMonth)
                            {
                                dateMonth += 1;
                                dateDay = 1;
                            }

                            var myDate = new DateTime(dateYear, dateMonth, dateDay, 0, 0, 0);                            
                            myComparativeDate = myDate;
                            myComparativeDate = myComparativeDate.Value.AddDays(paymentTerm.Days);

                            if (invoice.DueDate != myComparativeDate)
                            {
                                invoice.DueDate = myComparativeDate;
                            }
                        }
                    }
                }
            }
        }
        
        public APInvoice GetAPInvoiceByInvoiceNumber(string number, int tenant)
        {
            try
            {


                //var temp = query.GetSinglePM(null, tenant, number);
                var temp = query.GetSinglePMByNumber(number, tenant);
                if (temp == null)
                    throw new ApplicationException("APInvoice with number " + number + " doesn't exist");

                return APInvoiceDataMapping(temp, tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public APInvoice GetAPInvoiceByInternalNumber(string number, int tenant)
        {
            try
            {
                APInvoicePM temp = query.GetSinglePMByInternalNumber(number, tenant);

                if (temp == null)
                {
                    throw new ApplicationException("APInvoice with internal number " + number + " doesn't exist");
                }

                return APInvoiceDataMapping(temp, tenant);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public APInvoice GetSingleInvoiceByExternalEntityId(string externalId, int tenant)
        {
            try
            {
                var temp = query.GetSingleInvoiceByExternlaEntityId(externalId, tenant);
                if (temp == null)
                    throw new ApplicationException("APInvoice with external ID " + externalId + " doesn't exist");

                return APInvoiceDataMapping(temp, tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void APInvoiceCustomDataMapping(APInvoice apinvoice, int tenant)
        {
            apinvoice.Tenant = tenant;
            apinvoice.InvoiceExpectedAmount = apinvoice.AmountInInvoiceCurrency;

            if (apinvoice.InvoiceCurrencyExchangeRate == null)
            {
                apinvoice.InvoiceCurrencyExchangeRate = GetInvoiceCurrencyExchangeRate(apinvoice, tenant);
            }

            TenantPM tenantPM = GetTenantPM(tenant);
            if (apinvoice.LocalCurrency == null)
            {
                apinvoice.LocalCurrency = GetCurrency(tenantPM?.CurrencyId, tenant);
            }
            if (apinvoice.ProfitCurrency == null)
            {
                apinvoice.ProfitCurrency = GetCurrency(tenantPM?.ProfitCurrencyId, tenant);
                apinvoice.ProfitCurrencyExchangeRate = GetRateByTenantAndCurrency(apinvoice.ProfitCurrency.Id, tenantPM);
            }

            apinvoice.AmountDue = apinvoice.AmountInInvoiceCurrency == null ? 0 : apinvoice.AmountInInvoiceCurrency;
            apinvoice.IsExternalEntity = true;
            apinvoice.IsGeneralInvoice = true;

            if (apinvoice.AccountingDate == null)
            {
                apinvoice.AccountingDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            }
        }

        public void PaymentTermMapAndValidate(APInvoice apinvoice, APInvoicePM apinvoicePM, int tenant)
        {
            if (apinvoice.PaymentTerm == null && apinvoice.DueDate != null)
            {
                double daysDifference = GetDaysDiffernceForDate(apinvoice.DueDate, tenant);
                var paymentTerm = GetPaymentTermByDaysDifference(tenant, daysDifference);
                if (paymentTerm == null)
                    apinvoice.PaymentTerm = GetManuallySetPaymentTerm(tenant);
                else
                    apinvoice.PaymentTerm = paymentTerm;
            }
            else if (apinvoice.PaymentTerm != null && apinvoice.DueDate != null)
            {
                double daysDifference = GetDaysDiffernceForDate(apinvoice.DueDate, tenant);
                if (apinvoice.PaymentTerm.Days != daysDifference)
                {
                    apinvoice.PaymentTerm = GetManuallySetPaymentTerm(tenant);
                }
            }
            else if (apinvoice.PaymentTerm != null && apinvoice.DueDate == null)
            {
                int daysDifference = apinvoice.PaymentTerm.Days;
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                DateTime dueDate = new DateTime(todayDate.Year, todayDate.Month, todayDate.Day + daysDifference, 0, 0, 0);

                apinvoicePM.DueDate = dueDate;
            }
            else
            {
                throw new ApplicationException("Neither Due Date nor Payment Term is provided!");
            }

            if (apinvoicePM.PaymentTermId == null)
                apinvoicePM.PaymentTermId = apinvoice.PaymentTerm?.Id;
        }

        private static PaymentTerm GetManuallySetPaymentTerm(int tenant)
        {
            PaymentTermQuery paymentTermQuery = new PaymentTermQuery(tenant);
            PaymentTermPM paymentTerm = paymentTermQuery.GetSinglePMByExternalId("MS", tenant);

            PaymentTerm manuallySetPaymentTerm = new PaymentTerm()
            {
                Days = paymentTerm.Days,
                EnglishName = paymentTerm.EnglishName,
                Id = paymentTerm.Id,
                LocalName = paymentTerm.LocalName,
                ExternalId = paymentTerm.ExternalId,
            };
            return manuallySetPaymentTerm;
        }

        private static double GetDaysDiffernceForDate(DateTime? dueDateTime, int tenant)
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime todayDate = new DateTime(todayDateTime.Year, todayDateTime.Month, todayDateTime.Day, 0, 0, 0);
            DateTime dueDate = new DateTime(dueDateTime.Value.Year, dueDateTime.Value.Month, dueDateTime.Value.Day, 0, 0, 0);
            double daysDifference = (dueDate - todayDate).TotalDays;
            return daysDifference;
        }

        private static PaymentTerm GetPaymentTermByDaysDifference(int tenant, double daysDifference)
        {
            PaymentTermQuery paymentTermQuery = new PaymentTermQuery(tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.PaymentTerm paymentTerm = paymentTermQuery.GetSingleByDaysDifference((int)daysDifference, tenant);

            PaymentTerm paymentTermAPI = null;
            if (paymentTerm != null)
            {
                paymentTermAPI = new PaymentTerm()
                {
                    Days = paymentTerm.Days,
                    EnglishName = paymentTerm.EnglishName,
                    Id = paymentTerm.Id,
                    LocalName = paymentTerm.LocalName,
                    ExternalId = paymentTerm.ExternalId,
                };
            }
            return paymentTermAPI;
        }

        public void CustomeValidateAPInvoice(APInvoice apinvoice)
        {
            if (apinvoice.InvoiceDate == null)
            {
                throw new ApplicationException("InvoiceDate is not provided");
            }

            if (apinvoice.AccountingDate == null)
            {
                throw new ApplicationException("AccountingDate is not provided");
            }

            if (apinvoice.Vendor == null)
            {
                throw new ApplicationException("Vendor is not provided");
            }

            if (apinvoice.VATNumber == null)
            {
                throw new ApplicationException("VATNumber is not provided");
            }

            if (apinvoice.AmountInInvoiceCurrency == null)
            {
                throw new ApplicationException("AmountInInvoiceCurrency is not provided");
            }

            if (apinvoice.InvoiceCurrency == null)
            {
                throw new ApplicationException("InvoiceCurrency is not provided");
            }

            if (apinvoice.Branch == null)
            {
                throw new ApplicationException("Branch is not provided");
            }

            foreach (APInvoiceLine line in apinvoice.InvoiceLines)
            {
                if (line.ChargesType == null)
                {
                    throw new ApplicationException("ChargesType is not provided");
                }

                if (line.InvoiceCurrencyAmount == null)
                {
                    throw new ApplicationException("InvoiceCurrencyAmount is not provided");
                }

                if (line.VatType == null)
                {
                    throw new ApplicationException("VatType is not provided");
                }
            }

            this.CheckIfExternlaEntiityIdExist(apinvoice);

            // validate totals
            double SubTotalInLocalCurrency = Math.Round(apinvoice.InvoiceLines.Sum(d => d.LocalCurrencyAmount).Value, 2);
            double linesInvoiceAmount = Math.Round(apinvoice.InvoiceLines.Sum(d => d.InvoiceCurrencyAmount.Value), 2);

            // commented to allow code to calculate totals with vat.
            //if(apinvoice.AmountInInvoiceCurrency != linesInvoiceAmount)
            //{
            //    throw new ApplicationException("Invoice Amount field doesnt match the total amount");
            //}
        }

        private void CheckIfExternlaEntiityIdExist(APInvoice apinvoice)
        {

            if (apinvoice.ExternalAccountingEntityId != null)
            {
                APInvoicePM invoice = query.GetSingleInvoiceByExternlaEntityId(apinvoice.ExternalAccountingEntityId, apinvoice.Tenant);
                if (invoice != null)
                {
                    throw new Exception("invoice with the same externla id already exist!");
                }
            }

        }

        private double GetInvoiceCurrencyExchangeRate(APInvoice apinvoice, int tenant)
        {
            CurrencyPM invoiceCurrency = GetInvoiceCurrency(apinvoice, tenant);
            TenantPM tenantPM = GetTenantPM(tenant);

            return GetRateByTenantAndCurrency(invoiceCurrency.Id, tenantPM);
        }

        private double GetRateByTenantAndCurrency(string currencyId, TenantPM tenantPM)
        {
            var tenant = tenantPM.Id;
            double rate;
            if (currencyId == tenantPM?.CurrencyId)
            {
                rate = 1;

            }
            else
            {
                RatesTableQuery ratesTableQuery = new RatesTableQuery(tenant);
                LastRate lastRate = ratesTableQuery.GetLastRecord(tenant, currencyId, tenantPM?.CurrencyId);
                rate = (double)lastRate.Rate;
            }

            return rate;
        }

        private static TenantPM GetTenantPM(int tenant)
        {
            //TenantQuery tenantQuery = new TenantQuery();
            TenantPM tenantPM = TenantQuery.GetSingleTenantPM(tenant);
            return tenantPM;
        }

        private static CurrencyPM GetInvoiceCurrency(APInvoice apinvoice, int tenant)
        {
            CurrencyQueryService InvoiceCurrencyCurrencyService = new CurrencyQueryService(tenant);
            CurrencyPM invoiceCurrency = InvoiceCurrencyCurrencyService.CurrencyDataMappingAndValidatin(apinvoice.InvoiceCurrency, tenant, "");
            return invoiceCurrency;
        }

        private CommonDataModel.APIDataContract.ApiV1.Currency GetCurrency(string id, int tenant)
        {
            CurrencyQueryService currencyQuery = new CurrencyQueryService(tenant);
            CommonDataModel.APIDataContract.ApiV1.Currency currency = currencyQuery.GetCurrencyById(id, tenant);
            return currency;
        }
    }
}
