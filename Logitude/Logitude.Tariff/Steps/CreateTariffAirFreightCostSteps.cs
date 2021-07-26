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
    public class CreateTariffAirFreightCostSteps
    {
        private readonly TariffContext tariffContext;
        private readonly TariffAirFreightCostServices tariffAirFreightCostServices;

        public CreateTariffAirFreightCostSteps(TariffContext tariffContext, TariffAirFreightCostServices tariffAirFreightCostServices)
        {
            this.tariffContext = tariffContext;
            this.tariffAirFreightCostServices = tariffAirFreightCostServices;
        }

        [Given(@"a air freight cost tariff with the following properties")]
        public void GivenATariffWithTheFollowingProperties(Table table)
        {
            tariffContext.TariffAirFreightCost = tariffAirFreightCostServices.CreateInstance(table);
        }
        
        [When(@"create air freight cost tariff")]
        public void WhenCreateTariff()
        {
            tariffContext.TariffAirFreightCost = APICaller.CallPost<TariffPM>(tariffContext.TariffAirFreightCost, Urls.TariffsController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the air freight cost tariff should create successfully")]
        public void ThenTheTariffShouldCreateSuccessfully()
        {
            tariffContext.TariffAirFreightCost.Should().NotBeNull();
            tariffContext.TariffAirFreightCost.Id.Should().NotBeNull();
            TariffData.TariffAirFreightCost = tariffContext.TariffAirFreightCost;
        }
    }
}
