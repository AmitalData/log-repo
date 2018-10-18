
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
   
   public partial class DeclarationTaxDataMapping: IMapping<DeclarationTaxPM, DeclarationTax>
   {

        public void CustomPMToPOCO(DeclarationTaxPM entityPM, DeclarationTax entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            entityPOCO.DeclarationId = entityPM.DeclarationId;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.TaxTypeCode);
            entityPOCO.TaxTypeCode = entityPM.TaxTypeCode;
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            entityPOCO.Tenant = entityPM.Tenant;
        }

        public void CustomPOCOToPM(DeclarationTaxPM entityPM, DeclarationTax entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.TaxTypeName);
            if (entityPOCO.TaxTypeCode != null)
            {
                ParagraphTypeQueryService paragraphTypeQueryService = new ParagraphTypeQueryService(entityPOCO.Tenant);
                ParagraphTypePM paragraphType = paragraphTypeQueryService.GetSingle(entityPOCO.TaxTypeCode, false, true);
                entityPM.TaxTypeName = paragraphType.LocalName;
            }
            entityPM.TaxToPay = (entityPOCO.TotalAmount == null ? 0 : entityPOCO.TotalAmount) + (entityPOCO.DeferredTaxAmount == null ? 0 : entityPOCO.DeferredTaxAmount);
            if (entityPM.TaxToPay == 0)
                entityPM.TaxToPay = null;
        }
   }


}
   