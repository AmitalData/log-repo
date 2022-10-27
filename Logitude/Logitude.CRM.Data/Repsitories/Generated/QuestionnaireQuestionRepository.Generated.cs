 
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
   
        private ICRMContext currentContext;
        public QuestionnaireQuestionRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public QuestionnaireQuestionRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuestionnaireQuestion GetSingle(string questioneerid, int versionnumber, int questionnumber, int tenant)
        {
            return (from a in context.QuestionnaireQuestions
                    where a.QuestioneerId == questioneerid && a.VersionNumber == versionnumber && a.QuestionNumber == questionnumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuestionnaireQuestion> GetAll(int tenant)
        {
            return from a in context.QuestionnaireQuestions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuestionnaireQuestion GetSingle(EntityKeyFields entityKeys)
        {
            QuestionnaireQuestionKeys keys = entityKeys as QuestionnaireQuestionKeys;
            return (from a in context.QuestionnaireQuestions
                    where a.QuestioneerId == keys.QuestioneerId && a.VersionNumber == keys.VersionNumber && a.QuestionNumber == keys.QuestionNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuestionnaireQuestion entity)
        {
            onAdd();
            context.QuestionnaireQuestions.Add(entity);
        }

        public void Remove(QuestionnaireQuestion entity)
        {
            context.QuestionnaireQuestions.Attach(entity);
            context.QuestionnaireQuestions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuestionnaireQuestion entity)
        {
            onUpdate();
            context.QuestionnaireQuestions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuestionnaireQuestion> All()
        {
            return context.QuestionnaireQuestions.ToList();
        }

        private ICRMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 