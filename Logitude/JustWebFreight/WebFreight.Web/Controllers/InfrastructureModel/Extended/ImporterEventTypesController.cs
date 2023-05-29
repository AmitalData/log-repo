using Logitude.BL.InfrastructureModel.EntityAMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Controllers.InfrastructureModel.Services;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class ImporterEventTypesController : ApiController
    {
        public bool GetIfEventTypeExists(int tenant, string eventTypeCode, string objectTableName)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.AuthenticationOnEntityTenant("EventType", tenant, 0);
            EventTypeQuery eventTypeQuery = new EventTypeQuery(tenant);
            EventTypePM eventTypePM = eventTypeQuery.GetSingleEventTypePMByCodeObjectTableName(eventTypeCode, objectTableName, tenant);
            return eventTypePM != null;
        }

        public HttpResponseMessage Post(EventTypeAM eventTypeAM)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(eventTypeAM.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("EventType", eventTypeAM.Tenant, 0);
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                ImporterEventTypesExtendedService importerEventTypesExtendedService = new ImporterEventTypesExtendedService(eventTypeAM.Tenant, CorrelationId);
                APILogsPM LogPM = importerEventTypesExtendedService.GetLogPM("Inserting EventType To Importer Tenant", eventTypeAM.Code);
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Inserting EventType To Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(eventTypeAM), null, null, "");
                try
                {
                    return HandleInsertEventType(eventTypeAM, importerEventTypesExtendedService, LogPM);
                }
                catch (Exception exception)
                {
                    return HandleUpsertEventTypeException(LogPM, exception, "Update EventType At Importer Tenant Faild ");
                }
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(exception));
            }
        }

        public HttpResponseMessage Put(EventTypeAM eventTypeAM)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(eventTypeAM.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("EventType", eventTypeAM.Tenant, 0);
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                ImporterEventTypesExtendedService importerEventTypesExtendedService = new ImporterEventTypesExtendedService(eventTypeAM.Tenant, CorrelationId);
                APILogsPM LogPM = importerEventTypesExtendedService.GetLogPM("Update EventType To Importer Tenant", eventTypeAM.Code);
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start updating EventType To Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(eventTypeAM), null, null, "");
                try
                {
                    return HandleUpdateEventType(eventTypeAM, importerEventTypesExtendedService, LogPM);
                }
                catch (Exception exception)
                {
                    return HandleUpsertEventTypeException(LogPM, exception, "Insert EventType At Importer Tenant Faild ");
                }
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(exception));
            }
        }

        private HttpResponseMessage HandleInsertEventType(EventTypeAM eventTypeAM, ImporterEventTypesExtendedService importerEventTypesExtendedService, APILogsPM LogPM)
        {
            APIException result = null;
            result = importerEventTypesExtendedService.GetEventTypeAPIResult(eventTypeAM, true);
            if (result != null)
                return MarkProcessAsFailed(LogPM, result, "Updating EventType Faild" + DateTime.Now);
            importerEventTypesExtendedService.CreateImporterEventType();
            return MarkProcessAsDone(eventTypeAM, LogPM, "EventType Is Added Successfully " + DateTime.Now);
        }

        private HttpResponseMessage HandleUpdateEventType(EventTypeAM eventTypeAM, ImporterEventTypesExtendedService importerEventTypesExtendedService, APILogsPM LogPM)
        {
            APIException result = null;
            result = importerEventTypesExtendedService.GetEventTypeAPIResult(eventTypeAM, false);
            if (result != null)
                return MarkProcessAsFailed(LogPM, result, "Updating EventType Faild " + DateTime.Now);
            importerEventTypesExtendedService.UpdateImporterEventType();
            return MarkProcessAsDone(eventTypeAM, LogPM, "EventType Updated Successfully " + DateTime.Now);
        }

        private HttpResponseMessage HandleUpsertEventTypeException(APILogsPM LogPM, Exception exception, string logMessage)
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
            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, logMessage + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));
            return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
        }

        private HttpResponseMessage MarkProcessAsFailed(APILogsPM LogPM, APIException Result, string failMsg)
        {
            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, failMsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(Result), null, "");
            return Request.CreateResponse(HttpStatusCode.BadRequest, Result);
        }
        
        private HttpResponseMessage MarkProcessAsDone(EventTypeAM eventTypeAM, APILogsPM LogPM, string doneMsg)
        {
            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, doneMsg, null, null, null, "");
            return Request.CreateResponse(HttpStatusCode.OK, eventTypeAM);
        }
    }
}