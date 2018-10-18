using System;
using System.Linq;

using Logitude.Test.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Global.Data.GlobalModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Mocks;
using Simplog.Data.QuoteModel.Repositories;

using Telerik.JustMock;
using Logitude.Server.Tools.Helpers;
using WebFreight.Web.GlobalModelDB;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.QuoteModel.DomainServices;
using Logitude.BL.QuoteModel.EntityPMs;
using WebFreight.Web.Security;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Test.QuoteModel
{
    [TestClass]
    public class QuoteTests
    {
        MockWebFreightContext webFreightContext;

        [TestInitialize]
        public void InitializeAPInvoiceTests()
        {
            webFreightContext = new MockWebFreightContext();
          //  GeneralMocking.FillInfrastructureData(webFreightContext);
        }

        [TestMethod]
        public void GetSingleQuotePMTest()
        {
            MockQuoteContext qouteContext = new MockQuoteContext();
            QuotesDomainService qouteDomainService = new QuotesDomainService();
            Mock.Arrange(() => QuotesContext.GetContext(Arg.IsAny<int>())).Returns(qouteContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
    
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
         
            QuotePM quotePM = qouteDomainService.GetSingleQuotePMById("1-1", 1);
            Assert.AreEqual("1-1", quotePM.Id);
        }



        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleQuotePMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("notexisteduser@fnarsoft.com");
            MockQuoteContext qouteContext = new MockQuoteContext();
            QuotesDomainService qouteDomainService = new QuotesDomainService();
          
          
            Mock.Arrange(() => QuotesContext.GetContext(Arg.IsAny<int>())).Returns(qouteContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            QuotePM quotePM = qouteDomainService.GetSingleQuotePMById("1-1",1);
          
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleQuotePMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockQuoteContext qouteContext = new MockQuoteContext();
            QuotesDomainService qouteDomainService = new QuotesDomainService();
            Mock.Arrange(() => QuotesContext.GetContext(Arg.IsAny<int>())).Returns(qouteContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            QuotePM quotePM = qouteDomainService.GetSingleQuotePMById("1-1", 2);
          
        }

        [TestMethod]
        public void CreateQuoteTest()
        {

            MockQuoteContext qouteContext = new MockQuoteContext();
            QuotesDomainService qouteDomainService = new QuotesDomainService();
            WebFreightDomainService service = new WebFreightDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            int tenant = 1;
            QuoteRepository quoteRepository = new QuoteRepository(qouteContext);
            QuotePM newQuote = new QuotePM()
            {
                Id = "1-5",
                Tenant = tenant,

                ShipmentTypeId = "1-1",

                ToPortId = "111",

                ToPortCountry = "Malysia",
                ToPortName = "Malysia Port",
                FromPortId = "111",


                TransportModeId = "111"

            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");


            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => QuotesContext.GetContext(Arg.IsAny<int>())).Returns(qouteContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-5");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
            Mock.Arrange(() => TableCounter.GetNumber(Arg.IsAny<int>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => EventTracer.CreateTraceEvent(Arg.IsAny<TraceEvent>(), Arg.IsAny<string>(), Arg.IsAny<int>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>(), false)).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).Returns("user1@fnarsoft.com");

            qouteDomainService.InsertQuotePM(newQuote);
            Quote quote = qouteContext.Quotes.Where(d => d.Id == "1-5").FirstOrDefault();
            Assert.AreNotEqual(quote, null);
        }


        [TestMethod]
        public void UpdateQuoteTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockQuoteContext qouteContext = new MockQuoteContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            QuotesDomainService qouteDomainService = new QuotesDomainService();
            MockCommonContext commonContext = new MockCommonContext();
            MockWebFreightContext webFreightContext = new MockWebFreightContext();
            WebFreightDomainService webFreightDomainService = new WebFreightDomainService();

            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => QuotesContext.GetContext(Arg.IsAny<int>())).Returns(qouteContext);
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).Returns("user1@fnarsoft.com");
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
            Mock.Arrange(() => EventTracer.CreateTraceEvent(Arg.IsAny<TraceEvent>(), Arg.IsAny<string>(), Arg.IsAny<int>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<bool>())).DoNothing();

            QuotePM quotePM = qouteDomainService.GetSingleQuotePMById("1-1", 1);


            quotePM.Notes = "Edited Note";
            qouteDomainService.UpdateQuotePM(quotePM);

            Quote quote = qouteContext.Quotes.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreEqual("Edited Note", quote.Notes);

        }

        [TestMethod]
        public void GetQuoteFiltersTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            QuotesDomainService qouteDomainService = new QuotesDomainService();
            MockQuoteContext qouteContext = new MockQuoteContext();
            MockWebFreightContext webContext = new MockWebFreightContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();


            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-5");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => QuotesContext.GetContext(Arg.IsAny<int>())).Returns(qouteContext);
      //      Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentsContext);

            QueryOperations queryoperations = new QueryOperations();
            queryoperations.ObjectTableName = "Quote";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = true, FieldName = "IsByKG", FieldValue = true, Operator = "Equals" });

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<QuoteList> list = qouteDomainService.GetQuoteFilters(bytearray, 1);


        }
    }
}
