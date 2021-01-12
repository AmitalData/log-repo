using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Shipment.EntitiesInitializer;
using Logitude.IntegrationTest.Shipment.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Automation.EntitiesInitializer
{
    class ShipmentInitializer
    {

        ShipmentTestService service;
        IEntityInitializer initializer;
        EntityInitializerFactory factory;
        public async Task CreateShipment()
        {
            EntityInitializerArguments args = new EntityInitializerArguments()
            {
                DirectionId = "E",
                TransportModeId = "A",
                ShipmentLevelCode = "D",
            };

            initializer = factory.GetInitializer("Shipment");
            ShipmentPM entityPM = (ShipmentPM)initializer.Create(args);
            AutomationVariables.ShipmentId = await service.CreateShipment(entityPM);
        }
    }
}
