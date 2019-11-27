using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class AgentWcfCaller
    {
        public static Response CallAgentUpsert()
        {
            Response prepareResponse = CountryWcfCaller.PrepareCountry();
            if (!prepareResponse.HasError)
            {
                AgentPM entityPM = new AgentPM()
                {
                    Code = HybridData.AgentCode,
                    EnglishName = "Hybrid Agent",
                    LocalName = "Hybrid Agent",
                    CityName = "Hybrid City",
                    CountryCode = HybridData.CountryCode,
                    PartnerTypeId = "AG",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Agent",
                    ServiceOperation = "Upsert",
                    ServiceType = typeof(AgentPM),
                    ServiceFilterType = null,
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                if (!serviceResponse.HasError && serviceResponse.Result != null)
                    HybridData.AgentId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response PrepareAgent()
        {
            if (HybridData.AgentId == null)
            {
                return CallAgentUpsert();
            }
            return new Response();
        }
    }
}
