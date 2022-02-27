using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.Helpers;
using System.Data.Entity;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;

namespace WebFreight.Web.App_Code
{
    public class DocumentsDataController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public List<SharedLogisticDocumentPM> GetEntityDocuments_Old(string oldParam1, string oldParam2, string entityId, string partnerType, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            ShipmentRepository rep = new ShipmentRepository(tenant);
            Shipment shipment = rep.GetSingleShipment(entityId, tenant);

            CheckSharedContactAuthenticationForShipment(shipment.AgentId, shipment.CustomerId, tenant);


            List<SharedLogisticDocumentPM> list = new List<SharedLogisticDocumentPM>();

            DocumentsFilingQuery documentInQuery = new DocumentsFilingQuery(tenant);
            DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);

            List<DocumentsFilingPM> docsInsData = documentInQuery.GetDocumentsFilingPMsByEntityId(entityId,"I", tenant);
            List<DocumentOutPM> docsOutsData = documentOutQuery.GetDocumentOutPMsByEntityId(entityId, tenant);

            List<DocumentsFilingPM> docsIns = new List<DocumentsFilingPM>();
            List<DocumentOutPM> docsOuts = new List<DocumentOutPM>();

            if (partnerType == "AG")
            {
                docsIns = docsInsData.Where(d => d.IsAgentView).ToList();
                docsOuts = docsOutsData.Where(d => d.IsAgentView).ToList();
            }

            else if (partnerType == "CS")
            {
                docsIns = docsInsData.Where(d => d.IsCustomerView).ToList();
                docsOuts = docsOutsData.Where(d => d.IsCustomerView).ToList();
            }

            foreach (DocumentsFilingPM item in docsIns)
            {
                //string documentName = tenant + "_" + item.DocumentId;
                //string url = "../WebPages/Downloadpage.aspx?id=" + documentName;
                string url = "../WebPages/SharedDownloadPage.aspx?id=" + tenant + ":" + item.DocumentId + ":ship:" + shipment.Id;//"../WebPages/SharedDownloadPage.aspx?id=" + documentName;
                 
                list.Add(new SharedLogisticDocumentPM()
                {
                    Id = "DocIn:" + item.Id,
                    DocumentId = item.DocumentId,
                    Name = item.DocumentTypeName,
                    Url = url,
                    FileExtension = item.FileExtension,
                    FileName = item.FileName,
                    IsDigitallySigned = item.IsDigitallySigned,
                    Reference = item.ChildEntityReference,
                });
            }

            foreach (DocumentOutPM item in docsOuts)
            {
                if (item.TemplateType == "P")
                {
                    string docId = item.Id;
                    if (item.DocumentOutCopies.Count() > 0)
                    {
                        docId = item.DocumentOutCopies.FirstOrDefault().DocumentId;
                    }

                    //string documentName = tenant + "_" + docId;
                    string url = "../WebPages/SharedDownloadPage.aspx?id=" + tenant + ":" + docId + ":ship:" + shipment.Id;//"../WebPages/SharedDownloadPage.aspx?id=" + documentName;
                    //string url = "../WebPages/SharedDownloadPage.aspx?id=" + tenant + ":" + item.DocumentId+":" +documenttype+":"+ entityId;

                    list.Add(new SharedLogisticDocumentPM()
                    {
                        Id = "DocOut:" + item.Id,
                        //DocumentId = item.DocumentId,
                        Name = item.DocumentTypeName,
                        Url = url,
                        FileExtension = "",
                        FileName = item.FileName,
                        IsDigitallySigned = false,
                        Reference = item.ChildEntityReference,
                    });
                }
            }

            return list.OrderBy(o => o.Name).ToList();
        }

        public List<SharedLogisticDocumentPM> GetEntityDocuments(string entityId, string partnerType, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<SharedLogisticDocumentPM> output = new List<SharedLogisticDocumentPM>();

            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            Shipment shipment = shipmentRepository.GetSingleShipment(entityId, tenant);

            if (shipment != null)
            {
                CheckSharedContactAuthenticationForShipment(shipment.AgentId, shipment.CustomerId, tenant);

                output = this.GetShipmentSharedDocuments(shipment, partnerType, tenant, false);
            }

            return output.OrderBy(o => o.Name).ToList();
        }

        public List<SharedLogisticDocumentPM> GetShipmentDocuments(string securitykey, string entityId, string partnerType, int tenant)
        {
            //string token = HttpContext.Current.Request.Headers["Token"];
            //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            //SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            //SecurityUtility.AuthenticationOnTenant(tenant);

            List<SharedLogisticDocumentPM> output = new List<SharedLogisticDocumentPM>();

            ShipmentRepository rep = new ShipmentRepository(tenant);
            Shipment shipment = rep.GetSingleShipment(entityId, tenant);

            if (shipment != null)
            {
                if (shipment.SecurityKey != null && securitykey != null)
                {
                    if (shipment.SecurityKey.ToLower() == securitykey.ToLower())
                    {
                        if(partnerType !="AG") partnerType = "CS"; //To avoid change PartnerType if it's agent

                        output = this.GetShipmentSharedDocuments(shipment, partnerType, tenant, true);
                    }
                }
            }

            return output.OrderBy(o => o.Name).ToList();
        }

        public bool GetIsDocumentsApprovalRequried(string entityId, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            ShipmentAdditionalCloudDataRepository rep = new ShipmentAdditionalCloudDataRepository(tenant);
            ShipmentAdditionalCloudData shipment = rep.GetSingleShipmentAdditionalCloudData(entityId, tenant);

            if (shipment != null /*&& shipment.IsDocumentsApprovalRequried*/ && string.IsNullOrEmpty(shipment.DocumentsApprovedByUserName))
            {
                    return true;
            }

            return false;
        }

        public bool GetPutDocumentsApprovedByUserName(string entityId, string documentsApprovedByUserName, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            ShipmentAdditionalCloudDataRepository rep = new ShipmentAdditionalCloudDataRepository(tenant);
            ShipmentAdditionalCloudData shipment = rep.GetSingleShipmentAdditionalCloudData(entityId, tenant);
            if (shipment != null /*&& shipment.IsDocumentsApprovalRequried*/ && string.IsNullOrEmpty(shipment.DocumentsApprovedByUserName))
            {
                shipment.DocumentsApprovedByUserName = documentsApprovedByUserName;
                rep.Update(shipment);
                rep.SubmitChanges();
                OpenDocumentApprovalQueue(entityId, documentsApprovedByUserName, tenant);
                return true;
            }
            return false;

        }

        public bool OpenDocumentApprovalQueue(string entityId, string documentsApprovedByUserName, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("DocumentApprovalQueue", 0);
            queueservice.Send(new Dictionary<string, string>() { { "Id", entityId }, { "Tenant", tenant.ToString() }, { "ApprovedByUserName", documentsApprovedByUserName } }, tenant);
            return true;
        }

        private List<SharedLogisticDocumentPM> GetShipmentSharedDocuments(Shipment shipment, string partnerType, int tenant, bool isExternalURL)
        {
            string entityId = shipment.Id;
            string shipmentLevelCode = shipment.ShipmentLevelCode;
            ARInvoiceRepository arInvoiceReps = new ARInvoiceRepository(tenant);
            List<ARInvoice> invoices = arInvoiceReps.GetInvoicesByShipmentIdAndBillToId(entityId, shipment.CustomerId, tenant);
            
            List<SharedLogisticDocumentPM> output = new List<SharedLogisticDocumentPM>();

            ICommonDataContext myContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingRepository myDocumentsFilingRepository = new DocumentsFilingRepository(myContext);
            DocumentsFilingQuery myDocumentsFilingQuery = new DocumentsFilingQuery(myDocumentsFilingRepository);
            List<DocumentsFilingPM> myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(entityId, tenant);
            Uploader uploader = new Uploader();
            if (partnerType == "AG")
            {
                myDocumentFilings = uploader.GetAgentDocuments(myDocumentFilings, shipmentLevelCode, tenant);
            }

            else if (partnerType == "CS")
            {
                myDocumentFilings = myDocumentFilings.Where(d => d.IsCustomerView).ToList();
            }

            IQueryable<DocumentOutCopy> allcopies = Enumerable.Empty<DocumentOutCopy>().AsQueryable();
            List<DocumentsFilingPM> missedDocuments = myDocumentFilings.Where(d => d.DirectionCode == "O" && d.DoucmentTypeTemplateFormatCode != "M" && d.DocumentId == null).ToList();
            if (missedDocuments.Count > 0)
            {
                List<string> allIds = missedDocuments.Select(s => s.Id).ToList();

                allcopies = (from d in myContext.DocumentOutCopies.Include("DocumentTypeCopy")
                             where allIds.Contains(d.DocumentOutId)
                             select d);
            }

            foreach (DocumentsFilingPM item in myDocumentFilings)
            {
                if (item.DirectionCode == "O" && item.DoucmentTypeTemplateFormatCode == "M")
                {
                    continue;
                }

                else
                {
                    bool addDocument = true;
                    if (item.DocumentTypeCode == "999S" && !string.IsNullOrEmpty(item.ChildEntityId))
                    {
                        if (!invoices.Where(d => d.Id == item.ChildEntityId).Any())
                        {
                            addDocument = false;
                        }
                    }

                    if (addDocument)
                    {
                        string myDocumentId = item.DocumentId;
                        string myFileName = isExternalURL ? item.FileName : item.CalculatedFileName;
                        string myFileExtension = item.FileExtension;
                        DocumentOutCopy documentOutCopy = null;

                        if (item.DirectionCode == "O" && item.DocumentId == null)
                        {
                            List<DocumentOutCopy> myCopies = allcopies.Where(d => d.DocumentOutId == item.Id).ToList();
                            if (myCopies.Count > 0)
                            {
                                documentOutCopy = myCopies.FirstOrDefault();
                                myDocumentId = documentOutCopy.DocumentId;
                                if (myDocumentId != null)
                                {
                                    Document myDocument = (from d in myContext.Documents
                                                           where d.Id == myDocumentId
                                                           select d).FirstOrDefault();

                                    if (myDocument != null)
                                    {
                                        if (isExternalURL)
                                        {
                                            myFileName = myDocument.FileName ?? documentOutCopy.DocumentTypeCopy.Name;
                                        }

                                        else
                                        {
                                            myFileName = !string.IsNullOrEmpty(myDocument.CalculatedFileName) ? myDocument.CalculatedFileName : myDocument.FileName;
                                        }

                                        myFileExtension = myDocument.Extension;
                                    }
                                }
                            }
                        }

                        string url = null;
                        string id = null;

                        if (isExternalURL)
                        {
                            id = item.Id;

                            string encodedUrl = item.SecurityId + "~" + tenant;
                            encodedUrl = documentOutCopy == null ? encodedUrl : encodedUrl + "~" + documentOutCopy.Id;
                            encodedUrl = HttpUtility.UrlEncode(encodedUrl);
                            url = "../WebPages/CorrespondenceDownloadpage.aspx?id=" + encodedUrl;
                        }

                        else
                        {
                            string myPrefix = (item.DirectionCode == "I") ? "DocIn:" : "DocOut:";
                            id = myPrefix + item.Id;

                            url = "../WebPages/SharedDownloadPage.aspx?id=" + tenant + ":" + myDocumentId + ":ship:" + entityId;
                        }

                        output.Add(new SharedLogisticDocumentPM()
                        {
                            Id = id,
                            Url = url,
                            Name = item.DocumentTypeName,
                            DocumentId = myDocumentId,
                            FileName = myFileName,
                            FileExtension = myFileExtension,
                            IsDigitallySigned = item.IsDigitallySigned,
                            Reference = item.ChildEntityReference,
                        });
                    }
                }
            }

            return output;
        }

        private bool CheckSharedContactAuthenticationForShipment(string agentId, string customerId, int tenant)
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

        //public string GetDownloadDocument(string documentId, string entityId, int tenant)
        //{
        //    DocumentInQuery documentInQuery = new DocumentInQuery(tenant);
        //    DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
            
        //    string theUri = "";
        //    string[] arr = documentId.Split(':');
        //    string docId = arr[1];
        //    if (arr[0] == "DocOut")
        //    {
        //        DocumentOutPM pm = documentOutQuery.GetSinglePM(docId, tenant);
        //        if (pm.TemplateType == "P")
        //        {
                    
        //            if (pm.DocumentOutCopies.Count() > 0)
        //            {
        //                docId = pm.DocumentOutCopies.FirstOrDefault().DocumentId;
        //            }
                    
        //            string documentName = tenant + "_" + docId;
        //            theUri = "../WebPages/Downloadpage.aspx?id=" + documentName;
        //        }
        //        else
        //        {
        //           // CommunicationLogPM commLog = communicationLogQuery.GetCommunicationLogPMsByEntityId(entityId, tenant).Where(l => l.DocumentOutId == docId).FirstOrDefault();

        //           // HtmlEditorWebService service = new HtmlEditorWebService();

        //           // byte[] resultFile = service.GetSentMessageHtmlBody(commLog.DocumentId, tenant);
        //           //System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
        //           //string htmlString = enc.GetString(resultFile);
        //            string filedata = docId + ":" + entityId + ":" + tenant;
        //           theUri = "../WebPages/Downloadpage.aspx?messagefiledata=" + filedata;//+ htmlString;

        //        }
        //        //MyHyperlinkButton myhyperButton = new MyHyperlinkButton(theUri);
        //        // myhyperButton.ClickMe();

        //    }
        //    else
        //    {
        //        DocumentInPM pm = documentInQuery.GetSinglePM(arr[1], tenant);
        //        string documentName = tenant + "_" + pm.DocumentId; 
        //        theUri = "../WebPages/Downloadpage.aspx?id=" + documentName;
        //    }

        //    return theUri;
        //}
    }
}