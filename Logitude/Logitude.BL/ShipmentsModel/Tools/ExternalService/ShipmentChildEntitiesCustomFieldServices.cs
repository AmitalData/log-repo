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
            Update("ShipmentPackage", ShipmentPackages.Cast<object>().ToList());
            Update("ShipmentReceivable", ShipmentReceivables.Cast<object>().ToList());
            Update("ShipmentPayable", ShipmentPayables.Cast<object>().ToList());
            Update("ShipmentPickUpDelivery", ShipmentPickUps.Cast<object>().ToList());
            Update("ShipmentPickUpDelivery", ShipmentDeliveries.Cast<object>().ToList());
            //var tasks = BuildTasks();
            //foreach (Task task in tasks)
            //{
            //    task.Start();
            //}
            //Task.WaitAll(tasks);
        }

        public Task[] BuildTasks()
        {
            List<Task> result = new List<Task>();
            result.Add(new Task(() => Update("ShipmentPackage", ShipmentPackages.Cast<object>().ToList())));
            result.Add(new Task(() => Update("ShipmentReceivable", ShipmentReceivables.Cast<object>().ToList())));
            result.Add(new Task(() => Update("ShipmentPayable", ShipmentPayables.Cast<object>().ToList())));
            result.Add(new Task(() => Update("ShipmentPickUpDelivery", ShipmentPickUps.Cast<object>().ToList())));
            result.Add(new Task(() => Update("ShipmentPickUpDelivery", ShipmentDeliveries.Cast<object>().ToList())));
            return result.ToArray();
        }

        private void Update(string childObjectTableName ,List<object> childEntities)
        {
            new ChildEntitiesCustomFieldService().Update(new ChildEntitiesCustomFieldArgs()
            {
                Tenant = shipment.Tenant,
                EntityId = shipment.Id,
                ObjectTableName = "Shipment",
                ChildObjectTableName = childObjectTableName,
                ChildEntities = childEntities
            }); 
        }

        public List<ShipmentPackagePM> ShipmentPackages
        {
            get
            {
                if (shipmentServiceInitializer.ShipmentPackagesChangeSet == null) return shipment.ShipmentPackages;
                return shipmentServiceInitializer.ShipmentPackagesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.None).ToList();
            }
        }
        public List<ShipmentReceivablePM> ShipmentReceivables
        {
            get
            {
                if (shipmentServiceInitializer.ShipmentReceivablesChangeSet == null) return shipment.ShipmentReceivables;
                return shipmentServiceInitializer.ShipmentReceivablesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.None).ToList();
            }
        }

        public List<ShipmentPayablePM> ShipmentPayables
        {
            get
            {
                if (shipmentServiceInitializer.ShipmentPayablesChangeSet == null) return shipment.ShipmentPayables;
                return shipmentServiceInitializer.ShipmentPayablesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.None).ToList();
            }
        }

        public List<ShipmentPickUpPM> ShipmentPickUps
        {
            get
            {
                if (shipmentServiceInitializer.ShipmentPickUpsChangeSet == null) return shipment.ShipmentPickUps;
                return shipmentServiceInitializer.ShipmentPickUpsChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.None).ToList();
            }
        }


        public List<ShipmentDeliveryPM> ShipmentDeliveries
        {
            get
            {
                if (shipmentServiceInitializer.ShipmentDeliveriesChangeSet == null) return shipment.ShipmentDeliveries;
                return shipmentServiceInitializer.ShipmentDeliveriesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.None).ToList();
            }
        }


    }
}
