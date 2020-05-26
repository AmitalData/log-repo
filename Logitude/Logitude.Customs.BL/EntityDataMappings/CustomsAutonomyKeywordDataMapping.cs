
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


namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsAutonomyKeywordDataMapping: IMapping<CustomsAutonomyKeywordPM, CustomsAutonomyKeyword>
   {

        public void CustomPMToPOCO(CustomsAutonomyKeywordPM entityPM, CustomsAutonomyKeyword entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);


            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(CustomsAutonomyKeywordPM entityPM, CustomsAutonomyKeyword entityPOCO)
        {
            //throw new NotImplementedException();
        }
        private void BuildSearchFields(CustomsAutonomyKeywordPM entityPM, CustomsAutonomyKeyword entityPOCO, bool isNewEntity)
        {
            string result = "";
            if (!string.IsNullOrEmpty(entityPM.KeywordtypeCode))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.KeywordtypeCode : result + "," + entityPM.KeywordtypeCode;
            }
            if (!string.IsNullOrEmpty(entityPM.KeywordsList))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.KeywordsList : result + "," + entityPM.KeywordsList;
            }
            entityPM.SearchFields = result.ToLower();
            entityPOCO.SearchFields = entityPM.SearchFields;
        }
    }


}
   