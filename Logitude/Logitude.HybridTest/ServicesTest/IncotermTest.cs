using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class IncotermTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        [TestMethod]
        public void Test_Incoterm_UPSERT()
        {
            IncotermPM incotermPM = new IncotermPM()
            {
                Code = HybridData.IncotermCodeHI,
                Name = "Hybrid Incoterm",
                LocalName = "Hybrid Incoterm",
                Freight = "C",
                OtherCharges = "C",
                Tenant = EnvironmentGlobalParams.MainTenant
            };
            ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(incotermPM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Incoterm_GetIncoterms()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Incoterm",
                ServiceOperation = "GetIncoterms",
                ServiceResponseIndex = 0,
                ServiceType = typeof(IncotermList),
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            IncotermList[] incoterms = (IncotermList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Incoterms Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Incoterms Failed! " + serviceOutcome.Response.Result);
            if (incoterms.Length == 0)
                Assert.Inconclusive("There Isn't Any Incoterm!");
        }
    }
}