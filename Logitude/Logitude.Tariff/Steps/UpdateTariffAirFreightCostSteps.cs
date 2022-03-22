using System;
using TechTalk.SpecFlow;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using Logitude.Tariff.Models;
using Logitude.Tariff.Services;
using FluentAssertions;

namespace Logitude.Tariff.Steps
{
    [Binding]
    public class UpdateTariffAirFreightCostSteps
    {
        private readonly TariffContext tariffContext;
        private readonly TariffAirFreightCostServices tariffAirFreightCostServices;
        private TariffPM updatedTariff;

        public UpdateTariffAirFreightCostSteps(TariffContext tariffContext, TariffAirFreightCostServices tariffAirFreightCostServices)
        {
            this.tariffContext = tariffContext;
            this.tariffAirFreightCostServices = tariffAirFreightCostServices;
        }

        [Given(@"an air freight cost tariff")]
        public void GivenAnAirFreightCostTariff()
        {
            tariffContext.AirFreightCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.AirFreightCostId), UserTenant.Token).Data;
        }

        [Given(@"following air freight cost tariff properties")]
        public void GivenFollowingAirFreightCostTariffProperties(Table table)
        {
            tariffAirFreightCostServices.UpdateInstance(table, tariffContext.AirFreightCost);
        }

        [When(@"update air freight cost tariff")]
        public void WhenUpdateAirFreightCostTariff()
        {
            updatedTariff = APICaller.CallPut<TariffPM>(tariffContext.AirFreightCost, Urls.TariffsController, UserTenant.Token)?.Data;
        }

        [Then(@"the air freight cost tariff should update successfully")]
        public void ThenTheAirFreightCostTariffShouldUpdateSuccessfully()
        {
            updatedTariff.Id.Should().NotBeNull();
            updatedTariff.Name.Should().Equals(tariffContext.AirFreightCost.Name);
            updatedTariff.Notes.Should().Equals(tariffContext.AirFreightCost.Notes);
        }
    }
}
