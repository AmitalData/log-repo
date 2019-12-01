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
        [TestMethod]
        public void Test_CustomerExport_UPSERT()
        {
            CustomerPM customerPM = CustomerWcfFactory.GetCustomerPM();
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(customerPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
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
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            CustomerPM customer = (CustomerPM)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer PM Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Customer PM Failed! " + serviceResponse.Result);
            Assert.AreEqual(customer.Id, HybridData.CustomerIdHCustomer, "Get Customer PM Failed!");
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
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            AddressPM[] addresses = (AddressPM[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer Addresses Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Customer Addresses Failed! " + serviceResponse.Result);
            if (addresses.Length == 0)
                Assert.Inconclusive("There isn't any address for this customer");
            else
                Assert.AreEqual(addresses[0].AddressTypeId,"M", "Get Customer Addresses Failed! " + serviceResponse.ErrorMessage);
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
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            ContactPM[] contacts = (ContactPM[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer Contacts Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Customer Contacts Failed! " + serviceResponse.Result);
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
            object[] serviceParameters = new object[] { HybridData.CustomerCodeHCustomer, "hybriduser@logitudeworld.com", true, TestEnvironmentGlobalParameters.Tenant1, 0, 10, serviceResponse };
            CustomerList[] customers = (CustomerList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Customer List Failed! " + serviceResponse.Result);
            if (customers.Length == 0)
                Assert.Inconclusive("There isn't any customer with this search field!");
            else
                Assert.AreEqual(customers[0].Id, HybridData.CustomerIdHCustomer, "Get Custome Failed! " + serviceResponse.ErrorMessage);
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
            object[] serviceParameters = new object[] { "HybridContact@logitudeworld.com", TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            CustomerList[] customers = (CustomerList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Customer List Failed! " + serviceResponse.Result);
            Assert.AreEqual(customers[0].EnglishName, "TestShipperExport1", "Get Customer Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Customer_GetCustomerListById()
        {
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
            object[] serviceParameters = new object[] { HybridData.CustomerIdHCustomer, TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            CustomerList customer = (CustomerList)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer List By Id Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Customer List By Id Failed! " + serviceResponse.Result);
            Assert.AreEqual(customer.Id, HybridData.CustomerIdHCustomer, "Get Customer By Id Failed! " + serviceResponse.ErrorMessage);
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

        [TestMethod]
        public void Test_Customer_GetActivationQuestionnaireAnswers()
        {
            Assert.Inconclusive("Not Implemented !");
        }
    }
}
