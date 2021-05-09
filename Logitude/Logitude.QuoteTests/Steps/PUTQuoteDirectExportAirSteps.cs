using FluentAssertions;
using Logitude.QuoteTests.Models;
using Logitude.QuoteTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.QuoteTests.Steps
{
    [Binding]
    public class PUTQuoteDirectExportAirSteps
    {

        protected readonly QuoteContext QuoteContext;
        private QuotePackagePM Packages;
        private QuoteChargePM Charge;

        public PUTQuoteDirectExportAirSteps(QuoteContext quoteContext)
        {
            QuoteContext = quoteContext;
        }

        [Given(@"a package with the following properties")]
        public void GivenAPackageWithTheFollowingProperties(Table table)
        {
            Packages = CreatePackageInstance(table);
        }

        [Given(@"a Charge with the following properties")]
        public void GivenAChargeWithTheFollowingProperties(Table table)
        {
            Charge = CreateChargeInstance(table);
        }

        [Given(@"an export air quote")]
        public void GivenAnExportAirQuote()
        {
            QuoteContext.ExportAirQuote = CreateAndGetQuote(GetValidaQuotePM());
        }

        [When(@"update a quote")]
        public void WhenUpdateAQuote()
        {
            ApiResponse<QuotePM> response = UpdateQuote(QuoteContext.ExportAirQuote, Packages, Charge);
            QuoteContext.ExportAirQuote.Id = response.Data?.Id;
        }

        [Then(@"the quote should update successfully")]
        public void ThenTheQuoteShouldUpdateSuccessfully()
        {
            QuoteContext.ExportAirQuote.Id.Should().NotBeNull();
        }

        private ApiResponse<QuotePM> UpdateQuote(QuotePM exportAirQuote, QuotePackagePM packages, QuoteChargePM charge)
        {
            exportAirQuote = AddPackagesToQuote(exportAirQuote, packages);
            exportAirQuote = AddChargeToQuote(exportAirQuote, Charge);
            return APICaller.CallPut<QuotePM>(exportAirQuote, Urls.QuoteController, UserTenant.Token);
        }

        private QuotePM AddPackagesToQuote(QuotePM exportAirQuote, QuotePackagePM package)
        {
            package = new PackageBuilder().WithModel(package)
               .QuoteId(exportAirQuote.Id)
               .Build();

            return new QuoteBuilder().WithModel(exportAirQuote)
                .NumberOfPackages(package.Quantity)
                .QuotePackages(package)
                .Build();
        }

        private QuotePM AddChargeToQuote(QuotePM exportAirQuote, QuoteChargePM charge)
        {
            charge = new ChargesBuilder().WithModel(charge)
               .QuoteId(exportAirQuote.Id)
               .Build();

            return new QuoteBuilder().WithModel(exportAirQuote)
                .QuoteCharges(charge)
                .Build();
        }

        private QuotePM CreateAndGetQuote(QuotePM quotePM)
        {
            ApiResponse<QuotePM> PostResponse = APICaller.CallPost<QuotePM>(quotePM, Urls.QuoteController, UserTenant.Token);
            QuotePM quote = PostResponse.Data;

            string singleQuoteUrl = Urls.QuoteGetSingle(quote?.Id);

            ApiResponse<QuotePM> GetResponse = APICaller.CallGet<QuotePM>(singleQuoteUrl, UserTenant.Token);
            return GetResponse.Data;
        }

        #region Build Models Region
        private QuotePackagePM CreatePackageInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new PackageBuilder().WithDefualtValues()
               .Quantity((int)dataTable.Quantity)
               .Length((double)dataTable.Length)
               .Width((double)dataTable.Width)
               .Height((double)dataTable.Height)
               .Weight((double)dataTable.Weight)
               .Build();
        }

        private QuoteChargePM CreateChargeInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new ChargesBuilder().WithDefualtValues()
                .ChargesTypeCode((string)dataTable.ChargesType)
                .ChargesTypeId((string)dataTable.ChargesType)
                .ChargesTypeName((string)dataTable.ChargesTypeName)
                .CostMeasurementId((string)dataTable.Measurement)
                .SaleMeasurementId((string)dataTable.Measurement)
                .CostCurrencyId((string)dataTable.Currency)
                .SaleCurrencyId((string)dataTable.Currency)
                .CostExchangeRate((double)dataTable.ExchangeRate)
                .SaleExchangeRate((double)dataTable.ExchangeRate)
                .Build();
        }
        private QuotePM GetValidaQuotePM()
        {
            return new QuoteBuilder().WithDefualtValues()
                .DirectionId("E")
                .TransportModeId("A")
                .QuoteTypeCode("A")
                .FromPortId("JFK")
                .ToPortId("MIA")
                .ExchangeRate(1)
                .SaleCurrencyId("EUR")
                .QuoteCustomerTypeCode("SHI")
                .Build();
        }
        #endregion
    }
}
