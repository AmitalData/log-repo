using System.Collections.Generic;
using System.Linq;
using System.Net;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.WebServices;
using System.Data.Entity;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.Controllers.DigitalPortal.Helpers
{
    public class DigitalPortalDocumentHelper
    {
        public List<SharedLogisticDocumentPM> GetShipmentSharedDocuments(dynamic args /*Shipment shipment, string partnerType, int tenant, bool isExternalURL*/)
        {
            string entityId = args.Id;
            string shipmentLevelCode = args.ShipmentLevelCode;
            var tenant = args.Tenant;
            ARInvoiceRepository arInvoiceReps = new ARInvoiceRepository(tenant);
            List<ARInvoice> invoices = arInvoiceReps.GetDigitalInvoicesByShipmentIdAndBillToId(entityId, args.CustomerId, tenant);
            List<SharedLogisticDocumentPM> output = new List<SharedLogisticDocumentPM>();
            ICommonDataContext myContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingRepository myDocumentsFilingRepository = new DocumentsFilingRepository(myContext);
            DocumentsFilingQuery myDocumentsFilingQuery = new DocumentsFilingQuery(myDocumentsFilingRepository);
            List<DocumentsFilingPM> myDocumentFilings = myDocumentsFilingQuery.GetDocumentsFilingPMsByEntityId(entityId, tenant);
            Uploader uploader = new Uploader();
            var docAth = AuthenticationUtil.GenerateToken();

            if (args.PartnerType == "AG")
            {
                myDocumentFilings = uploader.GetAgentDocuments(myDocumentFilings, shipmentLevelCode, tenant);
            }
            else if (args.PartnerType == "CS")
            {
                myDocumentFilings = myDocumentFilings.Where(d => d.IsCustomerView || d.IsCustomerUploadPermission).ToList();
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
                        string myFileName = item.CalculatedFileName;
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
                                        myFileName = !string.IsNullOrEmpty(myDocument.CalculatedFileName) ? myDocument.CalculatedFileName : myDocument.FileName;
                                        myFileExtension = myDocument.Extension;
                                    }
                                }
                            }
                        }

                        string url = null;
                        string id = null;
                        if (args.IsExternal)
                        {
                            id = item.Id;
                            string encodedUrl = item.SecurityId + "~" + tenant;
                            encodedUrl = documentOutCopy == null ? encodedUrl : encodedUrl + "~" + documentOutCopy.Id;
                            encodedUrl = WebUtility.UrlEncode(encodedUrl);
                        //  url = "../WebPages/CorrespondenceDownloadpage.aspx?id=" + encodedUrl;
                            url = "../api/CorrespondenceDownload/ValidateAndDownloadDocument?id=" + encodedUrl;
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
                            IsCustomerUploadPermission = item.IsCustomerUploadPermission,
                            IsRequired = item.IsRequested,
                        });
                    }
                }
            }

            return output;
        }
    }
}