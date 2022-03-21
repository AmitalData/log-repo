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
    public class CreateTariffAirFreightCostSteps
    {
        private readonly TariffContext tariffContext;
        private readonly TariffAirFreightCostServices tariffAirFreightCostServices;

        public CreateTariffAirFreightCostSteps(TariffContext tariffContext, TariffAirFreightCostServices tariffAirFreightCostServices)
        {
            this.tariffContext = tariffContext;
            this.tariffAirFreightCostServices = tariffAirFreightCostServices;
        }

        [Given(@"an air freight cost tariff with the following properties")]
        public void GivenATariffWithTheFollowingProperties(Table table)
        {
            tariffContext.AirFreightCost = tariffAirFreightCostServices.CreateInstance(table);
        }
        
        [When(@"create air freight cost tariff")]
        public void WhenCreateTariff()
        {
            tariffContext.AirFreightCost = APICaller.CallPost<TariffPM>(tariffContext.AirFreightCost, Urls.TariffsController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the air freight cost tariff should create successfully")]
        public void ThenTheTariffShouldCreateSuccessfully()
        {
            tariffContext.AirFreightCost.Should().NotBeNull();
            tariffContext.AirFreightCost.Id.Should().NotBeNull();
        }
    }
}
