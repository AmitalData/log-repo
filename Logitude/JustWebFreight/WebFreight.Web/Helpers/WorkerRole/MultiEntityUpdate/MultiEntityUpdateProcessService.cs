using Logitude.BL.InfrastructureModel.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.EntityChanges.AutomationResult;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.Helpers;
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

namespace WebFreight.Web.Helpers.WorkerRole.MultiEntityUpdate
{
    public class MultiEntityUpdateProcessService
    {
        public MultiEntityUpdateData multiEntityUpdateData = null;
        public List<MultiEntityUpdateDataEntity> modifiedMultiEntityUpdateDataEntities;
        private List<ObjectField> objectFields;
        private AutomationSetValueResultService automationSetValueResultService;
        private ObjectFieldRepository objectFieldRepository;
        private int tenant;
        private MultiEntityUpdateLogPM multiEntityUpdateLogPM;
        private int numberOfExecutedEntitiesEachUpdate = 5;
        private MultiEntityUpdateGeneralService multiEntityUpdateGeneralService = null;
        private MultiEntityUpdateLogExecutionService multiEntityUpdateLogService = null;
        public MultiEntityUpdateProcessService(MultiEntityUpdateLogPM multiEntityUpdateLogPM, MultiEntityUpdateLogExecutionService multiEntityUpdateLogService, int tenant)
        {
            this.multiEntityUpdateLogPM = multiEntityUpdateLogPM;
            this.tenant = tenant;
            this.multiEntityUpdateLogService = multiEntityUpdateLogService;
            Initiallize(tenant);
        }

        private void Initiallize(int tenant)
        {
            objectFieldRepository = new ObjectFieldRepository(tenant);
            modifiedMultiEntityUpdateDataEntities = new List<MultiEntityUpdateDataEntity>();
            automationSetValueResultService = new AutomationSetValueResultService();
            multiEntityUpdateGeneralService = new MultiEntityUpdateGeneralService();
        }

        public void Update()
        {
            multiEntityUpdateData = GetMultiEntityUpdateData();
            objectFields = GetSetValueObjectFields();
            IEnumerable<List<MultiEntityUpdateDataEntity>> listOfMultiEntityUpdateDataEntities = multiEntityUpdateGeneralService.SplitListIntoNList(multiEntityUpdateData.Entities, numberOfExecutedEntitiesEachUpdate);
            listOfMultiEntityUpdateDataEntities.ToList().ForEach(entities =>
            {
                HandleUpdateEntities(entities);
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
        
        private void HandleUpdateEntities(List<MultiEntityUpdateDataEntity> entityList)
        {
            object entityPMs = GetListofEntityPMs(entityList);
            Parallel.ForEach(((IEnumerable)entityPMs).Cast<object>().ToList(), (entityPM) =>
            {
                HandleUpdateEntityPM(entityPM);
            });
            multiEntityUpdateLogService.Update(new MultiEntityUpdateLogArgs() { StatusCode = "P", UpdatedEntitiesNumber = multiEntityUpdateLogPM.UpdatedEntitiesNumber });
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

        private void HandleUpdateEntityPM(object entityPM)
        {
            MultiEntityUpdateDataEntity multiEntityUpdateDataEntity = GetMultiEntityUpdateDataEntity(entityPM);
            try
            {
                UpdateEntityPM(entityPM, multiEntityUpdateDataEntity);
            }
            catch (Exception exception)
            {
                UpdateMultiEntityDataEntity(multiEntityUpdateDataEntity, exception);
            }

            modifiedMultiEntityUpdateDataEntities.Add(multiEntityUpdateDataEntity);
            multiEntityUpdateLogPM.UpdatedEntitiesNumber += 1;
        }

        private MultiEntityUpdateDataEntity GetMultiEntityUpdateDataEntity(object entityPM)
        {
            string entityPMId = entityPM.GetType().GetProperty("Id")?.GetValue(entityPM)?.ToString();
            MultiEntityUpdateDataEntity multiEntityUpdateDataEntity = multiEntityUpdateData.Entities.Where(multiEntityUpdateDataEntity2 => multiEntityUpdateDataEntity2.EntityId == entityPMId).FirstOrDefault();
            return multiEntityUpdateDataEntity;
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

        private void UpdateMultiEntityDataEntity(MultiEntityUpdateDataEntity multiEntityUpdateDataEntity, Exception exception)
        {
            multiEntityUpdateDataEntity.HasException = exception == null ? false : true;
            multiEntityUpdateDataEntity.Exception = exception == null ? "" : exception.Message.ToString();
            multiEntityUpdateDataEntity.StatusCode = exception == null ? "D" : "F";
        }
    }
}