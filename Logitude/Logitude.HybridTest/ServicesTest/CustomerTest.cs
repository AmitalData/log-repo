using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.HybridTest.WcfFactory;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class CustomerTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        [TestMethod]
        public void Test_CustomerExport_UPSERT()
        {
            CustomerPM customerPM = CustomerWcfFactory.GetCustomerPM();
            ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(customerPM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Customer_GetCustomerPM()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Customer",
                ServiceOperation = "GetCustomerPM",
                ServiceResponseIndex = 2,
                ServiceType = typeof(CustomerPM),
                ServiceFilterType = typeof(CustomerServiceReference.CustomerApiFilters),
            };
            CustomerServiceReference.CustomerApiFilters filters = new CustomerServiceReference.CustomerApiFilters
            {
                ByCode = true,
                SearchCode = HybridData.CustomerCodeHCustomer
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            CustomerPM customer = (CustomerPM)serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Customer PM Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Customer PM Failed! " + serviceOutcome.Response.Result);
            Assert.AreEqual(customer.Code, HybridData.CustomerCodeHCustomer, "Get Customer PM Failed!");
        }

        [TestMethod]
        public void Test_Customer_GetCustomerAddresses()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Customer",
                ServiceOperation = "GetCustomerAddresses",
                ServiceResponseIndex = 2,
                ServiceType = typeof(AddressPM),
                ServiceFilterType = typeof(CustomerServiceReference.CustomerApiFilters),
            };
            CustomerServiceReference.CustomerApiFilters filters = new CustomerServiceReference.CustomerApiFilters
            {
                ByCode = true,
                SearchCode = HybridData.CustomerCodeHCustomer
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            AddressPM[] addresses = (AddressPM[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Customer Addresses Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Customer Addresses Failed! " + serviceOutcome.Response.Result);
            if (addresses.Length == 0)
                Assert.Inconclusive("There isn't any address for this customer");
            //else
            //    Assert.AreEqual(addresses[0].AddressTypeId, "M", "Get Customer Addresses Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Customer_GetCustomerContacts()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Customer",
                ServiceOperation = "GetCustomerContacts",
                ServiceResponseIndex = 2,
                ServiceType = typeof(ContactPM),
                ServiceFilterType = typeof(CustomerServiceReference.CustomerApiFilters),
            };
            CustomerServiceReference.CustomerApiFilters filters = new CustomerServiceReference.CustomerApiFilters
            {
                ByCode = true,
                SearchCode = HybridData.CustomerCodeHCustomer
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            ContactPM[] contacts = (ContactPM[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Customer Contacts Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Customer Contacts Failed! " + serviceOutcome.Response.Result);
            if (contacts.Length == 0)
                Assert.Inconclusive("There isn't any contact for this customer");
        }

        [TestMethod]
        public void Test_Customer_GetCustomerList()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Customer",
                ServiceOperation = "GetCustomerList",
                ServiceResponseIndex = 6,
                ServiceType = typeof(CustomerList),
                ServiceFilterType = typeof(CustomerServiceReference.CustomerApiFilters),
            };
            CustomerServiceReference.CustomerApiFilters filters = new CustomerServiceReference.CustomerApiFilters
            {
                ByCode = true,
                SearchCode = HybridData.CustomerCodeHCustomer
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.CustomerCodeHCustomer, "hybriduser@logitudeworld.com", true, EnvironmentGlobalParams.MainTenant, 0, 10, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            CustomerList[] customers = (CustomerList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Customer List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Customer List Failed! " + serviceOutcome.Response.Result);
            if (customers.Length == 0)
                Assert.Inconclusive("There isn't any customer with this search field!");
            else
                Assert.AreEqual(customers[0].Id, HybridData.CustomerIdHCustomer, "Get Custome Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Customer_GetCustomerListByEmail()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Customer",
                ServiceOperation = "GetCustomerListByEmail",
                ServiceResponseIndex = 2,
                ServiceType = typeof(CustomerList),
                ServiceFilterType = typeof(CustomerServiceReference.CustomerApiFilters),
            };
            CustomerServiceReference.CustomerApiFilters filters = new CustomerServiceReference.CustomerApiFilters
            {
                ByCode = true,
                SearchCode = HybridData.CustomerCodeHCustomer
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "HybridContact@logitudeworld.com", EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            CustomerList[] customers = (CustomerList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Customer List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Customer List Failed! " + serviceOutcome.Response.Result);
            //Assert.AreEqual(customers[0].EnglishName, "TestShipperExport1", "Get Customer Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Customer_GetCustomerListById()
        {
            Assert.Inconclusive("Check!");
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Customer",
                ServiceOperation = "GetCustomerListById",
                ServiceResponseIndex = 2,
                ServiceType = typeof(CustomerList),
                ServiceFilterType = typeof(CustomerServiceReference.CustomerApiFilters),
            };
            CustomerServiceReference.CustomerApiFilters filters = new CustomerServiceReference.CustomerApiFilters
            {
                ByCode = true,
                SearchCode = HybridData.CustomerCodeHCustomer
            };
            
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.CustomerIdHCustomer, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            CustomerList customer = (CustomerList)serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Customer List By Id Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Customer List By Id Failed! " + serviceOutcome.Response.Result);
            Assert.AreEqual(customer.Id, HybridData.CustomerIdHCustomer, "Get Customer By Id Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Customer_GetReadyForActivationCustomer()
        {
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Customer_RemoveFromCustomersQueue()
        {
            Assert.Inconclusive("Not Implemented !");
        }
    }
}
