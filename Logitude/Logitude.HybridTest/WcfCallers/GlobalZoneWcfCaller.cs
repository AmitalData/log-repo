using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class GlobalZoneWcfCaller
    {
        public static Response CallGlobalZoneUpsert()
        {
            GlobalZonePM entityPM = new GlobalZonePM()
            {
                Code = HybridData.GlobalZoneCode,
                EnglishName = "Hybrid GlobalZone",
                LocalName = "Hybrid GlobalZone",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "GlobalZone",
                ServiceOperation = "Upsert",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, typeof(GlobalZonePM), ref serviceResponse);
            if(serviceResponse.Result != null)
                HybridData.GlobalZoneId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareGlobalZone()
        {
            if(HybridData.GlobalZoneId == null)
            {
                return CallGlobalZoneUpsert();
            }
            return new Response();
        }
    }
}
