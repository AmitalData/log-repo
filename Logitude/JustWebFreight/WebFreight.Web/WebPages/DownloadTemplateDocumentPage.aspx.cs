using System;
using System.Linq;
using System.Transactions;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.WebServices;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using WebFreight.Web.WcfApi;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Xsl;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel;
using Stimulsoft.Report;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebPages
{
    public partial class DownloadTemplateDocumentPage : System.Web.UI.Page
    {
        public byte[] _DatainByte;
        int? tenant = null;

        private static bool IsUser(string email, int tenant)
        {
            bool isUser = false;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IGlobalContext globalContext = GlobalContext.GetContext();
                isUser = globalContext.GlobalContacts.Where(c => c.Email == email && (c.GlobalTenantId == tenant || c.GlobalTenantId == 0) && c.IsUser == true).Any();
            }
            return isUser;
        }

        public bool CheckAvailablityTenantsForEmail(string email, int tenant)
        {
            UserRepository userRep = new UserRepository(0);
            Simplog.Data.CommonDataModel.EntityPOCOs.User user = userRep.GetSingleUserByEmail(email, 0, false);

            bool available = true;
            if (user != null)
            {
                TenantManagementRepository tenantManagementRep = new TenantManagementRepository();
                bool isDistributorToCurrentTenant = tenantManagementRep.CheckDistributor(user.DistributorCode, tenant);
                if (user.IsDistributor)
                {
                    if (isDistributorToCurrentTenant)
                    {
                        available = true;
                    }
                    else
                    {
                        available = false;
                    }
                }
                else
                {
                    available = true;
                }
            }
            else
            {
                ContactRepository contactRep = new ContactRepository(tenant);
                available = contactRep.CheckEmailAvailabilityForTenant(email, tenant);
            }
            return available;
        }

        protected void Page_Load(object sender, EventArgs e)
        {




            string id = Request["id"] ?? "";
            string fileName = Request["fileName"] ?? "";
            string type = Request["type"] ?? "";
            string token = Request["tempId"] ?? "";
            string xamlDocumentId = Request["XamlDocumentId"] ?? "";

            SecurityDocumentResult securityDocumentResult = SecurityDocumentHelper.ValidationDocumentToken(token);
            bool isValid = securityDocumentResult.IsValid;
            string email = securityDocumentResult.Email;
            string exceptionMessage = securityDocumentResult.ExceptionResult;
            tenant = securityDocumentResult.Tenant;


            if (isValid)
            {
                isValid = false;
                if (IsUser(email, (int)tenant) && CheckAvailablityTenantsForEmail(email, (int)tenant) || tenant == 0) isValid = true;
            }

            if (isValid)
            {
                if (type == "ReportTemplate")
                {
                    ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository((int)tenant);
                    string documentId = reportsTemplatesVersionRepository.GetReportDocumentIdByReportTemplateId(id, (int)tenant);
                    if (!string.IsNullOrEmpty(documentId))
                    {

                        byte[] fileData = null;
                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                        BlobFileInfo fileInfo = new BlobFileInfo()
                        {
                            FileName = documentId,
                            FolderName = "reports",
                            Extension = "html",
                            Tenant = (int)tenant,

                        };
                        fileData = storageservice.Read(fileInfo);
                        HtmlTemplateClass htmlTemplateClass = new HtmlTemplateClass();

                        if (fileData != null)
                        {
                            htmlTemplateClass.BodyHtml = System.Text.Encoding.UTF8.GetString(fileData);
                        }

                        _DatainByte = LogitudeXmlSerializer.SerializeObject(htmlTemplateClass);

                    }
                }
                else
                {
                    List<object> htmlResult = null;
                    DocumentTypeTemplate documentTypeTemplate = null;
                    if (!string.IsNullOrEmpty(xamlDocumentId))
                    {
                        ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();
                        htmlResult = exportDocumentHelper.GetDownloadFileFromServer(xamlDocumentId, (int)tenant);
                    }
                    else
                    {
                        DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository((int)tenant);
                        documentTypeTemplate = documentTypeTemplateRepository.GetSingleDocumentTypeTemplateWithOutInClude(id, (int)tenant);
                    }

                    if (documentTypeTemplate != null || htmlResult != null)
                    {
                        string headerHtml = string.Empty;
                        string footerHtml = string.Empty;
                        string bodyHtml = string.Empty;
                        HtmlTemplateClass htmlTemplateClass = new HtmlTemplateClass();

                        if (htmlResult != null)
                        {
                            htmlTemplateClass.HeaderHtml = htmlResult[0] != null ? htmlResult[0].ToString() : "";
                            htmlTemplateClass.BodyHtml = htmlResult[1] != null ? htmlResult[1].ToString() : "";
                            htmlTemplateClass.FooterHtml = htmlResult[2] != null ? htmlResult[2].ToString() : "";
                            htmlTemplateClass.HeaderHeight = htmlResult[3] != null ? Int32.Parse(htmlResult[3].ToString()) : 0;
                            htmlTemplateClass.FooterHeight = htmlResult[4] != null ? Int32.Parse(htmlResult[4].ToString()) : 0;

                        }
                        else
                        {
                            if (documentTypeTemplate.TemplateHeaderHtml != null)
                            {
                                htmlTemplateClass.HeaderHtml = System.Text.Encoding.UTF8.GetString(documentTypeTemplate.TemplateHeaderHtml);
                                htmlTemplateClass.HeaderHeight = documentTypeTemplate.TemplateHeaderHeight;
                            }

                            if (documentTypeTemplate.TemplateFooterHtml != null)
                            {
                                htmlTemplateClass.FooterHtml = System.Text.Encoding.UTF8.GetString(documentTypeTemplate.TemplateFooterHtml);
                                htmlTemplateClass.FooterHeight = documentTypeTemplate.TemplateFooterHeight;
                            }
                            if (documentTypeTemplate.TemplateBodyHtml != null)
                            {
                                htmlTemplateClass.BodyHtml = System.Text.Encoding.UTF8.GetString(documentTypeTemplate.TemplateBodyHtml);
                            }
                        }

                        _DatainByte = LogitudeXmlSerializer.SerializeObject(htmlTemplateClass);
                    }
                }

                string documentName = fileName + ".xml";
                string ShowType = "attachment";
                if (_DatainByte != null)
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.AddHeader("Content-Length", _DatainByte.Length.ToString());
                    var browser = HttpContext.Current.Request.Browser;
                    HttpContext.Current.Response.ContentType = "application/" + "xml";

                    if (browser != null && browser.Browser.Equals("ie", StringComparison.OrdinalIgnoreCase))
                    {

                        HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename*=UTF-8''" + HttpUtility.UrlPathEncode(documentName) + "\"");
                    }
                    else
                    {
                        HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename=\"" + HttpUtility.UrlPathEncode(documentName) + "\"");
                    }

                    HttpContext.Current.Response.BinaryWrite(_DatainByte);

                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.Close();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();

                    }

                }

            }
            else
            {
                var message = exceptionMessage;
                if (string.IsNullOrEmpty(exceptionMessage)) message = "Sorry you’re not authenticated to view this report.";
                Response.Output.Write(message);
             //   throw new ApplicationException(message);

            }
        }
    }
}
