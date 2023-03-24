using Logitude.Server.Tools;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   public partial class ServiceProviderSubscriptionDataMapping: IMapping<ServiceProviderSubscriptionPM, ServiceProviderSubscription>
   {
        public void CustomPMToPOCO(ServiceProviderSubscriptionPM entityPM, ServiceProviderSubscription entityPOCO)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
            }

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                entityPM.CreateDate = entityPOCO.CreateDate;
                entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }
        }

        public void CustomPOCOToPM(ServiceProviderSubscriptionPM entityPM, ServiceProviderSubscription entityPOCO)
        {
            
        }
   }
}