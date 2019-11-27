using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class IncotermTest
    {
        [TestMethod]
        public void Test_Incoterm_UPSERT()
        {
            IncotermPM incotermPM = new IncotermPM()
            {
                Code = HybridData.IncotermCode,
                Name = "Hybrid Incoterm",
                LocalName = "Hybrid Incoterm",
                Freight = "C",
                OtherCharges = "C",
                Tenant = TestEnvironmentGlobalParameters.Tenant
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(incotermPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Incoterm_GetIncoterms()
        {
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = IncotermWcfCaller.PrepareIncoterm();
            Assert.IsFalse(prepareResponse.HasError, "Prepare User Failed! " + prepareResponse.ErrorMessage);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Incoterm",
                ServiceOperation = "GetIncoterms",
                ServiceResponseIndex = 0,
                ServiceType = typeof(IncotermList),
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { serviceResponse };
            IncotermList[] incoterms = (IncotermList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Incoterms Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Incoterms Failed! " + serviceResponse.Result);
            if(incoterms.Length == 0)
                Assert.Inconclusive("There Isn't Any Incoterm!");
        }
    }
}