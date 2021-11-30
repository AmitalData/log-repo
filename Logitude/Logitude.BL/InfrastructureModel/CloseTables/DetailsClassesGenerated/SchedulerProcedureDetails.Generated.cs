

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.CloseTablesClasses;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs; 
using Simplog.Data.InfrastructureModel;

namespace Logitude.BL.InfrastructureModel
{
   public class SchedulerProcedureDetails : SchedulerProcedure, ICloseTable<SchedulerProcedure, SchedulerProcedureDetails>
   {
       public List<SchedulerProcedureDetails> GetAll()
       {
		    var all = new List<SchedulerProcedureDetails>();  
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "DeleteDoneQueueMessagesTask", 
                Name = "DeleteDoneQueueMessagesTask", 
                SearchFields = "DeleteDoneQueueMessagesTask,DeleteDoneQueueMessagesTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "DeleteOldAPILogsTask", 
                Name = "DeleteOldAPILogsTask", 
                SearchFields = "DeleteOldAPILogsTask,DeleteOldAPILogsTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "DeleteOldCommunicationLogsTask", 
                Name = "DeleteOldCommunicationLogsTask", 
                SearchFields = "DeleteOldCommunicationLogsTask,DeleteOldCommunicationLogsTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "DeleteOldAuthenticationTokensTask", 
                Name = "DeleteOldAuthenticationTokensTask", 
                SearchFields = "DeleteOldAuthenticationTokensTask,DeleteOldAuthenticationTokensTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "DeleteOldErrorLogsQueueMessagesTask", 
                Name = "DeleteOldErrorLogsQueueMessagesTask", 
                SearchFields = "DeleteOldErrorLogsQueueMessagesTask,DeleteOldErrorLogsQueueMessagesTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "DeleteOldQueueMessageMoreDetailsTask", 
                Name = "DeleteOldQueueMessageMoreDetailsTask", 
                SearchFields = "DeleteOldQueueMessageMoreDetailsTask,DeleteOldQueueMessageMoreDetailsTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "DeleteQueueMessagesDetailsTask", 
                Name = "DeleteQueueMessagesDetailsTask", 
                SearchFields = "DeleteQueueMessagesDetailsTask,DeleteQueueMessagesDetailsTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "DeleteTaskSchedulerHistoriesTask", 
                Name = "DeleteTaskSchedulerHistoriesTask", 
                SearchFields = "DeleteTaskSchedulerHistoriesTask,DeleteTaskSchedulerHistoriesTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "FTPSchedulerTask", 
                Name = "FTPSchedulerTask", 
                SearchFields = "FTPSchedulerTask,FTPSchedulerTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "FutureOpenChequesTask", 
                Name = "FutureOpenChequesTask", 
                SearchFields = "FutureOpenChequesTask,FutureOpenChequesTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "PayableARPaymentChequeTask", 
                Name = "PayableARPaymentChequeTask", 
                SearchFields = "PayableARPaymentChequeTask,PayableARPaymentChequeTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "RetriesAndReschedulingTask", 
                Name = "RetriesAndReschedulingTask", 
                SearchFields = "RetriesAndReschedulingTask,RetriesAndReschedulingTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "SFTPSchedulerTask", 
                Name = "SFTPSchedulerTask", 
                SearchFields = "SFTPSchedulerTask,SFTPSchedulerTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "TestLoggingInfoWithExceptionIfCurrentMinuteisEvenTask", 
                Name = "TestLoggingInfoWithExceptionIfCurrentMinuteisEvenTask", 
                SearchFields = "TestLoggingInfoWithExceptionIfCurrentMinuteisEvenTask,TestLoggingInfoWithExceptionIfCurrentMinuteisEvenTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "TestLoggingWarningTask", 
                Name = "TestLoggingWarningTask", 
                SearchFields = "TestLoggingWarningTask,TestLoggingWarningTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "TestUnexpectedShutDownHandling", 
                Name = "TestUnexpectedShutDownHandling", 
                SearchFields = "TestUnexpectedShutDownHandling,TestUnexpectedShutDownHandling", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "UpdateTimeManagementDurations", 
                Name = "UpdateTimeManagementDurations", 
                SearchFields = "UpdateTimeManagementDurations,UpdateTimeManagementDurations", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "TestLogToFileTask", 
                Name = "TestLogToFileTask", 
                SearchFields = "TestLogToFileTask", 
                Description = "TestLogToFileTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "GLAccountsCSVTask", 
                Name = "GLAccountsCSVTask", 
                SearchFields = "GLAccountsCSVTask, GLAccount", 
                Description = "Send Updated GLAccounts list to hybrid", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "QuoteAutomaticallyClosingTask", 
                Name = "QuoteAutomaticallyClosingTask", 
                SearchFields = "QuoteAutomaticallyClosingTask", 
                Description = "QuoteAutomaticallyClosingTask", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "DeleteOldCargoTrackingData", 
                Name = "DeleteOldCargoTrackingData", 
                SearchFields = "DeleteOldCargoTrackingData", 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "ExchangeRateUpdateTask", 
                Name = "ExchangeRateUpdateTask", 
                SearchFields = "ExchangeRateUpdateTask", 
                Description = "ExchangeRateUpdateTask", 
                IsInternallyDefined = false, 
			});
			 
            all.Add(new SchedulerProcedureDetails()
            {    
                Code = "ContainerAutomaticallyClosingTask", 
                Name = "ContainerAutomaticallyClosingTask", 
                SearchFields = "ContainerAutomaticallyClosingTask", 
                Description = "ContainerAutomaticallyClosingTask", 
                IsInternallyDefined = false, 
			});
			
            return all;
       }

	    public void MapPoco(SchedulerProcedure newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(SchedulerProcedure rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

