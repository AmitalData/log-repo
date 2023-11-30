using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.Tariff.Models;
using Logitude.Tariff.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Billings;
using Logitude.Base.Models.Partners;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;


namespace Logitude.Tariff.Services
{
    public class TariffOceanLCLSurchargeCostDataPreparation
    {
        public void Prepar()
        {
            try
            {
                ApiResponse<TariffPM> response = APICaller.CallPost<TariffPM>(GetValidTariffPM(), Urls.TariffsController, UserTenant.Token);
                TariffDataMap(response.Data);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating Ocean LCL Surcharges Tariff Before Feature Run :" + e.InnerException);
            }
        }

        private TariffPM GetValidTariffPM()
        {
            return new TariffBuilder()
                   .WithDefualtValues()
                   .TypeCode("Ocean LCL Surcharge")
                   .Name("pre specflow name")
                   .SellerId(new TariffOceanLCLSurchargeCostServices().GetAgentId())
                   .CurrencyId("EUR")
                   .Notes("pre specflow notes")
                   .Surcharge1Id(BillingData.ChargeTypeOFTId)
                   .Surcharge1UOM(BillingData.MeasurementGRWTId)
                   .ContractNumber("2324232")
                   .Build();
        }

        private void TariffDataMap(TariffPM tariff)
        {
            TariffData.OceanLCLSurchargeCostId = tariff.Id;
        }
    }
}
