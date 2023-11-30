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
    public class UpdateTariffOceanLCLSurchargeCostSteps
    {

        private readonly TariffContext tariffContext;
        private readonly TariffOceanLCLSurchargeCostServices tariffOceanLCLSurchargeCostServices;
        private TariffPM updatedTariff;

        public UpdateTariffOceanLCLSurchargeCostSteps(TariffContext tariffContext, TariffOceanLCLSurchargeCostServices tariffOceanLCLSurchargeCostServices, TariffPM updatedTariff)
        {
            this.tariffContext = tariffContext;
            this.tariffOceanLCLSurchargeCostServices = tariffOceanLCLSurchargeCostServices;
            this.updatedTariff = updatedTariff;
        }

        [Given(@"an ocean LCL surcharge cost tariff")]
        public void GivenAnOceanLCLSurchargeCostTariff()
        {
            tariffContext.OceanLCLSurchargeCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.OceanLCLSurchargeCostId), UserTenant.Token).Data;
        }

        [Given(@"following ocean LCL surcharge cost tariff properties")]
        public void GivenFollowingOceanLCLSurchargeCostTariffProperties(Table table)
        {
            tariffOceanLCLSurchargeCostServices.UpdateInstance(table, tariffContext.OceanLCLSurchargeCost);
        }

        [When(@"update ocean LCL surcharge cost tariff")]
        public void WhenUpdateOceanLCLSurchargeCostTariff()
        {
            updatedTariff = APICaller.CallPut<TariffPM>(tariffContext.OceanLCLSurchargeCost, Urls.TariffsController, UserTenant.Token)?.Data;
        }

        [Then(@"the ocean LCL surcharge cost tariff should update successfully")]
        public void ThenTheOceanLCLSurchargeCostTariffShouldUpdateSuccessfully()
        {
            tariffOceanLCLSurchargeCostServices.AssertUpdate(tariffContext.OceanLCLSurchargeCost, updatedTariff);
        }
    }
}
