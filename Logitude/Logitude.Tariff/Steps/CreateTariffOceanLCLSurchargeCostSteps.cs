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
    public class CreateTariffOceanLCLSurchargeCostSteps
    {
        private readonly TariffContext tariffContext;
        private readonly TariffOceanLCLSurchargeCostServices tariffOceanLCLSurchargeCostServices;
        private string insertException;

        public CreateTariffOceanLCLSurchargeCostSteps(TariffContext tariffContext, TariffOceanLCLSurchargeCostServices tariffOceanLCLSurchargeCostServices)
        {
            this.tariffContext = tariffContext;
            this.tariffOceanLCLSurchargeCostServices = tariffOceanLCLSurchargeCostServices;
        }

        [Given(@"an ocean LCL surcharge cost tariff with the following properties")]
        public void GivenAnOceanLCLSurchargeCostTariffWithTheFollowingProperties(Table table)
        {
            tariffContext.OceanLCLSurchargeCost = tariffOceanLCLSurchargeCostServices.CreateInstance(table);

        }

        [When(@"create ocean LCL surcharge cost tariff")]
        public void WhenCreateOceanLCLSurchargeCostTariff()
        {

            tariffContext.OceanLCLSurchargeCost = APICaller.CallPost<TariffPM>(tariffContext.OceanLCLSurchargeCost, Urls.TariffsController, UserTenant.Token)?.Data;
        }

        [Then(@"the ocean LCL surcharge cost tariff should create successfully")]
        public void ThenTheOceanLCLSurchargeCostTariffShouldCreateSuccessfully()
        {

            tariffContext.OceanLCLSurchargeCost.Should().NotBeNull();
            tariffContext.OceanLCLSurchargeCost.Id.Should().NotBeNull();

        }
    }
}
