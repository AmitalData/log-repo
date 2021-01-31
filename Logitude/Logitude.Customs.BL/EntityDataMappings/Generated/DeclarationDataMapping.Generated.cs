
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class DeclarationDataMapping: IMapping<DeclarationPM, Declaration>,IMappingEncodeBase64NVARCHARFields<DeclarationPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CustomFileNo, 
	         CustomerId, 
	         ImporterId, 
	         SearchFields, 
	         DeclarationNumber, 
	         VersionId, 
	         ExternalDeclarationNumber, 
	         DeclarationOfficeCode, 
	         TaxationDateTime, 
	         AgentId, 
	         ProcedureCurrentCode, 
	         AutonomyRegionTypeCode, 
	         ImporterPassCountryCode, 
	         TransferImporterId, 
	         TransferImporterCountryCode, 
	         EntitleImporterId, 
	         ImporterEntitlementTypeCode, 
	         EntitleImporterCountryCode, 
	         DeclarationDocumentId, 
	         DeclarationDocumentTypeCode, 
	         CreatedByUserId, 
	         IsChanged, 
	         PaymentDate, 
	         HatraDate, 
	         DeclarationStatusTypeCode, 
	         LoadingFactor, 
	         DealValue, 
	         CIFValue, 
	         TotalTax, 
	         FileState, 
	         TransportModeId, 
	         ErrosXml, 
	         ImporterName, 
	         DepartmentId, 
	         ReferentUserId, 
	         StorageSiteCode, 
	         PlatformFee, 
	         CreateDateTime, 
	         UpdateDateTime, 
	         IsCancelled, 
	         EntitleImporterName, 
	         TransferImporterName, 
	         DealValueWithoutFactor, 
	         ImporterCode, 
	         TransferImporterCode, 
	         EntitleImporterCode, 
	         ConcurrencyGUID, 
	         ImporterTypeCode, 
	         TransferImporterTypeCode, 
	         EntitleImporterTypeCode, 
	         UserNotes, 
	         HasConstraint, 
	         PrimaryInvoiceCounterKey, 
	         PaymentOrderNumber, 
	         PaymentStatusCode, 
	         IsSignedVersion, 
	         SignedByUserId, 
	         StorageSiteName, 
	         SignerPersonalId, 
	         IsConvertedDeclaration, 
	         CorrectionsXml, 
	         IsReleaseFile, 
	         IsConnectedToUnifreight, 
	         MainImporterEntitlemntTypeCode, 
	         TransImporterEntitleTypeCode, 
	         ImporterAddress, 
	         TransferImporterAddress, 
	         EntitleImporterAddress, 
	         ImporterPassportNumber, 
	         TransferPassportNumber, 
	         EntitlePassportNumber, 
	         StorageStatusCode, 
	         CasualSupplierName, 
	         CasualSupplierAddress, 
	         IsCourierDeclaration, 
	         ManifestCargoStatusCode, 
	         ManifestErrorXml, 
	         CourierHAWB, 
	         ExcludeConsignment, 
	         CourierCustomStatusCode, 
	         CourierSuspentionReasonCode, 
	         CourierReleaseStatusCode, 
	         CourierHataraStatusCode, 
	         DealValueWithFactor, 
	         IsValueForCustomsOnly, 
	         WeightValue, 
	         CourierSearchFields, 
	         AcceptanceStatusCode, 
	         CasualImporterAddress1, 
	         CasualImporterAddress2, 
	         CasualImporterCity, 
	         CasualImporterZipCode, 
	         CasualImporterFax, 
	         CasualImporterEmail, 
	         CasualImporterTel, 
	         CasualImporterContact, 
	         ItemsProcessTypesList, 
	         IsClose, 
	         CourierSuspentionCode, 
	         DepositionStatusCode, 
	         IsPaymentProtested, 
	         AmendmentRequestNumber, 
	         AmendmentStatus, 
	         AmendmentissueDate, 
	         AmendmentRemarks, 
	         AmendmentDeficitInitiated, 
	         AmendDeficitInitiatedReasTo, 
	         AmendmentCorrectedByUserId, 
	         AmendmentRejectionReason, 
	         IsAmendment, 
	         AmendmentOriginalDeclartation, 
	         IsDiamondDeclaration, 
	         AmendmentDontDisplayInList, 
	         IsMissMandatoryDiamond, 
	         IsValidTicketsDiamond, 
	         AvailabilityDate, 
	         LoadingDateTime, 
	         ShipCode, 
	         IsExporterConfirmation, 
	         Direction, 
	         AgentRoleCode, 
	         ExportFile, 
	         DestinationCountryCode, 
	         ExportAutonomyRegionTypeCode, 
	         DeclarationTypeCode, 
	         CancelRequestReasonCode, 
	         CancelRequestReasonExplanation, 
	         CancelRequestNumber, 
	         CustomCancelRequestRemarks, 
	         CancelRequestStatusCode, 
	         CancelRequestRejectionReason, 
	         CancelRequestApproveDate, 
	         IsClaimable, 
	         ReplacingRepairRequest, 
	         AmendmentErrorXml, 
	         FOBValueNIS, 
	         FOBValueDollar, 
	         TransshipmentApprovalDateTime, 
	         FinalLoadingSite, 
	         PalestinianCode, 
	         RequestedCustomsDocId, 
	         ExportDeclarationOfficeCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CustomFileNo, 
	         CustomerId, 
	         ImporterId, 
	         SearchFields, 
	         DeclarationNumber, 
	         CustomerName, 
	         VersionId, 
	         ExternalDeclarationNumber, 
	         DeclarationOfficeCode, 
	         TaxationDateTime, 
	         AgentId, 
	         ProcedureCurrentCode, 
	         ProcedureCurrentName, 
	         AutonomyRegionTypeCode, 
	         AutonomyRegionTypeName, 
	         ImporterPassCountryCode, 
	         ImporterPassCountryName, 
	         TransferImporterId, 
	         TransferImporterCountryCode, 
	         TransferImporterCountryName, 
	         EntitleImporterId, 
	         ImporterEntitlementTypeCode, 
	         ImporterEntitlementTypeName, 
	         EntitleImporterCountryCode, 
	         EntitleImporterCountryName, 
	         DeclarationDocumentId, 
	         DeclarationDocumentTypeCode, 
	         DeclarationDocumentTypeName, 
	         CreatedByUserId, 
	         IsChanged, 
	         PaymentDate, 
	         HatraDate, 
	         DeclarationStatusTypeCode, 
	         LoadingFactor, 
	         DealValue, 
	         CIFValue, 
	         TotalTax, 
	         DeclarationNumberandVersionId, 
	         FileState, 
	         TransportModeId, 
	         ErrosXml, 
	         DeclarationOfficeName, 
	         ImporterName, 
	         DepartmentId, 
	         DepartmentName, 
	         ReferentUserId, 
	         DeclarationStatusTypeName, 
	         StorageSiteCode, 
	         PlatformFee, 
	         CustomerCode, 
	         CreateDateTime, 
	         UpdateDateTime, 
	         IsCancelled, 
	         EntitleImporterName, 
	         TransportModeName, 
	         TransferImporterName, 
	         DealValueWithoutFactor, 
	         ImporterCode, 
	         TransferImporterCode, 
	         EntitleImporterCode, 
	         MarkAsChanged, 
	         ConcurrencyGUID, 
	         NewConcurrencyGUID, 
	         CreatedByUserName, 
	         ImporterTypeCode, 
	         TransferImporterTypeCode, 
	         EntitleImporterTypeCode, 
	         ImporterTypeName, 
	         TransferImporterTypeName, 
	         EntitleImporterTypeName, 
	         UserNotes, 
	         FreightValuesFilled, 
	         PaidDeclarationWithoutRelease, 
	         DeclarationWithoutRelease, 
	         HasConstraint, 
	         PrimaryInvoiceCounterKey, 
	         PaymentOrderNumber, 
	         PaymentStatusCode, 
	         IsSignedVersion, 
	         SignedByUserId, 
	         StorageSiteName, 
	         HasDocument, 
	         SignerPersonalId, 
	         CustomsNumeral, 
	         IsConvertedDeclaration, 
	         CorrectionsXml, 
	         ResetDeclarationNumber, 
	         IsCopiedFromOtherDeclaration, 
	         RequestFileNumber, 
	         IsReleaseFile, 
	         VatChanged, 
	         IsConnectedToUnifreight, 
	         MainImporterEntitlemntTypeCode, 
	         TransImporterEntitleTypeCode, 
	         ImporterAddress, 
	         TransferImporterAddress, 
	         EntitleImporterAddress, 
	         ImporterPassportNumber, 
	         TransferPassportNumber, 
	         EntitlePassportNumber, 
	         FacilityTypeName, 
	         CalculatedImporterName, 
	         CalculatedTransferImporterName, 
	         CalculatedEntitleImporterName, 
	         CustomerVatNo, 
	         DeclarationPaymentChanged, 
	         CustomsRequestsSheetId, 
	         DocumentDeclarationId, 
	         StorageStatusCode, 
	         CasualSupplierName, 
	         CasualSupplierAddress, 
	         IsCourierDeclaration, 
	         ManifestCargoStatusCode, 
	         ManifestErrorXml, 
	         CourierHAWB, 
	         ManifestNumber, 
	         StorageStatusName, 
	         IsAccumulated, 
	         ExcludeConsignment, 
	         CourierCustomStatusCode, 
	         CourierSuspentionReasonCode, 
	         CourierReleaseStatusCode, 
	         CourierHataraStatusCode, 
	         CourierData, 
	         InvoiceHasFreight, 
	         DealValueWithFactor, 
	         IsValueForCustomsOnly, 
	         WeightValue, 
	         WeightValueName, 
	         CourierSearchFields, 
	         CourierCustomStatusName, 
	         ManifestCargoStatusName, 
	         MAWBCourierMaster, 
	         CourierSuspentionReasonName, 
	         AcceptanceStatusCode, 
	         CasualImporterAddress1, 
	         CasualImporterAddress2, 
	         CasualImporterCity, 
	         CasualImporterZipCode, 
	         CasualImporterFax, 
	         CasualImporterEmail, 
	         CasualImporterTel, 
	         CasualImporterContact, 
	         ItemsProcessTypesList, 
	         IsClose, 
	         AcceptanceStatusName, 
	         CourierSuspentionCode, 
	         CourierSuspentionName, 
	         DepositionStatusCode, 
	         CourierMasterId, 
	         IsClosedForFollowUp, 
	         FastIndividualProcessCode, 
	         TotalInvoiceAmountInUSD, 
	         IsPending902, 
	         IsCourierMissingClassification, 
	         MAWB, 
	         IsPending900, 
	         CourierPendingReasonList, 
	         CargoDescription, 
	         IsPaymentProtested, 
	         FastIndividualProcessName, 
	         AmendmentRequestNumber, 
	         AmendmentStatus, 
	         AmendmentissueDate, 
	         AmendmentRemarks, 
	         AmendmentDeficitInitiated, 
	         AmendDeficitInitiatedReasTo, 
	         AmendmentCorrectedByUserId, 
	         AmendmentRejectionReason, 
	         IsAmendment, 
	         AmendmentOriginalDeclartation, 
	         AmendmentCorrectedByUserName, 
	         AmendmentStatusName, 
	         CourierManifestStatusCode, 
	         CourierPaymentStatusCode, 
	         IsPendingNotNull, 
	         IsDiamondDeclaration, 
	         AmendmentDontDisplayInList, 
	         AmendmentMessage, 
	         IsAmendmentDisplayOnly, 
	         IsMissMandatoryDiamond, 
	         IsValidTicketsDiamond, 
	         CustomFileAmendment, 
	         DeclarationNoAmendment, 
	         AvailabilityDate, 
	         AmendmentNumber, 
	         CourierPendingReasonName, 
	         AutomaticPayment, 
	         LoadingDateTime, 
	         ShipCode, 
	         IsExporterConfirmation, 
	         ShipName, 
	         DestinationCountryName, 
	         Direction, 
	         AgentRoleCode, 
	         ExportFile, 
	         DestinationCountryCode, 
	         ExportAutonomyRegionTypeCode, 
	         DeclarationTypeCode, 
	         CancelRequestReasonCode, 
	         CancelRequestReasonExplanation, 
	         CancelRequestNumber, 
	         CustomCancelRequestRemarks, 
	         CancelRequestStatusCode, 
	         CancelRequestRejectionReason, 
	         CancelRequestApproveDate, 
	         IsClaimable, 
	         CancelRequestStatusName, 
	         ReplacingRepairRequest, 
	         AmendmentErrorXml, 
	         FOBValueNIS, 
	         FOBValueDollar, 
	         AmendmentRejectionReasonName, 
	         TransshipmentApprovalDateTime, 
	         FinalLoadingSite, 
	         PalestinianCode, 
	         RequestedCustomsDocId, 
	         ExportDeclarationOfficeCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DeclarationPM entityPM, Declaration entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomFileNo))
            {
				entityPOCO.CustomFileNo = entityPM.CustomFileNo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
				entityPOCO.CustomerId = entityPM.CustomerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterId))
            {
				entityPOCO.ImporterId = entityPM.ImporterId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationNumber))
            {
				entityPOCO.DeclarationNumber = entityPM.DeclarationNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VersionId))
            {
				entityPOCO.VersionId = entityPM.VersionId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalDeclarationNumber))
            {
				entityPOCO.ExternalDeclarationNumber = entityPM.ExternalDeclarationNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationOfficeCode))
            {
				entityPOCO.DeclarationOfficeCode = entityPM.DeclarationOfficeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxationDateTime))
            {
				entityPOCO.TaxationDateTime = entityPM.TaxationDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentId))
            {
				entityPOCO.AgentId = entityPM.AgentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProcedureCurrentCode))
            {
				entityPOCO.ProcedureCurrentCode = entityPM.ProcedureCurrentCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutonomyRegionTypeCode))
            {
				entityPOCO.AutonomyRegionTypeCode = entityPM.AutonomyRegionTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterPassCountryCode))
            {
				entityPOCO.ImporterPassCountryCode = entityPM.ImporterPassCountryCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferImporterId))
            {
				entityPOCO.TransferImporterId = entityPM.TransferImporterId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferImporterCountryCode))
            {
				entityPOCO.TransferImporterCountryCode = entityPM.TransferImporterCountryCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitleImporterId))
            {
				entityPOCO.EntitleImporterId = entityPM.EntitleImporterId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterEntitlementTypeCode))
            {
				entityPOCO.ImporterEntitlementTypeCode = entityPM.ImporterEntitlementTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitleImporterCountryCode))
            {
				entityPOCO.EntitleImporterCountryCode = entityPM.EntitleImporterCountryCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationDocumentId))
            {
				entityPOCO.DeclarationDocumentId = entityPM.DeclarationDocumentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationDocumentTypeCode))
            {
				entityPOCO.DeclarationDocumentTypeCode = entityPM.DeclarationDocumentTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsChanged))
            {
				entityPOCO.IsChanged = entityPM.IsChanged;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentDate))
            {
				entityPOCO.PaymentDate = entityPM.PaymentDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HatraDate))
            {
				entityPOCO.HatraDate = entityPM.HatraDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationStatusTypeCode))
            {
				entityPOCO.DeclarationStatusTypeCode = entityPM.DeclarationStatusTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LoadingFactor))
            {
				entityPOCO.LoadingFactor = entityPM.LoadingFactor;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DealValue))
            {
				entityPOCO.DealValue = entityPM.DealValue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CIFValue))
            {
				entityPOCO.CIFValue = entityPM.CIFValue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalTax))
            {
				entityPOCO.TotalTax = entityPM.TotalTax;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FileState))
            {
				entityPOCO.FileState = entityPM.FileState;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
				entityPOCO.TransportModeId = entityPM.TransportModeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrosXml))
            {
				entityPOCO.ErrosXml = entityPM.ErrosXml;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterName))
            {
				entityPOCO.ImporterName = entityPM.ImporterName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepartmentId))
            {
				entityPOCO.DepartmentId = entityPM.DepartmentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReferentUserId))
            {
				entityPOCO.ReferentUserId = entityPM.ReferentUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageSiteCode))
            {
				entityPOCO.StorageSiteCode = entityPM.StorageSiteCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PlatformFee))
            {
				entityPOCO.PlatformFee = entityPM.PlatformFee;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDateTime))
            {
				entityPOCO.CreateDateTime = entityPM.CreateDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDateTime))
            {
				entityPOCO.UpdateDateTime = entityPM.UpdateDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
				entityPOCO.IsCancelled = entityPM.IsCancelled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitleImporterName))
            {
				entityPOCO.EntitleImporterName = entityPM.EntitleImporterName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferImporterName))
            {
				entityPOCO.TransferImporterName = entityPM.TransferImporterName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DealValueWithoutFactor))
            {
				entityPOCO.DealValueWithoutFactor = entityPM.DealValueWithoutFactor;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterCode))
            {
				entityPOCO.ImporterCode = entityPM.ImporterCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferImporterCode))
            {
				entityPOCO.TransferImporterCode = entityPM.TransferImporterCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitleImporterCode))
            {
				entityPOCO.EntitleImporterCode = entityPM.EntitleImporterCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
				entityPOCO.ConcurrencyGUID = entityPM.ConcurrencyGUID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterTypeCode))
            {
				entityPOCO.ImporterTypeCode = entityPM.ImporterTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferImporterTypeCode))
            {
				entityPOCO.TransferImporterTypeCode = entityPM.TransferImporterTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitleImporterTypeCode))
            {
				entityPOCO.EntitleImporterTypeCode = entityPM.EntitleImporterTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserNotes))
            {
				entityPOCO.UserNotes = entityPM.UserNotes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HasConstraint))
            {
				entityPOCO.HasConstraint = entityPM.HasConstraint;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrimaryInvoiceCounterKey))
            {
				entityPOCO.PrimaryInvoiceCounterKey = entityPM.PrimaryInvoiceCounterKey;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentOrderNumber))
            {
				entityPOCO.PaymentOrderNumber = entityPM.PaymentOrderNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentStatusCode))
            {
				entityPOCO.PaymentStatusCode = entityPM.PaymentStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSignedVersion))
            {
				entityPOCO.IsSignedVersion = entityPM.IsSignedVersion;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SignedByUserId))
            {
				entityPOCO.SignedByUserId = entityPM.SignedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageSiteName))
            {
				entityPOCO.StorageSiteName = entityPM.StorageSiteName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SignerPersonalId))
            {
				entityPOCO.SignerPersonalId = entityPM.SignerPersonalId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConvertedDeclaration))
            {
				entityPOCO.IsConvertedDeclaration = entityPM.IsConvertedDeclaration;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CorrectionsXml))
            {
				entityPOCO.CorrectionsXml = entityPM.CorrectionsXml;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsReleaseFile))
            {
				entityPOCO.IsReleaseFile = entityPM.IsReleaseFile;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConnectedToUnifreight))
            {
				entityPOCO.IsConnectedToUnifreight = entityPM.IsConnectedToUnifreight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainImporterEntitlemntTypeCode))
            {
				entityPOCO.MainImporterEntitlemntTypeCode = entityPM.MainImporterEntitlemntTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransImporterEntitleTypeCode))
            {
				entityPOCO.TransImporterEntitleTypeCode = entityPM.TransImporterEntitleTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterAddress))
            {
				entityPOCO.ImporterAddress = entityPM.ImporterAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferImporterAddress))
            {
				entityPOCO.TransferImporterAddress = entityPM.TransferImporterAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitleImporterAddress))
            {
				entityPOCO.EntitleImporterAddress = entityPM.EntitleImporterAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterPassportNumber))
            {
				entityPOCO.ImporterPassportNumber = entityPM.ImporterPassportNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferPassportNumber))
            {
				entityPOCO.TransferPassportNumber = entityPM.TransferPassportNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitlePassportNumber))
            {
				entityPOCO.EntitlePassportNumber = entityPM.EntitlePassportNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageStatusCode))
            {
				entityPOCO.StorageStatusCode = entityPM.StorageStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualSupplierName))
            {
				entityPOCO.CasualSupplierName = entityPM.CasualSupplierName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualSupplierAddress))
            {
				entityPOCO.CasualSupplierAddress = entityPM.CasualSupplierAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCourierDeclaration))
            {
				entityPOCO.IsCourierDeclaration = entityPM.IsCourierDeclaration;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManifestCargoStatusCode))
            {
				entityPOCO.ManifestCargoStatusCode = entityPM.ManifestCargoStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManifestErrorXml))
            {
				entityPOCO.ManifestErrorXml = entityPM.ManifestErrorXml;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierHAWB))
            {
				entityPOCO.CourierHAWB = entityPM.CourierHAWB;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExcludeConsignment))
            {
				entityPOCO.ExcludeConsignment = entityPM.ExcludeConsignment;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierCustomStatusCode))
            {
				entityPOCO.CourierCustomStatusCode = entityPM.CourierCustomStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierSuspentionReasonCode))
            {
				entityPOCO.CourierSuspentionReasonCode = entityPM.CourierSuspentionReasonCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierReleaseStatusCode))
            {
				entityPOCO.CourierReleaseStatusCode = entityPM.CourierReleaseStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierHataraStatusCode))
            {
				entityPOCO.CourierHataraStatusCode = entityPM.CourierHataraStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DealValueWithFactor))
            {
				entityPOCO.DealValueWithFactor = entityPM.DealValueWithFactor;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsValueForCustomsOnly))
            {
				entityPOCO.IsValueForCustomsOnly = entityPM.IsValueForCustomsOnly;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WeightValue))
            {
				entityPOCO.WeightValue = entityPM.WeightValue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierSearchFields))
            {
				entityPOCO.CourierSearchFields = entityPM.CourierSearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AcceptanceStatusCode))
            {
				entityPOCO.AcceptanceStatusCode = entityPM.AcceptanceStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterAddress1))
            {
				entityPOCO.CasualImporterAddress1 = entityPM.CasualImporterAddress1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterAddress2))
            {
				entityPOCO.CasualImporterAddress2 = entityPM.CasualImporterAddress2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterCity))
            {
				entityPOCO.CasualImporterCity = entityPM.CasualImporterCity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterZipCode))
            {
				entityPOCO.CasualImporterZipCode = entityPM.CasualImporterZipCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterFax))
            {
				entityPOCO.CasualImporterFax = entityPM.CasualImporterFax;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterEmail))
            {
				entityPOCO.CasualImporterEmail = entityPM.CasualImporterEmail;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterTel))
            {
				entityPOCO.CasualImporterTel = entityPM.CasualImporterTel;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterContact))
            {
				entityPOCO.CasualImporterContact = entityPM.CasualImporterContact;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemsProcessTypesList))
            {
				entityPOCO.ItemsProcessTypesList = entityPM.ItemsProcessTypesList;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClose))
            {
				entityPOCO.IsClose = entityPM.IsClose;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierSuspentionCode))
            {
				entityPOCO.CourierSuspentionCode = entityPM.CourierSuspentionCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepositionStatusCode))
            {
				entityPOCO.DepositionStatusCode = entityPM.DepositionStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPaymentProtested))
            {
				entityPOCO.IsPaymentProtested = entityPM.IsPaymentProtested;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentRequestNumber))
            {
				entityPOCO.AmendmentRequestNumber = entityPM.AmendmentRequestNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentStatus))
            {
				entityPOCO.AmendmentStatus = entityPM.AmendmentStatus;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentissueDate))
            {
				entityPOCO.AmendmentissueDate = entityPM.AmendmentissueDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentRemarks))
            {
				entityPOCO.AmendmentRemarks = entityPM.AmendmentRemarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentDeficitInitiated))
            {
				entityPOCO.AmendmentDeficitInitiated = entityPM.AmendmentDeficitInitiated;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendDeficitInitiatedReasTo))
            {
				entityPOCO.AmendDeficitInitiatedReasTo = entityPM.AmendDeficitInitiatedReasTo;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentCorrectedByUserId))
            {
				entityPOCO.AmendmentCorrectedByUserId = entityPM.AmendmentCorrectedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentRejectionReason))
            {
				entityPOCO.AmendmentRejectionReason = entityPM.AmendmentRejectionReason;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAmendment))
            {
				entityPOCO.IsAmendment = entityPM.IsAmendment;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentOriginalDeclartation))
            {
				entityPOCO.AmendmentOriginalDeclartation = entityPM.AmendmentOriginalDeclartation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDiamondDeclaration))
            {
				entityPOCO.IsDiamondDeclaration = entityPM.IsDiamondDeclaration;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentDontDisplayInList))
            {
				entityPOCO.AmendmentDontDisplayInList = entityPM.AmendmentDontDisplayInList;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMissMandatoryDiamond))
            {
				entityPOCO.IsMissMandatoryDiamond = entityPM.IsMissMandatoryDiamond;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsValidTicketsDiamond))
            {
				entityPOCO.IsValidTicketsDiamond = entityPM.IsValidTicketsDiamond;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AvailabilityDate))
            {
				entityPOCO.AvailabilityDate = entityPM.AvailabilityDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LoadingDateTime))
            {
				entityPOCO.LoadingDateTime = entityPM.LoadingDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipCode))
            {
				entityPOCO.ShipCode = entityPM.ShipCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExporterConfirmation))
            {
				entityPOCO.IsExporterConfirmation = entityPM.IsExporterConfirmation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Direction))
            {
				entityPOCO.Direction = entityPM.Direction;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentRoleCode))
            {
				entityPOCO.AgentRoleCode = entityPM.AgentRoleCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportFile))
            {
				entityPOCO.ExportFile = entityPM.ExportFile;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationCountryCode))
            {
				entityPOCO.DestinationCountryCode = entityPM.DestinationCountryCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportAutonomyRegionTypeCode))
            {
				entityPOCO.ExportAutonomyRegionTypeCode = entityPM.ExportAutonomyRegionTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationTypeCode))
            {
				entityPOCO.DeclarationTypeCode = entityPM.DeclarationTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelRequestReasonCode))
            {
				entityPOCO.CancelRequestReasonCode = entityPM.CancelRequestReasonCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelRequestReasonExplanation))
            {
				entityPOCO.CancelRequestReasonExplanation = entityPM.CancelRequestReasonExplanation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelRequestNumber))
            {
				entityPOCO.CancelRequestNumber = entityPM.CancelRequestNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomCancelRequestRemarks))
            {
				entityPOCO.CustomCancelRequestRemarks = entityPM.CustomCancelRequestRemarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelRequestStatusCode))
            {
				entityPOCO.CancelRequestStatusCode = entityPM.CancelRequestStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelRequestRejectionReason))
            {
				entityPOCO.CancelRequestRejectionReason = entityPM.CancelRequestRejectionReason;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelRequestApproveDate))
            {
				entityPOCO.CancelRequestApproveDate = entityPM.CancelRequestApproveDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClaimable))
            {
				entityPOCO.IsClaimable = entityPM.IsClaimable;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReplacingRepairRequest))
            {
				entityPOCO.ReplacingRepairRequest = entityPM.ReplacingRepairRequest;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentErrorXml))
            {
				entityPOCO.AmendmentErrorXml = entityPM.AmendmentErrorXml;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FOBValueNIS))
            {
				entityPOCO.FOBValueNIS = entityPM.FOBValueNIS;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FOBValueDollar))
            {
				entityPOCO.FOBValueDollar = entityPM.FOBValueDollar;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransshipmentApprovalDateTime))
            {
				entityPOCO.TransshipmentApprovalDateTime = entityPM.TransshipmentApprovalDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalLoadingSite))
            {
				entityPOCO.FinalLoadingSite = entityPM.FinalLoadingSite;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PalestinianCode))
            {
				entityPOCO.PalestinianCode = entityPM.PalestinianCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestedCustomsDocId))
            {
				entityPOCO.RequestedCustomsDocId = entityPM.RequestedCustomsDocId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportDeclarationOfficeCode))
            {
				entityPOCO.ExportDeclarationOfficeCode = entityPM.ExportDeclarationOfficeCode;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(DeclarationPM entityPM, Declaration entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomFileNo))
            {
					entityPM.CustomFileNo = entityPOCO.CustomFileNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerId))
            {
					entityPM.CustomerId = entityPOCO.CustomerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterId))
            {
					entityPM.ImporterId = entityPOCO.ImporterId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationNumber))
            {
					entityPM.DeclarationNumber = entityPOCO.DeclarationNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VersionId))
            {
					entityPM.VersionId = entityPOCO.VersionId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExternalDeclarationNumber))
            {
					entityPM.ExternalDeclarationNumber = entityPOCO.ExternalDeclarationNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationOfficeCode))
            {
					entityPM.DeclarationOfficeCode = entityPOCO.DeclarationOfficeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TaxationDateTime))
            {
					entityPM.TaxationDateTime = entityPOCO.TaxationDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AgentId))
            {
					entityPM.AgentId = entityPOCO.AgentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProcedureCurrentCode))
            {
					entityPM.ProcedureCurrentCode = entityPOCO.ProcedureCurrentCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AutonomyRegionTypeCode))
            {
					entityPM.AutonomyRegionTypeCode = entityPOCO.AutonomyRegionTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterPassCountryCode))
            {
					entityPM.ImporterPassCountryCode = entityPOCO.ImporterPassCountryCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransferImporterId))
            {
					entityPM.TransferImporterId = entityPOCO.TransferImporterId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransferImporterCountryCode))
            {
					entityPM.TransferImporterCountryCode = entityPOCO.TransferImporterCountryCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntitleImporterId))
            {
					entityPM.EntitleImporterId = entityPOCO.EntitleImporterId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterEntitlementTypeCode))
            {
					entityPM.ImporterEntitlementTypeCode = entityPOCO.ImporterEntitlementTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntitleImporterCountryCode))
            {
					entityPM.EntitleImporterCountryCode = entityPOCO.EntitleImporterCountryCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationDocumentId))
            {
					entityPM.DeclarationDocumentId = entityPOCO.DeclarationDocumentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationDocumentTypeCode))
            {
					entityPM.DeclarationDocumentTypeCode = entityPOCO.DeclarationDocumentTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsChanged))
            {
					entityPM.IsChanged = entityPOCO.IsChanged;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentDate))
            {
					entityPM.PaymentDate = entityPOCO.PaymentDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HatraDate))
            {
					entityPM.HatraDate = entityPOCO.HatraDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationStatusTypeCode))
            {
					entityPM.DeclarationStatusTypeCode = entityPOCO.DeclarationStatusTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LoadingFactor))
            {
					entityPM.LoadingFactor = entityPOCO.LoadingFactor;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DealValue))
            {
					entityPM.DealValue = entityPOCO.DealValue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CIFValue))
            {
					entityPM.CIFValue = entityPOCO.CIFValue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalTax))
            {
					entityPM.TotalTax = entityPOCO.TotalTax;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FileState))
            {
					entityPM.FileState = entityPOCO.FileState;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransportModeId))
            {
					entityPM.TransportModeId = entityPOCO.TransportModeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ErrosXml))
            {
					entityPM.ErrosXml = entityPOCO.ErrosXml;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterName))
            {
					entityPM.ImporterName = entityPOCO.ImporterName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DepartmentId))
            {
					entityPM.DepartmentId = entityPOCO.DepartmentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReferentUserId))
            {
					entityPM.ReferentUserId = entityPOCO.ReferentUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StorageSiteCode))
            {
					entityPM.StorageSiteCode = entityPOCO.StorageSiteCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PlatformFee))
            {
					entityPM.PlatformFee = entityPOCO.PlatformFee;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDateTime))
            {
					entityPM.CreateDateTime = entityPOCO.CreateDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDateTime))
            {
					entityPM.UpdateDateTime = entityPOCO.UpdateDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCancelled))
            {
					entityPM.IsCancelled = entityPOCO.IsCancelled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntitleImporterName))
            {
					entityPM.EntitleImporterName = entityPOCO.EntitleImporterName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransferImporterName))
            {
					entityPM.TransferImporterName = entityPOCO.TransferImporterName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DealValueWithoutFactor))
            {
					entityPM.DealValueWithoutFactor = entityPOCO.DealValueWithoutFactor;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterCode))
            {
					entityPM.ImporterCode = entityPOCO.ImporterCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransferImporterCode))
            {
					entityPM.TransferImporterCode = entityPOCO.TransferImporterCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntitleImporterCode))
            {
					entityPM.EntitleImporterCode = entityPOCO.EntitleImporterCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConcurrencyGUID))
            {
					entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterTypeCode))
            {
					entityPM.ImporterTypeCode = entityPOCO.ImporterTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransferImporterTypeCode))
            {
					entityPM.TransferImporterTypeCode = entityPOCO.TransferImporterTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntitleImporterTypeCode))
            {
					entityPM.EntitleImporterTypeCode = entityPOCO.EntitleImporterTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UserNotes))
            {
					entityPM.UserNotes = entityPOCO.UserNotes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HasConstraint))
            {
					entityPM.HasConstraint = entityPOCO.HasConstraint;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PrimaryInvoiceCounterKey))
            {
					entityPM.PrimaryInvoiceCounterKey = entityPOCO.PrimaryInvoiceCounterKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentOrderNumber))
            {
					entityPM.PaymentOrderNumber = entityPOCO.PaymentOrderNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentStatusCode))
            {
					entityPM.PaymentStatusCode = entityPOCO.PaymentStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsSignedVersion))
            {
					entityPM.IsSignedVersion = entityPOCO.IsSignedVersion;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SignedByUserId))
            {
					entityPM.SignedByUserId = entityPOCO.SignedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StorageSiteName))
            {
					entityPM.StorageSiteName = entityPOCO.StorageSiteName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SignerPersonalId))
            {
					entityPM.SignerPersonalId = entityPOCO.SignerPersonalId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsConvertedDeclaration))
            {
					entityPM.IsConvertedDeclaration = entityPOCO.IsConvertedDeclaration;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CorrectionsXml))
            {
					entityPM.CorrectionsXml = entityPOCO.CorrectionsXml;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsReleaseFile))
            {
					entityPM.IsReleaseFile = entityPOCO.IsReleaseFile;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsConnectedToUnifreight))
            {
					entityPM.IsConnectedToUnifreight = entityPOCO.IsConnectedToUnifreight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainImporterEntitlemntTypeCode))
            {
					entityPM.MainImporterEntitlemntTypeCode = entityPOCO.MainImporterEntitlemntTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransImporterEntitleTypeCode))
            {
					entityPM.TransImporterEntitleTypeCode = entityPOCO.TransImporterEntitleTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterAddress))
            {
					entityPM.ImporterAddress = entityPOCO.ImporterAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransferImporterAddress))
            {
					entityPM.TransferImporterAddress = entityPOCO.TransferImporterAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntitleImporterAddress))
            {
					entityPM.EntitleImporterAddress = entityPOCO.EntitleImporterAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterPassportNumber))
            {
					entityPM.ImporterPassportNumber = entityPOCO.ImporterPassportNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransferPassportNumber))
            {
					entityPM.TransferPassportNumber = entityPOCO.TransferPassportNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntitlePassportNumber))
            {
					entityPM.EntitlePassportNumber = entityPOCO.EntitlePassportNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StorageStatusCode))
            {
					entityPM.StorageStatusCode = entityPOCO.StorageStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualSupplierName))
            {
					entityPM.CasualSupplierName = entityPOCO.CasualSupplierName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualSupplierAddress))
            {
					entityPM.CasualSupplierAddress = entityPOCO.CasualSupplierAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCourierDeclaration))
            {
					entityPM.IsCourierDeclaration = entityPOCO.IsCourierDeclaration;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ManifestCargoStatusCode))
            {
					entityPM.ManifestCargoStatusCode = entityPOCO.ManifestCargoStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ManifestErrorXml))
            {
					entityPM.ManifestErrorXml = entityPOCO.ManifestErrorXml;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CourierHAWB))
            {
					entityPM.CourierHAWB = entityPOCO.CourierHAWB;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExcludeConsignment))
            {
					entityPM.ExcludeConsignment = entityPOCO.ExcludeConsignment;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CourierCustomStatusCode))
            {
					entityPM.CourierCustomStatusCode = entityPOCO.CourierCustomStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CourierSuspentionReasonCode))
            {
					entityPM.CourierSuspentionReasonCode = entityPOCO.CourierSuspentionReasonCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CourierReleaseStatusCode))
            {
					entityPM.CourierReleaseStatusCode = entityPOCO.CourierReleaseStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CourierHataraStatusCode))
            {
					entityPM.CourierHataraStatusCode = entityPOCO.CourierHataraStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DealValueWithFactor))
            {
					entityPM.DealValueWithFactor = entityPOCO.DealValueWithFactor;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsValueForCustomsOnly))
            {
					entityPM.IsValueForCustomsOnly = entityPOCO.IsValueForCustomsOnly;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WeightValue))
            {
					entityPM.WeightValue = entityPOCO.WeightValue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CourierSearchFields))
            {
					entityPM.CourierSearchFields = entityPOCO.CourierSearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AcceptanceStatusCode))
            {
					entityPM.AcceptanceStatusCode = entityPOCO.AcceptanceStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterAddress1))
            {
					entityPM.CasualImporterAddress1 = entityPOCO.CasualImporterAddress1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterAddress2))
            {
					entityPM.CasualImporterAddress2 = entityPOCO.CasualImporterAddress2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterCity))
            {
					entityPM.CasualImporterCity = entityPOCO.CasualImporterCity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterZipCode))
            {
					entityPM.CasualImporterZipCode = entityPOCO.CasualImporterZipCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterFax))
            {
					entityPM.CasualImporterFax = entityPOCO.CasualImporterFax;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterEmail))
            {
					entityPM.CasualImporterEmail = entityPOCO.CasualImporterEmail;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterTel))
            {
					entityPM.CasualImporterTel = entityPOCO.CasualImporterTel;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterContact))
            {
					entityPM.CasualImporterContact = entityPOCO.CasualImporterContact;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ItemsProcessTypesList))
            {
					entityPM.ItemsProcessTypesList = entityPOCO.ItemsProcessTypesList;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClose))
            {
					entityPM.IsClose = entityPOCO.IsClose;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CourierSuspentionCode))
            {
					entityPM.CourierSuspentionCode = entityPOCO.CourierSuspentionCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DepositionStatusCode))
            {
					entityPM.DepositionStatusCode = entityPOCO.DepositionStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsPaymentProtested))
            {
					entityPM.IsPaymentProtested = entityPOCO.IsPaymentProtested;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AmendmentRequestNumber))
            {
					entityPM.AmendmentRequestNumber = entityPOCO.AmendmentRequestNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AmendmentStatus))
            {
					entityPM.AmendmentStatus = entityPOCO.AmendmentStatus;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AmendmentissueDate))
            {
					entityPM.AmendmentissueDate = entityPOCO.AmendmentissueDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AmendmentRemarks))
            {
					entityPM.AmendmentRemarks = entityPOCO.AmendmentRemarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AmendmentDeficitInitiated))
            {
					entityPM.AmendmentDeficitInitiated = entityPOCO.AmendmentDeficitInitiated;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AmendDeficitInitiatedReasTo))
            {
					entityPM.AmendDeficitInitiatedReasTo = entityPOCO.AmendDeficitInitiatedReasTo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AmendmentCorrectedByUserId))
            {
					entityPM.AmendmentCorrectedByUserId = entityPOCO.AmendmentCorrectedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AmendmentRejectionReason))
            {
					entityPM.AmendmentRejectionReason = entityPOCO.AmendmentRejectionReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsAmendment))
            {
					entityPM.IsAmendment = entityPOCO.IsAmendment;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AmendmentOriginalDeclartation))
            {
					entityPM.AmendmentOriginalDeclartation = entityPOCO.AmendmentOriginalDeclartation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDiamondDeclaration))
            {
					entityPM.IsDiamondDeclaration = entityPOCO.IsDiamondDeclaration;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AmendmentDontDisplayInList))
            {
					entityPM.AmendmentDontDisplayInList = entityPOCO.AmendmentDontDisplayInList;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsMissMandatoryDiamond))
            {
					entityPM.IsMissMandatoryDiamond = entityPOCO.IsMissMandatoryDiamond;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsValidTicketsDiamond))
            {
					entityPM.IsValidTicketsDiamond = entityPOCO.IsValidTicketsDiamond;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AvailabilityDate))
            {
					entityPM.AvailabilityDate = entityPOCO.AvailabilityDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LoadingDateTime))
            {
					entityPM.LoadingDateTime = entityPOCO.LoadingDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipCode))
            {
					entityPM.ShipCode = entityPOCO.ShipCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsExporterConfirmation))
            {
					entityPM.IsExporterConfirmation = entityPOCO.IsExporterConfirmation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Direction))
            {
					entityPM.Direction = entityPOCO.Direction;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AgentRoleCode))
            {
					entityPM.AgentRoleCode = entityPOCO.AgentRoleCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExportFile))
            {
					entityPM.ExportFile = entityPOCO.ExportFile;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DestinationCountryCode))
            {
					entityPM.DestinationCountryCode = entityPOCO.DestinationCountryCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExportAutonomyRegionTypeCode))
            {
					entityPM.ExportAutonomyRegionTypeCode = entityPOCO.ExportAutonomyRegionTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationTypeCode))
            {
					entityPM.DeclarationTypeCode = entityPOCO.DeclarationTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CancelRequestReasonCode))
            {
					entityPM.CancelRequestReasonCode = entityPOCO.CancelRequestReasonCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CancelRequestReasonExplanation))
            {
					entityPM.CancelRequestReasonExplanation = entityPOCO.CancelRequestReasonExplanation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CancelRequestNumber))
            {
					entityPM.CancelRequestNumber = entityPOCO.CancelRequestNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomCancelRequestRemarks))
            {
					entityPM.CustomCancelRequestRemarks = entityPOCO.CustomCancelRequestRemarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CancelRequestStatusCode))
            {
					entityPM.CancelRequestStatusCode = entityPOCO.CancelRequestStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CancelRequestRejectionReason))
            {
					entityPM.CancelRequestRejectionReason = entityPOCO.CancelRequestRejectionReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CancelRequestApproveDate))
            {
					entityPM.CancelRequestApproveDate = entityPOCO.CancelRequestApproveDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClaimable))
            {
					entityPM.IsClaimable = entityPOCO.IsClaimable;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReplacingRepairRequest))
            {
					entityPM.ReplacingRepairRequest = entityPOCO.ReplacingRepairRequest;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AmendmentErrorXml))
            {
					entityPM.AmendmentErrorXml = entityPOCO.AmendmentErrorXml;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FOBValueNIS))
            {
					entityPM.FOBValueNIS = entityPOCO.FOBValueNIS;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FOBValueDollar))
            {
					entityPM.FOBValueDollar = entityPOCO.FOBValueDollar;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransshipmentApprovalDateTime))
            {
					entityPM.TransshipmentApprovalDateTime = entityPOCO.TransshipmentApprovalDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FinalLoadingSite))
            {
					entityPM.FinalLoadingSite = entityPOCO.FinalLoadingSite;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PalestinianCode))
            {
					entityPM.PalestinianCode = entityPOCO.PalestinianCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestedCustomsDocId))
            {
					entityPM.RequestedCustomsDocId = entityPOCO.RequestedCustomsDocId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExportDeclarationOfficeCode))
            {
					entityPM.ExportDeclarationOfficeCode = entityPOCO.ExportDeclarationOfficeCode;
            }

		}

		public void PMToOldPM(DeclarationPM entityPM, DeclarationPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomFileNo))
            {
                oldEntityPM.CustomFileNo = entityPM.CustomFileNo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
                oldEntityPM.CustomerId = entityPM.CustomerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterId))
            {
                oldEntityPM.ImporterId = entityPM.ImporterId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationNumber))
            {
                oldEntityPM.DeclarationNumber = entityPM.DeclarationNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VersionId))
            {
                oldEntityPM.VersionId = entityPM.VersionId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExternalDeclarationNumber))
            {
                oldEntityPM.ExternalDeclarationNumber = entityPM.ExternalDeclarationNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationOfficeCode))
            {
                oldEntityPM.DeclarationOfficeCode = entityPM.DeclarationOfficeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TaxationDateTime))
            {
                oldEntityPM.TaxationDateTime = entityPM.TaxationDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentId))
            {
                oldEntityPM.AgentId = entityPM.AgentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProcedureCurrentCode))
            {
                oldEntityPM.ProcedureCurrentCode = entityPM.ProcedureCurrentCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutonomyRegionTypeCode))
            {
                oldEntityPM.AutonomyRegionTypeCode = entityPM.AutonomyRegionTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterPassCountryCode))
            {
                oldEntityPM.ImporterPassCountryCode = entityPM.ImporterPassCountryCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferImporterId))
            {
                oldEntityPM.TransferImporterId = entityPM.TransferImporterId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferImporterCountryCode))
            {
                oldEntityPM.TransferImporterCountryCode = entityPM.TransferImporterCountryCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitleImporterId))
            {
                oldEntityPM.EntitleImporterId = entityPM.EntitleImporterId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterEntitlementTypeCode))
            {
                oldEntityPM.ImporterEntitlementTypeCode = entityPM.ImporterEntitlementTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitleImporterCountryCode))
            {
                oldEntityPM.EntitleImporterCountryCode = entityPM.EntitleImporterCountryCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationDocumentId))
            {
                oldEntityPM.DeclarationDocumentId = entityPM.DeclarationDocumentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationDocumentTypeCode))
            {
                oldEntityPM.DeclarationDocumentTypeCode = entityPM.DeclarationDocumentTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsChanged))
            {
                oldEntityPM.IsChanged = entityPM.IsChanged;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentDate))
            {
                oldEntityPM.PaymentDate = entityPM.PaymentDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HatraDate))
            {
                oldEntityPM.HatraDate = entityPM.HatraDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationStatusTypeCode))
            {
                oldEntityPM.DeclarationStatusTypeCode = entityPM.DeclarationStatusTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LoadingFactor))
            {
                oldEntityPM.LoadingFactor = entityPM.LoadingFactor;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DealValue))
            {
                oldEntityPM.DealValue = entityPM.DealValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CIFValue))
            {
                oldEntityPM.CIFValue = entityPM.CIFValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalTax))
            {
                oldEntityPM.TotalTax = entityPM.TotalTax;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FileState))
            {
                oldEntityPM.FileState = entityPM.FileState;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
                oldEntityPM.TransportModeId = entityPM.TransportModeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ErrosXml))
            {
                oldEntityPM.ErrosXml = entityPM.ErrosXml;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterName))
            {
                oldEntityPM.ImporterName = entityPM.ImporterName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepartmentId))
            {
                oldEntityPM.DepartmentId = entityPM.DepartmentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReferentUserId))
            {
                oldEntityPM.ReferentUserId = entityPM.ReferentUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageSiteCode))
            {
                oldEntityPM.StorageSiteCode = entityPM.StorageSiteCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PlatformFee))
            {
                oldEntityPM.PlatformFee = entityPM.PlatformFee;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDateTime))
            {
                oldEntityPM.CreateDateTime = entityPM.CreateDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDateTime))
            {
                oldEntityPM.UpdateDateTime = entityPM.UpdateDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
                oldEntityPM.IsCancelled = entityPM.IsCancelled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitleImporterName))
            {
                oldEntityPM.EntitleImporterName = entityPM.EntitleImporterName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferImporterName))
            {
                oldEntityPM.TransferImporterName = entityPM.TransferImporterName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DealValueWithoutFactor))
            {
                oldEntityPM.DealValueWithoutFactor = entityPM.DealValueWithoutFactor;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterCode))
            {
                oldEntityPM.ImporterCode = entityPM.ImporterCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferImporterCode))
            {
                oldEntityPM.TransferImporterCode = entityPM.TransferImporterCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitleImporterCode))
            {
                oldEntityPM.EntitleImporterCode = entityPM.EntitleImporterCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
                oldEntityPM.ConcurrencyGUID = entityPM.ConcurrencyGUID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterTypeCode))
            {
                oldEntityPM.ImporterTypeCode = entityPM.ImporterTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferImporterTypeCode))
            {
                oldEntityPM.TransferImporterTypeCode = entityPM.TransferImporterTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitleImporterTypeCode))
            {
                oldEntityPM.EntitleImporterTypeCode = entityPM.EntitleImporterTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserNotes))
            {
                oldEntityPM.UserNotes = entityPM.UserNotes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HasConstraint))
            {
                oldEntityPM.HasConstraint = entityPM.HasConstraint;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrimaryInvoiceCounterKey))
            {
                oldEntityPM.PrimaryInvoiceCounterKey = entityPM.PrimaryInvoiceCounterKey;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentOrderNumber))
            {
                oldEntityPM.PaymentOrderNumber = entityPM.PaymentOrderNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentStatusCode))
            {
                oldEntityPM.PaymentStatusCode = entityPM.PaymentStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSignedVersion))
            {
                oldEntityPM.IsSignedVersion = entityPM.IsSignedVersion;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SignedByUserId))
            {
                oldEntityPM.SignedByUserId = entityPM.SignedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageSiteName))
            {
                oldEntityPM.StorageSiteName = entityPM.StorageSiteName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SignerPersonalId))
            {
                oldEntityPM.SignerPersonalId = entityPM.SignerPersonalId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConvertedDeclaration))
            {
                oldEntityPM.IsConvertedDeclaration = entityPM.IsConvertedDeclaration;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CorrectionsXml))
            {
                oldEntityPM.CorrectionsXml = entityPM.CorrectionsXml;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsReleaseFile))
            {
                oldEntityPM.IsReleaseFile = entityPM.IsReleaseFile;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsConnectedToUnifreight))
            {
                oldEntityPM.IsConnectedToUnifreight = entityPM.IsConnectedToUnifreight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainImporterEntitlemntTypeCode))
            {
                oldEntityPM.MainImporterEntitlemntTypeCode = entityPM.MainImporterEntitlemntTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransImporterEntitleTypeCode))
            {
                oldEntityPM.TransImporterEntitleTypeCode = entityPM.TransImporterEntitleTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterAddress))
            {
                oldEntityPM.ImporterAddress = entityPM.ImporterAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferImporterAddress))
            {
                oldEntityPM.TransferImporterAddress = entityPM.TransferImporterAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitleImporterAddress))
            {
                oldEntityPM.EntitleImporterAddress = entityPM.EntitleImporterAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterPassportNumber))
            {
                oldEntityPM.ImporterPassportNumber = entityPM.ImporterPassportNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransferPassportNumber))
            {
                oldEntityPM.TransferPassportNumber = entityPM.TransferPassportNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntitlePassportNumber))
            {
                oldEntityPM.EntitlePassportNumber = entityPM.EntitlePassportNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageStatusCode))
            {
                oldEntityPM.StorageStatusCode = entityPM.StorageStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualSupplierName))
            {
                oldEntityPM.CasualSupplierName = entityPM.CasualSupplierName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualSupplierAddress))
            {
                oldEntityPM.CasualSupplierAddress = entityPM.CasualSupplierAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCourierDeclaration))
            {
                oldEntityPM.IsCourierDeclaration = entityPM.IsCourierDeclaration;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManifestCargoStatusCode))
            {
                oldEntityPM.ManifestCargoStatusCode = entityPM.ManifestCargoStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ManifestErrorXml))
            {
                oldEntityPM.ManifestErrorXml = entityPM.ManifestErrorXml;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierHAWB))
            {
                oldEntityPM.CourierHAWB = entityPM.CourierHAWB;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExcludeConsignment))
            {
                oldEntityPM.ExcludeConsignment = entityPM.ExcludeConsignment;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierCustomStatusCode))
            {
                oldEntityPM.CourierCustomStatusCode = entityPM.CourierCustomStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierSuspentionReasonCode))
            {
                oldEntityPM.CourierSuspentionReasonCode = entityPM.CourierSuspentionReasonCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierReleaseStatusCode))
            {
                oldEntityPM.CourierReleaseStatusCode = entityPM.CourierReleaseStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierHataraStatusCode))
            {
                oldEntityPM.CourierHataraStatusCode = entityPM.CourierHataraStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DealValueWithFactor))
            {
                oldEntityPM.DealValueWithFactor = entityPM.DealValueWithFactor;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsValueForCustomsOnly))
            {
                oldEntityPM.IsValueForCustomsOnly = entityPM.IsValueForCustomsOnly;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WeightValue))
            {
                oldEntityPM.WeightValue = entityPM.WeightValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierSearchFields))
            {
                oldEntityPM.CourierSearchFields = entityPM.CourierSearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AcceptanceStatusCode))
            {
                oldEntityPM.AcceptanceStatusCode = entityPM.AcceptanceStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterAddress1))
            {
                oldEntityPM.CasualImporterAddress1 = entityPM.CasualImporterAddress1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterAddress2))
            {
                oldEntityPM.CasualImporterAddress2 = entityPM.CasualImporterAddress2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterCity))
            {
                oldEntityPM.CasualImporterCity = entityPM.CasualImporterCity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterZipCode))
            {
                oldEntityPM.CasualImporterZipCode = entityPM.CasualImporterZipCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterFax))
            {
                oldEntityPM.CasualImporterFax = entityPM.CasualImporterFax;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterEmail))
            {
                oldEntityPM.CasualImporterEmail = entityPM.CasualImporterEmail;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterTel))
            {
                oldEntityPM.CasualImporterTel = entityPM.CasualImporterTel;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterContact))
            {
                oldEntityPM.CasualImporterContact = entityPM.CasualImporterContact;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ItemsProcessTypesList))
            {
                oldEntityPM.ItemsProcessTypesList = entityPM.ItemsProcessTypesList;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClose))
            {
                oldEntityPM.IsClose = entityPM.IsClose;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CourierSuspentionCode))
            {
                oldEntityPM.CourierSuspentionCode = entityPM.CourierSuspentionCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepositionStatusCode))
            {
                oldEntityPM.DepositionStatusCode = entityPM.DepositionStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPaymentProtested))
            {
                oldEntityPM.IsPaymentProtested = entityPM.IsPaymentProtested;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentRequestNumber))
            {
                oldEntityPM.AmendmentRequestNumber = entityPM.AmendmentRequestNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentStatus))
            {
                oldEntityPM.AmendmentStatus = entityPM.AmendmentStatus;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentissueDate))
            {
                oldEntityPM.AmendmentissueDate = entityPM.AmendmentissueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentRemarks))
            {
                oldEntityPM.AmendmentRemarks = entityPM.AmendmentRemarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentDeficitInitiated))
            {
                oldEntityPM.AmendmentDeficitInitiated = entityPM.AmendmentDeficitInitiated;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendDeficitInitiatedReasTo))
            {
                oldEntityPM.AmendDeficitInitiatedReasTo = entityPM.AmendDeficitInitiatedReasTo;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentCorrectedByUserId))
            {
                oldEntityPM.AmendmentCorrectedByUserId = entityPM.AmendmentCorrectedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentRejectionReason))
            {
                oldEntityPM.AmendmentRejectionReason = entityPM.AmendmentRejectionReason;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAmendment))
            {
                oldEntityPM.IsAmendment = entityPM.IsAmendment;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentOriginalDeclartation))
            {
                oldEntityPM.AmendmentOriginalDeclartation = entityPM.AmendmentOriginalDeclartation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDiamondDeclaration))
            {
                oldEntityPM.IsDiamondDeclaration = entityPM.IsDiamondDeclaration;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentDontDisplayInList))
            {
                oldEntityPM.AmendmentDontDisplayInList = entityPM.AmendmentDontDisplayInList;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMissMandatoryDiamond))
            {
                oldEntityPM.IsMissMandatoryDiamond = entityPM.IsMissMandatoryDiamond;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsValidTicketsDiamond))
            {
                oldEntityPM.IsValidTicketsDiamond = entityPM.IsValidTicketsDiamond;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AvailabilityDate))
            {
                oldEntityPM.AvailabilityDate = entityPM.AvailabilityDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LoadingDateTime))
            {
                oldEntityPM.LoadingDateTime = entityPM.LoadingDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipCode))
            {
                oldEntityPM.ShipCode = entityPM.ShipCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExporterConfirmation))
            {
                oldEntityPM.IsExporterConfirmation = entityPM.IsExporterConfirmation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Direction))
            {
                oldEntityPM.Direction = entityPM.Direction;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentRoleCode))
            {
                oldEntityPM.AgentRoleCode = entityPM.AgentRoleCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportFile))
            {
                oldEntityPM.ExportFile = entityPM.ExportFile;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationCountryCode))
            {
                oldEntityPM.DestinationCountryCode = entityPM.DestinationCountryCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportAutonomyRegionTypeCode))
            {
                oldEntityPM.ExportAutonomyRegionTypeCode = entityPM.ExportAutonomyRegionTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationTypeCode))
            {
                oldEntityPM.DeclarationTypeCode = entityPM.DeclarationTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelRequestReasonCode))
            {
                oldEntityPM.CancelRequestReasonCode = entityPM.CancelRequestReasonCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelRequestReasonExplanation))
            {
                oldEntityPM.CancelRequestReasonExplanation = entityPM.CancelRequestReasonExplanation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelRequestNumber))
            {
                oldEntityPM.CancelRequestNumber = entityPM.CancelRequestNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomCancelRequestRemarks))
            {
                oldEntityPM.CustomCancelRequestRemarks = entityPM.CustomCancelRequestRemarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelRequestStatusCode))
            {
                oldEntityPM.CancelRequestStatusCode = entityPM.CancelRequestStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelRequestRejectionReason))
            {
                oldEntityPM.CancelRequestRejectionReason = entityPM.CancelRequestRejectionReason;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CancelRequestApproveDate))
            {
                oldEntityPM.CancelRequestApproveDate = entityPM.CancelRequestApproveDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClaimable))
            {
                oldEntityPM.IsClaimable = entityPM.IsClaimable;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReplacingRepairRequest))
            {
                oldEntityPM.ReplacingRepairRequest = entityPM.ReplacingRepairRequest;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AmendmentErrorXml))
            {
                oldEntityPM.AmendmentErrorXml = entityPM.AmendmentErrorXml;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FOBValueNIS))
            {
                oldEntityPM.FOBValueNIS = entityPM.FOBValueNIS;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FOBValueDollar))
            {
                oldEntityPM.FOBValueDollar = entityPM.FOBValueDollar;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransshipmentApprovalDateTime))
            {
                oldEntityPM.TransshipmentApprovalDateTime = entityPM.TransshipmentApprovalDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalLoadingSite))
            {
                oldEntityPM.FinalLoadingSite = entityPM.FinalLoadingSite;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PalestinianCode))
            {
                oldEntityPM.PalestinianCode = entityPM.PalestinianCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestedCustomsDocId))
            {
                oldEntityPM.RequestedCustomsDocId = entityPM.RequestedCustomsDocId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExportDeclarationOfficeCode))
            {
                oldEntityPM.ExportDeclarationOfficeCode = entityPM.ExportDeclarationOfficeCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DeclarationPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ErrosXml)) //T4 find type == nText 
            {
                entityPM.ErrosXml = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ErrosXml));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ImporterName)) //T4 find type == nText 
            {
                entityPM.ImporterName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ImporterName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.EntitleImporterName)) //T4 find type == nText 
            {
                entityPM.EntitleImporterName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.EntitleImporterName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.TransferImporterName)) //T4 find type == nText 
            {
                entityPM.TransferImporterName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.TransferImporterName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.UserNotes)) //T4 find type == nText 
            {
                entityPM.UserNotes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.UserNotes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.StorageSiteName)) //T4 find type == nText 
            {
                entityPM.StorageSiteName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.StorageSiteName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CorrectionsXml)) //T4 find type == nText 
            {
                entityPM.CorrectionsXml = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CorrectionsXml));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ImporterAddress)) //T4 find type == nText 
            {
                entityPM.ImporterAddress = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ImporterAddress));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.TransferImporterAddress)) //T4 find type == nText 
            {
                entityPM.TransferImporterAddress = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.TransferImporterAddress));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.EntitleImporterAddress)) //T4 find type == nText 
            {
                entityPM.EntitleImporterAddress = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.EntitleImporterAddress));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CasualSupplierName)) //T4 find type == nText 
            {
                entityPM.CasualSupplierName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CasualSupplierName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CasualSupplierAddress)) //T4 find type == nText 
            {
                entityPM.CasualSupplierAddress = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CasualSupplierAddress));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CourierSearchFields)) //T4 find type == nText 
            {
                entityPM.CourierSearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CourierSearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CasualImporterAddress1)) //T4 find type == nText 
            {
                entityPM.CasualImporterAddress1 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CasualImporterAddress1));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CasualImporterAddress2)) //T4 find type == nText 
            {
                entityPM.CasualImporterAddress2 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CasualImporterAddress2));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CasualImporterCity)) //T4 find type == nText 
            {
                entityPM.CasualImporterCity = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CasualImporterCity));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CasualImporterContact)) //T4 find type == nText 
            {
                entityPM.CasualImporterContact = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CasualImporterContact));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.AmendmentRemarks)) //T4 find type == nText 
            {
                entityPM.AmendmentRemarks = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.AmendmentRemarks));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.AmendDeficitInitiatedReasTo)) //T4 find type == nText 
            {
                entityPM.AmendDeficitInitiatedReasTo = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.AmendDeficitInitiatedReasTo));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CancelRequestReasonExplanation)) //T4 find type == nText 
            {
                entityPM.CancelRequestReasonExplanation = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CancelRequestReasonExplanation));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CustomCancelRequestRemarks)) //T4 find type == nText 
            {
                entityPM.CustomCancelRequestRemarks = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CustomCancelRequestRemarks));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CancelRequestRejectionReason)) //T4 find type == nText 
            {
                entityPM.CancelRequestRejectionReason = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CancelRequestRejectionReason));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.AmendmentErrorXml)) //T4 find type == nText 
            {
                entityPM.AmendmentErrorXml = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.AmendmentErrorXml));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.PalestinianCode)) //T4 find type == nText 
            {
                entityPM.PalestinianCode = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.PalestinianCode));
            }
            entityPM.EncodeBase64NVARCHARFieldsBy=null;
		}


	    public void AddPOCOPropertyName(POCOPropertyNames pocoPropertyName)
        {
            CustomMappedPOCOProperties.Add(pocoPropertyName);
        }

        public void AddPMPropertyName(PMPropertyNames pocoPropertyName)
        {
            CustomMappedPMProperties.Add(pocoPropertyName);
        }
		
		private void BuildSearchFieldsGenerated(DeclarationPM entityPM, Declaration entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 