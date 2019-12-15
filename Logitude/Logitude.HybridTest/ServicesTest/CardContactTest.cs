using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
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
                CardId = HybridData.AgentCodeHAgent,
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(cardContactPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_CardContact_DELETE()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "CardContact",
                ServiceOperation = "Delete",
                ServiceResponseIndex = 0,
                ServiceType = null,
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.ContactCode, HybridData.AgentCodeHAgent, EnvironmentGlobalParams.MainTenant, false };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Delete Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Delete Failed! " + serviceOutcome.Response.Result);
        }

        [TestMethod]
        public void Test_CardContact_GetCardContactPM()
        {
            Assert.Inconclusive("Check code!");
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "CardContact",
                ServiceOperation = "GetCardContactPM",
                ServiceResponseIndex = 3,
                ServiceType = typeof(CardContactPM),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.ContactCode, HybridData.AgentCodeHAgent, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            CardContactPM cardContact = (CardContactPM)serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Card Contact PM Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Card Contact PM Failed! " + serviceOutcome.Response.Result);
        }
    }
}
