using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
  public partial  class QuestionnaireUpdateService
    {

        protected override void OnCreating(QuestionnairePM entityPM, EntityPM entityParentPM)
        {
            ICRMContext context = CRMContext.GetContext(entityPM.Tenant);

            entityPM.Id = IdCounter.GetNumber("Questionnaire", entityPM.Tenant);

            if (entityPM.IsCopy)
            {
                //  var m = context.Questionnaires.Where(c => c.Name.ToLower() == entityPM.Name.ToLower() && c.Tenant == entityPM.Tenant).ToList();
                entityPM.VersionNumber = context.Questionnaires.Where(c => c.Id == entityPM.Id).Max(c => c.VersionNumber) + 1;

            }

            else
            {
                entityPM.VersionNumber = 1;
            }
  
        }

    


        protected override void UpdateComposition(QuestionnairePM entityPM)
        {

            QuestionnaireQuestionUpdateService questionnaireQuestionUpdateService = new QuestionnaireQuestionUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            questionnaireQuestionUpdateService.UpdateMulti(entityPM.QuestionnaireQuestions, entityPM.DeletedQuestionnaireQuestions, entityPM, false);

     
        }

        protected override void OnUpdating(QuestionnairePM entityPM)
        {
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
        }


    }
}
