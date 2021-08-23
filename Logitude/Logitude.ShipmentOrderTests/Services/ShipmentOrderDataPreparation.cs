using System;
using Logitude.ShipmentOrderTests.Models;
using Logitude.ShipmentOrderTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;

namespace Logitude.ShipmentOrderTests.Services
{
    public class ShipmentOrderDataPreparation
    {
        public void Prepar()
        {

            ShipmentOrder shipmentOrder = null;
            try
            {
                shipmentOrder = GetValidShipmentOrder();
                shipmentOrder.OrderNumber = "28467";
                ApiResponse<ShipmentOrder> response = APICaller.CallPost<ShipmentOrder>(shipmentOrder, Urls.ShipmentOrderController, UserTenant.Token);
                ShipmentOrderDataMap(response.Data);
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_ShipmentOrders_Tenant_OrderNumber'"))
                {
                    ShipmentOrderDataMap(GetShipmentOrderByNumber(shipmentOrder.OrderNumber));
                }
                else
                    throw new InvalidOperationException("Failed Creating ShipmentOrder Before Feature Run :" + e.InnerException);
            }

        }

        public ShipmentOrder GetShipmentOrderByNumber(string orderNumber)
        {
            return APICaller.CallGet<ShipmentOrder>(Urls.ShipmentOrderSingle(orderNumber), UserTenant.Token).Data;
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
