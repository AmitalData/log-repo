using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
  public partial  class QuestionnaireQuestionUpdateService
    {


      protected override void OnCreating(QuestionnaireQuestionPM entityPM, QuestionnairePM entityParentPM)
      {
          entityPM.QuestioneerId = entityParentPM.Id;
          entityPM.VersionNumber = entityParentPM.VersionNumber;
    
         
          base.OnCreating(entityPM, entityParentPM);
      }


    }
}
