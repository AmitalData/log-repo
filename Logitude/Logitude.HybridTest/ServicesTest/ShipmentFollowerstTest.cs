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
            //InvokedProperties serviceProperties = new InvokedProperties
            //{
            //    ServiceName = "ShipmenFollowerst",
            //    ServiceOperation = "GetShipmentFollowersByShipmentNumber",
            //    ServiceResponseIndex = 2,
            //    ServiceType = typeof(ContactList),
            //    ServiceFilterType = null,
            //};

            //Response serviceResponse = new Response();
            //object[] serviceParameters = new object[] { "Hybrid Shipment", EnvironmentGlobalParams.MainTenant, serviceResponse };
            //ContactList[] contacts = (ContactList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            //Assert.IsFalse(serviceResponse.HasError, "Get Shipment Followers By Shipment Number Failed! " + serviceResponse.ErrorMessage);
            //Assert.IsNull(serviceResponse.Result, "Get Shipment Followers By Shipment Number Failed! " + serviceResponse.Result);
            //if (contacts.Length == 0)
            //    Assert.Inconclusive("There Isn't Any Shipment Followers!");
        }
    }
}
