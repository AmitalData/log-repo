using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.Services;
using System.Xml.Serialization;
using Microsoft.WindowsAzure.Storage;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using WebFreight.Web.Azure;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Helpers;
using WebFreight.Web.ReportsWebServices;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.Testing;
using Microsoft.WindowsAzure.Storage.Blob;
using Logitude.SystemLogs;
using Logitude.Server.Tools.Counters;
using System.Threading;
using System.Web;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using WebFreight.Web.CustomWebServices;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for ExportDocument
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ExportDocument : System.Web.Services.WebService
    {
        public void JustTestIt()
        {
            StiReport report = new StiReport();
            report.ReportName = @".mrt";
            
        }

        [WebMethod]
        public string ExportDocument2Pdf(string documentTypeId, string entityId, string entityObjectTableId, string childEntityId, string childObjectTableId, string documentOutId, int tenant, ref string errorMessage, string documentTypeCopyId)
        {
                ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                string result = exportDocumentHelper.ExportDocument2Pdf(documentTypeId, entityId, entityObjectTableId, childEntityId, childObjectTableId, documentOutId, tenant, documentTypeCopyId);
                return result;
      
        }

        [WebMethod]
        public byte[] GetDocumentTypebyte(string documentTypeTemplateId, string entityId, string entityObjectTableId, string childEntityId, string childObjectTableId, int tenant)
        {
            ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
            return exportDocumentHelper.GetDocumentTypebyte(documentTypeTemplateId, entityId, entityObjectTableId, childEntityId, childObjectTableId, tenant);
        }

        [WebMethod]
        public string SaveEditedReportToServer(string documentOutId, byte[] pdfDataFile, byte[] xamlDataFile, int tenant, string documentTypeCopyId)
        {
            ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
            return exportDocumentHelper.SaveEditedReportToServer(documentOutId, pdfDataFile, xamlDataFile, tenant, documentTypeCopyId);
        }

        [WebMethod]
        public byte[] DownloadFileFromServer(string documentId,int tenant)
        {
            ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
            return exportDocumentHelper.DownloadFileFromServer(documentId, tenant);
        }

        [WebMethod]
        public string BuildInvoiceDocument(string documentTypeId, string currentEntityId, string entityObjectTableId, string invoiceId, string documentOutId, int tenant, ref string errorMessage)
        {
            ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
            return exportDocumentHelper.BuildInvoiceDocument(invoiceId, documentOutId, tenant);
        }

    }
}
