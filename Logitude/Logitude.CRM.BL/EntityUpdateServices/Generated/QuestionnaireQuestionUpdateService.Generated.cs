 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityDataMappings;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityUpdateServices
{ 
   public partial class QuestionnaireQuestionUpdateService:EntityUpdateService<QuestionnaireQuestion,QuestionnaireQuestionPM,QuestionnairePM>
   {
   
        QuestionnaireQuestionRepository entityRepository;
        public QuestionnaireQuestionUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICRMContext  context = mainContext as CRMContext;
            context = context ??mainContext as ICRMContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new QuestionnaireQuestionDataMapping();
            Repository = new QuestionnaireQuestionRepository(context);
        }

       
        private ICRMContext currentContext;
        public QuestionnaireQuestionUpdateService(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public QuestionnaireQuestionUpdateService(ICRMContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(QuestionnaireQuestionPM entityPM)
        {
            QuestionnaireQuestionKeys entityKeys = new QuestionnaireQuestionKeys() { QuestioneerId = entityPM.QuestioneerId, VersionNumber = entityPM.VersionNumber, QuestionNumber = entityPM.QuestionNumber };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(QuestionnaireQuestionPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(QuestionnaireQuestionPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 