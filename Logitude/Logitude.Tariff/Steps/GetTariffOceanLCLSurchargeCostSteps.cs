using Logitude.Tariff.Models;
using System;
using TechTalk.SpecFlow;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using FluentAssertions;

namespace Logitude.Tariff.Steps
{
    [Binding]
    public class GetTariffOceanLCLSurchargeCostSteps
    {

        private readonly TariffContext tariffContext;
        public GetTariffOceanLCLSurchargeCostSteps(TariffContext tariffContext)
        {
            this.tariffContext = tariffContext;
        }


        [When(@"get ocean LCL surcharge cost tariff with TariffId")]
        public void WhenGetOceanLCLSurchargeCostTariffWithTariffId()
        {
            tariffContext.OceanLCLSurchargeCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.OceanLCLSurchargeCostId), UserTenant.Token).Data;
        }

        [Then(@"ocean LCL surcharge cost tariff should be avaliable")]
        public void ThenOceanLCLSurchargeCostTariffShouldBeAvaliable()
        {
            tariffContext.OceanLCLSurchargeCost.Should().NotBeNull();
            tariffContext.OceanLCLSurchargeCost.Id.Should().NotBeNull();
        }
    }
}
