using CommunicationWorkerRole.Tasks;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.FTP;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebFreight.Web.Helpers;
using System.Data.SqlClient;
using System.Data;


namespace CommunicationWorkerRole.Services
{
    public class QueryReportService
	{

        int trackerCounter = 0;
        string[,] trackerLogs = new string[,] //tracker(Step, DateTime)
        {
            {"Prepare report scheduler details", null},
            {"Get DataTable By Procedure", null},
            {"Build DataTable To Excel", null},           
            {"Stored pdf report in Blob", null},
            {"Send email to reciepents", null},
            {"", null},
            {"", null},
            {"", null},

        };

        TaskManagerBase currentTask;
        public QueryReportService(TaskManagerBase task)
        {
            this.currentTask = task;
        }

        public void RunTask(TasksSchedulerPM reportTask)
        {
            try
            {
                SchedulerDetails schedulerDetails = GetSchedulerDetails(reportTask);
                reportTask.CreatedBy = schedulerDetails.ReportDetails.CreatedByUserId;
                string procedureName = schedulerDetails.ReportDetails.ProcedureName;

				if (procedureName != null)
                {
					DataTable dataTable= GetDataTableByProc(procedureName, reportTask.Tenant);

					var xls = new ExportToExcelHelper();

					var res = xls.DataTableToExcel(dataTable, "report1", true, procedureName + ".xlsx");

					this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
					this.trackerCounter += 1;

					if (reportTask.ResultType == null || reportTask.ResultType == "Email")
					{
						SendExelReportToReceipent(reportTask, schedulerDetails, res);
					}
					else if (reportTask.ResultType == "FTP")
					{
						
					}
				}
            }
            catch (Exception ex)
            {
                string logsMessage = this.GetAllTaskLogs();
                string errorMessage = new StringBuilder().Append(logsMessage).AppendLine().ToString();
                errorMessage += new StringBuilder().Append("Exception Message: ").AppendLine().Append(ex.Message).AppendLine().ToString();
                errorMessage += new StringBuilder().Append("Stack Trace:").AppendLine().Append(ex.StackTrace).AppendLine().ToString();

                throw new Exception(errorMessage);
            }
        }
		private DataTable GetDataTableByProc(string proceName,int tenant)
		{			
			DataTable dataTable = new DataTable();
			string connectionString = TenantServerConfigration.GetDbConnection(0);
			string query =  proceName;

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@tenant", tenant);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						// Load the reader data into the DataTable
						dataTable.Load(reader);
					}
				}
			}
			this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
			this.trackerCounter += 1;
			return dataTable;
		}
        private void SendExelReportToReceipent(TasksSchedulerPM reportTask, SchedulerDetails schedulerDetails, byte[] byteData)
        {
            this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Preparing report data"));
            schedulerDetails.ReportDetails.Recepients = GetReportPermittedContacts(reportTask, schedulerDetails);
            if (schedulerDetails.ReportDetails.Recepients != null)
            {
                TryToSendReportAfterMeetACertainConditions(reportTask, schedulerDetails, byteData);
            }
        }

        private void TryToSendReportAfterMeetACertainConditions(TasksSchedulerPM reportTask, SchedulerDetails schedulerDetails,byte[] byteData)
        {
			if (byteData == null || byteData.Length == 0)
            {
                this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Report Is Empty"));
            }
            else
            {
				this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Before CreateDocument"));
				string documentId = CreateDocument(new ReportScedulerDocumentArgs { Name = reportTask.Name, Format = "xlsx", Tenant = reportTask.Tenant, ByteData = byteData });
				this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("SendHtmlDocument report to reciepents"));
				SendHtmlDocument(new SendHtmlDocumentArgs() { documentId = documentId, recepients = schedulerDetails.ReportDetails.Recepients, reportTask = reportTask, stiReport = null });
				this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("SendHtmlDocument report to reciepents finished successfully"));
				
            }
        }
		
		private ReportSchedulerRecepients GetReportPermittedContacts(TasksSchedulerPM reportTask, SchedulerDetails schedulerDetails)
        {
            List<ContactList> allPermittedContacts = GetAllPermittedContacts(reportTask.Tenant, null);
            string mainCustomerFieldName = string.IsNullOrEmpty(schedulerDetails.ReportDetails.MainCustomerFieldName) ? "GLAccountId" : schedulerDetails.ReportDetails.MainCustomerFieldName;
            string gLAccountId = GetFilterFieldValueByName(schedulerDetails.ReportDetails.ReportFilterItems, mainCustomerFieldName);
            List<ContactList> allPermittedCards = GetAllPermittedContacts(reportTask.Tenant, gLAccountId);
            allPermittedContacts = allPermittedContacts.Concat(allPermittedCards).ToList();
            ReportSchedulerRecepients recepients = RemoveNonPermittedContacts(schedulerDetails.ReportDetails.Recepients, allPermittedContacts);
            return recepients;
        }

        private SchedulerDetails GetSchedulerDetails(TasksSchedulerPM reportTask)
        {
            SchedulerDetails schedulerDetails = LogitudeXmlSerializer.DeserializeObject<SchedulerDetails>(reportTask.SchedulerDetailsXML);
            schedulerDetails.Tenant = reportTask.Tenant;
            schedulerDetails = ModifyNullFilters(schedulerDetails);

            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;
            return schedulerDetails;
        }

        private string GetFilterFieldValueByName(List<QueryFilterItem> reportFilterItems, string fieldName)
        {
            string fieldValue = null;
            reportFilterItems.ForEach(item => {
                if (item.FieldName == fieldName)
                {
                    if (item.FieldValue != null)
                        fieldValue = item.FieldValue.ToString();
                }
            });
            return fieldValue;
        }

        public ReportSchedulerRecepients RemoveNonPermittedContacts(ReportSchedulerRecepients recepients, List<ContactList> allPermittedContacts)
        {
            List<ActivatedEmail> toEmails = FillAllRecepients(recepients.To, false);
            List<ActivatedEmail> ccEmails = FillAllRecepients(recepients.Cc, false);
            List<ActivatedEmail> bccEmails = FillAllRecepients(recepients.Bcc, false);
            string inActiveRecipients = "";
            allPermittedContacts.ForEach(contact => {
                toEmails.ForEach(to => {
                    if (contact.Email == to.To)
                    {
                        if (!contact.InActive) to.IsActive = true;
                        else inActiveRecipients += contact.Email + ',';
                    }
                });
                ccEmails.ForEach(cc => {
                    if (contact.Email == cc.To)
                    {
                        if (!contact.InActive) cc.IsActive = true;
                        else inActiveRecipients += contact.Email + ',';
                    }
                });
                bccEmails.ForEach(bcc => {
                    if (contact.Email == bcc.To)
                    {
                        if (!contact.InActive) bcc.IsActive = true;
                        else inActiveRecipients += contact.Email + ',';
                    }
                });
            });

            if (!string.IsNullOrEmpty(inActiveRecipients))
            {
                inActiveRecipients = this.RemoveDuplicateEmails(inActiveRecipients);
                string warningMessage = "The E-mail was not sent to " + inActiveRecipients;
                warningMessage = ReformatWarningMessage(warningMessage);
                currentTask.LogWarning(warningMessage);
            }

            recepients.To = FillOnlyActiveRecepients(toEmails);
            recepients.Cc = FillOnlyActiveRecepients(ccEmails);
            recepients.Bcc = FillOnlyActiveRecepients(bccEmails);
            if (string.IsNullOrEmpty(recepients.To))
                return null;
            return recepients;
        }

        private string ReformatWarningMessage(string warningMessage)
        {
            warningMessage = warningMessage.Remove(warningMessage.LastIndexOf(','));
            if (warningMessage.IndexOf(',') > 0)
                warningMessage = warningMessage.Substring(0, warningMessage.LastIndexOf(',')) + " and " + warningMessage.Substring(warningMessage.LastIndexOf(',') + 1, warningMessage.Length - warningMessage.LastIndexOf(',') - 1);
            return warningMessage;
        }

        private string RemoveDuplicateEmails(string inActiveRecipientsEmails)
        {
            string[] recepientsEmails = inActiveRecipientsEmails.Split(',');
            string[] recepients = recepientsEmails.Distinct().ToArray();
            string emails = string.Join(",", recepients);

            return emails;
        }

        private List<ActivatedEmail> FillAllRecepients(string recepients, bool isActive)
        {
            List<ActivatedEmail> activatedEmails = new List<ActivatedEmail>();
            ActivatedEmail activatedEmail = null;
            string[] allRecepients = recepients.Split(';');
            for (int i = 0; i < allRecepients.Length; i++)
            {
                activatedEmail = new ActivatedEmail
                {
                    To = allRecepients[i],
                    IsActive = isActive
                };
                activatedEmails.Add(activatedEmail);
            }
            return activatedEmails;
        }

        private string FillOnlyActiveRecepients(List<ActivatedEmail> emails)
        {
            string recepients = "";
            emails.ForEach(to => {
                if (to.IsActive)
                    recepients += to.To + ";";
            });
            if (recepients.Length > 0)
                recepients = recepients.Substring(0, recepients.Length - 1);
            return recepients;
        }

        public List<ContactList> GetAllPermittedContacts(int tenant, string cardId)
        {
            QueryOperations queryOperations = new QueryOperations()
            {
                ObjectTableName = "Contact",
                PageIndex = 0,
                PageSize = 30,
                QuerySection = "Contacts",
                SortByColumnName = null,
                SortDirectin = "Ascending",
                GetAll = false,
            };

            queryOperations.SetFilter("NotEqual", "", true, "NotEqual", null, true);
            queryOperations.SetFilter("InActive", false, true, "Equals", null, true);

            if (!string.IsNullOrEmpty(cardId))
                queryOperations.SetFilter("CardId", cardId, true, "InListExact", null, true);

            ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRepository = new ContactRepository(MyContext);
            IQueryable<Contact> entityPocos = contactRepository.GetContacts(tenant);
            GenericFilter genericFilter = new GenericFilter();

            ContactQuery contactQuery = new ContactQuery(contactRepository);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            ContactCustomFilter customfilters = new ContactCustomFilter(tenant);
            entityPocos = customfilters.GetFilteredQuery(queryOperations, entityPocos);

            entityPocos = genericFilter.GetFilteredQuery<Contact>(nonListQueryOperation, entityPocos);
            int skippedEntities = queryOperations.PageIndex;

            IQueryable<ContactList> entityLists = contactQuery.GetIQueryableEntityList(entityPocos);
            entityLists = genericFilter.GetFilteredQuery<ContactList>(listQueryOperation, entityLists);
            return entityLists.ToList();
        }

        private SchedulerDetails ModifyNullFilters(SchedulerDetails schedulerDetails)
        {
            schedulerDetails.ReportDetails.ReportFilterItems.ForEach(filterItem => {
                if (filterItem.FieldValue.GetType().Name == "XmlNode[]")
                    filterItem.FieldValue = null;
            });

            return schedulerDetails;
        }
              
        public string CreateDocument(ReportScedulerDocumentArgs reportScedulerDocumentArgs)
        {
            DocumentRepository documentRepository = new DocumentRepository(reportScedulerDocumentArgs.Tenant);
            Document document = new Document()
            {
                FileName = reportScedulerDocumentArgs.Name,
                CreateDate = DateTime.Now,
                Extension = reportScedulerDocumentArgs.Format,
                FileSize = reportScedulerDocumentArgs.ByteData.Length,
                Tenant = reportScedulerDocumentArgs.Tenant,
                Id = IdCounter.GetNumber("Document", reportScedulerDocumentArgs.Tenant),
                HasFile = true,
                Folder = "reports",
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();
            StoredDocumentInBlob(document, reportScedulerDocumentArgs.Tenant, reportScedulerDocumentArgs.ByteData);
            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;

            return document.Id;
        }

        private void StoredDocumentInBlob(Document document, int tenant, byte[] ByteData)
        {
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = "reports",
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = document.FileSize,
            };

            storageservice.Write(ByteData, fileInfo);
        }
        private void SendHtmlDocument(SendHtmlDocumentArgs args)
        {
			HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
			string htmlString = "<html><head><meta http- equiv='Content- Type' content= 'text/html; charset = iso-8859-1' > <style type='text/css' style= 'display: none; '></style></head><body>";
			htmlString += "</body></html>";
			byte[] bytedata = Encoding.UTF8.GetBytes(htmlString);
			string reportTableId = GetReportTableId(args.reportTask.Tenant);
            string subject =  "מצורף דוח: " + args.reportTask.Name;
            htmlEditorHelper.SendHtmlDocument(bytedata, null, null, args.reportTask.Tenant, args.recepients.To, subject, args.recepients.Cc, args.recepients.Bcc, args.reportTask.CreatedBy, args.reportTask.EntityId, reportTableId, args.documentId + ",", "", "", "");
            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;
        }
        private string GetReportTableId(int tenant)
        {
            ObjectTableQuery objectTableQuery = new ObjectTableQuery(tenant);
            string reportId = objectTableQuery.GetObjectTableIdByName("Report");
            return reportId;
        }

        public string GetAllTaskLogs()
        {
            string logsMessage = "";
            int trackerLogsCount;

            for (trackerLogsCount = 0; trackerLogsCount < this.trackerLogs.Length / 2; trackerLogsCount++)
            {
                if (this.trackerLogs[trackerLogsCount, 1] != null)
                {
                    logsMessage += new StringBuilder().Append(this.trackerLogs[trackerLogsCount, 1]).Append(" : ").Append(this.trackerLogs[trackerLogsCount, 0]).Append(" ... Done ").AppendLine().ToString();
                }
                else
                {
                    logsMessage += new StringBuilder().Append(DateTime.Now.ToString()).Append(" : ").Append(this.trackerLogs[trackerLogsCount, 0]).Append(" ... Failed ").AppendLine().ToString();
                    break;
                }
            }

            for (int i = trackerLogsCount + 1; i < this.trackerLogs.Length / 2; i++)
            {
                logsMessage += new StringBuilder().Append("    \t...\t    : ").Append(this.trackerLogs[i, 0]).Append(" ... Stopped").AppendLine().ToString();
            }

            return logsMessage;
        }
    }

}

