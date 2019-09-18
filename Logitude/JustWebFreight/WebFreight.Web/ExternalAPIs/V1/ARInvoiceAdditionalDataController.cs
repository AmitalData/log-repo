using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.APIDataContract;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
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
using System.Web.Http.ModelBinding;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class ARInvoiceAdditionalDataController : ApiController
    {
        ARInvoiceAdditionalData oldEntity;
        public HttpResponseMessage Put(ARInvoiceAdditionalData invoiceAdditionalData)
        {
            ARInvoiceAdditionalData oldEntity = invoiceAdditionalData;

            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        AuthenticationToken authToken = GetAuthenticationToken();
                        
                        int tenant = authToken.Tenant;
                        SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                        if (invoiceAdditionalData != null)
                        {
                            oldEntity = LogitudeXmlSerializer.DeserializeObject<ARInvoiceAdditionalData>(LogitudeXmlSerializer.SerializeObjectToXmlString(invoiceAdditionalData));
                        }
                        
                        ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(tenant);
                        ARInvoicePM invoice = invoiceQuery.GetSingleInvoiceByInvoiceNumber(invoiceAdditionalData.InvoiceNumber, tenant);
                        DocumentsFiling documentsFiling = GetDocumentsFiling(invoiceAdditionalData.DocumentFilingId, tenant);
                        if (documentsFiling != null)
                        {
                            IpdateARInvocie(invoice, invoiceAdditionalData);
                        }
                        else
                        {
                            throw new Exception("Document filing with id " + invoiceAdditionalData.DocumentFilingId + " does not exist");
                        }
                        APIHelper.AddCommunicationLog("D", oldEntity, invoiceAdditionalData, "ARInvoice", invoice.Id, "ARInvoiceAdditionalData API", authToken.Tenant);

                        scope.Complete();


                        return Request.CreateResponse(HttpStatusCode.OK, invoiceAdditionalData);
                    }
                }

                catch (Exception exception)
                {
                    return CreateResponse(exception, null);
                }
            }
            else
            {
                return CreateResponse(null, ModelState);
               
            }
        }

        private void IpdateARInvocie(ARInvoicePM invoice, ARInvoiceAdditionalData invoiceAdditionalData)
        {
            IInvoiceContext MyContext = InvoiceContext.GetContext(invoice.Tenant);
            invoice.DocumentFilingId = invoiceAdditionalData.DocumentFilingId;

            ARInvoiceService service = new ARInvoiceService(MyContext, invoice.Tenant);
            service.Update(invoice, true);
        }
        private DocumentsFiling GetDocumentsFiling(string documentFilingId, int tenant)
        {
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(tenant);
          return documentsFilingRepository.GetSingleDocumentsFiling(documentFilingId, tenant);

        }

        private AuthenticationToken GetAuthenticationToken()
        {

            string token = HttpContext.Current.Request.Headers["Token"];
        
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            return authToken;
        }

        private HttpResponseMessage CreateResponse(Exception exception, ModelStateDictionary modelState)
        {
            APIExceptionResult apiExceptionResult =null;
            if (exception != null && modelState == null)
            {
                 apiExceptionResult = ApiExceptionHandler.HandleException(exception);
            }

            else{
                apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
            }
            APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "ARInvoice", null, "ARInvoice API");
            return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);


            
         
        }
    }
}