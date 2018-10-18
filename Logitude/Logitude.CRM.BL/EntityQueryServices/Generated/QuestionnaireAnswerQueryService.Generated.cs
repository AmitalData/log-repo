 
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
   public partial class QuestionnaireAnswerQueryService: EntityQueryService<QuestionnaireAnswer,QuestionnaireAnswerKeys,QuestionnaireAnswerPM,object,QuestionnaireAnswerKeys>
   {
   
        QuestionnaireAnswerRepository repository;
		ICRMContext  context;
        public QuestionnaireAnswerQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new QuestionnaireAnswerRepository(context);
            Repository = repository;
            mapping = new QuestionnaireAnswerDataMapping();
        }

        public QuestionnaireAnswerQueryService(QuestionnaireAnswerRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new QuestionnaireAnswerDataMapping();
        }

        public QuestionnaireAnswerQueryService(ICRMContext context)
        {
            this.repository = new QuestionnaireAnswerRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new QuestionnaireAnswerDataMapping();
        }
		 
		public  QuestionnaireAnswerPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new QuestionnaireAnswerKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(QuestionnaireAnswer entityPOCO)
        {
            QuestionnaireAnswerKeys entityKeys = new QuestionnaireAnswerKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 