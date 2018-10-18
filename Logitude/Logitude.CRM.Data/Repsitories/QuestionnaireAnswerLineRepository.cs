 
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
   public partial class QuestionnaireAnswerLineRepository:IRepository<QuestionnaireAnswerLine>
   {
        
		public List<QuestionnaireAnswerLine> GetMulti(EntityKeyFields entityKeys)
        {

            QuestionnaireAnswerKeys myEntityKeys = entityKeys as QuestionnaireAnswerKeys;
    
            return (from a in context.QuestionnaireAnswerLines where a.QuestionnaireAnswerId == myEntityKeys.Id select a).ToList();
      
        }

   }

}
   