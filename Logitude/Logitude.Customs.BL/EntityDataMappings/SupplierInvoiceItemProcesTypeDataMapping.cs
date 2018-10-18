
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

    public partial class SupplierInvoiceItemProcesTypeDataMapping : IMapping<SupplierInvoiceItemProcesTypePM, SupplierInvoiceItemProcesType>
   {

       public void CustomPMToPOCO(SupplierInvoiceItemProcesTypePM entityPM, SupplierInvoiceItemProcesType entityPOCO)
       {
           CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceCounterKey);
           CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
           CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
           CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
           CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceItemLineNumber);
           if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
           {
               entityPOCO.DeclarationId = entityPM.DeclarationId;
               entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;
               entityPOCO.InvoiceItemLineNumber = entityPM.InvoiceItemLineNumber;
               entityPOCO.LineNumber = entityPM.LineNumber;
               entityPOCO.Tenant = entityPM.Tenant;
           }
       }

        public void CustomPOCOToPM(SupplierInvoiceItemProcesTypePM entityPM, SupplierInvoiceItemProcesType entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ProcessTypeName);
            if (entityPOCO.ProcessTypeCode != null)
            {
                ItemGovernmentProcedureTypeQueryService itemGovernmentProcedureTypeQueryService = new ItemGovernmentProcedureTypeQueryService(entityPOCO.Tenant);
                ItemGovernmentProcedureTypePM itemGovernmentProcedureType = itemGovernmentProcedureTypeQueryService.GetSingle(entityPOCO.ProcessTypeCode, false, true);
                entityPM.ProcessTypeName = itemGovernmentProcedureType.LocalName;
            }
        }
   }


}
   