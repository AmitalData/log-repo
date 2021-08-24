using FluentAssertions;
using Logitude.Tariff.Models;
using Logitude.Tariff.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.LocationsPreparation;
using Logitude.Test.Base.Models.PackageTypesPreparation;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.Tariff.Services
{
    public class TariffOceanFCLSurchargeCostServices
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


        public string GetAgentId()
        {
            return DataPreparation.CreatePartnerForUserTenant(new PartnerParameters { TypeCode = "AG", Name = "TestAgentExport" });
        }

        public void AssertUpdate(TariffPM tariff, TariffPM updatedTariff)
        {
            updatedTariff.Id.Should().NotBeNull();
            updatedTariff.Name.Should().Equals(tariff.Name);
            updatedTariff.Notes.Should().Equals(tariff.Notes);
        }
    }
}
