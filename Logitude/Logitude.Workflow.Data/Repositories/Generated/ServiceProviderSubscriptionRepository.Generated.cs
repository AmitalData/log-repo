 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.Data.Repositories
{
   public partial class ServiceProviderSubscriptionRepository:IRepository<ServiceProviderSubscription>
   {
   
        private IWorkflowContext currentContext;
        public ServiceProviderSubscriptionRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public ServiceProviderSubscriptionRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  ServiceProviderSubscription GetSingle(string id, int tenant)
        {
            return (from a in context.ServiceProviderSubscriptions
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ServiceProviderSubscription> GetAll(int tenant)
        {
            return from a in context.ServiceProviderSubscriptions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ServiceProviderSubscription GetSingle(EntityKeyFields entityKeys)
        {
            ServiceProviderSubscriptionKeys keys = entityKeys as ServiceProviderSubscriptionKeys;
            return (from a in context.ServiceProviderSubscriptions
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ServiceProviderSubscription entity)
        {
            onAdd();
            context.ServiceProviderSubscriptions.Add(entity);
        }

        public void Remove(ServiceProviderSubscription entity)
        {
            context.ServiceProviderSubscriptions.Attach(entity);
            context.ServiceProviderSubscriptions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ServiceProviderSubscription entity)
        {
            onUpdate();
            context.ServiceProviderSubscriptions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ServiceProviderSubscription> All()
        {
            return context.ServiceProviderSubscriptions.ToList();
        }

        private IWorkflowContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 