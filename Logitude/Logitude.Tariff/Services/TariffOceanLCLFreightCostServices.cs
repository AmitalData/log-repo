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
    public class TariffOceanLCLFreightCostServices
    {
        public TariffPM CreateInstance(Table tariffTable)
        {
            dynamic dataTable = tariffTable.CreateDynamicInstance();
            return new TariffBuilder()
                .WithDefualtValues()
                .TypeCode((string)dataTable.Freight)
                .Name((string)dataTable.Name)
                .SellerId(PartnersData.ShippingLineMAEUId)
                .CurrencyId((string)dataTable.Currency)
                .StartDate((DateTime)dataTable.StartDate)
                .ExpirationDate((DateTime)dataTable.ExpirationDate)
                .FreightChargeId(BillingData.ChargeTypeOFTId)
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

    }
}
