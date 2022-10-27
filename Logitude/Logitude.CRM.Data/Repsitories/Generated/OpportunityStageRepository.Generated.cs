 
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
   public partial class OpportunityStageRepository:IRepository<OpportunityStage>
   {
   
        private ICRMContext currentContext;
        public OpportunityStageRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OpportunityStageRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  OpportunityStage GetSingle(string id, int tenant)
        {
            return (from a in context.OpportunityStages
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<OpportunityStage> GetAll(int tenant)
        {
            return from a in context.OpportunityStages  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public OpportunityStage GetSingle(EntityKeyFields entityKeys)
        {
            OpportunityStageKeys keys = entityKeys as OpportunityStageKeys;
            return (from a in context.OpportunityStages
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OpportunityStage entity)
        {
            onAdd();
            context.OpportunityStages.Add(entity);
        }

        public void Remove(OpportunityStage entity)
        {
            context.OpportunityStages.Attach(entity);
            context.OpportunityStages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OpportunityStage entity)
        {
            onUpdate();
            context.OpportunityStages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OpportunityStage> All()
        {
            return context.OpportunityStages.ToList();
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
	 