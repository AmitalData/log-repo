using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class Declaration
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CustomFileNo")]
	    public string CustomFileNo { get; set; }
        [ForeignKey("CustomerCard")]
        [Column("CustomerId")]
	    public string CustomerId { get; set; }
	      
        public virtual Card CustomerCard { get; set; }
        [ForeignKey("Importer")]
        [Column("ImporterId")]
	    public string ImporterId { get; set; }
	      
        public virtual Client Importer { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("DeclarationNumber")]
	    public string DeclarationNumber { get; set; }
        [Column("VersionId")]
	    public string VersionId { get; set; }
        [Column("ExternalDeclarationNumber")]
	    public string ExternalDeclarationNumber { get; set; }
        [ForeignKey("DeclarationOffice")]
        [Column("DeclarationOfficeCode")]
	    public string DeclarationOfficeCode { get; set; }
	      
        public virtual CustomsHouseType DeclarationOffice { get; set; }
        [Column("TaxationDateTime")]
	    public DateTime? TaxationDateTime { get; set; }
        [Column("AgentId")]
	    public string AgentId { get; set; }
        [ForeignKey("GovernmentProcedureCurrent")]
        [Column("ProcedureCurrentCode")]
	    public string ProcedureCurrentCode { get; set; }
	      
        public virtual GovernmentProcedureType GovernmentProcedureCurrent { get; set; }
        [ForeignKey("AutonomyRegionType")]
        [Column("AutonomyRegionTypeCode")]
	    public string AutonomyRegionTypeCode { get; set; }
	      
        public virtual AutonomyType AutonomyRegionType { get; set; }
        [ForeignKey("ImporterPassCountry")]
        [Column("ImporterPassCountryCode")]
	    public string ImporterPassCountryCode { get; set; }
	      
        public virtual CustomsCountry ImporterPassCountry { get; set; }
        [ForeignKey("TransferImporter")]
        [Column("TransferImporterId")]
	    public string TransferImporterId { get; set; }
	      
        public virtual Client TransferImporter { get; set; }
        [ForeignKey("TransferImporterCountry")]
        [Column("TransferImporterCountryCode")]
	    public string TransferImporterCountryCode { get; set; }
	      
        public virtual CustomsCountry TransferImporterCountry { get; set; }
        [ForeignKey("EntitleImporter")]
        [Column("EntitleImporterId")]
	    public string EntitleImporterId { get; set; }
	      
        public virtual Client EntitleImporter { get; set; }
        [ForeignKey("ImporterEntitlementType")]
        [Column("ImporterEntitlementTypeCode")]
	    public string ImporterEntitlementTypeCode { get; set; }
	      
        public virtual EntitlementType ImporterEntitlementType { get; set; }
        [ForeignKey("EntitleImporterCountry")]
        [Column("EntitleImporterCountryCode")]
	    public string EntitleImporterCountryCode { get; set; }
	      
        public virtual CustomsCountry EntitleImporterCountry { get; set; }
        [Column("DeclarationDocumentId")]
	    public string DeclarationDocumentId { get; set; }
        [ForeignKey("DeclarationDocumentType")]
        [Column("DeclarationDocumentTypeCode")]
	    public string DeclarationDocumentTypeCode { get; set; }
	      
        public virtual LeadDocumentType DeclarationDocumentType { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("IsChanged")]
	    public bool IsChanged { get; set; }
        [Column("PaymentDate")]
	    public DateTime? PaymentDate { get; set; }
        [Column("HatraDate")]
	    public DateTime? HatraDate { get; set; }
        [ForeignKey("DeclarationStatusType")]
        [Column("DeclarationStatusTypeCode")]
	    public string DeclarationStatusTypeCode { get; set; }
	      
        public virtual DeclarationStatusType DeclarationStatusType { get; set; }
        [Column("LoadingFactor")]
	    public decimal? LoadingFactor { get; set; }
        [Column("DealValue")]
	    public decimal? DealValue { get; set; }
        [Column("CIFValue")]
	    public decimal? CIFValue { get; set; }
        [Column("TotalTax")]
	    public decimal? TotalTax { get; set; }
        [Column("FileState")]
	    public string FileState { get; set; }
        [ForeignKey("CustomsTransportMode")]
        [Column("TransportModeId")]
	    public string TransportModeId { get; set; }
	      
        public virtual CustomsTransportMode CustomsTransportMode { get; set; }
        [Column("ErrosXml")]
	    public string ErrosXml { get; set; }
        [Column("ImporterName")]
	    public string ImporterName { get; set; }
        [ForeignKey("Department")]
        [Column("DepartmentId")]
	    public string DepartmentId { get; set; }
	      
        public virtual Department Department { get; set; }
        [ForeignKey("User")]
        [Column("ReferentUserId")]
	    public string ReferentUserId { get; set; }
	      
        public virtual User User { get; set; }
        [ForeignKey("DeliverySiteType")]
        [Column("StorageSiteCode")]
	    public string StorageSiteCode { get; set; }
	      
        public virtual DeliverySiteType DeliverySiteType { get; set; }
        [Column("PlatformFee")]
	    public decimal? PlatformFee { get; set; }
        [Column("CreateDateTime")]
	    public DateTime? CreateDateTime { get; set; }
        [Column("UpdateDateTime")]
	    public DateTime? UpdateDateTime { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [Column("EntitleImporterName")]
	    public string EntitleImporterName { get; set; }
        [Column("TransferImporterName")]
	    public string TransferImporterName { get; set; }
        [Column("DealValueWithoutFactor")]
	    public decimal? DealValueWithoutFactor { get; set; }
        [Column("ImporterCode")]
	    public string ImporterCode { get; set; }
        [Column("TransferImporterCode")]
	    public string TransferImporterCode { get; set; }
        [Column("EntitleImporterCode")]
	    public string EntitleImporterCode { get; set; }
        [Column("ConcurrencyGUID")]
	    public string ConcurrencyGUID { get; set; }
        [ForeignKey("ImporterType")]
        [Column("ImporterTypeCode")]
	    public string ImporterTypeCode { get; set; }
	      
        public virtual CustomerIdentifyType ImporterType { get; set; }
        [ForeignKey("TransferImporterType")]
        [Column("TransferImporterTypeCode")]
	    public string TransferImporterTypeCode { get; set; }
	      
        public virtual CustomerIdentifyType TransferImporterType { get; set; }
        [ForeignKey("EntitleImporterType")]
        [Column("EntitleImporterTypeCode")]
	    public string EntitleImporterTypeCode { get; set; }
	      
        public virtual CustomerIdentifyType EntitleImporterType { get; set; }
        [Column("UserNotes")]
	    public string UserNotes { get; set; }
        [Column("HasConstraint")]
	    public bool HasConstraint { get; set; }
        [Column("PrimaryInvoiceCounterKey")]
	    public string PrimaryInvoiceCounterKey { get; set; }
        [Column("PaymentOrderNumber")]
	    public string PaymentOrderNumber { get; set; }
        [ForeignKey("PaymentOrderStatus")]
        [Column("PaymentStatusCode")]
	    public string PaymentStatusCode { get; set; }
	      
        public virtual PaymentOrderStatus PaymentOrderStatus { get; set; }
        [Column("IsSignedVersion")]
	    public bool IsSignedVersion { get; set; }
        [ForeignKey("SignedByUser")]
        [Column("SignedByUserId")]
	    public string SignedByUserId { get; set; }
	      
        public virtual User SignedByUser { get; set; }
        [Column("StorageSiteName")]
	    public string StorageSiteName { get; set; }
        [Column("SignerPersonalId")]
	    public string SignerPersonalId { get; set; }
        [Column("IsConvertedDeclaration")]
	    public bool IsConvertedDeclaration { get; set; }
        [Column("CorrectionsXml")]
	    public string CorrectionsXml { get; set; }
        [Column("IsReleaseFile")]
	    public bool IsReleaseFile { get; set; }
        [Column("IsConnectedToUnifreight")]
	    public bool IsConnectedToUnifreight { get; set; }
        [ForeignKey("EntitlementType")]
        [Column("MainImporterEntitlemntTypeCode")]
	    public string MainImporterEntitlemntTypeCode { get; set; }
	      
        public virtual EntitlementType EntitlementType { get; set; }
        [Column("TransImporterEntitleTypeCode")]
	    public string TransImporterEntitleTypeCode { get; set; }
        [Column("ImporterAddress")]
	    public string ImporterAddress { get; set; }
        [Column("TransferImporterAddress")]
	    public string TransferImporterAddress { get; set; }
        [Column("EntitleImporterAddress")]
	    public string EntitleImporterAddress { get; set; }
        [Column("ImporterPassportNumber")]
	    public string ImporterPassportNumber { get; set; }
        [Column("TransferPassportNumber")]
	    public string TransferPassportNumber { get; set; }
        [Column("EntitlePassportNumber")]
	    public string EntitlePassportNumber { get; set; }
        [ForeignKey("StorageStatus")]
        [Column("StorageStatusCode")]
	    public string StorageStatusCode { get; set; }
	      
        public virtual StorageStatus StorageStatus { get; set; }
        [Column("CasualSupplierName")]
	    public string CasualSupplierName { get; set; }
        [Column("CasualSupplierAddress")]
	    public string CasualSupplierAddress { get; set; }
        [Column("IsCourierDeclaration")]
	    public bool IsCourierDeclaration { get; set; }
        [ForeignKey("ManifestCargoStatus")]
        [Column("ManifestCargoStatusCode")]
	    public string ManifestCargoStatusCode { get; set; }
	      
        public virtual ManifestCargoStatus ManifestCargoStatus { get; set; }
        [Column("ManifestErrorXml")]
	    public string ManifestErrorXml { get; set; }
        [Column("CourierHAWB")]
	    public string CourierHAWB { get; set; }
        [Column("ExcludeConsignment")]
	    public bool ExcludeConsignment { get; set; }
        [ForeignKey("CourierCustomStatus")]
        [Column("CourierCustomStatusCode")]
	    public string CourierCustomStatusCode { get; set; }
	      
        public virtual CourierCustomStatus CourierCustomStatus { get; set; }
        [ForeignKey("AgentTalkBackType")]
        [Column("CourierSuspentionReasonCode")]
	    public string CourierSuspentionReasonCode { get; set; }
	      
        public virtual AgentTalkBackType AgentTalkBackType { get; set; }
        [Column("CourierReleaseStatusCode")]
	    public string CourierReleaseStatusCode { get; set; }
        [Column("CourierHataraStatusCode")]
	    public string CourierHataraStatusCode { get; set; }
        [Column("DealValueWithFactor")]
	    public decimal? DealValueWithFactor { get; set; }
        [Column("IsValueForCustomsOnly")]
	    public bool IsValueForCustomsOnly { get; set; }
        [ForeignKey("FreightPaymentMethod")]
        [Column("WeightValue")]
	    public string WeightValue { get; set; }
	      
        public virtual FreightPaymentMethod FreightPaymentMethod { get; set; }
        [Column("CourierSearchFields")]
	    public string CourierSearchFields { get; set; }
        [ForeignKey("AcceptanceStatus")]
        [Column("AcceptanceStatusCode")]
	    public string AcceptanceStatusCode { get; set; }
	      
        public virtual AcceptanceStatus AcceptanceStatus { get; set; }
        [Column("CasualImporterAddress1")]
	    public string CasualImporterAddress1 { get; set; }
        [Column("CasualImporterAddress2")]
	    public string CasualImporterAddress2 { get; set; }
        [Column("CasualImporterCity")]
	    public string CasualImporterCity { get; set; }
        [Column("CasualImporterZipCode")]
	    public string CasualImporterZipCode { get; set; }
        [Column("CasualImporterFax")]
	    public string CasualImporterFax { get; set; }
        [Column("CasualImporterEmail")]
	    public string CasualImporterEmail { get; set; }
        [Column("CasualImporterTel")]
	    public string CasualImporterTel { get; set; }
        [Column("CasualImporterContact")]
	    public string CasualImporterContact { get; set; }
        [Column("ItemsProcessTypesList")]
	    public string ItemsProcessTypesList { get; set; }
        [Column("IsClose")]
	    public bool IsClose { get; set; }
        [ForeignKey("CourierSuspention")]
        [Column("CourierSuspentionCode")]
	    public string CourierSuspentionCode { get; set; }
	      
        public virtual DeclarationStatusType CourierSuspention { get; set; }
        [Column("DepositionStatusCode")]
	    public string DepositionStatusCode { get; set; }
        [Column("IsPaymentProtested")]
	    public bool IsPaymentProtested { get; set; }
        [Column("AmendmentRequestNumber")]
	    public string AmendmentRequestNumber { get; set; }
        [Column("AmendmentStatus")]
	    public string AmendmentStatus { get; set; }
        [Column("AmendmentissueDate")]
	    public DateTime? AmendmentissueDate { get; set; }
        [Column("AmendmentRemarks")]
	    public string AmendmentRemarks { get; set; }
        [Column("AmendmentDeficitInitiated")]
	    public bool? AmendmentDeficitInitiated { get; set; }
        [Column("AmendDeficitInitiatedReasTo")]
	    public string AmendDeficitInitiatedReasTo { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("AmendmentCorrectedByUserId")]
	    public string AmendmentCorrectedByUserId { get; set; }
        [Column("AmendmentRejectionReason")]
	    public string AmendmentRejectionReason { get; set; }
        [Column("IsAmendment")]
	    public bool? IsAmendment { get; set; }
        [Column("AmendmentOriginalDeclartation")]
	    public string AmendmentOriginalDeclartation { get; set; }
    }
}
	 