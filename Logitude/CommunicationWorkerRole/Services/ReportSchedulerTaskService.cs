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
        public ReportSchedulerTaskService()
        {
        }

        public void SendPdfReportToReceipent(TasksSchedulerPM reportTask)
        {
            SchedulerDetails schedulerDetails = GetSchedulerDetails(reportTask);
            reportTask.CreatedBy = schedulerDetails.ReportDetails.CreatedByUserId;
            ReportFliter reportFilter = GetReportFilters(reportTask, schedulerDetails);
            StiReport stiReport = GetStimulReportByReportFilter(reportFilter);
            string documentId = GetDocumentIdAfterExport(stiReport, reportTask.Name, reportTask.Tenant);
            string toEmails = GetRecepientsEmails(schedulerDetails.ReportDetails.Recepients, reportFilter.tenant);
            SendHtmlDocument(documentId, toEmails, reportTask);
        }

        private SchedulerDetails GetSchedulerDetails(TasksSchedulerPM reportTask)
        {
            SchedulerDetails schedulerDetails = LogitudeXmlSerializer.DeserializeObject<SchedulerDetails>(reportTask.SchedulerDetailsXML);
            schedulerDetails.Tenant = reportTask.Tenant;
            schedulerDetails = ModifyNullFilters(schedulerDetails);
            return schedulerDetails;
        }

        private SchedulerDetails ModifyNullFilters(SchedulerDetails schedulerDetails)
        {
            schedulerDetails.ReportDetails.ReportFilterItems.ForEach(filterItem=> {
                if(filterItem.FieldValue.GetType().Name == "XmlNode[]")
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
            return stiReport;
        }

        private string GetDocumentIdAfterExport(StiReport stiReport, string reportName, int tenant)
        {
            string documentId = String.Empty;
            MemoryStream memoryStream = new MemoryStream();
            stiReport.ExportDocument(StiExportFormat.Pdf, memoryStream);
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

        private string GetRecepientsEmails(string Recepients, int tenant)
        {
            string toEmails = string.Empty;
            ContactQuery contactQuery = new ContactQuery(tenant);
            string[] allRecepients = Recepients.Split(';');
            foreach (string recep in allRecepients)
            {
                toEmails += contactQuery.GetContactEmailById(recep, tenant);
                toEmails += ';';
            }

            return toEmails;
        }

        private void SendHtmlDocument(string documentId, string toEmails, TasksSchedulerPM reportTask)
        {
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            Byte[] htmlData = enc.GetBytes("");
            string reportTableId = GetReportTableId(reportTask.Tenant);
            htmlEditorHelper.SendHtmlDocument(htmlData, null, null, reportTask.Tenant, toEmails, reportTask.Name, "", "", reportTask.CreatedBy, reportTask.EntityId, reportTableId, documentId + ",", "", "", "");
        }

        private string GetReportTableId(int tenant)
        {
            ObjectTableQuery objectTableQuery = new ObjectTableQuery(tenant);
            string reportId = objectTableQuery.GetObjectTableIdByName("Report");
            return reportId;
        }
    }
}

