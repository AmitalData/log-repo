using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class AddressTest
    {
        [TestMethod]
        public void Test_Address_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = AddressWcfCaller.CallAddressUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Address_GetAddressByExternalId()
        {
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = AddressWcfCaller.PrepareAddress();
            Assert.IsFalse(prepareResponse.HasError, "Prepare Address Failed! " + prepareResponse.ErrorMessage);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Address",
                ServiceOperation = "GetAddressByExternalId",
                ServiceResponseIndex = 2,
                ServiceType = typeof(AddressPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.AddressId, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            AddressPM address = (AddressPM)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Address By External Id Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Address By External Id Failed! " + serviceResponse.ErrorMessage);
            Assert.AreEqual(address.Name, "Hybrid Address", "Get Hybrid Address Item From Addresses Failed!");
        }
    }
}