using CommunicationWorkerRole.ReportScheduler;
using CommunicationWorkerRole.Tasks;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.FTP;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Helpers;
using Logitude.Accounting.Data.Repositories;
using System.Web;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.CloseTables;

namespace CommunicationWorkerRole.Services
{
    public class CustomerDebNotificationsTaskService
	{

        int trackerCounter = 0;
        string[,] trackerLogs = new string[,] //tracker(Step, DateTime)
        {
            {"Prepare report scheduler details", null},
            {"Get report filters", null},
            {"Build Report Data Provider and get stimul report", null},
            {"Export pdf report", null},
            {"Stored pdf report in Blob", null},
            {"Send email to reciepents", null},
            {"", null},
            {"", null},
            {"", null},

        };

        TaskManagerBase currentTask;
		ReportSchedulerTaskService reportSchedulerTaskService;

		public CustomerDebNotificationsTaskService(TaskManagerBase task)
        {
            this.currentTask = task;
			this.reportSchedulerTaskService = new ReportSchedulerTaskService(this.currentTask);
			this.reportSchedulerTaskService.IsCUstomerDebitNotification = true;
		}

        public void RunTask(TasksSchedulerPM reportTask)
        {
            try
            {

				CustomerDebtNotificationRepository customerDebtNotificationRepository = new CustomerDebtNotificationRepository(reportTask.Tenant);
                CustomerDebtNotification customerDebtNotification =  customerDebtNotificationRepository.GetCustomerDebtNotificationByTaskSchudler(reportTask.Tenant,reportTask.Id);
                if(customerDebtNotification == null || customerDebtNotification.InActive == IsActiveEnum.NotActive)
				{
					this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Customer Debt Notification Task is not active or not found."));
					return;
				}
                if (!string.IsNullOrEmpty(customerDebtNotification.AccountId))
                {
                    SendReport(reportTask, customerDebtNotification, customerDebtNotification.AccountId);
                }
                else if (customerDebtNotification.InActive == IsActiveEnum.ActiveAllCustomers)
                {
					this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("status TasksScheduler Maintainence ActiveAllCustomers"));

					GLAccountRepository glAccountRepository = new GLAccountRepository(reportTask.Tenant);
                    var GLAccountByTenantAndCustomerDebtNotification = glAccountRepository.GetGLAccountByTenantAndCustomerDebtNotification(reportTask.Tenant);

                    foreach (var account in GLAccountByTenantAndCustomerDebtNotification)
                    {
                        SendReport(reportTask, customerDebtNotification, account.Id);
                    }
                }
                else
                {
					this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("status TasksScheduler Maintainence ActiveSelectedCustomers"));
					return;
				}

			}
            catch (Exception ex)
            {
                string logsMessage = this.reportSchedulerTaskService.GetAllTaskLogs();
                string errorMessage = new StringBuilder().Append(logsMessage).AppendLine().ToString();
                errorMessage += new StringBuilder().Append("Exception Message: ").AppendLine().Append(ex.Message).AppendLine().ToString();
                errorMessage += new StringBuilder().Append("Stack Trace:").AppendLine().Append(ex.StackTrace).AppendLine().ToString();

                throw new Exception(errorMessage);
            }
        }

		private void SendReport(TasksSchedulerPM reportTask, CustomerDebtNotification customerDebtNotification,string accountId)
		{
			if (!CalculateDebts(customerDebtNotification, accountId))
			{
				this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("No Debt for GLAccountId: " + accountId + "task cancelled"));
				return;
			}
			else
			{
				

				SchedulerDetails schedulerDetails = this.reportSchedulerTaskService.GetSchedulerDetails(reportTask);
				if (string.IsNullOrEmpty(schedulerDetails.ReportDetails.Recepients.To))
				{
					schedulerDetails.ReportDetails.Recepients.To = GetAllContatByGLAccountId(reportTask.Tenant, accountId);
                    if (string.IsNullOrEmpty(schedulerDetails.ReportDetails.Recepients.To))
                    {
                        this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("No recepients found for GLAccountId: " + accountId + ". Task cancelled."));
                        return;
                    }
				}
				this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Preparing report scheduler details" + accountId));
				this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
				this.trackerCounter += 1;


				schedulerDetails.ReportDetails.ReportFilterItems.ForEach(filterItem => {
					if (filterItem.FieldName == "GLAccountId")
					{
						filterItem.FieldValue = accountId;
					}
				});
				reportTask.CreatedBy = schedulerDetails.ReportDetails.CreatedByUserId;
				ReportFliter reportFilter = this.reportSchedulerTaskService.GetReportFilters(reportTask, schedulerDetails);

				if (reportTask.ResultType == null || reportTask.ResultType == "Email")
				{
					this.reportSchedulerTaskService.SendPdfReportToReceipent(reportTask, schedulerDetails, reportFilter);
				}
				else if (reportTask.ResultType == "FTP")
				{
					this.reportSchedulerTaskService.SendReportToFTP(reportTask, schedulerDetails, reportFilter);
				}
			}
		}
		private bool CalculateDebts(CustomerDebtNotification customerDebtNotification,string accountId)
		{
			GLAccountMoreDataRepository glaccountMoreDataRepository = new GLAccountMoreDataRepository(customerDebtNotification.Tenant);
            GLAccountMoreData glAccountMoreData = glaccountMoreDataRepository.GetSingle(accountId, customerDebtNotification.Tenant);
            bool isDebt = false;
            switch (customerDebtNotification.TypesDebts)
            {
				case TypesDebtsEnum.Obligato:
                    decimal obligoAmount = (glAccountMoreData.TotFutureOpenChequesInLocalCur ?? 0m) + glAccountMoreData.BalanceInLocalCurrency;

					if (customerDebtNotification.DebtLevel == DebtLevelEnum.TotalAmount)
                    {
						isDebt = obligoAmount > customerDebtNotification.DebtLevelAmount;
					}
					else if(customerDebtNotification.DebtLevel == DebtLevelEnum.Percentage)
					{
						isDebt = glAccountMoreData.BalanceInLocalCurrency < (obligoAmount * ((customerDebtNotification.DebtLevelAmount / 100) + 1));
					}
					break;
				case TypesDebtsEnum.AccountingBalance:
					isDebt = glAccountMoreData.BalanceInLocalCurrency > customerDebtNotification.DebtLevelAmount;
					break;
				case TypesDebtsEnum.BalanceRegarding:
					isDebt = glAccountMoreData.LocalBalanceInDue > customerDebtNotification.DebtLevelAmount;
					break;
			}     
			return isDebt;
		}
        private string GetAllContatByGLAccountId(int tenant,string glaccountId)
		{
			string recepients = string.Empty;

			ContactRepository contactRep = new ContactRepository(tenant);
			List<Contact> contacts = contactRep.GetContactsForAccountingByGLAccountId(glaccountId, tenant);
	
			contacts.ForEach(contact => {
				if (!string.IsNullOrEmpty(contact.Email))
				{
					recepients += contact.Email + ";";
				}
			});
			if (recepients.Length > 0)
				recepients = recepients.Substring(0, recepients.Length - 1);
			return recepients;
		}
    }
}

