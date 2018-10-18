 
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
   public partial class OpportunityProductLocationRepository:IRepository<OpportunityProductLocation>
   {
   
        private ICRMContext currentContext;
        public OpportunityProductLocationRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OpportunityProductLocationRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  OpportunityProductLocation GetSingle(string opportunityid, string opportunityproducttypecode, int linenumber, int tenant)
        {
            return (from a in context.OpportunityProductLocations
                    where a.OpportunityId == opportunityid && a.OpportunityProductTypeCode == opportunityproducttypecode && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<OpportunityProductLocation> GetAll(int tenant)
        {
            return from a in context.OpportunityProductLocations  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public OpportunityProductLocation GetSingle(EntityKeyFields entityKeys)
        {
            OpportunityProductLocationKeys keys = entityKeys as OpportunityProductLocationKeys;
            return (from a in context.OpportunityProductLocations
                    where a.OpportunityId == keys.OpportunityId && a.OpportunityProductTypeCode == keys.OpportunityProductTypeCode && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OpportunityProductLocation entity)
        {
            onAdd();
            context.OpportunityProductLocations.Add(entity);
        }

        public void Remove(OpportunityProductLocation entity)
        {
            context.OpportunityProductLocations.Attach(entity);
            context.OpportunityProductLocations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OpportunityProductLocation entity)
        {
            onUpdate();
            context.OpportunityProductLocations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OpportunityProductLocation> All()
        {
            return context.OpportunityProductLocations.ToList();
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
	 