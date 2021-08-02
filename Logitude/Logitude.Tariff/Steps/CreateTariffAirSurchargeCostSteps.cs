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
    public class CreateTariffAirSurchargeCostSteps
    {
        private readonly TariffContext tariffContext;
        private readonly TariffAirSurchargeCostServices tariffAirSurchargeCostServices;
        private string insertException;

        public CreateTariffAirSurchargeCostSteps(TariffContext tariffContext, TariffAirSurchargeCostServices tariffAirSurchargeCostServices)
        {
            this.tariffContext = tariffContext;
            this.tariffAirSurchargeCostServices = tariffAirSurchargeCostServices;
        }

        [Given(@"an air surcharge cost tariff with the following properties")]
        public void GivenAnAirSurchargeCostTariffWithTheFollowingProperties(Table table)
        {
            tariffContext.AirSurchargeCost = tariffAirSurchargeCostServices.CreateInstance(table);
        }

        [When(@"create air surcharge cost tariff")]
        public void WhenCreateAirSurchargeCostTariff()
        {
            try
            {
                tariffContext.AirSurchargeCost = APICaller.CallPost<TariffPM>(tariffContext.AirSurchargeCost, Urls.TariffsController, UserTenant.Token)?.Data;
            }
            catch (Exception e)
            {
                insertException = e.InnerException.Message;
            }

        }

        [Then(@"the air surcharge cost tariff should create successfully")]
        public void ThenTheAirSurchargeCostTariffShouldCreateSuccessfully()
        {
            if (string.IsNullOrEmpty(insertException))
            {
                tariffContext.AirSurchargeCost.Should().NotBeNull();
                tariffContext.AirSurchargeCost?.Id.Should().NotBeNull();
            }
            else
            {
                insertException.Should().Contain("Tariff surcharge seller should be unique");
            }

        }

    }
}
