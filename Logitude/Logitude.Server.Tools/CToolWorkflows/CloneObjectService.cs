using Newtonsoft.Json;

namespace Logitude.Server.Tools.CToolWorkflows
{
    public static class CloneObjectService
    {
        public static T Clone<T>(T source)
        {
            string serializedSource = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serializedSource);
        }
    }
}
