using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class APInvoiceCancellationController : ApiController
    {
        public HttpResponseMessage GetCancel(string externalId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    AuthenticationToken authToken = GetAuthenticationToken();
                    int tenant = authToken.Tenant;
                    SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                    APInvoiceQueryService Service = new APInvoiceQueryService(tenant);
                    APInvoice apinvoice = Service.GetSingleInvoiceByExternalEntityId(externalId, tenant);
                    APInvoicePM apinvoicePM = MapAPInvoiceToAPInvoicePM(apinvoice, tenant);
                    SubmitChanges(apinvoicePM);
                    scope.Complete();
                    return CreateResponse(null, "apinvoice has been voided");
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(ex, null);
            }
        }

        private void SubmitChanges(APInvoicePM apinvoice)
        {
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(apinvoice.Tenant);

            APInvoiceService apinvoiceService = new APInvoiceService(invoiceContext, apinvoice.Tenant);
            apinvoiceService.Update(apinvoice, true);
        }

        private HttpResponseMessage CreateResponse(Exception exception, string message)
        {

            if (exception == null && message != null)
            {

                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(exception);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);


            }
        }

        private APInvoicePM MapAPInvoiceToAPInvoicePM(APInvoice invoice, int tenant)
        {
            APInvoicePM apinvoicePM = null;
            APInvoiceQueryService apinvoiceQuery = new APInvoiceQueryService(tenant);
            if (invoice != null)
            {
                apinvoicePM = apinvoiceQuery.APInvoiceDataMappingAndValidatin(invoice, tenant);
                apinvoicePM = SetAPInvoicePMVoided(apinvoicePM);

            }
            return apinvoicePM;
        }

        private AuthenticationToken GetAuthenticationToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            return authToken;
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


    }
}