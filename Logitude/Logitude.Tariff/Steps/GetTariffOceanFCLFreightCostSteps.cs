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
    public class GetTariffOceanFCLFreightCostSteps
    {
        private readonly TariffContext tariffContext;
        public GetTariffOceanFCLFreightCostSteps(TariffContext tariffContext)
        {
            this.tariffContext = tariffContext;
        }
        [When(@"get ocean FCL freight cost tariff with TariffId")]
        public void WhenGetOceanFCLFreightCostTariffWithTariffId()
        {
            tariffContext.OceanFCLFreightCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.OceanFCLFreightCostId), UserTenant.Token).Data;
        }

        [Then(@"ocean FCL freight cost tariff should be avaliable")]
        public void ThenOceanFCLFreightCostTariffShouldBeAvaliable()
        {
            tariffContext.OceanFCLFreightCost.Should().NotBeNull();
            tariffContext.OceanFCLFreightCost.Id.Should().NotBeNull();
        }
    }
}
