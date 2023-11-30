 
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
   public partial class OpportunityClosingReasonRepository:IRepository<OpportunityClosingReason>
   {
   
        private ICRMContext currentContext;
        public OpportunityClosingReasonRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OpportunityClosingReasonRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  OpportunityClosingReason GetSingle(string id, int tenant)
        {
            return (from a in context.OpportunityClosingReasons
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<OpportunityClosingReason> GetAll(int tenant)
        {
            return from a in context.OpportunityClosingReasons  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public OpportunityClosingReason GetSingle(EntityKeyFields entityKeys)
        {
            OpportunityClosingReasonKeys keys = entityKeys as OpportunityClosingReasonKeys;
            return (from a in context.OpportunityClosingReasons
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OpportunityClosingReason entity)
        {
            onAdd();
            context.OpportunityClosingReasons.Add(entity);
        }

        public void Remove(OpportunityClosingReason entity)
        {
            context.OpportunityClosingReasons.Attach(entity);
            context.OpportunityClosingReasons.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OpportunityClosingReason entity)
        {
            onUpdate();
            context.OpportunityClosingReasons.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OpportunityClosingReason> All()
        {
            return context.OpportunityClosingReasons.ToList();
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
	 