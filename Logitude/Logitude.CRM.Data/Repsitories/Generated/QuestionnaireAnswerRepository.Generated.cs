 
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
   public partial class QuestionnaireAnswerRepository:IRepository<QuestionnaireAnswer>
   {
   
        private ICRMContext currentContext;
        public QuestionnaireAnswerRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public QuestionnaireAnswerRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuestionnaireAnswer GetSingle(string id, int tenant)
        {
            return (from a in context.QuestionnaireAnswers
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuestionnaireAnswer> GetAll(int tenant)
        {
            return from a in context.QuestionnaireAnswers  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuestionnaireAnswer GetSingle(EntityKeyFields entityKeys)
        {
            QuestionnaireAnswerKeys keys = entityKeys as QuestionnaireAnswerKeys;
            return (from a in context.QuestionnaireAnswers
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuestionnaireAnswer entity)
        {
            onAdd();
            context.QuestionnaireAnswers.Add(entity);
        }

        public void Remove(QuestionnaireAnswer entity)
        {
            context.QuestionnaireAnswers.Attach(entity);
            context.QuestionnaireAnswers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuestionnaireAnswer entity)
        {
            onUpdate();
            context.QuestionnaireAnswers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuestionnaireAnswer> All()
        {
            return context.QuestionnaireAnswers.ToList();
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
	 