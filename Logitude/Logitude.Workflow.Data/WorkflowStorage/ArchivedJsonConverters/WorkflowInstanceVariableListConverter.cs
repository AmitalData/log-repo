using System;
using Logitude.Workflow.Data.EntityLists;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Logitude.Workflow.Data.WorkflowStorage.ArchivedJsonConverters
{
    public class WorkflowInstanceVariableListConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(WorkFlowInstanceVariableList);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            JToken valueJToken = jObject["Value"];
            if(valueJToken.Type == JTokenType.Object || valueJToken.Type == JTokenType.Array)
            {
                jObject["Value"] = JsonConvert.SerializeObject(jObject["Value"]);
            }
            WorkFlowInstanceVariableList workFlowInstanceVariableList = new WorkFlowInstanceVariableList();
            serializer.Populate(jObject.CreateReader(), workFlowInstanceVariableList);
            return workFlowInstanceVariableList;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}