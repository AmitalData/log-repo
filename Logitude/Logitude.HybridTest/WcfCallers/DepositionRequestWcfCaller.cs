using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class DepositionRequestWcfCaller
    {
        public static Response CallDepositionRequestUpsert()
        {
            DepositionRequestPM entityPM = new DepositionRequestPM()
            {
                VendorCode = HybridData.VendorCode,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "DepositionRequest",
                ServiceOperation = "Upsert",
                ServiceType = typeof(DepositionRequestPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            return serviceResponse;
        }
    }
}
