using System;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using FluentAssertions;
using Logitude.Tariff.Models;
using Logitude.Tariff.Services;

namespace Logitude.Tariff.Steps
{
    [Binding]
    public class CreateTariffOceanLCLFreightCostSteps
    {
        private readonly TariffContext tariffContext;
        private readonly TariffOceanLCLFreightCostServices tariffOceanLCLFreightCostServices;

        public CreateTariffOceanLCLFreightCostSteps(TariffContext tariffContext, TariffOceanLCLFreightCostServices tariffOceanLCLFreightCostServices)
        {
            this.tariffContext = tariffContext;
            this.tariffOceanLCLFreightCostServices = tariffOceanLCLFreightCostServices;
        }

        [Given(@"an ocean LCL freight cost tariff with the following properties")]
        public void GivenAnOceanLCLFreightCostTariffWithTheFollowingProperties(Table table)
        {
            tariffContext.OceanLCLFreightCost = tariffOceanLCLFreightCostServices.CreateInstance(table);
        }
        
        [When(@"create ocean LCL freight cost tariff")]
        public void WhenCreateOceanLCLFreightCostTariff()
        {
            tariffContext.OceanLCLFreightCost = APICaller.CallPost<TariffPM>(tariffContext.OceanLCLFreightCost, Urls.TariffsController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the ocean LCL freight cost tariff should create successfully")]
        public void ThenTheOceanLCLFreightCostTariffShouldCreateSuccessfully()
        {
            tariffContext.OceanLCLFreightCost.Should().NotBeNull();
            tariffContext.OceanLCLFreightCost.Id.Should().NotBeNull();
        }
    }
}
