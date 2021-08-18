using System;
using Logitude.ShipmentOrderTests.Models;
using Logitude.ShipmentOrderTests.Models.Builders;

namespace Logitude.ShipmentOrderTests.Services
{
    public class ShipmentOrderDataPreparation
    {
        public void Prepar()
        {
            try
            {
                ShipmentOrder shipmentOrder = new ShipmentOrderServices().Create(GetValidShipmentOrder());
                ShipmentOrderDataMap(shipmentOrder);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating ShipmentOrder Before Feature Run :" + e.InnerException);
            }
        }

        private ShipmentOrder GetValidShipmentOrder()
        {
            return new ShipmentOrderBuilder()
                  .WithDefualtValues()
                  .DirectionCode("E")
                  .TransportModeCode("O")
                  .DescriptionOfGoods("pre specflow description")
                  .CustomerReferences("pre specflow references")
                  .ShipmentNumber("222")
                  .PONumber("111")
                  .CreateDate(DateTime.Now)
                  .Build();
        }

        private void ShipmentOrderDataMap(ShipmentOrder shipmentOrder)
        {
            ShipmentOrderData.OrderNumber = shipmentOrder.OrderNumber;
        }
    }
}
