
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Data.InvoiceModel.Mocks;

using WebFreight.Web.InvoiceModel.DomainServices;
using Logitude.BL.InvoiceModel.EntityPMs;
using Telerik.JustMock;

using WebFreight.Web.Security;

using Simplog.Data.CommonDataModel;
using Simplog.Data.InvoiceModel;
using Simplog.Data.ShipmentsModel;

using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Data.ShipmentsModel.Mocks;
using Logitude.Test.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.Helpers;
using System;
using WebFreight.Web.Helpers;
using WebFreight.Web.GlobalModelDB;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Linq;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;



namespace Logitude.Test.InvoiceModel
{
    [TestClass]
    public class ARInvoiceTypeTests
    {
        [TestMethod]
        public void GetSingleARInvoiceTypePMTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ARInvoiceTypePM aRInvoiceTypepm = invoiceDomainService.GetSingleInvoiceType("IT", 1);
            Assert.AreEqual("IT", aRInvoiceTypepm.Code);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleARInvoiceTypePMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ARInvoiceTypePM aRInvoiceTypepm = invoiceDomainService.GetSingleInvoiceType("IT", 1);
         
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleARInvoiceTypePMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ARInvoiceTypePM aRInvoiceTypepm = invoiceDomainService.GetSingleInvoiceType("IT", 2);
         
        }
        [TestMethod]
        public void GetARInvoiceTypeFiltersTest()
        {

            MockCommonContext commonContext = new MockCommonContext();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
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
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);

            QueryOperations queryoperations = new QueryOperations();
            queryoperations.ObjectTableName = "ARInvoiceType";
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

            IQueryable<ARInvoiceTypeList> list = invoiceDomainService.GetInvoiceTypeFilters(bytearray, 1);


        }
    }
}
