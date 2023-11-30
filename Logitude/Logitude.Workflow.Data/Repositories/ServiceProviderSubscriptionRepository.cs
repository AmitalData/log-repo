using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.Workflow.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.Data.Repositories
{
   public partial class ServiceProviderSubscriptionRepository:IRepository<ServiceProviderSubscription>
   {
		public List<ServiceProviderSubscription> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public ServiceProviderSubscription GetSingleByWorkflowNumber(string workflowNumber, int tenant)
        {
            return (from a in context.ServiceProviderSubscriptions
                    where a.WorkflowNumber == workflowNumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
    }
}