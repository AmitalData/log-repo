using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using System.Runtime.Serialization;

using Logitude.Server.Tools;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

using Newtonsoft.Json;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    [CustomValidation(typeof(Validators.ShipmentValidator), "IsShipmentValid")]
    public partial class ShipmentPM : EntityPM
    {
 
        #region Fields

        public VerticalTimeLineData TimeLineData { get; set; }


        #endregion

 
        //public bool NoFreightFile { get; set; }

        //public bool IsExceptionResolved { get; set; }
        //public bool IsRefreshFollowUp { get; set; }


        #region Booking
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? OrderGrossWeight { get; set; }
        public double? BookingVolume { get; set; }

        public bool OrderGrossWeightEdited { get; set; }
        public bool OrderChargeableWeightEdited { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public int? BookingNumberOfPackages { get; set; }
        public bool OrderIsDangerouseGoods { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string BookingConfirmationNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string BookingConfirmedBy { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string BookingConfirmationNotes { get; set; }
        public double? OrderVolumetricWeight { get; set; }
        public double? OrderChargeableWeight { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? CutoffDate { get; set; }
        #endregion

        #region AWB

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool AWBPrint { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string FWBStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string FWBStatusName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string FHLStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string FHLStatusName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBCurrencyId { get; set; }
        public string AWBCurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? AWBFreightAmountPrepaid { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? AWBFreightAmountCollect { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBCarrierTarrifReference { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBDeclaredValueForCarriage { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBDeclaredValueForCustoms { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBAccountingInformation { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBInsurrenceValue { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBHandlingInformation { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string SCI { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBComments { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBPrintingComments { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBSignature { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBPlace { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBChargesCodeCode { get; set; }
        #endregion

        #region FWB CCS Dummy fields
        public string TenantZeroAirlineId { get; set; }
        public string TenantZeroAirlineTTY { get; set; }
        public string TenantZeroAirlinePIMA { get; set; }
        public bool TenantZeroAirlineChampFWB { get; set; }
        public bool TenantZeroAirlineChampFHL { get; set; }
        public bool TenantZeroAirlineChampFSU { get; set; }
        public bool TenantZeroAirlineChampFSRFSA { get; set; }
        public bool TenantZeroAirlineChampFVRFVA { get; set; }
        public bool CarrierIsChampRegistered { get; set; }
        public bool TenantZeroAirlineChampNeedsRegistration { get; set; }
        public bool TenantZeroAirlineGLSHKFWB { get; set; }
        public bool TenantZeroAirlineGLSHKFHL { get; set; }
        public bool TenantZeroAirlineGLSHKFSU { get; set; }
        public bool TenantZeroAirlineGLSHKFSRFSA { get; set; }
        public bool TenantZeroAirlineGLSHKFVRFVA { get; set; }
        public bool CarrierIsGLSHKRegistered { get; set; }
        public bool TenantZeroAirlineGLSHKNeedsRegistration { get; set; }
        public bool CarrierIsCheckDigit { get; set; }
        public bool CarrierIsLimitedLength { get; set; }
        #endregion

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string LocalCustomsTransmissionsStatusCode { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string LocalCustomsTransmissionsStatusName { get; set; }

        //public string LocalCustomsTransmissionsStatusError { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public DateTime? LocalCustomsTransmissionsStatusDate { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string LocalCustomsSentByUserId { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OperationalClosedByUserId { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string LocalCustomsSentByUserName { get; set; }

        //public bool IncludesCustoms { get; set; }
        //public bool IsUpdateByAutomation { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string DeclarationNumber { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public DateTime? DeclarationDate { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public DateTime? CustomsClearanceDate { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string MasterShipmentNumber { get; set; }


        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string ProductCode { get; set; }

        //public string SecurityKey { get; set; }
        //public double? TEU { get; set; }

        //public bool DontCreateConvertEvent { get; set; }
        //public bool ConvertFromHouseToDirect { get; set; }
        //public bool ConvertFromDirectToHouse { get; set; }
        //public bool IsRefreshShipmentFollowUps { get; set; }
        //public string CustomerRankName { get; set; }
        //public DateTime? FinalArrivalDate { get; set; }
        //public DateTime? EstimatedFinalArrivalDate { get; set; }
        //public DateTime? ActualFinalArrivalDate { get; set; }
        //public string ForeignPartnerCountryCode { get; set; }
        //public DateTime? LastStatusLogDate { get; set; }
        //public string MainCarriageFullCarrierNumber { get; set; }
        //public string Transshipment1FullCarrierNumber { get; set; }
        //public string Transshipment2FullCarrierNumber { get; set; }
        //public string Transshipment3FullCarrierNumber { get; set; }

        //public string ExceptionDescription { get; set; }
        //public string ExceptionResolvedDescription { get; set; }
        //public string LastExceptionDescription { get; set; }

        //public DateTime? ExceptionDate { get; set; }
        //public bool HasException { get; set; }
        //public string HasExceptionMessage { get; set; }

        //public bool IsManifestSentToAgent { get; set; }
        //public string AgentSharedManifestRef { get; set; }


        //public string FromLocation { get; set; }
        //public string ToLocation { get; set; }

        //public string MoveTypeId { get; set; }
        //public string MoveTypeCode { get; set; }
        //public string MoveTypeName { get; set; }

        //public bool IsCreatedFromAgentSharedManifest { get; set; }
        //public bool IsDocumentsNeedApprove { get; set; }

        //public string AMSBL { get; set; }
        //public string CustomFileId { get; set; }
        //public string CustomFileNumber { get; set; }
        //public DateTime? MainCarriageSTD { get; set; }
        //public DateTime? MainCarriageSTA { get; set; }
        //public DateTime? Transshipment1STD { get; set; }
        //public DateTime? Transshipment1STA { get; set; }
        //public DateTime? Transshipment2STD { get; set; }
        //public DateTime? Transshipment2STA { get; set; }
        //public DateTime? Transshipment3STD { get; set; }
        //public DateTime? Transshipment3STA { get; set; }

        //public bool IsMultipleCommodities { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string NominatedHandlingPartyId { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantIdCode1 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantIdCode2 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantIdCode3 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantInformationCode1 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantInformationCode2 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantInformationCode3 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantInformationPortCode1 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantInformationPortCode2 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantInformationPortCode3 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantInformationName1 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantInformationName2 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantInformationName3 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantInformationReference1 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantInformationReference2 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OtherParticipantInformationReference3 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string AccountingInformation1 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string AccountingInformation2 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string AccountingInformation3 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string AccountingInformation4 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string AccountingInformation5 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string AccountingInformation6 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string AccountingInformationIdentifierCode1 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string AccountingInformationIdentifierCode2 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string AccountingInformationIdentifierCode3 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string AccountingInformationIdentifierCode4 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string AccountingInformationIdentifierCode5 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string AccountingInformationIdentifierCode6 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string ReferenceNumber { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string SupplementaryShipmentInformation1 { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string SupplementaryShipmentInformation2 { get; set; }

        //public string LastSentByUserId { get; set; }

        //public double? ValueOfGoods { get; set; }
        //public string ValueOfGoodsCurrencyId { get; set; }
        //public DateTime? OperationalDate { get; set; }

        //public bool FBLIsFromStock { get; set; }
        //public bool FBLReturnedToStock { get; set; }
        //public bool FBLTakenFromStock { get; set; }
        //public string FBLStockNumber { get; set; }
        //public bool FBLReturnedToStockWithCancel { get; set; }


        //public string Transshipment3ToPortStateCode { get; set; }
        //public string Transshipment2ToPortStateCode { get; set; }
        //public string Transshipment1ToPortStateCode { get; set; }

        public string StandaloneShipmentId { get; set; }
        public bool IsConnectToMasterShipment { get; set; }


        //private List<ShipmentFollowUpPM> followUps;
        [Include]
        [Composition]
        [Association("FollowUpShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentFollowUpPM> FollowUps
        {
            get
            {
                if (followUps == null)
                {
                    followUps = new List<ShipmentFollowUpPM>();
                }

                return this.followUps;
            }
            set
            {
                if (value != null)
                {
                    followUps = value;
                }
            }
        }

        //private List<ShipmentReceivablePM> shipmentReceivables;
        [Include]
        [Association("ShipmentReceivableShipment", "Id", "ShipmentId")]
        [Composition]
        public virtual List<ShipmentReceivablePM> ShipmentReceivables
        {
            get
            {

                if (this.shipmentReceivables == null)
                {
                    shipmentReceivables = new List<ShipmentReceivablePM>();
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

        private List<ShipmentPayablePM> shipmentPayables;
        [Include]
        [Association("ShipmentPayableShipment", "Id", "ShipmentId")]
        [Composition]
        public virtual List<ShipmentPayablePM> ShipmentPayables
        {
            get
            {

                if (this.shipmentPayables == null)
                {
                    shipmentPayables = new List<ShipmentPayablePM>();
                }
                return this.shipmentPayables;
            }
            set
            {
                if (value != null)
                {
                    shipmentPayables = value;
                }
            }
        }

        //private List<ShipmentPackagePM> shipmentPackages;
        [Include]
        [DataMember]
        [Composition]
        [Association("ShipmentPackagePMShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentPackagePM> ShipmentPackages
        {
            get
            {
                if (shipmentPackages == null)
                {
                    shipmentPackages = new List<ShipmentPackagePM>();
                }

                return this.shipmentPackages;
            }

            set
            {
                if (value != null)
                {
                    shipmentPackages = value;
                }
            }
        }

        private List<ShipmentPackagePM> connectedMasterPackages;
        [Include]
        [DataMember]
        [Composition]
        [Association("MasterPackagePMShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentPackagePM> ConnectedMasterPackages
        {
            get
            {
                if (connectedMasterPackages == null)
                {
                    connectedMasterPackages = new List<ShipmentPackagePM>();
                }

                return this.connectedMasterPackages;
            }

            set
            {
                if (value != null)
                {
                    connectedMasterPackages = value;
                }
            }
        }

        //private List<ShipmentOrderPackagePM> shipmentOrderPackages;
        [Include]
        [Composition]
        [Association("ShipmentOrderPackagePMShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentOrderPackagePM> ShipmentOrderPackages
        {
            get
            {
                if (shipmentOrderPackages == null)
                {
                    shipmentOrderPackages = new List<ShipmentOrderPackagePM>();
                }

                return this.shipmentOrderPackages;
            }

            set
            {
                if (value != null)
                {
                    shipmentOrderPackages = value;
                }
            }
        }

        //private List<ShipmentPickUpPM> shipmentPickUps;
        [Include]
        [Composition]
        [DataMember]
        [Association("ShipmentPickUpPMShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentPickUpPM> ShipmentPickUps
        {
            get
            {
                if (shipmentPickUps == null)
                {
                    shipmentPickUps = new List<ShipmentPickUpPM>();
                }

                return this.shipmentPickUps;
            }
            set
            {
                if (value != null)
                {
                    shipmentPickUps = value;
                }
            }
        }

        //private List<ShipmentDeliveryPM> shipmentDeliveries;
        [Include]
        [Composition]
        [Association("ShipmentDeliveryPMShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentDeliveryPM> ShipmentDeliveries
        {
            get
            {
                if (shipmentDeliveries == null)
                {
                    shipmentDeliveries = new List<ShipmentDeliveryPM>();
                }

                return this.shipmentDeliveries;
            }
            set
            {
                if (value != null)
                {
                    shipmentDeliveries = value;
                }
            }
        }

        private List<ShipmentARInvoicePM> shipmentArInvoices;
        [Include]
        [Composition]
        [Association("ShipmentARInvoicePMShipment", "Id", "ShipmentId")]
        public List<ShipmentARInvoicePM> ShipmentARInvoices
        {
            get
            {
                if (shipmentArInvoices == null) { shipmentArInvoices = new List<ShipmentARInvoicePM>(); }
                return shipmentArInvoices;
            }

            set
            {
                if (value != null) { shipmentArInvoices = value; }
            }
        }

        private List<ShipmentAPInvoicePM> shipmentApInvoices;
        [Include]
        [Composition]
        [Association("ShipmentAPInvoicePMShipment", "Id", "ShipmentId")]
        public List<ShipmentAPInvoicePM> ShipmentAPInvoices
        {
            get
            {
                if (shipmentApInvoices == null) { shipmentApInvoices = new List<ShipmentAPInvoicePM>(); }
                return shipmentApInvoices;
            }

            set
            {
                if (value != null) { shipmentApInvoices = value; }
            }
        }

        private List<ConsoleShipmentPM> shipmentConsoleShipments;
        [Include]
        [Composition]
        [Association("ConsoleShipmentPMShipment", "Id", "MasterShipmentDataId")]
        public List<ConsoleShipmentPM> ShipmentConsoleShipments
        {
            get
            {
                if (shipmentConsoleShipments == null) { shipmentConsoleShipments = new List<ConsoleShipmentPM>(); }
                return shipmentConsoleShipments;
            }

            set
            {
                if (value != null) { shipmentConsoleShipments = value; }
            }
        }

        private List<ShipmentAWBPrintOnlyPM> shipmentAWBPrintOnlies;
        [Include]
        [Association("ShipmentAWBPrintOnlyPMShipment", "Id", "ShipmentId")]
        [Composition]
        public virtual List<ShipmentAWBPrintOnlyPM> ShipmentAWBPrintOnlies
        {
            get
            {

                if (this.shipmentAWBPrintOnlies == null)
                {
                    shipmentAWBPrintOnlies = new List<ShipmentAWBPrintOnlyPM>();
                }
                return this.shipmentAWBPrintOnlies;
            }
            set
            {
                if (value != null)
                {
                    shipmentAWBPrintOnlies = value;
                }
            }
        }

        private List<ShipmentCarrierStatusPM> shipmentCarrierStatuses;
        [Include]
        [Composition]
        [Association("ShipmentShipmentCarrierStatuses", "Id", "ShipmentId")]
        public List<ShipmentCarrierStatusPM> ShipmentCarrierStatuses
        {
            get
            {
                if (shipmentCarrierStatuses == null) { shipmentCarrierStatuses = new List<ShipmentCarrierStatusPM>(); }
                return shipmentCarrierStatuses;
            }

            set
            {
                if (value != null) { shipmentCarrierStatuses = value; }
            }
        }

        private List<AWBOCIPM> aWBOCIPMs;
        [Include]
        [Composition]
        [Association("ShipmentAWBOCI", "Id", "ShipmentId")]
        public List<AWBOCIPM> AWBOCIPMs
        {
            get
            {
                if (aWBOCIPMs == null) { aWBOCIPMs = new List<AWBOCIPM>(); }
                return aWBOCIPMs;
            }

            set
            {
                if (value != null) { aWBOCIPMs = value; }
            }
        }


        private List<ShipmentCommodityPM> shipmentCommodities;
        [Include]
        [Composition]
        [Association("ShipmentCommodityPMShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentCommodityPM> ShipmentCommodities
        {
            get
            {
                if (shipmentCommodities == null)
                {
                    shipmentCommodities = new List<ShipmentCommodityPM>();
                }

                return this.shipmentCommodities;
            }

            set
            {
                if (value != null)
                {
                    shipmentCommodities = value;
                }
            }
        }

        private List<ShipmentAssemblyPM> shipmentAssemblies;
        [Include]
        [Association("ShipmentAssemblyShipment", "Id", "ShipmentId")]
        [Composition]
        public virtual List<ShipmentAssemblyPM> ShipmentAssemblies
        {
            get
            {

                if (this.shipmentAssemblies == null)
                {
                    shipmentAssemblies = new List<ShipmentAssemblyPM>();
                }
                return this.shipmentAssemblies;
            }
            set
            {
                if (value != null)
                {
                    shipmentAssemblies = value;
                }
            }
        }


        private List<DocumentsFilingPM> missingDocuments;
        public virtual List<DocumentsFilingPM> MissingDocuments
        {
            get
            {
                if (missingDocuments == null)
                {
                    missingDocuments = new List<DocumentsFilingPM>();
                }

                return this.missingDocuments;
            }

            set
            {
                if (value != null)
                {
                    missingDocuments = value;
                }
            }
        }

        private List<DocumentsFilingPM> receivedDocuments;
        public virtual List<DocumentsFilingPM> ReceivedDocuments
        {
            get
            {
                if (receivedDocuments == null)
                {
                    receivedDocuments = new List<DocumentsFilingPM>();
                }

                return this.receivedDocuments;
            }

            set
            {
                if (value != null)
                {
                    receivedDocuments = value;
                }
            }
        }

        private List<DocumentsFilingPM> requiredDocuments;
        public virtual List<DocumentsFilingPM> RequiredDocuments
        {
            get
            {
                if (requiredDocuments == null)
                {
                    requiredDocuments = new List<DocumentsFilingPM>();
                }

                return this.requiredDocuments;
            }

            set
            {
                if (value != null)
                {
                    requiredDocuments = value;
                }
            }
        }

        public string MainCarriageFromPartnerId { get; set; }
        public string MainCarriageFromAddressId { get; set; }
        public string MainCarriageToPartnerId { get; set; }
        public string MainCarriageToAddressId { get; set; }
        public string Driver { get; set; }
        public string TruckNumber { get; set; }
        public string TrailerNumber { get; set; }
        public string Transshipment1TrailerNumber { get; set; }
        public string Transshipment2TrailerNumber { get; set; }
        public string Transshipment3TrailerNumber { get; set; }
        public bool AsAgreedFreight { get; set; }
        public bool AsAgreedOtherCharges { get; set; }
        public bool ARInvoiceIssued { get; set; }
        public bool CreditNoteIssued { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AccountNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string CargonautFHLStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string CargonautFHLStatusName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? CargonautFHLStatusDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string CargonautFWBStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string CargonautFWBStatusName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? CargonautFWBStatusDate { get; set; }

        // Dummy
        public bool MarkFollowUpsAsDone { get; set; }
        public bool CalculateProfit { get; set; }
        public bool CalculateStatus { get; set; }
        public string ComputedStatusId { get; set; }
        public DateTime? ComputedStatusDate { get; set; }
        public string ComputedStatusName { get; set; }
        public string CustomFilePocoId { get; set; }
        public DateTime? ManifestLastSharingDate { get; set; }
        public bool CalculatePayables { get; set; }
        public bool CalculateReceivables { get; set; }
        public bool IsAddingStackEvents { get; set; }
        public bool IsRemovingStackEvents { get; set; }
        public string StackAirlineId { get; set; }
        public string FromPartnerCity { get; set; }
        public string FromPartnerCountryCode { get; set; }
        public string FromPartnerCountryName { get; set; }
        public string ToPartnerCity { get; set; }
        public string ToPartnerCountryCode { get; set; }
        public string ToPartnerCountryName { get; set; }
        public bool IsSendFSRCreatingShipment { get; set; }
        public string ToCountryId { get; set; }
        public string FromCountryId { get; set; }
        public string CopyFromShipmentId { get; set; }
        public bool IsCopyFromShipment { get; set; }
        public bool IsBuildFromQuote { get; set; }
        public bool IsBuildFromBooking { get; set; }
        public string ShipperMainAddressId { get; set; }
        public string ShipperPickAddressId { get; set; }
        public string ConsigneeMainAddressId { get; set; }
        public string ConsigneePickAddressId { get; set; }

        public bool IncludePickUp { get; set; }
        public string FromAddressCity { get; set; }
        public string FromAddressZipCode { get; set; }
        public string FromAddressCountryId { get; set; }
        public string PickUpAddressId { get; set; }
        public bool CustomConnectToShipment { get; set; }
        public bool IncludeDelivery { get; set; }
        public string ToAddressCity { get; set; }
        public string ToAddressZipCode { get; set; }
        public string ToAddressCountryId { get; set; }
        public string DeliveryAddressId { get; set; }
        public string SpecialServicesTypeId { get; set; }
        public string SpecialServicesTypeName { get; set; }

        public string ContainerNumber1 { get; set; }
        public string ContainerNumber2 { get; set; }
        public string ContainerNumber3 { get; set; }
        public string ContainerNumber4 { get; set; }
        public string ContainerNumber5 { get; set; }

        #region Dummy fields needed for the Build from Quote screen
        public int? Quantity1 { get; set; }
        public int? Quantity2 { get; set; }
        public int? Quantity3 { get; set; }
        public int? Quantity4 { get; set; }
        public int? Quantity5 { get; set; }
         
        public string PackageTypeId1 { get; set; }
        public string PackageTypeId2 { get; set; }
        public string PackageTypeId3 { get; set; }
        public string PackageTypeId4 { get; set; }
        public string PackageTypeId5 { get; set; }

        public double? Weight1 { get; set; }
        public double? Weight2 { get; set; }
        public double? Weight3 { get; set; }
        public double? Weight4 { get; set; }
        #endregion
        public string NewConcurrencyGUID { get; set; }

        public int ConnectedShipmentsPayablesCount { get; set; }
        public int ConnectedShipmentsReceivablesCount { get; set; }
        public int NumberOfInsidePackages { get; set; }
        public string NumberOfInsidePackagesDetails { get; set; }

        public string AccountManagerUserId { get; set; }
        public string AccountManagerUserName { get; set; }
        public string AccountManagerUserEmail { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ManifestReason { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ManifestStatusCode { get; set; }

        public bool IsKnownCargo { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string RegulatedAgentRANumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string KnownConsignorNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? KCExpirationDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ColoaderRANumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBPrintingSecurityStatusId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AWBPrintingRANumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AdditionalHandlingInfo { get; set; }

        public bool AWBPrintingSecurityStatusEdited { get; set; }
        public bool AWBPrintingRANumberEdited { get; set; }
        public bool AdditionalHandlingInfoEdited { get; set; }

        public bool ViaColoader { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string IssuingCarrierReference1 { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool IsMissingDocument { get; set; }
        public string DocumentsSearchFields { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string InterlineId { get; set; }

        public string ShipperAddress1 { get; set; }
        public string ShipperAddress2 { get; set; }
        public string ShipperZipCode { get; set; }
        public string ShipperStateId { get; set; }
        public string ShipperCountryId { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperPhoneNumber { get; set; }
        public string ShipperFaxNumber { get; set; }

        public string ConsigneeAddress1 { get; set; }
        public string ConsigneeAddress2 { get; set; }
        public string ConsigneeZipCode { get; set; }
        public string ConsigneeStateId { get; set; }
        public string ConsigneeCountryId { get; set; }
        public string ConsigneeCity { get; set; }
        public string ConsigneePhoneNumber { get; set; }
        public string ConsigneeFaxNumber { get; set; }

        public string Notify1Address1 { get; set; }
        public string Notify1Address2 { get; set; }
        public string Notify1ZipCode { get; set; }
        public string Notify1StateId { get; set; }
        public string Notify1CountryId { get; set; }
        public string Notify1City { get; set; }
        public string Notify1PhoneNumber { get; set; }
        public string Notify1FaxNumber { get; set; }

        public string IssuingCarrierCity { get; set; }
        public string Notify1AddressCountryCode { get; set; }

        public bool MAWBReturnedToStackWithCancel { get; set; }

        public string MAWBStackAirlineId { get; set; }

        public bool DontAddToImportersQueue { get; set; }

        public bool DontAddToForwarderQueue { get; set; }

        public string ForwarderPartnerId { get; set; }

        public DateTime? OperationalCloseDate { get; set; }
        public DateTime? AccountingCloseDate { get; set; }
        public string FromCountryCode { get; set; }
        public string ToCountryCode { get; set; }

        //islam: for testing the importers data mapping
        public bool ConvertToCustomFile { get; set; }

        public int? CustomerTenantNumber { get; set; }

        public DateTime? MainCarriageFinalDestinationETA { get; set; }
        public DateTime? MainCarriageFinalDestinationATA { get; set; }
        public DateTime? DepartureArrivalFromDate { get; set; }
        public DateTime? DepartureArrivalToDate { get; set; }

        public string FirstPickupLocation { get; set; }
        public string FinalDeliveryLocation { get; set; }
        public bool IsImporterShipment { get; set; }
        public bool IsUpdatedByChampAnalyzer { get; set; }
        public bool IsUpdatedByGLSHKAnalyzer { get; set; }
        public bool IsUpdatedByINTTRAAnalyzer { get; set; }
        public bool IsUpdatedOceanInsightsAnalyzer { get; set; }
        public bool IsUpdatedVizionAnalyzer { get; set; }
        public bool IsUpdatedOceanInsightsMainCarriageDates { get; set; }
        public bool IsUpdatedVizionMainCarriageDates { get; set; }
        public bool ShipmentUpdatedFromContainer { get; set; }
        public bool IsCreatedFromCustomerOverview { get; set; }
        public string DeclarationXMLData { get; set; }
        public DateTime? DocumentInspection { get; set; }
        public DateTime? GatepassDocumentsReady { get; set; }
        public DateTime? GoodsClassification { get; set; }
        public DateTime? InvoiceIssuedDate { get; set; }
        public bool IsImporterApprovalRequired { get; set; }
        public bool SendUpdatesToAgentEnabled { get; set; }
        public bool UpdateSendUpdatesToAgentEnabledField { get; set; }
        public bool DocsSentToAgent { get; set; }
        public string VersionApproved { get; set; }
        public string ApprovedBy { get; set; }
        public string DocumentsApprovedByUserName { get; set; }
        public DateTime? ApproveDateTime { get; set; }
        public bool IsNewARInvoiceBlocked { get; set; }
        public string ShipmentAddtionalDataXML { get; set; }
        public ShipmentAdditionalData ShipmentAdditionalData { get; set; }
        //public bool IsMappingXSDFields { get; set; }
        //public bool StopConcurrencyValidating { get; set; }
        public string OriginShipmentId { get; set; }
        public bool ProrateReceivables { get; set; }
        public bool ReleasesPackagesAddedOnTheShipment { get; set; }


        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? FreightRelease { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? TerminalAvailable { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? Terminal2Available { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ISFNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? ISFDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ITNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? ITDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? DocumentsClosingDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string OBLTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ENSNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? ENSDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string TruckerId { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string TruckerName { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string TruckerNote { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string TruckerAddressId { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string TruckerContactId { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string TruckerReference1 { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string TruckerReference2 { get; set; }
        

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? AssignedToTruckerDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? AssginedToCustomsAgentDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? PaymentRequestDateTime { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ReferantUserId { get; set; }
        public string IskaNumber { get; set; }

        #region WarehouseLeg
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
		[DataMember]
		public string WarehouseLegWarehouseName { get; set; }
		[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLegWarehouseId { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLegAddressId { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLegTerminalCode { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegExpectedEntryDate { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegActualEntryDate { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegExpectedReleaseDate { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegActualReleaseDate { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegLastFreeDate { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLegRemarks { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLegAddressName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLegAddressAddress1 { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLegAddressAddress2 { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLegAddressCity { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLegAddressCountryName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLegAddressCountryCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLegAddressPhoneNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLegAddressFaxNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLegReference { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        [DataMember]
        public string WarehouseLegTerminalName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegEntryDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegReleaseDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegVGMCutOffDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegCutOffDate { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLeg2WarehouseId { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLeg2AddressId { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLeg2TerminalCode { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLeg2ExpectedEntryDate { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLeg2ActualEntryDate { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLeg2ExpectedReleaseDate { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLeg2ActualReleaseDate { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLeg2Remarks { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLeg2AddressName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLeg2AddressAddress1 { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLeg2AddressAddress2 { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLeg2AddressCity { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLeg2AddressCountryName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLeg2AddressCountryCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLeg2AddressPhoneNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLeg2AddressFaxNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WarehouseLeg2Reference { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        [DataMember]
        public string WarehouseLeg2TerminalName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLeg2EntryDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLeg2ReleaseDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLeg2VGMCutOffDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? WarehouseLeg2CutOffDate { get; set; }
        #endregion 

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? RegistryDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool IsAssembly { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string LastSharedEventId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string LastSharedEventLocation { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string LastSharedEventNotes { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? LastSharedEventDate { get; set; }

        public string LastSharedEventName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? GrossWeightPerTon { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        [DataMember]
        public double? GrossWeightPerStorageDays { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? FirstOperationalCloseDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? FirstAccountingCloseDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string OldStatusValue { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? AMSClosingDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string UpdatedByPartner { get; set; }

        public string SplitFromShipmentNo { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string INTTRASIStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string INTTRASIStatusName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? INTTRASIStatusDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string INTTRASIError { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string EmergencyContactId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string INTTRAContractNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string INTTRAInstructions { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string INTTRAComments { get; set; }

        public int? INTTRADocumentQTY { get; set; }
        public bool SIHasAttachList { get; set; }
        public bool INTTRAIsFreighted { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string INTTRADocumentTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string INTTRABookingTransStatusCode { get; set; }
        public string INTTRABookingTransStatusName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string INTTRABookingStatusCode { get; set; }
        public string INTTRABookingStatusName { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? INTTRALastEBbookingSendDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string INTTRABookingError { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string INTTRALastBookingResponse { get; set; }

        public string INTTRABookingResponse_Voyage { get; set; }
        public DateTime INTTRABookingResponse_POLDate { get; set; }
        public string INTTRABookingResponse_POFPort { get; set; }
        public string INTTRABookingResponse_POFPortCode { get; set; }
        public string INTTRABookingResponse_POFCCode { get; set; }
        public string INTTRABookingResponse_POFCName { get; set; }
        public DateTime INTTRABookingResponse_PODDate { get; set; }
        public string INTTRABookingResponse_PODPort { get; set; }
        public string INTTRABookingResponse_PODPortCode { get; set; }
        public string INTTRABookingResponse_PODCCode { get; set; }
        public string INTTRABookingResponse_PODCName { get; set; }
        public string INTTRABookingResponse_ShippingLine { get; set; }
        public string INTTRABookingResponse_Vessel { get; set; }
        public string INTTRABookingResponse_VesselId { get; set; }


        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string LastFinalDestination { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string From { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string To { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string Origin { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? FirstPickupETD { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? FirstPickupETA { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? INTTRALastStatusDate { get; set; }

        public bool IsPaymentRequired { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string PaymentRequestXML { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? PaymentDateTime { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string Notify1Reference { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string Notify1Reference2 { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string Notify2Reference { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ShipperNotExporterReference { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ConsigneeNotImporterReference { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ForwardingPartnerId { get; set; }

        public bool IsSharedLogisticsMoneyTabEnabled { get; set; }
        public bool IsSharedLogisticsMainCarrierVisible { get; set; }
        public bool IsSharedLogisticsPickDelvCarrierVisible { get; set; }
        public bool IsSharedLogisticsAgentVisible { get; set; }
        public bool IsSharedLogisticsShipperVisible { get; set; }
        public bool IsSharedLogisticsConsigneeVisible { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? FirstPickupATA { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? FirstPickupATD { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? FinalDeliveryETA { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? FinalDeliveryETD { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? FinalDeliveryATA { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? FinalDeliveryATD { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string DeclarationWCOXml { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ProjectNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? ContainerLastStatusDate { get; set; }

        public bool ShipmentContanisDangerousGoods { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string BasicFreightId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string DestinationPortChargesId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string DestinationHaulageChargesId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AdditionalChargesId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string FreightPayerId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string FreightPayerAddressId { get; set; }

        public bool HasContainerException { get; set; }

        public DateTime? FirstARInvoiceApprovalDate { get; set; }


        //ShipmentComputedFields
        public bool IsMissingDocuments { get; set; }
        public DateTime? LastDocumentDateTime { get; set; }
        public int MissingDocumentsCount { get; set; }
        public string MissingDocumentsNames { get; set; }
        public bool IsRequestedDocuments { get; set; }
        public int RequestedDocumentsCount { get; set; }
        public int NumberOfHouses { get; set; }
        public bool IsDigitalSignRequired { get; set; }
        public bool IsDepositionRequired { get; set; }
        public string ImporterDepositionRequestDetails { get; set; }
        public bool IsShipmentComputedFieldChange { get; set; }
        public bool IsShipmentAdditionalCloudDataChange { get; set; }
        public bool IsStatusChange { get; set; }
        public bool IsOperationalStatusChange { get; set; }

        public string PackagesTypesNames { get; set; }
        public string PackagesTypesPrintAs { get; set; }
        public string ContainersNumbers { get; set; }
        public string ARInvoices { get; set; }
        public bool ConvertShipmentToLCL { get; set; }
        public bool ConvertShipmentToFCL { get; set; }

        public bool ConvertShipmentToLTL { get; set; }
        public bool ConvertShipmentToFTL { get; set; }
        public string HousesNumbers { get; set; }

        public bool ShipmentDirectionConverted { get; set; }
        public bool ShipmentConvertedNewNumber { get; set; }
        public string OldShipmentNumber { get; set; }
        public bool FromCountryIsEC { get; set; }
        public bool ToCountryIsEC { get; set; }
        public bool Transshipment1ToCountryIsEC { get; set; }
        public bool Transshipment2ToCountryIsEC { get; set; }
        public bool Transshipment3ToCountryIsEC { get; set; }
        public bool PackagesDeleted { get; set; }
        public string MasterCreatedFromHouseId { get; set; }
        public bool IsDeletingAllPayables { get; set; }
        public double? NotInvoicedReceivablesAmount { get; set; }
        public string CreatedByPartner { get; set; }

        // Fields of Champ analyzer Concurrency
        public string FWBStatusCode_Original { get; set; }
        public DateTime? FWBStatusDate_Original { get; set; }
        public string FHLStatusCode_Original { get; set; }
        public DateTime? FHLStatusDate_Original { get; set; }
        public string CarrierLastStatusCode_Original { get; set; }
        public DateTime? CarrierLastStatusDate_Original { get; set; }
        public string MainCarriageToPortId_Original { get; set; }
        public int? NumberOfPackages_Original { get; set; }
        public double? GrossWeight_Original { get; set; }
        public double? ChargeableWeight_Original { get; set; }
        public string GrossWeightUnitCode_Original { get; set; }
        public DateTime? MainCarriageATD_Original { get; set; }
        public DateTime? MainCarriageETD_Original { get; set; }
        public DateTime? MainCarriageSTD_Original { get; set; }
        public DateTime? Transshipment1ATD_Original { get; set; }
        public DateTime? Transshipment1ETD_Original { get; set; }
        public DateTime? Transshipment1STD_Original { get; set; }
        public DateTime? Transshipment2ATD_Original { get; set; }
        public DateTime? Transshipment2ETD_Original { get; set; }
        public DateTime? Transshipment2STD_Original { get; set; }
        public DateTime? Transshipment3ATD_Original { get; set; }
        public DateTime? Transshipment3ETD_Original { get; set; }
        public DateTime? Transshipment3STD_Original { get; set; }
        public DateTime? MainCarriageATA_Original { get; set; }
        public DateTime? MainCarriageETA_Original { get; set; }
        public DateTime? MainCarriageSTA_Original { get; set; }
        public DateTime? Transshipment1ATA_Original { get; set; }
        public DateTime? Transshipment1ETA_Original { get; set; }
        public DateTime? Transshipment1STA_Original { get; set; }
        public DateTime? Transshipment2ATA_Original { get; set; }
        public DateTime? Transshipment2ETA_Original { get; set; }
        public DateTime? Transshipment2STA_Original { get; set; }
        public DateTime? Transshipment3ATA_Original { get; set; }
        public DateTime? Transshipment3ETA_Original { get; set; }
        public DateTime? Transshipment3STA_Original { get; set; }
        public string INTTRABookingStatusCode_Original { get; set; }
        public string BookingConfirmedBy_Original { get; set; }
        public string MAN_FromPortId_Original { get; set; }
        public string FIN_PortId_Original { get; set; }
        public string TR3_ToPortId_Original { get; set; }
        public string TR2_ToPortId_Original { get; set; }
        public string TR1_ToPortId_Original { get; set; }
        public string BookingConfNumber_Original { get; set; }
        public string MAN_CarrierNumber_Original { get; set; }

        public bool IsUserIDNumberRequired { get; set; }
        public DateTime? UserIdNumberUpdateDate { get; set; }
        public string UserIdNumberXMLData { get; set; }
        public string UserIdNumber { get; set; }
        public string WarehouseReleasesIds { get; set; }
        public int? WarehouseStorageFreeDays { get; set; }
        public bool IsDeclarationApprovalRequest { get; set; }
        public bool CreatedFromDigital { get; set; }

        public string DocumentFilingIds { get; set; }



        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string SLAC { get; set; }

        public string MasterProjectNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ShipmentSubTypeId { get; set; }
        public string ShipmentSubTypeName { get; set; }
        public bool IsUpdateWarehouseLegData { get; set; }
        public bool IsUpdateEntityException { get; set; }
        public string MasterHousesNumbers { get; set; }
        public string HousesDescriptionofGoods { get; set; }
        public DateTime? BookingConfirmationSentDate { get; set; }
        public DateTime? PreAlertSentDate { get; set; }
        public DateTime? DeliveryNoticeSentDate { get; set; }
        public DateTime? ExpectedArrivalNoticeSentDate { get; set; }
        public DateTime? ArrivalNoticeSentDate { get; set; }
        public DateTime? T1ReceivedDate { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool ChargeStorage { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ChargeStorageCurrencyId { get; set; }
        public string ChargeStorageCurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WeightMeasurementCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string WeightRoundingCode { get; set; }

        private List<ShipmentStoragePricingPM> shipmentStoragePricings;
        [Include]
        [Association("shipmentStoragePricingShipment", "Id", "ShipmentId")]
        [Composition]
        public virtual List<ShipmentStoragePricingPM> ShipmentStoragePricings
        {
            get
            {
                if (this.shipmentStoragePricings == null)
                {
                    shipmentStoragePricings = new List<ShipmentStoragePricingPM>();
                }
                return this.shipmentStoragePricings;
            }
            set
            {
                if (value != null)
                {
                    shipmentStoragePricings = value;
                }
            }
        }

        public List<TransshipmentLeg> MainCarriageLegs { get; set; }
        public bool IsCFSWarehouse { get; set; }
        public bool IsCFSWarehouseChanged { get; set; }
        public string ViewSharedDocuments { get; set; }
        public bool IsAccrualsApproved { get; set; }
        public DateTime? AccrualsApprovalDate { get; set; }
        public bool IsExternalAPI { get; set; }
        public double? HousesOpenPayablesInLocal { get; set; }
        public double? HousesOpenPayablesInProfit { get; set; }
        public double? HousesACCTPayablesInLocal { get; set; }
        public double? HousesACCTPayablesInProfit { get; set; }
        public double? HousesOpenReceivablesInLocal { get; set; }
        public double? HousesOpenReceivablesInProfit { get; set; }
        public double? HousesACCTReceivablesInLocal { get; set; }
        public double? HousesACCTReceivablesInProfit { get; set; }
        public string ExternalStatuses { get; set; }
        public bool IsGroupageHousesUpdated { get; set; }

        public string PreForwardingTransportModeId { get; set; }
        public string PreForwardingFromPortId { get; set; }
        public string PreForwardingToPortId { get; set; }
        public string PreForwardingCarrierId { get; set; }
        public string PreForwardingCarrierNumber { get; set; }
        public string PreForwardingCarrierName { get; set; }
        public string PreForwardingCarrierCode { get; set; }
        public string PreForwardingFromPortCode { get; set; }
        public string PreForwardingFromPortName { get; set; }
        public string PreForwardingFromPortCountryCode { get; set; }
        public string PreForwardingFromPortCountryName { get; set; }
        public string PreForwardingToPortCode { get; set; }
        public string PreForwardingToPortName { get; set; }
        public string PreForwardingToPortCountryCode { get; set; }
        public string PreForwardingToPortCountryName { get; set; }
        public DateTime? PreForwardingETD { get; set; }
        public DateTime? PreForwardingATD { get; set; }
        public DateTime? PreForwardingETA { get; set; }
        public DateTime? PreForwardingATA { get; set; }
        public string PreForwardingCarrierWebSite { get; set; }
        public string PreForwardingVesselId { get; set; }
        public string PreForwardingVesselName { get; set; }
        public bool HasPreForwarding { get; set; }
        public DateTime? PreForwardingATD_Original { get; set; }
        public DateTime? PreForwardingETD_Original { get; set; }
        public DateTime? PreForwardingATA_Original { get; set; }
        public DateTime? PreForwardingETA_Original { get; set; }

        public string OnForwardingTransportModeId { get; set; }
        public string OnForwardingFromPortId { get; set; }
        public string OnForwardingToPortId { get; set; }
        public string OnForwardingCarrierId { get; set; }
        public string OnForwardingCarrierNumber { get; set; }
        public string OnForwardingCarrierName { get; set; }
        public string OnForwardingCarrierCode { get; set; }
        public string OnForwardingFromPortCode { get; set; }
        public string OnForwardingFromPortName { get; set; }
        public string OnForwardingFromPortCountryCode { get; set; }
        public string OnForwardingFromPortCountryName { get; set; }
        public string OnForwardingToPortCode { get; set; }
        public string OnForwardingToPortName { get; set; }
        public string OnForwardingToPortCountryCode { get; set; }
        public string OnForwardingToPortCountryName { get; set; }
        public DateTime? OnForwardingETD { get; set; }
        public DateTime? OnForwardingATD { get; set; }
        public DateTime? OnForwardingETA { get; set; }
        public DateTime? OnForwardingATA { get; set; }
        public string OnForwardingCarrierWebSite { get; set; }
        public string OnForwardingVesselId { get; set; }
        public string OnForwardingVesselName { get; set; }
        public string OnForwardingAdditionalTransportModeCode { get; set; }
        public bool HasOnForwarding { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool SplitOnForwarding { get; set; }
        public DateTime? OnForwardingATD_Original { get; set; }
        public DateTime? OnForwardingETD_Original { get; set; }
        public DateTime? OnForwardingATA_Original { get; set; }
        public DateTime? OnForwardingETA_Original { get; set; }
        public string OriginPreCarriageFromPortId { get; set; }
        public string OriginOnCarriageToPortId { get; set; }
        public string OriginPreCarriageToPortId { get; set; }
        public string OriginOnCarriageFromPortId { get; set; }
        public string TotalTax { get; set; }
        public string WarehouseLegLocalName { get; set; }
        public string WarehouseLegEnglishName { get; set; }
        public bool IsHTSMissing { get; set; }
        public bool IsProductItemsUpdated { get; set; }
        public List<CustomChildEntity> CustomChildEntities { get; set; }

        private List<ShipmentProductItemPM> shipmentProductItems;
        [Include]
        [Association("ShipmentProductItemShipment", "Id", "ShipmentId")]
        [Composition]
        public virtual List<ShipmentProductItemPM> ShipmentProductItems
        {
            get
            {

                if (this.shipmentProductItems == null)
                {
                    shipmentProductItems = new List<ShipmentProductItemPM>();
                }
                return this.shipmentProductItems;
            }
            set
            {
                if (value != null)
                {
                    shipmentProductItems = value;
                }
            }
        }

        private List<ShipmentUnassignedFieldPM> shipmentUnassignedFields;
        [Include]
        [Association("ShipmentUnassignedFields", "Id", "ShipmentId")]
        [Composition]
        public virtual List<ShipmentUnassignedFieldPM> ShipmentUnassignedFields
        {
            get
            {

                if (this.shipmentUnassignedFields == null)
                {
                    shipmentUnassignedFields = new List<ShipmentUnassignedFieldPM>();
                }
                return this.shipmentUnassignedFields;
            }
            set
            {
                if (value != null)
                {
                    shipmentUnassignedFields = value;
                }
            }
        }

        // Standalone shipment
        public bool IsStandalonePickupDelivery { get; set; }
        public string StandalonePickupDeliveryId { get; set; }
        public string StandalonePickupDeliveryNumber { get; set; }
        public string ForwarderStandaloneShipmentId { get; set; }
        public string ForwarderPickUpDeliveryType { get; set; }
        public string ParentShipmentNumber { get; set; }
        public string ParentShipmentType { get; set; }
        public string ParentShipmentDirectionId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string PrivateLabelInvoiceNumber { get; set; }


        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? RequestedFlightDate { get; set; }


        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool PrivateLabelIncludePickup { get; set; }


        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public bool PrivateLabelIncludeDelivery { get; set; }

        public DateTime? PlannedCargoReadyDate { get; set; }
        public DateTime? ApprovedCargoReadyDate { get; set; }
        public string HandlerUserId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string InlandDomesticFromZipCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string InlandDomesticToZipCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string InlandDomesticFromCity { get; set; }
        public string InlandDomesticFromStateId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string InlandDomesticToCity { get; set; }
        public string InlandDomesticToStateId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string InlandDomesticFromCountryId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string InlandDomesticToCountryId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string InlandDomesticFromTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string InlandDomesticToTypeCode { get; set; }
        public string MainCarriageFromPortAddress { get; set; }
        public string MainCarriageToPortAddress { get; set; }

        public string ShipperNotExporterReference1 { get; set; }
        public string ShipperNotExporterReference2 { get; set; }
        public string OperationalStatusId { get; set; }
        public string OperationalStatusName { get; set; }
        public string BillingStatusId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string AccountingClosedByUserId { get; set; }
        public string HouseMasterConcurrencyGUID { get; set; }
        public string HouseMasterNewConcurrencyGUID { get; set; }
        public bool IsPODReceived { get; set; }
        public DateTime? PODReceivedDate { get; set; }
        public bool IsDocsKPIsUpdatedFromWR{ get; set; }
        public string UnassignedShipperAddressId { get; set; }
        public string UnassignedConsigneeAddressId { get; set; }
        public bool HasUnassignedData { get; set; }
        public string DestinationWarehouseId { get; set; }
        public string DestinationWarehouseName { get; set; }
        public List<TraceEventPM> EventList { get; set; }
        public List<TraceEventPM> AddManualEvents { get; set; }
        public string ShippingAgent { get; set; }
        public string PrivateLabelAgentName { get; set; }
        public bool IsShipmentOrder { get; set; }

        public string FirstPickupFullAddress { get; set; }
        public string LastDeliveryFullAddress { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public DateTime? QuoteFreightExpirationDate { get; set; }
        public string CarrierServiceLineId { get; set; }
        public string UnassignedShipperNotExporterAddressId { get; set; }
        public string UnassignedConsigneeNotImporterAddressId { get; set; }

        public string InlandDomesticToAddress1 { get; set; }
        public string InlandDomesticToAddress2 { get; set; }
        public string InlandDomesticToPhone { get; set; }
        public string InlandDomesticToFax { get; set; }
        public string InlandDomesticFromAddress1 { get; set; }
        public string InlandDomesticFromAddress2 { get; set; }
        public string InlandDomesticFromPhone { get; set; }
        public string InlandDomesticFromFax { get; set; }
        public int? NumberOfTransshipments { get; set; }
        public string Transshipments { get; set; }
        public bool FromCTool { get; set; }
        public string SalesmanEmail { get; set; }
        public bool IsPartiallyInvoiced { get; set; }
        public bool IsFullInvoiced { get; set; }
        public bool IsCustomerArchived { get; set; }
        public bool IsINTTRAFROB { get; set; }
        public string MainCarriageFromStateId { get; set; }
        public string MainCarriageToStateId { get; set; }
        public double? GrossWeightInLB { get; set; }
        public double? VolumeInCBF { get; set; }
        public double? ChargeableWeightInLB { get; set; }
        public string ShippingLine { get; set; }
        public string PlaceOfDelivery { get; set; }
        public string PickupPlace { get; set; }
        public string SealNo { get; set; }
        public string HSCode { get; set; }
        public string OrderNumber { get; set; }
        public bool IsUpdatedByAutomationSetValueResult { get; set; }

    }

    public partial class TransshipmentLeg
    {
        public string Id { get; set; }
        public int LegIndex { get; set; }
        public string FromPortId { get; set; }
        public string FromPortCode { get; set; }
        public string FromPortName { get; set; }
        public string ToPortId { get; set; }
        public string ToPortCode { get; set; }
        public string ToPortName { get; set; }
        public string CarrierId { get; set; }
        public string CarrierName { get; set; }
        public string CarrierNumber { get; set; }
        public string VesselId { get; set; }
        public string VesselName { get; set; }
        public string MasterNumber { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ATD { get; set; }
        public DateTime? ATA { get; set; }
        public string CarrierTypeName { get; set; }
    }

    [JsonObject(IsReference = false)]
    public partial class VerticalTimeLineData
    {
        [DataMember]
        public VerticalTimeLineStop Pickup { get; set; }
        public VerticalTimeLineStop MainCarriageFrom { get; set; }
        [DataMember]
        public VerticalTimeLineStop MainCarriageTo { get; set; }
        [DataMember]
        public VerticalTimeLineStop Delivery { get; set; }
        [DataMember]
        public VerticalTimeLineStop Transshipment1 { get; set; }
        [DataMember]
        public VerticalTimeLineStop Transshipment2 { get; set; }
        [DataMember]
        public VerticalTimeLineStop Transshipment3 { get; set; }
        [DataMember]
        public VerticalTimeLineStop PreCarriage { get; set; }
        [DataMember]
        public VerticalTimeLineStop OnCarriage { get; set; }
        [DataMember]
        public VerticalTimeLineStop Warehouse1 { get; set; }
        [DataMember]
        public VerticalTimeLineStop Warehouse2 { get; set; }

    }

    [JsonObject(IsReference = false, ItemIsReference = false)]
    public partial class VerticalTimeLineStop
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public string CountryCode { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public DateTime? ATDDate { get; set; }
        [DataMember]
        public string ATDDateType { get; set; }
        [DataMember]
        public DateTime? Date { get; set; }
        [DataMember]
        public string DateType { get; set; }
        [DataMember]
        public DateTime? ATADate { get; set; }
        [DataMember]
        public string ATADateType { get; set; }
        [DataMember]
        public bool IsViaPortsDatesFilled { get; set; }
        [DataMember]
        public string TransportModeId { get; set; }
        public string LegTransportModeId { get; set; }
        [XmlIgnore]
        public Dictionary<string, string> LegDetails { get; set; }
    }
}
