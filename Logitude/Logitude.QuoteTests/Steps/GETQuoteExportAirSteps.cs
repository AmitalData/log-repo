using FluentAssertions;
using Logitude.QuoteTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.QuoteTests.Steps
{
    [Binding]
    public class GETQuoteExportAirSteps
    {
        protected readonly QuoteContext QuoteContext;
        protected QuotePM Response;
        private ApiQueryFilters apiQueryFilters;

        public GETQuoteExportAirSteps(QuoteContext quoteContext)
        {
            QuoteContext = quoteContext;
        }

        [When(@"get quote with QuoteId")]
        public void WhenGetQuoteWithQuoteId()
        {
            string quotesGetSingleUrl = Urls.QuoteGetSingle(QuoteData.Id);
            ApiResponse<QuotePM> quoteResponse = APICaller.CallGet<QuotePM>(quotesGetSingleUrl, UserTenant.Token);
            Response = quoteResponse.Data;
        }

        [When(@"get single list quote with QuoteId")]
        public void WhenGetSingleListQuoteWithQuoteId()
        {
            string quotesGetSingleListUrl = Urls.QuoteGetSingleList(QuoteData.Id);
            ApiResponse<QuotePM> quoteResponse = APICaller.CallGet<QuotePM>(quotesGetSingleListUrl, UserTenant.Token);
            Response = quoteResponse.Data;
        }

        [Given(@"a quote with the following params")]
        public void GivenAQuoteWithTheFollowingParams(Table table)
        {
            apiQueryFilters = CreateApiQueryFilterInstance(table);
        }

        [When(@"get quote by filter")]
        public void WhenGetQuoteByFilter()
        {
            ApiResponse<IEnumerable<QuotePM>> response = APICaller.CallGetByFilters<IEnumerable<QuotePM>>(Urls.QuoteViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            Response = response.Data?.FirstOrDefault();

        }

        [Then(@"quote should be avaliable")]
        public void ThenQuoteShouldBeAvaliable()
        {
            Response.Id.Should().NotBeNull();
        }

        private ApiQueryFilters CreateApiQueryFilterInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("TransportModeId")
                .Filter1Operator("equals")
                .Filter1Value((string)dataTable.TransportMode)
                .Filter2Name("DirectionId")
                .Filter2Operator("equals")
                .Filter2Value((string)dataTable.Direction)
                .Build();
        }
    }
}
