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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole.Services
{
    public class ReportSchedulerTaskService
    {
        TaskManagerBase currentTask;
        int trackerCounter = 0;
        string[,] trackerLogs = new string[,] //tracker(Step, DateTime)
        {
            {"Prepare report scheduler details", null},
            {"Get report filters", null},
            {"Build Report Data Provider and get stimul report", null},
            {"Export pdf report", null},
            {"Stored pdf report in Blob", null},
            {"Send email to reciepents", null},
        };
        public ReportSchedulerTaskService()
        {
        }
        public ReportSchedulerTaskService(TaskManagerBase taskManagerBase) : this()
        {
            this.currentTask = taskManagerBase;
        }

        public void SendPdfReportToReceipent(TasksSchedulerPM reportTask)
        {
            try
            {
                SchedulerDetails schedulerDetails = GetSchedulerDetails(reportTask);
                reportTask.CreatedBy = schedulerDetails.ReportDetails.CreatedByUserId;
                ReportFliter reportFilter = GetReportFilters(reportTask, schedulerDetails);
                List<ContactList> allPermittedContacts = GetAllPermittedContacts(reportTask.Tenant, null);
                string cardId = GetcardIdValueField(schedulerDetails);
                List<ContactList> allPermittedCards = GetAllPermittedContacts(reportTask.Tenant, cardId);
                allPermittedContacts = allPermittedContacts.Concat(allPermittedCards).ToList();
                schedulerDetails.ReportDetails.Recepients = RemoveNonPermittedContacts(schedulerDetails.ReportDetails.Recepients, allPermittedContacts);
                if (schedulerDetails.ReportDetails.Recepients != null)
                {
                    StiReport stiReport = GetStimulReportByReportFilter(reportFilter);
                    string documentId = GetDocumentIdAfterExport(stiReport, reportTask.Name, reportTask.Tenant);
                    bool isActiveCustomer = CheckIsActiveCustomer(schedulerDetails.ReportDetails.CreatedByUserId, reportTask.Tenant);
                    if (isActiveCustomer)
                    {
                        SendHtmlDocument(documentId, schedulerDetails.ReportDetails.Recepients, reportTask);
                    }
                    else
                    {
                        currentTask.LogWarning("The E-mail was not sent, the customer status is inactive.");
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

        private SchedulerDetails GetSchedulerDetails(TasksSchedulerPM reportTask)
        {
            SchedulerDetails schedulerDetails = LogitudeXmlSerializer.DeserializeObject<SchedulerDetails>(reportTask.SchedulerDetailsXML);
            schedulerDetails.Tenant = reportTask.Tenant;
            schedulerDetails = ModifyNullFilters(schedulerDetails);

            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;
            return schedulerDetails;
        }
        
        private string GetcardIdValueField(SchedulerDetails schedulerDetails)
        {
            string cardId = null;
            schedulerDetails.ReportDetails.ReportFilterItems.ForEach(item => {
                if (item.FieldName == "GLAccountId")
                {
                    if(item.FieldValue != null)
                        cardId = item.FieldValue.ToString();
                }
            });
            return cardId;
        }

        private ReportSchedulerRecepients RemoveNonPermittedContacts(ReportSchedulerRecepients recepients, List<ContactList> allPermittedContacts)
        {
            List<ActivatedEmail> toEmails = FillAllRecepients(recepients.To);
            List<ActivatedEmail> ccEmails = FillAllRecepients(recepients.Cc);
            List<ActivatedEmail> bccEmails = FillAllRecepients(recepients.Bcc);
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

        private List<ActivatedEmail> FillAllRecepients(string recepients)
        {
            List<ActivatedEmail> activatedEmails = new List<ActivatedEmail>();
            ActivatedEmail activatedEmail = null;
            string[] allRecepients = recepients.Split(';');
            for (int i = 0; i < allRecepients.Length; i++)
            {
                activatedEmail = new ActivatedEmail
                {
                    To = allRecepients[i],
                    IsActive = false
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
            if(recepients.Length > 0)
                recepients = recepients.Substring(0, recepients.Length - 1);
            return recepients;
        }

        private List<ContactList> GetAllPermittedContacts(int tenant, string cardId)
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

        private ReportFliter GetReportFilters(TasksSchedulerPM reportTask, SchedulerDetails schedulerDetails)
        {
            ReportQuery reportQuery = new ReportQuery(reportTask.Tenant);
            string reportCode = reportQuery.GetReportCodeById(reportTask.EntityId, reportTask.Tenant);

            ReportFliter reportFilter = new ReportFliter
            {
                ReportId = reportTask.EntityId,
                ReportName = reportTask.Name,
                UserId = reportTask.CreatedBy,
                QueryFilterItemLists = schedulerDetails.ReportDetails.ReportFilterItems,
                DefaultTemplateId = schedulerDetails.ReportDetails.ReportTemplateId,
                tenant = schedulerDetails.Tenant,
                ReportCode = reportCode
            };

            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;
            return reportFilter;
        }

        private StiReport GetStimulReportByReportFilter(ReportFliter reportFilter)
        {
            StiReport stiReport = null;
            if (reportFilter != null)
            {
                ReportHelper reportHelper = new ReportHelper();
                stiReport = reportHelper.GetStimulReportByReportFilter(reportFilter);
            }

            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;
            return stiReport;
        }

        private string GetDocumentIdAfterExport(StiReport stiReport, string reportName, int tenant)
        {
            string documentId = String.Empty;
            MemoryStream memoryStream = new MemoryStream();
            stiReport.ExportDocument(StiExportFormat.Pdf, memoryStream);
            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;

            if (memoryStream != null)
            {
                documentId = CreateDocument(reportName, tenant, memoryStream);
            }
            return documentId;
        }

        private string CreateDocument(string reportName, int tenant, MemoryStream memoryStream)
        {
            byte[] ByteData = memoryStream.ToArray();

            DocumentRepository documentRepository = new DocumentRepository(tenant);
            Document document = new Document()
            {
                FileName = reportName,
                CreateDate = DateTime.Now,
                Extension = "pdf",
                FileSize = ByteData.Length,
                Tenant = tenant,
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "reports",
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();
            StoredDocumentInBlob(document, tenant, ByteData);
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

        private bool CheckIsActiveCustomer(string customerId, int tenant)
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactList contact = contactQuery.GetContactListsById(customerId, tenant);
            if(contact == null ) contactQuery.GetContactListsById(customerId, 0);
            if (contact != null && !contact.InActive) return true;
            return false;
        }

        private void SendHtmlDocument(string documentId, ReportSchedulerRecepients recepients, TasksSchedulerPM reportTask)
        {
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            Byte[] htmlData = enc.GetBytes("");
            string reportTableId = GetReportTableId(reportTask.Tenant);
            htmlEditorHelper.SendHtmlDocument(htmlData, null, null, reportTask.Tenant, recepients.To, reportTask.Name, recepients.Cc, recepients.Bcc, reportTask.CreatedBy, reportTask.EntityId, reportTableId, documentId + ",", "", "", "");
            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;
        }

        private string GetReportTableId(int tenant)
        {
            ObjectTableQuery objectTableQuery = new ObjectTableQuery(tenant);
            string reportId = objectTableQuery.GetObjectTableIdByName("Report");
            return reportId;
        }

        private string GetAllTaskLogs()
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
    public class ActivatedEmail
    {
        public string To;
        public bool IsActive;
    }
}

