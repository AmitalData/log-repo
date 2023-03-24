using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.Data.EntityPOCOs;

namespace Logitude.Workflow.BL.EntityQueryServices
{
    public partial class ServiceProviderSubscriptionQueryService
    {
        public ServiceProviderSubscriptionPM GetSingleByWorkflowNumber(string workflowNumber, int tenant)
        {
            ServiceProviderSubscription entity = repository.GetSingleByWorkflowNumber(workflowNumber, tenant);
            if (entity != null)
            {
                EntityPM = new ServiceProviderSubscriptionPM();
                mapping.CustomPOCOToPM(EntityPM, entity);
                mapping.POCOToPM(EntityPM, entity);
            }
            return EntityPM;
        }
    }
}