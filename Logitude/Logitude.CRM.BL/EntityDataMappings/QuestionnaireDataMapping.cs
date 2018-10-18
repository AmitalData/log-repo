
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class QuestionnaireDataMapping: IMapping<QuestionnairePM, Questionnaire>
   {

        public void CustomPMToPOCO(QuestionnairePM entityPM, Questionnaire entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;
            entityPOCO.VersionNumber = entityPM.VersionNumber;
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        public void CustomPOCOToPM(QuestionnairePM entityPM, Questionnaire entityPOCO)
        {
            //throw new NotImplementedException();
        }


        private void BuildSearchFields(QuestionnairePM entityPM, Questionnaire entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPM.Name))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Name : mySearchFields + "," + entityPM.Name;
            }

            

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
   }


}
   