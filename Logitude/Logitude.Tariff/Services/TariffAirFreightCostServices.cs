using Logitude.Tariff.Models;
using Logitude.Tariff.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.BillingsPreparation;
using Logitude.Base.Models.PartnersPreparation;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.Tariff.Services
{
    public class TariffAirFreightCostServices
    {
        public TariffPM CreateInstance(Table tariffTable)
        {
            dynamic dataTable = tariffTable.CreateDynamicInstance();
            return new TariffBuilder()
                .WithDefualtValues()
                .TypeCode((string)dataTable.Freight)
                .Name((string)dataTable.Name)
                .SellerId(PartnersData.AirlineAAId)
                .CurrencyId((string)dataTable.Currency)
                .StartDate((DateTime)dataTable.StartDate)
                .ExpirationDate((DateTime)dataTable.ExpirationDate)
                .TariffProductId(GetTariffProductIdByName((string)dataTable.Product))
                .FreightChargeId(BillingData.ChargeTypeAFTId)
                .Notes((string)dataTable.Notes)
                .ContractNumber(Convert.ToString(dataTable.ContractNumber))
                .Build();
        }

        public TariffPM UpdateInstance(Table tariffTable, TariffPM tariff)
        {
            dynamic dataTable = tariffTable.CreateDynamicInstance();
            return new TariffBuilder()
                .WithModel(tariff)
                .Name((string)dataTable.Name)
                .Notes((string)dataTable.Notes)
                .Build();
        }

        public string GetTariffProductIdByName(string tariffProductname)
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("Name")
                .Filter1Operator("equals")
                .Filter1Value(tariffProductname).Build();

            ApiResponse<IEnumerable<dynamic>> response = APICaller.CallGetByFilters<IEnumerable<dynamic>>(Urls.TariffProductViews, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?["Id"];
        }
    }
}
