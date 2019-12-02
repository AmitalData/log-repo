using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class AddressTest
    {
        [TestMethod]
        public void Test_Address_UPSERT()
        {
            AddressPM addressPM = new AddressPM()
            {
                ExternalId = HybridData.AddressCodeHA,
                Name = "Hybrid Address",
                City = "Hybrid City",
                AddressTypeId = "M",
                Description = "Main Address",
                CountryId = HybridData.CountryCodeHC,
                CardId = HybridData.CustomerCodeHCustomer,
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(addressPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
            HybridData.AddressIdHA = serviceResponse.Result;
        }

        [TestMethod]
        public void Test_Address_GetAddressByExternalId()
        {
            if(HybridData.AddressIdHA == null)
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
            object[] serviceParameters = new object[] { HybridData.AddressIdHA, TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            AddressPM address = (AddressPM)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Address By External Id Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Address By External Id Failed! " + serviceResponse.ErrorMessage);
            Assert.AreEqual(address.Name, "Hybrid Address", "Get Hybrid Address Item From Addresses Failed!");
        }
    }
}