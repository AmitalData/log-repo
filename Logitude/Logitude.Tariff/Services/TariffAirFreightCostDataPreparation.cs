using System;
using Logitude.Tariff.Models;
using Logitude.Tariff.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.BillingsPreparation;
using Logitude.Base.Models.PartnersPreparation;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;

namespace Logitude.Tariff.Services
{
    public class TariffAirFreightCostDataPreparation
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
                throw new InvalidOperationException("Failed Creating Tariff Before Feature Run :" + e.InnerException);
            }
        }

        private TariffPM GetValidTariffPM()
        {
            return new TariffBuilder()
                   .WithDefualtValues()
                   .TypeCode("Air")
                   .Name("pre specflow name")
                   .SellerId(PartnersData.AirlineAAId)
                   .CurrencyId("EUR")
                   .StartDate(DateTime.Now)
                   .ExpirationDate(DateTime.Now.AddMonths(1))
                   .TariffProductId(new TariffAirFreightCostServices().GetTariffProductIdByName("General"))
                   .FreightChargeId(BillingData.ChargeTypeAFTId)
                   .Notes("pre specflow notes")
                   .ContractNumber("2324232")
                   .Build();
        }

        private void TariffDataMap(TariffPM tariff)
        {
            TariffData.AirFreightCostId = tariff.Id;
        }
    }
}
