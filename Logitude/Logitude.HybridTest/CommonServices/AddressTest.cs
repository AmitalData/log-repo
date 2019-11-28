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
            AddressPM addressPM = new AddressPM()
            {
                ExternalId = HybridData.AddressCode,
                Name = "Hybrid Address",
                City = "Hybrid City",
                AddressTypeId = "M",
                Description = "Main Address",
                CountryId = HybridData.CountryCode,
                CardId = HybridData.CustomerCode,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(addressPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
            HybridData.AddressId = serviceResponse.Result;
        }
        [TestMethod]
        public void Test_Address_GetAddressByExternalId()
        {
            Test_Address_UPSERT();
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