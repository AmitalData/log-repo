using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class ContactTest
    {
        [TestMethod]
        public void Test_Contact_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CallContactUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
         }

        public static Response CallContactUpsert()
        {
            ContactServiceReference.ContactWcfServiceClient serviceClient = new ContactServiceReference.ContactWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                ContactServiceReference.ContactPM entityPM = new ContactServiceReference.ContactPM()
                {
                    EnglishName = "Hybrid Contact",
                    LocalName = "Hybrid Contact",
                    Email = "HybridContact@logitudeworld.com",
                    Password = "!H0",
                    ExternalId = HybridCodes.ContactCode,
                    Tenant = TestEnvironmentGlobalParameters.Tenant,
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, false);
                return serviceResponse;
            }
        }

        [TestMethod]
        public void Test_Contact_GetContactPMByEmail()
        {
            Test_Contact_UPSERT();
            ContactServiceReference.ContactWcfServiceClient serviceClient = new ContactServiceReference.ContactWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                ContactServiceReference.ContactPM entityPM = serviceClient.GetContactPMByEmail("HybridContact@logitudeworld.com", TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Contact PM By Email Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Contact PM By Email Failed! " + serviceResponse.ErrorMessage);
                if (entityPM != null)
                {
                    Assert.AreEqual(entityPM.EnglishName, "Hybrid Contact", "Get Contact PM By Email Failed! " + serviceResponse.ErrorMessage);
                }
                else
                {
                    Assert.Inconclusive("There Isn't Contact With This Email!");
                }
            }
        }

        [TestMethod]
        public void Test_Contact_GetContactList()
        {
            Test_Contact_UPSERT();
            ContactServiceReference.ContactWcfServiceClient serviceClient = new ContactServiceReference.ContactWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                ContactServiceReference.ContactApiFilters filters = new ContactServiceReference.ContactApiFilters
                {
                    Take = 10,
                    ByCode = true,
                    SearchFields = HybridCodes.ContactCode
                };
                ContactServiceReference.ContactList[] serviceResult = serviceClient.GetContactList(filters, TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Contact PM By Email Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Contact PM By Email Failed! " + serviceResponse.ErrorMessage);
                if (serviceResult.Length != 0)
                {
                    Assert.AreEqual(serviceResult[0].EnglishName, "Hybrid Contact", "Get Contact PM By Email Failed! " + serviceResponse.ErrorMessage);
                }
                else
                {
                    Assert.Inconclusive("There Isn't Contact With This Code!");
                }
            }
        }

        [TestMethod]
        public void Test_Contact_GetContactByExternalId()
        {
            Test_Contact_UPSERT();
            ContactServiceReference.ContactWcfServiceClient serviceClient = new ContactServiceReference.ContactWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                ContactServiceReference.ContactPM entityPM = serviceClient.GetContactByExternalId("?", TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Contact By External Id Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Contact By External Id Failed! " + serviceResponse.ErrorMessage);
                if (entityPM != null)
                {
                    Assert.AreEqual(entityPM.EnglishName, "Hybrid Contact", "Get Contact By External Id Failed! " + serviceResponse.ErrorMessage);
                }
                else
                {
                    Assert.Inconclusive("There Isn't Contact With This External ID!");
                }
            }
        }
        
    }


    //public class InvokedParameters
    //{
    //    public string ServiceName { get; set; }
    //    public string IServiceName { get; set; }
    //    public string ServiceOperation { get; set; }
    //}
}
