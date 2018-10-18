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
   public partial class QuestionnaireAnswerQueryService
    {
       public override void GetComposition(EntityKeyFields entityKeys, QuestionnaireAnswerPM entityPM)
       {
           ICRMContext context = MainContext as ICRMContext;
           QuestionnaireAnswerKeys questionnaireAnswerKeys = entityKeys as QuestionnaireAnswerKeys;
           QuestionnaireAnswerLineQueryService questionnaireAnswerLineQueryService = new QuestionnaireAnswerLineQueryService(context);
           entityPM.QuestionnaireAnswerLines = questionnaireAnswerLineQueryService.GetMulti(questionnaireAnswerKeys, true);
       }


       public QuestionnaireAnswerPM GetQuestionnaireAnswerPMByquestioneerIdAndversion(string questioneerId, int tenant, string tableId, string entityId)
       {

      
           QuestionnaireAnswerPM questionnaireAnswerPM = (from a in context.QuestionnaireAnswers
                                                          where a.QuestioneerId == questioneerId
                                                         && a.Tenant == tenant && a.ObjectTableId == tableId && a.EntityId == entityId
                                                           select new QuestionnaireAnswerPM()
                                                 {
                                                     CreateDate = a.CreateDate,
                                                     Tenant = a.Tenant,
                                                     Id = a.Id,
                                                     CreatedByUserId = a.CreatedByUserId,
                                                     QuestioneerId = a.QuestioneerId,
                                                     VersionNumber = a.VersionNumber,
                                                     ObjectTableId = a.ObjectTableId,
                                                     EntityId = a.EntityId,
                                                     HasTwoColumn = a.HasTwoColumn,
                                                 }).FirstOrDefault();

           if (questionnaireAnswerPM != null)
           {
               QuestionnaireAnswerLineQueryService questionnaireAnswerLineQueryService = new QuestionnaireAnswerLineQueryService(questionnaireAnswerPM.Tenant);
               QuestionnaireAnswerKeys questionnaireAnswerKeys = new QuestionnaireAnswerKeys() { Id = questionnaireAnswerPM.Id };
               questionnaireAnswerPM.QuestionnaireAnswerLines = questionnaireAnswerLineQueryService.GetMulti(questionnaireAnswerKeys, true);
           }

           return questionnaireAnswerPM;
       }

    }
}
