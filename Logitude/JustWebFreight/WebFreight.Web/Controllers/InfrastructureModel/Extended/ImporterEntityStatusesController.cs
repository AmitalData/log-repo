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
    public class ImporterEntityStatusesController : ApiController
    {
        public bool GetIfEntityStatusExists(int tenant, string entityStatusCode, string objectTableName)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.AuthenticationOnEntityTenant("EntityStatus", tenant, 0);
            EntityStatusQuery entityStatusQuery = new EntityStatusQuery(tenant);
            EntityStatusPM entityStatusPM = entityStatusQuery.GetSingleEntityStatusPMByCodeObjectTableName(entityStatusCode, objectTableName, tenant);
            return entityStatusPM != null;
        }

        public HttpResponseMessage Post(EntityStatusAM entityStatusAM)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityStatusAM.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("EntityStatus", entityStatusAM.Tenant, 0);
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                ImporterEntityStatusesExtendedService importerEntityStatusesExtendedService = new ImporterEntityStatusesExtendedService(entityStatusAM.Tenant, CorrelationId);
                APILogsPM LogPM = importerEntityStatusesExtendedService.GetLogPM("Inserting EntityStatus To Importer Tenant", entityStatusAM.Code);
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Inserting EntityStatus To Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(entityStatusAM), null, null, "");
                try
                {
                    return HandleInsertEntityStatus(entityStatusAM, importerEntityStatusesExtendedService, LogPM);
                }
                catch (Exception exception)
                {
                    return HandleUpsertEntityStatusException(LogPM, exception, "Update EntityStatus At Importer Tenant Faild ");
                }
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(exception));
            }
        }

        public HttpResponseMessage Put(EntityStatusAM entityStatusAM)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityStatusAM.Tenant);
                SecurityUtility.AuthenticationOnEntityTenant("EntityStatus", entityStatusAM.Tenant, 0);
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                ImporterEntityStatusesExtendedService importerEntityStatusesExtendedService = new ImporterEntityStatusesExtendedService(entityStatusAM.Tenant, CorrelationId);
                APILogsPM LogPM = importerEntityStatusesExtendedService.GetLogPM("Update EntityStatus To Importer Tenant", entityStatusAM.Code);
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start updating EntityStatus To Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(entityStatusAM), null, null, "");
                try
                {
                    return HandleUpdateEntityStatus(entityStatusAM, importerEntityStatusesExtendedService, LogPM);
                }
                catch (Exception exception)
                {
                    return HandleUpsertEntityStatusException(LogPM, exception, "Insert EntityStatus At Importer Tenant Faild ");
                }
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(exception));
            }
        }

        private HttpResponseMessage HandleInsertEntityStatus(EntityStatusAM entityStatusAM, ImporterEntityStatusesExtendedService importerEntityStatusesExtendedService, APILogsPM LogPM)
        {
            APIException result = null;
            result = importerEntityStatusesExtendedService.GetEntityStatusAPIResult(entityStatusAM, true);
            if (result != null)
                return MarkProcessAsFailed(LogPM, result, "Updating EntityStatus Faild" + DateTime.Now);
            importerEntityStatusesExtendedService.CreateImporterEntityStatus();
            return MarkProcessAsDone(entityStatusAM, LogPM, "EntityStatus Is Added Successfully " + DateTime.Now);
        }

        private HttpResponseMessage HandleUpdateEntityStatus(EntityStatusAM entityStatusAM, ImporterEntityStatusesExtendedService importerEntityStatusesExtendedService, APILogsPM LogPM)
        {
            APIException result = null;
            result = importerEntityStatusesExtendedService.GetEntityStatusAPIResult(entityStatusAM, false);
            if (result != null)
                return MarkProcessAsFailed(LogPM, result, "Updating EntityStatus Faild " + DateTime.Now);
            importerEntityStatusesExtendedService.UpdateImporterEntityStatus();
            return MarkProcessAsDone(entityStatusAM, LogPM, "EntityStatus Updated Successfully " + DateTime.Now);
        }

        private HttpResponseMessage HandleUpsertEntityStatusException(APILogsPM LogPM, Exception exception, string logMessage)
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
        
        private HttpResponseMessage MarkProcessAsDone(EntityStatusAM entityStatusAM, APILogsPM LogPM, string doneMsg)
        {
            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, doneMsg, null, null, null, "");
            return Request.CreateResponse(HttpStatusCode.OK, entityStatusAM);
        }
    }
}