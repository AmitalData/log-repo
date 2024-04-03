using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.BL.QueueMessages;

namespace Logitude.Workflow.BL.EntityUpdateServices
{
    public partial class ServiceProviderSubscriptionUpdateService
    {
        protected override void OnCreating(ServiceProviderSubscriptionPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                SendRenewServiceProviderSubscriptionQueueMessage(entityPM);
            }
        }

        private void SendRenewServiceProviderSubscriptionQueueMessage(ServiceProviderSubscriptionPM entityPM)
        {
            new RenewServiceProviderSubscriptionQueueMessage() { ServiceProviderSubscription = entityPM }.Produce();
        }
    }
}