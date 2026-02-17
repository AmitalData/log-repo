using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WebPages
{
    public partial class SharedDownloadPage : System.Web.UI.Page
    {


        public byte[] _DatainByte;

        public bool CheckAvailablityTenantsForEmail(string email, int tenant)
        {
            ContactRepository contactRep = new ContactRepository(tenant);
            return contactRep.CheckEmailAvailabilityForTenant(email, tenant);
        }

        public bool CheckSharedContactAuthenticationForInvoice(string partnerId, int tenant)
        {
            if (tenant != 0)
            {

                bool exists = false;
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {//using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    //{
                    //}
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                    string email = HttpContext.Current.User.Identity.Name;

                    ContactRepository contactrep = new ContactRepository(commonDataContext);
                    Contact contact = contactrep.GetSingleContactByEmail(email, tenant);

                    if (contact != null)
                    {
                        CardContact cardContact = commonDataContext.CardContacts.Where(d => d.ContactId == contact.Id && d.CardId == partnerId).FirstOrDefault();
                        if (cardContact != null)
                        {
                            exists = true;

                        }
                    }


                }
                if (!exists)
                {
                    throw new AutenticationException("Sorry! you are not authorized to read data!");
                }
                return exists;
            }
            return true;


        }
        public bool CheckSharedContactAuthenticationForShipment(string agentId, string customerId, int tenant)
        {
            if (tenant != 0)
            {

                bool exists = false;
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {//using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    //{
                    //}
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                    string email = HttpContext.Current.User.Identity.Name;

                    ContactRepository contactrep = new ContactRepository(commonDataContext);
                    Contact contact = contactrep.GetSingleContactByEmail(email, tenant);

                    if (contact != null)
                    {
                        CardContact cardContact = commonDataContext.CardContacts.Where(d => d.ContactId == contact.Id && (d.CardId == customerId || d.CardId == agentId)).FirstOrDefault();
                        if (cardContact != null)
                        {
                            exists = true;

                        }
                    }


                }
                if (!exists)
                {
                    throw new AutenticationException("Sorry! you are not authorized to read data!");
                }
                return exists;
            }
            return true;


        }

        public void DownloadAll(string entityId, int tenant, string partnerType,string token=null)
        {
            try
            {


                string email = this.Context.User.Identity.Name;

                ShipmentRepository rep = new ShipmentRepository(tenant);
                Shipment shipment = rep.GetSingleShipment(entityId, tenant);
                bool isAuothenticatedRequest=true;
                bool CheckForTenantAvailability = true;

                if (!string.IsNullOrEmpty(token))
                {
                    SecurityDocumentResult securityDocumentResult = SecurityDocumentHelper.ValidationDocumentToken(token);
                    if (securityDocumentResult.IsValid)
                    {
                        email = securityDocumentResult.Email;
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(email), new string[0]);
                        SecurityUtility.AuthenticationOnTenant((int)tenant);
                        SecurityUtility.CheckContactFeature("Shipment", "READ", (int)tenant);
                    }

                }
                else
                {

                    isAuothenticatedRequest = CheckSharedContactAuthenticationForShipment(shipment.AgentId, shipment.CustomerId, tenant);
                    CheckForTenantAvailability = CheckAvailablityTenantsForEmail(email, tenant);
                }
                if (CheckForTenantAvailability && isAuothenticatedRequest)
                {
                    Uploader up = new Uploader();
                    List<DocumentsFilingPM> documents = up.GetDocumentByEntityAndTenant(entityId, tenant);

                    if (string.IsNullOrEmpty(token))
                    {
                        if (partnerType == "AG")
                        {
                            documents = documents.Where(d => d.IsAgentView).ToList();
                        }

                        else if (partnerType == "CS")
                        {
                            documents = documents.Where(d => d.IsCustomerView == true).ToList();
                        }
                    }

                    Dictionary<string, byte[]> CompressedArray = new Dictionary<string, byte[]>();
                    bool DocumentsExistance = false;
                    var ItemNum = 0;

                    foreach (DocumentsFilingPM document in documents)
                    {
                        if (document.DirectionCode == "O" && document.DoucmentTypeTemplateFormatCode == "M")
                        {
                            continue;
                        }

                        if (!string.IsNullOrEmpty(token))
                        {
                            if (partnerType == "O")
                            {
                                if (document.DirectionCode == "I")
                                    continue;
                            }
                            else
                            {
                                if (document.DirectionCode == "O")
                                    continue;
                            }
                        }


                         if (!string.IsNullOrEmpty(document.FileExtension))
                        {
                            DocumentsExistance = true;
                            string fileName = !string.IsNullOrEmpty(document.CalculatedFileName) ? document.CalculatedFileName : document.FileName;
                            fileName= fileName.Replace('/', ' ');
                            fileName += ("." + document.FileExtension);

                            while (CompressedArray.ContainsKey(document.FileExtension + "@" + fileName))
                            {
                                ItemNum += 1;
                                fileName = !string.IsNullOrEmpty(document.CalculatedFileName) ? document.CalculatedFileName + " (" + ItemNum + ")" : document.FileName + " (" + ItemNum + ")";
                                fileName = fileName.Replace('/', ' ');
                                fileName += ("." + document.FileExtension);
                            }
                            ItemNum = 0;
                            CompressedArray.Add(document.FileExtension + "@" + fileName, up.DownloadFile(document.DocumentId, document.FileExtension, "", tenant));
                        }

                    }
                    if (DocumentsExistance)
                    {
                        string name = "Documents";
                        if(!string.IsNullOrEmpty(token))
                        {
                            name = shipment.ShipmentNumber;
                        }
                        byte[] CompressedData = CompressionData(name, CompressedArray, false);
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.AddHeader("Content-Length", CompressedData.Length.ToString());
                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename="+ name+".zip");
                        HttpContext.Current.Response.ContentType = "application/zip";
                        HttpContext.Current.Response.BinaryWrite(CompressedData);

                        if (HttpContext.Current.Response.IsClientConnected)
                        {
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.Close();
                            HttpContext.Current.ApplicationInstance.CompleteRequest();

                        }
                    }
                }
                

            }
            catch(Exception e)
            {

            }
        }



        public static byte[] CompressionData(string listKey, Dictionary<string, byte[]> dataBackList, bool saveetodisk = false)
        {
            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(3);
            byte[] bytes = null;
            foreach (string key in dataBackList.Keys)
            {

                string fileName = key.Split('@')[1];
                var newEntry = new ZipEntry(fileName);
                newEntry.DateTime = DateTime.Now;

                zipStream.PutNextEntry(newEntry);

                bytes = dataBackList[key];

                MemoryStream inStream = new MemoryStream(bytes);
                long inStreamLength = inStream.Length;
                if (inStreamLength < 200)
                {
                    inStreamLength = 200;
                }

                StreamUtils.Copy(inStream, zipStream, new byte[inStreamLength]);
                inStream.Close();
                zipStream.CloseEntry();

            }

            zipStream.IsStreamOwner = false;
            zipStream.Close();
            outputMemStream.Position = 0;
            if (saveetodisk)
            {

                string appPath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\ZipFiles\"; // <---
                if (Directory.Exists(appPath) == false)                                              // <---
                {                                                                                    // <---
                    Directory.CreateDirectory(appPath);                                              // <---
                }                                                                                    // <---

                appPath += listKey + ".zip";

                System.IO.File.WriteAllBytes(appPath, outputMemStream.ToArray());
            }
            return outputMemStream.ToArray();

        }




        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string email = this.Context.User.Identity.Name;
                string documentExtension = "";
                string filename = "";
                string entityType = "";
                string entityId = "";
                string[] filestrings = null;
                int tenant = 0;
                //if (Request["id"] != null)
                //{
                string headerRequest = Request["id"];
                filestrings = headerRequest.Split(':');

                //string documentName = 
                //string url = "../WebPages/SharedDownloadPage.aspx?id=" + tenant + ":" + item.DocumentId +documenttype+ entityId;




                tenant = Convert.ToInt32(filestrings[0]);
                if (filestrings[1] == "null")
                {
                    if (filestrings.Length==6)
                    {
                        DownloadAll(filestrings[3], int.Parse(filestrings[0]), filestrings[4], filestrings[5]);

                    }
                    else
                    {
                        DownloadAll(filestrings[3], int.Parse(filestrings[0]), filestrings[4]);
                    }
                }
                else
                {
                    filename = filestrings[1].ToString();
                    string documentId = filename.Split('.')[0].ToString();

                    Uploader up = new Uploader();

                    //if (filestrings.Count() > 2)
                    //{
                    //    documentExtension = "pdf";
                    //    filename += ".pdf";
                    //    string containername = filestrings[2].ToString();
                    //    _DatainByte = up.DownloadStaticFile(filename, containername);

                    //}
                    //else
                    //{
                    Document document = up.GetDocumentById(documentId, tenant);
                    if (document != null)
                    {
                        documentExtension = document.Extension;
                        filename = document.CalculatedFileName;
                    }

                    if (!string.IsNullOrEmpty(documentExtension))
                    {
                        _DatainByte = up.DownloadFile(documentId, documentExtension, "", tenant);
                    }


                    entityType = filestrings[2];
                    entityId = filestrings[3];

                    //invc//ship
                    bool isAuothenticatedRequest = false;

                    switch (entityType)
                    {
                        case "ship":
                            ShipmentRepository rep = new ShipmentRepository(tenant);
                            Shipment shipment = rep.GetSingleShipment(entityId, tenant);
                            isAuothenticatedRequest = CheckSharedContactAuthenticationForShipment(shipment.AgentId, shipment.CustomerId, tenant);

                            break;
                        case "invc":
                            ARInvoiceQuery entityQuery = new ARInvoiceQuery(tenant);
                            ARInvoicePM entityPM = entityQuery.GetSinglePM(entityId, tenant);
                            isAuothenticatedRequest = CheckSharedContactAuthenticationForInvoice(entityPM.BillToId, tenant);
                            break;
                    }


                    if (CheckAvailablityTenantsForEmail(email, tenant) && isAuothenticatedRequest)
                    {

                        if (_DatainByte != null)
                        {
                            string documentName = filename + "." + documentExtension;
                            // _DatainByte = sender as byte[];
                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.AddHeader("Content-Length", _DatainByte.Length.ToString());
                            //HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + DocumentName);

                            // Get content type
                            // FileExtension = filename.Split('.')[1];
                            switch (documentExtension)
                            {
                                case "pdf":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "pdf";
                                    break;

                                case "doc":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "msword";
                                    break;

                                case "docx":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.wordprocessingml.document";
                                    break;

                                case "xls":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.ms-excel";
                                    break;

                                case "xlsx":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    break;

                                case "ppt":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.ms-powerpoint";
                                    break;

                                case "pptx":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/" + "vnd.openxmlformats-officedocument.presentationml.presentation";
                                    break;

                                case "jpg":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "image/jpeg";
                                    break;

                                case "zip":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/zip";
                                    break;

                                case "xml":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/xml";
                                    break;

                                case "html":
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/html";
                                    break;
                                default:
                                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + documentName);
                                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                                    break;

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

                        }

                    }
                    else
                    {
                        Response.Output.Write("Sorry you’re not authenticated to view this document.");
                        throw new ApplicationException("Sorry you’re not authenticated to view this document.");
                    }
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
    }
}
