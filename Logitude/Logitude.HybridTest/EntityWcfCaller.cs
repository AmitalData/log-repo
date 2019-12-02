using Logitude.Server.Tools;

namespace Logitude.HybridTest.WcfCallers
{
    class EntityWcfCaller
    {
        public static Response CallEntityUpsert<T>(T entityPM)
        {
            string entityPMName = entityPM.GetType().Name; //ServiceNamePM = entityPM
            string serviceName = entityPMName.Substring(0, entityPMName.Length - 2);//ServiceName = entity
            if (serviceName == "CountryCity")
            {
                serviceName = serviceName.Substring(7);
            }
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = serviceName,
                ServiceOperation = "Upsert",
                ServiceType = entityPM.GetType(),
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            return serviceResponse;
        }
    }
}
