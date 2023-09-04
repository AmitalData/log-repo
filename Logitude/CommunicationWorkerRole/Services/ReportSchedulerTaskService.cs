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

namespace CommunicationWorkerRole.Services
{
    public class ReportSchedulerTaskService
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
        public ReportSchedulerTaskService(TaskManagerBase task)
        {
            this.currentTask = task;
        }

        public void RunTask(TasksSchedulerPM reportTask)
        {
            try
            {
                SchedulerDetails schedulerDetails = GetSchedulerDetails(reportTask);
                reportTask.CreatedBy = schedulerDetails.ReportDetails.CreatedByUserId;
                ReportFliter reportFilter = GetReportFilters(reportTask, schedulerDetails);

                if (reportTask.ResultType == null || reportTask.ResultType == "Email")
                {
                    SendPdfReportToReceipent(reportTask, schedulerDetails, reportFilter);
                }
                else if (reportTask.ResultType == "FTP")
                {
                    SendReportToFTP(reportTask, schedulerDetails, reportFilter);
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

        private void SendReportToFTP(TasksSchedulerPM reportTask, SchedulerDetails schedulerDetails, ReportFliter reportFilter)
        {
            this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Preparing report data"));
            MemoryStream memoryStream = GetMemoryStreamAfterExportDocument(reportTask, reportFilter);
            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;
            if (memoryStream != null && schedulerDetails.FTPDetails != null)
            {

                this.trackerLogs[trackerCounter, 0] = "Uploading report to ftp";
                this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
                this.trackerCounter += 1;

                string p_message = "";
                string p_status = "";
                string schedulerFormatExtension = reportTask.Format == "PDF" ? "pdf" : "xlsx";
                var fileName = reportTask.Name + "." + schedulerFormatExtension;
                FTPServiceMod ftpService = new FTPServiceMod(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password);
                ftpService.Upload(fileName, schedulerDetails.FTPDetails.Folder, memoryStream.ToArray(), out p_message, out p_status, true, true);

                if (p_status == "-1")
                {
                    currentTask.LogWarning(p_message);
                }
                else
                {
                    currentTask.LogInfo(p_message);
                }
            }
        }

        private MemoryStream GetMemoryStreamAfterExportDocument(TasksSchedulerPM reportTask, ReportFliter reportFilter)
        {

            string schedulerFormat = (string.IsNullOrEmpty(reportTask.Format) || reportTask.Format == "PDF") ? "pdf" : "Excel";
            StiReport stiReport = GetStimulReportByReportFilter(reportFilter);
            MemoryStream memoryStream = new MemoryStream();
            this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Exporting report to " + schedulerFormat + " file"));
            if (schedulerFormat == "Excel")
            {
                StiExcel2007ExportSettings stiExcelSettings = new StiExcel2007ExportSettings();
                bool useOnePageHeaderAndFooter = reportTask.Format == "EXCL" || reportTask.AdvancedFormat == "OP";
                bool exportDataOnly = reportTask.AdvancedFormat == "DO";
                stiExcelSettings.ExportObjectFormatting = false;
                stiExcelSettings.UseOnePageHeaderAndFooter = useOnePageHeaderAndFooter;
                stiExcelSettings.ExportDataOnly = exportDataOnly;
                StiExcel2007ExportService stiExcelService = new StiExcel2007ExportService();
                stiExcelService.ExportExcel(stiReport, memoryStream, stiExcelSettings);
            }
            else
            {
                stiReport.ExportDocument(StiExportFormat.Pdf, memoryStream);
            }

            return memoryStream;
        }

        private void SendPdfReportToReceipent(TasksSchedulerPM reportTask, SchedulerDetails schedulerDetails, ReportFliter reportFilter)
        {
            this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Preparing report data"));
            schedulerDetails.ReportDetails.Recepients = GetReportPermittedContacts(reportTask, schedulerDetails);
            if (schedulerDetails.ReportDetails.Recepients != null)
            {
                TryToSendReportAfterMeetACertainConditions(reportTask, schedulerDetails, reportFilter);
            }
        }

        private void TryToSendReportAfterMeetACertainConditions(TasksSchedulerPM reportTask, SchedulerDetails schedulerDetails, ReportFliter reportFilter)
        {
            StiReport stiReport = GetStimulReportByReportFilter(reportFilter);
            if (stiReport == null)
            {
                this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Report Is Empty"));
            }
            else
            {
                this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Exporting report to pdf file"));
                string documentId = GetDocumentIdAfterExport(stiReport, reportTask.Name, reportTask.Tenant, schedulerDetails, reportFilter, reportTask);
                SendPdfReportIfIsValid(reportTask, schedulerDetails, documentId, stiReport);
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

        private void SendPdfReportIfIsValid(TasksSchedulerPM reportTask, SchedulerDetails schedulerDetails, string documentId, StiReport stiReport)
        {
            AdditionalValidate additionalValidate = new AdditionalValidate();
            ReportSchedulerRecepients reportRecepients = schedulerDetails.ReportDetails.Recepients;
            string gLAccountId = GetFilterFieldValueByName(schedulerDetails.ReportDetails.ReportFilterItems, "GLAccountId");
            if (!string.IsNullOrEmpty(gLAccountId))
            {
                reportRecepients = GetActiveRecepientsForGLAccount(gLAccountId, reportTask.Tenant, reportRecepients);
                additionalValidate.recepients = reportRecepients;
            }
            this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Validate Selected Partners"));
            ValidateResult result = ValidateSelectedPartners(schedulerDetails.ReportDetails.ReportFilterItems, reportTask.Tenant, additionalValidate);
            ValidateResult gLAccountBalanceInLocalValidateResult = ValidateGLAccountBalanceInLocalCurrency(schedulerDetails.ReportDetails.ReportFilterItems, stiReport, reportTask.Tenant);
            if (result.IsValid && gLAccountBalanceInLocalValidateResult.IsValid)
            {
                this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Sending report to reciepents"));
                SendHtmlDocument(new SendHtmlDocumentArgs() { documentId = documentId, recepients = reportRecepients, reportTask = reportTask, stiReport = stiReport });
                this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Sending report to reciepents finished successfully"));
            }
            else
            {
                LogErrorMessage(result, gLAccountBalanceInLocalValidateResult);
            }

        }

        private void LogErrorMessage(ValidateResult result, ValidateResult gLAccountBalanceInLocalValidateResult) {
            if (!result.IsValid)
            {
                currentTask.LogWarning(result.ErrorMessage);
            }

            if (!gLAccountBalanceInLocalValidateResult.IsValid)
            {
                currentTask.LogWarning(gLAccountBalanceInLocalValidateResult.ErrorMessage);
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
                ReportCode = reportCode,
                IsSchedulerReport = true,
                SendIfEmpty = schedulerDetails.SendIfEmpty,
            };

            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;
            return reportFilter;
        }

        private StiReport GetStimulReportByReportFilter(ReportFliter reportFilter)
        {
            AdvancedDateResolver advancedDateResolver = new AdvancedDateResolver();
            List<QueryFilterItem> reportFilterItems = advancedDateResolver.ResolveDateValues(reportFilter.QueryFilterItemLists);
            reportFilter.QueryFilterItemLists = reportFilterItems;

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

        private string GetDocumentIdAfterExport(StiReport stiReport, string reportName, int tenant, SchedulerDetails schedulerDetails, ReportFliter reportFilter, TasksSchedulerPM reportTask)
        {
            string documentId = String.Empty;
            MemoryStream memoryStream = new MemoryStream();
            if (reportFilter.ReportCode == "LTRP" && schedulerDetails?.ReportDetails?.ReportTemplateType == "E")
            {
                reportTask.Format = "Excel";
                memoryStream = GetMemoryStreamAfterExportDocument(reportTask, reportFilter);
            }
            else
            {
                stiReport.ExportDocument(StiExportFormat.Pdf, memoryStream);
            }
            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;
            if (memoryStream == null)
            {
                return documentId;
            }
            //LogitudeSettings.WorkEnvironment == "cloud" &&
            if (reportFilter.ReportCode == "LTRP" && schedulerDetails?.ReportDetails?.ReportTemplateType == "E")
            {
                documentId = CreateDocument(new ReportScedulerDocumentArgs { Name = reportName, Format = "xlsx", Tenant = tenant, ByteData = memoryStream.ToArray() });
            }
            else
            {
                documentId = CreateDocument(new ReportScedulerDocumentArgs { Name = reportName, Format = "pdf", Tenant = tenant, ByteData = memoryStream.ToArray() });
            }

            return documentId;
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

        private ReportSchedulerRecepients GetActiveRecepientsForGLAccount(string gLAccountId, int tenant, ReportSchedulerRecepients recepients)
        {
            ReportSchedulerRecepients myRecepients = recepients;
            if (string.IsNullOrEmpty(gLAccountId)) return myRecepients;
            CardQuery cardQuery = new CardQuery(tenant);
            List<ShortPartnersDetails> connectedPartners = cardQuery.GetConnectedPartnerIdsByGLAccountId(gLAccountId, tenant);

            if (connectedPartners.Count() == 0) return myRecepients;
            else if (GetInactivePartnerCounts(connectedPartners) == 0) return myRecepients;
            else if (GetInactivePartnerCounts(connectedPartners) == connectedPartners.Count()) return null;
            else
            {
                myRecepients = RemoveInActiveCustomerRecepients(myRecepients, connectedPartners, tenant);
                if (string.IsNullOrEmpty(myRecepients.To))
                    return null;
            }

            return myRecepients;
        }

        private ValidateResult ValidateSelectedPartners(List<QueryFilterItem> reportFilterItems, int tenant, AdditionalValidate additionalValidate)
        {
            ReportSchedulerValidator reportSchedulerValidator = new ReportSchedulerValidator(reportFilterItems, tenant, additionalValidate);
            ValidateResult result = reportSchedulerValidator.validate();
            return result;
        }

        private ValidateResult ValidateGLAccountBalanceInLocalCurrency(List<QueryFilterItem> reportFilterItems, StiReport stiReport, int tenant)
        {
            ValidateResult result = new ValidateResult() { IsValid = true, ErrorMessage = ""};
            string balanceInLocalCurrency = GetFilterFieldValueByName(reportFilterItems, "BalanceInLocalCurrency");
            string balanceInLocalCurrencyOperator = GetFilterFieldOperatorByName(reportFilterItems, "BalanceInLocalCurrency");
            if (stiReport.BusinessObjectsStore.Where(x => x.Category == "LTRP").Any() && !string.IsNullOrWhiteSpace(balanceInLocalCurrency)
                && !string.IsNullOrWhiteSpace(balanceInLocalCurrencyOperator))
            {
                var stiBusinessObjectData = stiReport.BusinessObjectsStore.Where(x => x.Category == "LTRP").FirstOrDefault();
                var ledgerTransactionsDataProvider = stiBusinessObjectData != null ? (LedgerTransactionsDataProvider)stiBusinessObjectData.BusinessObjectValue : null;
                result.IsValid = CompareBalanceInLocalCurrencyWithLocalClosedBalance(Convert.ToDecimal(balanceInLocalCurrency), ledgerTransactionsDataProvider.LocalClosedBalance, balanceInLocalCurrencyOperator);
                if (!result.IsValid)
                    result.ErrorMessage = "The E-mail was not sent, the GLaccount local closed balance " + getOperatorName(balanceInLocalCurrencyOperator) + " the closed balance in local currency";
            }
            return result;
        }

        private bool CompareBalanceInLocalCurrencyWithLocalClosedBalance(decimal balanceInLocalCurrency, decimal localclosedBalance, string balanceInLocalCurrencyOperator)
        {
            bool isValid = false;
            switch (balanceInLocalCurrencyOperator) {
                case "Equals":
                    isValid = (localclosedBalance == balanceInLocalCurrency);
                    break;
                case "NotEqual":
                    isValid = (localclosedBalance != balanceInLocalCurrency);
                    break;
                case "LargerThan":
                    isValid = (localclosedBalance > balanceInLocalCurrency);
                    break;
                case "LessThan":
                    isValid = (localclosedBalance < balanceInLocalCurrency);
                    break;
                case "LessThanOrEqual":
                    isValid = (localclosedBalance <= balanceInLocalCurrency);
                    break;
                case "GreaterThanOrEqual":
                    isValid = (localclosedBalance >= balanceInLocalCurrency);
                    break;
            }
            return isValid;
        }

        private string getOperatorName(string operatorCode)
        {
            string operatorName = "";
            switch (operatorCode)
            {
                case "Equals":
                    operatorName = "is not equal";
                    break;
                case "NotEqual":
                    operatorName = "is equal";
                    break;
                case "LargerThan":
                    operatorName = "is not larger than";
                    break;
                case "LessThan":
                    operatorName = "is not less than";
                    break;
                case "LessThanOrEqual":
                    operatorName = "is not less than or equal";
                    break;
                case "GreaterThanOrEqual":
                    operatorName = "is not greater than or equal";
                    break;
            }
            return operatorName;
        }

        private string GetFilterFieldOperatorByName(List<QueryFilterItem> reportFilterItems, string fieldName)
        {
            string fieldOperator = null;
            reportFilterItems.ForEach(item => {
                if (item.FieldName == fieldName)
                {
                    if (item.FieldValue != null)
                        fieldOperator = item.Operator.ToString();
                }
            });
            return fieldOperator;
        }

        private ReportSchedulerRecepients RemoveInActiveCustomerRecepients(ReportSchedulerRecepients recepients, List<ShortPartnersDetails> connectedPartners, int tenant)
        {
            ReportSchedulerRecepients myRecepients = recepients;
            CardContactRepository repositry = new CardContactRepository(tenant);
            List<ActivatedEmail> toEmails = FillAllRecepients(myRecepients.To, true);
            List<ActivatedEmail> ccEmails = FillAllRecepients(myRecepients.Cc, true);
            List<ActivatedEmail> bccEmails = FillAllRecepients(myRecepients.Bcc, true);
            string inActiveRecipients = "";
            connectedPartners.ForEach(connectedPartner =>
            {
                if (connectedPartner.InActive)
                {
                    List<Contact> partnerContacts = repositry.GetContactsByCardId(connectedPartner.PartnerId).ToList();
                    partnerContacts.ForEach(partner =>
                    {
                        toEmails.ForEach(to =>
                        {
                            if (partner.Email == to.To)
                            {
                                to.IsActive = false;
                                inActiveRecipients += partner.Email + ',';
                            }
                        });
                        ccEmails.ForEach(cc =>
                        {
                            if (partner.Email == cc.To)
                            {
                                cc.IsActive = false;
                                inActiveRecipients += partner.Email + ',';
                            }
                        });
                        bccEmails.ForEach(bcc =>
                        {
                            if (partner.Email == bcc.To)
                            {
                                bcc.IsActive = false;
                                inActiveRecipients += partner.Email + ',';
                            }
                        });
                    });
                }
            });


            myRecepients.To = FillOnlyActiveRecepients(toEmails);
            myRecepients.Cc = FillOnlyActiveRecepients(ccEmails);
            myRecepients.Bcc = FillOnlyActiveRecepients(bccEmails);

            if (!string.IsNullOrEmpty(inActiveRecipients) && !string.IsNullOrEmpty(myRecepients.To))
            {
                inActiveRecipients = this.RemoveDuplicateEmails(inActiveRecipients);
                string warningMessage = "The E-mail was not sent to " + inActiveRecipients;
                warningMessage = ReformatWarningMessage(warningMessage);
                currentTask.LogWarning(warningMessage);
            }

            return myRecepients;
        }

        private int GetInactivePartnerCounts(List<ShortPartnersDetails> connectedPartners)
        {
            int inactivePartnerCounts = 0;
            connectedPartners.ForEach(partner =>
            {
                if (partner.InActive) inactivePartnerCounts += 1;
            });

            return inactivePartnerCounts;
        }
        private void SendHtmlDocument(SendHtmlDocumentArgs args)
        {
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
            SchedulerDetails schedulerDetails = GetSchedulerDetails(args.reportTask); 
            EmailDetails emailDetails = GetEmailDetailsByMessageTemplateId(new GetEmailDetailsByMessageTemplateIdArgs() { messageTemplateId = schedulerDetails.ReportDetails.MessageTemplateId, tenant = args.reportTask.Tenant, userId = args.reportTask.CreatedBy, stiReport = args.stiReport });
            string reportTableId = GetReportTableId(args.reportTask.Tenant);
            string subject = !string.IsNullOrEmpty(emailDetails.Subject) ? emailDetails.Subject : args.reportTask.Name;
            htmlEditorHelper.SendHtmlDocument(emailDetails.Body, null, null, args.reportTask.Tenant, args.recepients.To, subject, args.recepients.Cc, args.recepients.Bcc, args.reportTask.CreatedBy, args.reportTask.EntityId, reportTableId, args.documentId + ",", "", "", "");
            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;
        }
        public EmailDetails GetEmailDetailsByMessageTemplateId(GetEmailDetailsByMessageTemplateIdArgs args)
        {
            if (string.IsNullOrEmpty(args.messageTemplateId) || string.IsNullOrWhiteSpace(args.messageTemplateId))
            {
                return GetInstanceOfEmailDetails();
            }
            return GetEmailDetails(new GetEmailDetailsArgs() { messageTemplateId = args.messageTemplateId, tenant = args.tenant, userId = args.userId, stiReport = args.stiReport });
        }

        private EmailDetails GetEmailDetails(GetEmailDetailsArgs args)
        {
            ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(args.tenant);
            string documentId = reportsTemplatesVersionRepository.GetReportDocumentIdByReportTemplateId(args.messageTemplateId, args.tenant);
            ReportsTemplateQuery reportsTemplateQuery = new ReportsTemplateQuery(args.tenant);
            ReportsTemplatePM reportsTemplatePM = reportsTemplateQuery.GetSinglePM(args.messageTemplateId, args.tenant);
            EmailDetails emailDetails = GetInstanceOfEmailDetails();

            if (string.IsNullOrEmpty(documentId)) return emailDetails;
            string html = GetHtmlBody(args.tenant, documentId);

            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
            string subject = reportsTemplatePM != null ? reportsTemplatePM.Subject : null;
            string from = null;
            string replyTo = null;
            string cc = null;
            UTF8Encoding utf8Encoding = new UTF8Encoding();

            object dataProvider = GetDataProviderFromStiReport(args.stiReport);
            string dataProviderName = GetDataProviderNameFromStiReport(args.stiReport);
            html = htmlEditorHelper.ResolveDataProviderHtml(new DataProviderResolverArgs() { htmlValue = html, dataProvider = dataProvider, dataProviderName = dataProviderName });
            subject = htmlEditorHelper.ResolveDataProviderHtml(new DataProviderResolverArgs() { htmlValue = subject, dataProvider = dataProvider, dataProviderName = dataProviderName });
            string htmlstring = htmlEditorHelper.ResolveSystemDataHtml(html, args.userId, ref subject, ref from, ref replyTo, ref cc, args.tenant);     
            emailDetails.Body = utf8Encoding.GetBytes(htmlstring);
            emailDetails.Subject = subject;
            return emailDetails;
        }

        private object GetDataProviderFromStiReport(StiReport stiReport)
        {
            if (stiReport == null) return null;
            if (stiReport.BusinessObjectsStore == null) return null;
            if (stiReport.BusinessObjectsStore[0] == null) return null;
            return stiReport.BusinessObjectsStore[0].BusinessObjectValue;
        }

        private string GetDataProviderNameFromStiReport(StiReport stiReport)
        {
            if (stiReport == null) return null;
            if (stiReport.BusinessObjectsStore == null) return null;
            if (stiReport.BusinessObjectsStore[0] == null) return null;
            return stiReport.BusinessObjectsStore[0].Name;
        }

        private static string GetHtmlBody(int tenant, string documentId)
        {
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = documentId,
                FolderName = "reports",
                Extension = "html",
                Tenant = tenant,

            };

            var fileData = storageservice.Read(fileInfo);
            string html = string.Empty;
            if (fileData != null)
            {
                html = System.Text.Encoding.UTF8.GetString(fileData);
            }

            return html;
        }

        private static EmailDetails GetInstanceOfEmailDetails()
        {
            UTF8Encoding utf8Encoding = new UTF8Encoding();
            return new EmailDetails()
            {
                Body = utf8Encoding.GetBytes(""),
                Subject = null
            };
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
    public class ActivatedEmail
    {
        public string To;
        public bool IsActive;
    }

    public class ReportScedulerDocumentArgs
    {
        public string Name;
        public string Format;
        public int Tenant;
        public byte[] ByteData;
    }
    public class SendHtmlDocumentArgs
    {
        public string documentId;
        public ReportSchedulerRecepients recepients;
        public TasksSchedulerPM reportTask;
        public StiReport stiReport;
    }
    public class GetEmailDetailsByMessageTemplateIdArgs
    {
        public string messageTemplateId;
        public int tenant;
        public string userId;
        public StiReport stiReport;
    }
    public class GetEmailDetailsArgs
    {
        public string messageTemplateId;
        public int tenant;
        public string userId;
        public StiReport stiReport;
    }
    public class EmailDetails
    {
        public byte[] Body;
        public string Subject;
    }
}

