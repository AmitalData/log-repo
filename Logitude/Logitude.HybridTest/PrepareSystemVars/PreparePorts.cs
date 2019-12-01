using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.LoginServiceReference;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.WcfCallers
{

    class PreparePorts
    {
        public static void PreparePortsVars()
        {
            GetPortIdLHR();
            GetPortIdLAS();
            GetPortIdMIA();
            GetPortIdAirJFK();
            GetPortIdOceanSOU();
            GetPortIdInlandNYC();
            GetPortIdLON();
            GetPortIdMAN();
        }
        private static void GetPortIdLHR()
        {
            PortList LHRport = GetPortId(HybridData.PortCodeLHR);
            HybridData.PortIdLHR = LHRport.Id;
            HybridData.CountryIdForPortLHR = LHRport.CountryId;
        }
        private static void GetPortIdLAS()
        {
            PortList LASport = GetPortId(HybridData.PortCodeLAS+" ");
            HybridData.PortIdLAS = LASport.Id;
        }
        private static void GetPortIdMIA()
        {
            PortList MIAport = GetPortId(HybridData.PortCodeMIA);
            HybridData.PortIdMIA = MIAport.Id;
        }
        private static void GetPortIdAirJFK()
        {
            PortList JFKport = GetPortId(HybridData.PortCodeAirJFK);
            HybridData.PortIdAirJFK = JFKport.Id;
            HybridData.CountryIdForPortJFK = JFKport.CountryId;
        }
        private static void GetPortIdOceanSOU()
        {
            PortList SOUport = GetPortId(HybridData.PortCodeOceanSOU+ "tham");
            HybridData.PortIdOceanSOU = SOUport.Id;
        }
        private static void GetPortIdInlandNYC()
        {
            PortList NYCport = GetPortId(HybridData.PortCodeInlandNYC);
            HybridData.PortIdInlandNYC = NYCport.Id;
        }
        private static void GetPortIdLON()
        {
            PortList LONport = GetPortId(HybridData.PortCodeLON + "don");
            HybridData.PortIdLON = LONport.Id;
        }
        private static void GetPortIdMAN()
        {
            PortList SOUport = GetPortId(HybridData.PortCodeMAN + "ch");
            HybridData.PortIdMAN = SOUport.Id;
        }
        private static PortList GetPortId(string code)
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
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            PortList[] ports = (PortList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            if (ports.Length == 0)
            {
                serviceParameters = new object[] { filters, 0, serviceResponse };
                ports = (PortList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
                CopyPortFromTenant0ToTestTenant(ports[0]);
            }
            return ports[0];
        }
        private static void CopyPortFromTenant0ToTestTenant(PortList portPM)
        {
            portPM.Tenant = TestEnvironmentGlobalParameters.Tenant;
            portPM.AddedManually = true;
            AssertResponse(portPM);
        }
        private static void AssertResponse<T>(T entityPM)
        {
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceResponse.HasError, "Prepare Port Vars Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Prepare Port Vars Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
