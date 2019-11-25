using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void Test_Customer_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CustomerWcfCaller.CallCustomerUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Customer_GetCustomerPM()
        {
            Assert.Inconclusive("Not Implemented !");
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = CustomerWcfCaller.PrepareCustomer();
            Assert.IsFalse(prepareResponse.HasError, "Prepare Customer Failed! " + prepareResponse.ErrorMessage);
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
                SearchCode = HybridData.CustomerCode
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            CustomerPM customer = (CustomerPM)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer PM Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Customer PM Failed! " + serviceResponse.Result);
            Assert.AreEqual(customer.Code, HybridData.CustomerCode, "Get Customer PM Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Customer_GetCustomerAddresses()
        {
            Assert.Inconclusive("Not Implemented !");
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = CustomerWcfCaller.PrepareCustomer();
            Assert.IsFalse(prepareResponse.HasError, "Prepare Customer Failed! " + prepareResponse.ErrorMessage);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Customer",
                ServiceOperation = "GetCustomerPM",
                ServiceResponseIndex = 2,
                ServiceType = typeof(AddressPM),
                ServiceFilterType = typeof(CustomerServiceReference.CustomerApiFilters),
            };
            CustomerServiceReference.CustomerApiFilters filters = new CustomerServiceReference.CustomerApiFilters
            {
                ByCode = true,
                SearchCode = HybridData.CustomerCode
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            AddressPM[] addresses = (AddressPM[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer Addresses Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Customer Addresses Failed! " + serviceResponse.Result);
            Assert.AreEqual(addresses[0].CountryCode, HybridData.CountryCode, "Get Customer Addresses Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Customer_GetCustomerContacts()
        {
            Assert.Inconclusive("Not Implemented !");
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = CustomerWcfCaller.PrepareCustomer();
            Assert.IsFalse(prepareResponse.HasError, "Prepare Customer Failed! " + prepareResponse.ErrorMessage);
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
                SearchCode = HybridData.CustomerCode
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            ContactPM[] contacts = (ContactPM[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer Addresses Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Customer Addresses Failed! " + serviceResponse.Result);
            Assert.AreEqual(contacts[0].EnglishName, "Hybrid Contact", "Get Customer Contacts Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Customer_GetCustomerList()
        {
            Assert.Inconclusive("Not Implemented !");
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = CustomerWcfCaller.PrepareCustomer();
            Assert.IsFalse(prepareResponse.HasError, "Prepare Customer Failed! " + prepareResponse.ErrorMessage);
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
                SearchCode = HybridData.CustomerCode
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.CustomerCode, "HybridUser@logitudeworld.com", true, TestEnvironmentGlobalParameters.Tenant, 0, 10, serviceResponse };
            CustomerList[] customers = (CustomerList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Customer List Failed! " + serviceResponse.Result);
            Assert.AreEqual(customers[0].EnglishName, "Hybrid Customer", "Get Custome Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Customer_GetCustomerListByEmail()
        {
            Assert.Inconclusive("Not Implemented !");
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = CustomerWcfCaller.PrepareCustomer();
            Assert.IsFalse(prepareResponse.HasError, "Prepare Customer Failed! " + prepareResponse.ErrorMessage);
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
                SearchCode = HybridData.CustomerCode
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "HybridContact@logitudeworld.com", TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            CustomerList[] customers = (CustomerList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Customer List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Customer List Failed! " + serviceResponse.Result);
            Assert.AreEqual(customers[0].EnglishName, "Hybrid Customer", "Get Customer Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Customer_GetCustomerListById()
        {
            Assert.Inconclusive("Not Implemented !");
            LoginService.GetLoginTokenByCredentials();
        Response prepareResponse = CustomerWcfCaller.PrepareCustomer();
        Assert.IsFalse(prepareResponse.HasError, "Prepare Customer Failed! " + prepareResponse.ErrorMessage);
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
            SearchCode = HybridData.CustomerCode
        };

        Response serviceResponse = new Response();
        object[] serviceParameters = new object[] { HybridData.ContactCode, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
        CustomerList customer = (CustomerList)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
        Assert.IsFalse(serviceResponse.HasError, "Get Customer List By Id Failed! " + serviceResponse.ErrorMessage);
        Assert.IsNull(serviceResponse.Result, "Get Customer List By Id Failed! " + serviceResponse.Result);
        Assert.AreEqual(customer.EnglishName, "Hybrid Customer", "Get Customer By Id Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Customer_GetReadyForActivationCustomer()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Customer_RemoveFromCustomersQueue()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Customer_GetActivationQuestionnaireAnswers()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }
    }
}
