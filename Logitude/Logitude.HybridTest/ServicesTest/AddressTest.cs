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
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_Address_UPSERT()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                AddressPM addressPM = new AddressPM()
                {
                    ExternalId = HybridData.AddressCodeHA,
                    Name = "Hybrid Address",
                    City = "Hybrid City",
                    AddressTypeId = "M",
                    Description = "Main Address",
                    CountryId = HybridData.CountryCodeUS,
                    StateId = HybridData.StateCodeAK,
                    CardId = HybridData.CustomerCodeHCustomer,
                    Tenant = EnvironmentGlobalParams.MainTenant,
                };
                ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(addressPM);
                Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
                HybridData.AddressIdHA = serviceOutcome.Response.Result;
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
            
        }

        [TestMethod]
        public void Test_Address_GetAddressByExternalId()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
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
                //Assert.AreEqual(address.Name, "Hybrid Address", "Get Hybrid Address Item From Addresses Failed!");
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }
    }
}