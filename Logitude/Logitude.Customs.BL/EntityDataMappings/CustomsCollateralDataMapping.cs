
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
   
   public partial class CustomsCollateralDataMapping: IMapping<CustomsCollateralPM, CustomsCollateral>
   {

        public void CustomPMToPOCO(CustomsCollateralPM entityPM, CustomsCollateral entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CreateDateTime = entityPM.CreateDateTime;
            }
            
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
         
        }

        private void BuildSearchFields(CustomsCollateralPM entityPM, CustomsCollateral entityPOCO, bool isInsert)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.CollateralRequestNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.CollateralRequestNumber : result + "," + entityPM.CollateralRequestNumber;
            }

            if (!string.IsNullOrEmpty(entityPM.EntityIdKey1))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.EntityIdKey1 : result + "," + entityPM.EntityIdKey1;
            }

            if (!string.IsNullOrEmpty(entityPM.EntityIdKey2))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.EntityIdKey2 : result + "," + entityPM.EntityIdKey2;
            }

            if (!string.IsNullOrEmpty(entityPM.EntityIdKey3))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.EntityIdKey3 : result + "," + entityPM.EntityIdKey3;
            }

            DeclarationQueryService declarationQuery = new DeclarationQueryService(entityPOCO.Tenant);
            DeclarationPM declaration = declarationQuery.GetSingle(entityPM.DeclarationId, false, false);
            if(declaration != null)
            {
                if (!string.IsNullOrEmpty(declaration.DeclarationNumber))
                {
                    result = string.IsNullOrEmpty(result) ? declaration.DeclarationNumber : result + "," + declaration.DeclarationNumber;
                }

                    if (!string.IsNullOrEmpty(declaration.CustomFileNo))
                    {
                        result = string.IsNullOrEmpty(result) ? declaration.CustomFileNo : result + "," + declaration.CustomFileNo;
                    }
            }
          
            
            entityPM.SearchFields = result.ToLower();
            entityPOCO.SearchFields = entityPM.SearchFields;
        
        }

        public void CustomPOCOToPM(CustomsCollateralPM entityPM, CustomsCollateral entityPOCO)
        {
            /*this.CustomMappedPMProperties.Add(PMPropertyNames.CollateralRequestStatusCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.RequestedCollateralTypeCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomsEntityTypeCode);*/
            this.CustomMappedPMProperties.Add(PMPropertyNames.CollateralRequestStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.RequestedCollateralTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomsEntityTypeName);

            if (entityPOCO.CustomsEntityTypeCode != null)
            {
                EntityTypeLookupQueryService entityTypeLookupQueryService = new EntityTypeLookupQueryService(entityPOCO.Tenant);
                EntityTypeLookupPM entityTypeLookup = entityTypeLookupQueryService.GetSingle(entityPOCO.CustomsEntityTypeCode, false, true);
                entityPM.CustomsEntityTypeName = entityTypeLookup.LocalName;
            }

            if (entityPOCO.CollateralRequestStatusCode != null)
            {
                CollateralRequestStatusQueryService collateralRequestStatusQueryService = new CollateralRequestStatusQueryService(entityPOCO.Tenant);
                CollateralRequestStatusPM collateralRequestStatus = collateralRequestStatusQueryService.GetSingle(entityPOCO.CollateralRequestStatusCode, false, true);
                entityPM.CollateralRequestStatusName = collateralRequestStatus.LocalName;
            }

              if (entityPOCO.RequestedCollateralTypeCode != null)
            {
                CollateralTypeQueryService CollateralTypeQueryService = new CollateralTypeQueryService(entityPOCO.Tenant);
                CollateralTypePM CollateralType = CollateralTypeQueryService.GetSingle(entityPOCO.RequestedCollateralTypeCode, false, true);
                entityPM.RequestedCollateralTypeName = CollateralType.LocalName;
            }
        }
   }


}
   