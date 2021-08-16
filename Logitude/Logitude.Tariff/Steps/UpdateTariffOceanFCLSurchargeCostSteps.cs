using System;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using Logitude.Tariff.Models;
using Logitude.Tariff.Services;
using FluentAssertions;

namespace Logitude.Tariff.Steps
{
    [Binding]
    public class UpdateTariffOceanFCLSurchargeCostSteps
    {
        private readonly TariffContext tariffContext;
        private readonly TariffOceanFCLSurchargeCostServices tariffOceanFCLSurchargeCostServices;
        private TariffPM updatedTariff;

        public UpdateTariffOceanFCLSurchargeCostSteps(TariffContext tariffContext, TariffOceanFCLSurchargeCostServices tariffOceanFCLSurchargeCostServices, TariffPM updatedTariff)
        {
            this.tariffContext = tariffContext;
            this.tariffOceanFCLSurchargeCostServices = tariffOceanFCLSurchargeCostServices;
            this.updatedTariff = updatedTariff;
        }

        [Given(@"an ocean FCL surcharge cost tariff")]
        public void GivenAnOceanFCLSurchargeCostTariff()
        {
            tariffContext.OceanFCLSurchargeCost = APICaller.CallGet<TariffPM>(Urls.TariffSingle(TariffData.OceanFCLSurchargeCostId), UserTenant.Token).Data;
        }

        [Given(@"following ocean FCL surcharge cost tariff properties")]
        public void GivenFollowingOceanFCLSurchargeCostTariffProperties(Table table)
        {
            tariffOceanFCLSurchargeCostServices.UpdateInstance(table, tariffContext.OceanFCLSurchargeCost);
        }

        [When(@"update ocean FCL surcharge cost tariff")]
        public void WhenUpdateOceanFCLSurchargeCostTariff()
        {
            updatedTariff = APICaller.CallPut<TariffPM>(tariffContext.OceanFCLSurchargeCost, Urls.TariffsController, UserTenant.Token)?.Data;
        }

        [Then(@"the ocean FCL surcharge cost tariff should update successfully")]
        public void ThenTheOceanFCLSurchargeCostTariffShouldUpdateSuccessfully()
        {
            tariffOceanFCLSurchargeCostServices.AssertUpdate(tariffContext.OceanFCLSurchargeCost, updatedTariff);
        }
    }
}
