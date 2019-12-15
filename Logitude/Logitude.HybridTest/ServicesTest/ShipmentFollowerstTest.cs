using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class ShipmentFollowerstTest
    {
        [TestMethod]
        public void Test_ShipmentFollowerst_GetShipmentFollowersByShipmentNumber()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "ShipmenFollowerst",
                ServiceOperation = "GetShipmentFollowersByShipmentNumber",
                ServiceResponseIndex = 2,
                ServiceType = typeof(ContactList),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "Hybrid Shipment", EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            ContactList[] contacts = (ContactList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Shipment Followers By Shipment Number Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Shipment Followers By Shipment Number Failed! " + serviceOutcome.Response.Result);
            if (contacts.Length == 0)
                Assert.Inconclusive("There Isn't Any Shipment Followers!");
        }
    }
}
