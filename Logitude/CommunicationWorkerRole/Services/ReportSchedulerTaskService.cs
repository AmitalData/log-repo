using CommunicationWorkerRole.Tasks;
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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
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

        public void SendPdfReportToReceipent(TasksSchedulerPM reportTask)
        {
            try
            {
                SchedulerDetails schedulerDetails = GetSchedulerDetails(reportTask);
                reportTask.CreatedBy = schedulerDetails.ReportDetails.CreatedByUserId;
                ReportFliter reportFilter = GetReportFilters(reportTask, schedulerDetails);
                StiReport stiReport = GetStimulReportByReportFilter(reportFilter);
                string documentId = GetDocumentIdAfterExport(stiReport, reportTask.Name, reportTask.Tenant);
                SendHtmlDocument(documentId, schedulerDetails.ReportDetails.Recepients, reportTask);
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
}

