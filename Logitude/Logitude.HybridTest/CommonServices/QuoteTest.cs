using System;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class QuoteTest
    {
        [TestMethod]
        public void Test_Quote_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = QuoteWcfCaller.CallQuoteUpsert();
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
        [TestMethod]
        public void Test_Quote_GetQuoteList()
        {
            Assert.Inconclusive("Not Implemented !");
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = QuoteWcfCaller.PrepareQuote();
            Assert.IsFalse(prepareResponse.HasError, "Prepare Quote Failed! " + prepareResponse.ErrorMessage);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Quote",
                ServiceOperation = "GetQuoteList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(QuoteList),
                ServiceFilterType = typeof(QuoteServiceReference.QuoteApiFilters),
            };
            QuoteServiceReference.QuoteApiFilters filters = new QuoteServiceReference.QuoteApiFilters
            {
                Take = 10,
                Skip = 0,
                SearchFields = HybridData.QuoteCode
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            QuoteList[] quotes = (QuoteList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            Assert.AreEqual(quotes[0].FromPortId, HybridData.FromPortId, "Get Hybrid Quote Item From Quotes Failed!");
        }

        [TestMethod]
        public void Test_Quote_UploadQuotationDocument()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Quote_CreateEvent()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Quote_BuildEventsList()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Quote_DeleteQuoteEvent()
        {
            LoginService.GetLoginTokenByCredentials();
            Assert.Inconclusive("Not Implemented !");
        }
    }
}
