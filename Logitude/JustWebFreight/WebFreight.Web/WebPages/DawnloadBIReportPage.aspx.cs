using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebPages
{
    public partial class DawnloadBIReportPage : System.Web.UI.Page
    {
        private byte[] _DatainByte;
        private string fileName = string.Empty;
        private string queryName = string.Empty;
        private string fileType = string.Empty;
        private string token = string.Empty;
        private int? tenant = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            token = Request["tempId"] ?? "";
            fileName = Request["fileName"] ?? "";
            queryName = Request["qname"] ?? "";
            fileType = Request["fileType"] ?? "";
            SecurityDocumentResult securityDocumentResult = GetSecurityDocumentResult();
             
            if (securityDocumentResult.IsValid)
            {
                tenant = securityDocumentResult.Tenant;
                byte[] result = GetFileData();
                
                if (result != null)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    var browser = HttpContext.Current.Request.Browser;
                    string documentName = GetBIReportName() + "." + GetFileExtension();
                    string ShowType = "attachment";
                    HttpContext.Current.Response.ContentType = GetContentType();
                    if (browser != null && browser.Browser.Equals("ie", StringComparison.OrdinalIgnoreCase))
                    {
                        HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename*=UTF-8''" + HttpUtility.UrlPathEncode(documentName) + "\"");
                    }
                    else
                    {
                        HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename=\"" + HttpUtility.UrlPathEncode(documentName) + "\"");
                    }

                    HttpContext.Current.Response.BinaryWrite(result);

                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        HttpContext.Current.Response.Flush();
                        if (fileType == "Excel") HttpContext.Current.Response.Close();
                        HttpContext.Current.ApplicationInstance.CompleteRequest();

                    }
                }
            }
            else
            {
                 Response.Output.Write(!string.IsNullOrEmpty(securityDocumentResult.ExceptionResult) ? securityDocumentResult.ExceptionResult : "Sorry you’re not authenticated to view this excel.");
            }
        }

        private SecurityDocumentResult GetSecurityDocumentResult()
        {
            SecurityDocumentResult securityDocumentResult = SecurityDocumentHelper.ValidationDocumentToken(token);
            int tenant =(int) securityDocumentResult.Tenant;
            string email = securityDocumentResult.Email;
            if (securityDocumentResult.IsValid)
            {
                securityDocumentResult.IsValid = false;
                if (IsUser(securityDocumentResult.Email,tenant) && CheckAvailablityTenantsForEmail(email,tenant) || tenant == 0) securityDocumentResult.IsValid = true;
            }
            return securityDocumentResult;
        }

        private byte[] GetFileData()
        {
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = fileName,
                FolderName = "others",
                Extension = GetFileExtension(),
                Tenant = (int)tenant,

            };
         
            return storageservice.Read(fileInfo);

        }

        private string GetFileExtension()
        {
            string extension = fileType;
            if (fileType == "Excel") extension = "xlsx";
            return extension;
        }

        private string GetContentType()
        {
            string contentType = string.Empty;
            if (fileType == "Pdf") contentType = "application/" + "pdf";
            else if (fileType == "Excel") contentType = "application/" + "vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return contentType;
        }

        private  string GetBIReportName()
        {
            string biReportName = fileName.Split(new string[] { "!BIReportName=" }, StringSplitOptions.None)[1];
            return biReportName;
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
        public bool CheckAvailablityTenantsForEmail(string email, int tenant)
        {
            UserRepository userRep = new UserRepository(0);
            Simplog.Data.CommonDataModel.EntityPOCOs.User user = userRep.GetSingleUserByEmail(email, tenant, false);

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
    }
}