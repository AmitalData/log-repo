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
using Microsoft.Practices.Unity;
using Logitude.Server.Tools;
using Stimulsoft.Report.Export;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebPages
{
    public partial class DawnLoadExcelPage : System.Web.UI.Page
    {

        public byte[] _DatainByte;

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

        int? tenant = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            string token = Request["tempId"] ?? "";
            string FileName = Request["fileName"] ?? "";
            string QName = Request["qname"] ?? "";
            string Type = Request["Type"] ?? "";
            string requestArea = Request["requestArea"] ?? "";



            
            SecurityDocumentResult securityDocumentResult = SecurityDocumentHelper.ValidationDocumentToken(token);
            bool isValid = securityDocumentResult.IsValid;
            string email = securityDocumentResult.Email;
            string exceptionMessage = securityDocumentResult.ExceptionResult;
            tenant = securityDocumentResult.Tenant;

            if (isValid)
            {
                isValid = false;
                if (IsUser(email, (int)tenant)  || CheckAvailablityTenantsForEmail(email, (int)tenant) || tenant == 0) isValid = true;
            }

            if (isValid)
            {
                ICommonDataContext context = CommonDataContext.GetContext(0);


                //StiReport stiReport = new StiReport();
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = FileName,
                    FolderName = "others",
                    Extension = "xls",
                    Tenant = (int)tenant,

                };
                if (!string.IsNullOrEmpty(Type) && Type == "SaveToMicrosoftExcel2007")
                {
                    fileInfo.Extension = "xlsx";
                }

                byte[] result = storageservice.Read(fileInfo);


                //string[] Names = FileName.Split('@');
                //if (Names.Count() > 1)
                //{

                //    FileName = Names[1];
                //}



                if (result != null)
                {
                    //stiReport.LoadDocumentFromString(System.Text.Encoding.Default.GetString(result));
                    MemoryStream memoryStream = new MemoryStream();

                    var browser = HttpContext.Current.Request.Browser;
                    string documentName = "";
                    string ShowType = "attachment";
                    //switch (Type)
                    //{
                    //    case "SaveToMicrosoftExcel2007":
                    //        {
                    //            stiReport.ExportDocument(StiExportFormat.Excel2007, memoryStream);
                    //            HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //            documentName = FileName + ".xlsx";
                    //            break;
                    //        }

                    //    case "SaveToMicrosoftExcel":
                    //        {
                    //stiReport.ExportDocument(StiExportFormat.Excel, memoryStream);
                    if (Type == "SaveToMicrosoftExcel2007")
                    {
                        HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        var fileName = FileName;
                        if (fileName.Contains("!BIReportName="))
                        {
                            fileName = fileName.Split(new string[] { "!BIReportName=" }, StringSplitOptions.None)[1];
                        }
                        documentName = fileName + ".xlsx";
                    }
                    else
                    {
                        HttpContext.Current.Response.ContentType = "application/" + "vnd.ms-excel";
                        documentName = QName + ".xls";
                    }

                    //break;
                    //        }
                    //    case "PrintToPDF":
                    //        {
                    //            stiReport.ExportDocument(StiExportFormat.Pdf, memoryStream);
                    //            HttpContext.Current.Response.ContentType = "application/" + "pdf";
                    //            documentName = FileName + ".pdf";
                    //            ShowType = "inline";
                    //            break;
                    //        }
                    //}

                    if (browser != null && browser.Browser.Equals("ie", StringComparison.OrdinalIgnoreCase))
                    {
                        HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename*=UTF-8''" + HttpUtility.UrlPathEncode(documentName) + "\"");
                    }
                    else
                    {
                        HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename=\"" + HttpUtility.UrlPathEncode(documentName) + "\"");
                    }

                    //if (memoryStream != null)
                    //{
                    HttpContext.Current.Response.BinaryWrite(result);

                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        HttpContext.Current.Response.Flush();
                        if (Type == "SaveToMicrosoftExcel2007")
                        {
                            HttpContext.Current.Response.Close();
                        }

                        HttpContext.Current.ApplicationInstance.CompleteRequest();

                    }
                    // }


                }
            }
            else
            {
                var message = exceptionMessage;
                if (string.IsNullOrEmpty(exceptionMessage)) message = "Sorry you’re not authenticated to view this excel.";
                Response.Output.Write(message);
                //  throw new ApplicationException(message);

            }
        }


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

    }
}