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
    public class UpdateTariffOceanFCLFreightCostSteps
    {

        private readonly TariffContext tariffContext;
        private readonly TariffOceanFCLFreightCostServices tariffOceanFCLFreightCostServices;
        private TariffPM updatedTariff;

        public UpdateTariffOceanFCLFreightCostSteps(TariffContext tariffContext, TariffOceanFCLFreightCostServices tariffOceanFCLFreightCostServices, TariffPM updatedTariff)
        {
            this.tariffContext = tariffContext;
            this.tariffOceanFCLFreightCostServices = tariffOceanFCLFreightCostServices;
            this.updatedTariff = updatedTariff;
        }

        [Given(@"an ocean FCL freight cost tariff")]
        public void GivenAnOceanFCLFreightCostTariff()
        {
            tariffContext.OceanFCLFreightCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.OceanFCLFreightCostId), UserTenant.Token).Data;
        }

        [Given(@"following ocean FCL freight cost tariff properties")]
        public void GivenFollowingOceanFCLFreightCostTariffProperties(Table table)
        {
            tariffOceanFCLFreightCostServices.UpdateInstance(table, tariffContext.OceanFCLFreightCost);
        }

        [When(@"update ocean FCL freight cost tariff")]
        public void WhenUpdateOceanFCLFreightCostTariff()
        {
            updatedTariff = APICaller.CallPut<TariffPM>(tariffContext.OceanFCLFreightCost, Urls.TariffsController, UserTenant.Token)?.Data;
        }

        [Then(@"the ocean FCL freight cost tariff should update successfully")]
        public void ThenTheOceanFCLFreightCostTariffShouldUpdateSuccessfully()
        {
            updatedTariff.Id.Should().NotBeNull();
            updatedTariff.Name.Should().Equals(tariffContext.OceanFCLFreightCost.Name);
            updatedTariff.Notes.Should().Equals(tariffContext.OceanFCLFreightCost.Notes);
        }
    }
}
