 
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
   public partial class OpportunityCompetitorRepository:IRepository<OpportunityCompetitor>
   {
   
        private ICRMContext currentContext;
        public OpportunityCompetitorRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OpportunityCompetitorRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  OpportunityCompetitor GetSingle(string opportunityid, string competitorid, int tenant)
        {
            return (from a in context.OpportunityCompetitors
                    where a.OpportunityId == opportunityid && a.CompetitorId == competitorid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<OpportunityCompetitor> GetAll(int tenant)
        {
            return from a in context.OpportunityCompetitors  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public OpportunityCompetitor GetSingle(EntityKeyFields entityKeys)
        {
            OpportunityCompetitorKeys keys = entityKeys as OpportunityCompetitorKeys;
            return (from a in context.OpportunityCompetitors
                    where a.OpportunityId == keys.OpportunityId && a.CompetitorId == keys.CompetitorId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OpportunityCompetitor entity)
        {
            onAdd();
            context.OpportunityCompetitors.Add(entity);
        }

        public void Remove(OpportunityCompetitor entity)
        {
            context.OpportunityCompetitors.Attach(entity);
            context.OpportunityCompetitors.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OpportunityCompetitor entity)
        {
            onUpdate();
            context.OpportunityCompetitors.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OpportunityCompetitor> All()
        {
            return context.OpportunityCompetitors.ToList();
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
	 