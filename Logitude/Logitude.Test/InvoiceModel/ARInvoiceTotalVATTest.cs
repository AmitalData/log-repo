
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
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System.Linq;
using System;
using WebFreight.Web.Helpers;
using Simplog.Data.InfrastructureModel;
using WebFreight.Web.GlobalModelDB;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.Repositories;
using WebFreight.Web.DataContracts;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Test.InvoiceModel
{
    [TestClass]
    public class ARInvoiceTotalVATTest
    {
       [TestMethod]
        public void GetSingleARInvoiceTotalVATPMTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ARInvoiceTotalVATPM aRInvoiceTotalVATpm = invoiceDomainService.GetSingleInvoiceTotalVAT("1-1", 1);
            Assert.AreEqual("1-1", aRInvoiceTotalVATpm.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
           public void GetSingleARInvoiceTotalVATPMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ARInvoiceTotalVATPM aRInvoiceTotalVATpm = invoiceDomainService.GetSingleInvoiceTotalVAT("1-1", 1);
         
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleARInvoiceTotalVATPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ARInvoiceTotalVATPM aRInvoiceTotalVATpm = invoiceDomainService.GetSingleInvoiceTotalVAT("1-1", 2);
         
        }

        [TestMethod]
        public void CreateARInvoiceTotalVATTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            int tenant = 1;
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();

            ARInvoiceTotalVATPM newARInvoiceTotalVAT = new ARInvoiceTotalVATPM()
            {
                Id = "1-1",
                Tenant = tenant,
                

            };
            

            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-1");
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).Returns("user1@fnarsoft.com");
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);


         
            
          
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);

            invoiceDomainService.InsertInvoiceTotalVAT(newARInvoiceTotalVAT);
            ARInvoiceTotalVAT aRInvoiceTotalVAT = invoiceContext.ARInvoiceTotalVATs.Where(d => d.Id == "1-1").FirstOrDefault();
            Assert.AreNotEqual(aRInvoiceTotalVAT, null);
        }

        [TestMethod]
        public void ARInvoiceTotalVATUpdateTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            MockCommonContext commonContext = new MockCommonContext();
            MockShipmentContext shipmentContext = new MockShipmentContext();
            int tenant = 1;

            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            ARInvoiceTotalVATRepository invoiceRepository = new ARInvoiceTotalVATRepository(invoiceContext);
            ARInvoiceTotalVATQuery invoiceQuery = new ARInvoiceTotalVATQuery(invoiceRepository);
            CacheManager.CacheWrapper = new MockCacheWrapper();
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentContext);
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).Returns("user1@fnarsoft.com");


            ARInvoiceTotalVATPM invoicePM = invoiceDomainService.GetSingleInvoiceTotalVAT("1-1" , tenant);

            invoicePM.LocalVATAmount = 0.3;

            invoiceDomainService.UpdateInvoiceTotalVAT(invoicePM);

            ARInvoiceTotalVAT aRInvoiceTotalVAT = invoiceRepository.GetSingleInvoiceTotalVAT("1-1");

            Assert.AreEqual(aRInvoiceTotalVAT.LocalVATAmount, 0.3);

        }
        [TestMethod]
        public void GetARInvoiceTotalVATFiltersTest()
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
            queryoperations.ObjectTableName = "InvoiceTotalVAT";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = true, FieldName = "VATPercent"});

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<ARInvoiceTotalVATList> list = invoiceDomainService.GetInvoiceTotalVATFilters(bytearray, 1);


        }
    }
}
