 
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
   public partial class OpportunityTypeRepository:IRepository<OpportunityType>
   {
   
        private ICRMContext currentContext;
        public OpportunityTypeRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OpportunityTypeRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  OpportunityType GetSingle(string id, int tenant)
        {
            return (from a in context.OpportunityTypes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<OpportunityType> GetAll(int tenant)
        {
            return from a in context.OpportunityTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public OpportunityType GetSingle(EntityKeyFields entityKeys)
        {
            OpportunityTypeKeys keys = entityKeys as OpportunityTypeKeys;
            return (from a in context.OpportunityTypes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OpportunityType entity)
        {
            onAdd();
            context.OpportunityTypes.Add(entity);
        }

        public void Remove(OpportunityType entity)
        {
            context.OpportunityTypes.Attach(entity);
            context.OpportunityTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OpportunityType entity)
        {
            onUpdate();
            context.OpportunityTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OpportunityType> All()
        {
            return context.OpportunityTypes.ToList();
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
	 