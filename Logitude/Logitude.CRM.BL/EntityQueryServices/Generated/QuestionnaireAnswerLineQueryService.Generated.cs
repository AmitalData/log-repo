 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityDataMappings;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.CRM.BL.EntityQueryServices
{ 
   public partial class QuestionnaireAnswerLineQueryService: EntityQueryService<QuestionnaireAnswerLine,QuestionnaireAnswerLineKeys,QuestionnaireAnswerLinePM,QuestionnaireAnswerPM,QuestionnaireAnswerKeys>
   {
   
        QuestionnaireAnswerLineRepository repository;
		ICRMContext  context;
        public QuestionnaireAnswerLineQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuestionnaireAnswerLineRepository(context);
            Repository = repository;
            mapping = new QuestionnaireAnswerLineDataMapping();
        }

        public QuestionnaireAnswerLineQueryService(QuestionnaireAnswerLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuestionnaireAnswerLineDataMapping();
        }

        public QuestionnaireAnswerLineQueryService(ICRMContext context)
        {
            this.repository = new QuestionnaireAnswerLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuestionnaireAnswerLineDataMapping();
        }
		 
		public  QuestionnaireAnswerLinePM GetSingle(string questionnaireanswerid, int questionnumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuestionnaireAnswerLineKeys(){ QuestionnaireAnswerId = questionnaireanswerid, QuestionNumber = questionnumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuestionnaireAnswerLine entityPOCO)
        {
            QuestionnaireAnswerLineKeys entityKeys = new QuestionnaireAnswerLineKeys() { QuestionnaireAnswerId = entityPOCO.QuestionnaireAnswerId, QuestionNumber = entityPOCO.QuestionNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 