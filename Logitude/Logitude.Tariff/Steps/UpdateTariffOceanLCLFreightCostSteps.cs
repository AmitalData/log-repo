using System;
using TechTalk.SpecFlow;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using Logitude.Tariff.Models;
using Logitude.Tariff.Services;
using FluentAssertions;

namespace Logitude.Tariff.Steps
{
    [Binding]
    public class UpdateTariffOceanLCLFreightCostSteps
    {
        private readonly TariffContext tariffContext;
        private readonly TariffOceanLCLFreightCostServices tariffOceanLCLFreightCostServices;
        private TariffPM updatedTariff;

        public UpdateTariffOceanLCLFreightCostSteps(TariffContext tariffContext, TariffOceanLCLFreightCostServices tariffOceanLCLFreightCostServices, TariffPM updatedTariff)
        {
            this.tariffContext = tariffContext;
            this.tariffOceanLCLFreightCostServices = tariffOceanLCLFreightCostServices;
            this.updatedTariff = updatedTariff;
        }

        [Given(@"an ocean LCL freight cost tariff")]
        public void GivenAnOceanLCLFreightCostTariff()
        {
            tariffContext.OceanLCLFreightCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.OceanLCLFreightCostId), UserTenant.Token).Data;
        }
        
        [Given(@"following ocean LCL freight cost tariff properties")]
        public void GivenFollowingOceanLCLFreightCostTariffProperties(Table table)
        {
            tariffOceanLCLFreightCostServices.UpdateInstance(table, tariffContext.OceanLCLFreightCost);
        }
        
        [When(@"update ocean LCL freight cost tariff")]
        public void WhenUpdateOceanLCLFreightCostTariff()
        {
            updatedTariff = APICaller.CallPut<TariffPM>(tariffContext.OceanLCLFreightCost, Urls.TariffsController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the ocean LCL freight cost tariff should update successfully")]
        public void ThenTheOceanLCLFreightCostTariffShouldUpdateSuccessfully()
        {
            updatedTariff.Id.Should().NotBeNull();
            updatedTariff.Name.Should().Equals(tariffContext.OceanLCLFreightCost.Name);
            updatedTariff.Notes.Should().Equals(tariffContext.OceanLCLFreightCost.Notes);
        }
    }
}
