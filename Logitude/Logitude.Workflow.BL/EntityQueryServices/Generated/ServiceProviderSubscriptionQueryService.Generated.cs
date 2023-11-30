 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.BL.EntityDataMappings;
using Logitude.Workflow.Data.Repositories;
using Logitude.Workflow.Data.EntityKeys;
using Logitude.Workflow.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Workflow.BL.EntityQueryServices
{ 
   public partial class ServiceProviderSubscriptionQueryService: EntityQueryService<ServiceProviderSubscription,ServiceProviderSubscriptionKeys,ServiceProviderSubscriptionPM,object,ServiceProviderSubscriptionKeys>
   {
   
        ServiceProviderSubscriptionRepository repository;
		IWorkflowContext  context;
        public ServiceProviderSubscriptionQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new ServiceProviderSubscriptionRepository(context);
            Repository = repository;
            mapping = new ServiceProviderSubscriptionDataMapping();
        }

        public ServiceProviderSubscriptionQueryService(ServiceProviderSubscriptionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ServiceProviderSubscriptionDataMapping();
        }

        public ServiceProviderSubscriptionQueryService(IWorkflowContext context)
        {
            this.repository = new ServiceProviderSubscriptionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ServiceProviderSubscriptionDataMapping();
        }
		 
		public  ServiceProviderSubscriptionPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ServiceProviderSubscriptionKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ServiceProviderSubscription entityPOCO)
        {
            ServiceProviderSubscriptionKeys entityKeys = new ServiceProviderSubscriptionKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 