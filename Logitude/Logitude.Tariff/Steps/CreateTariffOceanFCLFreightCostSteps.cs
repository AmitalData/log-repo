using System;
using TechTalk.SpecFlow;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using FluentAssertions;
using Logitude.Tariff.Models;
using Logitude.Tariff.Services;

namespace Logitude.Tariff.Steps
{
    [Binding]
    public class CreateTariffOceanFCLFreightCostSteps
    {

        private readonly TariffContext tariffContext;
        private readonly TariffOceanFCLFreightCostServices tariffOceanFCLFreightCostServices;

        public CreateTariffOceanFCLFreightCostSteps(TariffContext tariffContext, TariffOceanFCLFreightCostServices tariffOceanFCLFreightCostServices)
        {
            this.tariffContext = tariffContext;
            this.tariffOceanFCLFreightCostServices = tariffOceanFCLFreightCostServices;
        }

        [Given(@"an ocean FCL freight cost tariff with the following properties")]
        public void GivenAnOceanFCLFreightCostTariffWithTheFollowingProperties(Table table)
        {
            tariffContext.OceanFCLFreightCost = tariffOceanFCLFreightCostServices.CreateInstance(table);
        }
        
        [When(@"create ocean FCL freight cost tariff")]
        public void WhenCreateOceanFCLFreightCostTariff()
        {
            tariffContext.OceanFCLFreightCost = APICaller.CallPost<TariffPM>(tariffContext.OceanFCLFreightCost, Urls.TariffsController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the ocean FCL freight cost tariff should create successfully")]
        public void ThenTheOceanFCLFreightCostTariffShouldCreateSuccessfully()
        {
            tariffContext.OceanFCLFreightCost.Should().NotBeNull();
            tariffContext.OceanFCLFreightCost.Id.Should().NotBeNull();
        }
    }
}
