using System;
using System.Collections.Generic;
using System.Text;

namespace Simplog.PortableData.EntityDTOs
{
    public class ShipmentDTO:EntityDTO<ShipmentDTO>
    {
        private string id;
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public bool IsCopy { get; set; }
        public string BaseShipmentNumber { get; set; }
        public string ConcurrencyGUID { get; set; }
        public bool ConvertFromHouseToDirect { get; set; }
        public bool ConvertFromDirectToHouse { get; set; }
        public string CustomerRankName { get; set; }
        public DateTime? FinalArrivalDate { get; set; }
        public string MainCarriageFullCarrierNumber { get; set; }
        public string Transshipment1FullCarrierNumber { get; set; }
        public string Transshipment2FullCarrierNumber { get; set; }
        public string Transshipment3FullCarrierNumber { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }

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

        #region Fields
        public bool IsFSRSent { get; set; }
        public DateTime? LastFSRStatusRequestDate { get; set; }
        public DateTime? FHLStatusDate { get; set; }
        public DateTime? FWBStatusDate { get; set; }
        public string CarrierLastStatusCode { get; set; }
        public string CarrierLastStatusName { get; set; }
        public DateTime? CarrierLastStatusDate { get; set; }
        public string FNAReason { get; set; }
        public string AWBSpecialHandlingCodeCode1 { get; set; }
        public string AWBSpecialHandlingCodeCode2 { get; set; }
        public string AWBSpecialHandlingCodeCode3 { get; set; }
        public string AWBSpecialHandlingCodeCode4 { get; set; }
        public string AWBSpecialHandlingCodeCode5 { get; set; }
        public string AWBSpecialHandlingCodeCode6 { get; set; }
        public string AWBSpecialHandlingCodeCode7 { get; set; }
        public string AWBSpecialHandlingCodeCode8 { get; set; }
        public string AWBSpecialHandlingCodeCode9 { get; set; }
        public double? AWBChargeRate { get; set; }
        public double? AWBChargeAmount { get; set; }
        public string AWBCommodityItemNumber { get; set; }
        public string PPCC { get; set; }
        public DateTime? FlightDate { get; set; }
        public bool IsFlightDateActual { get; set; }
        public int ConnectedShipments { get; set; }
        public string MasterShipmentNumber { get; set; }
        public double? ChargeableWeightInKG { get; set; }
        public double? GrossWeightInKG { get; set; }
        public double? ChargeableWeight { get; set; }
        public double? GrossWeight { get; set; }
        public string CurrentUserId { get; set; }
        public int Tenant { get; set; }
        public string BasketId { get; set; }
        public string ShipmentNumber { get; set; }
        public string DirectionId { get; set; }
        public string DirectionName { get; set; }
        public string TransportModeId { get; set; }
        public string TransportModeName { get; set; }
        public string ShipmentTypeId { get; set; }
        public string ShipmentTypeName { get; set; }
        public string House { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool MAWBTakenFromStack { get; set; }
        public bool MAWBReturnedToStack { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string IncotermId { get; set; }
        public string IncotermCode { get; set; }
        public string Routing { get; set; }
        public string SalesmanUserId { get; set; }
        public string CreatedByUserId { get; set; }
        public string DepartmentId { get; set; }
        public string Notes { get; set; }
        public string DescriptionOfGoods { get; set; }
        public byte[] LastModified { get; set; }
        public DateTime? HAWBDate { get; set; }
        public string MAWBStackNumber { get; set; }
        public bool IsOperationalClosed { get; set; }
        //public CustomFieldClass Field1 { get; set; }
        //public CustomFieldClass Field2 { get; set; }
        //public CustomFieldClass Field3 { get; set; }
        //public CustomFieldClass Field4 { get; set; }
        //public CustomFieldClass Field5 { get; set; }
        //public CustomFieldClass Field6 { get; set; }
        //public CustomFieldClass Field7 { get; set; }
        //public CustomFieldClass Field8 { get; set; }
        //public CustomFieldClass Field9 { get; set; }
        //public CustomFieldClass Field10 { get; set; }
        public string SearchFields { get; set; }
        public bool IsSecured { get; set; }
        public string Master { get; set; }
        public string ShipmentTypeViewField { get; set; }
        public string LongMaster { get; set; }
        public string FreightPrepaidCollectId { get; set; }
        public string OtherPrepaidCollectId { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public string DimensionsUnitCode { get; set; }
        public string RateClassCode { get; set; }
        public double? VolumetricWeight { get; set; }
        public double? VolumeInCBM { get; set; }
        public double? Volume { get; set; }
        public int? NumberOfPackages { get; set; }
        public int? NumberOfContainers { get; set; }
        public double? Ratio { get; set; }
        public DateTime? MAWBOBLDate { get; set; }
        public bool GrossWeightEdited { get; set; }
        public bool ChargeableWeightEdited { get; set; }
        public string VolumeUnitCode { get; set; }
        public string StatusId { get; set; }
        string statusname;
        public string StatusName { get { return statusname; } set { statusname = value; } }
        public string QuoteId { get; set; }
        public string TotalContainers { get; set; }
        public string ShipmentType { get; set; }
        public string FollowUpType { get; set; }
        public string FollowUpId { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string ShipmentPMId { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool NewMessage { get; set; }
        public string FollowUpNotes { get; set; }
        public bool IsAnyConversation { get; set; }
        public int NumberOfShipments { get; set; }
        public string MainCarriageFinalDestinationPortId { get; set; }
        public string MainCarriageFinalDestinationPortCode { get; set; }
        public string MainCarriageFinalDestinationPortName { get; set; }
        public string MainHarmonize { get; set; }
        public bool IsDangerous { get; set; }
        public string DangerousClassNumber { get; set; }
        public string DangerousUnNumber { get; set; }
        public string DangerousPackagingGroup { get; set; }
        public string DangerousIMDGCode { get; set; }
        public string DangerousFlashPoint { get; set; }
        public string DangerousMaterialDescription { get; set; }
        public bool LTCWEdited { get; set; }
        public int ShipmentPickUpIndex { get; set; }
        public int ShipmentDeliveryIndex { get; set; }
        public string ShipmentPayableStatusCode { get; set; }
        public string ShipmentReceivableStatusCode { get; set; }
        public string ShipmentReceivableStatusName { get; set; }
        public string ShipmentPayableStatusName { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsAccountingClosed { get; set; }
        public DateTime AccessDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string ProfitCurrencyId { get; set; }
        public double? ProfitExchangeRate { get; set; }
        public string UpdatedByUserName { get; set; }
        public string EventNote { get; set; }
        public string NextLegCode { get; set; }
        public string NextLegName { get; set; }
        public DateTime? NextETD { get; set; }
        public DateTime? NextETA { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string ShipmentLevelName { get; set; }
        public string MasterShipmentDataId { get; set; }
        #endregion

        #region Partners
        public string ShipmentCustomerTypeCode { get; set; }
        public bool ConsigneeAddressOneTime { get; set; }
        public bool ShipperAddressOneTime { get; set; }
        public string CustomerId { get; set; }
        public string CustomerAddressId { get; set; }
        public string CustomerContactId { get; set; }
        public string CustomerReference { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNote { get; set; }

        public string IssuingCarrierAgentId { get; set; }
        public string IssuingCarrierAddressId { get; set; }
        public string IssuingCarrierAgentName { get; set; }
        public string IssuingCarrierAgentNote { get; set; }
        public string IssuingCarrierIATACode { get; set; }

        public string FreightForwarderId { get; set; }
        public string FreightForwarderAddressId { get; set; }
        public string FreightForwarderContactId { get; set; }
        public string FreightForwarderReference { get; set; }
        public string FreightForwarderName { get; set; }
        public string FreightForwarderNote { get; set; }

        public string ShipperId { get; set; }
        public string ShipperAddressId { get; set; }
        public string ShipperContactId { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }
        public string ShipperName { get; set; }
        public string ShipperNote { get; set; }
        public string ShipperAddressText { get; set; }

        public string ConsigneeId { get; set; }
        public string ConsigneeAddressId { get; set; }
        public string ConsigneeContactId { get; set; }
        public string ConsigneeReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeNote { get; set; }
        public string ConsigneeAddressText { get; set; }

        public string AgentId { get; set; }
        public string AgentAddressId { get; set; }
        public string AgentContactId { get; set; }
        public string AgentReference1 { get; set; }
        public string AgentReference2 { get; set; }
        public string AgentName { get; set; }
        public string AgentNote { get; set; }

        public string CustomAgentExportId { get; set; }
        public string CustomAgentExportAddressId { get; set; }
        public string CustomAgentExportContactId { get; set; }
        public string CustomAgentExportReference { get; set; }
        public string CustomAgentExportName { get; set; }
        public string CustomAgentExportNote { get; set; }

        public string CustomAgentImportId { get; set; }
        public string CustomAgentImportAddressId { get; set; }
        public string CustomAgentImportContactId { get; set; }
        public string CustomAgentImportReference { get; set; }
        public string CustomAgentImportName { get; set; }
        public string CustomAgentImportNote { get; set; }

        public string Notify1Id { get; set; }
        public string Notify1AddressId { get; set; }
        public string Notify1ContactId { get; set; }
        public string Notify1Name { get; set; }
        public string Notify1Note { get; set; }

        public string Notify2Id { get; set; }
        public string Notify2AddressId { get; set; }
        public string Notify2ContactId { get; set; }
        public string Notify2Name { get; set; }
        public string Notify2Note { get; set; }

        public string ShipperNotExporterId { get; set; }
        public string ShipperNotExporterAddressId { get; set; }
        public string ShipperNotExporterContactId { get; set; }
        public string ShipperNotExporterName { get; set; }
        public string ShipperNotExporterNote { get; set; }

        public string ConsigneeNotImporterId { get; set; }
        public string ConsigneeNotImporterAddressId { get; set; }
        public string ConsigneeNotImporterContactId { get; set; }
        public string ConsigneeNotImporterName { get; set; }
        public string ConsigneeNotImporterNote { get; set; }
        #endregion

        #region Routings
        public string MainCarriageCarrierPrefix { get; set; }
        public string Transshipment1CarrierPrefix { get; set; }
        public string Transshipment2CarrierPrefix { get; set; }
        public string Transshipment3CarrierPrefix { get; set; }
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public string PreCarriageTransportModeId { get; set; }
        public string PreCarriageFromPortId { get; set; }
        public string PreCarriageToPortId { get; set; }
        public string PreCarriageCarrierId { get; set; }
        public string PreCarriageCarrierNumber { get; set; }
        public string PreCarriageCarrierName { get; set; }
        public string PreCarriageCarrierCode { get; set; }
        public string PreCarriageFromPortCode { get; set; }
        public string PreCarriageFromPortName { get; set; }
        public string PreCarriageFromPortCountryCode { get; set; }
        public string PreCarriageFromPortCountryName { get; set; }
        public string PreCarriageToPortCode { get; set; }
        public string PreCarriageToPortName { get; set; }
        public string PreCarriageToPortCountryCode { get; set; }
        public string PreCarriageToPortCountryName { get; set; }
        public DateTime? PreCarriageETD { get; set; }
        public DateTime? PreCarriageATD { get; set; }
        public DateTime? PreCarriageETA { get; set; }
        public DateTime? PreCarriageATA { get; set; }
        public string PreCarriageVesselId { get; set; }
        public string PreCarriageCarrierWebSite { get; set; }

        public string OnCarriageTransportModeId { get; set; }
        public string OnCarriageFromPortId { get; set; }
        public string OnCarriageToPortId { get; set; }
        public string OnCarriageCarrierId { get; set; }
        public string OnCarriageCarrierNumber { get; set; }
        public string OnCarriageCarrierName { get; set; }
        public string OnCarriageCarrierCode { get; set; }
        public string OnCarriageFromPortCode { get; set; }
        public string OnCarriageFromPortName { get; set; }
        public string OnCarriageFromPortCountryCode { get; set; }
        public string OnCarriageFromPortCountryName { get; set; }
        public string OnCarriageToPortCode { get; set; }
        public string OnCarriageToPortName { get; set; }
        public string OnCarriageToPortCountryCode { get; set; }
        public string OnCarriageToPortCountryName { get; set; }
        public DateTime? OnCarriageETD { get; set; }
        public DateTime? OnCarriageATD { get; set; }
        public DateTime? OnCarriageETA { get; set; }
        public DateTime? OnCarriageATA { get; set; }

        public DateTime? MainCarriageFinalDestinationETA { get; set; }
        public DateTime? MainCarriageFinalDestinationATA { get; set; }

        public string OnCarriageVesselId { get; set; }
        public string OnCarriageCarrierWebSite { get; set; }

        public string MainCarriageTransportModeId { get; set; }
        public string MainCarriageFromPortId { get; set; }
        public string MainCarriageToPortId { get; set; }
        public string MainCarriageCarrierName { get; set; }
        public string MainCarriageCarrierCode { get; set; }
        public string MainCarriageFromPortCode { get; set; }
        public string MainCarriageFromPortName { get; set; }
        public string MainCarriageFromPortCountryName { get; set; }
        public string MainCarriageFromPortCountryCode { get; set; }
        public string MainCarriageToPortCode { get; set; }
        public string MainCarriageToPortName { get; set; }
        public string MainCarriageToPortCountryCode { get; set; }
        public string MainCarriageToPortCountryName { get; set; }
        public bool MainCarriageFromPortCountryEC { get; set; }
        public bool MainCarriageToPortCountryEC { get; set; }

        public string MainCarriageVesselId { get; set; }
        public string MainCarriageVesselName { get; set; }

        public bool MainCarriageIsFromStack { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public string MainCarriageCarrierAddressId { get; set; }

        public string MainCarriageCarrierId { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public string MainCarriageCarrierWebSite { get; set; }

        public string Transshipment1FromPortId { get; set; }
        public string Transshipment1ToPortId { get; set; }
        public DateTime? Transshipment1ATD { get; set; }
        public DateTime? Transshipment1ATA { get; set; }
        public DateTime? Transshipment1ETD { get; set; }
        public DateTime? Transshipment1ETA { get; set; }
        public string Transshipment1CarrierNumber { get; set; }
        public string Transshipment1CarrierId { get; set; }
        public string Transshipment1CarrierName { get; set; }
        public string Transshipment1CarrierCode { get; set; }
        public string Transshipment1FromPortCode { get; set; }
        public string Transshipment1FromPortName { get; set; }
        public string Transshipment1FromPortCountryCode { get; set; }
        public string Transshipment1FromPortCountryName { get; set; }
        public string Transshipment1ToPortCode { get; set; }
        public string Transshipment1ToPortName { get; set; }
        public string Transshipment1ToPortCountryCode { get; set; }
        public string Transshipment1ToPortCountryName { get; set; }
        public string Transshipment1CarrierWebSite { get; set; }
        public string Transshipment2CarrierWebSite { get; set; }
        public string Transshipment3CarrierWebSite { get; set; }

        public string Transshipment2FromPortId { get; set; }
        public string Transshipment2ToPortId { get; set; }
        public DateTime? Transshipment2ATD { get; set; }
        public DateTime? Transshipment2ATA { get; set; }
        public DateTime? Transshipment2ETD { get; set; }
        public DateTime? Transshipment2ETA { get; set; }
        public string Transshipment2CarrierNumber { get; set; }
        public string Transshipment2CarrierId { get; set; }
        public string Transshipment2CarrierName { get; set; }
        public string Transshipment2CarrierCode { get; set; }
        public string Transshipment2FromPortCode { get; set; }
        public string Transshipment2FromPortName { get; set; }
        public string Transshipment2FromPortCountryCode { get; set; }
        public string Transshipment2FromPortCountryName { get; set; }
        public string Transshipment2ToPortCode { get; set; }
        public string Transshipment2ToPortName { get; set; }
        public string Transshipment2ToPortCountryCode { get; set; }
        public string Transshipment2ToPortCountryName { get; set; }

        public string Transshipment3FromPortId { get; set; }
        public string Transshipment3ToPortId { get; set; }
        public DateTime? Transshipment3ATD { get; set; }
        public DateTime? Transshipment3ATA { get; set; }
        public DateTime? Transshipment3ETD { get; set; }
        public DateTime? Transshipment3ETA { get; set; }
        public string Transshipment3CarrierNumber { get; set; }
        public string Transshipment3CarrierId { get; set; }
        public string Transshipment3CarrierName { get; set; }
        public string Transshipment3CarrierCode { get; set; }
        public string Transshipment3FromPortCode { get; set; }
        public string Transshipment3FromPortName { get; set; }
        public string Transshipment3FromPortCountryCode { get; set; }
        public string Transshipment3FromPortCountryName { get; set; }
        public string Transshipment3ToPortCode { get; set; }
        public string Transshipment3ToPortName { get; set; }
        public string Transshipment3ToPortCountryCode { get; set; }
        public string Transshipment3ToPortCountryName { get; set; }

        public bool Transshipment1ToPortCountryEC { get; set; }
        public bool Transshipment2ToPortCountryEC { get; set; }
        public bool Transshipment3ToPortCountryEC { get; set; }
        public string FinalDistenationPortId { get; set; }

        public string Transshipment1VesselId { get; set; }
        public string Transshipment2VesselId { get; set; }
        public string Transshipment3VesselId { get; set; }
        public string Transshipment1AdditionalMAWBOBLBL { get; set; }
        public string Transshipment2AdditionalMAWBOBLBL { get; set; }
        public string Transshipment3AdditionalMAWBOBLBL { get; set; }

        public string FromPort { get; set; }
        public string FromPortName { get; set; }
        public string FromPortCountry { get; set; }
        public string FromPortCountryName { get; set; }

        public string ToPort { get; set; }
        public string ToPortName { get; set; }
        public string ToPortCountry { get; set; }
        public string ToPortCountryName { get; set; }
        #endregion

        #region Booking
        public double? OrderGrossWeight { get; set; }
        public double? BookingVolume { get; set; }
        public int? BookingNumberOfPackages { get; set; }
        public bool OrderIsDangerouseGoods { get; set; }
        public string BookingConfirmationNumber { get; set; }
        public string BookingConfirmedBy { get; set; }
        public string BookingConfirmationNotes { get; set; }
        public double? OrderVolumetricWeight { get; set; }
        public double? OrderChargeableWeight { get; set; }
        public DateTime? CutoffDate { get; set; }
        #endregion

        #region AWB
        public bool AWBPrint { get; set; }
        public string FWBStatusCode { get; set; }
        public string FWBStatusName { get; set; }
        public string FHLStatusCode { get; set; }
        public string FHLStatusName { get; set; }
        public string AWBCurrencyId { get; set; }
        public string AWBCurrencyCode { get; set; }
        public double? AWBFreightAmountPrepaid { get; set; }
        public double? AWBFreightAmountCollect { get; set; }
        public string AWBCarrierTarrifReference { get; set; }
        public string AWBDeclaredValueForCarriage { get; set; }
        public string AWBDeclaredValueForCustoms { get; set; }
        public string AWBAccountingInformation { get; set; }
        public string AWBInsurrenceValue { get; set; }
        public string AWBHandlingInformation { get; set; }
        public string SCI { get; set; }
        public string AWBComments { get; set; }
        public string AWBSignature { get; set; }
        public string AWBPlace { get; set; }
        public string AWBChargesCodeCode { get; set; }
        #endregion

        private List<ShipmentReceivableDTO> shipmentReceivables;
        public virtual List<ShipmentReceivableDTO> ShipmentReceivables
        {
            get
            {

                if (this.shipmentReceivables == null)
                {
                    shipmentReceivables = new List<ShipmentReceivableDTO>();
                }
                return this.shipmentReceivables;
            }
            set
            {
                if (value != null)
                {
                    shipmentReceivables = value;
                }
            }
        }
    }
}
