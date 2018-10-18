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

namespace WebFreight.Web.WebPages
{
    public partial class DownloadingPage : System.Web.UI.Page
    {
        public byte[] _DatainByte;
        int? tenant = null;

        protected void Page_Load(object sender, EventArgs e)
        {
         
            try
            {
                string securityKey = Request["securityId"] ?? null;
                string token = Request["tempId"] ?? "";
                string securityId = null;
                string documentOutCopyId = null;
                string userId = null;
                Document document = null;

                string documentExtension = null;
                string CustomName = null;
                string email = null;
                string fileName = null;
                bool IsValid = true;

                ICommonDataContext context = CommonDataContext.GetContext(0);
                if (!string.IsNullOrEmpty(securityKey))
                {
                    var securityArray = securityKey.Split('~');
                    if (securityArray.Length >0)
                    {
                        securityId = securityArray[0];
                        if (!string.IsNullOrEmpty(securityId))
                        {
                            securityId = System.Net.WebUtility.UrlEncode(securityId);

                            if (securityArray.Length >1) documentOutCopyId = securityArray[1];
                            if (securityArray.Length > 2) userId = securityArray[2];


                            DocumentsFilingRepository documentRepository = new DocumentsFilingRepository(0);
                            DocumentsFiling documentFiling = documentRepository.GetSingleDocumentFilingBySecurityId(securityId);
                            if (documentFiling != null)
                            {
                             

                                if (!string.IsNullOrEmpty(documentOutCopyId))
                                {
                                    var docoutcopy = context.DocumentOutCopies.Where(d => d.DocumentOutId == documentFiling.Id && d.Id == documentOutCopyId && d.Tenant == tenant).FirstOrDefault();
                                    if (docoutcopy != null)
                                    {
                                        string documentId = !string.IsNullOrEmpty(docoutcopy.DocumentId) ? docoutcopy.DocumentId : docoutcopy.Id;
                                        document = context.Documents.Where(doc => doc.Id == documentId && doc.Tenant == tenant).FirstOrDefault();
                                    }
                                    else IsValid = false;
                                }
                                else
                                {
                                    document = context.Documents.Where(doc => doc.Id == documentFiling.DocumentId && doc.Tenant == tenant).FirstOrDefault();
                                }

                                if (document != null)
                                {
                                    if (string.IsNullOrEmpty(document.CalculatedFileName) || documentFiling.DirectionCode == "I")
                                    {
                                        document.CalculatedFileName = document.FileName;
                                    }
                                    documentExtension = document.Extension;
                                    CustomName = document.CalculatedFileName;
                                    fileName = document.Id;

                                }
                                else IsValid = false;


                            }
                            else IsValid = false;
                        }
                        else IsValid = false;
                        
                    }
                    else IsValid = false;


                 }

                else if (!string.IsNullOrEmpty(token))
                {
                    AuthenticationTokenRepository tokenRep = new AuthenticationTokenRepository(context);
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    if (authToken != null)
                    {
                        email = authToken.Email;
                        tenant = authToken.Tenant;

                        bool overrideSecDueIsConnectedToUniFreight = false;
                        if (LogitudeSettings.IsCostomsDeploy)
                        {
                            var setting = CustomsSettingQueryService.GetSettingByTenant((int)tenant) ?? new CustomsSettingPM();
                            overrideSecDueIsConnectedToUniFreight = setting.IsConnectedToUniFreight;
                            if (!overrideSecDueIsConnectedToUniFreight)//semi a like Connected  == not cloud !!
                            {
                                if (!String.IsNullOrWhiteSpace(setting.OnPremiseFillingService)) overrideSecDueIsConnectedToUniFreight = true;
                            }
                        }
                        
                        if (overrideSecDueIsConnectedToUniFreight || CheckAvailablityTenantsForEmail(email, (int)tenant) || tenant == 0)
                        {
                            if (overrideSecDueIsConnectedToUniFreight || IsUser(email, (int)tenant)){

                                fileName = Request["id"] ?? "";
                                document = context.Documents.Where(doc => doc.Id == fileName && doc.Tenant == tenant).FirstOrDefault();

                                if (document != null)
                                {
                                    documentExtension = document.Extension;
                                    fileName = document.Id;

                                } else IsValid = false;
                            } else IsValid = false;

                        } else IsValid = false;
                    } else IsValid = false;

                } else IsValid = false;

                if (IsValid)
                {
                    Uploader up = new Uploader();
                    if (!string.IsNullOrEmpty(documentExtension) && !string.IsNullOrEmpty(fileName))
                    {
                        _DatainByte = up.DownloadFile(fileName, documentExtension, "", (int)tenant);
                    }
                    
                     if (_DatainByte != null)
                        {
                            string documentName = (!string.IsNullOrEmpty(CustomName) ? CustomName : fileName) + "." + documentExtension;
                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.AddHeader("Content-Length", _DatainByte.Length.ToString());
                            var browser = HttpContext.Current.Request.Browser;
                            string ShowType = "attachment";
                            switch (documentExtension)
                            {
                                case "pdf":
                                    HttpContext.Current.Response.ContentType = "application/" + "pdf";
                                    ShowType = "inline";
                                    break;

                                case "doc":
                                    HttpContext.Current.Response.ContentType = "application/" + "msword";
                                    break;

                                case "docx":
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.wordprocessingml.document";
                                    break;

                                case "xls":
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.ms-excel";
                                    break;

                                case "xlsx":
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    break;

                                case "ppt":
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.ms-powerpoint";
                                    break;

                                case "pptx":
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.presentationml.presentation";
                                    break;

                                case "jpg":
                                    HttpContext.Current.Response.ContentType = "image/jpeg";
                                    break;

                                case "zip":
                                    HttpContext.Current.Response.ContentType = "application/zip";
                                    break;

                                case "xml":
                                    HttpContext.Current.Response.ContentType = "application/xml";
                                    ShowType = "inline";
                                    break;

                                case "html":
                                    HttpContext.Current.Response.ContentType = "application/html";
                                    ShowType = "inline";
                                    break;
                                default:
                                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                                    break;

                            }

                            documentName = documentName.Replace(" ", "");
                            if (browser != null && browser.Browser.Equals("ie", StringComparison.OrdinalIgnoreCase))
                            {
                                HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename*=UTF-8''" + HttpUtility.UrlPathEncode(documentName) + "\"");
                            }
                            else
                            {
                                HttpContext.Current.Response.AppendHeader("Content-Disposition", ShowType + "; filename=\"" + HttpUtility.UrlPathEncode(documentName) + "\"");
                            }

                            if (!string.IsNullOrEmpty(userId))
                            {
                                UserRepository userRep = new UserRepository((int)tenant);
                                User printedBy = null;
                                if (!string.IsNullOrEmpty(userId)) printedBy = userRep.GetSingleUser(userId, (int)tenant);
                                else printedBy = userRep.GetSingleUserByEmail(email, (int)tenant, false);

                                DocumentOutCopyRepository myRep = new DocumentOutCopyRepository((int)tenant);
                                DocumentOutCopy documentoutCopy = myRep.GetSingleDocumentOutCopyByTenant(documentOutCopyId, (int)tenant);
                                documentoutCopy.LastPrintDate = TenantServerConfigration.GetCurrentDateTime((int)tenant);
                                documentoutCopy.LastPrintedByUserId = printedBy != null ? printedBy.Id : "";
                                myRep.Update(documentoutCopy);
                                myRep.SubmitChanges();
                            }

                            HttpContext.Current.Response.BinaryWrite(_DatainByte);
                            if (HttpContext.Current.Response.IsClientConnected)
                            {
                                HttpContext.Current.Response.Flush();
                                HttpContext.Current.Response.Close();
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }

                        }
                    else
                    {
                        Response.Output.Write("document is missing.");
                        throw new ApplicationException("document is missing.");
                    }
                }
                else
                {
                    Response.Output.Write("Sorry you’re not authenticated to view this document.");
                    throw new ApplicationException("Sorry you’re not authenticated to view this document.");

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
                if (!ErrorMessage.Contains("Sorry you’re not authenticated to view this document") && !ErrorMessage.Contains("Sorry, your download link has expired.") && !ErrorMessage.Contains("Document file is empty."))
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
                    ExceptionHandler.HandleException(errorInfo, DateTime.Now, (int)tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "DownloadingPage : PageLoad Method", null);
                }
                //AzureLog.SaveLogsInStorage(ErrorMessage, "E", 0, User.Identity.Name, User.Identity.Name);
            }

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
