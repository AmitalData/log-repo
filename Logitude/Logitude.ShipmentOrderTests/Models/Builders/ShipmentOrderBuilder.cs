using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.Infrastructure;
using Logitude.Test.Base.Models.LocationsPreparation;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentOrderTests.Models.Builders
{
    public class ShipmentOrderBuilder
    {
        private ShipmentOrder _shipmentOrder;

        public ShipmentOrderBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _shipmentOrder = new ShipmentOrder();
        }

        public ShipmentOrderBuilder OrderNumber(string orderNumber)
        {
            _shipmentOrder.OrderNumber = orderNumber;
            return this;
        }

        public ShipmentOrderBuilder PONumber(string poNumber)
        {
            _shipmentOrder.PONumber = poNumber;
            return this;
        }

        public ShipmentOrderBuilder Master(string master)
        {
            _shipmentOrder.Master = master;
            return this;
        }

        public ShipmentOrderBuilder House(string house)
        {
            _shipmentOrder.House = house;
            return this;
        }

        public ShipmentOrderBuilder DescriptionOfGoods(string descriptionOfGoods)
        {
            _shipmentOrder.DescriptionOfGoods = descriptionOfGoods;
            return this;
        }

        public ShipmentOrderBuilder CustomerReferences(string customerReferences)
        {
            _shipmentOrder.CustomerReferences = customerReferences;
            return this;
        }

        public ShipmentOrderBuilder ShipmentNumber(string shipmentNumber)
        {
            _shipmentOrder.ShipmentNumber = shipmentNumber;
            return this;
        }

        public ShipmentOrderBuilder CreateDate(DateTime createDate)
        {
            _shipmentOrder.CreateDate = createDate;
            return this;
        }

        public ShipmentOrderBuilder DirectionCode(string directionCode)
        {
            if (_shipmentOrder.Direction == null)
                _shipmentOrder.Direction = new Direction();

            _shipmentOrder.Direction.Code = directionCode;
            return this;
        }

        public ShipmentOrderBuilder TransportModeCode(string transportModeCode)
        {
            if (_shipmentOrder.TransportMode == null)
                _shipmentOrder.TransportMode = new TransportMode();

            _shipmentOrder.TransportMode.Code = transportModeCode;
            return this;
        }


        public ShipmentOrder Build()
        {
            ShipmentOrder result = _shipmentOrder;
            this.Reset();
            return result;
        }

        public ShipmentOrderBuilder WithModel(ShipmentOrder shipmentOrder)
        {
            _shipmentOrder = shipmentOrder;
            return this;
        }

        public ShipmentOrderBuilder WithDefualtValues()
        {
            _shipmentOrder = new ShipmentOrder
            {
                Shipper = new Card { Code = PartnersData.ShipperExportCode },
                Agent = new Card { Code = PartnersData.AgentCode },
                Incoterm = new Incoterm { Code = "LDE" },
                OriginPort = new Port { Id = LocationsData.PortAirJFKId },
                OrderNumber = (RandomGeneratorService.RandomGuid().Substring(15) + RandomGeneratorService.RandomNumber(5))
            };
            return this;
        }

    }
}
