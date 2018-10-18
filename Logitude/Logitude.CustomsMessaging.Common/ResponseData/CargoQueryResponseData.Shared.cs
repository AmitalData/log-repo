using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class CargoQueryResponseData : ResponseDataBase
    {
        public string ApplicationID { get; set; }
        public string ResponseStatusXML { get; set; }
        public bool IsShowUserMessage { get; set; } //Yuval Chalup 07.03.2016 TASK-20176

        public string ManifestType { get; set; }
        public string ManifestTypeName { get; set; }
        public string ManifestStatus { get; set; }
        public string ManifestStatusName { get; set; }
        public string Manifestnumber { get; set; }
        public bool IsSendingMorethenOneFlights { get; set; }
        public CargoResult CargoResultList { get; set; }
        public List<DeliveryOrderResult> DeliveryOrderResultList { get; set; }
        public List<CargosVersionResult> CargosVersionResultList { get; set; }
        public List<GatepassResult> GatepassResultList { get; set; }
        public List<CargoItemResult> CargoItemResultList { get; set; }
        public List<CargoDocumentResult> CargoDocumentResultList { get; set; }
        public List<CargoErrorResult> CargoErrorResultList { get; set; }
        //public OutgoingMessageResult OutgoingMessageResultList { get; set; }
    }

    public class CargoResult
    {
        public string CargoIdentifierKey1 { get; set; }
        public string CargoIdentifierKey2 { get; set; }
        public string ParentCargoID { get; set; }
        public string MasterBolNumber { get; set; }
        public string BillOfLadingNumber { get; set; }
        public string GovernmentProcedureType { get; set; }
        public string GovernmentProcedureTypeName { get; set; }
        public string TreatmentWayCode { get; set; }
        public string TreatmentWayName { get; set; }
        public string TotalNumberOfPackeges { get; set; }
        public string TotalWeight { get; set; }
        public CargoAdditionalData CargoAdditionalDataList { get; set; }
    }

    public class CargoAdditionalData
    {
        public string Version { get; set; }
        public string ManifestNumber { get; set; }
        public string UnloadingLocationID { get; set; }
        public string UnloadingLocationName { get; set; }
        public string GoodsReceiptPlaceSiteID { get; set; }
        public string GoodsReceiptPlaceSiteName { get; set; }
        public string BoardedQuantity { get; set; }
        public string TotalRecordNumberOfPackeges { get; set; }
        public string TotalRecordWeight { get; set; }
        public string TransitDestinationLocationID { get; set; }
        public string TransitDestinationLocationName { get; set; }
        public string AcceptedArrivalSiteID { get; set; }
        public string StorageSiteID { get; set; }
        public string StorageSiteName { get; set; }
        public string StorageDate { get; set; }
    }

    public class DeliveryOrderResult
    {
        public string DeliveryOrderNumber { get; set; }
        public string ProducerName { get; set; }
        public string ReceiverCustomerActivityType { get; set; }
        public string ReceiverCustomerActivityTypeName { get; set; }
        public string ReceiverName { get; set; }
        public string DeliveryOrderDate { get; set; }
        public string DeliveryOrderStatus { get; set; }
        public string DeliveryOrderStatusName { get; set; }
        public string DeliverySiteId { get; set; }
        public string DeliverySiteName { get; set; }
        public string ResponseStatus { get; set; }
    }

    public class CargosVersionResult
    {
        public string Version { get; set; }
        public string SubmiterName { get; set; }
        public string CreateDate { get; set; }
        public string ActionDate { get; set; }
        public string CargoStatus { get; set; }
        public string CargoStausName { get; set; }
    }

    public class GatepassResult
    {
        public string GatepassNumber { get; set; }
        public string UpdateCode { get; set; }
        public string UpdateName { get; set; }
        public string SubmiterName { get; set; }
        public string SourceSiteCode { get; set; }
        public string SourceSiteName { get; set; }
        public string DestinationSiteCode { get; set; }
        public string DestinationSiteName { get; set; }
        public string RequestDate { get; set; }
        public string ConfirmDate { get; set; }
        public string GatepassStatus { get; set; }
        public string GatepassStatusName { get; set; }
        public bool IsGatepassImplemented { get; set; }
    }

    public class CargoItemResult
    {
        public string RowNumber { get; set; }
        public string ParentCargoRowDetailsID { get; set; }
        public string ContainerNumber { get; set; }
        public string CharacteristicCode { get; set; }
        public string ContainerType { get; set; }
        public string Length { get; set; }
        public string PackingType { get; set; }
        public string Quantity { get; set; }
        public string GrossMassMeasureWeight { get; set; }
        public string RecordNumberOfPackeges { get; set; }
        public string TotalRecordWeight { get; set; }
        public string DangerousGoodsIndication { get; set; }
        public List<SealDetails> SealDetailsList { get; set; }
        public List<CargoMovment> CargoMovmentList { get; set; }
    }

    public class SealDetails
    {
        public string RowNumber { get; set; }
        public string SealType { get; set; }
        public string SealTypeName { get; set; }
        public string SealNumber { get; set; }
    }

    public class CargoMovment
    {
        public string RowNumber { get; set; }
        public string ExitReasonID { get; set; }
        public string ExitReasonName { get; set; }
        public string StatusID { get; set; }
        public string StatusName { get; set; }
        public string DocumentNumber { get; set; }
        public string ReferenceTypeID { get; set; }
        public string ReferenceTypeName { get; set; }
        public string ReferenceNum { get; set; }
        public string ExitSiteId { get; set; }
        public string ExitSiteName { get; set; }
        public string ExitDateTime { get; set; }
        public string EntrySiteId { get; set; }
        public string EntrySiteName { get; set; }
        public string EntryDateTime { get; set; }
    }

    public class CargoDocumentResult
    {
        public string ExternalIDNum { get; set; }
        public string TypeName { get; set; }
        public string RequiredDate { get; set; }
        public string AcceptedDate { get; set; }
        public string DocumentID { get; set; }
    }

    public class CargoErrorResult
    {
        public string errorDate { get; set; }
        public string errorCode { get; set; }
        public string errorSource { get; set; }
        public string errorSourceName { get; set; }
        public string errorText { get; set; }
    }
}
