using Logitude.Tariff.Models;
using System;
using TechTalk.SpecFlow;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using FluentAssertions;


namespace Logitude.Tariff.Steps
{
    [Binding]
    public class GetTariffOceanFCLSurchargeCostSteps
    {
        private readonly TariffContext tariffContext;
        public GetTariffOceanFCLSurchargeCostSteps(TariffContext tariffContext)
        {
            this.tariffContext = tariffContext;
        }

        [When(@"get ocean FCL surcharge cost tariff with TariffId")]
        public void WhenGetOceanFCLSurchargeCostTariffWithTariffId()
        {
            tariffContext.OceanFCLSurchargeCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.OceanFCLSurchargeCostId), UserTenant.Token).Data;
        }

        [Then(@"ocean FCL surcharge cost tariff should be avaliable")]
        public void ThenOceanFCLSurchargeCostTariffShouldBeAvaliable()
        {
            tariffContext.OceanFCLSurchargeCost.Should().NotBeNull();
            tariffContext.OceanFCLSurchargeCost.Id.Should().NotBeNull();
        }
    }
}
