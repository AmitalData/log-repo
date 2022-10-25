using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services.Logbox
{
    public class LogboxExceptionHandler
    {
        public static LogboxExceptionHandlerResult Handle(IQueueService queueService, LogboxExceptionHandlerArgs logboxExceptionHandlerArgs)
        {
            LogboxExceptionHandlerResult logboxExceptionHandlerResult = new LogboxExceptionHandlerResult();

            logboxExceptionHandlerResult.FailMessage = logboxExceptionHandlerArgs.Exception.Message + " " + DateTime.Now;
            logboxExceptionHandlerResult.ErrorMessage = logboxExceptionHandlerArgs.Exception.Message + Environment.NewLine;
            if (logboxExceptionHandlerArgs.Exception.InnerException != null)
            {
                logboxExceptionHandlerResult.ErrorMessage = logboxExceptionHandlerResult.ErrorMessage + " (" + logboxExceptionHandlerArgs.Exception.InnerException.Message + ")" + Environment.NewLine;
            }

            logboxExceptionHandlerResult.ErrorMessage = logboxExceptionHandlerResult.ErrorMessage + logboxExceptionHandlerArgs.Exception.StackTrace + Environment.NewLine;
            if (logboxExceptionHandlerArgs.queueResponse.RetryNumber <= 1)
            {
                queueService.Delay(new TimeSpan(0, 0, 0, 5));
            }

            if (logboxExceptionHandlerArgs.queueResponse.RetryNumber > 1 && logboxExceptionHandlerArgs.queueResponse.RetryNumber <= 2)
            {
                queueService.Delay(new TimeSpan(0, 0, 0, 10));
            }

            if (logboxExceptionHandlerArgs.queueResponse.RetryNumber >= 3)
            {
                queueService.CompleteAsFailed();
            }

            return logboxExceptionHandlerResult;
        }
    }

    public class LogboxExceptionHandlerArgs
    {
        public QueueResponse queueResponse { get; set; }
        public Exception Exception { get; set; }
        public int Tenant { get; set; }
    }

    public class LogboxExceptionHandlerResult
    {
        public string FailMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}
