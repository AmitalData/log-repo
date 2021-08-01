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
            tariffContext.TariffOceanLCLFreightCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.TariffOceanLCLFreightCostId), UserTenant.Token).Data;
        }
        
        [Then(@"ocean LCL freight cost tariff should be avaliable")]
        public void ThenOceanLCLFreightCostTariffShouldBeAvaliable()
        {
            tariffContext.TariffOceanLCLFreightCost.Should().NotBeNull();
            tariffContext.TariffOceanLCLFreightCost.Id.Should().NotBeNull();
        }
    }
}
