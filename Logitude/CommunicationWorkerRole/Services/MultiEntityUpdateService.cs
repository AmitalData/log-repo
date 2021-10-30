using CommunicationWorkerRole.MultiEntityUpdate;
using Logitude.Server.Tools;
using Logitude.Server.Tools.EntityChanges.AutomationResult;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
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

namespace CommunicationWorkerRole.Services
{
    public class MultiEntityUpdateService
    {
        private readonly DbQueueService queueService = null;
        private readonly QueueResponse queueResponse = null;
        private readonly int? tenant;
        private readonly string multiEntityUpdateId = string.Empty;

        public MultiEntityUpdateService(DbQueueService queueService, QueueResponse queueResponse)
        {
            this.queueService = queueService;
            this.queueResponse = queueResponse;
            if (queueService == null || queueResponse == null) return;

            multiEntityUpdateId = GetMultiEntityUpdateIdValueFromQueueResponse(queueResponse);
            tenant = GetTenantValueFromQueueResponse(queueResponse);
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

        public void ExecuteMultiEntityUpdateQueue()
        {
            try
            {
                if (queueService != null && queueResponse != null && tenant != null)
                {
                    //Get Multi Entity Update PM by multiEntityUpdateId

                    MultiEntityUpdateLogPM multiEntityUpdateLogPM = new MultiEntityUpdateLogPM();// GET
                    if (multiEntityUpdateLogPM == null) queueService.Complete();
                    if (multiEntityUpdateLogPM.RetryNumber >=2) queueService.Complete();
                    if (multiEntityUpdateLogPM.StatusCode != "W" && multiEntityUpdateLogPM.StatusCode != "P") queueService.Complete();

                    multiEntityUpdateLogPM.UpdatedEntitiesNumber = 0;
                    multiEntityUpdateLogPM.StartDate = DateTime.UtcNow;
                    MultiEntityUpdateData multiEntityUpdateData = !string.IsNullOrEmpty(multiEntityUpdateLogPM.XMLData) ? LogitudeXmlSerializer.DeserializeObject<MultiEntityUpdateData>(multiEntityUpdateLogPM.XMLData) : null;
                    List<MultiEntityUpdateDataEntity> multiEntityUpdateDataEntities = multiEntityUpdateData.Entities;

                    List<string> multiEntityUpdateDataEntitiesIds = multiEntityUpdateDataEntities.Select(multiEntityUpdateDataEntity => multiEntityUpdateDataEntity.EntityId).ToList();

                    EntityGetReflector entityGetReflector = new EntityGetReflector()
                    {
                        EntityName = multiEntityUpdateData.ObjectTableName,
                        MethodName = "Get" + multiEntityUpdateData.ObjectTableName + "PMsByIds",
                        Parameters = new object[] { multiEntityUpdateDataEntitiesIds, (int)tenant }, 
                        Tenant = (int)tenant
                    };
                    object entityPMs = InjectionUtil.Instance.GetEntity(entityGetReflector);

                    if (entityPMs == null) queueService.Complete();

                    IEnumerable<List<object>> listOfEntitiesList = (IEnumerable<List<object>>)SplitListIntoNList(((IList)entityPMs).Cast<List<object>>().ToList(), 5);

                    List<MultiEntityUpdateDataEntity> multiEntityUpdateDataEntities1 = new List<MultiEntityUpdateDataEntity>();
                    ObjectFieldRepository objectFieldRepository = new ObjectFieldRepository((int)tenant);
                    AutomationSetValueResultService automationSetValueResultService = new AutomationSetValueResultService();
                    ObjectField objectField;

                    listOfEntitiesList.ToList().ForEach(entityList => {
                        Parallel.ForEach(entityList, (entityPM) => {
                            string entityPMId = entityPM.GetType().GetProperty("Id")?.GetValue(entityPM)?.ToString();
                            MultiEntityUpdateDataEntity multiEntityUpdateDataEntity = multiEntityUpdateDataEntities.Where(multiEntityUpdateDataEntity => multiEntityUpdateDataEntity.EntityId == entityPMId).FirstOrDefault();
                            multiEntityUpdateLogPM.UpdatedEntitiesNumber += 1;
                            try
                            {
                                if (multiEntityUpdateDataEntity.StatusCode != "D")
                                {
                                    foreach (AutomationSetValue item in multiEntityUpdateData.AutomationSetValue)
                                    {
                                        objectField = objectFieldRepository.GetSingleObjectFieldByFieldCode(item.ObjectFieldCode, (int)tenant);
                                        object newfieldValue = automationSetValueResultService.ResolveSetFieldValue(new List<Field>(), item);
                                        SetPropertyValueToEntity(objectField, entityPM, newfieldValue);
                                    }

                                    InjectionUtil.Instance.UpdateEntity(entityPM, multiEntityUpdateData.ObjectTableName, (int)tenant);

                                    multiEntityUpdateDataEntity.HasException = false;
                                    multiEntityUpdateDataEntity.Exception = "";
                                    multiEntityUpdateDataEntity.StatusCode = "D";
                                }
                            } catch (Exception exception) {
                                multiEntityUpdateDataEntity.HasException = true;
                                multiEntityUpdateDataEntity.Exception = exception.Message.ToString();
                                multiEntityUpdateDataEntity.StatusCode = "F";
                            }
                            multiEntityUpdateDataEntities1.Add(multiEntityUpdateDataEntity);
                        });

                        multiEntityUpdateLogPM.StatusCode = "P";
                        //update multiEntityUpdateLogPM
                    });

                    multiEntityUpdateData.Entities = multiEntityUpdateDataEntities1;

                    multiEntityUpdateLogPM.XMLData = LogitudeXmlSerializer.SerializeObjectToXmlString(multiEntityUpdateData);
                    multiEntityUpdateLogPM.DoneDate = DateTime.UtcNow;
                    multiEntityUpdateLogPM.StatusCode = "D";
                    multiEntityUpdateLogPM.ExceptionMessage = "";
                    //Update multiEntityUpdateLogPM
                    queueService.Complete();
                }
            }
            catch (Exception exception)
            {
                DatabaseInitializer.RunOnSeconderyDB = false;
                //HandleMultiEntityUpdateException(ex);
            }
        }


        public IEnumerable<List<T>> SplitListIntoNList<T>(List<T> fullList, int nSize)

        {
            for (int i = 0; i < fullList.Count; i += nSize)
            {
                yield return fullList.GetRange(i, Math.Min(nSize, fullList.Count - i)).ToList();
            }
        }

        private void SetPropertyValueToEntity(ObjectField objectField, object entity, object fieldValue)
        {
            PropertyInfo propInfo = entity.GetType().GetProperty(objectField.FieldName);
            if (propInfo != null) propInfo.SetValue(entity, fieldValue, null);
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


    public class MultiEntityUpdateLogPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CreatedByUserId { get; set; }
        public string StatusCode { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public string XMLData { get; set; }
        public string ObjectTableId { get; set; }
        public int RetryNumber { get; set; }
        public int UpdatedEntitiesNumber { get; set; }
        public DateTime? StartDate { get; set; }
    }
    public class MultiEntityUpdateData
    {
        public string ObjectTableId { get; set; }
        public string ObjectTableName { get; set; }

        public string UserId { get; set; }

        public List<AutomationSetValue> AutomationSetValue { get; set; }

        public List<MultiEntityUpdateDataEntity> Entities { get; set; }
    }



    public class MultiEntityUpdateDataEntity
    {
        public string EntityId { get; set; }

        public string Tenant { get; set; }

        public string EntityNumber { get; set; }

        public bool HasException { get; set; }

        public string Exception { get; set; }
        public string StatusCode { get; set; }
    }
}