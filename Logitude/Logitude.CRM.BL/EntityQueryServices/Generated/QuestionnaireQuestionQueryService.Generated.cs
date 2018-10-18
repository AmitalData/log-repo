 
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
   public partial class QuestionnaireQuestionQueryService: EntityQueryService<QuestionnaireQuestion,QuestionnaireQuestionKeys,QuestionnaireQuestionPM,QuestionnairePM,QuestionnaireKeys>
   {
   
        QuestionnaireQuestionRepository repository;
		ICRMContext  context;
        public QuestionnaireQuestionQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuestionnaireQuestionRepository(context);
            Repository = repository;
            mapping = new QuestionnaireQuestionDataMapping();
        }

        public QuestionnaireQuestionQueryService(QuestionnaireQuestionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuestionnaireQuestionDataMapping();
        }

        public QuestionnaireQuestionQueryService(ICRMContext context)
        {
            this.repository = new QuestionnaireQuestionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuestionnaireQuestionDataMapping();
        }
		 
		public  QuestionnaireQuestionPM GetSingle(string questioneerid, int versionnumber, int questionnumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuestionnaireQuestionKeys(){ QuestioneerId = questioneerid, VersionNumber = versionnumber, QuestionNumber = questionnumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuestionnaireQuestion entityPOCO)
        {
            QuestionnaireQuestionKeys entityKeys = new QuestionnaireQuestionKeys() { QuestioneerId = entityPOCO.QuestioneerId, VersionNumber = entityPOCO.VersionNumber, QuestionNumber = entityPOCO.QuestionNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 