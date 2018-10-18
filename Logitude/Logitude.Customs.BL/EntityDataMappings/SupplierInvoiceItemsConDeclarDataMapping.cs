
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

    public partial class SupplierInvoiceItemsConDeclarDataMapping : IMapping<SupplierInvoiceItemsConDeclarPM, SupplierInvoiceItemsConDeclar>
   {

        public void CustomPMToPOCO(SupplierInvoiceItemsConDeclarPM entityPM, SupplierInvoiceItemsConDeclar entityPOCO)
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

        public void CustomPOCOToPM(SupplierInvoiceItemsConDeclarPM entityPM, SupplierInvoiceItemsConDeclar entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.DeclarationTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.QuantityTypeName);

            if (entityPOCO.DeclarationTypeCode != null)
            {
                LeadDocumentTypeQueryService leadDocumentTypeQueryService = new LeadDocumentTypeQueryService(entityPOCO.Tenant);
                LeadDocumentTypePM leadDocumentType = leadDocumentTypeQueryService.GetSingle(entityPOCO.DeclarationTypeCode, false, true);
                entityPM.DeclarationTypeName = leadDocumentType.LocalName;
            }

            if (entityPOCO.QuantityTypeCode != null)
            {
                MeasurmentUnitQueryService measurmentUnitQueryService = new MeasurmentUnitQueryService(entityPOCO.Tenant);
                MeasurmentUnitPM measurmentUnit = measurmentUnitQueryService.GetSingle(entityPOCO.QuantityTypeCode, false, true);
                entityPM.QuantityTypeName = measurmentUnit.LocalName;
            }
        }
   }


}
   