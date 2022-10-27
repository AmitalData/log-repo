 
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
   public partial class QuestionnaireRepository:IRepository<Questionnaire>
   {
   
        private ICRMContext currentContext;
        public QuestionnaireRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public QuestionnaireRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  Questionnaire GetSingle(string id, int tenant)
        {
            return (from a in context.Questionnaires
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Questionnaire> GetAll(int tenant)
        {
            return from a in context.Questionnaires  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Questionnaire GetSingle(EntityKeyFields entityKeys)
        {
            QuestionnaireKeys keys = entityKeys as QuestionnaireKeys;
            return (from a in context.Questionnaires
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Questionnaire entity)
        {
            onAdd();
            context.Questionnaires.Add(entity);
        }

        public void Remove(Questionnaire entity)
        {
            context.Questionnaires.Attach(entity);
            context.Questionnaires.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Questionnaire entity)
        {
            onUpdate();
            context.Questionnaires.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Questionnaire> All()
        {
            return context.Questionnaires.ToList();
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
	 