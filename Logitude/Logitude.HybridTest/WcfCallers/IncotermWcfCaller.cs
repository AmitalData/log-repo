using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class IncotermWcfCaller
    {
        public static Response CallIncotermUpsert()
        {
            IncotermPM entityPM = new IncotermPM()
            {
                Code = HybridData.IncotermCode,
                Name = "Hybrid Incoterm",
                LocalName = "Hybrid Incoterm",
                Freight = "C",
                OtherCharges = "C",
                Tenant = TestEnvironmentGlobalParameters.Tenant
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Incoterm",
                ServiceOperation = "Upsert",
                ServiceType = typeof(IncotermPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if(!serviceResponse.HasError && serviceResponse.Result != null)
                HybridData.IncotermId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareIncoterm()
        {
            if(HybridData.IncotermId == null)
            {
                return CallIncotermUpsert();
            }
            return new Response();
        }
    }
}
