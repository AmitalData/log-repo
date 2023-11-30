using Logitude.BL.DataContracts;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebFreight.Web.Helpers;

namespace ReportAndDocumentWRTesterTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CacheManager.CacheWrapper = new NoCache4uWrapper();
        }

        private void RunReportsButton_Click(object sender, EventArgs e)
        {
            string reportCode = "SHID";
            int tenant = Int32.Parse(TenantTextBox.Text);
            string email = EmailTextBox.Text;
            Report report = GetReportByCode(tenant, reportCode);
            string contactId = GetContactId(tenant, email);
            if (string.IsNullOrEmpty(contactId)) return;

            ReportFliter reportFliter = GetReportFilter(report, contactId, tenant);

            int numberOfRecords = Int32.Parse(NumberOfRecordsText.Text);
            for (int i = 0; i < numberOfRecords; i++)
            {
                RunReports(reportFliter);
            }
            MessageBox.Show("Finished!!");
        }

        private static ReportFliter GetReportFilter(Report report, string contactId, int tenant)
        {
            ReportFliter reportFliter = new ReportFliter
            {
                ReportName = report.Name,
                ReportCode = report.Code,
                ReportDocumentId = null,
                ReportsRunUsingWR = true,
                ProcessType = "GenerateReport",
                NumberOfPage = 1,
                FilterControlName = report.FilterControlName,
                DisablePreview = false,
                ReportId = report.Id,
                DefaultTemplateId = report.DefaultTemplateId,
                //DefaultTemplateVsersion = 7,////
                tenant = tenant,
                UserId = contactId,
            };
            reportFliter.QueryFilterItemLists = new List<QueryFilterItem>();
            reportFliter.QueryFilterItemLists.Add(new QueryFilterItem()
            {
                FieldName = "FromDate",
                FieldValue = "BLY",
                FieldDataType = "Date",
                DisplayInList = false,
            });
            reportFliter.QueryFilterItemLists.Add(new QueryFilterItem()
            {
                FieldName = "ToDate",
                FieldValue = "TOD",
                FieldDataType = "Date",
                DisplayInList = false,
            });
            return reportFliter;
        }

        private static Report GetReportByCode(int tenant, string reportCode)
        {
            ReportRepository reportRepository = new ReportRepository(tenant);
            Report report = reportRepository.GetSingleReportByCode(reportCode, tenant);
            return report;
        }

        private static string GetContactId(int tenant, string email)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            string contactId = contactRepository.GetConactIdByemail(email, tenant);
            if (string.IsNullOrEmpty(contactId))
            {
                contactId = contactRepository.GetConactIdByemail(email, 0);
            }
            if (string.IsNullOrEmpty(contactId))
            {
                MessageBox.Show("User Not Exist!");
            }

            return contactId;
        }

        private void RunReports(ReportFliter reportFliter)
        {
            ReportHelper reportHelper = new ReportHelper();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = reportHelper.GetReportDateTimeFormat(reportFliter);
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm";
            reportHelper.BuildReportDataViewWorkerRole(reportFliter);
            Thread.Sleep(new TimeSpan(0, 0, Int32.Parse(DelayBTWText.Text)));
        }

        private void RunDocumentsButton_Click(object sender, EventArgs e)
        {
            ExportDocumentArgs exportDocumentArgs = GetExportDocumentArgs();
            int numberOfRecords = Int32.Parse(NumberOfRecordsText.Text);
            for (int i = 0; i < numberOfRecords; i++)
            {
                RunDocuments(exportDocumentArgs);
            }
            MessageBox.Show("Finished!!");
        }

        private void RunDocuments(ExportDocumentArgs exportDocumentArgs)
        {
            ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
            DocumentsExecutionLog documentsExecutionLog = exportDocumentHelper.GetNewInStanceFromDocumentsExecutionLog(exportDocumentArgs);
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("DocumentsExecutionQueue", documentsExecutionLog.Tenant);
            queueservice.Send(new Dictionary<string, string>() { { "DocumentsExecutionLogId", documentsExecutionLog.Id }, { "Tenant", documentsExecutionLog.Tenant.ToString() } }, documentsExecutionLog.Tenant, null, null, null, null);
            Thread.Sleep(new TimeSpan(0, 0, Int32.Parse(DelayBTWText.Text)));
        }

        private ExportDocumentArgs GetExportDocumentArgs()
        {
            //HardCoded//
            return new ExportDocumentArgs
            {
                ChildEntityId = "",
                ChildObjectTableId = "",
                CurrentDocumentOutId = "1-85584",
                CurrentDocumentTypeCode = "COO",
                DocumentTemplateEditorTool = "S",
                DocumentTypeCopyIdsList = new List<string>
                {
                    "1-1136"
                },
                DocumentTypeId = "1-892",
                DocumentTypeName = "Certificate of Origin",
                DocumentTypeTemplateId = "1-47",
                EntityId = "1-79937",
                LoggedContactId = "1-1",
                ObjectTableId = "1-4",
                ObjectTableName = "Shipment",
                Tenant = Int32.Parse(TenantTextBox.Text),
            };
        }
    }
}
