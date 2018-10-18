
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

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class QuestionnaireAnswerLineDataMapping: IMapping<QuestionnaireAnswerLinePM, QuestionnaireAnswerLine>
   {

        public void CustomPMToPOCO(QuestionnaireAnswerLinePM entityPM, QuestionnaireAnswerLine entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.QuestionnaireAnswerId);
            entityPOCO.QuestionnaireAnswerId = entityPM.QuestionnaireAnswerId;
            entityPOCO.QuestionNumber = entityPM.QuestionNumber;
        }

        public void CustomPOCOToPM(QuestionnaireAnswerLinePM entityPM, QuestionnaireAnswerLine entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   