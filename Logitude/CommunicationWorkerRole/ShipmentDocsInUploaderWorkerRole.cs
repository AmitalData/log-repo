using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Data.Entity;
using Simplog.Data.ShipmentsModel;
using Logitude.Server.Tools.Helpers;

namespace CommunicationWorkerRole
{
    public class ShipmentDocsInUploaderWorkerRole : WorkerEntryPoint
    {
        private DbQueueService queueService;
        private int tenant;
        private string documentsFilingId = "";
        private string documentCode = "";
        private bool isDocumentUploaded = false;
        private bool isDocumentDeleted = false;
        private DateTime? recivedDate;
        private string recivedDateString;
        private ICommonDataContext commonContext;
        private ShipmentRepository shipmentRepository;
        private ShipmentDocsFieldRepository shipmentDocsFieldRepository;
        private IShipmentsContext shipmentContext;
        private ShipmentPM shipmentPM;
        private DocumentsFilingPM documentsFilingPM;
        private ShipmentDocsField shipmentDocsField;
        private bool isShipmentChange = false;
        private bool isApprovalRequired = false;
        private bool isUploadShipmentDocs = false;

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ShipmentDocsInUploaderWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            return base.OnStart();
        }

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        shipmentPM = null;
                        documentsFilingPM = null;
                        shipmentDocsField = null;
                        ReadQueueMessage();
                    }
                    catch (Exception exception)
                    {
                        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "Shipment Documnet Uploader execution log queue worker role start", null, null);
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }
        private void ReadQueueMessage()
        {
            queueService = new DbQueueService("ShipmentDocsInUploaderQueue", 0);
            var queueResponse = queueService.Receive();

            if (queueResponse != null && queueResponse.MessageId != null)
            {
                try
                {
                    MapQueueResponse(queueResponse);
                    GetReceivedDate();

                    if (recivedDate != null)
                    {
                        InitializeServices();
                        HandelShipmentFields();
                    }

                    queueService.Complete();
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "Shipment Documnet Uploader code WorkerRole Run Method", "", null);
                    queueService.CompleteAsFailed();
                    Thread.Sleep(10000);
                }
            }
            else
            {
                Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void MapQueueResponse(QueueResponse queueResponse)
        {
            tenant = int.Parse(queueResponse.MessageValues["Tenant"].ToString());
            documentsFilingId = queueResponse.MessageValues["EntityId"].ToString();
            documentCode = queueResponse.MessageValues["DocumentCode"].ToString();
            isDocumentUploaded = bool.Parse(queueResponse.MessageValues["IsDocumentUploaded"].ToString());
            isDocumentDeleted = bool.Parse(queueResponse.MessageValues["IsDocumentDeleted"].ToString());
            recivedDateString = queueResponse.MessageValues["RecivedDate"].ToString();
            isApprovalRequired = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("IsApprovalRequired") ? bool.Parse(queueResponse.MessageValues["IsApprovalRequired"].ToString()) : false;
            isUploadShipmentDocs = queueResponse.MessageValues != null && queueResponse.MessageValues.Keys.Contains("IsUploadShipmentDocs") ? bool.Parse(queueResponse.MessageValues["IsUploadShipmentDocs"].ToString()) : true;
        }
        private void GetReceivedDate()
        {
            if (!string.IsNullOrEmpty(recivedDateString))
            {
                DateTime parsedDate;
                bool success = DateTime.TryParse(recivedDateString, out parsedDate);
                if (success) 
                    recivedDate = parsedDate;
            }
        }

        private void InitializeServices()
        {
            commonContext = CommonDataContext.GetContext(tenant);
            shipmentContext = ShipmentsContext.GetContext(tenant);
            shipmentRepository = new ShipmentRepository(shipmentContext);
            shipmentDocsFieldRepository = new ShipmentDocsFieldRepository(shipmentContext);
        }

        private void HandelShipmentFields()
        {
            if (string.IsNullOrEmpty(documentsFilingId))
            {
                return;
            }
            documentsFilingPM = GetDocumentsFilingPM(documentsFilingId, tenant);
            if (documentsFilingPM == null)
            {
                return;
            }

            if (isDocumentDeleted && isUploadShipmentDocs)
            {
                HandelShipmentFieldsWhenDeletingDocument(documentsFilingPM);
            }
            else if (isDocumentUploaded && isUploadShipmentDocs)
            {
                HandelShipmentFieldsWhenUploadingDocument(documentsFilingPM);
            }

            if (isApprovalRequired)
            {
                MarkShipmentAsApprovalRequired();
            }

            UpdateShipment();
        }

        private void MarkShipmentAsApprovalRequired()
        {
            shipmentPM = GetShipment(documentsFilingPM.EntityId, documentsFilingPM.Tenant);
            if (shipmentPM == null || shipmentPM.IsDocumentsNeedApprove) return;
            shipmentPM.IsDocumentsNeedApprove = true;
            shipmentPM.IsDocsKPIsUpdatedFromWR = true;

            isShipmentChange = true;
        }

        private DocumentsFilingPM GetDocumentsFilingPM(string documentsFilingId, int tenant)
        {
            if (string.IsNullOrEmpty(documentsFilingId))
            {
                return null;
            }

            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
            return documentsFilingQuery?.GetSinglePM(documentsFilingId, tenant);
        }

        private void HandelShipmentFieldsWhenDeletingDocument(DocumentsFilingPM documentsFiling)
        {
            ObjectTableRepository ObjectTableRepository = new ObjectTableRepository(tenant);
            var shipmentObjectTable = ObjectTableRepository.GetSingleObjectTable(documentsFiling.ObjectTableId, tenant, false);
            if (shipmentObjectTable != null && shipmentObjectTable.Name == "Shipment")
            {
                var documentsFilings = this.GetShipmentDocumentFilings(documentsFiling, commonContext);
                this.UpdateShipmentWhenDeletingDocument(documentsFilings, documentsFiling);
            }
        }
        private List<DocumentsFilingPM> GetShipmentDocumentFilings(DocumentsFilingPM documentsFiling, ICommonDataContext commonContext)
        {
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(commonContext);
            var documentsFilings = (from a in commonContext.DocumentsFilings.Include("DocumentType")
                                    where a.Tenant == tenant && a.EntityId == documentsFiling.EntityId
                                    && a.ObjectTableId == documentsFiling.ObjectTableId
                                    && a.IsDeleted == false
                                    && (a.DocumentType != null && a.DocumentType.Code == documentCode)
                                    select new DocumentsFilingPM()
                                    {
                                        Id = a.Id,
                                        ReceivedDate = a.ReceivedDate,
                                        ReceivedByUserId = a.ReceivedByUserId
                                    }).ToList();

            return documentsFilings;
        }

        private void UpdateShipmentWhenDeletingDocument(List<DocumentsFilingPM> documentsFilings, DocumentsFilingPM documentsFiling)
        {
            if (documentsFilings == null || (documentsFilings != null && documentsFilings.Count() == 0))
            {
                UpdateShipment(documentsFiling, false, null);
            }
            else if ((documentsFilings != null && documentsFilings.Count() >= 1))
            {
                UpdateShipment(documentsFiling, true, documentsFilings.FirstOrDefault()?.ReceivedDate);
            }
        }

        private void HandelShipmentFieldsWhenUploadingDocument(DocumentsFilingPM documentFiling)
        {
            ObjectTableRepository ObjectTableRepository = new ObjectTableRepository(tenant);
            var shipmentObjectTable = ObjectTableRepository.GetSingleObjectTable(documentFiling.ObjectTableId, tenant, false);
            if (shipmentObjectTable != null && shipmentObjectTable.Name == "Shipment" && documentFiling.DocumentTypeCode == documentCode)
            {
                UpdateShipment(documentFiling, true, recivedDate);
            }
        }

        private void UpdateShipment(DocumentsFilingPM documentFiling, bool isReceived, DateTime? receivedDate)
        {
            string shipmentId = documentFiling.EntityId;
            if (string.IsNullOrEmpty(shipmentId))            
                return;           

            shipmentPM = GetShipment(shipmentId, tenant);
            if (shipmentPM == null) return;

            shipmentDocsField = this.GetEntity(shipmentId);
            if (shipmentDocsField != null)
            {
                this.HandleShipmentFields(shipmentDocsField, isReceived, receivedDate);
            }

            shipmentPM.IsDocsKPIsUpdatedFromWR = true;
            isShipmentChange = true;
        }

        private void UpdateShipment()
        {
            if (shipmentPM == null || !isShipmentChange) return;
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact receivedBy = contactRepository.GetSingleContact(documentsFilingPM.ReceivedByUserId, tenant);

            string email = "system@tenant" + tenant + ".com";
            if (receivedBy != null)
                email = receivedBy.Email;

            ShipmentService shipmentService = new ShipmentService(shipmentContext, shipmentPM, email);

            if (shipmentDocsField != null)
            {
                shipmentService.ShipmentDocsFieldFromWorkerRole = shipmentDocsField;
            }

            shipmentService.Update(true);
        }

        private ShipmentPM GetShipment(string shipmentId, int tenant)
        {
            if (shipmentPM != null) return shipmentPM;
            shipmentPM = new ShipmentQuery(shipmentRepository).GetSinglePMWithNoRestriction(shipmentId, tenant);
            return shipmentPM;
        }

        private ShipmentDocsField GetEntity(string id)
        {
            ShipmentDocsField shipmentDocsField = shipmentDocsFieldRepository.GetSingleShipmentDocsField(id, tenant);

            if (shipmentDocsField == null)
            {
                shipmentDocsField = new ShipmentDocsField();
            }

            return shipmentDocsField;
        }
        private void HandleShipmentFields(ShipmentDocsField shipmentDocsField, bool isReceived, DateTime? receivedDate)
        {
            if (documentCode == DocumentsCodes.POD)
            {
                shipmentDocsField.IsPODReceived = isReceived;
                shipmentDocsField.PODReceivedDate = receivedDate;
                shipmentPM.IsPODReceived = isReceived; // to be removed when Phoenix finish 
                shipmentPM.PODReceivedDate = receivedDate; // to be removed when Phoenix finish                
            }

            else if (documentCode == DocumentsCodes.CommercialInvoice)
            {
                shipmentDocsField.IsCommercialInvoiceReceived = isReceived;
                shipmentDocsField.CommercialInvoiceReceivedDate = receivedDate;
            }

            else if (documentCode == DocumentsCodes.PackingList)
            {
                shipmentDocsField.IsPackingListReceived = isReceived;
                shipmentDocsField.PackingListReceivedDate = receivedDate;
            }

            else if (documentCode == DocumentsCodes.BOL)
            {
                shipmentDocsField.IsBOLReceived = isReceived;
                shipmentDocsField.BOLReceivedDate = receivedDate;
            }

            else if (documentCode == DocumentsCodes.MasterBOL)
            {
                shipmentDocsField.IsMasterBOLReceived = isReceived;
                shipmentDocsField.MasterBOLReceivedDate = receivedDate;
            }

            else if (documentCode == DocumentsCodes.ArrivalNotice)
            {
                shipmentDocsField.IsArrivalNoticeReceived = isReceived;
                shipmentDocsField.ArrivalNoticeReceivedDate = receivedDate;
            }
        }
    }

    public class DocumentsCodes
    {
        public const string POD = "POD";
        public const string CommercialInvoice = "380";
        public const string PackingList = "721";
        public const string BOL = "706";
        public const string MasterBOL = "704";
        public const string ArrivalNotice = "ARNT";
    }
}
