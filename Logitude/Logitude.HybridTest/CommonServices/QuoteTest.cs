using System;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.HybridTest.WcfFactory;
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
            QuotePM quotePM = QuoteWcfFactory.GetQuotePM();
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(quotePM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
            HybridData.QuoteId = serviceResponse.Result;
        }
        [TestMethod]
        public void Test_Quote_GetQuoteList()
        {
            Assert.Inconclusive("Not Implemented !");
            if (HybridData.QuoteId == null)
                Test_Quote_UPSERT();
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
                SearchFields = HybridData.QuoteCode
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            QuoteList[] quotes = (QuoteList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            Assert.AreEqual(quotes[0].FromPortId, HybridData.PortCodeLON, "Get Hybrid Quote Item From Quotes Failed!");
        }

        [TestMethod]
        public void Test_Quote_UploadQuotationDocument()
        {
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Quote_CreateEvent()
        {
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Quote_BuildEventsList()
        {
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Quote_DeleteQuoteEvent()
        {
            Assert.Inconclusive("Not Implemented !");
        }
    }
}
