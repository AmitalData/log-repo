
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.EntityPMs; 
using Logitude.WarehouseLib.Data;
using Simplog.Server.Infrastructure;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.WarehouseLib.BL.EntityDataMappings
{
   
   public partial class WarehouseEntryPackageDataMapping: IMapping<WarehouseEntryPackagePM, WarehouseEntryPackage>
   {


        public void CustomPMToPOCO(WarehouseEntryPackagePM entityPM, WarehouseEntryPackage entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);

            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                UpdateInUseShipmentPackages(entityPM, entityPOCO);
            }
        }

        private static void UpdateInUseShipmentPackages(WarehouseEntryPackagePM entityPM, WarehouseEntryPackage entityPOCO)
        {
            int ChangedQuantity = entityPM.Quantity - entityPOCO.Quantity;
            if (ChangedQuantity != 0)
            {
                ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(entityPM.Tenant);
                ShipmentPackage shipmentPackage = shipmentPackageRepository.GetSingleShipmentPackage(entityPM.ShipmentPackageId, entityPM.Tenant);
                if (shipmentPackage != null)
                {
                    shipmentPackage.InUse += ChangedQuantity;
                    if (shipmentPackage.InUse > shipmentPackage.Quantity) shipmentPackage.InUse = shipmentPackage.Quantity;
                }
                shipmentPackageRepository.Update(shipmentPackage);
                shipmentPackageRepository.SubmitChanges();
            }
        }

        public void CustomPOCOToPM(WarehouseEntryPackagePM entityPM, WarehouseEntryPackage entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   