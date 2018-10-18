using Logitude.Test.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Global.Data.GlobalModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.Mocks;

using Telerik.JustMock;

using WebFreight.Web.GlobalModelDB;
using WebFreight.Web.QuoteModel.DomainServices;
using Logitude.BL.QuoteModel.EntityPMs;

using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using WebFreight.Web.Helpers;
using System;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Linq;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Test.QuoteModel
{
    [TestClass]
    public class QuoteCustomerTypeTests
    {
        [TestMethod]
        public void GetSingleQuoteCustomerTypePMTest()
        {
            MockQuoteContext qouteContext = new MockQuoteContext();
            QuotesDomainService qouteDomainService = new QuotesDomainService();
            Mock.Arrange(() => QuotesContext.GetContext(Arg.IsAny<int>())).Returns(qouteContext);

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            QuoteCustomerTypePM quoteCustomerTypePM = qouteDomainService.GetSingleQuoteCustomerTypePM("QC", 1);
            Assert.AreEqual("QC", quoteCustomerTypePM.Code);
        }



        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleQuoteCustomerTypePMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("notexisteduser@fnarsoft.com");
            MockQuoteContext qouteContext = new MockQuoteContext();
            QuotesDomainService qouteDomainService = new QuotesDomainService();
            Mock.Arrange(() => QuotesContext.GetContext(Arg.IsAny<int>())).Returns(qouteContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            QuoteCustomerTypePM quoteCustomerTypePM = qouteDomainService.GetSingleQuoteCustomerTypePM("QC", 1);
         
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleQuoteCustomerTypePMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockQuoteContext qouteContext = new MockQuoteContext();
            QuotesDomainService qouteDomainService = new QuotesDomainService();
            Mock.Arrange(() => QuotesContext.GetContext(Arg.IsAny<int>())).Returns(qouteContext);
             Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            QuoteCustomerTypePM quoteCustomerTypePM = qouteDomainService.GetSingleQuoteCustomerTypePM("QC", 2);
         
        }
        [TestMethod]
        public void GetQuoteCustomerTypeFiltersTest()
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
            queryoperations.ObjectTableName = "QuoteCustomerType";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = true, FieldName = "Code", Operator = "Equals" });

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<QuoteCustomerTypeList> list = qouteDomainService.GetQuoteCustomerTypeFilters(bytearray, 1);


        }
      
    }
}
