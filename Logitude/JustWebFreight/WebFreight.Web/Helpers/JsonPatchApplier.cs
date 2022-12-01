using Marvin.JsonPatch.Operations;
using System;
using System.Collections.Generic;
using Marvin.JsonPatch;

namespace Logitude.Server.Tools.Helpers
{
    public class JsonPatchApplier<T> where T : class
    {
        protected JsonPatchDocument<T> JsonPatchDocument;
        protected T ObjectToApplyPatchOn;

        public JsonPatchApplier(JsonPatchDocument<T> jsonPatchDocument, T objectToApplyPatchOn)
        {
            JsonPatchDocument = jsonPatchDocument;
            ObjectToApplyPatchOn = objectToApplyPatchOn;
        }

        public T Apply()
        {
            if (JsonPatchDocument != null && JsonPatchDocument.Operations != null && JsonPatchDocument.Operations.Count > 0 && ObjectToApplyPatchOn != null)
            {
                foreach (Operation<T> operation in JsonPatchDocument.Operations)
                {
                    ApplyOperation(operation);
                }
            }
            return ObjectToApplyPatchOn;
        }

        protected void ApplyOperation(Operation<T> operation)
        {
            if (JsonPatchDocument != null && operation != null)
            {
                try
                {
                    List<Operation<T>> operations = new List<Operation<T>>() { operation };
                    JsonPatchDocument<T> jsonPatchDocument = new JsonPatchDocument<T>(operations, JsonPatchDocument.ContractResolver);
                    jsonPatchDocument.ApplyTo(ObjectToApplyPatchOn);
                }
                catch (Exception) { }
            }
        }
    }
}