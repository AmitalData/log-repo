using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class AirlineWcfCaller
    {
        public static Response CallAirlineUpsert()
        {
            AirlinePM entityPM = new AirlinePM()
            {
                Code = HybridData.AirlineCode,
                EnglishName = "Hybrid Airline",
                LocalName = "Hybrid Airline",
                Prefix = TestEnvironmentGlobalParameters.Tenant.ToString(),
                Tenant = TestEnvironmentGlobalParameters.Tenant,
                CarrierTypeId = "AL",
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Airline",
                ServiceOperation = "Upsert",
                ServiceType = typeof(AirlinePM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if(serviceResponse.Result != null)
                HybridData.AirlineId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareAirline()
        {
            if(HybridData.AirlineId == null)
            {
                return CallAirlineUpsert();
            }
            return new Response();
        }
    }
}
