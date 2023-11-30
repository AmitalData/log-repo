using Logitude.BL.InfrastructureModel.EntityAMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebFreight.Web.Controllers.InfrastructureModel.Services
{
    public class ImporterEventTypesExtendedService
    {
        private int tenant;
        public string correlationId;
        private IWebFreightContext webFreightContext;
        private EventTypeService eventTypeService;
        private EventTypePM importerEventType = null;
        public ImporterEventTypesExtendedService(int tenant, string correlationId)
        {
            this.tenant = tenant;
            this.correlationId = correlationId;
            webFreightContext = WebFreightContext.GetContext(tenant);
            eventTypeService = new EventTypeService(webFreightContext, tenant);
        }

        public APIException GetEventTypeAPIResult(EventTypeAM eventTypeAM, bool isNew)
        {
            EventTypeQuery eventTypeQuery = new EventTypeQuery(eventTypeAM.Tenant);
            if (isNew)
            {
                importerEventType = new EventTypePM();
            }
            else if (!string.IsNullOrEmpty(eventTypeAM.Code) && !string.IsNullOrEmpty(eventTypeAM.ObjectTableName))
            {
                importerEventType = eventTypeQuery.GetSingleEventTypePMByCodeObjectTableName(eventTypeAM.Code, eventTypeAM.ObjectTableName, eventTypeAM.Tenant);
            }

            APIException result = MapAndValidateFieldsIds(eventTypeAM);
            if (result == null)
            {
                MapEntityAMToEntityPM(eventTypeAM);
            }

            return result;
        }

        public APIException MapAndValidateFieldsIds(EventTypeAM eventTypeAM)
        {
            APIException Responce = new APIException();
            if (eventTypeAM.ObjectTableName == null)
            {
                return ThrowEventTypeAMValidationError(Responce, "ObjectTable");
            }
            
            string objectTableId = GetObjectTableIdFromName(eventTypeAM.ObjectTableName);
            if (!string.IsNullOrEmpty(objectTableId))
            {
                importerEventType.ObjectTableId = objectTableId;
            }
            else
            {
                return ThrowDataBaseValidationError(Responce, "ObjectTable");
            }
            
            string entityStatusId = GetEntityStatusIdFromCode(eventTypeAM.EntityStatusCode, importerEventType.ObjectTableId, eventTypeAM.Tenant);
            if (eventTypeAM.EntityStatusCode != null && !string.IsNullOrEmpty(entityStatusId))
            {
                importerEventType.EntityStatusId = entityStatusId;
            }
            else if(eventTypeAM.EntityStatusCode != null)
            {
                return ThrowDataBaseValidationError(Responce, "EntityStatusId");
            }
            
            if (string.IsNullOrEmpty(eventTypeAM.IsAgentView.ToString()))
            {
                return ThrowEventTypeAMValidationError(Responce, "IsAgentView");
            }

            if (string.IsNullOrEmpty(eventTypeAM.IsCustomerView.ToString()))
            {
                return ThrowEventTypeAMValidationError(Responce, "IsCustomerView");
            }

            if (string.IsNullOrEmpty(eventTypeAM.IsFollowUp.ToString()))
            {
                return ThrowEventTypeAMValidationError(Responce, "IsFollowUp");
            }

            return null;
        }

        public string GetObjectTableIdFromName(string objectTableName)
        {
            string objectTableId = new ObjectTableRepository(webFreightContext).GetObjectTableIdByName(objectTableName);
            return objectTableId;
        }

        public string GetEntityStatusIdFromCode(string entityStatusCode, string objectTableId, int tenant)
        {
            if(string.IsNullOrEmpty(entityStatusCode)) return "";
            EntityStatus entityStatus = new EntityStatusRepository(webFreightContext).GetSingleEntityStatusByCodeTableId(entityStatusCode, objectTableId, tenant);
            if (entityStatus != null)
            {
                return entityStatus.Id;
            }
            return "";
        }

        private static APIException ThrowDataBaseValidationError(APIException Responce, string fieldName)
        {
            Responce.ErrorType = "Validation Error";
            Responce.ErrorMessage = fieldName + " field doesn't exist in the database, insert this entity before using it.";
            return Responce;
        }

        private static APIException ThrowEventTypeAMValidationError(APIException Responce, string fieldName)
        {
            Responce.ErrorType = "Validation Error";
            Responce.ErrorMessage = fieldName + " field is required.";
            return Responce;
        }

        public void MapEntityAMToEntityPM(EventTypeAM eventTypeAM)
        {
            importerEventType.Tenant = eventTypeAM.Tenant;
            importerEventType.Code = eventTypeAM.Code;
            importerEventType.EnglishName = eventTypeAM.EnglishName;
            importerEventType.LocalName = eventTypeAM.LocalName;
            importerEventType.ObjectTableName = eventTypeAM.ObjectTableName;
            importerEventType.EntityStatusCode = eventTypeAM.EntityStatusCode;
            importerEventType.IsFollowUp = eventTypeAM.IsFollowUp;
            importerEventType.IsCustomerView = eventTypeAM.IsCustomerView;
            importerEventType.IsAgentView = eventTypeAM.IsAgentView;
            importerEventType.FollowUpEnglishName = eventTypeAM.FollowUpEnglishName;
            importerEventType.FollowUpLocalName = eventTypeAM.FollowUpLocalName;
            importerEventType.EventTrigger = eventTypeAM.EventTrigger;
            importerEventType.IsFromWorkerRole = true;
        }

        public void UpdateImporterEventType()
        {
            eventTypeService.Update(importerEventType);
        }
        
        public void CreateImporterEventType()
        {
            eventTypeService.Create(importerEventType);
        }

        public APILogsPM GetLogPM(string subject, string reference)
        {
            APILogsRepository aPILogsRepository = new APILogsRepository(webFreightContext);
            APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(correlationId, tenant);
            if (Log != null)
            {
                return MapLogPMFromPoco(Log);
            }
            APILogsPM LogPM = GetNewLog(subject, reference);
            new APILogsService(webFreightContext, tenant).Create(LogPM);
            return LogPM;
        }
        
        private static APILogsPM MapLogPMFromPoco(APILogs Log)
        {
            return new APILogsPM()
            {
                Id = Log.Id,
                CorrelationId = Log.CorrelationId,
                CreateDate = Log.CreateDate,
                CreateDateUTC = Log.CreateDateUTC,
                Direction = Log.Direction,
                EntityId = Log.EntityId,
                LastUpdateDate = Log.LastUpdateDate,
                LastUpdateDateUTC = Log.LastUpdateDateUTC,
                NumberOfRetries = Log.NumberOfRetries++,
                ObjectTableId = Log.ObjectTableId,
                ExpirationDate = Log.ExpirationDate,
                Refrence = Log.Refrence,
                Status = "I",
                Tenant = Log.Tenant,
                QueueMessageMoreDetailsId = Log.QueueMessageMoreDetailsId,
                QueueType = "EventType",
                Subject = Log.Subject
            };
        }
        
        private APILogsPM GetNewLog(string subject, string reference)
        {
            return new APILogsPM()
            {
                Id = IdCounter.GetNumber("APILogs", tenant),
                CorrelationId = correlationId,
                CreateDate = DateTime.Now,
                CreateDateUTC = DateTime.UtcNow,
                Direction = "O",
                LastUpdateDate = DateTime.Now,
                LastUpdateDateUTC = DateTime.UtcNow,
                NumberOfRetries = 1,
                ExpirationDate = DateTime.Now.AddDays(90),
                Status = "I",
                QueueMessageMoreDetailsId = correlationId,
                QueueType = "EventType",
                Subject = subject,
                Refrence = reference,
            };
        }
    }
}