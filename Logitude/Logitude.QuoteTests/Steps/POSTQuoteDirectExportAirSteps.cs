using FluentAssertions;
using Logitude.QuoteTests.Models;
using Logitude.QuoteTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.QuoteTests.Steps
{
    [Binding]
    public class POSTQuoteDirectExportAirSteps
    {
        protected readonly QuoteContext QuoteContext;

        public POSTQuoteDirectExportAirSteps(QuoteContext quoteContext)
        {
            QuoteContext = quoteContext;
        }

        [Given(@"a quote with the following properties")]
        public void GivenAQuoteWithTheFollowingProperties(Table table)
        {
            QuoteContext.ExportAirQuote = CreateQuoteInstance(table);
        }

        [When(@"create quote")]
        public void WhenCreateQuote()
        {
            ApiResponse<QuotePM> response = APICaller.CallPost<QuotePM>(QuoteContext.ExportAirQuote, Urls.QuoteController, UserTenant.Token);
            QuoteContext.ExportAirQuote = response?.Data;
        }
        
        [Then(@"the quote should create successfully")]
        public void ThenTheQuoteShouldCreateSuccessfully()
        {
            QuoteContext.ExportAirQuote.Id.Should().NotBeNull();
        }

        private QuotePM CreateQuoteInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new QuoteBuilder().WithDefualtValues()
                .DirectionId((string)dataTable.Direction)
                .TransportModeId((string)dataTable.TransportMode)
                .QuoteTypeCode((string)dataTable.QuoteType)
                .FromPortId((string)dataTable.FromPort)
                .ToPortId((string)dataTable.ToPort)
                .ExchangeRate((double)dataTable.ExchangeRate)
                .SaleCurrencyId((string)dataTable.Currency)
                .QuoteCustomerTypeCode((string)dataTable.CustomerType)
                .Build();
        }
    }
}
