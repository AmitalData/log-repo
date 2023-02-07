using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Common
{
    public class CustomerTenantAccessExtendedController : ApiController
    {
        public HttpResponseMessage GetDenyRequest(string CustomerTenantAccessId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                  

                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.CheckContactFeature("CustomerTenantAccess", "READ", authToken.Tenant);
                        UpdateCustomerTenantAccessStatus updateCustomerTenantAccessStatus = new UpdateCustomerTenantAccessStatus();
                        updateCustomerTenantAccessStatus.DenyRequest(CustomerTenantAccessId, authToken.Tenant);
                        PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                        return Request.CreateResponse(HttpStatusCode.OK, "");
                    
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        public HttpResponseMessage GetByCompanyVatNumber(string companyVatNumber)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string logKey = PerformanceLogger.LogCurrentTime();


                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("CustomerTenantAccess", "READ", authToken.Tenant);
                    CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(authToken.Tenant);
                    CustomerTenantAccessPM customerTenantAccessPM = customerTenantAccessQuery.GetCustomerTenantAccessPMsByTenant(authToken.Tenant).Where(customer => customer.CompanyVat == companyVatNumber).FirstOrDefault();
                    PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                    return Request.CreateResponse(HttpStatusCode.OK, customerTenantAccessPM);
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        public HttpResponseMessage Put(CustomerTenantAccessUpdaterAM customerTenantAccessUpdaterAM)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(customerTenantAccessUpdaterAM.Tenant);
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(customerTenantAccessUpdaterAM.Tenant);
                APILogsService apiLogsService = new APILogsService(webFreightContext, customerTenantAccessUpdaterAM.Tenant);
                APILogsPM LogPM  = apiLogsService.GetAPILogsPMByCorrelationId("Update All Forwarder Requests", "CustomerTenantAccess");
                try
                {
                    var msg = "Start Updating All Forwarder CustomerTenantAccess" + DateTime.Now;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, msg, "", null, null, "");
                    CustomerTenantAccessService.UpdateAllCustomerTenantAccesses(customerTenantAccessUpdaterAM);
                    var Donemsg = "CustomerTenantAccess Forwarder Requests Updated Successfully " + DateTime.Now;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, null, null, "");
                    IWebFreightContext webfreightcontext = WebFreightContext.GetContext(customerTenantAccessUpdaterAM.Tenant);
                    TableLastUpdateClass.UpdateTableHistory(customerTenantAccessUpdaterAM.Tenant, "CustomerTenantAccess", webfreightcontext);
                    return Request.CreateResponse(HttpStatusCode.OK, "OK");
                }
                catch (Exception exception)
                {
                    APIException Responce = HandleException(LogPM, exception);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, Responce);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static APIException HandleException(APILogsPM LogPM, Exception exception)
        {
            string errorMessage = exception.Message + Environment.NewLine;
            errorMessage = exception.InnerException != null ? errorMessage + " (" + (exception.InnerException.InnerException != null ? exception.InnerException.InnerException.Message : exception.InnerException.Message) + ")" + Environment.NewLine : errorMessage;
            errorMessage = errorMessage + exception.StackTrace + Environment.NewLine;
            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, exception.Message, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
            APIException Responce = new APIException();
            Responce.ErrorType = exception.GetType().Name;
            Responce.ErrorMessage = errorMessage;
            return Responce;
        }
    }
}