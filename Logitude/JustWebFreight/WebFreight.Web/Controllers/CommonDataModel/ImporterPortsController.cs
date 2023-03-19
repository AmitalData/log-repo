using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.EntityAMs;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using WebFreight.Web.Helpers;
using WebFreight.Web.Controllers.CommonDataModel.Services;

namespace WebFreight.Web.Controllers.CommonDataModel
{
    public class ImporterPortsController : ApiController
    {

        public bool GetIfPortExists(int tenant , string portCode, string countryCode)// NotFinished
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            PortQuery portQuery = new PortQuery(tenant);
            PortPM port = portQuery.GetSinglePortPMByCodeCountryCode(portCode, countryCode, tenant);
            if (port != null && !port.InActive)
            {
                return true;
            }
            return false;
        }
        public HttpResponseMessage Put(PortAM Port)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(Port.Tenant);
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                ImporterPortsExtendedService importerPortsExtendedService = new ImporterPortsExtendedService(Port.Tenant, CorrelationId);
                PortQuery portQuery = new PortQuery(Port.Tenant);

                APILogsPM LogPM = importerPortsExtendedService.GetLogPM();

                LogPM.Subject = "Update Port To Importer Tenant";
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start updating Port To Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(Port), null, null, "");
                try
                {
                    var msg = "Start updating Port At Importer Tenant " + DateTime.Now;
                    PortPM ImporterPort = null;
                    var IsNew = false;
                    if (!string.IsNullOrEmpty(Port.Code) && !string.IsNullOrEmpty(Port.CountryCode))
                    {
                        ImporterPort = portQuery.GetSinglePortPMByCodeCountryCode(Port.Code, Port.CountryCode, Port.Tenant);
                    }
                    if (ImporterPort == null)
                    {
                        ImporterPort = new PortPM();
                        IsNew = true;
                    }
                    APIException Result = importerPortsExtendedService.MapEntityAMToEntityPM(Port, ImporterPort);

                    if (Result == null)
                    {
                        var Donemsg = "Port Updated Successfully " + DateTime.Now;
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, ImporterPort.Id, null, "");
                        return Request.CreateResponse(HttpStatusCode.OK, new List<string>() { ImporterPort.Id, ImporterPort.Code });
                    }
                    var Failmsg = "Updating Port Faild " + DateTime.Now;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(Result), null, "");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, Result);
                }
                catch (Exception ex)
                {
                    var apiException = new APIException()
                    {
                        ErrorType = ex.GetType().Name,
                        ErrorMessage = ex.Message
                    };
                    string errorMessage = ex.Message + Environment.NewLine;
                    if (ex.InnerException != null)
                    {
                        errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;
                    }
                    errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Update Port At Importer Tenant Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage Post(PortAM Port)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(Port.Tenant);
                PortQuery portQuery = new PortQuery(Port.Tenant);
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                ImporterPortsExtendedService importerPortsExtendedService = new ImporterPortsExtendedService(Port.Tenant, CorrelationId);

                APILogsPM LogPM = importerPortsExtendedService.GetLogPM();
                LogPM.Subject = "Inserting Port To Importer Tenant";
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Inserting Shipment To Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(Shipment), null, null, "");
                var IsNew = false;
                try
                {

                }
                catch (Exception ex)
                {
                    var apiException = new APIException()
                    {
                        ErrorType = ex.GetType().Name,
                        ErrorMessage = ex.Message
                    };
                    string errorMessage = ex.Message + Environment.NewLine;
                    if (ex.InnerException != null)
                    {
                        errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;
                    }
                    errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Insert Port At Importer Tenant Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}