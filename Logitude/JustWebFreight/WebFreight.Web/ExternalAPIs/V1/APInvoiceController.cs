using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel; 
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
namespace WebFreight.Web.ExternalAPIs.V1
{
    public class APInvoiceController : ApiController
    {

        public HttpResponseMessage GetSingleAPInvoice(string id, string number, string externalId)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);


                APInvoiceQueryService Service = new APInvoiceQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = new APInvoice();
                if (!string.IsNullOrEmpty(id))
                {
                    Result = Service.GetAPInvoiceById(id, tenant);
                }
                else if(!string.IsNullOrEmpty(number))
                {
                    Result = Service.GetAPInvoiceByInvoiceNumber(number, tenant);

                }
                else if (!string.IsNullOrEmpty(externalId))
                {
                    Result = Service.GetSingleInvoiceByExternalEntityId(externalId, tenant);
                }


                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }


        public HttpResponseMessage Post(APInvoice apinvoice)
        {
            APInvoice oldEntity = apinvoice;

            if (ModelState.IsValid)
            {
                try
                {

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        int tenant = authToken.Tenant;

                        SecurityUtility.AuthenticateAPICall(authToken.Tenant);

                        if (apinvoice != null) oldEntity = LogitudeXmlSerializer.DeserializeObject<APInvoice>(LogitudeXmlSerializer.SerializeObjectToXmlString(apinvoice));

                        IInvoiceContext MyContext = InvoiceContext.GetContext(tenant);
                        APInvoiceQueryService apinvoiceQuery = new APInvoiceQueryService(tenant);
                        apinvoice.Tenant = tenant;
                        apinvoiceQuery.CustomeValidateAPInvoice(apinvoice);
                        apinvoiceQuery.APInvoiceCustomDataMapping(apinvoice, tenant);

                        //
                        APInvoicePM apinvoicePM = apinvoiceQuery.APInvoiceDataMappingAndValidatin(apinvoice, tenant);

                        apinvoiceQuery.PaymentTermMapAndValidate(apinvoice, apinvoicePM, tenant);

                        // VendorGLAccountId
                        CardQuery cardQuery = new CardQuery(tenant);
                        CardPM vendor = cardQuery.GetSinglePM(apinvoicePM.VendorId, tenant);
                        apinvoicePM.VendorGLAccountId = vendor.GLAccountId;

                        // SET approved
                        apinvoicePM.SetVoided = false;
                        apinvoicePM.SetApproved = true;
                        apinvoicePM.SetReTransfer = false;
                        apinvoicePM.SetCancelApproval = false;

                        if (apinvoicePM.TransferStatusCode == null)
                            apinvoicePM.TransferStatusCode = "NR";

                        MapLines(apinvoice, tenant, apinvoicePM);

                        APInvoiceService apinvoiceService = new APInvoiceService(MyContext, tenant);
                        apinvoiceService.Create(apinvoicePM);


                        APIHelper.AddCommunicationLog("D", oldEntity, apinvoice, "APInvoice", apinvoicePM.Id, "APInvoice API", tenant);





                        scope.Complete();


                        return Request.CreateResponse(HttpStatusCode.OK, apinvoice);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "APInvoice", null, "APInvoice API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }

            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "APInvoice", null, "APInvoice API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        private void MapLines(APInvoice apinvoice, int tenant, APInvoicePM apinvoicePM)
        {
            foreach (APInvoiceLinePM line in apinvoicePM.InvoiceLines)
            {
                line.Tenant = apinvoice.Tenant;

                if (line.InvoiceCurrencyAmount == null)
                    line.InvoiceCurrencyAmount = 0;

                if (line.ForiegnCurrencyId == null)
                    line.ForiegnCurrencyId = apinvoicePM.InvoiceCurrencyId;

                // ForiegnExchangeRate
                if (line.ForiegnCurrencyId == apinvoicePM.InvoiceCurrencyId)
                {
                    line.ForiegnExchangeRate = apinvoice.InvoiceCurrencyExchangeRate;
                    line.ProfitCurrencyAmount = line.ForiegnCurrencyAmount;
                }
                else
                {
                    line.ForiegnExchangeRate = GetRateByTenantAndCurrency(line.ForiegnCurrencyId, GetTenantPM(tenant));
                    line.ProfitCurrencyAmount = line.LocalCurrencyAmount / apinvoice.ProfitCurrencyExchangeRate;
                }

                // vat
                VatTypePercentageQuery vatTypePercentageQuery = new VatTypePercentageQuery(tenant);
                VatTypePercentagePM vat = vatTypePercentageQuery.GetVatTypePercentagesForVatType(tenant, line.VatTypeId).FirstOrDefault();
                line.VatPercentage = vat?.Percentage;

                VatTypeQuery vatTypeQuery = new VatTypeQuery(tenant);
                VatTypePM vatType = vatTypeQuery.GetSinglePM(line.VatTypeId, line.Tenant);
                line.VatRecognizedPercentage = vatType.RecognizedPercentage/100;

                // LocalCurrencyAmount,ForiegnCurrencyAmount
                double? valueInLocal = line.InvoiceCurrencyAmount * apinvoice.InvoiceCurrencyExchangeRate;
                line.LocalCurrencyAmount = valueInLocal.Value;
                line.ForiegnCurrencyAmount = valueInLocal / line.ForiegnExchangeRate;

                // ProfitCurrencyAmount
                if (line.ForiegnCurrencyId == apinvoicePM.InvoiceCurrencyId)
                    line.ProfitCurrencyAmount = line.ForiegnCurrencyAmount;
                else
                    line.ProfitCurrencyAmount = line.LocalCurrencyAmount / apinvoice.ProfitCurrencyExchangeRate;

                // charge types
                // get charges
                ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(tenant);
                ChargesTypePM charge = chargesTypeQuery.GetSinglePM(line.ChargesTypeId, tenant);

                if (string.IsNullOrWhiteSpace(line.Description))
                    line.Description = charge.EnglishName;
                if (string.IsNullOrWhiteSpace(line.LocalDescription))
                    line.LocalDescription = charge.LocalName ?? charge.EnglishName;

                line.ChargeTypeGLAccountId = charge.PayableDebitGLAcountId;
            }
        }

        private TenantPM GetTenantPM(int tenant)
        {
            TenantQuery tenantQuery = new TenantQuery();
            TenantPM tenantPM = TenantQuery.GetSingleTenantPM(tenant);
            return tenantPM;
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

        public HttpResponseMessage GetCancel(string externalId)
        {
          
                try
                {

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        int tenant = authToken.Tenant;

                        SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                        APInvoiceQueryService Service = new APInvoiceQueryService(tenant);
                        APInvoice apinvoice = Service.GetSingleInvoiceByExternalEntityId(externalId, tenant);
                        APInvoicePM apinvoicePM = null;
                        APInvoiceQueryService apinvoiceQuery = new APInvoiceQueryService(tenant);
                        if (apinvoice != null)
                        {
                        apinvoicePM = apinvoiceQuery.APInvoiceDataMappingAndValidatin(apinvoice, tenant);
                        apinvoicePM = SetAPInvoicePMVoided(apinvoicePM);
                        
                        }
                    SubmitChanges(apinvoicePM);
                     scope.Complete();


                    return Request.CreateResponse(HttpStatusCode.OK, "apinvoice has been voided");
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            
            
        }
        private APInvoicePM SetAPInvoicePMVoided(APInvoicePM apinvoicePM)
        {
          
            apinvoicePM.SetVoided = true;
            apinvoicePM.SetApproved = false;
            apinvoicePM.SetReTransfer = false;
            apinvoicePM.SetCancelApproval = false;
            apinvoicePM.SetReSendQBO = false;
            return apinvoicePM;
        }

        private void SubmitChanges(APInvoicePM apinvoice)
        {
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(apinvoice.Tenant);

            APInvoiceService apinvoiceService = new APInvoiceService(invoiceContext, apinvoice.Tenant);
            apinvoiceService.Update(apinvoice, true);
        }
    }
}