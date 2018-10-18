 
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
   public partial class QuestionnaireQueryService: EntityQueryService<Questionnaire,QuestionnaireKeys,QuestionnairePM,object,QuestionnaireKeys>
   {
   
        QuestionnaireRepository repository;
		ICRMContext  context;
        public QuestionnaireQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuestionnaireRepository(context);
            Repository = repository;
            mapping = new QuestionnaireDataMapping();
        }

        public QuestionnaireQueryService(QuestionnaireRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuestionnaireDataMapping();
        }

        public QuestionnaireQueryService(ICRMContext context)
        {
            this.repository = new QuestionnaireRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuestionnaireDataMapping();
        }
		 
		public  QuestionnairePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuestionnaireKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Questionnaire entityPOCO)
        {
            QuestionnaireKeys entityKeys = new QuestionnaireKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 