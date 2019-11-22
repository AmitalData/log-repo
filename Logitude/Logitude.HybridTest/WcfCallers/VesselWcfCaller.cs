using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class VesselWcfCaller
    {
        public static Response CallVesselUpsert()
        {
            VesselPM entityPM = new VesselPM()
            {
                Code = HybridData.VesselCode,
                EnglishName = "Hybrid Vessel",
                LocalName = "Hybrid Vessel",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Vessel",
                ServiceOperation = "Upsert",
                ServiceType = typeof(VesselPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if(serviceResponse.Result != null)
                HybridData.VesselId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareVessel()
        {
            if(HybridData.VesselId == null)
            {
                return CallVesselUpsert();
            }
            return new Response();
        }
    }
}
