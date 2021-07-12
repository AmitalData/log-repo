
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
   
   public partial class CustomerClassificationTypeDataMapping: IMapping<CustomerClassificationTypePM, CustomerClassificationType>
   {

        public void CustomPMToPOCO(CustomerClassificationTypePM entityPM, CustomerClassificationType entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Code = entityPM.Code;
            }
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        private void BuildSearchFields(CustomerClassificationTypePM entityPM, CustomerClassificationType entityPOCO, bool v)
        {
            string result = entityPM.Code + ',' + entityPM.EnglishName + ',' + entityPM.LocalName;
            entityPM.SearchFields = result.ToLower();
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(CustomerClassificationTypePM entityPM, CustomerClassificationType entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   