using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using Logitude.ShipmentOrderTests.Models;
using Logitude.ShipmentOrderTests.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Logitude.Test.Base.Models.Infrastructure;
using FluentAssertions;

namespace Logitude.ShipmentOrderTests.Services
{
    public class ShipmentOrderServices
    {
        public ShipmentOrder CreateInstance(Table table)
        {
            dynamic dataTable = table.CreateDynamicInstance();
            return new ShipmentOrderBuilder()
                .WithDefualtValues()
                .DirectionCode((string)dataTable.Direction)
                .TransportModeCode((string)dataTable.TransportMode)
                .DescriptionOfGoods((string)dataTable.DescriptionOfGoods)
                .CustomerReferences((string)dataTable.CustomerReferences)
                .CreateDate(DateTime.Now)
                .ShipmentNumber((dataTable.ShipmentNumber).ToString())
                .PONumber((dataTable.PONumber).ToString())
                .Build();
        }

        public ShipmentOrder UpdateInstance(Table table, ShipmentOrder shipmentOrder)
        {
            dynamic dataTable = table.CreateDynamicInstance();
            return new ShipmentOrderBuilder()
                .WithModel(shipmentOrder)
                .DescriptionOfGoods((string)dataTable.DescriptionOfGoods)
                .CustomerReferences((string)dataTable.CustomerReferences)
                .Build();
        }

        public void AssertUpdate(ShipmentOrder shipmentOrder, ShipmentOrder updatedShipmentOrder)
        {
            updatedShipmentOrder.Id.Should().NotBeNull();
            updatedShipmentOrder.DescriptionOfGoods.Should().Equals(shipmentOrder.DescriptionOfGoods);
            updatedShipmentOrder.CustomerReferences.Should().Equals(shipmentOrder.CustomerReferences);
        }

    }
}
