using Logitude.Tariff.Models;
using System;
using TechTalk.SpecFlow;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using FluentAssertions;

namespace Logitude.Tariff.Steps
{
    [Binding]
    public class GetTariffAirSurchargeCostSteps
    {
        private readonly TariffContext tariffContext;

        public GetTariffAirSurchargeCostSteps(TariffContext tariffContext)
        {
            this.tariffContext = tariffContext;
        }

        [When(@"get air surcharge cost tariff with TariffId")]
        public void WhenGetAirSurchargesCostTariffWithTariffId()
        {
            tariffContext.AirSurchargeCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.AirSurchargeCostId), UserTenant.Token).Data;
        }
        
        [Then(@"air surcharge cost tariff should be avaliable")]
        public void ThenAirSurchargesCostTariffShouldBeAvaliable()
        {
            tariffContext.AirSurchargeCost.Should().NotBeNull();
            tariffContext.AirSurchargeCost.Id.Should().NotBeNull();
        }
    }
}
