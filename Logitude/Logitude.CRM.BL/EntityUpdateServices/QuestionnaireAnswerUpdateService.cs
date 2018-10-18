using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityUpdateServices
{
    public partial   class QuestionnaireAnswerUpdateService
    {

        protected override void OnCreating(EntityPMs.QuestionnaireAnswerPM entityPM, Server.Tools.EntityPM entityParentPM)
        {

            ICRMContext context = CRMContext.GetContext(entityPM.Tenant);
            entityPM.Id = IdCounter.GetNumber("QuestionnaireAnswer", entityPM.Tenant);

            base.OnCreating(entityPM, entityParentPM);
        }


        protected override void UpdateComposition(QuestionnaireAnswerPM entityPM)
        {
            QuestionnaireAnswerLineUpdateService questionnaireAnswerLineUpdateService = new QuestionnaireAnswerLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            questionnaireAnswerLineUpdateService.UpdateMulti(entityPM.QuestionnaireAnswerLines, entityPM.DeletedQuestionnaireAnswerLines, entityPM, false);
            base.UpdateComposition(entityPM);
        }

    }
}
