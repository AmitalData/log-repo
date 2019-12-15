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
        [TestMethod]
        public void Test_Port_UPSERT()
        {
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
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(portPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_PORT_GetList()
        {
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
            Assert.AreEqual(ports[0].Id, HybridData.PortIdLON, "Get Hybrid From Port From Ports Failed!");
        }
        [TestMethod]
        public void Test_PORT_GetPortId() //This mehtod get the port from tenant 0
        {
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
    }
}
