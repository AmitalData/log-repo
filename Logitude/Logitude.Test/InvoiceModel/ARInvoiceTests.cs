
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Global.Data.GlobalModel;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Mocks;

using WebFreight.Web.GlobalModelDB;
using WebFreight.Web.Helpers;
using WebFreight.Web.InvoiceModel.DomainServices;
using Logitude.BL.InvoiceModel.EntityPMs;
using Telerik.JustMock;

using Logitude.BL.InvoiceModel.Tools.EntityService;
using WebFreight.Web.InvoiceModel.Tools.TraceEvents;
using WebFreight.Web.InvoiceModel.Tools.Validating;
using WebFreight.Web.Security;

using Simplog.Data.CommonDataModel;
using Simplog.Data.InvoiceModel;
using Simplog.Data.ShipmentsModel;

using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Data.ShipmentsModel.Mocks;
using Logitude.Test.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.DataContracts;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;


namespace Logitude.Test.InvoiceModel
{
    [TestClass]
    public class ARInvoiceTests
    {
        MockWebFreightContext webFreightContext;
        [TestInitialize]
        public void ARInvoiceInitializeTest()
        {
            webFreightContext = new MockWebFreightContext();
           // GeneralMocking.FillInfrastructureData(webFreightContext);
        }

        [TestMethod]
        public void GetSingleARInvoicePMTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());

            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ARInvoicePM invoicepm = invoiceDomainService.GetSingleInvoice("1-1", 1);
            Assert.AreEqual("1-1", invoicepm.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleARInvoicePMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ARInvoicePM invoicepm = invoiceDomainService.GetSingleInvoice("1-1", 1);
         
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleARInvoicePMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            ARInvoicePM invoicepm = invoiceDomainService.GetSingleInvoice("1-1", 2);
         
        }

        [TestMethod]
        public void CreateARInvoiceTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            MockCommonContext commonContext = new MockCommonContext();
            
            int tenant = 1;
            ARInvoiceService service = new ARInvoiceService(invoiceContext, tenant);
            
            ARInvoicePM newInvoice = new ARInvoicePM()
            {
                Id = "1-1",
                AmountDue = 0,
                AmountDueInLocalCurrency = 0,
                AmountDueInProfitCurrency = 0,
                AmountInLocalCurrency = 1000,
                AmountInInvoiceCurrency = 1000,
                AmountInProfitCurrency = 1000,
                BranchId = "1-1",
                CreateDate = DateTime.Now.Date,
                CreatedByUserId = "1-1",
                DueDate = DateTime.Now.Date.AddDays(30),
                ExchangeRateDate = DateTime.Now.Date,
                DraftNumber = "1000",
                InvoiceCurrencyId = "1-1",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,
                 
                InvoiceNumber = "1000",
                IsClosed = false,
                LocalCurrencyId = "1-1",
                MainEntityId = "1-1",
                ObjectTableId = "1-1",
                PaymentTermId = "1-1",
                ProfitCurrencyExchangeRate = 1,
                ProfitCurrencyId = "1-1",
                Tenant = tenant,
                UpdateDate = DateTime.Now.Date,
                StatusCode = "DR",
                UpdatedByUserId = "1-1",
                

            };
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");


            Mock.Arrange(() => ARInvoiceValidator.Validate(Arg.IsAny<ARInvoicePM>())).DoNothing();
            Mock.Arrange(() => ARInvoiceTracing.Trace(Arg.IsAny<ARInvoicePM>(), Arg.IsAny<ARInvoice>(), Arg.IsAny<bool>(), null)).DoNothing();

            Mock.NonPublic.Arrange(service, "GetAllReceivables", new object[] { new List<ARInvoiceLinePM>() }).DoNothing();
            Mock.NonPublic.Arrange(service, "UpdateInvoiceEntities").DoNothing();
            Mock.NonPublic.Arrange(service, "UpdateSearchField").DoNothing();
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());

            service.Create(newInvoice);
            ARInvoice invoice = invoiceContext.ARInvoices.Where(d => d.Id == "1-1").FirstOrDefault();
            Assert.AreNotEqual
                (invoice, null,
              "create invoice test succeeded");
        }

        [TestMethod]
        public void CreateARInvoiceWithInvoiceLinesAndTotalVatsAndInvoiceEntitiesTest()
        {
            int tenant = 1;
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            MockCommonContext commonContext = new MockCommonContext();
            ARInvoiceService service = new ARInvoiceService(invoiceContext, tenant);
            ARInvoicePM newInvoice = new ARInvoicePM()
            {
                Id = "1-1",
                AmountDue = 0,
                AmountDueInLocalCurrency = 0,
                AmountDueInProfitCurrency = 0,
                AmountInLocalCurrency = 1000,
                AmountInInvoiceCurrency = 1000,
                AmountInProfitCurrency = 1000,
                BranchId = "1-1",
                CreateDate = DateTime.Now.Date,
                CreatedByUserId = "1-1",
                DueDate = DateTime.Now.Date.AddDays(30),
                ExchangeRateDate = DateTime.Now.Date,
                DraftNumber = "1000",
                InvoiceCurrencyId = "1-1",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,

                InvoiceNumber = "1000",
                IsClosed = false,
                LocalCurrencyId = "1-1",
                MainEntityId = "1-1",
                ObjectTableId = "1-1",
                PaymentTermId = "1-1",
                ProfitCurrencyExchangeRate = 1,
                ProfitCurrencyId = "1-1",
                Tenant = tenant,
                UpdateDate = DateTime.Now.Date,
                StatusCode = "DR",
                UpdatedByUserId = "1-1",


            };

            newInvoice.InvoiceLines = new List<ARInvoiceLinePM>()
            {
                new ARInvoiceLinePM()
                {
                    ARInvoiceId = "1-1",
                    InvoiceCurrencyAmount = 1000,
                    LocalCurrencyAmount = 1000,
                    ProfitCurrencyAmount = 1000,
                    ForiegnCurrencyAmount = 1000,
                    LineNumber = 1,
                    Tenant = tenant,
                    Id = "1-1",
                    EntityId = "1-1",
                    EntityReference = "1000",
                    ForiegnCurrencyId = "1-1",
                    PrepaidCollectId = "P",
                    ChargesTypeId = "1-1",
                    VatTypeId = "1-1",
                    MeasurementId = "1-1",
                    UnitPrice = 10,
                    Quantity = 100,
                    ReceivableId = "1-1",
                    ForiegnExchangeRate = 1,
                    VatPercentage = 10,

                },
            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");


            Mock.Arrange(() => ARInvoiceValidator.Validate(Arg.IsAny<ARInvoicePM>())).DoNothing();
            Mock.Arrange(() => ARInvoiceTracing.Trace(Arg.IsAny<ARInvoicePM>(), Arg.IsAny<ARInvoice>(), Arg.IsAny<bool>(),  null)).DoNothing();

            
           
            Mock.NonPublic.Arrange(service, "UpdateSearchField").DoNothing();
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
            Mock.Arrange(() => IdCounter.GetNumber("ARInvoiceTotalVAT", Arg.IsAny<int>())).Returns("1-1");
            Mock.Arrange(() => IdCounter.GetNumber("ARInvoiceEntity", Arg.IsAny<int>())).Returns("1-1");
            Mock.Arrange(() => IdCounter.GetNumber("ARInvoiceLine", Arg.IsAny<int>())).Returns("1-1");

            service.Create(newInvoice);
            ARInvoice invoice = invoiceContext.ARInvoices.Where(d => d.Id == "1-1").FirstOrDefault();
            ARInvoiceLine invoiceLine = invoiceContext.ARInvoiceLines.Where(d => d.ARInvoiceId == "1-1"&&d.LineNumber==1).FirstOrDefault();
            ARInvoiceTotalVAT invoiceTotalVat = invoiceContext.ARInvoiceTotalVATs.Where(d => d.Id == "1-1").FirstOrDefault();
            ARInvoiceEntity invoiceEntity = invoiceContext.ARInvoiceEntities.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreNotEqual(invoice, null);
            Assert.AreNotEqual(invoiceLine, null);
            Assert.AreNotEqual(invoiceTotalVat, null);
            Assert.AreNotEqual(invoiceEntity, null);
        }

        //[TestMethod]
        //public void ARInvoiceVatAmountOnCreateTest()
        //{
        //    int tenant = 1;
        //    MockInvoiceContext invoiceContext = new MockInvoiceContext();
        //    MockCommonContext commonContext = new MockCommonContext();
        //    ARInvoiceService service = new ARInvoiceService(invoiceContext, tenant);
        //    ARInvoicePM newInvoice = new ARInvoicePM()
        //    {
        //        Id = "1-1",
        //        AmountDue = 0,
        //        AmountDueInLocalCurrency = 0,
        //        AmountDueInProfitCurrency = 0,
        //        AmountInLocalCurrency = 1000,
        //        AmountInInvoiceCurrency = 1000,
        //        AmountInProfitCurrency = 1000,
        //        BranchId = "1-1",
        //        CreateDate = DateTime.Now.Date,
        //        CreatedByUserId = "1-1",
        //        DueDate = DateTime.Now.Date.AddDays(30),
        //        ExchangeRateDate = DateTime.Now.Date,
        //        DraftNumber = "1000",
        //        InvoiceCurrencyId = "1-1",
        //        InvoiceCurrencyExchangeRate = 1,
        //        InvoiceDate = DateTime.Now.Date,

        //        InvoiceNumber = "1000",
        //        IsClosed = false,
        //        LocalCurrencyId = "1-1",
        //        MainEntityId = "1-1",
        //        ObjectTableId = "1-1",
        //        PaymentTermId = "1-1",
        //        ProfitCurrencyExchangeRate = 1,
        //        ProfitCurrencyId = "1-1",
        //        Tenant = tenant,
        //        UpdateDate = DateTime.Now.Date,
        //        StatusCode = "DR",
        //        UpdatedByUserId = "1-1",


        //    };

        //    newInvoice.InvoiceLines = new List<ARInvoiceLinePM>()
        //    {
        //        new ARInvoiceLinePM()
        //        {
        //            ARInvoiceId = "1-1",
        //            InvoiceCurrencyAmount = 1000,
        //            LocalCurrencyAmount = 1000,
        //            ProfitCurrencyAmount = 1000,
        //            ForiegnCurrencyAmount = 1000,
        //            LineNumber = 1,
        //            ObjectTableId = "1-1",
        //            Tenant = tenant,
        //            Id = "1-1",
        //            EntityId = "1-1",
        //            EntityReference = "1000",
        //            ForiegnCurrencyId = "1-1",
        //            PrepaidCollectId = "P",
        //            ChargesTypeId = "1-1",
        //            VatTypeId = "1-1",
        //            MeasurementId = "1-1",
        //            UnitPrice = 10,
        //            Quantity = 100,
        //            ReceivableId = "1-1",
        //            ForiegnExchangeRate = 1,
        //            VatPercentage = 10,

        //        },
        //    };

        //    GeneralMocking.MockHttpContext("user1@fnarsoft.com");
       //   Mock.Arrange(() => ARInvoiceValidator.Validate(Arg.IsAny<ARInvoicePM>())).DoNothing();
        //    Mock.Arrange(() => ARInvoiceTracing.Trace(Arg.IsAny<ARInvoicePM>(), Arg.IsAny<ARInvoice>(), Arg.IsAny<bool>())).DoNothing();
        //    Mock.NonPublic.Arrange(service, "UpdateSearchField").DoNothing();
        //    Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
        //    Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
        //    Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
        //    Mock.Arrange(() => IdCounter.GetNumber("ARInvoiceTotalVAT", Arg.IsAny<int>())).Returns("1-1");
        //    Mock.Arrange(() => IdCounter.GetNumber("ARInvoiceEntity", Arg.IsAny<int>())).Returns("1-1");
        //    Mock.Arrange(() => IdCounter.GetNumber("ARInvoiceLine", Arg.IsAny<int>())).Returns("1-1");

        //    service.Create(newInvoice);


        //    ARInvoiceTotalVAT totalVat = invoiceContext.ARInvoiceTotalVATs.Where(d => d.Id == "1-1").FirstOrDefault();
        //    Assert.AreEqual(totalVat.LocalVATAmount, 100);

        //}

        [TestMethod]
        public void GetARInvoiceFiltersTest()
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
            queryoperations.ObjectTableName = "ARInvoice";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = true, FieldName = "IsClosed" , FieldValue=false});

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<ARInvoiceList> list = invoiceDomainService.GetARInvoiceFilters(bytearray, 1);


        }

    }
}
