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
            tariffContext.Tariff = tariffAirFreightCostServices.CreateInstance(table);
        }
        
        [When(@"create air freight cost tariff")]
        public void WhenCreateTariff()
        {
            tariffContext.Tariff = APICaller.CallPost<TariffPM>(tariffContext.Tariff, Urls.TariffsController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the air freight cost tariff should create successfully")]
        public void ThenTheTariffShouldCreateSuccessfully()
        {
            tariffContext.Tariff.Should().NotBeNull();
            tariffContext.Tariff.Id.Should().NotBeNull();
            TariffData.Tariff = tariffContext.Tariff;
        }
    }
}
