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
    public class ImporterEntityStatusesExtendedService
    {
        private int tenant;
        public string correlationId;
        private IWebFreightContext webFreightContext;
        private EntityStatusService entityStatusService;
        private EntityStatusPM importerEntityStatus = null;
        public ImporterEntityStatusesExtendedService(int tenant, string correlationId)
        {
            this.tenant = tenant;
            this.correlationId = correlationId;
            webFreightContext = WebFreightContext.GetContext(tenant);
            entityStatusService = new EntityStatusService(webFreightContext, tenant);
        }

        public APIException GetEntityStatusAPIResult(EntityStatusAM entityStatusAM, bool isNew)
        {
            EntityStatusQuery entityStatusQuery = new EntityStatusQuery(entityStatusAM.Tenant);
            if (isNew)
            {
                importerEntityStatus = new EntityStatusPM();
            }
            else if (!string.IsNullOrEmpty(entityStatusAM.Code) && !string.IsNullOrEmpty(entityStatusAM.ObjectTableName))
            {
                importerEntityStatus = entityStatusQuery.GetSingleEntityStatusPMByCodeObjectTableName(entityStatusAM.Code, entityStatusAM.ObjectTableName, entityStatusAM.Tenant);
            }

            APIException result = MapAndValidateFieldsIds(entityStatusAM);
            if (result == null)
            {
                MapEntityAMToEntityPM(entityStatusAM);
            }

            return result;
        }

        public APIException MapAndValidateFieldsIds(EntityStatusAM entityStatusAM)
        {
            APIException Responce = new APIException();
            if (entityStatusAM.ObjectTableName == null)
            {
                return ThrowEntityStatusAMValidationError(Responce, "ObjectTable");
            }
            
            string objectTableId = GetObjectTableIdFromName(entityStatusAM.ObjectTableName);
            if (!string.IsNullOrEmpty(objectTableId))
            {
                importerEntityStatus.ObjectTableId = objectTableId;
            }
            else
            {
                return ThrowDataBaseValidationError(Responce, "ObjectTable");
            }
            
            if(!IsValidEntityStatusTypeCode(entityStatusAM.EntityStatusTypeCode))
            {
                return ThrowDataBaseValidationError(Responce, "EntityStatusId");
            }
            
            if (string.IsNullOrEmpty(entityStatusAM.AllowPartial.ToString()))
            {
                return ThrowEntityStatusAMValidationError(Responce, "AllowPartial");
            }

            if (string.IsNullOrEmpty(entityStatusAM.IsDigitalPortal.ToString()))
            {
                return ThrowEntityStatusAMValidationError(Responce, "IsDigitalPortal");
            }

            return null;
        }

        public string GetObjectTableIdFromName(string objectTableName)
        {
            string objectTableId = new ObjectTableRepository(webFreightContext).GetObjectTableIdByName(objectTableName);
            return objectTableId;
        }

        public bool IsValidEntityStatusTypeCode(string entityStatusTypeCode)
        {
            if(string.IsNullOrEmpty(entityStatusTypeCode)) return true;
            EntityStatusType entityStatusType = new EntityStatusTypeRepository(webFreightContext).GetSingleEntityStatusType(entityStatusTypeCode);
            return entityStatusType != null;
        }

        private static APIException ThrowDataBaseValidationError(APIException Responce, string fieldName)
        {
            Responce.ErrorType = "Validation Error";
            Responce.ErrorMessage = fieldName + " field doesn't exist in the database, insert this entity before using it.";
            return Responce;
        }

        private static APIException ThrowEntityStatusAMValidationError(APIException Responce, string fieldName)
        {
            Responce.ErrorType = "Validation Error";
            Responce.ErrorMessage = fieldName + " field is required.";
            return Responce;
        }

        public void MapEntityAMToEntityPM(EntityStatusAM entityStatusAM)
        {
            importerEntityStatus.Tenant = entityStatusAM.Tenant;
            importerEntityStatus.Code = entityStatusAM.Code;
            importerEntityStatus.Name = entityStatusAM.Name;
            importerEntityStatus.DisplayName = entityStatusAM.DisplayName;
            importerEntityStatus.ObjectTableName = entityStatusAM.ObjectTableName;
            importerEntityStatus.StatusWeight = entityStatusAM.StatusWeight;
            importerEntityStatus.StatusLocalWeight = entityStatusAM.StatusLocalWeight;
            importerEntityStatus.EntityStatusTypeCode = entityStatusAM.EntityStatusTypeCode;
            importerEntityStatus.AllowPartial = entityStatusAM.AllowPartial;
            importerEntityStatus.IsDigitalPortal = entityStatusAM.IsDigitalPortal;
            importerEntityStatus.IsFromWorkerRole = true;
        }

        public void UpdateImporterEntityStatus()
        {
            entityStatusService.Update(importerEntityStatus);
        }
        
        public void CreateImporterEntityStatus()
        {
            entityStatusService.Create(importerEntityStatus);
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
                QueueType = "EntityStatus",
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
                QueueType = "EntityStatus",
                Subject = subject,
                Refrence = reference,
            };
        }
    }
}