using Newtonsoft.Json;

namespace Logitude.Server.Tools.CToolWorkflows
{
    public static class CloneObjectService
    {
        public static T Clone<T>(T source)
        {
            if (source != null)
            {
                string serializedSource = JsonConvert.SerializeObject(source, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                return JsonConvert.DeserializeObject<T>(serializedSource);
            }

            return source;
        }
    }
}
