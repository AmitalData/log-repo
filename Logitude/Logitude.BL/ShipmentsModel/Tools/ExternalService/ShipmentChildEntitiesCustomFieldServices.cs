using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.Server.Tools.CustomFields;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.ExternalService
{

    public class ShipmentChildEntitiesCustomFieldServices
    {
        public ShipmentPM shipment;
        private ShipmentServiceInitializer shipmentServiceInitializer;
        public ShipmentChildEntitiesCustomFieldServices(ShipmentPM shipment , ShipmentServiceInitializer shipmentServiceInitializer)
        {
            this.shipment = shipment;
            this.shipmentServiceInitializer = shipmentServiceInitializer;
        }



        public void Save()
        {
            this.SaveShipmentPackages();
            this.SaveShipmentReceivables();
            this.SaveShipmentPayables();
        }


        private void SaveShipmentPackages()
        {
            var shipmentPackages = shipmentServiceInitializer.ShipmentPackagesChangeSet == null ? shipment.ShipmentPackages : shipmentServiceInitializer.ShipmentPackagesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            UpdateCustomFields("ShipmentPackage", shipmentPackages.Cast<object>().ToList());
        }


        private void SaveShipmentReceivables()
        {
            var shipmentReceivables = shipmentServiceInitializer.ShipmentReceivablesChangeSet == null ? shipment.ShipmentReceivables : shipmentServiceInitializer.ShipmentReceivablesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            UpdateCustomFields("ShipmentReceivable", shipmentReceivables.Cast<object>().ToList());
        }


        private void SaveShipmentPayables()
        {
            var shipmentPayables = shipmentServiceInitializer.ShipmentPayablesChangeSet == null ? shipment.ShipmentPayables : shipmentServiceInitializer.ShipmentPayablesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            UpdateCustomFields("ShipmentPayable", shipmentPayables.Cast<object>().ToList());
        }



        private void UpdateCustomFields(string childObjectTableName ,List<object> childEntities)
        {
            new ChildEntitiesCustomFieldService().Update(new ChildEntitiesCustomFieldArgs()
            {
                Tenant = shipment.Tenant,
                EntityId = shipment.Id,
                ObjectTableName = "Shipment",
                ChildObjectTableName = childObjectTableName,
                ChildEntities = childEntities
            }); ;
        }
    }
}
