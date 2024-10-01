
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
using Logitude.BL.InfrastructureModel.EntityQueries;

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
            this.CustomMappedPMProperties.Add(PMPropertyNames.DeclarationTypeName);

            if (!string.IsNullOrEmpty(entityPOCO.DocumentTypeCode))
            {
                CustomDocumentTypeQueryService customDocumentTypeQueryService = new CustomDocumentTypeQueryService(entityPOCO.Tenant);
                CustomDocumentTypePM customDocumentTypePM = customDocumentTypeQueryService.GetSingleCustomDocumentTypeWithTenant(entityPOCO.DocumentTypeCode, entityPOCO.Tenant);
                entityPM.DocumentTypeName = customDocumentTypePM.LocalName;
            }


            if (!string.IsNullOrEmpty(entityPOCO.TransportationTypeCode))
            {
                TransportModeQuery TransportModeQueryService = new TransportModeQuery(entityPOCO.Tenant);
                var TransportModePM = TransportModeQueryService.GetSinglePM(entityPOCO.TransportationTypeCode);
                entityPM.TransportationTypeName = TransportModePM.LocalName;
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

            if (!string.IsNullOrEmpty(entityPOCO.DeclarationTypeCode))
            {
                LeadDocumentTypeQueryService leadDocumentTypeQueryService = new LeadDocumentTypeQueryService(entityPOCO.Tenant);
                LeadDocumentTypePM leadDocumentTypePM = leadDocumentTypeQueryService.GetSingle(entityPOCO.DeclarationTypeCode, false, true);
                entityPM.DeclarationTypeName = leadDocumentTypePM.LocalName;
            }
        }
   }


}
   