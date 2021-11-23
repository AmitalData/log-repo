using CommunicationWorkerRole.Services;
using System;
using System.Text;

namespace CommunicationWorkerRole.Tasks
{
    public class ContainerAutomaticallyClosingTask : TaskManagerBase
    {
        public ContainerAutomaticallyClosingTask(string Id, int tenant)
            : base(Id, tenant)
        {

        }

        public override void StartTask()
        {
            try
            {
                ContainerSchedulerTaskService containerSchedulerTaskService = new ContainerSchedulerTaskService(this);
                containerSchedulerTaskService.ExecuteDailyAutomaticallyClosingContainers();
            }
            catch (Exception exception)
            {
                this.HandelContainerAutomaticallyClosingTaskException(exception);
            }
        }

        private void HandelContainerAutomaticallyClosingTaskException(Exception exception)
        {
            string errorMessage = new StringBuilder().Append("Exception Message: ").AppendLine().Append(exception.Message).AppendLine().ToString();
            errorMessage += new StringBuilder().Append("Stack Trace:").AppendLine().Append(exception.StackTrace).AppendLine().ToString();
            throw new Exception(errorMessage);
        }
    }
}
