using System;
using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.HybridTest.WcfFactory;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class QuoteTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        [TestMethod]
        public void Test_Quote_UPSERT()
        {
            QuotePM quotePM = QuoteWcfFactory.GetQuotePM();
            ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(quotePM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Quote_GetQuoteList()
        {
            Assert.Inconclusive("Search Field Problem!");
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
            object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            QuoteList[] quotes = (QuoteList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.Result);
            Assert.AreEqual(quotes[0].FromPortId, HybridData.PortIdLON, "Get Hybrid Quote Item From Quotes Failed!");
        }

        [TestMethod]
        public void Test_Quote_UploadQuotationDocument()
        {
            Assert.Inconclusive("Not Implemented !");
        }

        [TestMethod]
        public void Test_Quote_CreateEvent()
        {
            QuotePM quotePM = QuoteWcfFactory.GetQuotePM();
            ServiceOutcome upsertOutcome = entityWcfCaller.CallEntityUpsert(quotePM);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Quote",
                ServiceOperation = "CreateEvent",
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, null, quotePM.QuoteNumber, HybridData.UserCodeHU, "QTCP", DateTime.Now, DateTime.Now, "Testing hybrid accepted" };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Create Event Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Create Event List Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Quote_BuildEventsList()
        {
            QuotePM quotePM = QuoteWcfFactory.GetQuotePM();
            ServiceOutcome upsertOutcome = entityWcfCaller.CallEntityUpsert(quotePM);
            List<TraceEventPM> events = new List<TraceEventPM>()
            {
               // new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = null, UserId = HybridData.UserCodeHU, EventTypeCode = "UPQT", EventDateTime = DateTime.Now.AddDays(-2), LogDateTime = DateTime.Now.AddDays(-2), Notes = "Testing hybrid updated" },
                new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = null, UserId = HybridData.UserCodeHU, EventTypeCode = "QTCP", EventDateTime = DateTime.Now, LogDateTime = DateTime.Now, Notes = "Testing hybrid accepted" },
                new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = null, UserId = HybridData.UserCodeHU, EventTypeCode = "RQTD", EventDateTime = DateTime.Now, LogDateTime = DateTime.Now, Notes = "Testing hybrid returned to draft" },
            };
            Quote_BuildEventsList(quotePM.QuoteNumber, events);
        }

        [TestMethod]
        public void Test_Quote_DeleteQuoteEvent()
        {
            QuotePM quotePM = QuoteWcfFactory.GetQuotePM();
            ServiceOutcome upsertOutcome = entityWcfCaller.CallEntityUpsert(quotePM);
            string acceptedExternalId = Guid.NewGuid().ToString();
            List<TraceEventPM> events = new List<TraceEventPM>()
            {
                new TraceEventPM() { Tenant = EnvironmentGlobalParams.MainTenant, ExternalId = acceptedExternalId, UserId = HybridData.UserCodeHU, EventTypeCode = "QTCP", EventDateTime = DateTime.Now, LogDateTime = DateTime.Now, Notes = "Testing hybrid departed" },
            };
            Quote_BuildEventsList(quotePM.QuoteNumber, events);

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Quote",
                ServiceOperation = "DeleteQuoteEvent",
            };
            
            object[] serviceParameters = new object[] { quotePM.QuoteNumber, acceptedExternalId, EnvironmentGlobalParams.MainTenant };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Delete Event Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Delete Event Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Quote_Packages()
        {
            //QuotePM quotePM = QuoteWcfFactory.GetQuotePM();
            //QuotePackagePM quotePackage = new QuotePackagePM()
            //{
            //    PackageTypeId = "20BU",
            //    Quantity = 1,
            //    Length = 10,
            //    Width = 10,
            //    Height = 10,
            //};

            //quotePM.QuotePackages.Add(quotePackage);

            //Response upsertResponse = EntityWcfCaller.CallEntityUpsert(quotePM);
            //RestAPIService restAPIService = new RestAPIService();
            //QuotePM quote = restAPIService.GetEntityPMById<QuotePM>("Quotes", upsertResponse.Result);
            //Assert.AreEqual(quote.QuotePackages.Count, 1, "Add Quote Package Failed!");

            //quotePM.QuotePackages.Remove(quotePackage);
            //upsertResponse = EntityWcfCaller.CallEntityUpsert(quotePM);
            //quote = restAPIService.GetEntityPMById<QuotePM>("Quotes", upsertResponse.Result);
            //Assert.AreEqual(quote.QuotePackages.Count, 0, "Remove Quote Package Failed!");
        }

        private static void Quote_BuildEventsList(string quoteNumber, List<TraceEventPM> events)
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Quote",
                ServiceOperation = "BuildEventsList",
                ServiceType = typeof(TraceEventPM),
            };
            
            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, quoteNumber, events.ToArray() };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Build Events List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Build Events List Failed! " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
