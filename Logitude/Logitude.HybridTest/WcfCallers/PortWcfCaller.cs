using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class PortWcfCaller
    {
        public static Response CallFromPortUpsert()
        {
            Response prepareResponse = CountryWcfCaller.PrepareCountry();
            if (!prepareResponse.HasError)
            {
                PortPM entityPM = new PortPM()
                {
                    Code = HybridData.FromPortCode,
                    EnglishName = "Hybrid From Port",
                    LocalName = "Hybrid From Port",
                    CountryCode = HybridData.CountryCode,
                    CountryId = HybridData.CountryCode,
                    AddedManually = true,
                    IsAir = true,
                    IsOcean = true,
                    IsInland = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Port",
                    ServiceOperation = "Upsert",
                    ServiceType = typeof(PortPM),
                    ServiceFilterType = null,
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                if (!serviceResponse.HasError && serviceResponse.Result != null)
                    HybridData.FromPortId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response CallToPortUpsert()
        {
            Response prepareResponse = CountryWcfCaller.PrepareCountry();
            if (!prepareResponse.HasError)
            {
                PortPM entityPM = new PortPM()
                {
                    Code = HybridData.ToPortCode,
                    EnglishName = "Hybrid To Port",
                    LocalName = "Hybrid To Port",
                    CountryCode = HybridData.CountryCode,
                    CountryId = HybridData.CountryCode,
                    AddedManually = true,
                    IsAir = true,
                    IsOcean = true,
                    IsInland = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };

                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Port",
                    ServiceOperation = "Upsert",
                    ServiceType = typeof(PortPM),
                    ServiceFilterType = null,
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { entityPM, false };
                WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                if (!serviceResponse.HasError && serviceResponse.Result != null)
                    HybridData.ToPortId = serviceResponse.Result;
                return serviceResponse;
            }
            return prepareResponse;
        }
        public static Response PrepareFromPort()
        {
            if (HybridData.FromPortId == null)
            {
                return CallFromPortUpsert();
            }
            return new Response();
        }
        public static Response PrepareToPort()
        {
            if (HybridData.ToPortId == null)
            {
                return CallToPortUpsert();
            }
            return new Response();
        }
    }
}
