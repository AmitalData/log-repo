using Logitude.Tariff.Models;
using Logitude.Tariff.Models.Builders;
using System;
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
                .SellerId("1-245684")
                .CurrencyId((string)dataTable.Currency)
                .StartDate((DateTime)dataTable.StartDate)
                .ExpirationDate((DateTime)dataTable.ExpirationDate)
                .TariffProductId("1-19827")
                .FreightChargeId("1-56535")
                .Notes((string)dataTable.Notes)
                .ContractNumber(Convert.ToString(dataTable.ContractNumber))
                .Build();
        }
    }
}
