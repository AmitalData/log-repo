using System;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class ActivityTest
    {
        [TestMethod]
        public void Test_Activity_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CallActivityUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Activity_GetActivities()
        {
            ActivityServiceReference.ActivityWcfServiceClient serviceClient = new ActivityServiceReference.ActivityWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                ActivityServiceReference.ActivityPM[] entityList = serviceClient.GetActivities("Hybrid@fnarsoft.com", TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Activities Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Activities Failed! " + serviceResponse.ErrorMessage);
                if (entityList.Length != 0)
                {
                    //
                }
                else
                {
                    Assert.Inconclusive("There Isn't any Activity!");
                }
            }
        }

        public static Response CallActivityUpsert()
        {
            ActivityServiceReference.ActivityWcfServiceClient serviceClient = new ActivityServiceReference.ActivityWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                ActivityServiceReference.ActivityPM entityPM = new ActivityServiceReference.ActivityPM()
                {
                    Subject = "Hybrid Activity",
                    Description = "Hybrid Activity",
                    ActivityTypeCode = "TS", //TS:Task, AP:Appointment, CL:Phone Cell
                    ActivityStatusCode ="N", //N:Not Started
                    Tenant = TestEnvironmentGlobalParameters.Tenant
                };
                Response serviceResponse = serviceClient.Upsert(entityPM, "Hybrid@fnarsoft.com");
                return serviceResponse;
            }
        }

        [TestMethod]
        public void Test_Activity_UpdateOutlookID()
        {
            Assert.Inconclusive("Not Implemented !");
            LoginService.GetLoginTokenByCredentials();
            Test_Activity_UPSERT();
            ActivityServiceReference.ActivityWcfServiceClient serviceClient = new ActivityServiceReference.ActivityWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = serviceClient.UpdateOutlookID("?", "?", TestEnvironmentGlobalParameters.Tenant);
                Assert.IsFalse(serviceResponse.HasError, "Update Outlook ID Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Update Outlook ID Failed! " + serviceResponse.ErrorMessage);
            }
        }

        [TestMethod]
        public void Test_Activity_SetAsSynchronized()
        {
            Assert.Inconclusive("Not Implemented !");
            LoginService.GetLoginTokenByCredentials();
            Test_Activity_UPSERT();
            ActivityServiceReference.ActivityWcfServiceClient serviceClient = new ActivityServiceReference.ActivityWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = serviceClient.SetAsSynchronized("?", "Hybrid@fnarsoft.com", TestEnvironmentGlobalParameters.Tenant);
                Assert.IsFalse(serviceResponse.HasError, "Set As Synchronized Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Set As Synchronized Failed! " + serviceResponse.ErrorMessage);
            }
        }

        [TestMethod]
        public void Test_Activity_Delete()
        {
            Assert.Inconclusive("Not Implemented !");
            Test_Activity_UPSERT();
            ActivityServiceReference.ActivityWcfServiceClient serviceClient = new ActivityServiceReference.ActivityWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = serviceClient.Delete("?", TestEnvironmentGlobalParameters.Tenant);
                Assert.IsFalse(serviceResponse.HasError, "Delete Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Delete Failed! " + serviceResponse.ErrorMessage);
            }
        }

        [TestMethod]
        public void Test_Activity_isOnline()
        {
            LoginService.GetLoginTokenByCredentials();
            ActivityServiceReference.ActivityWcfServiceClient serviceClient = new ActivityServiceReference.ActivityWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = serviceClient.isOnline();
                Assert.IsFalse(serviceResponse.HasError, "is Online Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "is Online Failed! " + serviceResponse.ErrorMessage);
            }
        }

        [TestMethod]
        public void Test_Activity_GetActivityPM()
        {
            Assert.Inconclusive("Not Implemented !");
            Test_Activity_UPSERT();
            ActivityServiceReference.ActivityWcfServiceClient serviceClient = new ActivityServiceReference.ActivityWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                Response serviceResponse = new Response();
                ActivityServiceReference.ActivityPM entityPM = serviceClient.GetActivityPM("?", TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Get Activity PM Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Get Activity PM Failed! " + serviceResponse.ErrorMessage);
                if (entityPM != null)
                {
                    string activitySubject = entityPM.Subject;
                    Assert.AreEqual(activitySubject, "Hybrid Activity", "Hybrid Activity Doesn't Exist!");
                }
                else
                {
                    Assert.Inconclusive("There Isn't Activity with This Id!");
                }
            }
        }

        [TestMethod]
        public void Test_Activity_UploadDocumentFileData()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Activity_BuidDocument()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }
    }
}