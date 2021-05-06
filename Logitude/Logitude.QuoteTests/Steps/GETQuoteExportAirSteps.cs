using FluentAssertions;
using Logitude.QuoteTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.QuoteTests.Steps
{
    [Binding]
    public class GETQuoteExportAirSteps
    {
        protected readonly QuoteContext QuoteContext;
        protected QuotePM Response;

        public GETQuoteExportAirSteps(QuoteContext quoteContext)
        {
            QuoteContext = quoteContext;
        }

        [When(@"get quote with QuoteId")]
        public void WhenGetQuoteWithQuoteId()
        {
            QuoteContext.ExportAirQuote = GetExportAirQuote();
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(QuoteContext.ExportAirQuote.Id);
            ApiResponse<IEnumerable<QuotePM>> response = APICaller.CallGetByFilters<IEnumerable<QuotePM>>(Urls.QuoteViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            Response = response.Data?.FirstOrDefault();

        }

        [Then(@"quote should be avaliable")]
        public void ThenQuoteShouldBeAvaliable()
        {
            Response.Id.Should().NotBeNull();
        }

        private QuotePM GetExportAirQuote()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1,
                Filter1Name = "TransportModeId",
                Filter1Operator = "equals",
                Filter1Value = "A",
                Filter2Name = "DirectionId",
                Filter2Operator = "equals",
                Filter2Value = "E"
            };

            ApiResponse<IEnumerable<QuotePM>> response = APICaller.CallGetByFilters<IEnumerable<QuotePM>>(Urls.QuoteViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }

        private ApiQueryFilters BuildApiQueryFilters(string quoteId)
        {
            return new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1,
                Filter1Name = "Id",
                Filter1Operator = "equals",
                Filter1Value = quoteId,
            };
        }
    }
}
