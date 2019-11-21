using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class TruckerWcfCaller
    {
        public static Response CallTruckerUpsert()
        {
            Response prepareResponse = CountryWcfCaller.PrepareCountry();
            if (!prepareResponse.HasError)
            {
                TruckerPM entityPM = new TruckerPM()
                {
                    Code = HybridData.TruckerCode,
                    EnglishName = "Hybrid Trucker",
                    LocalName = "Hybrid Trucker",
                    CityName = "Hybrid City",
                    CountryCode = HybridData.CountryCode,
                    CarrierTypeId = "TR",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Trucker",
                    ServiceOperation = "Upsert",
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, typeof(TruckerPM), ref serviceResponse);
                if (serviceResponse.Result != null)
                    HybridData.TruckerId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response PrepareTrucker()
        {
            if (HybridData.TruckerId == null)
            {
                return CallTruckerUpsert();
            }
            return new Response();
        }
    }
}
