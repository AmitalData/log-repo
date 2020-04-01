using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.EntitiesInitializer
{
    public interface IEntityInitializer
    {
        object Create(EntityInitializerArguments args);
    }

    public class EntityInitializerArguments
    {
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string ShipmentLevelCode { get; set; }
        public double? PayableQuantity { get; set; }
        public double? PayableUnitPrice { get; set; }
        public double? ReceivableQuantity { get; set; }
        public double? ReceivableUnitPrice { get; set; }
    }

    public class EntityInitializerFactory
    {
        public IEntityInitializer GetInitializer(string entity)
        {
            switch (entity)
            {
                case "Shipment":
                    {
                        return new ShipmentInitializer();
                    }

                case "ShipmentReceivable":
                    {
                        return new ShipmentReceivableInitializer();
                    }
            }

            return null;
        }
    }
}
