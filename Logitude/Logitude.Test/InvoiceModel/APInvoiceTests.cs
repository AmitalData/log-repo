using System;
using System.Collections.Generic;
using System.Linq;

using Logitude.Test.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Mocks;
using Simplog.Global.Data.GlobalModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Mocks;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Mocks;

using Telerik.JustMock;

using WebFreight.Web.GlobalModelDB;
using WebFreight.Web.Helpers;
using WebFreight.Web.InvoiceModel.DomainServices;
using Logitude.BL.InvoiceModel.EntityPMs;
using WebFreight.Web.InvoiceModel.Tools.EntityService;
using WebFreight.Web.InvoiceModel.Tools.TraceEvents;
using WebFreight.Web.InvoiceModel.Tools.Validating;
using WebFreight.Web.Security;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
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
    public class APInvoiceTests
    {
        MockWebFreightContext webFreightContext;

        [TestInitialize]
        public void InitializeAPInvoiceTests()
        {
            webFreightContext = new MockWebFreightContext();
            //GeneralMocking.FillInfrastructureData(webFreightContext);
        }

        [TestMethod]
        public void GetSingleAPInvoicePMTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());
            
            Mock.Arrange(() => SecurityUtility.AuthenticationOnTenant(Arg.IsAny<int>())).DoNothing();
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(),Arg.IsAny<string>(),Arg.IsAny<int>())).DoNothing();
            APInvoicePM invoicepm = invoiceDomainService.GetSingleAPInvoice("1-1", 1);
            Assert.AreEqual("1-1", invoicepm.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleAPInvoicePMWithAuthenticationOnTenantTest()
        {
            GeneralMocking.MockHttpContext("NotExistUser@fnarsoft.com");
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());
   
            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            APInvoicePM invoicepm = invoiceDomainService.GetSingleAPInvoice("1-1", 2);
            
        }

        [TestMethod]
        [ExpectedException(typeof(AutenticationException))]
        public void GetSingleAPInvoicePMFromAnoterTenantTest()
        {
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            InvoiceDomainService invoiceDomainService = new InvoiceDomainService();
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(new MockShipmentContext());

            Mock.Arrange(() => SecurityUtility.CheckContactFeature(Arg.IsAny<string>(), Arg.IsAny<string>(), Arg.IsAny<int>())).DoNothing();
            APInvoicePM invoicepm = invoiceDomainService.GetSingleAPInvoice("1-1", 3);

        }
        

        [TestMethod]
        public void CreateAPInvoiceTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            MockCommonContext commonContext = new MockCommonContext();
            
            int tenant = 1;
            APInvoiceService service = new APInvoiceService(invoiceContext, tenant);
            
            APInvoicePM newInvoice = new APInvoicePM()
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
                InternalNumber = "1000",
                InvoiceCurrencyId = "1-1",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,
                InvoiceExpectedAmount = 1000,
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
                StatusCode = "WA",
                UpdatedByUserId = "1-1",
                VendorId = "1-1",

            };
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
          

            Mock.Arrange(() => APInvoiceValidator.Validate(Arg.IsAny<APInvoicePM>())).DoNothing();
            Mock.Arrange(() => APInvoiceTracing.Trace(Arg.IsAny<APInvoicePM>(), Arg.IsAny<APInvoice>(), Arg.IsAny<bool>())).DoNothing();
            Mock.NonPublic.Arrange(service, "BuildUnexpectedPayables").DoNothing();
            Mock.NonPublic.Arrange(service, "GetAllPayables", new object[] {new List <APInvoiceLinePM>() }).DoNothing();
            Mock.NonPublic.Arrange(service, "UpdateInvoiceEntities").DoNothing();
            Mock.NonPublic.Arrange(service, "BuildSearchField").DoNothing();
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
          
            service.Create(newInvoice);
            APInvoice invoice = invoiceContext.APInvoices.Where(d=>d.Id=="1-1").FirstOrDefault();
            Assert.AreNotEqual
                (invoice, null,
              "create invoice test succeeded");
        }

        [TestMethod]
        public void CreateAPInvoiceWithInvoiceLineTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            int tenant = 1;
            APInvoiceService service = new APInvoiceService(invoiceContext, tenant);
            
            APInvoicePM newInvoice = new APInvoicePM()
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
                InternalNumber = "1000",
                InvoiceCurrencyId = "1-1",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,
                InvoiceExpectedAmount = 1000,
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
                StatusCode = "WA",
                UpdatedByUserId = "1-1",
                VendorId = "1-1",

            };

            newInvoice.InvoiceLines = new List<APInvoiceLinePM>()
            {
                new APInvoiceLinePM()
                {
                    APInvoiceId = "1-1",
                    Tenant = tenant,
                    ChargesTypeId = "1-1",
                    AmountTypeCode = "EXPT",
                    ChargesTypeCode = "FRT",
                    ExpectedAmount = 1000,
                    OpenAmount = 1000,
                    InvoiceCurrencyAmount = 1000,
                    LocalCurrencyAmount = 1000,
                    ProfitCurrencyAmount = 1000,
                    ForiegnExchangeRate = 1,
                    EntityId = "1-1",
                    EntityPayableId = "1-1",
                    LineNumber = 1,
                    EntityReference = "1000",
                    ForiegnCurrencyId = "1-1",
                    VendorId = "1-1",
                    VatTypeId = "1-1",
                    VatPercentage = 10
                },
            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            Mock.Arrange(() => APInvoiceValidator.Validate(Arg.IsAny<APInvoicePM>())).DoNothing();
            Mock.Arrange(() => APInvoiceTracing.Trace(Arg.IsAny<APInvoicePM>(), Arg.IsAny<APInvoice>(), Arg.IsAny<bool>())).DoNothing();
            Mock.NonPublic.Arrange(service, "BuildUnexpectedPayables").DoNothing();
            Mock.NonPublic.Arrange(service, "BuildSearchField").DoNothing();
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => IdCounter.GetNumber("APInvoiceTotalVAT", Arg.IsAny<int>())).Returns("1-1");
            Mock.Arrange(() => IdCounter.GetNumber("APInvoiceEntity", Arg.IsAny<int>())).Returns("1-1");
            
            service.Create(newInvoice);

            APInvoice invoice = invoiceContext.APInvoices.Where(d => d.Id == "1-1").FirstOrDefault();

            Assert.AreNotEqual(invoice, null);
          
        }

        [TestMethod]
        public void CreateAPInvoiceWithAPInvoiceTotalVATTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            int tenant = 1;
            APInvoiceService service = new APInvoiceService(invoiceContext, tenant);

            APInvoicePM newInvoice = new APInvoicePM()
            {
                Id = "1-6",
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
                InternalNumber = "1000",
                InvoiceCurrencyId = "1-1",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,
                InvoiceExpectedAmount = 1000,
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
                StatusCode = "WA",
                UpdatedByUserId = "1-1",
                VendorId = "1-1",

            };

            newInvoice.InvoiceLines = new List<APInvoiceLinePM>()
            {
                new APInvoiceLinePM()
                {
                    APInvoiceId = "1-6",
                    Tenant = tenant,
                    ChargesTypeId = "1-1",
                    AmountTypeCode = "EXPT",
                    ChargesTypeCode = "FRT",
                    ExpectedAmount = 1000,
                    OpenAmount = 1000,
                    InvoiceCurrencyAmount = 1000,
                    LocalCurrencyAmount = 1000,
                    ProfitCurrencyAmount = 1000,
                    ForiegnExchangeRate = 1,
                    EntityId = "1-1",
                    EntityPayableId = "1-1",
                    LineNumber = 1,
                    EntityReference = "1000",
                    ForiegnCurrencyId = "1-1",
                    VendorId = "1-1",
                    VatTypeId = "1-1",
                    VatPercentage = 10
                },
            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            Mock.Arrange(() => APInvoiceValidator.Validate(Arg.IsAny<APInvoicePM>())).DoNothing();
            Mock.Arrange(() => APInvoiceTracing.Trace(Arg.IsAny<APInvoicePM>(), Arg.IsAny<APInvoice>(), Arg.IsAny<bool>())).DoNothing();
            Mock.NonPublic.Arrange(service, "BuildUnexpectedPayables").DoNothing();
            Mock.NonPublic.Arrange(service, "BuildSearchField").DoNothing();
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => IdCounter.GetNumber("APInvoiceTotalVAT", Arg.IsAny<int>())).Returns("1-6");
            Mock.Arrange(() => IdCounter.GetNumber("APInvoiceEntity", Arg.IsAny<int>())).Returns("1-6");

            service.Create(newInvoice);

            APInvoiceTotalVAT totalVat = invoiceContext.APInvoiceTotalVATs.Where(d => d.Id == "1-6").FirstOrDefault();
            Assert.AreNotEqual(totalVat, null);
        }

        [TestMethod]
        public void CreateAPInvoiceWithAPInvoiceLineTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            int tenant = 1;
            APInvoiceService service = new APInvoiceService(invoiceContext, tenant);

            APInvoicePM newInvoice = new APInvoicePM()
            {
                Id = "1-5",
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
                InternalNumber = "1000",
                InvoiceCurrencyId = "1-1",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,
                InvoiceExpectedAmount = 1000,
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
                StatusCode = "WA",
                UpdatedByUserId = "1-1",
                VendorId = "1-1",

            };

           newInvoice.InvoiceLines = new List<APInvoiceLinePM>()
            {
                new APInvoiceLinePM()
                {
                    APInvoiceId = "1-5",
                    Tenant = tenant,
                    ChargesTypeId = "1-1",
                    AmountTypeCode = "EXPT",
                    ChargesTypeCode = "FRT",
                    ExpectedAmount = 1000,
                    OpenAmount = 1000,
                    InvoiceCurrencyAmount = 1000,
                    LocalCurrencyAmount = 1000,
                    ProfitCurrencyAmount = 1000,
                    ForiegnExchangeRate = 1,
                    EntityId = "1-1",
                    EntityPayableId = "1-1",
                    LineNumber = 1,
                    EntityReference = "1000",
                    ForiegnCurrencyId = "1-1",
                    VendorId = "1-1",
                    VatTypeId = "1-1",
                    VatPercentage = 10
                },
            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            Mock.Arrange(() => APInvoiceValidator.Validate(Arg.IsAny<APInvoicePM>())).DoNothing();
            Mock.Arrange(() => APInvoiceTracing.Trace(Arg.IsAny<APInvoicePM>(), Arg.IsAny<APInvoice>(), Arg.IsAny<bool>())).DoNothing();
            Mock.NonPublic.Arrange(service, "BuildUnexpectedPayables").DoNothing();
            Mock.NonPublic.Arrange(service, "BuildSearchField").DoNothing();
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => IdCounter.GetNumber("APInvoiceTotalVAT", Arg.IsAny<int>())).Returns("1-5");
            Mock.Arrange(() => IdCounter.GetNumber("APInvoiceEntity", Arg.IsAny<int>())).Returns("1-5");
            Mock.Arrange(() => SecurityUtility.GetAuthenticatedUser()).Returns("user1@fnarsoft.com");

            service.Create(newInvoice);


            APInvoiceLine InvoiceLine = invoiceContext.APInvoiceLines.Where(d => d.APInvoiceId == "1-5").FirstOrDefault();

            Assert.AreNotEqual(InvoiceLine, null);
        

        }


        [TestMethod]
        public void CreateAPInvoiceWithAPInvoiceEntityTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            MockCommonContext commonContext = new MockCommonContext();
            CacheManager.CacheWrapper = new MockCacheWrapper();
            int tenant = 1;
            APInvoiceService service = new APInvoiceService(invoiceContext, tenant);

            APInvoicePM newInvoice = new APInvoicePM()
            {
                Id = "1-9",
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
                InternalNumber = "1000",
                InvoiceCurrencyId = "1-1",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,
                InvoiceExpectedAmount = 1000,
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
                StatusCode = "WA",
                UpdatedByUserId = "1-1",
                VendorId = "1-1",

            };

            newInvoice.InvoiceLines = new List<APInvoiceLinePM>()
            {
                new APInvoiceLinePM()
                {
                    APInvoiceId = "1-9",
                    Tenant = tenant,
                    ChargesTypeId = "1-1",
                    AmountTypeCode = "EXPT",
                    ChargesTypeCode = "FRT",
                    ExpectedAmount = 1000,
                    OpenAmount = 1000,
                    InvoiceCurrencyAmount = 1000,
                    LocalCurrencyAmount = 1000,
                    ProfitCurrencyAmount = 1000,
                    ForiegnExchangeRate = 1,
                    EntityId = "1-1",
                    EntityPayableId = "1-1",
                    LineNumber = 1,
                    EntityReference = "1000",
                    ForiegnCurrencyId = "1-1",
                    VendorId = "1-1",
                    VatTypeId = "1-1",
                    VatPercentage = 10
                },
            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            Mock.Arrange(() => APInvoiceValidator.Validate(Arg.IsAny<APInvoicePM>())).DoNothing();
            Mock.Arrange(() => APInvoiceTracing.Trace(Arg.IsAny<APInvoicePM>(), Arg.IsAny<APInvoice>(), Arg.IsAny<bool>())).DoNothing();
            Mock.NonPublic.Arrange(service, "BuildUnexpectedPayables").DoNothing();
            Mock.NonPublic.Arrange(service, "BuildSearchField").DoNothing();
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => IdCounter.GetNumber("APInvoiceTotalVAT", Arg.IsAny<int>())).Returns("1-9");
            Mock.Arrange(() => IdCounter.GetNumber("APInvoiceEntity", Arg.IsAny<int>())).Returns("1-9");

            service.Create(newInvoice);

         
            APInvoiceEntity apInvoiceEntity = invoiceContext.APInvoiceEntities.Where(d => d.Id == "1-9").FirstOrDefault();

            Assert.AreNotEqual(apInvoiceEntity, null);

        }

        [TestMethod]
        public void APInvoiceVatAmountOnCreateTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            MockCommonContext commonContext = new MockCommonContext();
            MockShipmentContext shipmentContext = new MockShipmentContext();
            int tenant = 1;
            APInvoiceService service = new APInvoiceService(invoiceContext, tenant);
            APInvoiceRepository invoiceRepository = new APInvoiceRepository(invoiceContext);
            APInvoicePM newInvoice = new APInvoicePM()
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
                InternalNumber = "1000",
                InvoiceCurrencyId = "1-1",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,
                InvoiceExpectedAmount = 1000,
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
                StatusCode = "WA",
                UpdatedByUserId = "1-1",
                VendorId = "1-1",

            };

            newInvoice.InvoiceLines = new List<APInvoiceLinePM>()
            {
                new APInvoiceLinePM()
                {
                    APInvoiceId = "1-1",
                    Tenant = tenant,
                    ChargesTypeId = "1-1",
                    AmountTypeCode = "EXPT",
                    ChargesTypeCode = "FRT",
                    ExpectedAmount = 1000,
                    OpenAmount = 1000,
                    InvoiceCurrencyAmount = 1000,
                    LocalCurrencyAmount = 1000,
                    ProfitCurrencyAmount = 1000,
                    ForiegnExchangeRate = 1,
                    EntityId = "1-1",
                    EntityPayableId = "1-1",
                    LineNumber = 1,
                    EntityReference = "1000",
                    ForiegnCurrencyId = "1-1",
                    VendorId = "1-1",
                    VatTypeId = "1-1",
                    VatPercentage = 10
                },
            };

            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            Mock.Arrange(() => APInvoiceValidator.Validate(Arg.IsAny<APInvoicePM>())).DoNothing();
            Mock.Arrange(() => APInvoiceTracing.Trace(Arg.IsAny<APInvoicePM>(), Arg.IsAny<APInvoice>(), Arg.IsAny<bool>())).DoNothing();
            Mock.NonPublic.Arrange(service, "BuildUnexpectedPayables").DoNothing();
            Mock.NonPublic.Arrange(service, "BuildSearchField").DoNothing();
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(new MockCommonContext());
            Mock.Arrange(() => IdCounter.GetNumber("APInvoiceTotalVAT", Arg.IsAny<int>())).Returns("1-1");
            Mock.Arrange(() => IdCounter.GetNumber("APInvoiceEntity", Arg.IsAny<int>())).Returns("1-1");

            service.Create(newInvoice);

          
            APInvoiceTotalVAT totalVat = invoiceContext.APInvoiceTotalVATs.Where(d => d.Id == "1-1").FirstOrDefault();
            Assert.AreEqual(totalVat.LocalVATAmount, 100);

        }

        //[TestMethod]
        //[ExpectedException(typeof(ApplicationException))]
        //public void APInvoiceValidateNoLinesOnCreateTest()
        //{
        //    int tenant = 1;
        //    MockInvoiceContext invoiceContext = new MockInvoiceContext();
        //    MockCommonContext commonContext = new MockCommonContext();
        //    APInvoiceService service = new APInvoiceService(invoiceContext, tenant);
        //    APInvoicePM newInvoice = new APInvoicePM()
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
        //        InternalNumber = "1000",
        //        InvoiceCurrencyId = "1-1",
        //        InvoiceCurrencyExchangeRate = 1,
        //        InvoiceDate = DateTime.Now.Date,
        //        InvoiceExpectedAmount = 1000,
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
        //        StatusCode = "WA",
        //        UpdatedByUserId = "1-1",
        //        VendorId = "1-1",

        //    };

          
        //    GeneralMocking.MockHttpContext("user1@fnarsoft.com");
        //    Mock.Arrange(() => APInvoiceTracing.Trace(Arg.IsAny<APInvoicePM>(), Arg.IsAny<APInvoice>(), Arg.IsAny<bool>())).DoNothing();
        //    Mock.NonPublic.Arrange(service, "BuildUnexpectedPayables").DoNothing();
        //    Mock.NonPublic.Arrange(service, "BuildSearchField").DoNothing();
        //    Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
        //    Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
        //    Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
        //    Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
        //    Mock.Arrange(() => IdCounter.GetNumber("APInvoiceTotalVAT", Arg.IsAny<int>())).Returns("1-1");
        //    Mock.Arrange(() => IdCounter.GetNumber("APInvoiceEntity", Arg.IsAny<int>())).Returns("1-1");
            
            

        //    try
        //    {
        //        service.Create(newInvoice);
        //    }
        //    catch (ApplicationException exception)
        //    {
        //        Assert.AreEqual("You should have at least 1 invoice line", exception.Message);
        //        throw;
        //    }
            
            

        //}

        //[TestMethod]
        //[ExpectedException(typeof(ApplicationException))]
        //public void APInvoiceValidateFutureDateOnCreateTest()
        //{
        //    int tenant = 1;
        //    MockInvoiceContext invoiceContext = new MockInvoiceContext();
        //    MockCommonContext commonContext = new MockCommonContext();
        //    APInvoiceService service = new APInvoiceService(invoiceContext, tenant);
        //    APInvoicePM newInvoice = new APInvoicePM()
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
        //        InternalNumber = "1000",
        //        InvoiceCurrencyId = "1-1",
        //        InvoiceCurrencyExchangeRate = 1,
        //        InvoiceDate = DateTime.Now.Date.AddDays(30).Date,
        //        InvoiceExpectedAmount = 1000,
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
        //        StatusCode = "WA",
        //        UpdatedByUserId = "1-1",
        //        VendorId = "1-1",

        //    };

        //    newInvoice.InvoiceLines = new List<APInvoiceLinePM>()
        //    {
        //        new APInvoiceLinePM()
        //        {
        //            APInvoiceId = "1-1",
        //            Tenant = tenant,
        //            ChargesTypeId = "1-1",
        //            AmountTypeCode = "EXPT",
        //            ChargesTypeCode = "FRT",
        //            ExpectedAmount = 1000,
        //            OpenAmount = 1000,
        //            InvoiceCurrencyAmount = 1000,
        //            LocalCurrencyAmount = 1000,
        //            ProfitCurrencyAmount = 1000,
        //            InvoiceToPayableExchangeRate = 1,
        //            EntityId = "1-1",
        //            EntityPayableId = "1-1",
        //            LineNumber = 1,
        //            EntityReference = "1000",
        //            ObjectTableId = "1-1",
        //            PayableCurrencyId = "1-1",
        //            VendorId = "1-1",
        //            VatTypeId = "1-1",
        //            VatPercentage = 10
        //        },
        //    };

        //    GeneralMocking.MockHttpContext("user1@fnarsoft.com");
        //    Mock.Arrange(() => APInvoiceTracing.Trace(Arg.IsAny<APInvoicePM>(), Arg.IsAny<APInvoice>(), Arg.IsAny<bool>())).DoNothing();
        //    Mock.NonPublic.Arrange(service, "BuildUnexpectedPayables").DoNothing();
        //    Mock.NonPublic.Arrange(service, "BuildSearchField").DoNothing();
        //    Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
        //    Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
        //    Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
        //    Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
        //    Mock.Arrange(() => IdCounter.GetNumber("APInvoiceTotalVAT", Arg.IsAny<int>())).Returns("1-1");
        //    Mock.Arrange(() => IdCounter.GetNumber("APInvoiceEntity", Arg.IsAny<int>())).Returns("1-1");

        //    try
        //    {
        //        service.Create(newInvoice);
        //    }
        //    catch (ApplicationException exception)
        //    {
                
        //        Assert.AreEqual("Can't receive an invoice with a future date", exception.Message);
        //        throw;
        //    }

        //}

        //[TestMethod]
        //[ExpectedException(typeof(ApplicationException))]
        //public void APInvoiceValidateVatPercentageOnCreateTest()
        //{
        //    int tenant = 1;
        //    MockInvoiceContext invoiceContext = new MockInvoiceContext();
        //    MockCommonContext commonContext = new MockCommonContext();
        //    APInvoiceService service = new APInvoiceService(invoiceContext, tenant);
        //    APInvoicePM newInvoice = new APInvoicePM()
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
        //        InternalNumber = "1000",
        //        InvoiceCurrencyId = "1-1",
        //        InvoiceCurrencyExchangeRate = 1,
        //        InvoiceDate = DateTime.Now.Date,
        //        InvoiceExpectedAmount = 1000,
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
        //        StatusCode = "WA",
        //        UpdatedByUserId = "1-1",
        //        VendorId = "1-1",

        //    };

        //    newInvoice.InvoiceLines = new List<APInvoiceLinePM>()
        //    {
        //        new APInvoiceLinePM()
        //        {
        //            APInvoiceId = "1-1",
        //            Tenant = tenant,
        //            ChargesTypeId = "1-1",
        //            AmountTypeCode = "EXPT",
        //            ChargesTypeCode = "FRT",
        //            ExpectedAmount = 1000,
        //            OpenAmount = 1000,
        //            InvoiceCurrencyAmount = 1000,
        //            LocalCurrencyAmount = 1000,
        //            ProfitCurrencyAmount = 1000,
        //            InvoiceToPayableExchangeRate = 1,
        //            EntityId = "1-1",
        //            EntityPayableId = "1-1",
        //            LineNumber = 1,
        //            EntityReference = "1000",
        //            ObjectTableId = "1-1",
        //            PayableCurrencyId = "1-1",
        //            VendorId = "1-1",
        //            VatTypeId = "1-1",
                    
        //        },
        //    };

        //    GeneralMocking.MockHttpContext("user1@fnarsoft.com");
        //    Mock.Arrange(() => APInvoiceTracing.Trace(Arg.IsAny<APInvoicePM>(), Arg.IsAny<APInvoice>(), Arg.IsAny<bool>())).DoNothing();
        //    Mock.NonPublic.Arrange(service, "BuildUnexpectedPayables").DoNothing();
        //    Mock.NonPublic.Arrange(service, "BuildSearchField").DoNothing();
        //    Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
        //    Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
        //    Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
        //    Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
        //    Mock.Arrange(() => IdCounter.GetNumber("APInvoiceTotalVAT", Arg.IsAny<int>())).Returns("1-1");
        //    Mock.Arrange(() => IdCounter.GetNumber("APInvoiceEntity", Arg.IsAny<int>())).Returns("1-1");

        //    try
        //    {
        //        service.Create(newInvoice);
        //    }
        //    catch (ApplicationException exception)
        //    {
        //        Assert.AreEqual("Some of invoice lines Vat Type Percentage is empty", exception.Message);
        //        throw;
        //    }

        //}

        //[TestMethod]
        //[ExpectedException(typeof(ApplicationException))]
        //public void APInvoiceValidateIsVatNumberMandatoryOnCreateTest()
        //{
        //    int tenant = 2;
        //    MockInvoiceContext invoiceContext = new MockInvoiceContext();
        //    MockCommonContext commonContext = new MockCommonContext();
        //    APInvoiceService service = new APInvoiceService(invoiceContext, tenant);
        //    APInvoicePM newInvoice = new APInvoicePM()
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
        //        InternalNumber = "1000",
        //        InvoiceCurrencyId = "1-1",
        //        InvoiceCurrencyExchangeRate = 1,
        //        InvoiceDate = DateTime.Now.Date,
        //        InvoiceExpectedAmount = 1000,
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
        //        StatusCode = "WA",
        //        UpdatedByUserId = "1-1",
        //        VendorId = "1-1",

        //    };

        //    newInvoice.InvoiceLines = new List<APInvoiceLinePM>()
        //    {
        //        new APInvoiceLinePM()
        //        {
        //            APInvoiceId = "1-1",
        //            Tenant = tenant,
        //            ChargesTypeId = "1-1",
        //            AmountTypeCode = "EXPT",
        //            ChargesTypeCode = "FRT",
        //            ExpectedAmount = 1000,
        //            OpenAmount = 1000,
        //            InvoiceCurrencyAmount = 1000,
        //            LocalCurrencyAmount = 1000,
        //            ProfitCurrencyAmount = 1000,
        //            InvoiceToPayableExchangeRate = 1,
        //            EntityId = "1-1",
        //            EntityPayableId = "1-1",
        //            LineNumber = 1,
        //            EntityReference = "1000",
        //            ObjectTableId = "1-1",
        //            PayableCurrencyId = "1-1",
        //            VendorId = "1-1",
        //            VatTypeId = "1-1",
        //            VatPercentage = 10,
        //        },
        //    };

        //    GeneralMocking.MockHttpContext("user1@fnarsoft.com");
        //    Mock.Arrange(() => APInvoiceTracing.Trace(Arg.IsAny<APInvoicePM>(), Arg.IsAny<APInvoice>(), Arg.IsAny<bool>())).DoNothing();
        //    Mock.NonPublic.Arrange(service, "BuildUnexpectedPayables").DoNothing();
        //    Mock.NonPublic.Arrange(service, "BuildSearchField").DoNothing();
        //    Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
        //    Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
        //    Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
        //    Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
        //    Mock.Arrange(() => IdCounter.GetNumber("APInvoiceTotalVAT", Arg.IsAny<int>())).Returns("1-1");
        //    Mock.Arrange(() => IdCounter.GetNumber("APInvoiceEntity", Arg.IsAny<int>())).Returns("1-1");

        //    try
        //    {
        //        service.Create(newInvoice);
        //    }
        //    catch (ApplicationException exception)
        //    {
        //        Assert.AreEqual("Vat Number Field is Required", exception.Message);
        //        throw;
        //    }

        //}

        //[TestMethod]
        //[ExpectedException(typeof(ApplicationException))]
        //public void APInvoiceValidateInvoiceExpectedAmountOnCreateTest()
        //{
        //    int tenant = 1;
        //    MockInvoiceContext invoiceContext = new MockInvoiceContext();
        //    MockCommonContext commonContext = new MockCommonContext();
        //    APInvoiceService service = new APInvoiceService(invoiceContext, tenant);
        //    APInvoicePM newInvoice = new APInvoicePM()
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
        //        InternalNumber = "1000",
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
        //        StatusCode = "WA",
        //        UpdatedByUserId = "1-1",
        //        VendorId = "1-1",

        //    };

        //    newInvoice.InvoiceLines = new List<APInvoiceLinePM>()
        //    {
        //        new APInvoiceLinePM()
        //        {
        //            APInvoiceId = "1-1",
        //            Tenant = tenant,
        //            ChargesTypeId = "1-1",
        //            AmountTypeCode = "EXPT",
        //            ChargesTypeCode = "FRT",
        //            ExpectedAmount = 1000,
        //            OpenAmount = 1000,
        //            InvoiceCurrencyAmount = 1000,
        //            LocalCurrencyAmount = 1000,
        //            ProfitCurrencyAmount = 1000,
        //            InvoiceToPayableExchangeRate = 1,
        //            EntityId = "1-1",
        //            EntityPayableId = "1-1",
        //            LineNumber = 1,
        //            EntityReference = "1000",
        //            ObjectTableId = "1-1",
        //            PayableCurrencyId = "1-1",
        //            VendorId = "1-1",
        //            VatTypeId = "1-1",
        //            VatPercentage = 10,
        //        },
        //    };

        //    GeneralMocking.MockHttpContext("user1@fnarsoft.com");
        //    Mock.Arrange(() => APInvoiceTracing.Trace(Arg.IsAny<APInvoicePM>(), Arg.IsAny<APInvoice>(), Arg.IsAny<bool>())).DoNothing();
        //    Mock.NonPublic.Arrange(service, "BuildUnexpectedPayables").DoNothing();
        //    Mock.NonPublic.Arrange(service, "BuildSearchField").DoNothing();
        //    Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
        //    Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
        //    Mock.Arrange(() => WebFreightContext.GetContext(Arg.IsAny<int>())).Returns(webFreightContext);
        //    Mock.Arrange(() => GlobalContext.GetContext()).Returns(new MockGlobalContext());
        //    Mock.Arrange(() => IdCounter.GetNumber("APInvoiceTotalVAT", Arg.IsAny<int>())).Returns("1-1");
        //    Mock.Arrange(() => IdCounter.GetNumber("APInvoiceEntity", Arg.IsAny<int>())).Returns("1-1");
        //    try
        //    {
        //        service.Create(newInvoice);
        //    }
        //    catch (ApplicationException exception)
        //    {
        //        Assert.AreEqual("Invoice Amount Field is Required", exception.Message);
        //        throw;
        //    }

        //}

        [TestMethod]
        public void APInvoiceUpdateTest()
        {
            MockInvoiceContext invoiceContext = new MockInvoiceContext();
            MockCommonContext commonContext = new MockCommonContext();
            MockShipmentContext shipmentContext = new MockShipmentContext();
            int tenant = 1;
            APInvoiceService service = new APInvoiceService(invoiceContext, tenant);
            APInvoiceRepository invoiceRepository = new APInvoiceRepository(invoiceContext);
            APInvoiceQuery invoiceQuery=new APInvoiceQuery(invoiceRepository);
            CacheManager.CacheWrapper = new MockCacheWrapper();
            GeneralMocking.MockHttpContext("user1@fnarsoft.com");
            Mock.Arrange(() => APInvoiceValidator.Validate(Arg.IsAny<APInvoicePM>())).DoNothing();
            Mock.Arrange(() => APInvoiceTracing.Trace(Arg.IsAny<APInvoicePM>(), Arg.IsAny<APInvoice>(), Arg.IsAny<bool>())).DoNothing();
            Mock.NonPublic.Arrange(service, "BuildUnexpectedPayables").DoNothing();
            Mock.NonPublic.Arrange(service, "BuildSearchField").DoNothing();
            Mock.Arrange(() => TenantServerConfigration.GetCurrentDateTime(Arg.IsAny<int>())).Returns(DateTime.Now);
            Mock.Arrange(() => CommonDataContext.GetContext(Arg.IsAny<int>())).Returns(commonContext);
            Mock.Arrange(() => ShipmentsContext.GetContext(Arg.IsAny<int>())).Returns(shipmentContext);
            Mock.Arrange(() => InvoiceContext.GetContext(Arg.IsAny<int>())).Returns(invoiceContext);
            Mock.Arrange(() => IdCounter.GetNumber("APInvoiceTotalVAT", Arg.IsAny<int>())).Returns("1-1");
            Mock.Arrange(() => IdCounter.GetNumber("APInvoiceEntity", Arg.IsAny<int>())).Returns("1-1");

            APInvoicePM invoicePM = invoiceQuery.GetSinglePM("1-1", tenant);

            invoicePM.InternalNotes = "Edited Notes";

            service.Update(invoicePM,invoicePM.InvoiceLines,invoicePM.InvoicePayments);

            APInvoice invoice = invoiceRepository.GetSingleAPInvoice("1-1", tenant);

            Assert.AreEqual(invoice.InternalNotes, "Edited Notes");

        }

        [TestMethod]
        public void GetAPInvoiceFiltersTest()
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
            queryoperations.ObjectTableName = "APInvoice";
            queryoperations.PageIndex = 0;
            queryoperations.PageSize = 10;
            queryoperations.QueryFilterItems = new List<QueryFilterItem>();
            queryoperations.QueryFilterItems.Add(new QueryFilterItem() { DisplayInList = true, FieldName = "IsClosed" , FieldValue= false });

            MemoryStream memorystream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            serializer.Serialize(memorystream, queryoperations);
            memorystream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memorystream);
            string content = reader.ReadToEnd();
            byte[] bytearray = Encoding.ASCII.GetBytes(content);

            IQueryable<APInvoiceList> list = invoiceDomainService.GetAPInvoiceFilters(bytearray, 1);


        }
       
    }
}
