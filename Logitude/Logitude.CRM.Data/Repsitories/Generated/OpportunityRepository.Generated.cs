 
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
   public partial class OpportunityRepository:IRepository<Opportunity>
   {
   
        private ICRMContext currentContext;
        public OpportunityRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OpportunityRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  Opportunity GetSingle(string id, int tenant)
        {
            return (from a in context.Opportunities
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Opportunity> GetAll(int tenant)
        {
            return from a in context.Opportunities  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Opportunity GetSingle(EntityKeyFields entityKeys)
        {
            OpportunityKeys keys = entityKeys as OpportunityKeys;
            return (from a in context.Opportunities
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Opportunity entity)
        {
            onAdd();
            context.Opportunities.Add(entity);
        }

        public void Remove(Opportunity entity)
        {
            context.Opportunities.Attach(entity);
            context.Opportunities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Opportunity entity)
        {
            onUpdate();
            context.Opportunities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Opportunity> All()
        {
            return context.Opportunities.ToList();
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
	 