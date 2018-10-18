
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
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsDocumentsDefinitionDataMapping: IMapping<CustomsDocumentsDefinitionPM, CustomsDocumentsDefinition>
   {

        public void CustomPMToPOCO(CustomsDocumentsDefinitionPM entityPM, CustomsDocumentsDefinition entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(CustomsDocumentsDefinitionPM entityPM, CustomsDocumentsDefinition entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.DocumentTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.TransportationTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ProcessTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CargoTypeName);

            if (!string.IsNullOrEmpty(entityPOCO.DocumentTypeCode))
            {
                CustomDocumentTypeQueryService customDocumentTypeQueryService = new CustomDocumentTypeQueryService(entityPOCO.Tenant);
                CustomDocumentTypePM customDocumentTypePM = customDocumentTypeQueryService.GetSingle(entityPOCO.DocumentTypeCode, false, true);
                entityPM.DocumentTypeName = customDocumentTypePM.LocalName;
            }


            if (!string.IsNullOrEmpty(entityPOCO.TransportationTypeCode))
            {
                CustomsTransportModeQueryService customsTransportModeQueryService = new CustomsTransportModeQueryService(entityPOCO.Tenant);
                CustomsTransportModePM customsTransportModePM = customsTransportModeQueryService.GetSingle(entityPOCO.TransportationTypeCode, false, true);
                entityPM.TransportationTypeName = customsTransportModePM.LocalName;
            }

            if (!string.IsNullOrEmpty(entityPOCO.ProcessTypeCode))
            {
                GovernmentProcedureTypeQueryService governmentProcedureTypeQueryService = new GovernmentProcedureTypeQueryService(entityPOCO.Tenant);
                GovernmentProcedureTypePM governmentProcedureTypePM = governmentProcedureTypeQueryService.GetSingle(entityPOCO.ProcessTypeCode, false, true);
                entityPM.ProcessTypeName = governmentProcedureTypePM.LocalName;
            }

            if (!string.IsNullOrEmpty(entityPOCO.CargoTypeCode))
            {
                CargoIdentifireTypeQueryService cargoIdentifireTypeQueryService = new CargoIdentifireTypeQueryService(entityPOCO.Tenant);
                CargoIdentifireTypePM cargoIdentifireTypePM = cargoIdentifireTypeQueryService.GetSingle(entityPOCO.CargoTypeCode, false, true);
                entityPM.CargoTypeName = cargoIdentifireTypePM.LocalName;
            }
        }
   }


}
   