using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.Tariff.Models;
using Logitude.Tariff.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;

namespace Logitude.Tariff.Services
{
    public class TariffAirSurchargeCostDataPreparation
    {
        public void Prepar()
        {
            TariffPM tariff = null;
            try
            {
                tariff = GetValidTariffPM();
                ApiResponse<TariffPM> response = APICaller.CallPost<TariffPM>(tariff, Urls.TariffsController, UserTenant.Token);
                TariffDataMap(response.Data);
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("Tariff surcharge seller should be unique"))
                {
                    TariffDataMap(GetAirSurchargesCostTariffBySellerId(tariff.SellerId));
                }
                else
                    throw new InvalidOperationException("Failed Creating Air Surcharges Tariff Before Feature Run :" + e.InnerException);
            }
        }

        public TariffPM GetAirSurchargesCostTariffBySellerId(string sellerId)
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("SellerId")
                .Filter1Operator("equals")
                .Filter1Value(sellerId)
                .Filter1Name("TypeCode")
                .Filter1Operator("equals")
                .Filter1Value("ASC").Build();

            ApiResponse<IEnumerable<TariffPM>> response = APICaller.CallGetByFilters<IEnumerable<TariffPM>>(Urls.TariffViews, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }

        private TariffPM GetValidTariffPM()
        {

            return new TariffBuilder()
                    .WithDefualtValues()
                    .TypeCode("Air Surcharge")
                    .Name("pre specflow name")
                    .SellerId(PartnersData.AirlineAAId)
                    .CurrencyId("EUR")
                    .Notes("pre specflow notes")
                    .Surcharge1Id(BillingData.ChargeTypeAFTId)
                    .Surcharge1UOM(BillingData.MeasurementGRWTId)
                    .ContractNumber("2324232")
                    .Build();
        }

        private void TariffDataMap(TariffPM tariff)
        {
            TariffData.AirSurchargeCostId = tariff.Id;
        }
    }
}
