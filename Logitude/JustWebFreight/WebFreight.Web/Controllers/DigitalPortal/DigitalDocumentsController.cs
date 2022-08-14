using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Data.Entity;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;


namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalDocumentsController : ApiController
    {
        [HttpGet]
        [Route("DigitalDocuments/GetDigitalEntityDocuments")]
        public List<SharedLogisticDocumentPM> GetDigitalEntityDocuments(string entityId, string partnerType, int tenant)
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

        #region private 
        
        private bool CheckSharedContactAuthenticationForShipment(string agentId, string customerId, int tenant)
        {
            if (tenant != 0)
            {
                bool exists = false;
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
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
                    if ((item.DocumentTypeCode == "999S" || item.DocumentTypeCode == "999CI") && !string.IsNullOrEmpty(item.ChildEntityId))
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
                        var invoiceDocument = invoices.Where(d => d.Id == item.ChildEntityId).FirstOrDefault();
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
                            encodedUrl = WebUtility.UrlEncode(encodedUrl);
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
                            ReceivedDate = item.ReceivedDate,
                            PrintDate = invoiceDocument?.PrintDate,
                            IsPrinted = invoiceDocument?.IsPrinted,
                            DirectionCode = item.DirectionCode,
                        });
                    }
                }
            }

            return output;
        }

        #endregion private
    }
}