
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
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ProceduralFaultDataMapping: IMapping<ProceduralFaultPM, ProceduralFault>
   {

        public void CustomPMToPOCO(ProceduralFaultPM entityPM, ProceduralFault entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;



            }


            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        private static void BuildSearchFields(ProceduralFaultPM entityPM, ProceduralFault poco, bool isNewEntity)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.ProceduralFaultNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ProceduralFaultNumber : result + "," + entityPM.ProceduralFaultNumber;
            }

            DeclarationQueryService declarationQuery = new DeclarationQueryService(entityPM.Tenant);
            DeclarationPM declaration = declarationQuery.GetSingle(entityPM.DeclarationId, false, false);

            if (declaration != null)
            {
                if (!string.IsNullOrEmpty(declaration.CustomFileNo))
                {
                    result = string.IsNullOrEmpty(result) ? declaration.CustomFileNo : result + "," + declaration.CustomFileNo;
                }

                if (!string.IsNullOrEmpty(declaration.DeclarationNumber))
                {
                    result = string.IsNullOrEmpty(result) ? declaration.DeclarationNumber : result + "," + declaration.DeclarationNumber;
                }
            }





            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(ProceduralFaultPM entityPM, ProceduralFault entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.InputTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.InspectionTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ProceduralFaultInputProcesName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ProceduralFaultStatusName); 
            this.CustomMappedPMProperties.Add(PMPropertyNames.RansomViolationTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ProceduralFaultName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.SignedByUserName);


            if (entityPOCO.InputTypeCode != null)
            {
                ProceduralFaultInSourceTypeQueryService proceduralFaultInputSourceTypeQueryService = new ProceduralFaultInSourceTypeQueryService(entityPOCO.Tenant);
                ProceduralFaultInSourceTypePM proceduralFaultInputSourceType = proceduralFaultInputSourceTypeQueryService.GetSingle(entityPOCO.InputTypeCode, false, true);
                entityPM.InputTypeName = proceduralFaultInputSourceType != null ? proceduralFaultInputSourceType.LocalName : null;

            }

            if (entityPOCO.InspectionTypeCode != null)
            {
                FaultInspectionTypeQueryService faultInspectionTypeQueryService = new FaultInspectionTypeQueryService(entityPOCO.Tenant);
                FaultInspectionTypePM faultInspectionType = faultInspectionTypeQueryService.GetSingle(entityPOCO.InspectionTypeCode, false, true);
                entityPM.InspectionTypeName = faultInspectionType != null ? faultInspectionType.LocalName : null;

            }

            if (entityPOCO.ProceduralFaultInputProcesCode != null)
            {
                ProceduralFaultInProcessTypeQueryService proceduralFaultInputProcessTypeQueryService = new ProceduralFaultInProcessTypeQueryService(entityPOCO.Tenant);
                ProceduralFaultInProcessTypePM proceduralFaultInputProcessType = proceduralFaultInputProcessTypeQueryService.GetSingle(entityPOCO.ProceduralFaultInputProcesCode, false, true);
                entityPM.ProceduralFaultInputProcesName = proceduralFaultInputProcessType != null ? proceduralFaultInputProcessType.LocalName : null;

            }


            if (entityPOCO.ProceduralFaultStatusCode != null)
            {
                ProceduralFaultStatusQueryService proceduralFaultStatusQueryService = new ProceduralFaultStatusQueryService(entityPOCO.Tenant);
                ProceduralFaultStatusPM proceduralFaultStatus = proceduralFaultStatusQueryService.GetSingle(entityPOCO.ProceduralFaultStatusCode, false, true);
                entityPM.ProceduralFaultStatusName = proceduralFaultStatus != null ? proceduralFaultStatus.LocalName : null;

            }

            if (entityPOCO.RansomViolationTypeCode != null)
            {
                RansomViolationTypeQueryService ransomViolationTypeQueryService = new RansomViolationTypeQueryService(entityPOCO.Tenant);
                RansomViolationTypePM ransomViolationType = ransomViolationTypeQueryService.GetSingle(entityPOCO.RansomViolationTypeCode, false, true);
                entityPM.RansomViolationTypeName = ransomViolationType != null ? ransomViolationType.LocalName : null;

            }

            if (entityPOCO.ProceduralFaultCode != null)
            {
                ProceduralFaultTypeQueryService proceduralFaultTypeQueryService = new ProceduralFaultTypeQueryService(entityPOCO.Tenant);
                ProceduralFaultTypePM proceduralFaultType = proceduralFaultTypeQueryService.GetSingle(entityPOCO.ProceduralFaultCode, false, true);
                entityPM.ProceduralFaultName = proceduralFaultType != null ? proceduralFaultType.LocalName : null;

            }

            if (entityPOCO.SignedByUserId != null)
            {
                Simplog.Data.CommonDataModel.Repositories.UserRepository userRep = new Simplog.Data.CommonDataModel.Repositories.UserRepository(entityPM.Tenant);
                User user = userRep.GetSingleUser(entityPM.SignedByUserId, entityPM.Tenant);
                if (user != null)
                {
                    entityPM.SignedByUserName = user.Contact.LocalName != null ? user.Contact.LocalName : user.Contact.EnglishName;
                }

            }


            DeclarationQueryService declarationQuery = new DeclarationQueryService(entityPOCO.Tenant);
            DeclarationPM declaration = declarationQuery.GetSingle(entityPOCO.DeclarationId, false, false);
            entityPM.DeclarationNumber = declaration != null ? declaration.DeclarationNumber : null;
            entityPM.CustomFileNo = declaration != null ? declaration.CustomFileNo : null;

        }
   }


}
   