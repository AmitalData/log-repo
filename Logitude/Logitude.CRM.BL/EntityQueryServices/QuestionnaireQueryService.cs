using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityQueryServices
{
   public partial class QuestionnaireQueryService
    {

       public override void GetComposition(EntityKeyFields entityKeys, QuestionnairePM entityPM)
       {
           ICRMContext context = MainContext as ICRMContext;
           QuestionnaireKeys questionnaireKeys = entityKeys as QuestionnaireKeys;
           QuestionnaireQuestionQueryService questionnaireQuestionQueryService = new QuestionnaireQuestionQueryService(context);


           entityPM.QuestionnaireQuestions = questionnaireQuestionQueryService.GetMulti(questionnaireKeys, true).Where(Q => Q.VersionNumber == entityPM.VersionNumber).ToList();
       }


      

    }
}
