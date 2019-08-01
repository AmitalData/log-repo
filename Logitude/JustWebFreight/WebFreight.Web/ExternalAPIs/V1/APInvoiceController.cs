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

        public HttpResponseMessage GetSingleAPInvoice(string id, string number)
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
                else
                {
                    Result = Service.GetAPInvoiceByInvoiceNumber(number, tenant);
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
                        int tenant = apinvoice.Tenant;

                        SecurityUtility.AuthenticateAPICall(authToken.Tenant);

                        if (apinvoice != null) oldEntity = LogitudeXmlSerializer.DeserializeObject<APInvoice>(LogitudeXmlSerializer.SerializeObjectToXmlString(apinvoice));

                        IInvoiceContext MyContext = InvoiceContext.GetContext(apinvoice.Tenant);
                        APInvoiceQueryService apinvoiceQuery = new APInvoiceQueryService(apinvoice.Tenant);
                        APInvoicePM apinvoicePM = apinvoiceQuery.APInvoiceDataMappingAndValidatin(apinvoice, apinvoice.Tenant);

                        

                        APInvoiceService apinvoiceService = new APInvoiceService(MyContext, apinvoice.Tenant);
                        apinvoiceService.Create(apinvoicePM);


                        APIHelper.AddCommunicationLog("D", oldEntity, apinvoice, "APInvoice", apinvoicePM.Id, "APInvoice API", apinvoice.Tenant);





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

    }
}