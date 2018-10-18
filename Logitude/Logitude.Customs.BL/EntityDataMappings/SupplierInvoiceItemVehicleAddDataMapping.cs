
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class SupplierInvoiceItemVehicleAddDataMapping: IMapping<SupplierInvoiceItemVehicleAddPM, SupplierInvoiceItemVehicleAdd>
   {

        public void CustomPMToPOCO(SupplierInvoiceItemVehicleAddPM entityPM, SupplierInvoiceItemVehicleAdd entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceCounterKey);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceItemLineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);


            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;
                entityPOCO.InvoiceItemLineNumber = entityPM.InvoiceItemLineNumber;
                entityPOCO.LineNumber = entityPM.LineNumber;
            }
        }

        public void CustomPOCOToPM(SupplierInvoiceItemVehicleAddPM entityPM, SupplierInvoiceItemVehicleAdd entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   