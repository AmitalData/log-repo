using System;
using System.Collections.Generic;
using System.Text;

namespace Simplog.PortableData.EntityListDTOs
{
    public class ShipmentListDTO
    {
        public string Id { get; set; }

        #region Financial properties
        public double? OpenReceivablesInLocalCurrency { get; set; }
        public double? AccountedReceivablesInLocalCurrency { get; set; }
        public double ProfitInLocalCurrency { get; set; }
        public double? EstimateProfitInLocalCurrency { get; set; }
        public double? OpenReceivablesInProfitCurrency { get; set; }
        public double? AccountedReceivablesInProfitCurrency { get; set; }
        public double? ProfitInProfitCurrency { get; set; }
        public double? EstimateProfitInProfitCurrency { get; set; }
        public double? OpenPayablesInLocalCurrency { get; set; }
        public double? AccountedPayablesInLocalCurrency { get; set; }
        public double? OpenPayablesInProfitCurrency { get; set; }
        public double? AccountedPayablesInProfitCurrency { get; set; }
        #endregion

        public bool IsFSRSent { get; set; }
        public DateTime? LastFSRStatusRequestDate { get; set; }
        public DateTime? FHLStatusDate { get; set; }
        public DateTime? FWBStatusDate { get; set; }
        public string CarrierLastStatusCode { get; set; }
        public string CarrierLastStatusName { get; set; }
        public DateTime? CarrierLastStatusDate { get; set; }
        public string FNAReason { get; set; }
        public string TransportModeId { get; set; }
        public string DirectionId { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime CreateDateTime { get; set; }

        public string MainCarriageCarrierId { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public string TruckNumber { get; set; }
        public string MainCarriageCarrierName { get; set; }

        public string FromPortId { get; set; }
        public string FromPort { get; set; }
        public string FromPortName { get; set; }
        public string FromPortCountry { get; set; }

        public string ToPortId { get; set; }
        public string ToPort { get; set; }
        public string ToPortName { get; set; }
        public string ToPortCountry { get; set; }

        public string House { get; set; }
        public string ShipmentType { get; set; }

        public string FollowUpType { get; set; }
        public string FollowUpTypeId { get; set; }
        public string FollowUpId { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string FollowUpOwner { get; set; }
        public string FollowUpOwnerId { get; set; }

        
        public string ShipmentViewId { get; set; }
        public string BasketId { get; set; }
        public bool IsOperationalClosed { get; set; }
        public DateTime LastUpdate { get; set; }

        public string Field1Id { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        public bool NewMessage { get; set; }

        public string FollowUpNotes { get; set; }
        public bool IsAnyConversation { get; set; }
        public int NumberOfShipments { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public double? ChargeableWeightInKG { get; set; }
        public byte[] LastModified { get; set; }
        public bool hasChanges { get; set; }
        public string StatusId { get; set; }
        public string StatusName { get; set; }
        public string Master { get; set; }
        public string DirectionName { get; set; }
        public string TransportModeName { get; set; }
        public string QuoteId { get; set; }
        public string ShipmentReceivableStatusCode { get; set; }
        public string ShipmentPayableStatusCode { get; set; }

        public string ShipmentReceivableStatusName { get; set; }
        public string ShipmentPayableStatusName { get; set; }

        public string UpdatedByUserId { get; set; }
        public DateTime? LastUpdateDate { get; set; }

        public double? OrderVolumetricWeight { get; set; }
        public double? OrderChargeableWeight { get; set; }
        public string DepartmentId { get; set; }
        public string BranchId { get; set; }

        public string LongMaster { get; set; }

        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string ShipperReference1 { get; set; }
        public string ProfitCurrencyCode { get; set; }
        public string LocalCurrencyCode { get; set; }
        public string NextLegCode { get; set; }
        public string NextLegName { get; set; }
        public DateTime? NextETD { get; set; }
        public DateTime? NextETA { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string ShipmentLevelName { get; set; }
        public string MasterShipmentDataId { get; set; }
        public string BranchName { get; set; }
        public string CustomerName { get; set; }
        public double? GrossWeightInKG { get; set; }
        public double? VolumetricWeight { get; set; }

        public double? ChargeableWeight { get; set; }
        public double? GrossWeight { get; set; }
        public string MasterShipmentNumber { get; set; }
        public string Routing { get; set; }
        public string AgentName { get; set; }
        public string SearchFields { get; set; }
        public DateTime? CutoffDate { get; set; }

        public string MainCarriageFromPortId { get; set; }
        public string MainCarriageFromPortName { get; set; } // origin
        public DateTime? MainCarriageATA { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public string IncotermId { get; set; }
        public string IncotermCode { get; set; }
        public DateTime? FinalArrivalDate { get; set; }
        public string CustomerReference { get; set; }
        public string IssuingCarrierAgentId { get; set; }
        public double? AWBChargeAmount { get; set; }
        public string AWBCommodityItemNumber { get; set; }

        public bool AWBPrint { get; set; }
        public string FWBStatusCode { get; set; }
        public string FHLStatusCode { get; set; }
        public string FWBStatusName { get; set; }
        public string FHLStatusName { get; set; }

        public string MainCarriageFromPartnerId { get; set; }
        public string MainCarriageFromAddressId { get; set; }
        public string MainCarriageToPartnerId { get; set; }
        public string MainCarriageToAddressId { get; set; }

        public string MainCarriageCarrierPrefix { get; set; }
        public string Transshipment1CarrierPrefix { get; set; }
        public string Transshipment2CarrierPrefix { get; set; }
        public string Transshipment3CarrierPrefix { get; set; }

        public string MainCarriageFullCarrierNumber { get; set; }
        public string Transshipment1FullCarrierNumber { get; set; }
        public string Transshipment2FullCarrierNumber { get; set; }
        public string Transshipment3FullCarrierNumber { get; set; }
        public bool AsAgreed { get; set; }
        public DateTime? ActivityDate { get; set; }
        public string ActivityTypeName { get; set; }
        public string ActivityByUserName { get; set; }

    }
}
