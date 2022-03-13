using Logitude.Tariff.Models;
using System;
using TechTalk.SpecFlow;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
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
            tariffContext.AirFreightCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.AirFreightCostId), UserTenant.Token).Data;
        }
        
        [Then(@"air freight cost tariff should be avaliable")]
        public void ThenAirFreightCostTariffShouldBeAvaliable()
        {
            tariffContext.AirFreightCost.Should().NotBeNull();
            tariffContext.AirFreightCost.Id.Should().NotBeNull();
        }
    }
}
