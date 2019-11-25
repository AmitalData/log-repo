using System;
using Logitude.HybridTest.ActivityServiceReference;
using Logitude.HybridTest.WcfCallers;
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
            //LoginService.GetLoginTokenByCredentials();
            LoginServiceReference.LoginWcfServiceClient serviceClient = new LoginServiceReference.LoginWcfServiceClient();
            string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
            serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
            using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
            {
                Response loginResponse = serviceClient.Login("angular@fnarsoft.com", "1");
                TestEnvironmentGlobalParameters.Token = loginResponse.Result;
            }
            Response serviceResponse = ActivityWcfCaller.CallActivityUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Activity_GetActivities()
        {
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = ActivityWcfCaller.PrepareActivity();
            Assert.IsFalse(prepareResponse.HasError, "Prepare User Failed! " + prepareResponse.ErrorMessage);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Activity",
                ServiceOperation = "GetActivities",
                ServiceResponseIndex = 2,
                ServiceType = typeof(ActivityPM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "Hybrid@fnarsoft.com", TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            ActivityPM[] activities = (ActivityPM[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            Assert.AreEqual(activities[0].Subject, HybridData.ContactCode, "Get Hybrid Contact From Contacts Failed!");
        }

        [TestMethod]
        public void Test_Activity_UpdateOutlookID()
        {
            Assert.Inconclusive("Not Implemented !");

            Test_Activity_UPSERT();
            if (HybridData.ActivityId != null)
            {
                LoginService.GetLoginTokenByCredentials();
                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Activity",
                    ServiceOperation = "UpdateOutlookID",
                    ServiceResponseIndex = 0,
                    ServiceType = null,
                    ServiceFilterType = null,
                };
                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { HybridData.ActivityId, "Hybrid Update", TestEnvironmentGlobalParameters.Tenant };
                serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Update Outlook ID Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Update Outlook ID Failed! " + serviceResponse.Result);
            }
            else
                Assert.IsTrue(false, "Upsert Activity Failed!");
        }

        [TestMethod]
        public void Test_Activity_SetAsSynchronized()
        {
            Assert.Inconclusive("Not Implemented !");

            Test_Activity_UPSERT();
            if (HybridData.ActivityId != null)
            {
                LoginService.GetLoginTokenByCredentials();
                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Activity",
                    ServiceOperation = "SetAsSynchronized",
                    ServiceResponseIndex = 0,
                    ServiceType = null,
                    ServiceFilterType = null,
                };
                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { HybridData.ActivityId, "Hybrid@fnarsoft.com", TestEnvironmentGlobalParameters.Tenant };
                serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
                Assert.IsFalse(serviceResponse.HasError, "Set As Synchronized Failed! " + serviceResponse.ErrorMessage);
                Assert.IsNull(serviceResponse.Result, "Set As Synchronized Failed! " + serviceResponse.Result);
            }
            else
                Assert.IsTrue(false, "Upsert Activity Failed!");
        }

        //[TestMethod]
        //public void Test_Activity_Delete()
        //{
        //    Assert.Inconclusive("Not Implemented !");
        //    Test_Activity_UPSERT();
        //    ActivityServiceReference.ActivityWcfServiceClient serviceClient = new ActivityServiceReference.ActivityWcfServiceClient();
        //    string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
        //    serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
        //    using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
        //    {
        //        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
        //        Response serviceResponse = serviceClient.Delete("?", TestEnvironmentGlobalParameters.Tenant);
        //        Assert.IsFalse(serviceResponse.HasError, "Delete Failed! " + serviceResponse.ErrorMessage);
        //        Assert.IsNull(serviceResponse.Result, "Delete Failed! " + serviceResponse.ErrorMessage);
        //    }
        //}

        //[TestMethod]
        //public void Test_Activity_isOnline()
        //{
        //    LoginService.GetLoginTokenByCredentials();
        //    ActivityServiceReference.ActivityWcfServiceClient serviceClient = new ActivityServiceReference.ActivityWcfServiceClient();
        //    string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
        //    serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
        //    using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
        //    {
        //        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
        //        Response serviceResponse = serviceClient.isOnline();
        //        Assert.IsFalse(serviceResponse.HasError, "is Online Failed! " + serviceResponse.ErrorMessage);
        //        Assert.IsNull(serviceResponse.Result, "is Online Failed! " + serviceResponse.ErrorMessage);
        //    }
        //}

        //[TestMethod]
        //public void Test_Activity_GetActivityPM()
        //{
        //    Assert.Inconclusive("Not Implemented !");
        //    Test_Activity_UPSERT();
        //    ActivityServiceReference.ActivityWcfServiceClient serviceClient = new ActivityServiceReference.ActivityWcfServiceClient();
        //    string serviceAddress = serviceClient.Endpoint.Address.ToString().Replace("http://localhost:9996", TestEnvironmentGlobalParameters.ServerURL);
        //    serviceClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceAddress);
        //    using (new System.ServiceModel.OperationContextScope(serviceClient.InnerChannel))
        //    {
        //        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
        //        Response serviceResponse = new Response();
        //        ActivityServiceReference.ActivityPM entityPM = serviceClient.GetActivityPM("?", TestEnvironmentGlobalParameters.Tenant, ref serviceResponse);
        //        Assert.IsFalse(serviceResponse.HasError, "Get Activity PM Failed! " + serviceResponse.ErrorMessage);
        //        Assert.IsNull(serviceResponse.Result, "Get Activity PM Failed! " + serviceResponse.ErrorMessage);
        //        if (entityPM != null)
        //        {
        //            string activitySubject = entityPM.Subject;
        //            Assert.AreEqual(activitySubject, "Hybrid Activity", "Hybrid Activity Doesn't Exist!");
        //        }
        //        else
        //        {
        //            Assert.Inconclusive("There Isn't Activity with This Id!");
        //        }
        //    }
        //}

        //[TestMethod]
        //public void Test_Activity_UploadDocumentFileData()
        //{
        //    LoginService.GetLoginTokenByCredentials();
        //    Assert.Inconclusive("Not Implemented !");
        //}

        //[TestMethod]
        //public void Test_Activity_BuidDocument()
        //{
        //    LoginService.GetLoginTokenByCredentials();
        //    Assert.Inconclusive("Not Implemented !");
        //}
    }
}