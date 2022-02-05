using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.CRM.Data;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;

namespace CommunicationWorkerRole
{
    public class PODDocumnetUploaderWR : WorkerEntryPoint
    {
        DbQueueService queueService;
        int tenant;
        string documentsFilingId = "";
        bool isPODDocumentUploaded = false;
        bool isPODDocumentDeleted = false;
        DateTime? podRecivedDate;
        Shipment shipment;
        private List<EventType> allEventTypes;
        private string objectTableId;
        private string objectTableName = "Shipment";
        TraceEventRepository traceEventRepository;
        private IWebFreightContext objectContext;
        ObjectTable objectTable;
        private ShipmentMasterDataRepository shipmentMasterDataRepository;
        ShipmentRepository shipmentRepository;
        IShipmentsContext shipmentContext;
        ContainerService containerService;
        ShipmentPM shipmentPM;

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "PODDocumnetUploaderWorkerRole";
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
                        UpdateIsPODUploaded();
                    }
                    catch (Exception exception)
                    {
                        ExceptionHandler.HandleException(exception, DateTime.Now, 0, null, "POD Documnet Uploader execution log queue worker role start", null, null);
                        Thread.Sleep(new TimeSpan(0, 0, 1));
                    }
                }
                else Thread.Sleep(new TimeSpan(0, 0, 1));
            }
        }

        private void UpdateIsPODUploaded()
        {
            queueService = new DbQueueService("PODDocumnetUploaderQueue", 0);
            var queueResponse = queueService.Receive();

            if (queueResponse != null && queueResponse.MessageId != null)
            {
                try
                {
                    MapQueueResponse(queueResponse);
                    InitializeServices();
                    FillEventTraces();
                    HandelPODShipmentFields();
                    queueService.Complete();
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "POD Documnet Uploader code WorkerRole Run Method", "", null);
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
            isPODDocumentUploaded = bool.Parse(queueResponse.MessageValues["IsPODDocumentUploaded"].ToString());
            isPODDocumentDeleted = bool.Parse(queueResponse.MessageValues["IsPODDocumentDeleted"].ToString());
            podRecivedDate = DateTime.Parse(queueResponse.MessageValues["PODRecived"].ToString());
        }

        private void HandelPODShipmentFields()
        {
            if (string.IsNullOrEmpty(documentsFilingId))
            {
                return;
            }
            DocumentsFilingPM documentsFilingPM = GetDocumentsFilingPM(documentsFilingId, tenant);
            if (documentsFilingPM == null)
            {
                return;
            }

            if (isPODDocumentDeleted)
            {
                HandelPODShipmentFieldsWhenDeletingPODDocument(documentsFilingPM);
            }
            else if (isPODDocumentUploaded)
            {
                HandelPODShipmentFieldsWhenUploadingPODDocument(documentsFilingPM);
            }
        }
        private void InitializeServices()
        {
            objectContext = WebFreightContext.GetContext(tenant);
            var objectTabelRepository = new ObjectTableRepository(objectContext);
            traceEventRepository = new TraceEventRepository(objectContext);
            objectTable = objectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            shipmentMasterDataRepository = new ShipmentMasterDataRepository(tenant);
            shipmentRepository = new ShipmentRepository(tenant);
        }

        private void FillEventTraces()
        {
            this.objectTableId = objectTable.Id;
            var eventTypeRepository = new EventTypeRepository(objectContext);
            this.allEventTypes = eventTypeRepository.GetEventTypesByTenantAndObjectTableId(tenant, objectTableId).ToList();
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

        private void HandelPODShipmentFieldsWhenDeletingPODDocument(DocumentsFilingPM documentsFiling)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ObjectTableRepository ObjectTableRepository = new ObjectTableRepository(tenant); ;
            var shipmentObjectTable = ObjectTableRepository.GetSingleObjectTable(documentsFiling.ObjectTableId, tenant, false);
            if (shipmentObjectTable != null && shipmentObjectTable.Name == "Shipment")
            {
                var documentsFilings = this.GetShipmentDocumentFilings(documentsFiling, commonContext);
                this.UpdatePODShipmentWhenDeletingPODDocument(documentsFilings, documentsFiling);
            }
        }

        private List<DocumentsFilingPM> GetShipmentDocumentFilings(DocumentsFilingPM documentsFiling, ICommonDataContext commonContext)
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(commonDataContext);
            var documentsFilings = (from a in commonContext.DocumentsFilings.Include("DocumentType")
                                    where a.Tenant == tenant && a.EntityId == documentsFiling.EntityId
                                    && a.ObjectTableId == documentsFiling.ObjectTableId
                                    && a.IsDeleted == false
                                    && (a.DocumentType != null && a.DocumentType.Code == "POD")
                                    select new DocumentsFilingPM()
                                    {
                                        Id = a.Id,
                                        ReceivedDate = a.ReceivedDate,
                                    }).ToList();

            return documentsFilings;
        }

        private void UpdatePODShipmentWhenDeletingPODDocument(List<DocumentsFilingPM> documentsFilings, DocumentsFilingPM documentsFiling)
        {
            if (documentsFilings == null || (documentsFilings != null && documentsFilings.Count() == 0))
            {
                UpdateShipment(documentsFiling.EntityId, false, null);
            }
            else if ((documentsFilings != null && documentsFilings.Count() >= 1))
            {
                UpdateShipment(documentsFiling.EntityId, true, documentsFilings.FirstOrDefault()?.ReceivedDate);
            }
        }

        private void HandelPODShipmentFieldsWhenUploadingPODDocument(DocumentsFilingPM documentFiling)
        {
            ObjectTableRepository ObjectTableRepository = new ObjectTableRepository(tenant);
            var shipmentObjectTable = ObjectTableRepository.GetSingleObjectTable(documentFiling.ObjectTableId, tenant, false);
            if (shipmentObjectTable != null && shipmentObjectTable.Name == "Shipment" && documentFiling.DocumentTypeCode == "POD")
            {
                UpdateShipment(documentFiling.EntityId, true, podRecivedDate);
            }
        }

        private void UpdateShipment(string shipmentId, bool isPODReceived, DateTime? podReceivedDate)
        {
            if (string.IsNullOrEmpty(shipmentId))
            {
                return;
            }
            shipment = shipmentRepository.GetSingleShipment(shipmentId, tenant);
            shipment.IsPODReceived = isPODReceived;
            shipment.PODReceivedDate = podReceivedDate;
            this.UpdateShipmentContainers(shipmentId, podReceivedDate, tenant);
            this.HandelPODShipmentEvent(isPODReceived, podReceivedDate);
            shipmentRepository.Update(shipment);
            shipmentRepository.SubmitChanges();
        }

        private void UpdateShipmentContainers(string shipmentId, DateTime? podReceivedDate, int tenant)
        {
            shipmentContext = ShipmentsContext.GetContext(tenant);
            containerService = new ContainerService(shipmentContext, tenant);
            List<ContainerPM> containers = GetShipmentContainers(shipmentId, tenant);
            foreach (ContainerPM container in containers)
            {
                this.UpdateContainer(container, podReceivedDate);
            }
        }
        private List<ContainerPM> GetShipmentContainers(string shipmentId, int tenant)
        {
            ContainerQuery containerQuery = new ContainerQuery(tenant);
            List<ContainerPM> containerPMs = new List<ContainerPM>();
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            shipmentPM = shipmentQuery.GetSinglePM(shipmentId, tenant);
            var shipmentContainers = shipmentPM?.ShipmentPackages?.Where(a => !string.IsNullOrEmpty(a.ContainerEntityId));
            foreach (ShipmentPackagePM shipmentPackagePM in shipmentContainers)
            {
                containerPMs.Add(containerQuery.GetSinglePM(shipmentPackagePM.ContainerEntityId, shipmentPM.Tenant));
            }
            return containerPMs;
        }

        private void UpdateContainer(ContainerPM container, DateTime? podReceivedDate)
        {
            container.PODReceivedOnDate = podReceivedDate;
            containerService.Update(container);
        }
   
        private void HandelPODShipmentEvent(bool isPODReceived, DateTime? podReceivedDate)
        {
            var isNew = false;
            var shipmentTracing = new ShipmentTracing(shipmentPM, shipment, null, shipment?.UpdatedByUserId, isNew);
            shipmentTracing.TracePODReceived(isPODReceived, podReceivedDate);
        }
    }
}
