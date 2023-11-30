 
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
   
        private ICRMContext currentContext;
        public QuestionnaireAnswerLineRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public QuestionnaireAnswerLineRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuestionnaireAnswerLine GetSingle(string questionnaireanswerid, int questionnumber, int tenant)
        {
            return (from a in context.QuestionnaireAnswerLines
                    where a.QuestionnaireAnswerId == questionnaireanswerid && a.QuestionNumber == questionnumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuestionnaireAnswerLine> GetAll(int tenant)
        {
            return from a in context.QuestionnaireAnswerLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuestionnaireAnswerLine GetSingle(EntityKeyFields entityKeys)
        {
            QuestionnaireAnswerLineKeys keys = entityKeys as QuestionnaireAnswerLineKeys;
            return (from a in context.QuestionnaireAnswerLines
                    where a.QuestionnaireAnswerId == keys.QuestionnaireAnswerId && a.QuestionNumber == keys.QuestionNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuestionnaireAnswerLine entity)
        {
            onAdd();
            context.QuestionnaireAnswerLines.Add(entity);
        }

        public void Remove(QuestionnaireAnswerLine entity)
        {
            context.QuestionnaireAnswerLines.Attach(entity);
            context.QuestionnaireAnswerLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuestionnaireAnswerLine entity)
        {
            onUpdate();
            context.QuestionnaireAnswerLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuestionnaireAnswerLine> All()
        {
            return context.QuestionnaireAnswerLines.ToList();
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
	 