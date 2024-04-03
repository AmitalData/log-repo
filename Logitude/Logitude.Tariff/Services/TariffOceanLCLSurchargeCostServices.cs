using FluentAssertions;
using Logitude.Tariff.Models;
using Logitude.Tariff.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Billings;
using Logitude.Base.Models.Locations;
using Logitude.Base.Models.Partners;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.Tariff.Services
{
    public class TariffOceanLCLSurchargeCostServices
    {
        public TariffPM CreateInstance(Table tariffTable)
        {
            dynamic dataTable = tariffTable.CreateDynamicInstance();
            return new TariffBuilder()
                .WithDefualtValues()
                .TypeCode((string)dataTable.Freight)
                .Name((string)dataTable.Name)
                .SellerId(GetAgentId())
                .CurrencyId((string)dataTable.Currency)
                .Notes((string)dataTable.Notes)
                .Surcharge1Id(BillingData.ChargeTypeOFTId)
                .Surcharge1UOM(BillingData.MeasurementGRWTId)
                .ContractNumber(Convert.ToString(dataTable.ContractNumber))
                .Build();
        }

        public string GetAgentId()
        {
            return DataPreparation.CreatePartnerForUserTenant(new PartnerParameters { TypeCode = "AG", Name = "TestAgentExport" });
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

        public void AssertUpdate(TariffPM tariff, TariffPM updatedTariff)
        {
            updatedTariff.Id.Should().NotBeNull();
            updatedTariff.Name.Should().Equals(tariff.Name);
            updatedTariff.Notes.Should().Equals(tariff.Notes);
        }

    }
}
