using CommunicationWorkerRole.MultiEntityUpdate;
using Logitude.BL.InfrastructureModel.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.EntityChanges.AutomationResult;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
namespace WebFreight.Web.Helpers.WorkerRole.MultiEntityUpdate
{
    public class MultiEntityUpdateExecutionService
    {

        private readonly DbQueueService queueService = null;
        private readonly QueueResponse queueResponse = null;
        private readonly int? tenant;
        private readonly string multiEntityUpdateId = string.Empty;
        private ObjectFieldRepository objectFieldRepository;
        private AutomationSetValueResultService automationSetValueResultService;
        private List<MultiEntityUpdateDataEntity> modifiedMultiEntityUpdateDataEntities;
        private List<ObjectField> objectFields;

        private int numberOfExecutedEntitiesEachUpdate = 5;
        private MultiEntityUpdateLogPM multiEntityUpdateLogPM = null;
        private MultiEntityUpdateData multiEntityUpdateData = null;
        private MultiEntityUpdateLogExecutionService multiEntityUpdateLogService = null;
        private MultiEntityUpdateGeneralService multiEntityUpdateGeneralService = null;
        public MultiEntityUpdateExecutionService(DbQueueService queueService, QueueResponse queueResponse)
        {
            if (queueService == null || queueResponse == null) return;
            this.queueService = queueService;
            this.queueResponse = queueResponse;
            this.multiEntityUpdateId = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("MultiEntityUpdateId") ? queueResponse.MessageValues["MultiEntityUpdateId"].ToString() : "";
            this.tenant = GetTenantValueFromQueueResponse(queueResponse);
            Initiallize();
        }

        private int? GetTenantValueFromQueueResponse(QueueResponse queueResponse)
        {
            if (queueResponse.MessageValues == null) return null;
            if (!queueResponse.MessageValues.Keys.Contains("Tenant")) return null;

            string tenantString = queueResponse.MessageValues["Tenant"].ToString();
            if (string.IsNullOrEmpty(tenantString)) return null;

            return int.Parse(tenantString);
        }

        private void Initiallize()
        {
            automationSetValueResultService = new AutomationSetValueResultService();
            multiEntityUpdateLogService = new MultiEntityUpdateLogExecutionService(multiEntityUpdateId, (int)tenant);
            objectFieldRepository = new ObjectFieldRepository((int)tenant);
            multiEntityUpdateGeneralService = new MultiEntityUpdateGeneralService();
            modifiedMultiEntityUpdateDataEntities = new List<MultiEntityUpdateDataEntity>();
            multiEntityUpdateLogPM = multiEntityUpdateLogService.Get();
        }

        public void ExecuteMultiEntityUpdateQueue()
        {
            try
            {
                if (queueService == null || queueResponse == null || multiEntityUpdateLogPM == null) return;
                
                if (!multiEntityUpdateGeneralService.IsCompletedQueueService(multiEntityUpdateLogPM))
                {
                    multiEntityUpdateLogService.Update(new MultiEntityUpdateLogArgs() { StatusCode = "P", UpdatedEntitiesNumber = 0, StartDate = DateTime.UtcNow });
                    UpdateMultiEntities();
                    multiEntityUpdateLogService.Update(new MultiEntityUpdateLogArgs()
                    {
                        StatusCode = "D",
                        UpdatedEntitiesNumber = multiEntityUpdateLogPM.UpdatedEntitiesNumber,
                        DoneDate = DateTime.UtcNow,
                        XMLData = LogitudeXmlSerializer.SerializeObjectToXmlString(multiEntityUpdateData),
                        ExceptionMessage = ""
                    });
                }
                queueService.Complete();

            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }


        public void UpdateMultiEntities()
        {
            multiEntityUpdateData = GetMultiEntityUpdateData();
            objectFields = GetSetValueObjectFields();
            IEnumerable<List<MultiEntityUpdateDataEntity>> listOfMultiEntityUpdateDataEntities = multiEntityUpdateGeneralService.SplitListIntoNList(multiEntityUpdateData.Entities, numberOfExecutedEntitiesEachUpdate);
            listOfMultiEntityUpdateDataEntities.ToList().ForEach(entities =>
            {
                UpdateEntities(entities);
            });
            multiEntityUpdateData.Entities = modifiedMultiEntityUpdateDataEntities;

        }


        private MultiEntityUpdateData GetMultiEntityUpdateData()
        {
            return !string.IsNullOrEmpty(multiEntityUpdateLogPM.XMLData) ? LogitudeXmlSerializer.DeserializeObject<MultiEntityUpdateData>(multiEntityUpdateLogPM.XMLData) : null;
        }


        private List<ObjectField> GetSetValueObjectFields()
        {
            return objectFieldRepository.GetAutomationObjectFieldsByObjectTableId(multiEntityUpdateData.ObjectTableId, (int)tenant);
        }

        private void UpdateEntities(List<MultiEntityUpdateDataEntity> entityList)
        {
            object entityPMs = GetListofEntityPMs(entityList);
            Parallel.ForEach(((IEnumerable)entityPMs).Cast<object>().ToList(), (entityPM) =>
            {
                MultiEntityUpdateDataEntity multiEntityUpdateDataEntity = GetMultiEntityUpdateDataEntity(entityPM);
                try
                {
                    UpdateEntityPM( entityPM, multiEntityUpdateDataEntity);
                }
                catch (Exception exception)
                {
                    UpdateMultiEntityDataEntity(multiEntityUpdateDataEntity, exception);
                }

                modifiedMultiEntityUpdateDataEntities.Add(multiEntityUpdateDataEntity);
                multiEntityUpdateLogPM.UpdatedEntitiesNumber += 1;

            });

            multiEntityUpdateLogService.Update(new MultiEntityUpdateLogArgs() { StatusCode = "P", UpdatedEntitiesNumber = multiEntityUpdateLogPM.UpdatedEntitiesNumber });

        }

        private MultiEntityUpdateDataEntity GetMultiEntityUpdateDataEntity(object entityPM)
        {
            string entityPMId = entityPM.GetType().GetProperty("Id")?.GetValue(entityPM)?.ToString();
            MultiEntityUpdateDataEntity multiEntityUpdateDataEntity = multiEntityUpdateData.Entities.Where(multiEntityUpdateDataEntity2 => multiEntityUpdateDataEntity2.EntityId == entityPMId).FirstOrDefault();
            return multiEntityUpdateDataEntity;

        }

        private object GetListofEntityPMs(List<MultiEntityUpdateDataEntity> multiEntityUpdateDataEntities)
        {
            List<string> multiEntityUpdateDataEntitiesIds = multiEntityUpdateDataEntities.Select(multiEntityUpdateDataEntity => multiEntityUpdateDataEntity.EntityId).ToList();

            EntityGetReflector entityGetReflector = new EntityGetReflector()
            {
                EntityName = multiEntityUpdateData.ObjectTableName,
                MethodName = "Get" + multiEntityUpdateData.ObjectTableName + "PMsByIds",
                Parameters = new object[] { multiEntityUpdateDataEntitiesIds, (int)tenant },
                Tenant = (int)tenant
            };
            object entityPMs = InjectionUtil.Instance.GetEntity(entityGetReflector);
            return entityPMs;
        }

        private void UpdateEntityPM(object entityPM, MultiEntityUpdateDataEntity multiEntityUpdateDataEntity)
        {
            if (multiEntityUpdateDataEntity.StatusCode == "D") return;
            foreach (AutomationSetValue item in multiEntityUpdateData.SetValueLists)
            {
                SetNewValueToEntityPM(multiEntityUpdateData, entityPM, item);
            }

            InjectionUtil.Instance.UpdateEntity(entityPM, multiEntityUpdateData.ObjectTableName, (int)tenant);
            UpdateMultiEntityDataEntity(multiEntityUpdateDataEntity, null);
        }

        private void SetNewValueToEntityPM(MultiEntityUpdateData multiEntityUpdateData, object entityPM, AutomationSetValue item)
        {
            if (item.OperatorCode.Contains("F") && item.DataTypeCode.Trim() != "Boolean")
            {
                item.Value = entityPM.GetType().GetProperty(item.Value.Replace(multiEntityUpdateData.ObjectTableName + ".", ""))?.GetValue(entityPM)?.ToString();
                item.OperatorCode = "SV";
            }
            ObjectField objectField = objectFields.Where(objectField2 => objectField2.FieldCode == item.ObjectFieldCode).FirstOrDefault();
            object newfieldValue = automationSetValueResultService.ResolveSetFieldValue(new List<Field>(), item);
            SetPropertyValueToEntity(objectField, entityPM, newfieldValue);
        }

        private void SetPropertyValueToEntity(ObjectField objectField, object entity, object fieldValue)
        {
            PropertyInfo propInfo = entity.GetType().GetProperty(objectField.FieldName);
            if (propInfo != null) propInfo.SetValue(entity, fieldValue, null);
        }

        private MultiEntityUpdateDataEntity UpdateMultiEntityDataEntity(MultiEntityUpdateDataEntity multiEntityUpdateDataEntity, Exception exception)
        {
            MultiEntityUpdateDataEntity modifiedMultiEntityUpdateDataEntity = multiEntityUpdateDataEntity;
            modifiedMultiEntityUpdateDataEntity.HasException = exception == null ? false : true;
            modifiedMultiEntityUpdateDataEntity.Exception = exception == null ? "" : exception.Message.ToString();
            modifiedMultiEntityUpdateDataEntity.StatusCode = exception == null ? "D" : "F";
            return modifiedMultiEntityUpdateDataEntity;
        }



        private void HandleException(Exception exception)
        {
            DatabaseInitializer.RunOnSeconderyDB = false;
            multiEntityUpdateLogService.Update(new MultiEntityUpdateLogArgs()
            {
                StatusCode = "F",
                UpdatedEntitiesNumber = multiEntityUpdateLogPM.UpdatedEntitiesNumber,
                DoneDate = DateTime.UtcNow,
                XMLData = LogitudeXmlSerializer.SerializeObjectToXmlString(modifiedMultiEntityUpdateDataEntities),
                ExceptionMessage = multiEntityUpdateGeneralService.GetFullExceptionMessageFromException(exception)
            });
        }

      
    }

}