using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools;
using WebFreight.Web.Helpers;
using WebFreight.Web.Controllers.CommonDataModel.Services;

namespace WebFreight.Web.Controllers.CommonDataModel
{
    public class ImporterPortsController : ApiController
    {
        public bool GetIfPortExists(int tenant , string portCode, string countryCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.AuthenticationOnEntityTenant("Port", tenant, 0);
            PortQuery portQuery = new PortQuery(tenant);
            PortPM port = portQuery.GetSinglePortPMByCodeCountryCode(portCode, countryCode, tenant);
            return port != null;
        }
        public HttpResponseMessage Put(PortAM Port)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(Port.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("Port", Port.Tenant, 0);
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                ImporterPortsExtendedService importerPortsExtendedService = new ImporterPortsExtendedService(Port.Tenant, CorrelationId);
                APILogsPM LogPM = importerPortsExtendedService.GetLogPM("Update Port To Importer Tenant", Port.Code);
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start updating Port To Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(Port), null, null, "");
                try
                {
                    APIException result = null;
                    result = importerPortsExtendedService.GetPortAPIResult(Port, false);
                    if(result != null)
                        return MarkProcessAsFailed(LogPM, result, "Updating Port Faild " + DateTime.Now);
                    importerPortsExtendedService.UpdateImporterPort();
                    return MarkProcessAsDone(Port, LogPM, "Port Updated Successfully " + DateTime.Now);                    
                }
                catch (Exception exception)
                {
                    var apiException = new APIException()
                    {
                        ErrorType = exception.GetType().Name,
                        ErrorMessage = exception.Message
                    };
                    string errorMessage = exception.Message + Environment.NewLine;
                    if (exception.InnerException != null)
                    {
                        errorMessage = errorMessage + " (" + (exception.InnerException.InnerException != null ? exception.InnerException.InnerException.Message : exception.InnerException.Message) + ")" + Environment.NewLine;
                    }
                    errorMessage = errorMessage + exception.StackTrace + Environment.NewLine;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Insert Port At Importer Tenant Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                }
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(exception));
            }
        }
        public HttpResponseMessage Post(PortAM Port)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(Port.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("Port", Port.Tenant, 0);
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                ImporterPortsExtendedService importerPortsExtendedService = new ImporterPortsExtendedService(Port.Tenant, CorrelationId);
                APILogsPM LogPM = importerPortsExtendedService.GetLogPM("Inserting Port To Importer Tenant", Port.Code);
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Inserting port To Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(Port), null, null, "");
                try
                {
                    APIException result = null;
                    result = importerPortsExtendedService.GetPortAPIResult(Port, true);
                    if(result != null)
                        return MarkProcessAsFailed(LogPM, result, "Updating Port Faild" + DateTime.Now);
                    importerPortsExtendedService.CreateImporterPort();
                    return MarkProcessAsDone(Port, LogPM, "Port Is Added Successfully " + DateTime.Now);                    
                }
                catch (Exception exception)
                {
                    var apiException = new APIException()
                    {
                        ErrorType = exception.GetType().Name,
                        ErrorMessage = exception.Message
                    };
                    string errorMessage = exception.Message + Environment.NewLine;
                    if (exception.InnerException != null)
                    {
                        errorMessage = errorMessage + " (" + (exception.InnerException.InnerException != null ? exception.InnerException.InnerException.Message : exception.InnerException.Message) + ")" + Environment.NewLine;
                    }
                    errorMessage = errorMessage + exception.StackTrace + Environment.NewLine;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Insert Port At Importer Tenant Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                }
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(exception));
            }
        }
        private HttpResponseMessage MarkProcessAsFailed(APILogsPM LogPM, APIException Result, string failMsg)
        {
            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, failMsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(Result), null, "");
            return Request.CreateResponse(HttpStatusCode.BadRequest, Result);
        }
        private HttpResponseMessage MarkProcessAsDone(PortAM Port, APILogsPM LogPM, string doneMsg)
        {
            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, doneMsg, null, null, null, "");
            return Request.CreateResponse(HttpStatusCode.OK, Port);
        }
    }
}