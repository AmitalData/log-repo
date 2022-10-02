using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCustomsWindowsService.Tester
{
    public partial class Oracle2SQL
    {

        public void SCHEMA()
        {
            //find text  Schema="Customs"  C:\log2004\Logitude\Logitude.Customs.MetaData\DBTables\  
            string d =
//Code	File	Line	Column
@"            this.ToTable(""AcceptanceStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AcceptanceStatusMap.cs	22	6
			  this.ToTable(""AccumalationStates"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AccumalationStateMap.cs 22  6
			  this.ToTable(""ActionCodes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ActionCodeMap.cs    22  6
			  this.ToTable(""AddressContactStates"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AddressContactStateMap.cs   22  6
			  this.ToTable(""AddressPurposes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AddressPurposeMap.cs    22  6
			  this.ToTable(""AgentTalkBackTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AgentTalkBackTypeMap.cs 22  6
			  this.ToTable(""AmedmentTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AmedmentTypeMap.cs  22  6
			  this.ToTable(""AmendCancellRequestInitiators"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AmendCancellRequestInitiatorMap.cs  22  6
			  this.ToTable(""AmendmentFieldReasonTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AmendmentFieldReasonTypeMap.cs  22  6
			  this.ToTable(""AmendmentFieldStatusTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AmendmentFieldStatusTypeMap.cs  22  6
			  this.ToTable(""AmendmentRequestStatuses"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AmendmentRequestStatusMap.cs    22  6
			  this.ToTable(""AmendmentStatuses"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AmendmentStatusMap.cs   22  6
			  this.ToTable(""AmendmentTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AmendmentTypeMap.cs 22  6
			  this.ToTable(""AmendRequestRejectReasonTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AmendRequestRejectReasonTypeMap.cs  22  6
			  this.ToTable(""AmountTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AmountTypeMap.cs    22  6
			  this.ToTable(""ApprovedProfessions"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ApprovedProfessionMap.cs    22  6
			  this.ToTable(""AssigneeNotificationTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AssigneeNotificationTypeMap.cs  22  6
			  this.ToTable(""AttachmentTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AttachmentTypeMap.cs    22  6
			  this.ToTable(""Authorities"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AuthorityMap.cs 22  6
			  this.ToTable(""AuthorizedSignerPermits"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AuthorizedSignerPermitMap.cs    22  6
			  this.ToTable(""AutonomyRegionTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AutonomyRegionTypeMap.cs    22  6
			  this.ToTable(""AutonomyTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\AutonomyTypeMap.cs  22  6
			  this.ToTable(""Banks"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\BankMap.cs  22  6
			  this.ToTable(""BuyerRoleTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\BuyerRoleTypeMap.cs 22  6
			  this.ToTable(""CancellationReasonRequestTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CancellationReasonRequestTypeMap.cs 22  6
			  this.ToTable(""CancellationRequestStatuses"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CancellationRequestStatusMap.cs 22  6
			  this.ToTable(""CancelRequestRejectReasonTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CancelRequestRejectReasonTypeMap.cs 22  6
			  this.ToTable(""CargoIdentifireTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CargoIdentifireTypeMap.cs   22  6
			  this.ToTable(""CargoIdentityQualifiers"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CargoIdentityQualifierMap.cs    22  6
			  this.ToTable(""CargoSealIdentifiers"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CargoSealIdentifierMap.cs   22  6
			  this.ToTable(""CargoSeals"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CargoSealMap.cs 22  6
			  this.ToTable(""CargoSplitRequestStatuses"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CargoSplitRequestStatusMap.cs   22  6
			  this.ToTable(""CargoStatuses"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CargoStatusMap.cs   22  6
			  this.ToTable(""CargoTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CargoTypeMap.cs 22  6
			  this.ToTable(""CertificateExemptionTypes"", ""Customs""); C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CertificateExemptionTypeMap.cs	22	6
			  this.ToTable(""CertificatesStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CertificatesStatusMap.cs	22	6
			  this.ToTable(""CheckEntityTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CheckEntityTypeMap.cs	22	6
			  this.ToTable(""CheckEssenceLookups"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CheckEssenceLookupMap.cs	22	6
			  this.ToTable(""CheckQueueTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CheckQueueTypeMap.cs	22	6
			  this.ToTable(""CheckRepresentativeTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CheckRepresentativeTypeMap.cs	22	6
			  this.ToTable(""CheckTypeLookups"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CheckTypeLookupMap.cs	22	6
			  this.ToTable(""Cities"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CityMap.cs	22	6
			  this.ToTable(""ClaimEntities"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimEntityMap.cs	22	6
			  this.ToTable(""ClaimExplanationCodes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimExplanationCodeMap.cs	22	6
			  this.ToTable(""ClaimImporterDeclarsP3Lois"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimImporterDeclarsP3LoiMap.cs	22	6
			  this.ToTable(""ClaimImporterDeclarsPage3As"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimImporterDeclarsPage3AMap.cs	22	6
			  this.ToTable(""ClaimImporterDeclarsPage3Bs"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimImporterDeclarsPage3BMap.cs	22	6
			  this.ToTable(""ClaimImporterDeclarsPage3s"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimImporterDeclarsPage3Map.cs	22	6
			  this.ToTable(""Claims"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimMap.cs	22	6
			  this.ToTable(""ClaimReasonTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimReasonTypeMap.cs	22	6
			  this.ToTable(""ClaimsRelatedEntitiesAmounts"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimsRelatedEntitiesAmountMap.cs	22	6
			  this.ToTable(""ClaimsRelatedEntitiesReasons"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimsRelatedEntitiesReasonMap.cs	22	6
			  this.ToTable(""ClaimsRelatedEntitiesRefunds"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimsRelatedEntitiesRefundMap.cs	22	6
			  this.ToTable(""ClaimsRelatedEntitiesSeizures"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimsRelatedEntitiesSeizureMap.cs	22	6
			  this.ToTable(""ClaimsRelatedEntities"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimsRelatedEntityMap.cs	22	6
			  this.ToTable(""ClaimsRelatedEntsExpDeclars"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimsRelatedEntsExpDeclarMap.cs	22	6
			  this.ToTable(""ClaimsRelatedEntsReasonsExps"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClaimsRelatedEntsReasonsExpMap.cs	22	6
			  this.ToTable(""ClassificationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClassificationTypeMap.cs	22	6
			  this.ToTable(""ClientAddresses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClientAddressMap.cs	22	6
			  this.ToTable(""ClientDrivingLicenses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClientDrivingLicenseMap.cs	22	6
			  this.ToTable(""ClientDrivingLicenseTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClientDrivingLicenseTypeMap.cs	22	6
			  this.ToTable(""Clients"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClientMap.cs	22	6
			  this.ToTable(""ClientsAddressCommTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClientsAddressCommTypeMap.cs	22	6
			  this.ToTable(""ClientsPoas"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClientsPoaMap.cs	22	6
			  this.ToTable(""ClosedTableStatus"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ClosedTableStatusMap.cs	22	6
			  this.ToTable(""CollateralAnswerStatus"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CollateralAnswerStatusMap.cs	22	6
			  this.ToTable(""CollateralAnswerTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CollateralAnswerTypeMap.cs	22	6
			  this.ToTable(""CollateralRequestStatus"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CollateralRequestStatusMap.cs	22	6
			  this.ToTable(""CollateralsRequestFileConds"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CollateralsRequestFileCondMap.cs	22	6
			  this.ToTable(""CollateralTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CollateralTypeMap.cs	22	6
			  this.ToTable(""CommercialSales"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CommercialSaleMap.cs	22	6
			  this.ToTable(""CommunicationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CommunicationTypeMap.cs	22	6
			  this.ToTable(""ConfirmationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ConfirmationTypeMap.cs	22	6
			  this.ToTable(""ConsignmentInternalTransitions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ConsignmentInternalTransitionMap.cs	22	6
			  this.ToTable(""Consignments"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ConsignmentMap.cs	22	6
			  this.ToTable(""ConsignmentPackages"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ConsignmentPackageMap.cs	22	6
			  this.ToTable(""ConsignmentPackDangers"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ConsignmentPackDangerMap.cs	22	6
			  this.ToTable(""ConstraintApprovalDecisions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ConstraintApprovalDecisionMap.cs	22	6
			  this.ToTable(""ConstraintProcessTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ConstraintProcessTypeMap.cs	22	6
			  this.ToTable(""ConstraintStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ConstraintStatusMap.cs	22	6
			  this.ToTable(""ConstraintTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ConstraintTypeMap.cs	22	6
			  this.ToTable(""ContactRoleTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ContactRoleTypeMap.cs	22	6
			  this.ToTable(""ContainerizationHataraStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ContainerizationHataraStatusMap.cs	22	6
			  this.ToTable(""Containerizations"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ContainerizationMap.cs	22	6
			  this.ToTable(""ContainerizationStatusCodes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ContainerizationStatusCodeMap.cs	22	6
			  this.ToTable(""ContainerTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ContainerTypeMap.cs	22	6
			  this.ToTable(""ContinuousMessagesTypeCodes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ContinuousMessagesTypeCodeMap.cs	22	6
			  this.ToTable(""ContinuousRequestTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ContinuousRequestTypeMap.cs	22	6
			  this.ToTable(""ConverterTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ConverterTypeMap.cs	22	6
			  this.ToTable(""CoolingReportingMethods"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CoolingReportingMethodMap.cs	22	6
			  this.ToTable(""CountryGroups"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CountryGroupMap.cs	22	6
			  this.ToTable(""CourierCustomStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CourierCustomStatusMap.cs	22	6
			  this.ToTable(""CourierDeclarations"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CourierDeclarationMap.cs	22	6
			  this.ToTable(""CourierDeclarationStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CourierDeclarationStatusMap.cs	22	6
			  this.ToTable(""CourierManifestStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CourierManifestStatusMap.cs	22	6
			  this.ToTable(""CourierMasters"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CourierMasterMap.cs	22	6
			  this.ToTable(""CourierPaymentStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CourierPaymentStatusMap.cs	22	6
			  this.ToTable(""CourierPendingReasons"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CourierPendingReasonMap.cs	22	6
			  this.ToTable(""CourierStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CourierStatusMap.cs	22	6
			  this.ToTable(""CouriersVats"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CouriersVatMap.cs	22	6
			  this.ToTable(""CourtInstances"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CourtInstanceMap.cs	22	6
			  this.ToTable(""CurrencyTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CurrencyTypeMap.cs	22	6
			  this.ToTable(""CurrencyTypeTenants"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CurrencyTypeTenantMap.cs	22	6
			  this.ToTable(""CustomBanks"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomBankMap.cs	22	6
			  this.ToTable(""CustomBanksCards"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomBanksCardMap.cs	22	6
			  this.ToTable(""CustomDocumentTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomDocumentTypeMap.cs	22	6
			  this.ToTable(""CustomDocumentTypeMetaData"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomDocumentTypeMetaDataMap.cs	22	6
			  this.ToTable(""CustomerActivityTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomerActivityTypeMap.cs	22	6
			  this.ToTable(""CustomerClassificationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomerClassificationTypeMap.cs	22	6
			  this.ToTable(""CustomerIdentificationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomerIdentificationTypeMap.cs	22	6
			  this.ToTable(""CustomerIdentifyTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomerIdentifyTypeMap.cs	22	6
			  this.ToTable(""CustomerIndicationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomerIndicationTypeMap.cs	22	6
			  this.ToTable(""CustomerRoleTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomerRoleTypeMap.cs	22	6
			  this.ToTable(""CustomerTypeGenerals"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomerTypeGeneralMap.cs	22	6
			  this.ToTable(""CustomMetaDataTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomMetaDataTypeMap.cs	22	6
			  this.ToTable(""CustomsAddressTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsAddressTypeMap.cs	22	6
			  this.ToTable(""CustomsAirlines"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsAirlineMap.cs	22	6
			  this.ToTable(""CustomsAutonomyKeywords"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsAutonomyKeywordMap.cs	22	6
			  this.ToTable(""CustomsBooks"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsBookMap.cs	22	6
			  this.ToTable(""CustomsBookTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsBookTypeMap.cs	22	6
			  this.ToTable(""CustomsBranches"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsBranchMap.cs	22	6
			  this.ToTable(""CustomsClosedTables"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsClosedTableMap.cs	22	6
			  this.ToTable(""CustomsCollaterals"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsCollateralMap.cs	22	6
			  this.ToTable(""CustomsCollateralsAnswers"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsCollateralsAnswerMap.cs	22	6
			  this.ToTable(""CustomsCollateralsConditions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsCollateralsConditionMap.cs	22	6
			  this.ToTable(""CustomsCountries"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsCountryMap.cs	22	6
			  this.ToTable(""CustomsDocuments"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsDocumentMap.cs	22	6
			  this.ToTable(""CustomsDocumentMetaDataValues"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsDocumentMetaDataValueMap.cs	22	6
			  this.ToTable(""CustomsDocumentPointers"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsDocumentPointerMap.cs	22	6
			  this.ToTable(""CustomsDocumentsDefinitions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsDocumentsDefinitionMap.cs	22	6
			  this.ToTable(""CustomsDocumentStatusTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsDocumentStatusTypeMap.cs	22	6
			  this.ToTable(""CustomsDocumentsTickets"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsDocumentsTicketMap.cs	22	6
			  this.ToTable(""CustomsDocumentUploads"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsDocumentUploadMap.cs	22	6
				this.ToTable(""CustomsEnvironmentSettings"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsEnvironmentSettingMap.cs	22	5
			  this.ToTable(""CustomsEnvoirmentTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsEnvoirmentTypeMap.cs	22	6
			  this.ToTable(""CustomsExchangeRates"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsExchangeRateMap.cs	22	6
				this.ToTable(""CustomsGenerals"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsGeneralMap.cs	22	5
			  this.ToTable(""CustomsHouseTypeAdditionals"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsHouseTypeAdditionalMap.cs	22	6
			  this.ToTable(""CustomsHouseTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsHouseTypeMap.cs	22	6
			  this.ToTable(""CustomsInsuranceCompanies"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsInsuranceCompanyMap.cs	22	6
			  this.ToTable(""CustomsItemDetailsHistorys"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsItemDetailsHistoryMap.cs	22	6
			  this.ToTable(""CustomsItems"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsItemMap.cs	22	6
			  this.ToTable(""CustomsPartnerFtps"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsPartnerFtpMap.cs	22	6
			  this.ToTable(""CustomsPartnersItems"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsPartnersItemMap.cs	22	6
			  this.ToTable(""CustomsPaymentTerms"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsPaymentTermsMap.cs	22	6
			  this.ToTable(""CustomsRequestsSheets"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsRequestsSheetMap.cs	22	6
			  this.ToTable(""CustomsRequestsSheetStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsRequestsSheetStatusMap.cs	22	6
			  this.ToTable(""CustomsRequiredFields"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsRequiredFieldMap.cs	22	6
			  this.ToTable(""CustomsSettings"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsSettingMap.cs	22	6
			  this.ToTable(""CustomsShips"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsShipMap.cs	22	6
			  this.ToTable(""CustomsTransportModes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsTransportModeMap.cs	22	6
			  this.ToTable(""CustomsVendors"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsVendorMap.cs	22	6
			  this.ToTable(""CustomsVerificationStatusTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\CustomsVerificationStatusTypeMap.cs	22	6
			  this.ToTable(""DangerousGoodsPackingReqs"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DangerousGoodsPackingReqMap.cs	22	6
			  this.ToTable(""DBMigrationLines"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DBMigrationLineMap.cs	22	6
			  this.ToTable(""DBMigrations"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DBMigrationMap.cs	22	6
			  this.ToTable(""DebtNotificationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DebtNotificationTypeMap.cs	22	6
			  this.ToTable(""DecCargoSplitCargoIdentifiers"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DecCargoSplitCargoIdentifierMap.cs	22	6
			  this.ToTable(""DecCargoSplitCons"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DecCargoSplitConMap.cs	22	6
			  this.ToTable(""DecCargoSplitConsItems"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DecCargoSplitConsItemMap.cs	22	6
			  this.ToTable(""DecCargoSplitConsPackDets"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DecCargoSplitConsPackDetMap.cs	22	6
			  this.ToTable(""DecDangersContacts"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DecDangersContactsMap.cs	22	6
			  this.ToTable(""DecisionTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DecisionTypeMap.cs	22	6
			  this.ToTable(""DeclarationCargoSplits"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationCargoSplitMap.cs	22	6
			  this.ToTable(""DeclarationCasualDetailses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationCasualDetailsMap.cs	22	6
			  this.ToTable(""DeclarationConsAcceptances"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationConsAcceptanceMap.cs	22	6
			  this.ToTable(""DeclarationConstraints"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationConstraintMap.cs	22	6
			  this.ToTable(""DeclarationCourierStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationCourierStatusMap.cs	22	6
			  this.ToTable(""DeclarationErrorMappings"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationErrorMappingMap.cs	22	6
			  this.ToTable(""DeclarationExportRecipients"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationExportRecipientMap.cs	22	6
			  this.ToTable(""DeclarationMamanSpecialActions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationMamanSpecialActionMap.cs	22	6
			  this.ToTable(""Declarations"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationMap.cs	22	6
			  this.ToTable(""DeclarationPayments"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationPaymentMap.cs	22	6
			  this.ToTable(""DeclarationPaymentMethods"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationPaymentMethodMap.cs	22	6
			  this.ToTable(""DeclarationPaymentProtests"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationPaymentProtestMap.cs	22	6
			  this.ToTable(""DeclarationPendings"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationPendingMap.cs	22	6
			  this.ToTable(""DeclarationReferantDatas"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationReferantDataMap.cs	22	6
			  this.ToTable(""DeclarationStatementTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationStatementTypeMap.cs	22	6
			  this.ToTable(""DeclarationStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationStatusMap.cs	22	6
			  this.ToTable(""DeclarationStatusTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationStatusTypeMap.cs	22	6
			  this.ToTable(""DeclarationTaxes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeclarationTaxMap.cs	22	6
			  this.ToTable(""DeficitConnFileParagraphTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeficitConnFileParagraphTypeMap.cs	22	6
			  this.ToTable(""DeficitDecisions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeficitDecisionMap.cs	22	6
			  this.ToTable(""Deficits"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeficitsMap.cs	22	6
			  this.ToTable(""DeliverySiteTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeliverySiteTypeMap.cs	22	6
			  this.ToTable(""DeliveryTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DeliveryTypeMap.cs	22	6
			  this.ToTable(""DemanderTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DemanderTypeMap.cs	22	6
			  this.ToTable(""DepositConditions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DepositConditionMap.cs	22	6
			  this.ToTable(""DepositCustomerActivities"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DepositCustomerActivityMap.cs	22	6
			  this.ToTable(""DepositEssenceTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DepositEssenceTypeMap.cs	22	6
			  this.ToTable(""DepositFileTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DepositFileTypeMap.cs	22	6
			  this.ToTable(""Deposits"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DepositMap.cs	22	6
			  this.ToTable(""DocumentRejectTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DocumentRejectTypeMap.cs	22	6
			  this.ToTable(""DocumentTypeCustomsData"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\DocumentTypeCustomsDataMap.cs	22	6
			  this.ToTable(""EntitlementTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\EntitlementTypeMap.cs	22	6
			  this.ToTable(""EntityTypeLookups"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\EntityTypeLookupMap.cs	22	6
			  this.ToTable(""ExceptionReasons"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ExceptionReasonMap.cs	22	6
			  this.ToTable(""ExportDeclarationClosingDatas"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ExportDeclarationClosingDataMap.cs	22	6
			  this.ToTable(""ExportDeliveryDocumentMessages"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ExportDeliveryDocumentMessageMap.cs	22	6
			  this.ToTable(""ExporterRoleTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ExporterRoleTypeMap.cs	22	6
			  this.ToTable(""ExportLogisticPermitActions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ExportLogisticPermitActionMap.cs	22	6
			  this.ToTable(""ExportReferences"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ExportReferenceMap.cs	22	6
			  this.ToTable(""ExportStorages"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ExportStorageMap.cs	22	6
			  this.ToTable(""ExternalFieldMappings"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ExternalFieldMappingMap.cs	22	6
			  this.ToTable(""FacilitationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\FacilitationTypeMap.cs	22	6
			  this.ToTable(""FaultInspectionTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\FaultInspectionTypeMap.cs	22	6
			  this.ToTable(""FclLclCodes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\FclLclCodeMap.cs	22	6
			  this.ToTable(""FreightPaymentMethods"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\FreightPaymentMethodMap.cs	22	6
			  this.ToTable(""FuelTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\FuelTypeMap.cs	22	6
			  this.ToTable(""FullnessCodes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\FullnessCodeMap.cs	22	6
			  this.ToTable(""GatepassRequests"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\GatepassRequestMap.cs	22	6
			  this.ToTable(""GatepassReturnCodes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\GatepassReturnCodeMap.cs	22	6
			  this.ToTable(""Genders"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\GenderMap.cs	22	6
			  this.ToTable(""GovernmentProcedureTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\GovernmentProcedureTypeMap.cs	22	6
				this.ToTable(""GTBFUSTATUS"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\GTBFUSTATUMap.cs	22	5
			  this.ToTable(""GuaranteeCertificateTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\GuaranteeCertificateTypeMap.cs	22	6
			  this.ToTable(""GuaranteeConditions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\GuaranteeConditionMap.cs	22	6
			  this.ToTable(""GuaranteeCustomerActivities"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\GuaranteeCustomerActivityMap.cs	22	6
			  this.ToTable(""Guarantees"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\GuaranteeMap.cs	22	6
			  this.ToTable(""HandingCodes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\HandingCodeMap.cs	22	6
			  this.ToTable(""HazardousSubstances"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\HazardousSubstanceMap.cs	22	6
			  this.ToTable(""ImporterDeclarationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ImporterDeclarationTypeMap.cs	22	6
			  this.ToTable(""ImporterDespositions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ImporterDespositionMap.cs	22	6
			  this.ToTable(""ImporterPeriodicDeclarStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ImporterPeriodicDeclarStatusMap.cs	22	6
			  this.ToTable(""ImporterTypeForClaims"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ImporterTypeForClaimMap.cs	22	6
			  this.ToTable(""IncotemrsFileValidations"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\IncotemrsFileValidationMap.cs	22	6
			  this.ToTable(""InterfaceManagements"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\InterfaceManagementMap.cs	22	6
			  this.ToTable(""InterfaceSendOptions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\InterfaceSendOptionMap.cs	22	6
			  this.ToTable(""InterfaceTenantDefinitions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\InterfaceTenantDefinitionMap.cs	22	6
			  this.ToTable(""InternalBorderSiteTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\InternalBorderSiteTypeMap.cs	22	6
			  this.ToTable(""InternationalSites"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\InternationalSiteMap.cs	22	6
			  this.ToTable(""InvoiceTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\InvoiceTypeMap.cs	22	6
			  this.ToTable(""ItemGovernmentProcedureTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ItemGovernmentProcedureTypeMap.cs	22	6
			  this.ToTable(""LastReleaseFromWarehouses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\LastReleaseFromWarehouseMap.cs	22	6
			  this.ToTable(""LeadDocumentExceptionTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\LeadDocumentExceptionTypeMap.cs	22	6
			  this.ToTable(""LeadDocumentTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\LeadDocumentTypeMap.cs	22	6
			  this.ToTable(""LoadingSiteTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\LoadingSiteTypeMap.cs	22	6
			  this.ToTable(""LogisticActionRequests"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\LogisticActionRequestMap.cs	22	6
			  this.ToTable(""LogisticActionRequestTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\LogisticActionRequestTypeMap.cs	22	6
			  this.ToTable(""LogisticActionResponseReqSes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\LogisticActionResponseReqSMap.cs	22	6
			  this.ToTable(""LogisticPermits"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\LogisticPermitMap.cs	22	6
			  this.ToTable(""LogisticsReferenceTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\LogisticsReferenceTypeMap.cs	22	6
			  this.ToTable(""MamanSpecialActions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\MamanSpecialActionMap.cs	22	6
			  this.ToTable(""MamanSpecialActionStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\MamanSpecialActionStatusMap.cs	22	6
			  this.ToTable(""MamanStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\MamanStatusMap.cs	22	6
			  this.ToTable(""ManifestCargoStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ManifestCargoStatusMap.cs	22	6
			  this.ToTable(""MAWBTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\MAWBTypeMap.cs	22	6
			  this.ToTable(""MeasureQualifier"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\MeasureQualifierMap.cs	22	6
			  this.ToTable(""MeasurmentUnits"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\MeasurmentUnitMap.cs	22	6
			  this.ToTable(""ModificationAndDiscountTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ModificationAndDiscountTypeMap.cs	22	6
			  this.ToTable(""MorningMessageTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\MorningMessageTypeMap.cs	22	6
			  this.ToTable(""NbcDeclarationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\NbcDeclarationTypeMap.cs	22	6
			  this.ToTable(""NDMessageActionCodes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\NDMessageActionCodeMap.cs	22	6
			  this.ToTable(""NotificationDefinitions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\NotificationDefinitionMap.cs	22	6
			  this.ToTable(""Notifications"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\NotificationMap.cs	22	6
			  this.ToTable(""NotificationReplies"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\NotificationReplyMap.cs	22	6
			  this.ToTable(""NotificationTenantDefinition"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\NotificationTenantDefinitionMap.cs	22	6
			  this.ToTable(""NotificationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\NotificationTypeMap.cs	22	6
			  this.ToTable(""OrganizationUnitTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\OrganizationUnitTypeMap.cs	22	6
			  this.ToTable(""PackageMeasureQualifiers"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PackageMeasureQualifierMap.cs	22	6
			  this.ToTable(""PackingTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PackingTypeMap.cs	22	6
			  this.ToTable(""ParagraphTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ParagraphTypeMap.cs	22	6
			  this.ToTable(""PartyRelationshipTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PartyRelationshipTypeMap.cs	22	6
			  this.ToTable(""PassportTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PassportTypeMap.cs	22	6
			  this.ToTable(""PayerActivityTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PayerActivityTypeMap.cs	22	6
			  this.ToTable(""PayerTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PayerTypeMap.cs	22	6
			  this.ToTable(""PaymentMethodStatus"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PaymentMethodStatusMap.cs	22	6
			  this.ToTable(""PaymentMethodTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PaymentMethodTypeMap.cs	22	6
			  this.ToTable(""PaymentOrderConnectionTables"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PaymentOrderConnectionTableMap.cs	22	6
			  this.ToTable(""PaymentOrderLines"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PaymentOrderLineMap.cs	22	6
			  this.ToTable(""PaymentOrders"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PaymentOrderMap.cs	22	6
			  this.ToTable(""PaymentOrderMethods"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PaymentOrderMethodMap.cs	22	6
			  this.ToTable(""PaymentOrderProtestReasons"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PaymentOrderProtestReasonMap.cs	22	6
			  this.ToTable(""PaymentOrderStatus"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PaymentOrderStatusMap.cs	22	6
			  this.ToTable(""PaymentOrderTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PaymentOrderTypeMap.cs	22	6
			  this.ToTable(""PaymentProcesses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PaymentProcessMap.cs	22	6
			  this.ToTable(""PaymentProtestTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PaymentProtestTypeMap.cs	22	6
			  this.ToTable(""PaymentTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PaymentTypeMap.cs	22	6
			  this.ToTable(""PendingByKeywords"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PendingByKeywordMap.cs	22	6
			  this.ToTable(""PendingErrorPlaces"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PendingErrorPlaceMap.cs	22	6
			  this.ToTable(""PhysicalChecks"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PhysicalCheckMap.cs	22	6
			  this.ToTable(""PhysicalCheckOperations"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PhysicalCheckOperationMap.cs	22	6
			  this.ToTable(""PhysicalCheckSearchResultTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PhysicalCheckSearchResultTypeMap.cs	22	6
			  this.ToTable(""PhysicalCheckStatusMessages"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PhysicalCheckStatusMessageMap.cs	22	6
			  this.ToTable(""PoaAuthorizationTypeLookups"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PoaAuthorizationTypeLookupMap.cs	22	6
			  this.ToTable(""PoaStatusTypeLookUps"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PoaStatusTypeLookUpMap.cs	22	6
			  this.ToTable(""PointerLevels"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PointerLevelMap.cs	22	6
			  this.ToTable(""ProceduralFaultInProcessTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ProceduralFaultInProcessTypeMap.cs	22	6
			  this.ToTable(""ProceduralFaultInSourceTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ProceduralFaultInSourceTypeMap.cs	22	6
			  this.ToTable(""ProceduralFaultsConnEntities"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ProceduralFaultsConnEntityMap.cs	22	6
			  this.ToTable(""ProceduralFaults"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ProceduralFaultsMap.cs	22	6
			  this.ToTable(""ProceduralFaultStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ProceduralFaultStatusMap.cs	22	6
			  this.ToTable(""ProceduralFaultTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ProceduralFaultTypeMap.cs	22	6
			  this.ToTable(""ProcessingReasons"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ProcessingReasonMap.cs	22	6
			  this.ToTable(""ProductIdentificationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ProductIdentificationTypeMap.cs	22	6
			  this.ToTable(""ProductNameTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ProductNameTypeMap.cs	22	6
			  this.ToTable(""PropertiesDetailsHistorys"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\PropertiesDetailsHistoryMap.cs	22	6
			  this.ToTable(""RansomViolationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\RansomViolationTypeMap.cs	22	6
			  this.ToTable(""ReferantExceptions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ReferantExceptionMap.cs	22	6
			  this.ToTable(""ReferantTeams"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ReferantTeamMap.cs	22	6
			  this.ToTable(""ReferenceInputTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ReferenceInputTypeMap.cs	22	6
			  this.ToTable(""ReferenceStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ReferenceStatusMap.cs	22	6
			  this.ToTable(""RefundCustomerActivityTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\RefundCustomerActivityTypeMap.cs	22	6
			  this.ToTable(""RegisteredWarehouseSiteTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\RegisteredWarehouseSiteTypeMap.cs	22	6
			  this.ToTable(""ReleaseMessageTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ReleaseMessageTypeMap.cs	22	6
			  this.ToTable(""RequestStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\RequestStatusMap.cs	22	6
			  this.ToTable(""RequestTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\RequestTypeMap.cs	22	6
			  this.ToTable(""RequiredGuaranteeTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\RequiredGuaranteeTypeMap.cs	22	6
			  this.ToTable(""ReturnConditions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ReturnConditionMap.cs	22	6
			  this.ToTable(""SalesTaxExemptionTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SalesTaxExemptionTypeMap.cs	22	6
			  this.ToTable(""SealCompleteness"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SealCompletenesMap.cs	22	6
			  this.ToTable(""SealTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SealTypeMap.cs	22	6
			  this.ToTable(""SealUpdateReasonTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SealUpdateReasonTypeMap.cs	22	6
			  this.ToTable(""SecurityClearenceTypeCodes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SecurityClearenceTypeCodeMap.cs	22	6
			  this.ToTable(""SeizureFactorTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SeizureFactorTypeMap.cs	22	6
			  this.ToTable(""SeizureMethodTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SeizureMethodTypeMap.cs	22	6
			  this.ToTable(""SignatureTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SignatureTypeMap.cs	22	6
			  this.ToTable(""SiteLookups"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SiteLookupMap.cs	22	6
			  this.ToTable(""SiteTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SiteTypeMap.cs	22	6
			  this.ToTable(""SpecialActionDescriptionTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SpecialActionDescriptionTypeMap.cs	22	6
			  this.ToTable(""SpecializationTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SpecializationTypeMap.cs	22	6
			  this.ToTable(""SplitOrMergeReasons"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SplitOrMergeReasonMap.cs	22	6
			  this.ToTable(""StatusFieldTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\StatusFieldTypeMap.cs	22	6
			  this.ToTable(""StorageMessageTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\StorageMessageTypeMap.cs	22	6
			  this.ToTable(""StorageStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\StorageStatusMap.cs	22	6
			  this.ToTable(""StorageStatusTables"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\StorageStatusTableMap.cs	22	6
			  this.ToTable(""StuffingSiteTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\StuffingSiteTypeMap.cs	22	6
			  this.ToTable(""SubCountries"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SubCountryMap.cs	22	6
			  this.ToTable(""SuppInvoiceItemsAbachStatement"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SuppInvoiceItemsAbachStatementMap.cs	22	6
			  this.ToTable(""SupplierInvioceItemCertificats"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvioceItemCertificatMap.cs	22	6
			  this.ToTable(""SupplierInvoiceFreightAmounts"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceFreightAmountMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItems"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItemModVehicles"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemModVehicleMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItemProcesTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemProcesTypeMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItemsConDeclars"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemsConDeclarMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItemsDescripts"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemsDescriptMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItemsLevies"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemsLevyMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItemsMods"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemsModMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItemsPrices"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemsPriceMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItemsProdIdents"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemsProdIdentMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItemsSerialNums"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemsSerialNumMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItemsTaxes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemsTaxMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItemVehicleAdds"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemVehicleAddMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItemVehicles"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemVehicleMap.cs	22	6
			  this.ToTable(""SupplierInvoiceItemVehicleMods"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceItemVehicleModMap.cs	22	6
			  this.ToTable(""SupplierInvoices"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceMap.cs	22	6
			  this.ToTable(""SupplierInvoiceModifications"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceModificationMap.cs	22	6
			  this.ToTable(""SupplierInvoicePayments"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoicePaymentMap.cs	22	6
			  this.ToTable(""SupplierInvoiceUCRs"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierInvoiceUCRMap.cs	22	6
			  this.ToTable(""SupplierPartyTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\SupplierPartyTypeMap.cs	22	6
			  this.ToTable(""TapagConnectionTables"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\TapagConnectionTableMap.cs	22	6
			  this.ToTable(""Tapags"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\TapagMap.cs	22	6
			  this.ToTable(""TapagTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\TapagTypeMap.cs	22	6
			  this.ToTable(""TermsOfSaleTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\TermsOfSaleTypeMap.cs	22	6
			  this.ToTable(""TPGFileTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\TPGFileTypeMap.cs	22	6
			  this.ToTable(""TradeAgreements"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\TradeAgreementMap.cs	22	6
			  this.ToTable(""TradeAgreementProtocols"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\TradeAgreementProtocolMap.cs	22	6
			  this.ToTable(""TradeLevyExamptTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\TradeLevyExamptTypeMap.cs	22	6
			  this.ToTable(""TransactionNatureTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\TransactionNatureTypeMap.cs	22	6
			  this.ToTable(""TransferCargoMethodTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\TransferCargoMethodTypeMap.cs	22	6
			  this.ToTable(""TransportMeansTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\TransportMeansTypeMap.cs	22	6
			  this.ToTable(""TreatmentWays"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\TreatmentWayMap.cs	22	6
			  this.ToTable(""UIMessageAdditionals"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\UIMessageAdditionalMap.cs	22	6
			  this.ToTable(""UIMessages"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\UIMessageMap.cs	22	6
			  this.ToTable(""UnloadingSiteType"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\UnloadingSiteTypeMap.cs	22	6
			  this.ToTable(""UpdateCodes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\UpdateCodeMap.cs	22	6
			  this.ToTable(""ValidCustomsItems"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\ValidCustomsItemMap.cs	22	6
			  this.ToTable(""VehicleManufacturers"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VehicleManufacturerMap.cs	22	6
			  this.ToTable(""Vehicles"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VehicleMap.cs	22	6
			  this.ToTable(""VehicleOwners"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VehicleOwnerMap.cs	22	6
			  this.ToTable(""VehiclePoolTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VehiclePoolTypeMap.cs	22	6
			  this.ToTable(""VehiclePriceListType"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VehiclePriceListTypeMap.cs	22	6
			  this.ToTable(""VehicleReductionTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VehicleReductionTypeMap.cs	22	6
			  this.ToTable(""VehicleSafeAccessoryInstlTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VehicleSafeAccessoryInstlTypeMap.cs	22	6
			  this.ToTable(""VehicleSafetyAccessories"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VehicleSafetyAccessoryMap.cs	22	6
			  this.ToTable(""VehicleSafetyAccessoryTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VehicleSafetyAccessoryTypeMap.cs	22	6
			  this.ToTable(""VehicleStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VehicleStatusMap.cs	22	6
			  this.ToTable(""VehicleTecnologyTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VehicleTecnologyTypeMap.cs	22	6
			  this.ToTable(""VehicleTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VehicleTypeMap.cs	22	6
			  this.ToTable(""VendorCommissions"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VendorCommissionMap.cs	22	6
			  this.ToTable(""VendorCommunications"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VendorCommunicationMap.cs	22	6
			  this.ToTable(""VendorStatuses"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VendorStatusMap.cs	22	6
			  this.ToTable(""VendorTransactionTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VendorTransactionTypeMap.cs	22	6
			  this.ToTable(""VendorTypes"", ""Customs"");	C:\log2004\Logitude\Logitude.Customs.Data\EntityMapping\VendorTypeMap.cs	22	6

";

            var lines=d.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            Debug.WriteLine("CREATE SCHEMA Customs;  ");
			foreach (var line in lines)
			{

				//this.ToTable(""AcceptanceStatuses"
				int pos = line.IndexOf("this.ToTable(");
				string table = line.Substring(pos + "this.ToTable(".Length + 1);
				pos = table.IndexOf('"');
				table = table.Substring(0, pos);
				if (line.Contains(@", ""Customs"""))
				{
					Debug.WriteLine($"ALTER SCHEMA Customs TRANSFER OBJECT::dbo.{table/*.ToUpper()*/};  ");

				}
				else
				{
					//Debug.WriteLine($"ALTER SCHEMA dbo TRANSFER OBJECT::Customs.{table/*.ToUpper()*/}; ");
				}


			}
        }


#if false

INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008241136_EventTypesConverteLocalNameToEnglishName.sxml', '0001-01-01 00:00:00', 'If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID(''tempdb..#temp_EventTypes'') Is Not Null)
Begin
Drop Table #temp_EventTypes
End
CREATE TABLE #temp_EventTypes (
Id varchar(15) not null ,-- primary keyz
EnglishName varchar(200)  null
)
--drop table #tempTable
select  Id, EnglishName,LocalName
into #tempTable
FROM EventTypes where $LastCounterWhere$
CREATE INDEX IDX_tempTable_Id ON dbo.#tempTable(Id)
CREATE INDEX IDX_tempTable_EnglishName ON dbo.#tempTable(EnglishName)
CREATE INDEX IDX_tempTable_LocalName ON dbo.#tempTable(LocalName)
declare @Tenant as int
declare @Id as varchar(15)
declare @EnglishName as varchar(200)
declare @LocalName as nvarchar(200)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id,EnglishName,LocalName
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id,@EnglishName,@LocalName
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_EventTypes(Id,EnglishName) values (@Id,@EnglishName)
set @Count = @Count + 1;
--print @Count
if(@Count = 1000)
begin print @Count
update EventTypes
set LocalName= T.EnglishName
FROM EventTypes eventTypesTable
INNER JOIN #temp_EventTypes T
--on eventTypesTable.Id = T.Id
on eventTypesTable.Id COLLATE SQL_Latin1_General_CP1_CI_AS= T.Id COLLATE SQL_Latin1_General_CP1_CI_AS
truncate table #temp_EventTypes
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @Id,@EnglishName,@LocalName
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
update EventTypes
set LocalName= T.EnglishName
FROM EventTypes eventTypesTable
INNER JOIN #temp_EventTypes T
--on eventTypesTable.Id = T.Id
on eventTypesTable.Id COLLATE SQL_Latin1_General_CP1_CI_AS= T.Id COLLATE SQL_Latin1_General_CP1_CI_AS
drop table #tempTable
drop table #temp_EventTypes', 0, 'df27232d423f1854e0ad9261e82123a5', 5);



INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202012080427_PreparerUpdateCardSearchData.sxml', '2022-09-20 16:14:53', 'delete CardSearches    where  Weight = 30  and  $LastCounterWhere$', 0, '1dc2f64e71c0c08a726c5372e0857c5e', 1);






INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202012080421_UpdatePortsSearchFields.sxml', GETDATE(), 'update Ports set SearchFields = SearchFields+ '','' + CombinedCode  where $LastCounterWhere$', DATEDIFF(MS,GETDATE(),GETDATE()), '009ee8a75cb143607cdcf274440b7aaf', 1);


/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [SxmlFileName]
      ,[ExecutionDate]
      ,[ScriptBody]
      ,[ElapsedTimeInMs]
      ,[HashValue]
      ,[Version]
  FROM [Logitude2-5_Main].[dbo].[DBScriptsHistory]
  WHERE SxmlFileName LIKE '202008241136_EventTypesConverteLocalNameToEnglishName%'




INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008241136_EventTypesConverteLocalNameToEnglishName.sxml', GETDATE(), 'If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID(''tempdb..#temp_EventTypes'') Is Not Null)
Begin
Drop Table #temp_EventTypes
End
CREATE TABLE #temp_EventTypes (
Id varchar(15) not null ,-- primary keyz
EnglishName varchar(200)  null
)
--drop table #tempTable
select  Id, EnglishName,LocalName
into #tempTable
FROM EventTypes where $LastCounterWhere$
CREATE INDEX IDX_tempTable_Id ON dbo.#tempTable(Id)
CREATE INDEX IDX_tempTable_EnglishName ON dbo.#tempTable(EnglishName)
CREATE INDEX IDX_tempTable_LocalName ON dbo.#tempTable(LocalName)
declare @Tenant as int
declare @Id as varchar(15)
declare @EnglishName as varchar(200)
declare @LocalName as nvarchar(200)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id,EnglishName,LocalName
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id,@EnglishName,@LocalName
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_EventTypes(Id,EnglishName) values (@Id,@EnglishName)
set @Count = @Count + 1;
--print @Count
if(@Count = 1000)
begin print @Count
update EventTypes
set LocalName= T.EnglishName
FROM EventTypes eventTypesTable
INNER JOIN #temp_EventTypes T
--on eventTypesTable.Id = T.Id
on eventTypesTable.Id COLLATE SQL_Latin1_General_CP1_CI_AS= T.Id COLLATE SQL_Latin1_General_CP1_CI_AS
truncate table #temp_EventTypes
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @Id,@EnglishName,@LocalName
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
update EventTypes
set LocalName= T.EnglishName
FROM EventTypes eventTypesTable
INNER JOIN #temp_EventTypes T
--on eventTypesTable.Id = T.Id
on eventTypesTable.Id COLLATE SQL_Latin1_General_CP1_CI_AS= T.Id COLLATE SQL_Latin1_General_CP1_CI_AS
drop table #tempTable
drop table #temp_EventTypes', DATEDIFF(MS,getdate(),getdate()), 'df27232d423f1854e0ad9261e82123a5', 5);





UPDATE [Customs].[ExportStorages] SET [OPENDATE] = GETDATE() WHERE [OPENDATE] = '0001-01-01 00:00:00.0000000';
ALTER TABLE [Customs].[ExportStorages] ALTER COLUMN [OPENDATE] DATETIME NOT NULL;

SET ANSI_WARNINGS OFF;
-- Your insert TSQL here.
ALTER TABLE [Customs].[LogisticActionRequestTypes] ALTER COLUMN [LOCALNAME] NVARCHAR(40)
SET ANSI_WARNINGS ON;

-------------------------------

UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'if exists (select * from ChargesTypes where Code = ''SCX'' and Tenant = 0)
begin
declare @Tenant as int
declare @NewId as varchar(15)
declare @ChargesGroupId as varchar(15)
declare @ChargesGroupCode as varchar(5)
declare @VATTypeId as varchar(15)
declare @MeasurementId as varchar(15)
declare @IATACodeId as varchar(15)
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
if not exists (select * from ChargesTypes where Code = ''SCX'' and Tenant = @Tenant)
begin
set @ChargesGroupCode = ''SCH''
set @ChargesGroupId = (select Id from ChargesGroups where Tenant = @Tenant and Code = ''SCH'')
set @VATTypeId = (select Id from VatTypes where Tenant = @Tenant and Code = ''ZERO'')
set @IATACodeId = (select Id from IATACodes where Code = ''SC'')
set @MeasurementId = (select Id from Measurements where Tenant = @Tenant and Code = ''CHWT'')
if (@MeasurementId is null)
begin
EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,''Measurement''
insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
values(''CHWT'', ''Chargeable Weight / WM'', ''Ch Weight'', @MeasurementId, @Tenant, 0, 0, 0, ''CHWT,Chargeable Weight / WM,Ch Weight'', NULL)
end
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''ChargesType''
insert into ChargesTypes(Code, EnglishName, LocalName, Id, Tenant, AddedManually, InActive, ChargesGroupCode, VatTypeId, IsReceivable,
IsPayable, IsAir, IsOcean, IsInland, IsAutoDisplayInShipment, IsAutoDisplayInConsolidation, Description, AWBPrintDescription, DueTypeCode,
IsAutoDisplayInQuote, MeasurementId, ContainerMeasurementId, ViewOrder, SearchFields, ReceivableAccountId, PayableAccountId, AccountingVATSplit,
ReceivableCreditAccount, PayableDebitAccount, ReceivablesChargesTypeExternalCode, IATACodeId, PayableDebitGLAcountId, ReceivableCreditGLAccountId,
ChargesGroupId,PayablesChargesTypeExternalCode,IsBackToBack,IsAutoDisplayInCustoms,
IsCustoms,IsExpense,SATExternalId,IsImport,IsDomestic,IsExport,IsDrop,
ReceivablesDefaultCurrencyId,PayablesDefaultCurrencyId,ApplyRegionalTax, HasPickup, HasDelivery)
values(''SCX'', ''Screening Charge'', Null, @NewId, @Tenant, 1, 0, @ChargesGroupCode , @VATTypeId, 1,
1, 1, 0, 0, 0, 0, NULL, 1, ''CA'',
0, @MeasurementId, NULL, 100,''SCX,Screening Charge'', NULL, NULL, 0,
NULL, NULL, NULL, @IATACodeId, NULL, NULL,
@ChargesGroupId ,NULL,0,0,
0,0,NULL,0,0,0,0,
NULL,NULL,0,0,0)
end
FETCH NEXT FROM DataCursor INTO @Tenant
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = ''ChargesType'')
end',  [HashValue] = '3898bf25f589f2d14876cd55533575b0', [Version] = 3 WHERE [SxmlFileName] = '202107071250_CopySCXChargesTypeToAllTenants.sxml';

UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'if not exists (select Code from Toggles where Code = ''HRS'')
begin
insert into Toggles (Code, Name, SearchFields)
values (''HRS'', ''Horse'', ''HRS,Horse'')
end
update Features
set ToggleCode = ''HRS''
where Code = ''Horse.M.Horses''',  [HashValue] = '3e01d0de48c070160eed409222a814c8', [Version] = 2 WHERE [SxmlFileName] = '202008261425_SetToggleCodeForHorsesFeature.sxml';


UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'declare @TermOfUsePK nvarchar(1000)
declare @Command nvarchar(1000)
IF OBJECT_ID(''dbo.[FK_TermsofUseSignatureTermsofUse]'') IS NOT NULL
begin
ALTER TABLE TermsofUseSignatures
DROP CONSTRAINT [FK_TermsofUseSignatureTermsofUse];
end
IF OBJECT_ID(''dbo.[FK_TermsofUseSignatures_TermsofUses_TermsofUseId]'') IS NOT NULL
begin
ALTER TABLE TermsofUseSignatures
DROP CONSTRAINT [FK_TermsofUseSignatures_TermsofUses_TermsofUseId];
end
IF OBJECT_ID(''dbo.[PK_TermsofUses]'') IS NOT NULL
begin
ALTER TABLE TermsofUses
DROP CONSTRAINT [PK_TermsofUses];
end
set @TermOfUsePK = (select C.CONSTRAINT_NAME FROM
INFORMATION_SCHEMA.TABLE_CONSTRAINTS T
JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE C
ON C.CONSTRAINT_NAME=T.CONSTRAINT_NAME
WHERE
C.TABLE_NAME=''TermsofUses''
and T.CONSTRAINT_TYPE=''PRIMARY KEY'')
select @Command = (''ALTER TABLE TermsofUses
DROP CONSTRAINT '' + @TermOfUsePK);
if(@TermOfUsePK is not null)
begin
execute (@Command)
end
update TermsofUseSignatures set TermsofUseId = (select id from TermsofUses where VersionNumber =TermsofUseSignatures.TermsofUseId )
Alter TABLE [dbo].[TermsofUses] drop column [Id]
Alter TABLE [dbo].[TermsofUses]
add [id] [int] Constraint [PK_TermsofUses] PRIMARY KEY IDENTITY(1,1) NOT NULL
IF OBJECT_ID(''dbo.[FK_TermsofUseSignatureTermsofUse]'') IS NULL
begin
ALTER TABLE [dbo].[TermsofUseSignatures] WITH CHECK ADD CONSTRAINT [FK_TermsofUseSignatureTermsofUse] FOREIGN KEY([TermsofUseId])
REFERENCES [dbo].[TermsofUses] ([Id])
end',  [HashValue] = '8db426b901badc861dfbf65d9b5f98e3', [Version] = 5 WHERE [SxmlFileName] = 'UpdateTermsOfUsePrimaryKey.sxml';

UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'IF EXISTS (SELECT *
FROM   sys.objects
WHERE  object_id = OBJECT_ID(N''[dbo].[BuildSearchKeywordFunction]''))
DROP FUNCTION [dbo].[BuildSearchKeywordFunction]
declare @dateString as varchar(3000)
set @dateString = ''CREATE FUNCTION dbo.BuildSearchKeywordFunction ( @stringToSplit nvarchar(MAX)  , @firstweight int , @Secondweight int)
RETURNS
@returnList TABLE ([Keyword] [nvarchar] (500), [weight] int )
AS
BEGIN
set @stringToSplit =  RTrim(@stringToSplit)
DECLARE @IsFirstTime bit
set @IsFirstTime = 1;
DECLARE @name nvarchar(MAX)
DECLARE @pos INT
if(@stringToSplit!='''' '''') begin INSERT INTO @returnList  SELECT @stringToSplit ,@firstweight end
WHILE CHARINDEX('''' '''', @stringToSplit) > 0
BEGIN
SELECT @pos  = CHARINDEX('''' '''', @stringToSplit)
SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)
if(@IsFirstTime= 0 and @name!='''' '''')   begin INSERT INTO @returnList  SELECT @stringToSplit ,@Secondweight end
SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
set @IsFirstTime = 0;
END
if(@IsFirstTime= 0 and @stringToSplit!='''' '''')begin INSERT INTO @returnList SELECT @stringToSplit ,@Secondweight
end
RETURN
END''
EXEC(@dateString)',  [HashValue] = '598fa43f8677d92057a73ea7ffba534c', [Version] = 2 WHERE [SxmlFileName] = 'BuildSearchKeywordFunction.sxml';


UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'IF EXISTS (SELECT *
FROM   sys.objects
WHERE  object_id = OBJECT_ID(N''[dbo].[BuildSearchKeywordFunction]''))
DROP FUNCTION [dbo].[BuildSearchKeywordFunction]
declare @dateString as varchar(3000)
set @dateString = ''CREATE FUNCTION dbo.BuildSearchKeywordFunction ( @stringToSplit nvarchar(MAX)  , @firstweight int , @Secondweight int)
RETURNS
@returnList TABLE ([Keyword] [nvarchar] (500), [weight] int )
AS
BEGIN
set @stringToSplit =  RTrim(@stringToSplit)
DECLARE @IsFirstTime bit
set @IsFirstTime = 1;
DECLARE @name nvarchar(MAX)
DECLARE @pos INT
if(@stringToSplit!='''' '''') begin INSERT INTO @returnList  SELECT @stringToSplit ,@firstweight end
WHILE CHARINDEX('''' '''', @stringToSplit) > 0
BEGIN
SELECT @pos  = CHARINDEX('''' '''', @stringToSplit)
SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)
if(@IsFirstTime= 0 and @name!='''' '''')   begin INSERT INTO @returnList  SELECT @stringToSplit ,@Secondweight end
SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
set @IsFirstTime = 0;
END
if(@IsFirstTime= 0 and @stringToSplit!='''' '''')begin INSERT INTO @returnList SELECT @stringToSplit ,@Secondweight
end
RETURN
END''
EXEC(@dateString)',  [HashValue] = '598fa43f8677d92057a73ea7ffba534c', [Version] = 2 WHERE [SxmlFileName] = 'BuildSearchKeywordFunction.sxml';


UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'IF EXISTS (SELECT *
FROM   sys.objects
WHERE  object_id = OBJECT_ID(N''[dbo].[BuildSearchKeywordFunction]''))
DROP FUNCTION [dbo].[BuildSearchKeywordFunction]
declare @dateString as varchar(3000)
set @dateString = ''CREATE FUNCTION dbo.BuildSearchKeywordFunction ( @stringToSplit nvarchar(MAX)  , @firstweight int , @Secondweight int)
RETURNS
@returnList TABLE ([Keyword] [nvarchar] (500), [weight] int )
AS
BEGIN
set @stringToSplit =  RTrim(@stringToSplit)
DECLARE @IsFirstTime bit
set @IsFirstTime = 1;
DECLARE @name nvarchar(MAX)
DECLARE @pos INT
if(@stringToSplit!='''' '''') begin INSERT INTO @returnList  SELECT @stringToSplit ,@firstweight end
WHILE CHARINDEX('''' '''', @stringToSplit) > 0
BEGIN
SELECT @pos  = CHARINDEX('''' '''', @stringToSplit)
SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)
if(@IsFirstTime= 0 and @name!='''' '''')   begin INSERT INTO @returnList  SELECT @stringToSplit ,@Secondweight end
SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
set @IsFirstTime = 0;
END
if(@IsFirstTime= 0 and @stringToSplit!='''' '''')begin INSERT INTO @returnList SELECT @stringToSplit ,@Secondweight
end
RETURN
END''
EXEC(@dateString)',  [HashValue] = '598fa43f8677d92057a73ea7ffba534c', [Version] = 2 WHERE [SxmlFileName] = 'BuildSearchKeywordFunction.sxml';



UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'delete from FeatureToggles where ToggleCode = ''LIC'' or togglecode = ''TJC''
delete from toggles where Code = ''LIC'' or Code = ''TJC''',  [HashValue] = 'f857d2dfdf4f0a6357967be7824a6c3d', [Version] = 2 WHERE [SxmlFileName] = '202101280122_RemoveTicketNumberAndLicenseManagementFeatureToggle.sxml';

UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'NULL',  [HashValue] = 'NULL', [Version] = 2 WHERE [SxmlFileName] = '202010111400_UpdateRequiredFields.sxml';

UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'NULL',  [HashValue] = 'NULL', [Version] = 2 WHERE [SxmlFileName] = '202007201200_DepartmentSearchfield.sxml';

UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'UPDATE DWSubQueries
SET FiltersXML = REPLACE(FiltersXML, ''Direct / House'', ''Shipment Level'')
WHERE DWFactTableCode = ''Fact_Shipments'' or DWFactTableCode = ''Fact_Charges''
UPDATE DWSubQueries
SET ColumnsXML = REPLACE(ColumnsXML, ''Direct / House'', ''Shipment Level'')
WHERE DWFactTableCode = ''Fact_Shipments'' or DWFactTableCode = ''Fact_Charges''',  [HashValue] = 'd70988a186916a465d0bd13d6854ced1', [Version] = 2 WHERE [SxmlFileName] = 'UpdateDWSubQueries.sxml';


UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'DELETE FROM Toggles WHERE code = ''BDR'' or  code = ''BDW'' or code = ''CST'' or code = ''RRW'' or code = ''TJC'' or code = ''CWH'';',  [HashValue] = '9baab2aab85ad1595fd924121108876c', [Version] = 3 WHERE [SxmlFileName] = '20210221_DeleteUnusedTogglesCodes.sxml';
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'IF OBJECT_ID (N''dbo.GetDateFormate'', N''FN'') IS NOT NULL
begin
DROP FUNCTION dbo.GetDateFormate;
end
IF OBJECT_ID (N''dbo.GetLastRate'', N''FN'') IS NOT NULL
begin
DROP FUNCTION dbo.GetLastRate;
end',  [HashValue] = '59521f5722e232ab4ececd31d940b264', [Version] = 3 WHERE [SxmlFileName] = 'DropChangeCurrencyFunctions.sxml';


UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'DECLARE @Sql NVARCHAR(MAX)
SET @Sql =
''
CREATE FUNCTION dbo.GetDateFormate (@dateTime datetime)
RETURNS date
WITH EXECUTE AS CALLER
AS
BEGIN
DECLARE @Result date;
SET @Result = dateadd(dd, datediff(dd, 0, @dateTime), 0)
RETURN(@Result);
END
'';
EXEC(@Sql);',  [HashValue] = 'e1e261f0aa18cae4990b0f39fd13d929', [Version] = 2 WHERE [SxmlFileName] = 'GetDateFormateFunction.sxml';


UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'DECLARE @Sql NVARCHAR(MAX)
SET @Sql =
''
CREATE FUNCTION dbo.GetDateFormate (@dateTime datetime)
RETURNS date
WITH EXECUTE AS CALLER
AS
BEGIN
DECLARE @Result date;
SET @Result = dateadd(dd, datediff(dd, 0, @dateTime), 0)
RETURN(@Result);
END
'';
EXEC(@Sql);',  [HashValue] = 'e1e261f0aa18cae4990b0f39fd13d929', [Version] = 2 WHERE [SxmlFileName] = 'GetDateFormateFunction.sxml';


UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'DECLARE @Sql NVARCHAR(MAX)
SET @Sql =
''
CREATE FUNCTION dbo.GetDateFormate (@dateTime datetime)
RETURNS date
WITH EXECUTE AS CALLER
AS
BEGIN
DECLARE @Result date;
SET @Result = dateadd(dd, datediff(dd, 0, @dateTime), 0)
RETURN(@Result);
END
'';
EXEC(@Sql);',  [HashValue] = 'e1e261f0aa18cae4990b0f39fd13d929', [Version] = 2 WHERE [SxmlFileName] = 'GetDateFormateFunction.sxml';


UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'DECLARE @Sql NVARCHAR(MAX)
SET @Sql =
''
CREATE FUNCTION dbo.GetLastRate (@Tenant int, @DateTime datetime, @BaseCurrencyId varchar(15), @ForeignCurrencyId varchar(15))
RETURNS float
WITH EXECUTE AS CALLER
AS
BEGIN
DECLARE @Result float;
if (@BaseCurrencyId = @ForeignCurrencyId)
BEGIN
set @Result = 1
END
else
BEGIN
declare @ValueDate as date
set @ValueDate = dbo.GetDateFormate(@DateTime)
if exists (select * from RatesTables where BaseCurrencyId = @BaseCurrencyId and ForeignCurrencyId = @ForeignCurrencyId and Tenant = @Tenant)
begin
if exists (select * from RatesTables where BaseCurrencyId = @BaseCurrencyId and ForeignCurrencyId = @ForeignCurrencyId and Tenant = @Tenant and ValueDate = @ValueDate)
begin
set @Result = (select top(1) Rate from RatesTables
where BaseCurrencyId = @BaseCurrencyId
and ForeignCurrencyId = @ForeignCurrencyId
and Tenant = @Tenant
and ValueDate = @ValueDate
order by LogDateTime DESC
)
end
else if exists (select * from RatesTables where BaseCurrencyId = @BaseCurrencyId and ForeignCurrencyId = @ForeignCurrencyId and Tenant = @Tenant and ValueDate < @ValueDate)
begin
set @Result = (select top(1) Rate from RatesTables
where BaseCurrencyId = @BaseCurrencyId
and ForeignCurrencyId = @ForeignCurrencyId
and Tenant = @Tenant
and ValueDate < @ValueDate
order by LogDateTime DESC
)
end
else
begin
set @Result = (select top(1) Rate from RatesTables
where BaseCurrencyId = @BaseCurrencyId
and ForeignCurrencyId = @ForeignCurrencyId
and Tenant = @Tenant
order by LogDateTime DESC
)
end
end
END
RETURN(@Result);
END;
'';
EXEC(@Sql);',  [HashValue] = '8cb0bb15fe068ffc0f48ae2f08dadbe7', [Version] = 2 WHERE [SxmlFileName] = 'GetLastRateFunction.sxml';


UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'delete BatchServicesDefinitionMods where Code =''QueryExportExecutionLogWR''
delete BatchServicesDefinitions where Code =''QueryExportExecutionLogWR''
INSERT INTO [dbo].[BatchServicesDefinitions]
([Code]
,[ClassName]
,[Parameter1]
,[Parameter2] ,
[QueueDefinitionCode])
VALUES
(''QueryExportExecutionLogWR''
,''QueryExportExecutionLogWR''
,NULL
,NULL
,''QueryExportExecutionLogQueue'')
INSERT INTO [dbo].[BatchServicesDefinitionMods]
([Code]
,[InActive]
,[NumberOfThreads])
VALUES
(''QueryExportExecutionLogWR''
,0
,1)', [HashValue] = '18836d99a01ec00bf4c262270d834a3f', [Version] = 2 WHERE [SxmlFileName] = '202012100811_QueryExportExecutionLogWorkerRole.sxml';


UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = 'e0b77ba0474277f805cd2fd1ce239c71' WHERE [SxmlFileName] = '202012251232_UpdateUserDefinedReportLocalName.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '099d4043a382d2a5541bc3aff93bcc2b', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210123_DeleteMetadataBadRecords.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'a4cb43ffa16c14c5725d68c067aa700c', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210124_InsertFieldDataTypes.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '8d4ff07303bb81a884768ae00c3f570d', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210125_BeforeUpdateDeleteAllMetadata.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'e2e26a0a6214899eaa7d9aa6814a5164', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005270157_SetNullableForCDROPColumns_Global.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'e2e26a0a6214899eaa7d9aa6814a5164', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005270158_SetNullableForCDROPColumns_Main.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'c5ee283898fbf2d29232bee2606ab88a', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202006031027_AddReferantException901.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '584da0fff34716b45bb5f101fe923b74', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202007031145_DeclarationCancellationAddOn.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'ac7d69e9000b872dd8988527eb711e2e', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202007201100_ReferantSearchfield.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'af366a77871b1d596a800783cf529e3c', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202012211428_SetDeclarationDirection2I.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '6f302ba11564204604a906a530d1bc4d', [HashValue] = 'NULL' WHERE [SxmlFileName] = '2020122708949_SetOpenDeclarations.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '9d0980f5df82cf90579ade3c0a3e3d76', [HashValue] = 'NULL' WHERE [SxmlFileName] = '2021011719371_UpdateREQUESTEDCUSTOMSDOCID.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'b07357e20d2f79f27bede73b2301196d', [HashValue] = 'NULL' WHERE [SxmlFileName] = '2021011719372_UpdateREQUESTEDCUSTOMSDOCID2.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '4f23a5fa9c0f431e343480c5e66d56c9', [HashValue] = 'NULL' WHERE [SxmlFileName] = '2021022202_UpdatephysicalcheckDeclaration.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'a3d5952b30c074ad89c1ad7ef6b2901d', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202103251635_AddFCLLCL.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '7ac869d649bdbdf2a53e201b0f9bbe2d', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202104201027_AddCOURIERPENDINGREASONS903.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '6c07b6ba3b542df11d8dd064058d4270', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202105252040_AddAmendmentStatuses.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '4a622156f8039c15cbc0c99ede46012e', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202106061800_AddComputingPartners.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '775ab79942f057c720fea8bae95062db', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202107071340_SetCustomsDocumentsDefinitionDeclarationType.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '31c99bcb5d92f391d5ebaaea56da4060', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202107071341_SetDeclarationDeclarationTypeCode.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '76ff758cf62e4e88cdf899093e7d79c2', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202108011330_AddPhysicalCheckSearchResultTypes.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '9c4a786804d7d1f3799b74665009506a', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202111101021_SetCargoDescription.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '7c5e4d6dc42518fe2b0f6c050502dc21', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202111101341_InterfaceManagementDefaultPriority89.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '79b2c436715c4877816c95d23eb50059', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202201031031_SearchFieldCourierMaster.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '156768984036cf83b283967eb51ee7cb', [HashValue] = 'NULL' WHERE [SxmlFileName] = '2022010514_InterfaceTenantDefS8251.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'dd6afff692ae981f333727a1f8029cf5', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202201231428_SetReferantPackageType.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'bb7880f175f691ce8cdb16fedb084d7f', [HashValue] = 'NULL' WHERE [SxmlFileName] = '20220131_UpdateTerminalReleaseDate.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'cca15c06b8ca86273b5528a90db36619', [HashValue] = 'NULL' WHERE [SxmlFileName] = '20220309_RenameAcceptenceCode.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '444fc82b3bb155ca2b93b02b10c22504', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202208281500_UpdateLoadingOfDeclaration.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '4625ed0d323ddcee9d1f3289d6193b03', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210104_DropUnneededTables_Global.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '5c6712e2ad29e9e54cba0efd40fe739c', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210105_DropUnneededTables_Main.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '56586ad5b96c1f0cba7315d568348615', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210107_TablesBackup.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'd96ddf7fb32fcbba89bfea337b31cfac', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210108_AddMetadataColumns.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'c9bd141d9da965bef1f67ff47504b9c5', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210109_ColorIndexScripts.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'bdb444695b6612bacb08f6153f709fea', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210110_DeleteDuplicateCountries.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'b9c2ed8f6744f4056b1ecbefc4f610d4', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210111_DeleteDuplicateCountryCities.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '16801f24305bd7d07a264a8331f57836', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210112_DeleteDuplicateIncoterms.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '4748ef91dd5a55683be8b5cce851eeb2', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210113_DeleteDuplicatePorts.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'deda458c7925c1849d10883bbd32535c', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210114_DeleteDuplicateStates.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '6c7598f733c277356cd58a167ec4016f', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210115_NullColumnsMustHaveValueInTenants.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '780f349ec4408cd3df34891f439933fb', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210116_PackageTypePrintAsNullable.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '3fe2a763626fdb00300247dcf94ee1a7', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210118_UpdateObjectFieldCode.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'a7e121927a3b799001d84fe6577f75a0', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210119_DropIX_COMMUNICATIONLOGS_ENTITYID.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '71e43b24e6ca1d10e9b4b2a6deecb18f', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210120_DataTypeChanges_Global.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '7f4f85d1d78a130d1eafcd31ee3e1ac6', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210121_DataTypeChanges_Main.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '8392b23aa07a735b47d59ee9407558e6', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210122_BeforeDeleteMetadata.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '7d2cbf82c20be750d511bfa5aba057da', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005210123_DeleteDuplicateQueries.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'a716757bab182209e0c18c4117c7c4e3', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005261025_CreateSpellCheckedTextCodesTable.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '9208187e124d6416e0f4cd42f707e5e3', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202005261026_AddSpellCheckedTextCodes.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '6434eaf5b493db292a31d3527d336008', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202006031905_default0Isdiamond.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'e5baf9fdf328c5e96fb49321ad680314', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202006040612_RefIsClosedForFollowUp.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '9342bbc94ce47d052764b6b5423cde5c', [HashValue] = 'NULL' WHERE [SxmlFileName] = '2020070502_ISVALIDTICKETSDIAMOND.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '0368ee576c3ff23d233081e8e30bb32b', [HashValue] = 'NULL' WHERE [SxmlFileName] = '2020070560750_UQ_CARDS_TENANT_CODE_PAR_CXQOV.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'e4216978010eac162f76bfed72a0d5fe', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202007091441_FixFKCliam.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '9c2528dac3f9097da0c5f5e5e49cc06b', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202007091941_AddONDeclarationCancellationAndAmendment.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '698e9caa443d7c8c20a162c5bbd95730', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202009061641_FixFKCourierPendingReason.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '788d84dd564498e5be2a971794a80d53', [HashValue] = 'NULL' WHERE [SxmlFileName] = '20201215_AddAddonCourierAuto.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '422d36551bbf131fab1106a1ac4433bf', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202101131441_DocumentsMetaDataTypeByCodeVER.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '2ccce7bbb59f74b630f13e520d3c2e98', [HashValue] = 'NULL' WHERE [SxmlFileName] = '20210726_CustomPartnerFTPSDropIndex.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'a004805d0095af378359f58a1c2bba79', [HashValue] = 'NULL' WHERE [SxmlFileName] = '20210801_DeleteDuplicateQueryColumns .sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '7499b91ec4072688d7ea3d0d9bb8959c', [HashValue] = 'NULL' WHERE [SxmlFileName] = '202108111402_AddONSearchResult.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '23f6b81a856cd750f5757c5722d559dc', [HashValue] = 'NULL' WHERE [SxmlFileName] = '20210929_AddMoveTypes.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'e7df8cc46f8e969e02beadf76701a3e1', [HashValue] = 'NULL' WHERE [SxmlFileName] = '20211125_UpdateIsMultiCustomer.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'f3f11bd6218b9e82b64272e832fc41f5', [HashValue] = 'NULL' WHERE [SxmlFileName] = '20220425_COMMLOG_DBIDCOUNTERS_SEQ.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '0a7f31c9741257deda593926a337a3bf', [HashValue] = 'NULL' WHERE [SxmlFileName] = '20220425_DOCUMENTDBIDCOUNTERS_SEQ.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'bc7f5b4ae3d9ceb206e953bfdd25177f', [HashValue] = 'NULL' WHERE [SxmlFileName] = '20220720_UpdateExportStorages.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = '44e28b1939ecb24575174b17f88972b9' WHERE [SxmlFileName] = '202101051306_FillFeatureToggleSearchField.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = '3c46bcbe512657f0fc3f1187b86611c1' WHERE [SxmlFileName] = '202101061212_SetTariffModuleToggleFeature.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = 'ff06fc55785ad537f07b49feb27a1672' WHERE [SxmlFileName] = '20210221_DeleteUnusedFeatureTogglesCodes.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = '09b9c1fe46cc1fcc035a575fd37d1f9f' WHERE [SxmlFileName] = '202103020855SetToggleForChildPickupDeliveryFeature.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = 'b6bc0de541507b882f714d66ce92ad70' WHERE [SxmlFileName] = '20210516_DeleteUnusedTogglesAndFeatureToggles.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'a6b9e112ba7fac4e9000996028247e49', [HashValue] = '4fa490a8fc4e701f6a1cdef3ff29f3a2' WHERE [SxmlFileName] = 'InsertDBMigrationSettingsData.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = '34160d36613b33e37d2248815e5bcbfa', [HashValue] = 'a0aad5113b04708a7231548b1f995980' WHERE [SxmlFileName] = 'ScreenField Unique index removal.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = '5a31949cce6467cce78c87ba8d9dff83' WHERE [SxmlFileName] = '202007201431_FillWarehouseWeightClosedTables.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = 'b17d9a558fa92bab0d7aece163282d61' WHERE [SxmlFileName] = '202007201436_FillWarehouseFieldsDefaultValues.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = 'b252e3857be84e566299715d80b06883' WHERE [SxmlFileName] = '202008051018_AddNewMeasurementAndChargesType.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = '34b9d9902a50650ec4ac4fa4aa87bfe1' WHERE [SxmlFileName] = '202008110941_AddNewMeasurement.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = '60d427a5e1ea664ef4e7d5766423a171' WHERE [SxmlFileName] = '20200812_SetIATACodeToSRForImportStorageCharge.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = '5f18abe5124698d5536097a0706ca9fb' WHERE [SxmlFileName] = '202008300915_AddHorsesEventTypesToTenants.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = '830638ae226d14b30e478d0b45d1f45b' WHERE [SxmlFileName] = '202009011450_ImportToUSADropMaman.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = '6b0428b8661bd1f864b81e3312416826' WHERE [SxmlFileName] = '202009101501_AddChargeStorageGroupToTenants.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = 'c6e53b641761f823e7bdb3f2266498be' WHERE [SxmlFileName] = '202009151233_UpdateWarehouseReleaseEventTypeStatus.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = '03a869ba6c4d4cf9ac7fb5b2e97587c4' WHERE [SxmlFileName] = '202011190832_UpdateEORInumberInCustomerToField1Value.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = '31dbe5f968a3b4da7488b4029591d14a' WHERE [SxmlFileName] = '202012211256_AddNewPickupDeliveryEventTypesToShipment.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = 'e2176b1fe98afa2ba43ca7c61aed8895' WHERE [SxmlFileName] = '202012301040_UpdateChargeGroupById.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = '611c00bf79eb1de399089c4ae8189a41' WHERE [SxmlFileName] = '202105161305_AddCustomerProductItemEventTypesToTenants.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = 'e8e947aa23e770922cafa3580a5b3200' WHERE [SxmlFileName] = '202105301111_DeleteInsuredCreditLimitFromCustomerQuery.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = 'ef667edfcb0c5b514bb9f2a50077c314' WHERE [SxmlFileName] = 'UpdateAutomationWorkerRoleName.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = '681891ce355d3ec8ac984a9ae4b8c4d9' WHERE [SxmlFileName] = '202012311430_DontExecuteScriptAgain.sxml';

UPDATE [dbo].[DBScriptsHistory] SET  [ScriptBody] = 'NULL', [HashValue] = 'ca6fd61a011dc09396d6a9a333a90f40' WHERE [SxmlFileName] = '202101120950_ModifyDBMigrationLastScriptTrigger.sxml';

#endif


#if false
select * From objectfields f
RIGHT join ObjectFieldValidations v on f.FIELDCODE = v.OBJECTFIELDCODE

DELETE ObjectFieldValidations WHERE ID  IN ('1-251','1-252','1-253','1-254')
#endif
	}
}
