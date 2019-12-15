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
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(addressPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
            HybridData.AddressIdHA = serviceResponse.Result;
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
            object[] serviceParameters = new object[] { HybridData.AddressIdHA, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            AddressPM address = (AddressPM)serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Address By External Id Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Address By External Id Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.AreEqual(address.Name, "Hybrid Address", "Get Hybrid Address Item From Addresses Failed!");
        }
    }
}