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

namespace CommunicationWorkerRole.Services
{
    public class MultiEntityUpdateService
    {
        private readonly DbQueueService queueService = null;
        private readonly QueueResponse queueResponse = null;
        private readonly int? tenant;
        private readonly string multiEntityUpdateId = string.Empty;
        private readonly MultiEntityUpdateLogPM multiEntityUpdateLogPM;
        private MultiEntityUpdateLogService multiEntityUpdateLogService;
        private ObjectFieldRepository objectFieldRepository;
        private AutomationSetValueResultService automationSetValueResultService;
        private List<MultiEntityUpdateDataEntity> modifiedMultiEntityUpdateDataEntities;
        private List<ObjectField> objectFields;

        public MultiEntityUpdateService(DbQueueService queueService, QueueResponse queueResponse)
        {
            this.queueService = queueService;
            this.queueResponse = queueResponse;
            if (queueService == null || queueResponse == null) return;

            multiEntityUpdateId = GetMultiEntityUpdateIdValueFromQueueResponse(queueResponse);
            tenant = GetTenantValueFromQueueResponse(queueResponse);
            if (!string.IsNullOrEmpty(multiEntityUpdateId)) multiEntityUpdateLogPM = GetMultiEntityUpdateLogPM();
            InitiallizeServices();
            InitiallizeRepositories();
        }

        private string GetMultiEntityUpdateIdValueFromQueueResponse(QueueResponse queueResponse)
        {
            if (queueResponse.MessageValues == null) return string.Empty;
            if (!queueResponse.MessageValues.Keys.Contains("MultiEntityUpdateId")) return string.Empty;

            string multiEntityUpdateId = queueResponse.MessageValues["MultiEntityUpdateId"].ToString();
            return multiEntityUpdateId;
        }

        private int? GetTenantValueFromQueueResponse(QueueResponse queueResponse)
        {
            if (queueResponse.MessageValues == null) return null;
            if (!queueResponse.MessageValues.Keys.Contains("Tenant")) return null;

            string tenantString = queueResponse.MessageValues["Tenant"].ToString();
            if (string.IsNullOrEmpty(tenantString)) return null;

            return int.Parse(tenantString);
        }

        private MultiEntityUpdateLogPM GetMultiEntityUpdateLogPM()
        {
            MultiEntityUpdateLogQuery multiEntityUpdateLogQuery = new MultiEntityUpdateLogQuery((int)tenant);
            MultiEntityUpdateLogPM multiEntityUpdateLogPM = multiEntityUpdateLogQuery.GetSinglePM(multiEntityUpdateId, (int)tenant);

            return multiEntityUpdateLogPM;
        }

        private void InitiallizeServices()
        {
            IWebFreightContext objectContext = WebFreightContext.GetContext((int)tenant);
            multiEntityUpdateLogService = new MultiEntityUpdateLogService(objectContext, (int)tenant);
            automationSetValueResultService = new AutomationSetValueResultService();
        }

        private void InitiallizeRepositories()
        {
            objectFieldRepository = new ObjectFieldRepository((int)tenant);
        }

        public void ExecuteMultiEntityUpdateQueue()
        {
            try
            {
                if (queueService == null || queueResponse == null || tenant == null) return;
                if (IsCompletedQueueService()) return;

                UpdateMultiEntityUpdateLog(new MultiEntityUpdateLogArgs() { StatusCode = "P", UpdatedEntitiesNumber = 0, StartDate = DateTime.UtcNow });

                MultiEntityUpdateData multiEntityUpdateData = GetMultiEntityUpdateData();
                objectFields = GetAutomationObjectFields(multiEntityUpdateData);
                List<MultiEntityUpdateDataEntity> multiEntityUpdateDataEntities = multiEntityUpdateData.Entities;
                IEnumerable<List<MultiEntityUpdateDataEntity>> listOfMultiEntityUpdateDataEntities = SplitListIntoNList(multiEntityUpdateDataEntities, 5);
                modifiedMultiEntityUpdateDataEntities = new List<MultiEntityUpdateDataEntity>();

                listOfMultiEntityUpdateDataEntities.ToList().ForEach(entityList =>
                {
                    HandleProcessOfSubMultipleUpdate(entityList, multiEntityUpdateData, multiEntityUpdateDataEntities);
                });
                SuccessfullyFinishedMultiEntityUpdateLog(multiEntityUpdateData);
            }
            catch (Exception exception)
            {
                HandleMultiEntityUpdateLogException(exception);
            }
        }

        private bool IsCompletedQueueService()
        {
            bool markAsCompleted = false;
            if (multiEntityUpdateLogPM == null) markAsCompleted = true;
            if (multiEntityUpdateLogPM.RetryNumber >= 2) markAsCompleted = true;
            if (multiEntityUpdateLogPM.StatusCode != "W" && multiEntityUpdateLogPM.StatusCode != "P") markAsCompleted = true;

            if (!markAsCompleted) return false;

            queueService.Complete();
            return true;
        }

        private void UpdateMultiEntityUpdateLog(MultiEntityUpdateLogArgs multiEntityUpdateLogArgs)
        {
            if (multiEntityUpdateLogArgs != null)
            {
                multiEntityUpdateLogPM.StartDate = multiEntityUpdateLogArgs.StartDate != null ? multiEntityUpdateLogArgs.StartDate : multiEntityUpdateLogPM.StartDate;
                multiEntityUpdateLogPM.DoneDate = multiEntityUpdateLogArgs.DoneDate != null ? multiEntityUpdateLogArgs.DoneDate : multiEntityUpdateLogPM.DoneDate;
                multiEntityUpdateLogPM.StatusCode = !string.IsNullOrEmpty(multiEntityUpdateLogArgs.StatusCode) ? multiEntityUpdateLogArgs.StatusCode : multiEntityUpdateLogPM.StatusCode;
                multiEntityUpdateLogPM.UpdatedEntitiesNumber = multiEntityUpdateLogArgs.UpdatedEntitiesNumber;
                multiEntityUpdateLogPM.XMLData = !string.IsNullOrEmpty(multiEntityUpdateLogArgs.XMLData) ? multiEntityUpdateLogArgs.XMLData : multiEntityUpdateLogPM.XMLData;
                multiEntityUpdateLogPM.ExceptionMessage = !string.IsNullOrEmpty(multiEntityUpdateLogArgs.ExceptionMessage) ? multiEntityUpdateLogArgs.ExceptionMessage : multiEntityUpdateLogPM.ExceptionMessage;

                multiEntityUpdateLogService.Update(multiEntityUpdateLogPM);
            }
        }

        private MultiEntityUpdateData GetMultiEntityUpdateData()
        {
            return !string.IsNullOrEmpty(multiEntityUpdateLogPM.XMLData) ? LogitudeXmlSerializer.DeserializeObject<MultiEntityUpdateData>(multiEntityUpdateLogPM.XMLData) : null;
        }

        private List<ObjectField> GetAutomationObjectFields(MultiEntityUpdateData multiEntityUpdateData)
        {
            return objectFieldRepository.GetAutomationObjectFieldsByObjectTableId(multiEntityUpdateData.ObjectTableId, (int)tenant);
        }

        public IEnumerable<List<T>> SplitListIntoNList<T>(List<T> fullList, int nSize)

        {
            for (int i = 0; i < fullList.Count; i += nSize)
            {
                yield return fullList.GetRange(i, Math.Min(nSize, fullList.Count - i)).ToList();
            }
        }
        
        private void HandleProcessOfSubMultipleUpdate(List<MultiEntityUpdateDataEntity> entityList, MultiEntityUpdateData multiEntityUpdateData, List<MultiEntityUpdateDataEntity> multiEntityUpdateDataEntities)
        {
            object entityPMs = GetListofEntityPMs(multiEntityUpdateData, entityList);
            Parallel.ForEach(((IEnumerable)entityPMs).Cast<object>().ToList(), (entityPM) =>
            {
                string entityPMId = entityPM.GetType().GetProperty("Id")?.GetValue(entityPM)?.ToString();
                MultiEntityUpdateDataEntity multiEntityUpdateDataEntity = multiEntityUpdateDataEntities.Where(multiEntityUpdateDataEntity2 => multiEntityUpdateDataEntity2.EntityId == entityPMId).FirstOrDefault();
                multiEntityUpdateLogPM.UpdatedEntitiesNumber += 1;
                try
                {
                    UpdateEntityPM(multiEntityUpdateData, entityPM, multiEntityUpdateDataEntity);
                }
                catch (Exception exception)
                {
                    MapMultiEntityUpdateDataEntity(multiEntityUpdateDataEntity, exception);
                }
                modifiedMultiEntityUpdateDataEntities.Add(multiEntityUpdateDataEntity);
            });

            UpdateMultiEntityUpdateLog(new MultiEntityUpdateLogArgs()
            {
                StatusCode = "P",
                UpdatedEntitiesNumber = multiEntityUpdateLogPM.UpdatedEntitiesNumber,
            });
        }

        private object GetListofEntityPMs(MultiEntityUpdateData multiEntityUpdateData, List<MultiEntityUpdateDataEntity> multiEntityUpdateDataEntities)
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

        private void UpdateEntityPM(MultiEntityUpdateData multiEntityUpdateData, object entityPM, MultiEntityUpdateDataEntity multiEntityUpdateDataEntity)
        {
            if (multiEntityUpdateDataEntity.StatusCode == "D") return;
            foreach (AutomationSetValue item in multiEntityUpdateData.SetValueLists)
            {
                MapNewValueToEntityPM(multiEntityUpdateData, entityPM, item);
            }

            InjectionUtil.Instance.UpdateEntity(entityPM, multiEntityUpdateData.ObjectTableName, (int)tenant);
            MapMultiEntityUpdateDataEntity(multiEntityUpdateDataEntity, null);
        }

        private void MapNewValueToEntityPM(MultiEntityUpdateData multiEntityUpdateData, object entityPM, AutomationSetValue item)
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
        
        private MultiEntityUpdateDataEntity MapMultiEntityUpdateDataEntity(MultiEntityUpdateDataEntity multiEntityUpdateDataEntity, Exception exception)
        {
            MultiEntityUpdateDataEntity modifiedMultiEntityUpdateDataEntity = multiEntityUpdateDataEntity;
            modifiedMultiEntityUpdateDataEntity.HasException = exception == null ? false : true;
            modifiedMultiEntityUpdateDataEntity.Exception = exception == null ? "" : exception.Message.ToString();
            modifiedMultiEntityUpdateDataEntity.StatusCode = exception == null ? "D" : "F";

            return modifiedMultiEntityUpdateDataEntity;
        }

        private void SuccessfullyFinishedMultiEntityUpdateLog(MultiEntityUpdateData multiEntityUpdateData)
        {
            multiEntityUpdateData.Entities = modifiedMultiEntityUpdateDataEntities;
            UpdateMultiEntityUpdateLog(new MultiEntityUpdateLogArgs()
            {
                StatusCode = "D",
                UpdatedEntitiesNumber = multiEntityUpdateLogPM.UpdatedEntitiesNumber,
                DoneDate = DateTime.UtcNow,
                XMLData = LogitudeXmlSerializer.SerializeObjectToXmlString(multiEntityUpdateData),
                ExceptionMessage = ""
            });
            queueService.Complete();
        }
        
        private void HandleMultiEntityUpdateLogException(Exception exception)
        {
            DatabaseInitializer.RunOnSeconderyDB = false;
            UpdateMultiEntityUpdateLog(new MultiEntityUpdateLogArgs()
            {
                StatusCode = "F",
                UpdatedEntitiesNumber = multiEntityUpdateLogPM.UpdatedEntitiesNumber,
                DoneDate = DateTime.UtcNow,
                XMLData = LogitudeXmlSerializer.SerializeObjectToXmlString(modifiedMultiEntityUpdateDataEntities),
                ExceptionMessage = GetFullExceptionMessageFromException(exception)
            });
        }
        
        private string GetFullExceptionMessageFromException(Exception exception)
        {
            if (exception == null) return string.Empty;

            string exceptionMessage = exception.Message;
            if (exception.InnerException != null) exceptionMessage = exceptionMessage + Environment.NewLine + exception.InnerException;
            if (exception.StackTrace != null) exceptionMessage = exceptionMessage + Environment.NewLine + "Stack trace: " + exception.StackTrace;
            return exceptionMessage;
        }
    }

    public class MultiEntityUpdateLogArgs
    {
        public string StatusCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public int UpdatedEntitiesNumber { get; set; }
        public string XMLData { get; set; }
        public string ExceptionMessage { get; set; }
    }
}