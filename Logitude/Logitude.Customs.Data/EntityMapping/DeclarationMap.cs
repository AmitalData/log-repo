using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class DeclarationMap : EntityTypeConfiguration<Declaration>
    {
	    string dbms;
        public DeclarationMap()
        { 
			  this.ToTable("Declarations", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.CustomFileNo).HasColumnName("CustomFileNo").HasMaxLength(12).IsUnicode(false);

            this.Property(t => t.CustomerId).HasColumnName("CustomerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ImporterId).HasColumnName("ImporterId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.DeclarationNumber).HasColumnName("DeclarationNumber").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.VersionId).HasColumnName("VersionId").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.ExternalDeclarationNumber).HasColumnName("ExternalDeclarationNumber").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.DeclarationOfficeCode).HasColumnName("DeclarationOfficeCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.TaxationDateTime).HasColumnName("TaxationDateTime");

            this.Property(t => t.AgentId).HasColumnName("AgentId").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.ProcedureCurrentCode).HasColumnName("ProcedureCurrentCode").HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.AutonomyRegionTypeCode).HasColumnName("AutonomyRegionTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ImporterPassCountryCode).HasColumnName("ImporterPassCountryCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.TransferImporterId).HasColumnName("TransferImporterId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TransferImporterCountryCode).HasColumnName("TransferImporterCountryCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.EntitleImporterId).HasColumnName("EntitleImporterId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ImporterEntitlementTypeCode).HasColumnName("ImporterEntitlementTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.EntitleImporterCountryCode).HasColumnName("EntitleImporterCountryCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.DeclarationDocumentId).HasColumnName("DeclarationDocumentId").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.DeclarationDocumentTypeCode).HasColumnName("DeclarationDocumentTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsChanged).HasColumnName("IsChanged");

            this.Property(t => t.PaymentDate).HasColumnName("PaymentDate");

            this.Property(t => t.HatraDate).HasColumnName("HatraDate");

            this.Property(t => t.DeclarationStatusTypeCode).HasColumnName("DeclarationStatusTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.LoadingFactor).HasColumnName("LoadingFactor").HasPrecision(18, 10);

            this.Property(t => t.DealValue).HasColumnName("DealValue").HasPrecision(16, 2);

            this.Property(t => t.CIFValue).HasColumnName("CIFValue").HasPrecision(16, 2);

            this.Property(t => t.TotalTax).HasColumnName("TotalTax").HasPrecision(16, 2);

            this.Property(t => t.FileState).HasColumnName("FileState").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId").HasMaxLength(1).IsFixedLength();

            this.Property(t => t.ErrosXml).HasColumnName("ErrosXml").IsMaxLength().IsUnicode(true);

            this.Property(t => t.ImporterName).HasColumnName("ImporterName").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ReferentUserId).HasColumnName("ReferentUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StorageSiteCode).HasColumnName("StorageSiteCode").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.PlatformFee).HasColumnName("PlatformFee").HasPrecision(18, 2);

            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");

            this.Property(t => t.UpdateDateTime).HasColumnName("UpdateDateTime");

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");

            this.Property(t => t.EntitleImporterName).HasColumnName("EntitleImporterName").HasMaxLength(55).IsUnicode(true);

            this.Property(t => t.TransferImporterName).HasColumnName("TransferImporterName").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.DealValueWithoutFactor).HasColumnName("DealValueWithoutFactor").HasPrecision(16, 2);

            this.Property(t => t.ImporterCode).HasColumnName("ImporterCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TransferImporterCode).HasColumnName("TransferImporterCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntitleImporterCode).HasColumnName("EntitleImporterCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConcurrencyGUID).HasColumnName("ConcurrencyGUID").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.ImporterTypeCode).HasColumnName("ImporterTypeCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.TransferImporterTypeCode).HasColumnName("TransferImporterTypeCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.EntitleImporterTypeCode).HasColumnName("EntitleImporterTypeCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.UserNotes).HasColumnName("UserNotes").IsMaxLength().IsUnicode(true);

            this.Property(t => t.HasConstraint).HasColumnName("HasConstraint");

            this.Property(t => t.PrimaryInvoiceCounterKey).HasColumnName("PrimaryInvoiceCounterKey").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PaymentOrderNumber).HasColumnName("PaymentOrderNumber").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.PaymentStatusCode).HasColumnName("PaymentStatusCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.IsSignedVersion).HasColumnName("IsSignedVersion");

            this.Property(t => t.SignedByUserId).HasColumnName("SignedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StorageSiteName).HasColumnName("StorageSiteName").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.SignerPersonalId).HasColumnName("SignerPersonalId").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.IsConvertedDeclaration).HasColumnName("IsConvertedDeclaration");

            this.Property(t => t.CorrectionsXml).HasColumnName("CorrectionsXml").IsMaxLength().IsUnicode(true);

            this.Property(t => t.IsReleaseFile).HasColumnName("IsReleaseFile");

            this.Property(t => t.IsConnectedToUnifreight).HasColumnName("IsConnectedToUnifreight");

            this.Property(t => t.MainImporterEntitlemntTypeCode).HasColumnName("MainImporterEntitlemntTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.TransImporterEntitleTypeCode).HasColumnName("TransImporterEntitleTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ImporterAddress).HasColumnName("ImporterAddress").HasMaxLength(236).IsUnicode(true);

            this.Property(t => t.TransferImporterAddress).HasColumnName("TransferImporterAddress").HasMaxLength(236).IsUnicode(true);

            this.Property(t => t.EntitleImporterAddress).HasColumnName("EntitleImporterAddress").HasMaxLength(236).IsUnicode(true);

            this.Property(t => t.ImporterPassportNumber).HasColumnName("ImporterPassportNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TransferPassportNumber).HasColumnName("TransferPassportNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntitlePassportNumber).HasColumnName("EntitlePassportNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StorageStatusCode).HasColumnName("StorageStatusCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.CasualSupplierName).HasColumnName("CasualSupplierName").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.CasualSupplierAddress).HasColumnName("CasualSupplierAddress").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.IsCourierDeclaration).HasColumnName("IsCourierDeclaration");

            this.Property(t => t.ManifestCargoStatusCode).HasColumnName("ManifestCargoStatusCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.ManifestErrorXml).HasColumnName("ManifestErrorXml").IsMaxLength().IsUnicode(false);

            this.Property(t => t.CourierHAWB).HasColumnName("CourierHAWB").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.ExcludeConsignment).HasColumnName("ExcludeConsignment");

            this.Property(t => t.CourierCustomStatusCode).HasColumnName("CourierCustomStatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CourierSuspentionReasonCode).HasColumnName("CourierSuspentionReasonCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CourierReleaseStatusCode).HasColumnName("CourierReleaseStatusCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.CourierHataraStatusCode).HasColumnName("CourierHataraStatusCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.DealValueWithFactor).HasColumnName("DealValueWithFactor").HasPrecision(16, 2);

            this.Property(t => t.IsValueForCustomsOnly).HasColumnName("IsValueForCustomsOnly");

            this.Property(t => t.WeightValue).HasColumnName("WeightValue").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CourierSearchFields).HasColumnName("CourierSearchFields").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.AcceptanceStatusCode).HasColumnName("AcceptanceStatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CasualImporterAddress1).HasColumnName("CasualImporterAddress1").HasMaxLength(35).IsUnicode(true);

            this.Property(t => t.CasualImporterAddress2).HasColumnName("CasualImporterAddress2").HasMaxLength(35).IsUnicode(true);

            this.Property(t => t.CasualImporterCity).HasColumnName("CasualImporterCity").HasMaxLength(17).IsUnicode(true);

            this.Property(t => t.CasualImporterZipCode).HasColumnName("CasualImporterZipCode").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.CasualImporterFax).HasColumnName("CasualImporterFax").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.CasualImporterEmail).HasColumnName("CasualImporterEmail").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.CasualImporterTel).HasColumnName("CasualImporterTel").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.CasualImporterContact).HasColumnName("CasualImporterContact").HasMaxLength(50).IsUnicode(true);

            this.Property(t => t.ItemsProcessTypesList).HasColumnName("ItemsProcessTypesList").IsMaxLength().IsUnicode(false);

            this.Property(t => t.IsClose).HasColumnName("IsClose");

            this.Property(t => t.CourierSuspentionCode).HasColumnName("CourierSuspentionCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.DepositionStatusCode).HasColumnName("DepositionStatusCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.CargoDescription).HasColumnName("CargoDescription").HasMaxLength(256).IsUnicode(true);

            this.Property(t => t.IsPaymentProtested).HasColumnName("IsPaymentProtested");

            this.Property(t => t.AmendmentRequestNumber).HasColumnName("AmendmentRequestNumber").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.AmendmentStatus).HasColumnName("AmendmentStatus").HasMaxLength(3).IsUnicode(true);

            this.Property(t => t.AmendmentissueDate).HasColumnName("AmendmentissueDate");

            this.Property(t => t.AmendmentRemarks).HasColumnName("AmendmentRemarks").HasMaxLength(512).IsUnicode(true);

            this.Property(t => t.AmendmentDeficitInitiated).HasColumnName("AmendmentDeficitInitiated");

            this.Property(t => t.AmendDeficitInitiatedReasTo).HasColumnName("AmendDeficitInitiatedReasTo").HasMaxLength(512).IsUnicode(true);

            this.Property(t => t.AmendmentCorrectedByUserId).HasColumnName("AmendmentCorrectedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AmendmentRejectionReason).HasColumnName("AmendmentRejectionReason").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.IsAmendment).HasColumnName("IsAmendment");

            this.Property(t => t.AmendmentOriginalDeclartation).HasColumnName("AmendmentOriginalDeclartation").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsDiamondDeclaration).HasColumnName("IsDiamondDeclaration");

            this.Property(t => t.AmendmentDontDisplayInList).HasColumnName("AmendmentDontDisplayInList");

            this.Property(t => t.IsMissMandatoryDiamond).HasColumnName("IsMissMandatoryDiamond");

            this.Property(t => t.IsValidTicketsDiamond).HasColumnName("IsValidTicketsDiamond");

            this.Property(t => t.AvailabilityDate).HasColumnName("AvailabilityDate");

            this.Property(t => t.LoadingDateTime).HasColumnName("LoadingDateTime");

            this.Property(t => t.ShipCode).HasColumnName("ShipCode").HasMaxLength(25).IsUnicode(false);

            this.Property(t => t.IsExporterConfirmation).HasColumnName("IsExporterConfirmation");

            this.Property(t => t.Direction).HasColumnName("Direction").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.AgentRoleCode).HasColumnName("AgentRoleCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.ExportFile).HasColumnName("ExportFile").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DestinationCountryCode).HasColumnName("DestinationCountryCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.ExportAutonomyRegionTypeCode).HasColumnName("ExportAutonomyRegionTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.DeclarationTypeCode).HasColumnName("DeclarationTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CancelRequestReasonCode).HasColumnName("CancelRequestReasonCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CancelRequestReasonExplanation).HasColumnName("CancelRequestReasonExplanation").HasMaxLength(512).IsUnicode(true);

            this.Property(t => t.CancelRequestNumber).HasColumnName("CancelRequestNumber");

            this.Property(t => t.CustomCancelRequestRemarks).HasColumnName("CustomCancelRequestRemarks").HasMaxLength(512).IsUnicode(true);

            this.Property(t => t.CancelRequestStatusCode).HasColumnName("CancelRequestStatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CancelRequestRejectionReason).HasColumnName("CancelRequestRejectionReason").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CancelRequestApproveDate).HasColumnName("CancelRequestApproveDate");

            this.Property(t => t.IsClaimable).HasColumnName("IsClaimable");

            this.Property(t => t.ReplacingRepairRequest).HasColumnName("ReplacingRepairRequest").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.AmendmentErrorXml).HasColumnName("AmendmentErrorXml").IsMaxLength().IsUnicode(true);

            this.Property(t => t.FOBValueNIS).HasColumnName("FOBValueNIS").HasPrecision(16, 2);

            this.Property(t => t.FOBValueDollar).HasColumnName("FOBValueDollar").HasPrecision(16, 2);

            this.Property(t => t.TransshipmentApprovalDateTime).HasColumnName("TransshipmentApprovalDateTime");

            this.Property(t => t.FinalLoadingSite).HasColumnName("FinalLoadingSite").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.PalestinianCode).HasColumnName("PalestinianCode").HasMaxLength(15).IsUnicode(true);

            this.Property(t => t.RequestedCustomsDocId).HasColumnName("RequestedCustomsDocId");

            this.Property(t => t.ExportDeclarationOfficeCode).HasColumnName("ExportDeclarationOfficeCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.PhysicalCheck).HasColumnName("PhysicalCheck").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.IsSubmitDeclaration).HasColumnName("IsSubmitDeclaration");

            this.Property(t => t.AmedmentType).HasColumnName("AmedmentType").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.IsExportClosed).HasColumnName("IsExportClosed");

            this.Property(t => t.ExportClosedErrorXML).HasColumnName("ExportClosedErrorXML").IsMaxLength().IsUnicode(true);

            this.Property(t => t.ExportLoadingPortCode).HasColumnName("ExportLoadingPortCode").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.ReleaseStatusTypeCode).HasColumnName("ReleaseStatusTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.ClosingXml).HasColumnName("ClosingXml").IsMaxLength().IsUnicode(true);

            this.Property(t => t.ExportCloseAmendRequestNumber).HasColumnName("ExportCloseAmendRequestNumber").HasMaxLength(9).IsUnicode(false);

            this.Property(t => t.ExportCloseAmendmentStatus).HasColumnName("ExportCloseAmendmentStatus").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CasualImporterCountry).HasColumnName("CasualImporterCountry").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.ExcludeManifest).HasColumnName("ExcludeManifest");

            this.Property(t => t.ForwarderFiles).HasColumnName("ForwarderFiles").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.ShortProcedure).HasColumnName("ShortProcedure");

            this.Property(t => t.ExportFlightDate).HasColumnName("ExportFlightDate");

            this.Property(t => t.UNFCourier).HasColumnName("UNFCourier");

            this.Property(t => t.AutoSending).HasColumnName("AutoSending");

            this.Property(t => t.SystemConnection).HasColumnName("SystemConnection").HasMaxLength(1).IsUnicode(false);
        }
    }
}
	 