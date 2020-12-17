using Logitude.Server.Tools;
using System;
using System.Runtime.CompilerServices;

namespace Logitude.HybridTest.WcfCallers
{
    public class EntityWcfCaller
    {
        private Object thisLock = new Object();
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ServiceOutcome CallEntityUpsert<T>(T entityPM,string secondaryToken = null, AdditionalIncludedData includedData = null)
        {
            lock (thisLock)
            {
                string entityPMName = entityPM.GetType().Name; //ServiceNamePM = entityPM
                string serviceName = entityPMName.Substring(0, entityPMName.Length - 2);//ServiceName = entity
                if (serviceName == "CountryCity")
                {
                    serviceName = serviceName.Substring(7);
                }
                if (serviceName == "DocumentsFiling")
                {
                    serviceName = "DocumentIn";
                }
                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = serviceName,
                    ServiceOperation = "Upsert",
                    ServiceType = entityPM.GetType(),
                    SecondaryToken = secondaryToken,
                    IncludedData = includedData
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

                return serviceOutcome;
            };
        }
    }
}
