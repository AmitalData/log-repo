using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
    public class CustomerController : ApiController
    {
        public HttpResponseMessage GetSingleCustomer(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                CustomerQueryService Service = new CustomerQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetCustomerById(id, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Post(Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Customer entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    ContactInfo loggedContactInfo = SecurityUtility.GetContactInfo(authToken.Email, authToken.Tenant);
                    string computingPartnerCode = "";
                    if (loggedContactInfo != null)
                    {
                        computingPartnerCode = loggedContactInfo.ComputingPartnerCode;
                    }

                    ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
                    CustomerQueryService mappingService = new CustomerQueryService(authToken.Tenant);
                    CustomerPM entityPM = mappingService.CustomerCustomDataMappingAndValidating(entity, authToken.Tenant, computingPartnerCode);

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {    
                        if(entityPM.Addresses.Count == 0)
                        {
                            throw new ApplicationException("Missing Main Address");
                        }
                        else
                        {
                            Tenant myTenant = MyContext.Tenants.Where(d => d.Id == authToken.Tenant).FirstOrDefault();
                            if (myTenant.IsCustomerTelRequired)
                            {
                                if (string.IsNullOrEmpty(entity.MainAddress.PhoneNumber))
                                {
                                    throw new ApplicationException("Phone Number is required");
                                }
                            }

                            if (myTenant.IsCustomerFaxRequired)
                            {
                                if (string.IsNullOrEmpty(entity.MainAddress.FaxNumber))
                                {
                                    throw new ApplicationException("Fax Number is required");
                                }
                            }
                        }
                        
                        CustomerService service = new CustomerService(MyContext, entityPM);
                        service.Create();
                        service.Submit();

                        //if (entity.GLAccount != null)
                        //{
                        //    CardRepository cardRepository = new CardRepository(authToken.Tenant);
                        //    Simplog.Data.CommonDataModel.EntityPOCOs.Card card = cardRepository.GetSingleCard(entityPM.Id, authToken.Tenant);
                        //    if(card != null)
                        //    {
                        //        if(entity.GLAccount.IsMultiCurrency == false && string.IsNullOrEmpty(entity.GLAccount.CurrencyId))
                        //        {
                        //            throw new ApplicationException("GLAccount currency is required");
                        //        }

                        //        GLAccountPM gLAccountEntity = new GLAccountPM()
                        //        {
                        //            CurrencyId = entity.GLAccount.Currency,
                        //        };

                        //        FullAccountingHelper fullAccountingHelper = new FullAccountingHelper();
                        //        string glAccountId = fullAccountingHelper.CreateGLAccount(card);
                        //    }                            
                        //}

                        scope.Complete();
                    }

                    var result = mappingService.GetCustomerById(entityPM.Id, authToken.Tenant);
                    APIHelper.AddCommunicationLog("D", entity, result, "Customer", entityPM.Id, "Customer API", authToken.Tenant);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Customer", null, "Customer API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Customer", null, "Customer API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Customer entity)
        {
            var apiExceptionResult = ApiExceptionHandler.HandleException(new Exception("Updates are not supported"));
            APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Customer", null, "Customer API");
            return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
        }
    }
}