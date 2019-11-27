using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class SpecialServicesTypeWcfCaller
    {
        public static Response CallSpecialServicesTypeUpsert()
        {
            SpecialServicesTypePM entityPM = new SpecialServicesTypePM()
            {
                Code = HybridData.SpecialServicesTypeCode,
                EnglishName = "Hybrid SpecialServicesType",
                LocalName = "Hybrid SpecialServicesType",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "SpecialServicesType",
                ServiceOperation = "Upsert",
                ServiceType = typeof(SpecialServicesTypePM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if(!serviceResponse.HasError && serviceResponse.Result != null)
                HybridData.SpecialServicesTypeId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareSpecialServicesType()
        {
            if(HybridData.SpecialServicesTypeId == null)
            {
                return CallSpecialServicesTypeUpsert();
            }
            return new Response();
        }
    }
}
