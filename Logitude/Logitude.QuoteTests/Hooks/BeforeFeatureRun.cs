using Logitude.QuoteTests.Models;
using Logitude.QuoteTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.QuoteTests.Hooks
{
    [Binding]
    public class BeforeFeatureRun
    {
        [BeforeFeature("Pre-Prepare")]
        public static void SetUpPrepareDataBeforeFeatureRun()
        {
            CreateQuote();
        }

        private static void CreateQuote()
        {
            try
            {
                QuotePM quote = GetValidaQuotePM();
                ApiResponse<QuotePM> PostResponse = APICaller.CallPost<QuotePM>(quote, Urls.QuoteController, UserTenant.Token);
                QuoteDataMap(PostResponse.Data);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Failed Creating Quote Before Feature Run");
            }

        }

        private static void QuoteDataMap(QuotePM quote)
        {
            QuoteData.Id = quote.Id;
        }

        private static QuotePM GetValidaQuotePM()
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

    }
}
