 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class SatisfactionSurveyRepository:IRepository<SatisfactionSurvey>
   {
   
        private IInfrastructureContext currentContext;
        public SatisfactionSurveyRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public SatisfactionSurveyRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  SatisfactionSurvey GetSingle(string id, int tenant)
        {
            return (from a in context.SatisfactionSurveys
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SatisfactionSurvey> GetAll(int tenant)
        {
            return from a in context.SatisfactionSurveys  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SatisfactionSurvey GetSingle(EntityKeyFields entityKeys)
        {
            SatisfactionSurveyKeys keys = entityKeys as SatisfactionSurveyKeys;
            return (from a in context.SatisfactionSurveys
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SatisfactionSurvey entity)
        {
            onAdd();
            context.SatisfactionSurveys.Add(entity);
        }

        public void Remove(SatisfactionSurvey entity)
        {
            context.SatisfactionSurveys.Attach(entity);
            context.SatisfactionSurveys.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SatisfactionSurvey entity)
        {
            onUpdate();
            context.SatisfactionSurveys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SatisfactionSurvey> All()
        {
            return context.SatisfactionSurveys.ToList();
        }

        private IInfrastructureContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 