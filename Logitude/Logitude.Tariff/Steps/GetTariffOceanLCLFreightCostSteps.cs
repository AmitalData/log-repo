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
    public class GetTariffOceanLCLFreightCostSteps
    {
        private readonly TariffContext tariffContext;
        public GetTariffOceanLCLFreightCostSteps(TariffContext tariffContext)
        {
            this.tariffContext = tariffContext;
        }

        [When(@"get ocean LCL freight cost tariff with TariffId")]
        public void WhenGetOceanLCLFreightCostTariffWithTariffId()
        {
            tariffContext.OceanLCLFreightCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.OceanLCLFreightCostId), UserTenant.Token).Data;
        }
        
        [Then(@"ocean LCL freight cost tariff should be avaliable")]
        public void ThenOceanLCLFreightCostTariffShouldBeAvaliable()
        {
            tariffContext.OceanLCLFreightCost.Should().NotBeNull();
            tariffContext.OceanLCLFreightCost.Id.Should().NotBeNull();
        }
    }
}
