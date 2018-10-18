
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
   
   public partial class SupplierInvoiceItemsTaxDataMapping: IMapping<SupplierInvoiceItemsTaxPM, SupplierInvoiceItemsTax>
   {

        public void CustomPMToPOCO(SupplierInvoiceItemsTaxPM entityPM, SupplierInvoiceItemsTax entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceCounterKey);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.TaxTypeCode);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
              
                entityPOCO.DeclarationId = entityPM.DeclarationId;            
                entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;            
                entityPOCO.LineNumber = entityPM.LineNumber;            
                entityPOCO.TaxTypeCode = entityPM.TaxTypeCode;               
                entityPOCO.Tenant = entityPM.Tenant;

            }
        }

        public void CustomPOCOToPM(SupplierInvoiceItemsTaxPM entityPM, SupplierInvoiceItemsTax entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.TaxTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.TradeAgreementTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.AlternateMeasurementUnitName);
            CustomMappedPMProperties.Add(PMPropertyNames.MeasurementUnitName);

            if (entityPOCO.TaxTypeCode != null)
            {
                ParagraphTypeQueryService paragraphTypeQueryService = new ParagraphTypeQueryService(entityPOCO.Tenant);
                ParagraphTypePM paragraphType = paragraphTypeQueryService.GetSingle(entityPOCO.TaxTypeCode, false, true);
                entityPM.TaxTypeName = paragraphType.LocalName;
            }

            if (entityPOCO.TradeAgreementTypeCode != null)
            {
                TradeAgreementQueryService tradeAgreementQueryService = new TradeAgreementQueryService(entityPOCO.Tenant);
                TradeAgreementPM tradeAgreement = tradeAgreementQueryService.GetSingle(entityPOCO.TradeAgreementTypeCode, false, true);
                entityPM.TradeAgreementTypeName = tradeAgreement.LocalName;
            }

            if (entityPOCO.AlternateMeasurementUnitCode != null)
            {
                MeasurmentUnitQueryService measurmentUnitQueryService = new MeasurmentUnitQueryService(entityPOCO.Tenant);
                MeasurmentUnitPM measurmentUnit = measurmentUnitQueryService.GetSingle(entityPOCO.AlternateMeasurementUnitCode, false, true);
                entityPM.AlternateMeasurementUnitName = measurmentUnit.LocalName;
            }

            if (entityPOCO.MeasurementUnitCode != null)
            {
                MeasurmentUnitQueryService measurmentUnitQueryService = new MeasurmentUnitQueryService(entityPOCO.Tenant);
                MeasurmentUnitPM measurmentUnit = measurmentUnitQueryService.GetSingle(entityPOCO.MeasurementUnitCode, false, true);
                entityPM.MeasurementUnitName = measurmentUnit.LocalName;
            }
        }
   }


}
   