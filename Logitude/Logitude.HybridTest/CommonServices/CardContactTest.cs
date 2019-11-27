using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CardContactTest
    {
        [TestMethod]
        public void Test_CardContact_UPSERT()
        {
            CardContactPM cardContactPM = new CardContactPM()
            {
                IsAll = true,
                ContactId = HybridData.ContactCode,
                CardId = HybridData.AgentCode,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(cardContactPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
        [TestMethod]
        public void Test_CardContact_DELETE()
        {
            Test_CardContact_UPSERT();
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "CardContact",
                ServiceOperation = "Delete",
                ServiceResponseIndex = 0,
                ServiceType = null,
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.ContactCode, HybridData.AgentCode, TestEnvironmentGlobalParameters.Tenant, false };
            serviceResponse = (Response)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Delete Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Delete Failed! " + serviceResponse.Result);
        }
        [TestMethod]
        public void Test_CardContact_GetCardContactPM()
        {
            Test_CardContact_UPSERT();
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "CardContact",
                ServiceOperation = "GetCardContactPM",
                ServiceResponseIndex = 3,
                ServiceType = typeof(CardContactPM),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.ContactCode, HybridData.AgentCode, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            CardContactPM cardContact = (CardContactPM)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Card Contact PM Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Card Contact PM Failed! " + serviceResponse.Result);
        }
    }
}
