using Logitude.Server.Tools;

namespace Logitude.HybridTest.WcfCallers
{
    class EntityWcfCaller
    {
        public static Response CallEntityUpsert<T>(T entityPM)
        {
            string ServiceNamePM = entityPM.GetType().Name; //ServiceNamePM = entityPM
            ServiceNamePM = ServiceNamePM.Substring(0, ServiceNamePM.Length - 2);//ServiceName = entity
            if (ServiceNamePM == "CountryCity")
            {
                ServiceNamePM = ServiceNamePM.Substring(7);
            }
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = ServiceNamePM,
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
