using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;

using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using System.Net;
using System.Web.Http.ModelBinding;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class CustomerOpenFilesAmountController : ApiController
    {
        CustomerOpenFilesAmount oldEntity;
        public HttpResponseMessage Post(CustomerOpenFilesAmount customerOpenFilesAmount)
        {
            CustomerOpenFilesAmount oldEntity =customerOpenFilesAmount;
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = GetAuthenticationToken();
                        int tenant = authToken.Tenant;
                        SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                        if (customerOpenFilesAmount != null)
                        {
                            oldEntity = LogitudeXmlSerializer.DeserializeObject<CustomerOpenFilesAmount>(LogitudeXmlSerializer.SerializeObjectToXmlString(customerOpenFilesAmount));
                        }
                        CustomerOpenFilesAmountQuery customerOpenFilesAmountQuery = new CustomerOpenFilesAmountQuery(tenant);
                        CustomerOpenFilesAmountQueryService mappingService = new CustomerOpenFilesAmountQueryService(customerOpenFilesAmount.Tenant);
                        CustomerOpenFilesAmountPM customerOpenFilesAmountPM = mappingService.CustomerOpenFilesAmountDataMappingAndValidatin(customerOpenFilesAmount, tenant);

                        APIHelper.AddCommunicationLog("D", oldEntity, customerOpenFilesAmount, "CustomerOpenFilesAmount", customerOpenFilesAmount.CustomerId, "CustomerOpenFilesAmount API", authToken.Tenant);

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, customerOpenFilesAmount);
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

        private Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken GetAuthenticationToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            return authToken;
        }

        private HttpResponseMessage CreateResponse(Exception exception, ModelStateDictionary modelState)
        {
            APIExceptionResult apiExceptionResult = null;
            if (exception != null && modelState == null)
            {
                apiExceptionResult = ApiExceptionHandler.HandleException(exception);
            }

            else
            {
                apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
            }
            APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "CustomerOpenFilesAmount", null, "ARInvoice API");
            return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
        }

    }
}