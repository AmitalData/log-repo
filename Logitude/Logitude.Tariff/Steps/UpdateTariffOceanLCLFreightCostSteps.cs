using System;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
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
            tariffContext.TariffOceanLCLFreightCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.TariffOceanLCLFreightCostId), UserTenant.Token).Data;
        }
        
        [Given(@"following ocean LCL freight cost tariff properties")]
        public void GivenFollowingOceanLCLFreightCostTariffProperties(Table table)
        {
            tariffOceanLCLFreightCostServices.UpdateInstance(table, tariffContext.TariffOceanLCLFreightCost);
        }
        
        [When(@"update ocean LCL freight cost tariff")]
        public void WhenUpdateOceanLCLFreightCostTariff()
        {
            updatedTariff = APICaller.CallPut<TariffPM>(tariffContext.TariffOceanLCLFreightCost, Urls.TariffsController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the ocean LCL freight cost tariff should update successfully")]
        public void ThenTheOceanLCLFreightCostTariffShouldUpdateSuccessfully()
        {
            updatedTariff.Id.Should().NotBeNull();
            updatedTariff.Name.Should().Equals(tariffContext.TariffOceanLCLFreightCost.Name);
            updatedTariff.Notes.Should().Equals(tariffContext.TariffOceanLCLFreightCost.Notes);
        }
    }
}
