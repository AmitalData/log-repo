using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using Atp.Pdf;

//using Microsoft.WindowsAzure.Storage;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;

using WebFreight.Web.Azure;
using WebFreight.Web.CommonDataModel;
using WebFreight.Web.Testing;
using WebFreight.Web.WebServices;
using Microsoft.WindowsAzure.Storage.Blob;
using Logitude.SystemLogs;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using System.Transactions;
using Simplog.Global.Data.GlobalModel;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebPages
{
    public partial class MergeAllPage : System.Web.UI.Page
    {
        public Stream _Stream;
        int? tenant = null;
        byte[] datainByte;

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

        private string GetFileLocation(string theFilelocation)
        {
            string filelocation = "UserUploads";

            switch (theFilelocation)
            {
                case "UserUploads":
                    filelocation = "UserUploads";
                    break;

                case "logos":
                    filelocation = "logos";
                    break;
            }

            return filelocation;
        }

        public Stream DownloadFile(string documentId, string documentExtension, string fileLocation, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

            Document document = (from doc in commonContext.Documents
                                 where doc.Id == documentId
                                 select doc).FirstOrDefault();
            if (document != null)
            {
                //string filelocation = GetFileLocation(fileLocation);
                try
                {
                   
                    //else // In Azure
                    //{

                        //string filename = document.Id + "." + document.Extension;

                        //blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);

                        BlobFileInfo fileInfo = new BlobFileInfo()
                        {
                            FileName = document.Id,
                            FolderName = document.Folder,
                            Extension = document.Extension,
                            Tenant = document.Tenant,
                            FileSize = document.FileSize,
                        };
                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                        datainByte = storageservice.Read(fileInfo);


                        if (datainByte != null)
                        {
                            
                            MemoryStream stream = new MemoryStream(datainByte);
                            return stream;
                        }
                        else
                            return null;



                        //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));

                        //if (blobfile.Exists())
                        //{
                        //    MemoryStream stream = new MemoryStream();   
                        //    blobfile.DownloadToStream(stream);
                        //       // DatainByte = memstream.ToArray();
                        //    return stream;
                           
                        //}

                        //else
                        //    return null;
                   // }
                }
                catch (Exception e)
                {
                    string ip = "";
                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        ip = currentIP;
                    }
                    ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "MergeAllPage : DownloadFile Method ",ip);
                    return null;
                }
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                string token = Request["tempId"] ?? "";
                string securityId = "";
                string securityKey = "";
                string userId = "";
                string documentOutId = null;
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
                     securityKey = Request["securityId"] ?? "";

                    if (!string.IsNullOrEmpty(securityKey))
                    {
                        email = "";
                        var securityArray = securityKey.Split('~');
                        if (securityArray.Length > 0)
                        {
                            securityId = securityArray[0];
                            if (securityArray.Length > 1) userId = securityArray[1];

                            if (!string.IsNullOrEmpty(securityId))
                            {
                                DocumentOut doucmentOut = null;
                                ICommonDataContext commonContext = CommonDataContext.GetContext((tenant != null ? (int)tenant : 0));


                                doucmentOut = (from a in commonContext.DocumentOuts.Include("DocumentsFiling").Include("DocumentsFiling.DocumentType")
                                               where a.DocumentsFiling.SecurityId == securityId
                                               select a).FirstOrDefault();

                                if (doucmentOut != null)
                                {
                                    documentOutId = doucmentOut.Id;
                                    tenant = doucmentOut.Tenant;

                                    List<DocumentOutCopy> copies = (from a in commonContext.DocumentOutCopies
                                                                    where a.DocumentOutId == documentOutId && tenant == (int)tenant
                                                                    select a).OrderBy(d => d.DocumentTypeCopy.IndexOrder).ToList();

                                    Uploader up = new Uploader();
                                    PdfDocument pdfDoc = new PdfDocument();

                                    foreach (DocumentOutCopy copy in copies)
                                    {
                                        bool includeInPrint = true;
                                        if (doucmentOut.DocumentsFiling.DocumentType.IsDocumentOneTimePrintLimited && doucmentOut.DocumentsFiling.DocumentType.LimitedPrintCopyId == copy.DocumentTypeCopyId && !string.IsNullOrEmpty(copy.LastPrintedByUserId))
                                        {
                                            includeInPrint = false;
                                        }
                                        else if (doucmentOut.DocumentsFiling.DocumentType.IsDocumentOneTimePrintLimited && doucmentOut.DocumentsFiling.DocumentType.LimitedPrintCopyId == copy.DocumentTypeCopyId)
                                        {
                                            UserRepository userRep = new UserRepository((int)tenant);

                                            User printedBy = null;
                                            if (!string.IsNullOrEmpty(userId)) printedBy = userRep.GetSingleUser(userId, (int)tenant);
                                            else printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, email, (int)tenant, false);


                                            DocumentOutCopyRepository myRep = new DocumentOutCopyRepository((int)tenant);
                                            DocumentOutCopy documentoutCopy = myRep.GetSingleDocumentOutCopyByTenant(copy.Id, (int)tenant);
                                            documentoutCopy.LastPrintDate = TenantServerConfigration.GetCurrentDateTime((int)tenant);
                                            documentoutCopy.LastPrintedByUserId = printedBy.Id;
                                            myRep.Update(documentoutCopy);
                                            myRep.SubmitChanges();
                                        }

                                        if (includeInPrint)
                                        {

                                            string documentExtension = up.GetFileExtension(copy.DocumentId, (int)tenant);
                                            string documentId = copy.DocumentId;
                                            if (!string.IsNullOrEmpty(documentExtension))
                                            {
                                                _Stream = DownloadFile(documentId, documentExtension, "", (int)tenant);
                                                if (_Stream != null)
                                                {
                                                    PdfDocumentBase.Merge(pdfDoc, _Stream);
                                                    if ((pdfDoc.Pages.Count % 2 == 1) && doucmentOut.DocumentsFiling.DocumentType.Code == "740")
                                                    {
                                                        pdfDoc.Pages.Add();
                                                    }
                                                }
                                            }
                                        }

                                    }

                                    if (pdfDoc != null)
                                    {

                                        HttpContext.Current.Response.Clear();
                                        HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + documentOutId + ".pdf");
                                        HttpContext.Current.Response.ContentType = "application/" + "pdf";

                                        MemoryStream memoryStream = new MemoryStream();
                                        pdfDoc.Save(memoryStream);
                                        HttpContext.Current.Response.BinaryWrite(memoryStream.ToArray());

                                        if (HttpContext.Current.Response.IsClientConnected)
                                        {
                                            HttpContext.Current.Response.Flush();
                                            //HttpContext.Current.Response.Close();
                                            HttpContext.Current.ApplicationInstance.CompleteRequest();

                                        }

                                    }
                                }
                            }
                        }

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
            catch (Exception errorInfo)
            {
                 string errorMessage = errorInfo.Message;

                if (errorInfo.InnerException != null)
                {
                    errorMessage += Environment.NewLine + errorInfo.InnerException.Message;
                }
                errorMessage += Environment.NewLine + errorInfo.ToString();
                if (!string.IsNullOrEmpty(errorInfo.StackTrace))
                {
                    errorMessage += Environment.NewLine + errorInfo.StackTrace;
                }
                string ip="";
                if(HttpContext.Current!=null && HttpContext.Current.Request!=null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                AzureLog.SaveLogsInStorage(errorMessage, "E", DateTime.Now, errorInfo.Message, errorInfo.StackTrace, 0, User.Identity.Name, User.Identity.Name,ip);
            }
        
        }
    }
}
