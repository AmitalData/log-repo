using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class PortTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_Port_UPSERT()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                PortPM portPM = new PortPM()
                {
                    Code = HybridData.PortCodeHP,
                    EnglishName = "Hybrid Port",
                    LocalName = "Hybrid Port",
                    CountryCode = HybridData.CountryCodeUS,
                    CountryId = HybridData.CountryCodeUS,
                    AddedManually = true,
                    IsAir = true,
                    IsOcean = true,
                    IsInland = true,
                    Tenant = EnvironmentGlobalParams.MainTenant,
                };
                ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(portPM);
                Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }

        [TestMethod]
        public void Test_PORT_GetList()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Port",
                    ServiceOperation = "GetList",
                    ServiceResponseIndex = 2,
                    ServiceType = typeof(PortList),
                    ServiceFilterType = typeof(ApiSearchFilters)
                };
                ApiSearchFilters filters = new ApiSearchFilters
                {
                    Take = 10,
                    SearchFields = HybridData.PortCodeLON
                };

                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
                ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

                PortList[] ports = (PortList[])serviceOutcome.Result;
                Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.Result);
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }
        [TestMethod]
        public void Test_PORT_GetPortId() //This mehtod get the port from tenant 0
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
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
                object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
                ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

                string port = (string)serviceOutcome.Result;
                Assert.IsFalse(serviceOutcome.Response.HasError, "Get Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNotNull(serviceOutcome.Response.Result, "Get Failed! " + serviceOutcome.Response.ErrorMessage);
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }
    }
}
