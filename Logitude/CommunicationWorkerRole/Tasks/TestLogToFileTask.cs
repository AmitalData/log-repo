using Logitude.Server.Tools.QueueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class TestLogToFileTask : TaskManagerBase
    {
        public TestLogToFileTask(string Id, int tenant)
            : base(Id, tenant)
        {

        }
        public override void StartTask()
        {
            ConcurrentQueueService<LogQueueMessage> queueService = new ConcurrentQueueService<LogQueueMessage>("LogMessagesQueue");

            for (int i = 0; i <= 20; i++)
            {
                var message = string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF") + "Log Info # {0}.\r\n", i);
                queueService.Enqueue(new LogQueueMessage() { FileName = "test-log-queue", FileExtension = "txt", FolderName = "logtest", Message = message, Tenant = this.Task.Tenant });
                LogInfo(message);
            }

        }
    }
}
