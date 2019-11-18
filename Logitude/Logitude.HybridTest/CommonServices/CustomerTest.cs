using System;
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
            Response countryServiceResponse = CountryTest.CallCountryUpsert();
            Assert.IsFalse(countryServiceResponse.HasError, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Assert.IsNotNull(countryServiceResponse.Result, "Country Upsert Failed! " + countryServiceResponse.ErrorMessage);
            Response serviceResponse = CallCustomerUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Response CallCustomerUpsert()
        {
            CustomerServiceReference.CustomerWcfServiceClient serviceClient = new CustomerServiceReference.CustomerWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                CustomerServiceReference.CustomerPM entityPM = new CustomerServiceReference.CustomerPM()
                {
                    Code = HybridCodes.CustomerCode,
                    EnglishName = "Hybrid Customer",
                    LocalName = "Hybrid Customer",
                    CityName = "Hybrid City",
                    CountryCode = HybridCodes.CountryCode,
                    PartnerTypeId = "CS",
                    IsCustomer = true,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }

        [TestMethod]
        public void Test_Customer_GetCustomerPM()
        {
            Test_Customer_UPSERT();
            CustomerServiceReference.CustomerWcfServiceClient serviceClient = new CustomerServiceReference.CustomerWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                CustomerServiceReference.CustomerApiFilters filters = new CustomerServiceReference.CustomerApiFilters
                {
                    ByCode = true,
                    SearchCode = HybridCodes.CustomerCode
                };
                CustomerServiceReference.CustomerPM entityPM = serviceClient.GetCustomerPM(filters, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Customer PM Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Customer PM Failed! " + serviceResponse.ErrorMessage);
                if (entityPM != null)
                {
                    Assert.AreEqual(entityPM.Code, HybridCodes.CustomerCode, "Get Customer PM Failed! " + serviceResponse.ErrorMessage);
                }
                else
                {
                    Assert.Inconclusive("There Isn't Customer With This Code!");
                }
            }
        }

        [TestMethod]
        public void Test_Customer_GetCustomerAddresses()
        {
            Test_Customer_UPSERT();
            CustomerServiceReference.CustomerWcfServiceClient serviceClient = new CustomerServiceReference.CustomerWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                CustomerServiceReference.CustomerApiFilters filters = new CustomerServiceReference.CustomerApiFilters
                {
                    ByCode = true,
                    SearchCode = HybridCodes.CustomerCode
                };
                CustomerServiceReference.AddressPM[] entityList = serviceClient.GetCustomerAddresses(filters, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Customer Addresses Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Customer Addresses Failed! " + serviceResponse.ErrorMessage);
                if (entityList.Length != 0)
                {
                    Assert.AreEqual(entityList[0].CountryCode, HybridCodes.CountryCode, "Get Customer Addresses Failed! " + serviceResponse.ErrorMessage);
                }
                else
                {
                    Assert.Inconclusive("There Isn't Any Address For This Coustmer!");
                }
            }
        }

        [TestMethod]
        public void Test_Customer_GetCustomerContacts()
        {
            Test_Customer_UPSERT();
            CustomerServiceReference.CustomerWcfServiceClient serviceClient = new CustomerServiceReference.CustomerWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                CustomerServiceReference.CustomerApiFilters filters = new CustomerServiceReference.CustomerApiFilters
                {
                    ByCode = true,
                    SearchCode = HybridCodes.CustomerCode
                };
                CustomerServiceReference.ContactPM[] entityList = serviceClient.GetCustomerContacts(filters, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Customer Contacts Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Customer Contacts Failed! " + serviceResponse.ErrorMessage);
                if (entityList.Length != 0)
                {
                    Assert.AreEqual(entityList[0].EnglishName, "Hybrid Contact", "Get Customer Contacts Failed! " + serviceResponse.ErrorMessage);
                }
                else
                {
                    Assert.Inconclusive("There Isn't Any Contact For This Coustmer!");
                }
            }
        }

        [TestMethod]
        public void Test_Customer_GetCustomerList()
        {
            Test_Customer_UPSERT();
            CustomerServiceReference.CustomerWcfServiceClient serviceClient = new CustomerServiceReference.CustomerWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                CustomerServiceReference.CustomerApiFilters filters = new CustomerServiceReference.CustomerApiFilters
                {
                    ByCode = true,
                    SearchCode = HybridCodes.CustomerCode
                };
                CustomerServiceReference.CustomerList[] entityList = serviceClient.GetCustomerList(HybridCodes.CustomerCode, "HybridUser@logitudeworld.com", true, TestEnvironmentGlobalParameters.Tenant, 0, 10, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Customer List Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Customer List Failed! " + serviceResponse.ErrorMessage);
                if (entityList.Length != 0)
                {
                     Assert.AreEqual(entityList[0].EnglishName, "Hybrid Customer", "Get Customer  Failed! " + serviceResponse.ErrorMessage);
                }
                else
                {
                    Assert.Inconclusive("There Isn't Any Customer!");
                }
            }
        }

        [TestMethod]
        public void Test_Customer_GetCustomerListByEmail()
        {
            Test_Customer_UPSERT();
            CustomerServiceReference.CustomerWcfServiceClient serviceClient = new CustomerServiceReference.CustomerWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                CustomerServiceReference.CustomerApiFilters filters = new CustomerServiceReference.CustomerApiFilters
                {
                    ByCode = true,
                    SearchCode = HybridCodes.CustomerCode
                };
                CustomerServiceReference.CustomerList[] entityList = serviceClient.GetCustomerListByEmail("HybridContact@logitudeworld.com", TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Customer List By Email Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Customer Contacts Failed! " + serviceResponse.ErrorMessage);
                if (entityList.Length != 0)
                {
                    Assert.AreEqual(entityList[0].EnglishName, "Hybrid Customer", "Get Customer List By Email Failed! " + serviceResponse.ErrorMessage);
                }
                else
                {
                    Assert.Inconclusive("There Isn't Any Coustmer!");
                }
            }
        }

        [TestMethod]
        public void Test_Customer_GetCustomerListById()
        {
            Test_Customer_UPSERT();
            CustomerServiceReference.CustomerWcfServiceClient serviceClient = new CustomerServiceReference.CustomerWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                CustomerServiceReference.CustomerApiFilters filters = new CustomerServiceReference.CustomerApiFilters
                {
                    ByCode = true,
                    SearchCode = HybridCodes.CustomerCode
                };
                CustomerServiceReference.CustomerList entityList = serviceClient.GetCustomerListById(HybridCodes.ContactCode, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Customer List By Id Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Customer List By Id Failed! " + serviceResponse.ErrorMessage);
                if (entityList != null)
                {
                    Assert.AreEqual(entityList.EnglishName, "Hybrid Customer", "Get Customer List By Id Failed! " + serviceResponse.ErrorMessage);
                }
                else
                {
                    Assert.Inconclusive("There Isn't Coustomer With This Id!");
                }
            }
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
