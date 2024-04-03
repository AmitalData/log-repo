using Logitude.Tariff.Models;
using Logitude.Tariff.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Billings;
using Logitude.Base.Models.Partners;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Logitude.Tariff.Models.PackageTypes;

namespace Logitude.Tariff.Services
{
    public class TariffOceanFCLFreightCostServices
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
                .ContainerType1Id(PackageTypesData.PackageTypeOceanPC2Id)
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
