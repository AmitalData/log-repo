 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class QuestionnaireQuestionRepository:IRepository<QuestionnaireQuestion>
   {
        
		public List<QuestionnaireQuestion> GetMulti(EntityKeyFields entityKeys)
        {

            QuestionnaireKeys keys = entityKeys as QuestionnaireKeys;
            return (from a in context.QuestionnaireQuestions where a.QuestioneerId == keys.Id select a).ToList();
        }

   }

}
   