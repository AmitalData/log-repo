using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class PortTest
    {
        [TestMethod]
        public void Test_PORT_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceFromResponse = PortWcfCaller.CallFromPortUpsert();
            Assert.IsFalse(serviceFromResponse.HasError, serviceFromResponse.ErrorMessage);
            Assert.IsNotNull(serviceFromResponse.Result, "Upsert From Port Failed! " + serviceFromResponse.ErrorMessage);
            Response serviceToResponse = PortWcfCaller.CallToPortUpsert();
            Assert.IsFalse(serviceToResponse.HasError, serviceToResponse.ErrorMessage);
            Assert.IsNotNull(serviceToResponse.Result, "Upsert To Port Failed! " + serviceToResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_PORT_GetList()
        {
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = PortWcfCaller.PrepareFromPort();
            Assert.IsFalse(prepareResponse.HasError, "Prepare From Port Failed! " + prepareResponse.ErrorMessage);
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
                SearchFields = HybridData.FromPortCode
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            PortList[] ports = (PortList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsTrue(CheckResult(ports), "Get Hybrid From Port From Ports Failed!");
        }
        public bool CheckResult(PortList[] ports)
        {
            return ports[0].EnglishName == "Hybrid From Port";
        }
        [TestMethod]
        public void Test_PORT_GetPortId()
        {
            LoginService.GetLoginTokenByCredentials();
            //Response prepareResponse = PortWcfCaller.PrepareToPort();
            //Assert.IsFalse(prepareResponse.HasError, "Prepare To Port Failed! " + prepareResponse.ErrorMessage);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Port",
                ServiceOperation = "GetPortId",
                ServiceResponseIndex = 2,
                ServiceType = typeof(PortList),
                ServiceFilterType = typeof(PortServiceReference.PortApiFilters),
            };
            PortServiceReference.PortApiFilters filters = new PortServiceReference.PortApiFilters
            {
                PortCode = "TLV",
                CountryCode = "IL",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            string port = (string)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Get Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
