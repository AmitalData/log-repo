using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Logitude.Workflow.Data.EntityLists;
using Logitude.Workflow.Data.WorkflowStorage.ArchivedJsonConverters;

namespace Logitude.Workflow.Data.WorkflowStorage
{
    public class WorkflowInstanceStorage : WorkflowAzureStorage
    {
        public WorkflowInstanceStorage() : base(WorkflowStorageContainers.WorkflowInstances){}

        public List<WorkFlowInstanceActivityList> GetActivities(string workflowInstanceId, int tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(workflowInstanceId))
                {
                    byte[] zipFileBytes = GetWorkFlowInstanceBlobBytes(workflowInstanceId, tenant);
                    string jsonFileName = GetWorkFlowInstanceJsonFileName(workflowInstanceId, tenant);
                    ArchivedJsonDeserializer deserializer = new ArchivedJsonDeserializer(zipFileBytes, jsonFileName, null);
                    List<WorkFlowInstanceActivityList> activities = deserializer.Deserialize<List<WorkFlowInstanceActivityList>>("WorkFlowInstanceActivities");
                    return activities;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<WorkFlowInstanceVariableList> GetVariables(string workflowInstanceId, int tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(workflowInstanceId))
                {
                    byte[] zipFileBytes = GetWorkFlowInstanceBlobBytes(workflowInstanceId, tenant);
                    string jsonFileName = GetWorkFlowInstanceJsonFileName(workflowInstanceId, tenant);
                    JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                    {
                        Converters = { new WorkflowInstanceVariableListConverter() }
                    };
                    ArchivedJsonDeserializer deserializer = new ArchivedJsonDeserializer(zipFileBytes, jsonFileName, jsonSerializerSettings);
                    List<WorkFlowInstanceVariableList> variables = deserializer.Deserialize<List<WorkFlowInstanceVariableList>>("WorkFlowInstanceVariables");
                    return variables;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private byte[] GetWorkFlowInstanceBlobBytes(string workflowInstanceId, int tenant)
        {
            string zipFileName = string.Format("{0}.{1}.zip", workflowInstanceId, tenant);
            byte[] zipFileBytes = GetBlobBytes(zipFileName);
            return zipFileBytes;
        }

        private string GetWorkFlowInstanceJsonFileName(string workflowInstanceId, int tenant)
        {
            return string.Format("{0}.{1}.json", workflowInstanceId, tenant);
        }
    }
}