using Logitude.CRM.BL.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
  public partial  class QuestionnaireAnswerLineUpdateService
    {

      protected override void OnCreating(QuestionnaireAnswerLinePM entityPM, QuestionnaireAnswerPM entityParentPM)
      {
          entityPM.QuestionnaireAnswerId = entityParentPM.Id;
          base.OnCreating(entityPM, entityParentPM);
      }
    }
}
