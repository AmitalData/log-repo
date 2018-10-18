using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Data.InvoiceModel.Mocks;

using WebFreight.Web.InvoiceModel.DomainServices;
using Logitude.BL.InvoiceModel.EntityPMs;
using Telerik.JustMock;
using Logitude.Server.Tools.Helpers;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InvoiceModel;
using Simplog.Data.ShipmentsModel;
using System.Linq;
using Simplog.Data.CommonDataModel.Mocks;
using Logitude.Test.Helpers;
using Simplog.Data.ShipmentsModel.Mocks;
using Simplog.Data.InfrastructureModel;
using System;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using WebFreight.Web.InvoiceModel.Tools.Validating;
using WebFreight.Web.InvoiceModel.Tools.TraceEvents;
using WebFreight.Web.Helpers;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.GlobalModelDB;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityQueries;
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
    public class ARPaymentTests
    {
        [TestMethod]
        public void GetSingleARPaymentPMTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ARPaymentPM aRPaymentpm = invoiceDomainService.GetSingleARPayment("1-1", 1);
            Assert.AreEqual("1-1", aRPaymentpm.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleARPaymentPMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ARPaymentPM aRPaymentpm = invoiceDomainService.GetSingleARPayment("1-1", 1);
           
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleARPaymentPMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ARPaymentPM aRPaymentpm = invoiceDomainService.GetSingleARPayment("1-1", 2);
          
        }


        [TestMethod]
        public void CreateARPaymentTest()
        {
           MockInvoiceContext invoiceContext = new MockInvoiceContext();
            MockCommonContext commonContext = new MockCommonContext();

            int tenant = 1;
            ARPaymentService service = new ARPaymentService(invoiceContext, tenant);

            ARPaymentPM newPayment = new ARPaymentPM()
            {
                Id = "1-5",
                
                AmountInProfitCurrency = 1000,
                BranchId = "1-1",
                CreateDate = DateTime.Now.Date,
                ExchangeRateDate = DateTime.Now.Date,
                IsClosed = false,
                LocalCurrencyId = "1-1",
                ProfitCurrencyExchangeRate = 1,
             
                Tenant = tenant,
                UpdateDate = DateTime.Now.Date,
                StatusCode = "DR",
                UpdatedByUserId = "1-1",
           

            };
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");


            Mock.Arrange(() => ARPaymentValidator.Validate(Arg.IsAny<ARPaymentPM>())).DoNothing();
            Mock.Arrange(() => ARPaymentTracing.Trace(Arg.IsAny<ARPaymentPM>(), Arg.IsAny<ARPayment>(), Arg.IsAny<bool>())).DoNothing();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(new MockWebFreightContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());
            Mock.Arrange(() => TableCounter.GetNumber(Arg.IsAny<int>(),Arg.IsAny<string>(),Arg.IsAny<string>(),Arg.IsAny<string>())).Returns("1-5");
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-5");
            
            //Mock.NonPublic.Arrange(service, "BuildSearchField").DoNothing();

            service.Create(newPayment);
            ARPayment payment = invoiceContext.ARPayments.Where(d => d.Id == "1-5").FirstOrDefault();
            Assert.AreNotEqual
                (payment, null,
              "create payment test succeeded");
        }

        [TestMethod]
        public void UpdateARPaymentTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            int tenant = 1;
            MockCommonContext commonContext = new MockCommonContext();
            MockWebFreightContext webFreightContext = new MockWebFreightContext();
            ARPaymentService service = new ARPaymentService(invoiceContext, tenant);
          
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => ARPaymentValidator.Validate(Arg.IsAny<ARPaymentPM>())).DoNothing();
            Mock.Arrange(() => ARPaymentTracing.Trace(Arg.IsAny<ARPaymentPM>(), Arg.IsAny<ARPayment>(), Arg.IsAny<bool>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => TableLastUpdateClass.UpdateTableHistory(Arg.IsAny<int>(), Arg.IsAny<string>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).Returns("user1@fnarsoft.com");
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
            Mock.Arrange(() => EventTracer.CreateTraceEvent(Arg.IsAny<TraceEvent>(), Arg.IsAny<string>(), Arg.IsAny<int>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<bool>())).DoNothing();
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());
            Mock.Arrange(() => IdCounter.GetNumber(Arg.IsAny<string>(), Arg.IsAny<int>())).Returns("1-1");
            Mock.Arrange(() => TableCounter.GetNumber(Arg.IsAny<int>(), Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<string>())).Returns("1-1");


            ARPaymentPM aRPaymentpm = invoiceDomainService.GetSingleARPayment("1-1", 1);


            aRPaymentpm.InternalNotes = "Edited note";
            service.Update(aRPaymentpm, aRPaymentpm.PaymentInvoices);

            ARPayment aRPayment = invoiceContext.ARPayments.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreEqual("Edited note", aRPayment.InternalNotes);

        }


        [TestMethod]
        public void GetARPaymentFiltersTest()
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
            queryoperations.ObjectTableName = "ARPayment";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = true, FieldName = "IsClosed", FieldValue=false, Operator = "Equals" });

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<ARPaymentList> list = invoiceDomainService.GetARPaymentFilters(bytearray, 1);


        }
    }
}
