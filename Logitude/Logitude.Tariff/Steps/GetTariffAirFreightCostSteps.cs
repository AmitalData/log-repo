using Logitude.Tariff.Models;
using System;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using FluentAssertions;

namespace Logitude.Tariff.Steps
{
    [Binding]
    public class GetTariffAirFreightCostSteps
    {
        private readonly TariffContext tariffContext;

        public GetTariffAirFreightCostSteps(TariffContext tariffContext)
        {
            this.tariffContext = tariffContext;
        }

        [When(@"get air freight cost tariff with TariffId")]
        public void WhenGetAirFreightCostTariffWithTariffId()
        {
            tariffContext.TariffAirFreightCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.TariffAirFreightCostId), UserTenant.Token).Data;
        }
        
        [Then(@"air freight cost tariff should be avaliable")]
        public void ThenAirFreightCostTariffShouldBeAvaliable()
        {
            tariffContext.TariffAirFreightCost.Should().NotBeNull();
            tariffContext.TariffAirFreightCost.Id.Should().NotBeNull();
        }
    }
}
