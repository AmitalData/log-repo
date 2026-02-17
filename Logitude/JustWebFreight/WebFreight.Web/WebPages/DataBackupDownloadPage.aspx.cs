using System;
using System.IO;
using System.Web;

using Microsoft.WindowsAzure.Storage;

using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Azure;

using WebFreight.Web.Azure;
using WebFreight.Web.WebServices;
using Microsoft.WindowsAzure.Storage.Blob;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using System.Linq;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebPages
{
    public partial class DataBackupDownloadPage : System.Web.UI.Page
    {

        public bool CheckAvailablityTenantsForEmail(string email, int tenant)
        {
            ContactRepository contactRep = new ContactRepository(tenant);
            bool available= contactRep.CheckEmailAvailabilityForTenant(email, tenant);
            if (!available)
            {
                available = contactRep.CheckEmailAvailabilityForTenant(email, 0);
            }
            return available;
        }


        int? tenant = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                byte[] datainByte = null;
                string token = Request["tempId"] ?? "";
                string filename = "datapackup.zip";

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
                    Uploader up = new Uploader();

                    BlobFileInfo fileInfo = new BlobFileInfo()
                        {
                            FileName = "datapackup",
                            Extension = "zip",
                            Tenant = (int)tenant,
                            HasExternalContainer = true,
                            ExternalContainerName = "tenant" + tenant
                        };
                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                        datainByte = storageservice.Read(fileInfo);


                    if (datainByte != null)
                    {


                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.AddHeader("Content-Length", datainByte.Length.ToString());
                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                        HttpContext.Current.Response.ContentType = "application/zip";


                        HttpContext.Current.Response.BinaryWrite(datainByte);

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
                    if (string.IsNullOrEmpty(exceptionMessage)) message = "Sorry you’re not authenticated to view this backup.";
                    Response.Output.Write(message);
               

                }

            }
            catch (Exception errorInfo)
            {
                // string ErrorMessage = errorInfo.Message;

                //if (errorInfo.InnerException != null)
                //{
                //    ErrorMessage += Environment.NewLine + errorInfo.InnerException.Message;
                //}
                //ErrorMessage += Environment.NewLine + errorInfo.ToString();
                //if (!string.IsNullOrEmpty(errorInfo.StackTrace))
                //{
                //    ErrorMessage += Environment.NewLine + errorInfo.StackTrace;
                //}
                //AzureLog.SaveLogsInStorage(ErrorMessage, "E",0,User.Identity.Name,User.Identity.Name);
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