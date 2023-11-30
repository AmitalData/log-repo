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
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.BIReport;

namespace CommunicationWorkerRole.Services
{
    public class BIReportSchedulerTaskService
    {
        
        int trackerCounter = 0;
        string[,] trackerLogs = new string[,] //tracker(Step, DateTime)
        {
            {"Prepare bi report scheduler details", null},
            {"Get bi report data", null},
            {"Send email to reciepents", null},
        };

        TaskManagerBase currentTask;
        ReportSchedulerTaskService reportSchedulerTaskService;
        public BIReportSchedulerTaskService(TaskManagerBase task)
        {
            this.currentTask = task;
            reportSchedulerTaskService = new ReportSchedulerTaskService(task);
        }
   
        public void RunTask(TasksSchedulerPM reportTask)
        {
            try
            {
                SchedulerDetails schedulerDetails = GetSchedulerDetails(reportTask);
                reportTask.CreatedBy = schedulerDetails.ReportDetails.CreatedByUserId;
                byte[] biReportData = GetBIReportData(schedulerDetails, reportTask);

                if(biReportData == null)
                {
                    this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("BI Report Is Empty"));
                }
                else
                {
                    SendBIReport(reportTask, schedulerDetails, biReportData);
                }
            }
            catch (Exception ex)
            {
                HandleTaskFailure(ex);
                return;
            }
        }

        private void SendBIReport(TasksSchedulerPM reportTask, SchedulerDetails schedulerDetails, byte[] biReportData)
        {
            if (reportTask.ResultType == null || reportTask.ResultType == "Email")
            {
                SendBIReportToReceipent(reportTask, schedulerDetails, biReportData);
            }
            else if (reportTask.ResultType == "FTP")
            {
                SendBIReportToFTP(reportTask, schedulerDetails, biReportData);
            }
        }

        private void HandleTaskFailure(Exception ex)
        {
            string logsMessage = reportSchedulerTaskService.GetAllTaskLogs();
            string errorMessage = new StringBuilder().Append(logsMessage).AppendLine().ToString();
            errorMessage += new StringBuilder().Append("Exception Message: ").AppendLine().Append(ex.Message).AppendLine().ToString();
            errorMessage += new StringBuilder().Append("Stack Trace:").AppendLine().Append(ex.StackTrace).AppendLine().ToString();

            throw new Exception(errorMessage);
        }

        private byte[] GetBIReportData(SchedulerDetails schedulerDetails, TasksSchedulerPM reportTask)
        {
            this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Preparing bi report data"));
            ExportBIReportService exportBIReportService = new ExportBIReportService();
            BIReportXMLData bIReportXMLData = GetBIReportXMLData(schedulerDetails, reportTask);
            bIReportXMLData.ExportDataType = reportTask.Format == "PDF" || string.IsNullOrEmpty(reportTask.Format) ? "Pdf" : "xlsx";
            const string includeTotalFormatCode = "IT";
            bIReportXMLData.IncludeTotals = reportTask.AdvancedFormat == includeTotalFormatCode;
            return exportBIReportService.Run(bIReportXMLData, reportTask.Tenant, schedulerDetails.SendIfEmpty);
        }

        private BIReportXMLData GetBIReportXMLData(SchedulerDetails schedulerDetails, TasksSchedulerPM reportTask)
        {
            BIReportXMLDataService bIReportXMLDataService = new BIReportXMLDataService();
            RemoveNullFilterItemsValues(schedulerDetails);
            return bIReportXMLDataService.GetByBIReportId(new BIReportXMLDataServiceArgs { BIReportId = schedulerDetails.ReportDetails.BIReportEntityId, DWQueryId = schedulerDetails.ReportDetails.DWQueryId, Tenant = reportTask.Tenant, FiltersData = schedulerDetails.ReportDetails.DWQueryFilterData });
        }

        private void RemoveNullFilterItemsValues(SchedulerDetails schedulerDetails)
        {
            const string xmlNodeString = "System.Xml.XmlNode[]";
            schedulerDetails.ReportDetails.DWQueryFilterData.FilterItems.ForEach(filter => {
                filter.TextValue = filter.TextValue?.ToString() == xmlNodeString ? null : filter.TextValue;
            });
            if(schedulerDetails.ReportDetails.DWQueryFilterData.FilterItems != null && schedulerDetails.ReportDetails.DWQueryFilterData.FilterItems.Count == 0)
            {
                schedulerDetails.ReportDetails.DWQueryFilterData = null;
            }
        }

        private void SendBIReportToReceipent(TasksSchedulerPM reportTask, SchedulerDetails schedulerDetails, byte[] biReportData)
        {
            schedulerDetails.ReportDetails.Recepients = GetBIReportPermittedContacts(reportTask, schedulerDetails);
            if (schedulerDetails.ReportDetails.Recepients != null)
            {
                TryToSendBIReport(reportTask, schedulerDetails, biReportData);
            }
        }

        private ReportSchedulerRecepients GetBIReportPermittedContacts(TasksSchedulerPM reportTask, SchedulerDetails schedulerDetails)
        {
            List<ContactList> allPermittedContacts = reportSchedulerTaskService.GetAllPermittedContacts(reportTask.Tenant, null);
            ReportSchedulerRecepients recepients = reportSchedulerTaskService.RemoveNonPermittedContacts(schedulerDetails.ReportDetails.Recepients, allPermittedContacts);
            return recepients;
        }

        private void TryToSendBIReport(TasksSchedulerPM reportTask, SchedulerDetails schedulerDetails, byte[] biReportData)
        {
            string format = reportTask.Format == "PDF" ? "pdf" : "xlsx";
            this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Exporting bi report to " + format + " file"));
            string documentId = reportSchedulerTaskService.CreateDocument(new ReportScedulerDocumentArgs { Name = reportTask.Name, Format = format, Tenant = reportTask.Tenant, ByteData = biReportData });//GetDocumentId

            this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Sending report to reciepents"));
            ReportSchedulerRecepients reportRecepients = schedulerDetails.ReportDetails.Recepients;
            SendHtmlDocument(documentId, schedulerDetails, reportTask);
            this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("Sending report to reciepents finished successfully"));
        }

        private SchedulerDetails GetSchedulerDetails(TasksSchedulerPM reportTask)
        {
            SchedulerDetails schedulerDetails = LogitudeXmlSerializer.DeserializeObject<SchedulerDetails>(reportTask.SchedulerDetailsXML);
            schedulerDetails.Tenant = reportTask.Tenant;

            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;
            return schedulerDetails;
        }

        private void SendHtmlDocument(string documentId, SchedulerDetails schedulerDetails, TasksSchedulerPM reportTask)
        {
            ReportSchedulerRecepients recepients = schedulerDetails.ReportDetails.Recepients;
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
            string reportTableId = GetBIReportTableId(reportTask.Tenant);
            byte[] emailBody = GetEmailBody(schedulerDetails.ReportDetails.DocumentTypeTemplateId, reportTask.Tenant);
            htmlEditorHelper.SendHtmlDocument(emailBody, null, null, reportTask.Tenant, recepients.To, reportTask.Name, recepients.Cc, recepients.Bcc, reportTask.CreatedBy, reportTask.EntityId, reportTableId, documentId + ",", "", "", "");
            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;
        }

        public  byte[] GetEmailBody(string documentTypeTemplateId,  int tenant)
        {
            UTF8Encoding utf8Encoding = new UTF8Encoding();
            if (string.IsNullOrEmpty(documentTypeTemplateId) || string.IsNullOrWhiteSpace(documentTypeTemplateId))
            {
                return utf8Encoding.GetBytes("");
            }
            var messageArgs = HtmlEditorHelper.GetHtmlFromTemplate(documentTypeTemplateId, null , tenant);
            return utf8Encoding.GetBytes(messageArgs.HtmlTemplate);  
        }

        private string GetBIReportTableId(int tenant)
        {
            ObjectTableQuery objectTableQuery = new ObjectTableQuery(tenant);
            string reportId = objectTableQuery.GetObjectTableIdByName("BIReport");
            return reportId;
        }
        
        private void SendBIReportToFTP(TasksSchedulerPM reportTask, SchedulerDetails schedulerDetails, byte[] biReportData)
        {
            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;
            if (schedulerDetails.FTPDetails == null)
            {
                this.currentTask.LogInfo(FTPLogBuilder.BuildLogLine("FTPDetails is missing"));
                return;
            }

            this.trackerLogs[trackerCounter, 0] = "Uploading bi report to ftp";
            this.trackerLogs[trackerCounter, 1] = DateTime.Now.ToString();
            this.trackerCounter += 1;

            string p_message = "";
            string p_status = "";
            string schedulerFormatExtension = reportTask.Format == "PDF" ? "pdf" : "xlsx";
            string fileName = reportTask.Name + "." + schedulerFormatExtension;
            FTPServiceMod ftpService = new FTPServiceMod(schedulerDetails.FTPDetails.Host, schedulerDetails.FTPDetails.UserName, schedulerDetails.FTPDetails.Password);
            ftpService.Upload(fileName, schedulerDetails.FTPDetails.Folder, biReportData, out p_message, out p_status, true, true);

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
}
