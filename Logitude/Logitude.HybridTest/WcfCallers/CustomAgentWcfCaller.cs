using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class CustomAgentWcfCaller
    {
        public static Response CallCustomAgentUpsert()
        {
            Response prepareResponse = CountryWcfCaller.PrepareCountry();
            if (!prepareResponse.HasError)
            {
                CustomAgentPM entityPM = new CustomAgentPM()
                {
                    Code = HybridData.CustomAgentCode,
                    EnglishName = "Hybrid CustomAgent",
                    LocalName = "Hybrid CustomAgent",
                    CityName = "Hybrid City",
                    CountryCode = HybridData.CountryCode,
                    PartnerTypeId = "CG",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "CustomAgent",
                    ServiceOperation = "Upsert",
                    ServiceType = typeof(CustomAgentPM),
                    ServiceFilterType = null,
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                if (!serviceResponse.HasError && serviceResponse.Result != null)
                    HybridData.CustomAgentId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response PrepareCustomAgent()
        {
            if (HybridData.CustomAgentId == null)
            {
                return CallCustomAgentUpsert();
            }
            return new Response();
        }
    }
}
