
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
   
   public partial class SupplierInvoiceItemVehicleDataMapping: IMapping<SupplierInvoiceItemVehiclePM, SupplierInvoiceItemVehicle>
   {

        public void CustomPMToPOCO(SupplierInvoiceItemVehiclePM entityPM, SupplierInvoiceItemVehicle entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceCounterKey);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceItemLineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);


            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;
                entityPOCO.InvoiceItemLineNumber = entityPM.InvoiceItemLineNumber;
                entityPOCO.LineNumber = entityPM.LineNumber;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(SupplierInvoiceItemVehiclePM entityPM, SupplierInvoiceItemVehicle entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.RichbitFileStatus);


            if (entityPOCO.RichbitFileNumber != null || entityPOCO.VehicleChassisNumber != null)
            {
                VehicleQueryService vehicleQueryService = new VehicleQueryService(entityPOCO.Tenant);
                VehiclePM vehicle = vehicleQueryService.GetVehicleByVehicleChassisNumberOrRichbitFileNumber(entityPOCO.VehicleChassisNumber, entityPOCO.RichbitFileNumber, entityPOCO.Tenant);
                if (vehicle != null)
                    entityPM.RichbitFileStatus = vehicle.StatusName;
            }
        }
   }


}
   