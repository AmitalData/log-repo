
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
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.ClosedTable;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsRequestsSheetDataMapping: IMapping<CustomsRequestsSheetPM, CustomsRequestsSheet>
   {

        public void CustomPMToPOCO(CustomsRequestsSheetPM entityPM, CustomsRequestsSheet entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            entityPOCO.Tenant = entityPM.Tenant;

            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);

        }

        public void CustomPOCOToPM(CustomsRequestsSheetPM entityPM, CustomsRequestsSheet entityPOCO)
        {
            //throw new NotImplementedException();
            if (entityPOCO != null && entityPM != null)
            {
                var interfaceManagementDetails = new InterfaceManagementDetails();
                var pm = interfaceManagementDetails.GetAll().FirstOrDefault(rec => rec.Code == entityPOCO.InterfaceTypeCode);
                entityPM.InterfaceTypeName = pm.Description;
            }
             
        }


        private static void BuildSearchFields(CustomsRequestsSheetPM entityPM, CustomsRequestsSheet poco, bool isNewEntity)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.EntityReference))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.EntityReference : result + "," + entityPM.EntityReference;
            }

            if (!string.IsNullOrEmpty(entityPM.RequestDescription))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.RequestDescription : result + "," + entityPM.RequestDescription;
            }


            if (!string.IsNullOrEmpty(entityPM.CustomFileNo))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.CustomFileNo : result + "," + entityPM.CustomFileNo;
            }
            if (entityPM.ObjectTableId1 == ObjectTableRepository.GetObjectTableByName("Customs.Declaration"))
            {
                DeclarationRepository declarationRepository = new DeclarationRepository(entityPM.Tenant);
                DeclarationKeys declarationKeys = new DeclarationKeys() { Id = entityPM.EntityId1 };
                bool faster = true;//cache not relvanat due id Change all the time !!
                string DeclarationNumber = null;
                if (faster)
                {
                    DeclarationNumber =
                    declarationRepository.GetQSingle(declarationKeys)
                        .Select(r => r.DeclarationNumber)
                        .FirstOrDefault();
                }
                else
                {
                    Declaration declaration = declarationRepository.GetSingle(declarationKeys);
                }



                if (/*declaration != null*/ !string.IsNullOrWhiteSpace(DeclarationNumber))
                {
                    result = string.IsNullOrEmpty(result) ? /*declaration.*/DeclarationNumber : result + "," + /*declaration.*/DeclarationNumber;
                }


            }
            result = result ?? "";
            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }

   }


}
   