
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
   
   public partial class SupplierInvoiceItemsDescriptDataMapping: IMapping<SupplierInvoiceItemsDescriptPM, SupplierInvoiceItemsDescript>
   {

        public void CustomPMToPOCO(SupplierInvoiceItemsDescriptPM entityPM, SupplierInvoiceItemsDescript entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceCounterKey);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceItemLineNumber);


            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
              
                entityPOCO.DeclarationId = entityPM.DeclarationId;             
                entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;             
                entityPOCO.LineNumber = entityPM.LineNumber;               
                entityPOCO.Tenant = entityPM.Tenant;             
                entityPOCO.InvoiceItemLineNumber = entityPM.InvoiceItemLineNumber;
            }
        }

        public void CustomPOCOToPM(SupplierInvoiceItemsDescriptPM entityPM, SupplierInvoiceItemsDescript entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.TypeName);
            if (entityPOCO.TypeCode != null)
            {
                ProductNameTypeQueryService productNameTypeQueryService = new ProductNameTypeQueryService(entityPOCO.Tenant);
                ProductNameTypePM productNameType = productNameTypeQueryService.GetSingle(entityPOCO.TypeCode, false, true);
                entityPM.TypeName = productNameType.LocalName;
            }
        }
   }


}
   