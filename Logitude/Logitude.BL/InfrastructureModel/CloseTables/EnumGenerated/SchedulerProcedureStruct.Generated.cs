

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Logitude.BL.InfrastructureModel.CloseTables
{
    public struct SchedulerProcedureValues
    {   
       public const string DeleteDoneQueueMessagesTask = "DeleteDoneQueueMessagesTask";  
       public const string DeleteOldAPILogsTask = "DeleteOldAPILogsTask";  
       public const string DeleteOldAuthenticationTokensTask = "DeleteOldAuthenticationTokensTask";  
       public const string DeleteOldErrorLogsQueueMessagesTask = "DeleteOldErrorLogsQueueMessagesTask";  
       public const string DeleteOldQueueMessageMoreDetailsTask = "DeleteOldQueueMessageMoreDetailsTask";  
       public const string DeleteQueueMessagesDetailsTask = "DeleteQueueMessagesDetailsTask";  
       public const string DeleteTaskSchedulerHistoriesTask = "DeleteTaskSchedulerHistoriesTask";  
       public const string FTPSchedulerTask = "FTPSchedulerTask";  
       public const string FutureOpenChequesTask = "FutureOpenChequesTask";  
       public const string PayableARPaymentChequeTask = "PayableARPaymentChequeTask";  
       public const string RetriesAndReschedulingTask = "RetriesAndReschedulingTask";  
       public const string SFTPSchedulerTask = "SFTPSchedulerTask";  
       public const string TestLoggingInfoWithExceptionIfCurrentMinuteisEvenTask = "TestLoggingInfoWithExceptionIfCurrentMinuteisEvenTask";  
       public const string TestLoggingWarningTask = "TestLoggingWarningTask";  
       public const string TestUnexpectedShutDownHandling = "TestUnexpectedShutDownHandling";  
       public const string UpdateTimeManagementDurations = "UpdateTimeManagementDurations";  
       public const string TestLogToFileTask = "TestLogToFileTask";  
    }
}

