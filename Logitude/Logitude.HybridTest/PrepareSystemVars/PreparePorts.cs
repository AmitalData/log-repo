using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.LoginServiceReference;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.WcfCallers
{

    class PreparePorts
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public static void PreparePortsVars()
        {
            GetPortCode(HybridData.PortCodeLHR);
            GetPortCode(HybridData.PortCodeLAS + " ");
            GetPortCode(HybridData.PortCodeMIA);
            GetPortCode(HybridData.PortCodeAirJFK);
            GetPortCode(HybridData.PortCodeOceanSOU + "tham");
            GetPortCode(HybridData.PortCodeInlandNYC);
            GetPortCode(HybridData.PortCodeLON + "don");
            GetPortCode(HybridData.PortCodeMAN + "ch");
        }
        private static PortList GetPortCode(string code)
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Port",
                ServiceOperation = "GetList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(PortList),
                ServiceFilterType = typeof(ApiSearchFilters),
            };
            ApiSearchFilters filters = new ApiSearchFilters
            {
                Take = 10,
                SearchFields = code
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            PortList[] ports = (PortList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.Result);
            if (ports.Length == 0)
            {
                serviceParameters = new object[] { filters, 0, serviceResponse };
                serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

                ports = (PortList[])serviceOutcome.Result;
                Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
                CopyPortFromTenant0ToTestTenant(ports[0]);
            }
            return ports[0];
        }
        private static void CopyPortFromTenant0ToTestTenant(PortList portPM)
        {
            PortPM newPortPM = new PortPM()
            {
                Code = portPM.Code,
                EnglishName = portPM.EnglishName,
                LocalName = portPM.EnglishName,
                CountryCode = portPM.CountryCode,
                CountryId = portPM.CountryCode,
                AddedManually = portPM.AddedManually,
                IsAir = portPM.IsAir,
                IsOcean = portPM.IsOcean,
                IsInland = portPM.IsInland,
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            AssertResponse(newPortPM);
        }
        private static void AssertResponse<T>(T entityPM)
        {
            ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Prepare Port Vars Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Prepare Port Vars Failed! " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
