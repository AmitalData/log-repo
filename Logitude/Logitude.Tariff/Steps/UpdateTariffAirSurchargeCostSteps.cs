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
    public class UpdateTariffAirSurchargeCostSteps
    {
        private readonly TariffContext tariffContext;
        private readonly TariffAirSurchargeCostServices tariffAirSurchargeCostServices;
        private TariffPM updatedTariff;

        public UpdateTariffAirSurchargeCostSteps(TariffContext tariffContext, TariffAirSurchargeCostServices tariffAirSurchargeCostServices)
        {
            this.tariffContext = tariffContext;
            this.tariffAirSurchargeCostServices = tariffAirSurchargeCostServices;
        }
        [Given(@"an air surcharge cost tariff")]
        public void GivenAnAirSurchargesCostTariff()
        {
            tariffContext.AirSurchargeCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.AirSurchargeCostId), UserTenant.Token).Data;
        }
        
        [Given(@"following air surcharge cost tariff properties")]
        public void GivenFollowingAirSurchargesCostTariffProperties(Table table)
        {
            tariffAirSurchargeCostServices.UpdateInstance(table, tariffContext.AirSurchargeCost);
        }
        
        [When(@"update air surcharge cost tariff")]
        public void WhenUpdateAirSurchargesCostTariff()
        {
            updatedTariff = APICaller.CallPut<TariffPM>(tariffContext.AirSurchargeCost, Urls.TariffsController, UserTenant.Token)?.Data;
        }
        
        [Then(@"the air surcharge cost tariff should update successfully")]
        public void ThenTheAirSurchargesCostTariffShouldUpdateSuccessfully()
        {
            tariffAirSurchargeCostServices.AssertUpdate(tariffContext.AirSurchargeCost, updatedTariff);
        }

    }
}
