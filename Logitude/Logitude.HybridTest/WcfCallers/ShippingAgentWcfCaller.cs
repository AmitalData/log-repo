using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class ShippingAgentWcfCaller
    {
        public static Response CallShippingAgentUpsert()
        {
            Response prepareResponse = CountryWcfCaller.PrepareCountry();
            if (!prepareResponse.HasError)
            {
                ShippingAgentPM entityPM = new ShippingAgentPM()
                {
                    Code = HybridData.ShippingAgentCode,
                    EnglishName = "Hybrid ShippingAgent",
                    LocalName = "Hybrid ShippingAgent",
                    CityName = "Hybrid City",
                    CountryCode = HybridData.CountryCode,
                    PartnerTypeId = "SG",
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "ShippingAgent",
                    ServiceOperation = "Upsert",
                    ServiceType = typeof(ShippingAgentPM),
                    ServiceFilterType = null,
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                if (serviceResponse.Result != null)
                    HybridData.ShippingAgentId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response PrepareShippingAgent()
        {
            if (HybridData.ShippingAgentId == null)
            {
                return CallShippingAgentUpsert();
            }
            return new Response();
        }
    }
}
