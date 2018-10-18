
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class SupplierInvoiceItemModVehicleDataMapping: IMapping<SupplierInvoiceItemModVehiclePM, SupplierInvoiceItemModVehicle>
   {

        public void CustomPMToPOCO(SupplierInvoiceItemModVehiclePM entityPM, SupplierInvoiceItemModVehicle entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceCounterKey);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceItemLineNumber); // moran 26.11.15 - Task 17424 
            CustomMappedPOCOProperties.Add(POCOPropertyNames.AdjustmentTypeCode);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;
                entityPOCO.InvoiceItemLineNumber = entityPM.InvoiceItemLineNumber; // moran 26.11.15 - Task 17424
                entityPOCO.AdjustmentTypeCode = entityPM.AdjustmentTypeCode;
                entityPOCO.Tenant = entityPM.Tenant;

            }
        }

        public void CustomPOCOToPM(SupplierInvoiceItemModVehiclePM entityPM, SupplierInvoiceItemModVehicle entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.AdjustmentTypeName);


            if (entityPOCO.AdjustmentTypeCode != null )
            {
                VehicleReductionTypeQueryService vehicleReductionTypeQueryService = new VehicleReductionTypeQueryService(entityPOCO.Tenant);
                VehicleReductionTypePM vehicleReductionType = vehicleReductionTypeQueryService.GetSingle(entityPOCO.AdjustmentTypeCode, false, true);
                if (vehicleReductionType != null)
                    entityPM.AdjustmentTypeName = vehicleReductionType.LocalName;
            }
        }
   }


}
   