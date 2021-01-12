using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class MapCompositionBehaviour: IServiceBehaviour
    {
        private ShipmentServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;

            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            if (!initializer.IsNewEntity && initializer.IsMappingComposition)
            {
                foreach (var itemPM in initializer.EntityPM.ShipmentPackages)
                {
                    itemPM.InsideShipmentPackagesChangeSet = itemPM.InsideShipmentPackages;
                    itemPM.ShipmentPackageItemsChangeSet = itemPM.ShipmentPackageItems;
                    itemPM.ShipmentPackageHarmonizesChangeSet = itemPM.ShipmentPackageHarmonizes;

                    foreach (var item in itemPM.InsideShipmentPackages)
                    {
                        item.InsidePackageHarmonizesChangeSet = item.InsidePackageHarmonizes;
                    }
                }

                foreach (var itemPM in initializer.EntityPM.ShipmentPickUps)
                {
                    itemPM.ShipmentPickUpPackagesChangeSet = itemPM.ShipmentPickUpDeliveryPackages;

                    foreach (var item in itemPM.ShipmentPickUpDeliveryPackages)
                    {
                        item.PickUpDeliveryPackageHarmonizesChangeSet = item.PickUpDeliveryPackageHarmonizes;
                    }
                }

                foreach (var itemPM in initializer.EntityPM.ShipmentDeliveries)
                {
                    itemPM.ShipmentDeliveryPackagesChangeSet = itemPM.ShipmentPickUpDeliveryPackages;
                }

                initializer.ShipmentPackagesChangeSet = initializer.EntityPM.ShipmentPackages;
                initializer.ShipmentOrderPackagesChangeSet = initializer.EntityPM.ShipmentOrderPackages;
                initializer.ShipmentPickUpsChangeSet = initializer.EntityPM.ShipmentPickUps;
                initializer.ShipmentDeliveriesChangeSet = initializer.EntityPM.ShipmentDeliveries;
                initializer.ShipmentReceivablesChangeSet = initializer.EntityPM.ShipmentReceivables;
                initializer.ShipmentPayablesChangeSet = initializer.EntityPM.ShipmentPayables;
                initializer.ShipmentFollowUpsChangeSet = initializer.EntityPM.FollowUps;
                initializer.ShipmentAWBPrintOnliesChangeSet = initializer.EntityPM.ShipmentAWBPrintOnlies;
                initializer.ShipmentConsoleShipmentsChangeSet = initializer.EntityPM.ShipmentConsoleShipments;
                initializer.ShipmentCarrierStatusesChangeSet = initializer.EntityPM.ShipmentCarrierStatuses;
                initializer.AWBOCIPMChangeSet = initializer.EntityPM.AWBOCIPMs;
                initializer.ShipmentCommoditiesChangeSet = initializer.EntityPM.ShipmentCommodities;
                initializer.ShipmentAssembliesChangeSet = initializer.EntityPM.ShipmentAssemblies;
                initializer.ShipmentStoragePricingsChangeSet = initializer.EntityPM.ShipmentStoragePricings;
            }
        }
    }
}
