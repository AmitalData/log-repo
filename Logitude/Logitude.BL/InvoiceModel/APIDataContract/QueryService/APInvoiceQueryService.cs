using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
     public partial class APInvoiceQueryService
    {


        public APInvoice GetAPInvoiceByInvoiceNumber(string number, int tenant)
        {
            try
            {


                //var temp = query.GetSinglePM(null, tenant, number);
                var temp = query.GetSinglePMByNumber(number,tenant);
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
            try { 
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
                apinvoice.InvoiceCurrencyExchangeRate = GetInvoiceCurrencyExchangeRate(apinvoice, tenant);


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
                apinvoice.AccountingDate = TenantServerConfigration.GetCurrentDateTime(tenant);

 
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

            if(apinvoicePM.PaymentTermId == null)
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
                throw new ApplicationException("InvoiceDate is not provided");

            if (apinvoice.AccountingDate == null)
                throw new ApplicationException("AccountingDate is not provided");

            if(apinvoice.Vendor == null)
                throw new ApplicationException("Vendor is not provided");

            
            if (apinvoice.VATNumber == null)
                throw new ApplicationException("VATNumber is not provided");


            if (apinvoice.AmountInInvoiceCurrency == null)
                throw new ApplicationException("AmountInInvoiceCurrency is not provided");


            if (apinvoice.InvoiceCurrency == null)
                throw new ApplicationException("InvoiceCurrency is not provided");


            //if (apinvoice.DueDate == null)
            //    throw new ApplicationException("DueDate is not provided");

            if (apinvoice.Branch == null)
                throw new ApplicationException("Branch is not provided");

            foreach (APInvoiceLine line in apinvoice.InvoiceLines)
            {
                if (line.ChargesType == null)
                    throw new ApplicationException("ChargesType is not provided");
                if (line.InvoiceCurrencyAmount == null)
                    throw new ApplicationException("InvoiceCurrencyAmount is not provided");
                if (line.VatType == null)
                    throw new ApplicationException("VatType is not provided"); 

            }
            CheckIfExternlaEntiityIdExist(apinvoice);
            
            // validate totals
            double SubTotalInLocalCurrency =    Math.Round(apinvoice.InvoiceLines.Sum(d => d.LocalCurrencyAmount).Value, 2);
            double linesInvoiceAmount =  Math.Round(apinvoice.InvoiceLines.Sum(d => d.InvoiceCurrencyAmount.Value), 2);

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

        private Currency GetCurrency(string id, int tenant)
        {
            CurrencyQueryService currencyQuery = new CurrencyQueryService(tenant);
            Currency currency = currencyQuery.GetCurrencyById(id, tenant);
            return currency;
        }
    }
}
