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
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class CustomerOpenFilesAmountController : ApiController
    {
        CustomerOpenFilesAmount oldEntity;
        CustomerOpenFilesAmountQueryService mappingService;
        public HttpResponseMessage Post(CustomerOpenFilesAmount customerOpenFilesAmount)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = GetAuthenticationToken();
                        int tenant = authToken.Tenant;
                        SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                        SetOldEntity(customerOpenFilesAmount);
                        CustomerOpenFilesAmountPM customerOpenFilesAmountPM = GetCustomerOpenFilesAmountPM(customerOpenFilesAmount);                        
                        customerOpenFilesAmountPM = mappingService.SetCustomerId(customerOpenFilesAmountPM);
                        CreateOrUpdateCustomerOpenFilesAmountForCustomer(customerOpenFilesAmountPM);
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
        
        private CustomerOpenFilesAmountPM GetCustomerOpenFilesAmountPM(CustomerOpenFilesAmount customerOpenFilesAmount)
        {
            mappingService = new CustomerOpenFilesAmountQueryService(customerOpenFilesAmount.Tenant);
             return  mappingService.CustomerOpenFilesAmountDataMappingAndValidatin(customerOpenFilesAmount, customerOpenFilesAmount.Tenant); }

        private void SetOldEntity(CustomerOpenFilesAmount customerOpenFilesAmount)
        {
            if (customerOpenFilesAmount != null)
            {
                oldEntity = LogitudeXmlSerializer.DeserializeObject<CustomerOpenFilesAmount>(LogitudeXmlSerializer.SerializeObjectToXmlString(customerOpenFilesAmount));
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

        private void CreateOrUpdateCustomerOpenFilesAmountForCustomer(CustomerOpenFilesAmountPM customerOpenFilesAmount) {
            ICommonDataContext commonContext = CommonDataContext.GetContext(customerOpenFilesAmount.Tenant);
            CustomerOpenFilesAmountService CustomerOpenFilesAmountService = new CustomerOpenFilesAmountService(commonContext,customerOpenFilesAmount.Tenant);
            bool exist= CheckIfCustomerOpenFilesAmountExist(customerOpenFilesAmount);
            if (exist)  CustomerOpenFilesAmountService.Update(customerOpenFilesAmount);
            else CustomerOpenFilesAmountService.Create(customerOpenFilesAmount);
        }

        private bool CheckIfCustomerOpenFilesAmountExist(CustomerOpenFilesAmountPM customerOpenFilesAmount)
        {
            CustomerOpenFilesAmountQuery customerOpenFilesAmountQuery = new CustomerOpenFilesAmountQuery(customerOpenFilesAmount.Tenant);
            customerOpenFilesAmount = customerOpenFilesAmountQuery.GetSinglePMByCustomerId(customerOpenFilesAmount.CustomerId, customerOpenFilesAmount.Tenant);
            if (customerOpenFilesAmount != null) return true; 
            else return false;
        }
    }
}