 
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
   public partial class OpportunityProductRepository:IRepository<OpportunityProduct>
   {
   
        private ICRMContext currentContext;
        public OpportunityProductRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OpportunityProductRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  OpportunityProduct GetSingle(string opportunityid, string opportunityproducttypecode, int tenant)
        {
            return (from a in context.OpportunityProducts
                    where a.OpportunityId == opportunityid && a.OpportunityProductTypeCode == opportunityproducttypecode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<OpportunityProduct> GetAll(int tenant)
        {
            return from a in context.OpportunityProducts  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public OpportunityProduct GetSingle(EntityKeyFields entityKeys)
        {
            OpportunityProductKeys keys = entityKeys as OpportunityProductKeys;
            return (from a in context.OpportunityProducts
                    where a.OpportunityId == keys.OpportunityId && a.OpportunityProductTypeCode == keys.OpportunityProductTypeCode
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OpportunityProduct entity)
        {
            onAdd();
            context.OpportunityProducts.Add(entity);
        }

        public void Remove(OpportunityProduct entity)
        {
            context.OpportunityProducts.Attach(entity);
            context.OpportunityProducts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OpportunityProduct entity)
        {
            onUpdate();
            context.OpportunityProducts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OpportunityProduct> All()
        {
            return context.OpportunityProducts.ToList();
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
	 