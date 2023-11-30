 
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
   public partial class BusinessProcessQueueRepository:IRepository<BusinessProcessQueue>
   {
   
        private IInfrastructureContext currentContext;
        public BusinessProcessQueueRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public BusinessProcessQueueRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  BusinessProcessQueue GetSingle(string id, int tenant)
        {
            return (from a in context.BusinessProcessQueues
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BusinessProcessQueue> GetAll(int tenant)
        {
            return from a in context.BusinessProcessQueues  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BusinessProcessQueue GetSingle(EntityKeyFields entityKeys)
        {
            BusinessProcessQueueKeys keys = entityKeys as BusinessProcessQueueKeys;
            return (from a in context.BusinessProcessQueues
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BusinessProcessQueue entity)
        {
            onAdd();
            context.BusinessProcessQueues.Add(entity);
        }

        public void Remove(BusinessProcessQueue entity)
        {
            context.BusinessProcessQueues.Attach(entity);
            context.BusinessProcessQueues.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BusinessProcessQueue entity)
        {
            onUpdate();
            context.BusinessProcessQueues.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BusinessProcessQueue> All()
        {
            return context.BusinessProcessQueues.ToList();
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
	 