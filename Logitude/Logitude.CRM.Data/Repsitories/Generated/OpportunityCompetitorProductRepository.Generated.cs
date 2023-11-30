 
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
   public partial class OpportunityCompetitorProductRepository:IRepository<OpportunityCompetitorProduct>
   {
   
        private ICRMContext currentContext;
        public OpportunityCompetitorProductRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OpportunityCompetitorProductRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  OpportunityCompetitorProduct GetSingle(string opportunityid, string competitorid, string producttypecode, int tenant)
        {
            return (from a in context.OpportunityCompetitorProducts
                    where a.OpportunityId == opportunityid && a.CompetitorId == competitorid && a.ProductTypeCode == producttypecode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<OpportunityCompetitorProduct> GetAll(int tenant)
        {
            return from a in context.OpportunityCompetitorProducts  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public OpportunityCompetitorProduct GetSingle(EntityKeyFields entityKeys)
        {
            OpportunityCompetitorProductKeys keys = entityKeys as OpportunityCompetitorProductKeys;
            return (from a in context.OpportunityCompetitorProducts
                    where a.OpportunityId == keys.OpportunityId && a.CompetitorId == keys.CompetitorId && a.ProductTypeCode == keys.ProductTypeCode
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OpportunityCompetitorProduct entity)
        {
            onAdd();
            context.OpportunityCompetitorProducts.Add(entity);
        }

        public void Remove(OpportunityCompetitorProduct entity)
        {
            context.OpportunityCompetitorProducts.Attach(entity);
            context.OpportunityCompetitorProducts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OpportunityCompetitorProduct entity)
        {
            onUpdate();
            context.OpportunityCompetitorProducts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OpportunityCompetitorProduct> All()
        {
            return context.OpportunityCompetitorProducts.ToList();
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
	 