 
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
   public partial class OpportunityAdditionalServiceRepository:IRepository<OpportunityAdditionalService>
   {
   
        private ICRMContext currentContext;
        public OpportunityAdditionalServiceRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OpportunityAdditionalServiceRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  OpportunityAdditionalService GetSingle(string opportunityid, string additionalserviceid, int tenant)
        {
            return (from a in context.OpportunityAdditionalServices
                    where a.OpportunityId == opportunityid && a.AdditionalServiceId == additionalserviceid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<OpportunityAdditionalService> GetAll(int tenant)
        {
            return from a in context.OpportunityAdditionalServices  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public OpportunityAdditionalService GetSingle(EntityKeyFields entityKeys)
        {
            OpportunityAdditionalServiceKeys keys = entityKeys as OpportunityAdditionalServiceKeys;
            return (from a in context.OpportunityAdditionalServices
                    where a.OpportunityId == keys.OpportunityId && a.AdditionalServiceId == keys.AdditionalServiceId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OpportunityAdditionalService entity)
        {
            onAdd();
            context.OpportunityAdditionalServices.Add(entity);
        }

        public void Remove(OpportunityAdditionalService entity)
        {
            context.OpportunityAdditionalServices.Attach(entity);
            context.OpportunityAdditionalServices.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OpportunityAdditionalService entity)
        {
            onUpdate();
            context.OpportunityAdditionalServices.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OpportunityAdditionalService> All()
        {
            return context.OpportunityAdditionalServices.ToList();
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
	 