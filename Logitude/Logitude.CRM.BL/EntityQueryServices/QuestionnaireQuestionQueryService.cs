using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class QuestionnaireQuestionQueryService
    {


        public List<QuestionnaireQuestionPM> GetQuestionnaireQuestionByQuestioneerVersion(string questioneerid, int version, int tenant)
        {
                List<QuestionnaireQuestionPM> questionnaireQuestion =( from a in context.QuestionnaireQuestions
                                                                        where a.QuestioneerId == questioneerid
                                                                        && a.Tenant == tenant && a.VersionNumber == version
                                                                        select new QuestionnaireQuestionPM()
                                                                {
                                                                    IsMandatory = a.IsMandatory,
                                                                    CreateDate = a.CreateDate,
                                                                    Question = a.Question,
                                                                    QuestionNumber = a.QuestionNumber,
                                                                    QuestionTypeCode = a.QuestionTypeCode,
                                                                    Tenant = a.Tenant,
                                                                    // PickListId = a.PickListId,
                                                                    UpdateDate = a.UpdateDate,
                                                                    UpdatedByUserId = a.UpdatedByUserId,
                                                                    CreatedByUserId = a.CreatedByUserId,
                                                                    QuestioneerId = a.QuestioneerId,
                                                                    VersionNumber = a.VersionNumber,
                                                                    PickListCode = a.PickListCode,
                                                                    IsAddOther = a.IsAddOther,
                                                                }).ToList();
            return questionnaireQuestion;
        }


    }
}
