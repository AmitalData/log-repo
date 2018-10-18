using Logitude.Customs.BL.StimulReport.Mapping;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.Helpers;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.WebPages
{
    public partial class ReportPdfDownLoad : System.Web.UI.Page
    {

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
            Simplog.Data.CommonDataModel.EntityPOCOs.User user = userRep.GetSingleUserByCodeOrEmail(null, email, 0, false);

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


        int? tenant =null;
        protected void Page_Load(object sender, EventArgs e)
        {
         
            try
            {
                bool  IsValid = true;
                string templateId = Request["documentTypeTemplateId"] ?? "";
                string entityId = Request["entityId"] ?? "";
                string entityObjectTableId = Request["entityObjectTableId"] ?? "";
                string childEntityId = Request["childEntityId"] ?? "";
                string childObjectTableId = Request["childObjectTableId"] ?? "";
                string documentTypeTemplateName = Request["documentTypeTemplateName"] ?? "";


                string token = Request["tempId"] ?? "";
                SecurityDocumentResult securityDocumentResult = SecurityDocumentHelper.ValidationDocumentToken(token);
                bool isValid = securityDocumentResult.IsValid;
                string email = securityDocumentResult.Email;
                string exceptionMessage = securityDocumentResult.ExceptionResult;
                tenant = securityDocumentResult.Tenant;

                if (IsValid)
                {
                    IsValid = false;
                    if (IsUser(email, (int)tenant) && CheckAvailablityTenantsForEmail(email, (int)tenant) || tenant == 0) IsValid = true;
                }

                if (IsValid)
                {
                    if (templateId.Equals("PrintTzrufa", StringComparison.OrdinalIgnoreCase))
                    {

                        PrintTzrufa((int)tenant, entityId);
                        return;
                    }


                    byte[] _DatainByte = null;
                    ExportDocument exportDocumentservice = new ExportDocument();
                    if (!String.IsNullOrWhiteSpace(templateId) && !String.IsNullOrEmpty(templateId))

                    {
                        _DatainByte = exportDocumentservice.GetDocumentTypebyte(templateId, entityId, entityObjectTableId, childEntityId, childObjectTableId, (int)tenant);
                    }

                    bool isUser = false;
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalContext = GlobalContext.GetContext();
                        isUser = globalContext.GlobalContacts.Where(c => c.Email == email && (c.GlobalTenantId == tenant || c.GlobalTenantId == 0) && c.IsUser == true).Any();
                    }
                    string Name = documentTypeTemplateName + "." + "pdf";
                    if (isUser)
                    {
                        if (_DatainByte != null)
                        {
                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.AddHeader("Content-Length", _DatainByte.Length.ToString());

                            HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + Name);
                            HttpContext.Current.Response.ContentType = "application/" + "pdf";


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
                        Response.Output.Write("Sorry you’re not authenticated to view this document.");
                     //   throw new ApplicationException("Sorry you’re not authenticated to view this document.");
                    }

                }
                else
                {
                    var message = exceptionMessage;
                    if (string.IsNullOrEmpty(exceptionMessage)) message = "Sorry you’re not authenticated to view this document.";
                    Response.Output.Write(message);
                  //  throw new ApplicationException(message);
                }

            }
            catch (ExceptionInErrorLog ExceptionInErrorLog)
            {
                Response.Clear();
                Response.Output.Write(
//                    String.Format(
//@"An unhandled exception has been caught (our ref :{0})  
//{1}",ExceptionInErrorLog.ErrorlogId, ExceptionInErrorLog.Message)
ExceptionInErrorLog.ToString()
    );
            }
            catch (Exception errorInfo)
            {
                string ErrorMessage = errorInfo.Message;
                if (!ErrorMessage.Contains("Sorry you’re not authenticated to view this document") || !ErrorMessage.Contains("Sorry, your download link has expired."))
                {
                    if (errorInfo.InnerException != null)
                    {
                        ErrorMessage += Environment.NewLine + errorInfo.InnerException.Message;
                    }
                    ErrorMessage += Environment.NewLine + errorInfo.ToString();
                    if (!string.IsNullOrEmpty(errorInfo.StackTrace))
                    {
                        ErrorMessage += Environment.NewLine + errorInfo.StackTrace;
                    }
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, (int)tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "ReportPdfDownLoad : PageLoad Method", null);
                }
                //AzureLog.SaveLogsInStorage(ErrorMessage, "E", 0, User.Identity.Name, User.Identity.Name);
            }

        }

        private void PrintTzrufa(int tenant, string declarationId)
        {
            byte[] streamPdf = null;
            byte[] bytsTemplate = null;
            DeclarationSRMapping declarationSRMapping = new DeclarationSRMapping();
            var pdf = new WebFreight.Web.Helpers.PdfUtil();
            //var obj = declarationSRMapping.GetDefault();
            var exportDocument = new WebFreight.Web.Helpers.PdfUtil();
            bytsTemplate = //System.Text.Encoding.UTF8.GetBytes
                (WebFreight.Web.Properties.Resources.A252_2015_020151011133801779020300040816);
            exportDocument.template = bytsTemplate;
            DeclarationSRMapping declarationSRMapping1 = new DeclarationSRMapping();
            var xml = declarationSRMapping1.GetXml(tenant, declarationId);
            var dataStream = new MemoryStream(Encoding.UTF8.GetBytes(xml ?? ""));

            exportDocument.dataFile = dataStream;
            streamPdf = exportDocument.ExportPdfFromXml();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Length", streamPdf.Length.ToString());

            HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + "PrintTzrufa_" + declarationId + ".pdf");

            HttpContext.Current.Response.ContentType = "application/" + "pdf";


            HttpContext.Current.Response.BinaryWrite(streamPdf);

            if (HttpContext.Current.Response.IsClientConnected)
            {
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
                HttpContext.Current.ApplicationInstance.CompleteRequest();

            }

        }

    }
}