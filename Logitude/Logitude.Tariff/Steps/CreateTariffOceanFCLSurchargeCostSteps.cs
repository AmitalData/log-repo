using System;
using TechTalk.SpecFlow;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using FluentAssertions;
using Logitude.Tariff.Models;
using Logitude.Tariff.Services;

namespace Logitude.Tariff.Steps
{
    [Binding]
    public class CreateTariffOceanFCLSurchargeCostSteps
    {
        private readonly TariffContext tariffContext;
        private readonly TariffOceanFCLSurchargeCostServices tariffOceanFCLSurchargeCostServices;
        private string insertException;

        public CreateTariffOceanFCLSurchargeCostSteps(TariffContext tariffContext, TariffOceanFCLSurchargeCostServices tariffOceanFCLSurchargeCostServices)
        {
            this.tariffContext = tariffContext;
            this.tariffOceanFCLSurchargeCostServices = tariffOceanFCLSurchargeCostServices;
        }


        [Given(@"an ocean FCL surcharge cost tariff with the following properties")]
        public void GivenAnOceanFCLSurchargeCostTariffWithTheFollowingProperties(Table table)
        {
            tariffContext.OceanFCLSurchargeCost = tariffOceanFCLSurchargeCostServices.CreateInstance(table);
        }

        [When(@"create ocean FCL surcharge cost tariff")]
        public void WhenCreateOceanFCLSurchargeCostTariff()
        {
            tariffContext.OceanFCLSurchargeCost = APICaller.CallPost<TariffPM>(tariffContext.OceanFCLSurchargeCost, Urls.TariffsController, UserTenant.Token)?.Data;
        }

        [Then(@"the ocean FCL surcharge cost tariff should create successfully")]
        public void ThenTheOceanFCLSurchargeCostTariffShouldCreateSuccessfully()
        {
            tariffContext.OceanFCLSurchargeCost.Should().NotBeNull();
            tariffContext.OceanFCLSurchargeCost.Id.Should().NotBeNull();
        }
    }
}
