"use strict";
/*
 * [Note] To enable Typescript regions, install Web Essential extension: http://vswebessentials.com/
 */
Object.defineProperty(exports, "__esModule", { value: true });
//#region StandardList
var AddressContactStateListService_1 = require("./Services/StandardLists/AddressContactStateListService");
var AddressPurposeListService_1 = require("./Services/StandardLists/AddressPurposeListService");
var AmendmentFieldReasonTypeListService_1 = require("./Services/StandardLists/AmendmentFieldReasonTypeListService");
var AmendmentRequestStatusListService_1 = require("./Services/StandardLists/AmendmentRequestStatusListService");
var AssigneeNotificationTypeListService_1 = require("./Services/StandardLists/AssigneeNotificationTypeListService");
var AttachmentTypeListService_1 = require("./Services/StandardLists/AttachmentTypeListService");
var AuthorityListService_1 = require("./Services/StandardLists/AuthorityListService");
var AuthorizedSignerPermitListService_1 = require("./Services/StandardLists/AuthorizedSignerPermitListService");
var AutonomyTypeListService_1 = require("./Services/StandardLists/AutonomyTypeListService");
var BankListService_1 = require("./Services/StandardLists/BankListService");
var CargoIdentifireTypeListService_1 = require("./Services/StandardLists/CargoIdentifireTypeListService");
var CargoIdentityQualifierListService_1 = require("./Services/StandardLists/CargoIdentityQualifierListService");
var CertificateExemptionTypeListService_1 = require("./Services/StandardLists/CertificateExemptionTypeListService");
var CertificatesStatusListService_1 = require("./Services/StandardLists/CertificatesStatusListService");
var CheckEntityTypeListService_1 = require("./Services/StandardLists/CheckEntityTypeListService");
var CheckEssenceLookupListService_1 = require("./Services/StandardLists/CheckEssenceLookupListService");
var CheckQueueTypeListService_1 = require("./Services/StandardLists/CheckQueueTypeListService");
var CheckRepresentativeTypeListService_1 = require("./Services/StandardLists/CheckRepresentativeTypeListService");
var CheckTypeLookupListService_1 = require("./Services/StandardLists/CheckTypeLookupListService");
var CityListService_1 = require("./Services/StandardLists/CityListService");
var AgentTalkBackTypeListService_1 = require("./Services/StandardLists/AgentTalkBackTypeListService");
var ClaimEntityListService_1 = require("./Services/StandardLists/ClaimEntityListService");
var ClaimExplanationCodeListService_1 = require("./Services/StandardLists/ClaimExplanationCodeListService");
var ClaimListService_1 = require("./Services/StandardLists/ClaimListService");
var ClientListService_1 = require("./Services/StandardLists/ClientListService");
var ClosedTableStatusListService_1 = require("./Services/StandardLists/ClosedTableStatusListService");
var CollateralAnswerStatusListService_1 = require("./Services/StandardLists/CollateralAnswerStatusListService");
var CollateralAnswerTypeListService_1 = require("./Services/StandardLists/CollateralAnswerTypeListService");
var CollateralRequestStatusListService_1 = require("./Services/StandardLists/CollateralRequestStatusListService");
var CollateralTypeListService_1 = require("./Services/StandardLists/CollateralTypeListService");
var CommercialSaleListService_1 = require("./Services/StandardLists/CommercialSaleListService");
var CommunicationTypeListService_1 = require("./Services/StandardLists/CommunicationTypeListService");
var ConfirmationTypeListService_1 = require("./Services/StandardLists/ConfirmationTypeListService");
var ConstraintApprovalDecisionListService_1 = require("./Services/StandardLists/ConstraintApprovalDecisionListService");
var ConstraintProcessTypeListService_1 = require("./Services/StandardLists/ConstraintProcessTypeListService");
var ConstraintStatusListService_1 = require("./Services/StandardLists/ConstraintStatusListService");
var ConstraintTypeListService_1 = require("./Services/StandardLists/ConstraintTypeListService");
var ContactRoleTypeListService_1 = require("./Services/StandardLists/ContactRoleTypeListService");
var ContinuousMessagesTypeCodeListService_1 = require("./Services/StandardLists/ContinuousMessagesTypeCodeListService");
var ConverterTypeListService_1 = require("./Services/StandardLists/ConverterTypeListService");
var CountryGroupListService_1 = require("./Services/StandardLists/CountryGroupListService");
var CourtInstanceListService_1 = require("./Services/StandardLists/CourtInstanceListService");
var CurrencyTypeListService_1 = require("./Services/StandardLists/CurrencyTypeListService");
var CustomBankListService_1 = require("./Services/StandardLists/CustomBankListService");
var CustomDocumentTypeListService_1 = require("./Services/StandardLists/CustomDocumentTypeListService");
var CustomDocumentTypeMetaDataListService_1 = require("./Services/StandardLists/CustomDocumentTypeMetaDataListService");
var CustomerActivityTypeListService_1 = require("./Services/StandardLists/CustomerActivityTypeListService");
var CustomerIdentifyTypeListService_1 = require("./Services/StandardLists/CustomerIdentifyTypeListService");
var CustomerRoleTypeListService_1 = require("./Services/StandardLists/CustomerRoleTypeListService");
var CustomerTypeGeneralListService_1 = require("./Services/StandardLists/CustomerTypeGeneralListService");
var CustomMetaDataTypeListService_1 = require("./Services/StandardLists/CustomMetaDataTypeListService");
var CustomsAddressTypeListService_1 = require("./Services/StandardLists/CustomsAddressTypeListService");
var CustomsBookListService_1 = require("./Services/StandardLists/CustomsBookListService");
var CustomsBookTypeListService_1 = require("./Services/StandardLists/CustomsBookTypeListService");
var CustomsBranchListService_1 = require("./Services/StandardLists/CustomsBranchListService");
var CustomsClosedTableListService_1 = require("./Services/StandardLists/CustomsClosedTableListService");
var CustomsCollateralListService_1 = require("./Services/StandardLists/CustomsCollateralListService");
var CustomsCountryListService_1 = require("./Services/StandardLists/CustomsCountryListService");
var CustomsDocumentListService_1 = require("./Services/StandardLists/CustomsDocumentListService");
var CustomsDocumentStatusTypeListService_1 = require("./Services/StandardLists/CustomsDocumentStatusTypeListService");
var CustomsDocumentsTicketListService_1 = require("./Services/StandardLists/CustomsDocumentsTicketListService");
var CustomsEnvoirmentTypeListService_1 = require("./Services/StandardLists/CustomsEnvoirmentTypeListService");
var CustomsExchangeRateListService_1 = require("./Services/StandardLists/CustomsExchangeRateListService");
var CustomsHouseTypeAdditionalListService_1 = require("./Services/StandardLists/CustomsHouseTypeAdditionalListService");
var CustomsHouseTypeListService_1 = require("./Services/StandardLists/CustomsHouseTypeListService");
var CustomsInsuranceCompanyListService_1 = require("./Services/StandardLists/CustomsInsuranceCompanyListService");
var CustomsItemListService_1 = require("./Services/StandardLists/CustomsItemListService");
var CustomsPartnersItemListService_1 = require("./Services/StandardLists/CustomsPartnersItemListService");
var CustomsPaymentTermsListService_1 = require("./Services/StandardLists/CustomsPaymentTermsListService");
var CustomsRequestsSheetListService_1 = require("./Services/StandardLists/CustomsRequestsSheetListService");
var CustomsRequestsSheetStatusListService_1 = require("./Services/StandardLists/CustomsRequestsSheetStatusListService");
var CustomsRequiredFieldListService_1 = require("./Services/StandardLists/CustomsRequiredFieldListService");
var CustomsSettingListService_1 = require("./Services/StandardLists/CustomsSettingListService");
var CustomsTransportModeListService_1 = require("./Services/StandardLists/CustomsTransportModeListService");
var CustomsVendorListService_1 = require("./Services/StandardLists/CustomsVendorListService");
var CustomsVerificationStatusTypeListService_1 = require("./Services/StandardLists/CustomsVerificationStatusTypeListService");
var DangerousGoodsPackingReqListService_1 = require("./Services/StandardLists/DangerousGoodsPackingReqListService");
var DebtNotificationTypeListService_1 = require("./Services/StandardLists/DebtNotificationTypeListService");
var DeclarationErrorMappingListService_1 = require("./Services/StandardLists/DeclarationErrorMappingListService");
var DeclarationListService_1 = require("./Services/StandardLists/DeclarationListService");
var DeclarationPaymentListService_1 = require("./Services/StandardLists/DeclarationPaymentListService");
var DeclarationStatementTypeListService_1 = require("./Services/StandardLists/DeclarationStatementTypeListService");
var DeclarationStatusTypeListService_1 = require("./Services/StandardLists/DeclarationStatusTypeListService");
var DeficitConnFileParagraphTypeListService_1 = require("./Services/StandardLists/DeficitConnFileParagraphTypeListService");
var DeficitsListService_1 = require("./Services/StandardLists/DeficitsListService");
var DeliverySiteTypeListService_1 = require("./Services/StandardLists/DeliverySiteTypeListService");
var DemanderTypeListService_1 = require("./Services/StandardLists/DemanderTypeListService");
var DepositCustomerActivityListService_1 = require("./Services/StandardLists/DepositCustomerActivityListService");
var DepositEssenceTypeListService_1 = require("./Services/StandardLists/DepositEssenceTypeListService");
var DepositFileTypeListService_1 = require("./Services/StandardLists/DepositFileTypeListService");
var DepositListService_1 = require("./Services/StandardLists/DepositListService");
var EntitlementTypeListService_1 = require("./Services/StandardLists/EntitlementTypeListService");
var EntityTypeLookupListService_1 = require("./Services/StandardLists/EntityTypeLookupListService");
var FacilitationTypeListService_1 = require("./Services/StandardLists/FacilitationTypeListService");
var FaultInspectionTypeListService_1 = require("./Services/StandardLists/FaultInspectionTypeListService");
var FuelTypeListService_1 = require("./Services/StandardLists/FuelTypeListService");
var GenderListService_1 = require("./Services/StandardLists/GenderListService");
var GovernmentProcedureTypeListService_1 = require("./Services/StandardLists/GovernmentProcedureTypeListService");
var GuaranteeCertificateTypeListService_1 = require("./Services/StandardLists/GuaranteeCertificateTypeListService");
var GuaranteeCustomerActivityListService_1 = require("./Services/StandardLists/GuaranteeCustomerActivityListService");
var GuaranteeListService_1 = require("./Services/StandardLists/GuaranteeListService");
var ImporterDeclarationTypeListService_1 = require("./Services/StandardLists/ImporterDeclarationTypeListService");
var ImporterDespositionListService_1 = require("./Services/StandardLists/ImporterDespositionListService");
var ImporterPeriodicDeclarStatusListService_1 = require("./Services/StandardLists/ImporterPeriodicDeclarStatusListService");
var ImporterTypeForClaimListService_1 = require("./Services/StandardLists/ImporterTypeForClaimListService");
var InterfaceManagementListService_1 = require("./Services/StandardLists/InterfaceManagementListService");
var InterfaceSendOptionListService_1 = require("./Services/StandardLists/InterfaceSendOptionListService");
var InterfaceTenantDefinitionListService_1 = require("./Services/StandardLists/InterfaceTenantDefinitionListService");
var InternalBorderSiteTypeListService_1 = require("./Services/StandardLists/InternalBorderSiteTypeListService");
var InternationalSiteListService_1 = require("./Services/StandardLists/InternationalSiteListService");
var InvoiceTypeListService_1 = require("./Services/StandardLists/InvoiceTypeListService");
var ItemGovernmentProcedureTypeListService_1 = require("./Services/StandardLists/ItemGovernmentProcedureTypeListService");
var LastReleaseFromWarehouseListService_1 = require("./Services/StandardLists/LastReleaseFromWarehouseListService");
var LeadDocumentExceptionTypeListService_1 = require("./Services/StandardLists/LeadDocumentExceptionTypeListService");
var LeadDocumentTypeListService_1 = require("./Services/StandardLists/LeadDocumentTypeListService");
var MeasureQualifierListService_1 = require("./Services/StandardLists/MeasureQualifierListService");
var MeasurmentUnitListService_1 = require("./Services/StandardLists/MeasurmentUnitListService");
var ModificationAndDiscountTypeListService_1 = require("./Services/StandardLists/ModificationAndDiscountTypeListService");
var MorningMessageTypeListService_1 = require("./Services/StandardLists/MorningMessageTypeListService");
var NotificationDefinitionListService_1 = require("./Services/StandardLists/NotificationDefinitionListService");
var NotificationListService_1 = require("./Services/StandardLists/NotificationListService");
var NotificationTenantDefinitionListService_1 = require("./Services/StandardLists/NotificationTenantDefinitionListService");
var NotificationTypeListService_1 = require("./Services/StandardLists/NotificationTypeListService");
var OrganizationUnitTypeListService_1 = require("./Services/StandardLists/OrganizationUnitTypeListService");
var PackageMeasureQualifierListService_1 = require("./Services/StandardLists/PackageMeasureQualifierListService");
var PackingTypeListService_1 = require("./Services/StandardLists/PackingTypeListService");
var ParagraphTypeListService_1 = require("./Services/StandardLists/ParagraphTypeListService");
var PassportTypeListService_1 = require("./Services/StandardLists/PassportTypeListService");
var PayerActivityTypeListService_1 = require("./Services/StandardLists/PayerActivityTypeListService");
var PayerTypeListService_1 = require("./Services/StandardLists/PayerTypeListService");
var PaymentMethodStatusListService_1 = require("./Services/StandardLists/PaymentMethodStatusListService");
var PaymentMethodTypeListService_1 = require("./Services/StandardLists/PaymentMethodTypeListService");
var PaymentOrderListService_1 = require("./Services/StandardLists/PaymentOrderListService");
var PaymentOrderStatusListService_1 = require("./Services/StandardLists/PaymentOrderStatusListService");
var PaymentOrderTypeListService_1 = require("./Services/StandardLists/PaymentOrderTypeListService");
var PaymentProcessListService_1 = require("./Services/StandardLists/PaymentProcessListService");
var PaymentProtestTypeListService_1 = require("./Services/StandardLists/PaymentProtestTypeListService");
var PaymentTypeListService_1 = require("./Services/StandardLists/PaymentTypeListService");
var PhysicalCheckListService_1 = require("./Services/StandardLists/PhysicalCheckListService");
var PhysicalCheckOperationListService_1 = require("./Services/StandardLists/PhysicalCheckOperationListService");
var PhysicalCheckStatusMessageListService_1 = require("./Services/StandardLists/PhysicalCheckStatusMessageListService");
var ProceduralFaultInProcessTypeListService_1 = require("./Services/StandardLists/ProceduralFaultInProcessTypeListService");
var ProceduralFaultInSourceTypeListService_1 = require("./Services/StandardLists/ProceduralFaultInSourceTypeListService");
var ProceduralFaultListService_1 = require("./Services/StandardLists/ProceduralFaultListService");
var ProceduralFaultStatusListService_1 = require("./Services/StandardLists/ProceduralFaultStatusListService");
var ProceduralFaultTypeListService_1 = require("./Services/StandardLists/ProceduralFaultTypeListService");
var ProcessingReasonListService_1 = require("./Services/StandardLists/ProcessingReasonListService");
var ProductIdentificationTypeListService_1 = require("./Services/StandardLists/ProductIdentificationTypeListService");
var ProductNameTypeListService_1 = require("./Services/StandardLists/ProductNameTypeListService");
var RansomViolationTypeListService_1 = require("./Services/StandardLists/RansomViolationTypeListService");
var RegisteredWarehouseSiteTypeListService_1 = require("./Services/StandardLists/RegisteredWarehouseSiteTypeListService");
var RequestStatusListService_1 = require("./Services/StandardLists/RequestStatusListService");
var ReturnConditionListService_1 = require("./Services/StandardLists/ReturnConditionListService");
var SalesTaxExemptionTypeListService_1 = require("./Services/StandardLists/SalesTaxExemptionTypeListService");
var SignatureTypeListService_1 = require("./Services/StandardLists/SignatureTypeListService");
var SiteLookupListService_1 = require("./Services/StandardLists/SiteLookupListService");
var SiteTypeListService_1 = require("./Services/StandardLists/SiteTypeListService");
var SpecialActionDescriptionTypeListService_1 = require("./Services/StandardLists/SpecialActionDescriptionTypeListService");
var SpecializationTypeListService_1 = require("./Services/StandardLists/SpecializationTypeListService");
var StorageMessageTypeListService_1 = require("./Services/StandardLists/StorageMessageTypeListService");
var SubCountryListService_1 = require("./Services/StandardLists/SubCountryListService");
var SupplierInvoiceListService_1 = require("./Services/StandardLists/SupplierInvoiceListService");
var TapagConnectionTableListService_1 = require("./Services/StandardLists/TapagConnectionTableListService");
var TapagListService_1 = require("./Services/StandardLists/TapagListService");
var TapagTypeListService_1 = require("./Services/StandardLists/TapagTypeListService");
var TermsOfSaleTypeListService_1 = require("./Services/StandardLists/TermsOfSaleTypeListService");
var TradeAgreementListService_1 = require("./Services/StandardLists/TradeAgreementListService");
var TradeLevyExamptTypeListService_1 = require("./Services/StandardLists/TradeLevyExamptTypeListService");
var UnloadingSiteTypeListService_1 = require("./Services/StandardLists/UnloadingSiteTypeListService");
var ValidCustomsItemListService_1 = require("./Services/StandardLists/ValidCustomsItemListService");
var VehicleListService_1 = require("./Services/StandardLists/VehicleListService");
var VehicleManufacturerListService_1 = require("./Services/StandardLists/VehicleManufacturerListService");
var VehiclePoolTypeListService_1 = require("./Services/StandardLists/VehiclePoolTypeListService");
var VehiclePriceListTypeListService_1 = require("./Services/StandardLists/VehiclePriceListTypeListService");
var VehicleReductionTypeListService_1 = require("./Services/StandardLists/VehicleReductionTypeListService");
var VehicleSafeAccessoryInstlTypeListService_1 = require("./Services/StandardLists/VehicleSafeAccessoryInstlTypeListService");
var VehicleSafetyAccessoryTypeListService_1 = require("./Services/StandardLists/VehicleSafetyAccessoryTypeListService");
var VehicleStatusListService_1 = require("./Services/StandardLists/VehicleStatusListService");
var VehicleTecnologyTypeListService_1 = require("./Services/StandardLists/VehicleTecnologyTypeListService");
var VehicleTypeListService_1 = require("./Services/StandardLists/VehicleTypeListService");
var VendorCommissionListService_1 = require("./Services/StandardLists/VendorCommissionListService");
var VendorStatusListService_1 = require("./Services/StandardLists/VendorStatusListService");
var VendorTransactionTypeListService_1 = require("./Services/StandardLists/VendorTransactionTypeListService");
var VendorTypeListService_1 = require("./Services/StandardLists/VendorTypeListService");
var AccumalationStateListService_1 = require("./Services/StandardLists/AccumalationStateListService");
var CouriersVatListService_1 = require("./Services/StandardLists/CouriersVatListService");
var CourierPendingReasonListService_1 = require("./Services/StandardLists/CourierPendingReasonListService");
var StorageStatusListService_1 = require("./Services/StandardLists/StorageStatusListService");
var FreightPaymentMethodListService_1 = require("./Services/StandardLists/FreightPaymentMethodListService");
var CustomsDocumentsDefinitionListService_1 = require("./Services/StandardLists/CustomsDocumentsDefinitionListService");
var UIMessageListService_1 = require("./Services/StandardLists/UIMessageListService");
var UIMessageAdditionalListService_1 = require("./Services/StandardLists/UIMessageAdditionalListService");
var PointerLevelListService_1 = require("./Services/StandardLists/PointerLevelListService");
var CourierMasterListService_1 = require("./Services/StandardLists/CourierMasterListService");
var CourierDeclarationStatusListService_1 = require("./Services/StandardLists/CourierDeclarationStatusListService");
var DeclarationCourierStatusListService_1 = require("./Services/StandardLists/DeclarationCourierStatusListService");
var DeclarationCargoSplitListService_1 = require("./Services/StandardLists/DeclarationCargoSplitListService");
var ActionCodeListService_1 = require("./Services/StandardLists/ActionCodeListService");
var SplitOrMergeReasonListService_1 = require("./Services/StandardLists/SplitOrMergeReasonListService");
var CargoSplitRequestStatusListService_1 = require("./Services/StandardLists/CargoSplitRequestStatusListService");
var TreatmentWayListService_1 = require("./Services/StandardLists/TreatmentWayListService");
var TPGFileTypeListService_1 = require("./Services/StandardLists/TPGFileTypeListService");
var CustomsAirlineListService_1 = require("./Services/StandardLists/CustomsAirlineListService");
var PendingErrorPlaceListService_1 = require("./Services/StandardLists/PendingErrorPlaceListService");
var DecisionTypeListService_1 = require("./Services/StandardLists/DecisionTypeListService");
var SeizureMethodTypeListService_1 = require("./Services/StandardLists/SeizureMethodTypeListService");
var SeizureFactorTypeListService_1 = require("./Services/StandardLists/SeizureFactorTypeListService");
var RefundCustomerActivityTypeListService_1 = require("./Services/StandardLists/RefundCustomerActivityTypeListService");
//#endregion
//#region StandardPMs
var ClaimPMService_1 = require("./Services/StandardPMs/ClaimPMService");
var ClientPMService_1 = require("./Services/StandardPMs/ClientPMService");
var CustomBankPMService_1 = require("./Services/StandardPMs/CustomBankPMService");
var CustomsBookPMService_1 = require("./Services/StandardPMs/CustomsBookPMService");
var CustomsClosedTablePMService_1 = require("./Services/StandardPMs/CustomsClosedTablePMService");
var CustomsCollateralPMService_1 = require("./Services/StandardPMs/CustomsCollateralPMService");
var CustomsDocumentPMService_1 = require("./Services/StandardPMs/CustomsDocumentPMService");
var CustomsDocumentsTicketPMService_1 = require("./Services/StandardPMs/CustomsDocumentsTicketPMService");
var CustomsExchangeRatePMService_1 = require("./Services/StandardPMs/CustomsExchangeRatePMService");
var CustomsHouseTypeAdditionalPMService_1 = require("./Services/StandardPMs/CustomsHouseTypeAdditionalPMService");
var CustomsHouseTypePMService_1 = require("./Services/StandardPMs/CustomsHouseTypePMService");
var CustomsItemPMService_1 = require("./Services/StandardPMs/CustomsItemPMService");
var CustomsPartnersItemPMService_1 = require("./Services/StandardPMs/CustomsPartnersItemPMService");
var CustomsRequestsSheetPMService_1 = require("./Services/StandardPMs/CustomsRequestsSheetPMService");
var CustomsRequiredFieldPMService_1 = require("./Services/StandardPMs/CustomsRequiredFieldPMService");
var CustomsSettingPMService_1 = require("./Services/StandardPMs/CustomsSettingPMService");
var CustomsVendorPMService_1 = require("./Services/StandardPMs/CustomsVendorPMService");
var DeclarationErrorMappingPMService_1 = require("./Services/StandardPMs/DeclarationErrorMappingPMService");
var DeclarationPaymentPMService_1 = require("./Services/StandardPMs/DeclarationPaymentPMService");
var DeclarationPMService_1 = require("./Services/StandardPMs/DeclarationPMService");
var DeficitConnFileParagraphTypePMService_1 = require("./Services/StandardPMs/DeficitConnFileParagraphTypePMService");
var DeficitPMService_1 = require("./Services/StandardPMs/DeficitPMService");
var DepositPMService_1 = require("./Services/StandardPMs/DepositPMService");
var GovernmentProcedureTypePMService_1 = require("./Services/StandardPMs/GovernmentProcedureTypePMService");
var GuaranteePMService_1 = require("./Services/StandardPMs/GuaranteePMService");
var ImporterDespositionPMService_1 = require("./Services/StandardPMs/ImporterDespositionPMService");
var InterfaceManagementPMService_1 = require("./Services/StandardPMs/InterfaceManagementPMService");
var InterfaceTenantDefinitionPMService_1 = require("./Services/StandardPMs/InterfaceTenantDefinitionPMService");
var NotificationDefinitionPMService_1 = require("./Services/StandardPMs/NotificationDefinitionPMService");
var NotificationPMService_1 = require("./Services/StandardPMs/NotificationPMService");
var NotificationTenantDefinitionPMService_1 = require("./Services/StandardPMs/NotificationTenantDefinitionPMService");
var PaymentOrderPMService_1 = require("./Services/StandardPMs/PaymentOrderPMService");
var PhysicalCheckPMService_1 = require("./Services/StandardPMs/PhysicalCheckPMService");
var ProceduralFaultPMService_1 = require("./Services/StandardPMs/ProceduralFaultPMService");
var SupplierInvoicePMService_1 = require("./Services/StandardPMs/SupplierInvoicePMService");
var TapagConnectionTablePMService_1 = require("./Services/StandardPMs/TapagConnectionTablePMService");
var TapagPMService_1 = require("./Services/StandardPMs/TapagPMService");
var VehiclePMService_1 = require("./Services/StandardPMs/VehiclePMService");
var VendorCommissionPMService_1 = require("./Services/StandardPMs/VendorCommissionPMService");
var CouriersVatPMService_1 = require("./Services/StandardPMs/CouriersVatPMService");
var CourierPendingReasonPMService_1 = require("./Services/StandardPMs/CourierPendingReasonPMService");
var CourierMasterPMService_1 = require("./Services/StandardPMs/CourierMasterPMService");
var InternalBorderSiteTypePMService_1 = require("./Services/StandardPMs/InternalBorderSiteTypePMService");
var UIMessagePMService_1 = require("./Services/StandardPMs/UIMessagePMService");
var InternationalSitePMService_1 = require("./Services/StandardPMs/InternationalSitePMService");
var DeclarationCargoSplitPMService_1 = require("./Services/StandardPMs/DeclarationCargoSplitPMService");
var CustomsAirlinePMService_1 = require("./Services/StandardPMs/CustomsAirlinePMService");
//#endregion
//#region ExtendedLists
var CustomBankExtendedListService_1 = require("./Services/ExtendedLists/CustomBankExtendedListService");
var CustomsClosedTableExtendedListService_1 = require("./Services/ExtendedLists/CustomsClosedTableExtendedListService");
var CustomsRequestsSheetExtendedListService_1 = require("./Services/ExtendedLists/CustomsRequestsSheetExtendedListService");
var DeclarationExtendedListService_1 = require("./Services/ExtendedLists/DeclarationExtendedListService");
var SupplierInvoiceExtendedListService_1 = require("./Services/ExtendedLists/SupplierInvoiceExtendedListService");
var SupplierInvoiceFreightAmountExtendedListService_1 = require("./Services/ExtendedLists/SupplierInvoiceFreightAmountExtendedListService");
var SupplierInvoiceItemExtendedListService_1 = require("./Services/ExtendedLists/SupplierInvoiceItemExtendedListService");
//import { SupplierInvoiceItemsExtendedList } from './Services/ExtendedLists/SupplierInvoiceItemsExtendedList';
var NotificationExtendedListService_1 = require("./Services/ExtendedLists/NotificationExtendedListService");
var SupplierInvoiceItemsTaxExtendedListService_1 = require("./Services/ExtendedLists/SupplierInvoiceItemsTaxExtendedListService");
var SignStationExtendedListService_1 = require("./Services/ExtendedLists/SignStationExtendedListService");
var DeclarationCourierStatusExtendedListService_1 = require("./Services/ExtendedLists/DeclarationCourierStatusExtendedListService");
//#endregion
//#region ExtendedPMs
var CustomBankCardExtendedPMService_1 = require("./Services/ExtendedPMs/CustomBankCardExtendedPMService");
var CustomsDocumentPointersExtendedPMService_1 = require("./Services/ExtendedPMs/CustomsDocumentPointersExtendedPMService");
var CustomsDocumentsTicketsExtendedService_1 = require("./Services/ExtendedPMs/CustomsDocumentsTicketsExtendedService");
var CustomsExchangeRateExtendedPMService_1 = require("./Services/ExtendedPMs/CustomsExchangeRateExtendedPMService");
var CustomsHouseTypeExtendedPMService_1 = require("./Services/ExtendedPMs/CustomsHouseTypeExtendedPMService");
var CustomsRequestSheetExtendedPMService_1 = require("./Services/ExtendedPMs/CustomsRequestSheetExtendedPMService");
var PaymentOrderConnectionTableExtendedPMService_1 = require("./Services/ExtendedPMs/PaymentOrderConnectionTableExtendedPMService");
var SupplierInvoiceExtendedPMService_1 = require("./Services/ExtendedPMs/SupplierInvoiceExtendedPMService");
///????  import { CustomsRequestSheetExtendedPMService } from './Services/ExtendedPMs/CustomsRequestSheetExtendedPMService';
//#endregion
//#region WebServices
var ClientMessagesService_1 = require("./Services/WebServices/ClientMessagesService");
var CustDocMetaDataValuesWebService_1 = require("./Services/WebServices/CustDocMetaDataValuesWebService");
var CustDocRelatedDocsWebService_1 = require("./Services/WebServices/CustDocRelatedDocsWebService");
var CustDocsTicketWebService_1 = require("./Services/WebServices/CustDocsTicketWebService");
var DeclarationMessagesService_1 = require("./Services/WebServices/DeclarationMessagesService");
var DeclarationWebService_1 = require("./Services/WebServices/DeclarationWebService");
var IIGGeneralMessagesService_1 = require("./Services/WebServices/IIGGeneralMessagesService");
var PaymentMessagesService_1 = require("./Services/WebServices/PaymentMessagesService");
var PaymentOrderWebService_1 = require("./Services/WebServices/PaymentOrderWebService");
var QuantityTypeMessageService_1 = require("./Services/WebServices/QuantityTypeMessageService");
var TapagMessagesService_1 = require("./Services/WebServices/TapagMessagesService");
var VendorMessagesService_1 = require("./Services/WebServices/VendorMessagesService");
var LoadTestService_1 = require("./Services/WebServices/LoadTestService");
//#endregion
// Others
var GovernmentProcedureTypeDataChangeService_1 = require("./Services/DataChange/GovernmentProcedureTypeDataChangeService");
var CustomsDocumentPointerService_1 = require("./Services/Others/CustomsDocumentPointerService");
var CustomsRequestMenuService_1 = require("./Services/Others/CustomsRequestMenuService");
var MultiCertificatesService_1 = require("./Services/Others/MultiCertificatesService");
var SupplierInvoiceService_1 = require("./Services/Others/SupplierInvoiceService");
var DeclarationMenuButtonsHandler_1 = require("./Components/MenuButtons/DeclarationMenuButtonsHandler");
var VehicleMenuButtonsHandler_1 = require("./Components/MenuButtons/VehicleMenuButtonsHandler");
var PaymentOrderMenuButtonsHandler_1 = require("./Components/MenuButtons/PaymentOrderMenuButtonsHandler");
var ClaimMenuButtonsHandler_1 = require("./Components/MenuButtons/ClaimMenuButtonsHandler");
var PhysicalCheckMenuButtonsHandler_1 = require("./Components/MenuButtons/PhysicalCheckMenuButtonsHandler");
var DeclarationEditComponentController_1 = require("./Controller/DeclarationEditComponentController");
var VehicleEditComponentController_1 = require("./Controller/VehicleEditComponentController");
var VendorCommissionService_1 = require("./Services/WebServices/VendorCommissionService");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            //#region StandardList
            case "AddressContactStateListService": {
                myResult = new AddressContactStateListService_1.AddressContactStateListService();
                break;
            }
            case "AddressPurposeListService": {
                myResult = new AddressPurposeListService_1.AddressPurposeListService();
                break;
            }
            case "AmendmentFieldReasonTypeListService": {
                myResult = new AmendmentFieldReasonTypeListService_1.AmendmentFieldReasonTypeListService();
                break;
            }
            case "AmendmentRequestStatusListService": {
                myResult = new AmendmentRequestStatusListService_1.AmendmentRequestStatusListService();
                break;
            }
            case "AssigneeNotificationTypeListService": {
                myResult = new AssigneeNotificationTypeListService_1.AssigneeNotificationTypeListService();
                break;
            }
            case "AttachmentTypeListService": {
                myResult = new AttachmentTypeListService_1.AttachmentTypeListService();
                break;
            }
            case "AuthorityListService": {
                myResult = new AuthorityListService_1.AuthorityListService();
                break;
            }
            case "AuthorizedSignerPermitListService": {
                myResult = new AuthorizedSignerPermitListService_1.AuthorizedSignerPermitListService();
                break;
            }
            case "AutonomyTypeListService": {
                myResult = new AutonomyTypeListService_1.AutonomyTypeListService();
                break;
            }
            case "BankListService": {
                myResult = new BankListService_1.BankListService();
                break;
            }
            case "CargoIdentifireTypeListService": {
                myResult = new CargoIdentifireTypeListService_1.CargoIdentifireTypeListService();
                break;
            }
            case "CargoIdentityQualifierListService": {
                myResult = new CargoIdentityQualifierListService_1.CargoIdentityQualifierListService();
                break;
            }
            case "CertificateExemptionTypeListService": {
                myResult = new CertificateExemptionTypeListService_1.CertificateExemptionTypeListService();
                break;
            }
            case "CertificatesStatusListService": {
                myResult = new CertificatesStatusListService_1.CertificatesStatusListService();
                break;
            }
            case "CheckEntityTypeListService": {
                myResult = new CheckEntityTypeListService_1.CheckEntityTypeListService();
                break;
            }
            case "CheckEssenceLookupListService": {
                myResult = new CheckEssenceLookupListService_1.CheckEssenceLookupListService();
                break;
            }
            case "CheckQueueTypeListService": {
                myResult = new CheckQueueTypeListService_1.CheckQueueTypeListService();
                break;
            }
            case "CheckRepresentativeTypeListService": {
                myResult = new CheckRepresentativeTypeListService_1.CheckRepresentativeTypeListService();
                break;
            }
            case "CheckTypeLookupListService": {
                myResult = new CheckTypeLookupListService_1.CheckTypeLookupListService();
                break;
            }
            case "CityListService": {
                myResult = new CityListService_1.CityListService();
                break;
            }
            case "AgentTalkBackTypeListService": {
                myResult = new AgentTalkBackTypeListService_1.AgentTalkBackTypeListService();
                break;
            }
            case "ClaimEntityListService": {
                myResult = new ClaimEntityListService_1.ClaimEntityListService();
                break;
            }
            case "ClaimExplanationCodeListService": {
                myResult = new ClaimExplanationCodeListService_1.ClaimExplanationCodeListService();
                break;
            }
            case "ClaimListService": {
                myResult = new ClaimListService_1.ClaimListService();
                break;
            }
            case "ClientListService": {
                myResult = new ClientListService_1.ClientListService();
                break;
            }
            case "ClosedTableStatusListService": {
                myResult = new ClosedTableStatusListService_1.ClosedTableStatusListService();
                break;
            }
            case "CollateralAnswerStatusListService": {
                myResult = new CollateralAnswerStatusListService_1.CollateralAnswerStatusListService();
                break;
            }
            case "CollateralAnswerTypeListService": {
                myResult = new CollateralAnswerTypeListService_1.CollateralAnswerTypeListService();
                break;
            }
            case "CollateralRequestStatusListService": {
                myResult = new CollateralRequestStatusListService_1.CollateralRequestStatusListService();
                break;
            }
            case "CollateralTypeListService": {
                myResult = new CollateralTypeListService_1.CollateralTypeListService();
                break;
            }
            case "CommercialSaleListService": {
                myResult = new CommercialSaleListService_1.CommercialSaleListService();
                break;
            }
            case "CommunicationTypeListService": {
                myResult = new CommunicationTypeListService_1.CommunicationTypeListService();
                break;
            }
            case "ConfirmationTypeListService": {
                myResult = new ConfirmationTypeListService_1.ConfirmationTypeListService();
                break;
            }
            case "ConstraintApprovalDecisionListService": {
                myResult = new ConstraintApprovalDecisionListService_1.ConstraintApprovalDecisionListService();
                break;
            }
            case "ConstraintProcessTypeListService": {
                myResult = new ConstraintProcessTypeListService_1.ConstraintProcessTypeListService();
                break;
            }
            case "ConstraintStatusListService": {
                myResult = new ConstraintStatusListService_1.ConstraintStatusListService();
                break;
            }
            case "ConstraintTypeListService": {
                myResult = new ConstraintTypeListService_1.ConstraintTypeListService();
                break;
            }
            case "ContactRoleTypeListService": {
                myResult = new ContactRoleTypeListService_1.ContactRoleTypeListService();
                break;
            }
            case "ContinuousMessagesTypeCodeListService": {
                myResult = new ContinuousMessagesTypeCodeListService_1.ContinuousMessagesTypeCodeListService();
                break;
            }
            case "ConverterTypeListService": {
                myResult = new ConverterTypeListService_1.ConverterTypeListService();
                break;
            }
            case "CountryGroupListService": {
                myResult = new CountryGroupListService_1.CountryGroupListService();
                break;
            }
            case "CourtInstanceListService": {
                myResult = new CourtInstanceListService_1.CourtInstanceListService();
                break;
            }
            case "CurrencyTypeListService": {
                myResult = new CurrencyTypeListService_1.CurrencyTypeListService();
                break;
            }
            case "CustomBankListService": {
                myResult = new CustomBankListService_1.CustomBankListService();
                break;
            }
            case "CustomDocumentTypeListService": {
                myResult = new CustomDocumentTypeListService_1.CustomDocumentTypeListService();
                break;
            }
            case "CustomDocumentTypeMetaDataListService": {
                myResult = new CustomDocumentTypeMetaDataListService_1.CustomDocumentTypeMetaDataListService();
                break;
            }
            case "CustomerActivityTypeListService": {
                myResult = new CustomerActivityTypeListService_1.CustomerActivityTypeListService();
                break;
            }
            case "CustomerIdentifyTypeListService": {
                myResult = new CustomerIdentifyTypeListService_1.CustomerIdentifyTypeListService();
                break;
            }
            case "CustomerRoleTypeListService": {
                myResult = new CustomerRoleTypeListService_1.CustomerRoleTypeListService();
                break;
            }
            case "CustomerTypeGeneralListService": {
                myResult = new CustomerTypeGeneralListService_1.CustomerTypeGeneralListService();
                break;
            }
            case "CustomMetaDataTypeListService": {
                myResult = new CustomMetaDataTypeListService_1.CustomMetaDataTypeListService();
                break;
            }
            case "CustomsAddressTypeListService": {
                myResult = new CustomsAddressTypeListService_1.CustomsAddressTypeListService();
                break;
            }
            case "CustomsBookListService": {
                myResult = new CustomsBookListService_1.CustomsBookListService();
                break;
            }
            case "CustomsBookTypeListService": {
                myResult = new CustomsBookTypeListService_1.CustomsBookTypeListService();
                break;
            }
            case "CustomsBranchListService": {
                myResult = new CustomsBranchListService_1.CustomsBranchListService();
                break;
            }
            case "CustomsClosedTableListService": {
                myResult = new CustomsClosedTableListService_1.CustomsClosedTableListService();
                break;
            }
            case "CustomsCollateralListService": {
                myResult = new CustomsCollateralListService_1.CustomsCollateralListService();
                break;
            }
            case "CustomsCountryListService": {
                myResult = new CustomsCountryListService_1.CustomsCountryListService();
                break;
            }
            case "CustomsDocumentListService": {
                myResult = new CustomsDocumentListService_1.CustomsDocumentListService();
                break;
            }
            case "CustomsDocumentStatusTypeListService": {
                myResult = new CustomsDocumentStatusTypeListService_1.CustomsDocumentStatusTypeListService();
                break;
            }
            case "CustomsDocumentsTicketListService": {
                myResult = new CustomsDocumentsTicketListService_1.CustomsDocumentsTicketListService();
                break;
            }
            case "CustomsEnvoirmentTypeListService": {
                myResult = new CustomsEnvoirmentTypeListService_1.CustomsEnvoirmentTypeListService();
                break;
            }
            case "CustomsExchangeRateListService": {
                myResult = new CustomsExchangeRateListService_1.CustomsExchangeRateListService();
                break;
            }
            case "CustomsHouseTypeAdditionalListService": {
                myResult = new CustomsHouseTypeAdditionalListService_1.CustomsHouseTypeAdditionalListService();
                break;
            }
            case "CustomsHouseTypeListService": {
                myResult = new CustomsHouseTypeListService_1.CustomsHouseTypeListService();
                break;
            }
            case "CustomsInsuranceCompanyListService": {
                myResult = new CustomsInsuranceCompanyListService_1.CustomsInsuranceCompanyListService();
                break;
            }
            case "CustomsItemListService": {
                myResult = new CustomsItemListService_1.CustomsItemListService();
                break;
            }
            case "CustomsPartnersItemListService": {
                myResult = new CustomsPartnersItemListService_1.CustomsPartnersItemListService();
                break;
            }
            case "CustomsPaymentTermListService": {
                myResult = new CustomsPaymentTermsListService_1.CustomsPaymentTermListService();
                break;
            }
            case "CustomsRequestsSheetListService": {
                myResult = new CustomsRequestsSheetListService_1.CustomsRequestsSheetListService();
                break;
            }
            case "CustomsRequestsSheetStatusListService": {
                myResult = new CustomsRequestsSheetStatusListService_1.CustomsRequestsSheetStatusListService();
                break;
            }
            case "CustomsRequiredFieldListService": {
                myResult = new CustomsRequiredFieldListService_1.CustomsRequiredFieldListService();
                break;
            }
            case "CustomsSettingListService": {
                myResult = new CustomsSettingListService_1.CustomsSettingListService();
                break;
            }
            case "CustomsTransportModeListService": {
                myResult = new CustomsTransportModeListService_1.CustomsTransportModeListService();
                break;
            }
            case "CustomsVendorListService": {
                myResult = new CustomsVendorListService_1.CustomsVendorListService();
                break;
            }
            case "CustomsVerificationStatusTypeListService": {
                myResult = new CustomsVerificationStatusTypeListService_1.CustomsVerificationStatusTypeListService();
                break;
            }
            case "DangerousGoodsPackingReqListService": {
                myResult = new DangerousGoodsPackingReqListService_1.DangerousGoodsPackingReqListService();
                break;
            }
            case "DebtNotificationTypeListService": {
                myResult = new DebtNotificationTypeListService_1.DebtNotificationTypeListService();
                break;
            }
            case "DeclarationErrorMappingListService": {
                myResult = new DeclarationErrorMappingListService_1.DeclarationErrorMappingListService();
                break;
            }
            case "DeclarationListService": {
                myResult = new DeclarationListService_1.DeclarationListService();
                break;
            }
            case "DeclarationPaymentListService": {
                myResult = new DeclarationPaymentListService_1.DeclarationPaymentListService();
                break;
            }
            case "DeclarationStatementTypeListService": {
                myResult = new DeclarationStatementTypeListService_1.DeclarationStatementTypeListService();
                break;
            }
            case "DeclarationStatusTypeListService": {
                myResult = new DeclarationStatusTypeListService_1.DeclarationStatusTypeListService();
                break;
            }
            case "DeficitConnFileParagraphTypeListService": {
                myResult = new DeficitConnFileParagraphTypeListService_1.DeficitConnFileParagraphTypeListService();
                break;
            }
            case "DeficitListService": {
                myResult = new DeficitsListService_1.DeficitListService();
                break;
            }
            case "DeliverySiteTypeListService": {
                myResult = new DeliverySiteTypeListService_1.DeliverySiteTypeListService();
                break;
            }
            case "DemanderTypeListService": {
                myResult = new DemanderTypeListService_1.DemanderTypeListService();
                break;
            }
            case "DepositCustomerActivityListService": {
                myResult = new DepositCustomerActivityListService_1.DepositCustomerActivityListService();
                break;
            }
            case "DepositEssenceTypeListService": {
                myResult = new DepositEssenceTypeListService_1.DepositEssenceTypeListService();
                break;
            }
            case "DepositFileTypeListService": {
                myResult = new DepositFileTypeListService_1.DepositFileTypeListService();
                break;
            }
            case "DepositListService": {
                myResult = new DepositListService_1.DepositListService();
                break;
            }
            case "EntitlementTypeListService": {
                myResult = new EntitlementTypeListService_1.EntitlementTypeListService();
                break;
            }
            case "EntityTypeLookupListService": {
                myResult = new EntityTypeLookupListService_1.EntityTypeLookupListService();
                break;
            }
            case "FacilitationTypeListService": {
                myResult = new FacilitationTypeListService_1.FacilitationTypeListService();
                break;
            }
            case "FaultInspectionTypeListService": {
                myResult = new FaultInspectionTypeListService_1.FaultInspectionTypeListService();
                break;
            }
            case "FuelTypeListService": {
                myResult = new FuelTypeListService_1.FuelTypeListService();
                break;
            }
            case "GenderListService": {
                myResult = new GenderListService_1.GenderListService();
                break;
            }
            case "GovernmentProcedureTypeListService": {
                myResult = new GovernmentProcedureTypeListService_1.GovernmentProcedureTypeListService();
                break;
            }
            case "GuaranteeCertificateTypeListService": {
                myResult = new GuaranteeCertificateTypeListService_1.GuaranteeCertificateTypeListService();
                break;
            }
            case "GuaranteeCustomerActivityListService": {
                myResult = new GuaranteeCustomerActivityListService_1.GuaranteeCustomerActivityListService();
                break;
            }
            case "GuaranteeListService": {
                myResult = new GuaranteeListService_1.GuaranteeListService();
                break;
            }
            case "ImporterDeclarationTypeListService": {
                myResult = new ImporterDeclarationTypeListService_1.ImporterDeclarationTypeListService();
                break;
            }
            case "ImporterDespositionListService": {
                myResult = new ImporterDespositionListService_1.ImporterDespositionListService();
                break;
            }
            case "ImporterPeriodicDeclarStatusListService": {
                myResult = new ImporterPeriodicDeclarStatusListService_1.ImporterPeriodicDeclarStatusListService();
                break;
            }
            case "ImporterTypeForClaimListService": {
                myResult = new ImporterTypeForClaimListService_1.ImporterTypeForClaimListService();
                break;
            }
            case "InterfaceManagementListService": {
                myResult = new InterfaceManagementListService_1.InterfaceManagementListService();
                break;
            }
            case "InterfaceSendOptionListService": {
                myResult = new InterfaceSendOptionListService_1.InterfaceSendOptionListService();
                break;
            }
            case "InterfaceTenantDefinitionListService": {
                myResult = new InterfaceTenantDefinitionListService_1.InterfaceTenantDefinitionListService();
                break;
            }
            case "InternalBorderSiteTypeListService": {
                myResult = new InternalBorderSiteTypeListService_1.InternalBorderSiteTypeListService();
                break;
            }
            case "InternationalSiteListService": {
                myResult = new InternationalSiteListService_1.InternationalSiteListService();
                break;
            }
            case "InvoiceTypeListService": {
                myResult = new InvoiceTypeListService_1.InvoiceTypeListService();
                break;
            }
            case "ItemGovernmentProcedureTypeListService": {
                myResult = new ItemGovernmentProcedureTypeListService_1.ItemGovernmentProcedureTypeListService();
                break;
            }
            case "LastReleaseFromWarehouseListService": {
                myResult = new LastReleaseFromWarehouseListService_1.LastReleaseFromWarehouseListService();
                break;
            }
            case "LeadDocumentExceptionTypeListService": {
                myResult = new LeadDocumentExceptionTypeListService_1.LeadDocumentExceptionTypeListService();
                break;
            }
            case "LeadDocumentTypeListService": {
                myResult = new LeadDocumentTypeListService_1.LeadDocumentTypeListService();
                break;
            }
            case "MeasureQualifierListService": {
                myResult = new MeasureQualifierListService_1.MeasureQualifierListService();
                break;
            }
            case "MeasurmentUnitListService": {
                myResult = new MeasurmentUnitListService_1.MeasurmentUnitListService();
                break;
            }
            case "ModificationAndDiscountTypeListService": {
                myResult = new ModificationAndDiscountTypeListService_1.ModificationAndDiscountTypeListService();
                break;
            }
            case "MorningMessageTypeListService": {
                myResult = new MorningMessageTypeListService_1.MorningMessageTypeListService();
                break;
            }
            case "NotificationDefinitionListService": {
                myResult = new NotificationDefinitionListService_1.NotificationDefinitionListService();
                break;
            }
            case "NotificationListService": {
                myResult = new NotificationListService_1.NotificationListService();
                break;
            }
            case "NotificationTenantDefinitionListService": {
                myResult = new NotificationTenantDefinitionListService_1.NotificationTenantDefinitionListService();
                break;
            }
            case "NotificationTypeListService": {
                myResult = new NotificationTypeListService_1.NotificationTypeListService();
                break;
            }
            case "OrganizationUnitTypeListService": {
                myResult = new OrganizationUnitTypeListService_1.OrganizationUnitTypeListService();
                break;
            }
            case "PackageMeasureQualifierListService": {
                myResult = new PackageMeasureQualifierListService_1.PackageMeasureQualifierListService();
                break;
            }
            case "PackingTypeListService": {
                myResult = new PackingTypeListService_1.PackingTypeListService();
                break;
            }
            case "ParagraphTypeListService": {
                myResult = new ParagraphTypeListService_1.ParagraphTypeListService();
                break;
            }
            case "PassportTypeListService": {
                myResult = new PassportTypeListService_1.PassportTypeListService();
                break;
            }
            case "PayerActivityTypeListService": {
                myResult = new PayerActivityTypeListService_1.PayerActivityTypeListService();
                break;
            }
            case "PayerTypeListService": {
                myResult = new PayerTypeListService_1.PayerTypeListService();
                break;
            }
            case "PaymentMethodStatusListService": {
                myResult = new PaymentMethodStatusListService_1.PaymentMethodStatusListService();
                break;
            }
            case "PaymentMethodTypeListService": {
                myResult = new PaymentMethodTypeListService_1.PaymentMethodTypeListService();
                break;
            }
            case "PaymentOrderListService": {
                myResult = new PaymentOrderListService_1.PaymentOrderListService();
                break;
            }
            case "PaymentOrderStatusListService": {
                myResult = new PaymentOrderStatusListService_1.PaymentOrderStatusListService();
                break;
            }
            case "PaymentOrderTypeListService": {
                myResult = new PaymentOrderTypeListService_1.PaymentOrderTypeListService();
                break;
            }
            case "PaymentProcessListService": {
                myResult = new PaymentProcessListService_1.PaymentProcessListService();
                break;
            }
            case "PaymentProtestTypeListService": {
                myResult = new PaymentProtestTypeListService_1.PaymentProtestTypeListService();
                break;
            }
            case "PaymentTypeListService": {
                myResult = new PaymentTypeListService_1.PaymentTypeListService();
                break;
            }
            case "PhysicalCheckListService": {
                myResult = new PhysicalCheckListService_1.PhysicalCheckListService();
                break;
            }
            case "PhysicalCheckOperationListService": {
                myResult = new PhysicalCheckOperationListService_1.PhysicalCheckOperationListService();
                break;
            }
            case "PhysicalCheckStatusMessageListService": {
                myResult = new PhysicalCheckStatusMessageListService_1.PhysicalCheckStatusMessageListService();
                break;
            }
            case "ProceduralFaultInProcessTypeListService": {
                myResult = new ProceduralFaultInProcessTypeListService_1.ProceduralFaultInProcessTypeListService();
                break;
            }
            case "ProceduralFaultInSourceTypeListService": {
                myResult = new ProceduralFaultInSourceTypeListService_1.ProceduralFaultInSourceTypeListService();
                break;
            }
            case "ProceduralFaultListService": {
                myResult = new ProceduralFaultListService_1.ProceduralFaultListService();
                break;
            }
            case "ProceduralFaultStatusListService": {
                myResult = new ProceduralFaultStatusListService_1.ProceduralFaultStatusListService();
                break;
            }
            case "ProceduralFaultTypeListService": {
                myResult = new ProceduralFaultTypeListService_1.ProceduralFaultTypeListService();
                break;
            }
            case "ProcessingReasonListService": {
                myResult = new ProcessingReasonListService_1.ProcessingReasonListService();
                break;
            }
            case "ProductIdentificationTypeListService": {
                myResult = new ProductIdentificationTypeListService_1.ProductIdentificationTypeListService();
                break;
            }
            case "ProductNameTypeListService": {
                myResult = new ProductNameTypeListService_1.ProductNameTypeListService();
                break;
            }
            case "RansomViolationTypeListService": {
                myResult = new RansomViolationTypeListService_1.RansomViolationTypeListService();
                break;
            }
            case "RegisteredWarehouseSiteTypeListService": {
                myResult = new RegisteredWarehouseSiteTypeListService_1.RegisteredWarehouseSiteTypeListService();
                break;
            }
            case "RequestStatusListService": {
                myResult = new RequestStatusListService_1.RequestStatusListService();
                break;
            }
            case "ReturnConditionListService": {
                myResult = new ReturnConditionListService_1.ReturnConditionListService();
                break;
            }
            case "SalesTaxExemptionTypeListService": {
                myResult = new SalesTaxExemptionTypeListService_1.SalesTaxExemptionTypeListService();
                break;
            }
            case "SignatureTypeListService": {
                myResult = new SignatureTypeListService_1.SignatureTypeListService();
                break;
            }
            case "SiteLookupListService": {
                myResult = new SiteLookupListService_1.SiteLookupListService();
                break;
            }
            case "SiteTypeListService": {
                myResult = new SiteTypeListService_1.SiteTypeListService();
                break;
            }
            case "SpecialActionDescriptionTypeListService": {
                myResult = new SpecialActionDescriptionTypeListService_1.SpecialActionDescriptionTypeListService();
                break;
            }
            case "SpecializationTypeListService": {
                myResult = new SpecializationTypeListService_1.SpecializationTypeListService();
                break;
            }
            case "StorageMessageTypeListService": {
                myResult = new StorageMessageTypeListService_1.StorageMessageTypeListService();
                break;
            }
            case "SubCountryListService": {
                myResult = new SubCountryListService_1.SubCountryListService();
                break;
            }
            case "SupplierInvoiceListService": {
                myResult = new SupplierInvoiceListService_1.SupplierInvoiceListService();
                break;
            }
            case "TapagConnectionTableListService": {
                myResult = new TapagConnectionTableListService_1.TapagConnectionTableListService();
                break;
            }
            case "TapagListService": {
                myResult = new TapagListService_1.TapagListService();
                break;
            }
            case "TapagTypeListService": {
                myResult = new TapagTypeListService_1.TapagTypeListService();
                break;
            }
            case "TermsOfSaleTypeListService": {
                myResult = new TermsOfSaleTypeListService_1.TermsOfSaleTypeListService();
                break;
            }
            case "TradeAgreementListService": {
                myResult = new TradeAgreementListService_1.TradeAgreementListService();
                break;
            }
            case "TradeLevyExamptTypeListService": {
                myResult = new TradeLevyExamptTypeListService_1.TradeLevyExamptTypeListService();
                break;
            }
            case "UnloadingSiteTypeListService": {
                myResult = new UnloadingSiteTypeListService_1.UnloadingSiteTypeListService();
                break;
            }
            case "ValidCustomsItemListService": {
                myResult = new ValidCustomsItemListService_1.ValidCustomsItemListService();
                break;
            }
            case "VehicleListService": {
                myResult = new VehicleListService_1.VehicleListService();
                break;
            }
            case "VehicleManufacturerListService": {
                myResult = new VehicleManufacturerListService_1.VehicleManufacturerListService();
                break;
            }
            case "VehiclePoolTypeListService": {
                myResult = new VehiclePoolTypeListService_1.VehiclePoolTypeListService();
                break;
            }
            case "VehiclePriceListTypeListService": {
                myResult = new VehiclePriceListTypeListService_1.VehiclePriceListTypeListService();
                break;
            }
            case "VehicleReductionTypeListService": {
                myResult = new VehicleReductionTypeListService_1.VehicleReductionTypeListService();
                break;
            }
            case "VehicleSafeAccessoryInstlTypeListService": {
                myResult = new VehicleSafeAccessoryInstlTypeListService_1.VehicleSafeAccessoryInstlTypeListService();
                break;
            }
            case "VehicleSafetyAccessoryTypeListService": {
                myResult = new VehicleSafetyAccessoryTypeListService_1.VehicleSafetyAccessoryTypeListService();
                break;
            }
            case "VehicleStatusListService": {
                myResult = new VehicleStatusListService_1.VehicleStatusListService();
                break;
            }
            case "VehicleTecnologyTypeListService": {
                myResult = new VehicleTecnologyTypeListService_1.VehicleTecnologyTypeListService();
                break;
            }
            case "VehicleTypeListService": {
                myResult = new VehicleTypeListService_1.VehicleTypeListService();
                break;
            }
            case "VendorCommissionListService": {
                myResult = new VendorCommissionListService_1.VendorCommissionListService();
                break;
            }
            case "VendorStatusListService": {
                myResult = new VendorStatusListService_1.VendorStatusListService();
                break;
            }
            case "VendorTransactionTypeListService": {
                myResult = new VendorTransactionTypeListService_1.VendorTransactionTypeListService();
                break;
            }
            case "VendorTypeListService": {
                myResult = new VendorTypeListService_1.VendorTypeListService();
                break;
            }
            case "AccumalationStateListService": {
                myResult = new AccumalationStateListService_1.AccumalationStateListService();
                break;
            }
            case "CouriersVatListService": {
                myResult = new CouriersVatListService_1.CouriersVatListService();
                break;
            }
            case "CourierPendingReasonListService": {
                myResult = new CourierPendingReasonListService_1.CourierPendingReasonListService();
                break;
            }
            case "StorageStatusListService": {
                myResult = new StorageStatusListService_1.StorageStatusListService();
                break;
            }
            case "FreightPaymentMethodListService": {
                myResult = new FreightPaymentMethodListService_1.FreightPaymentMethodListService();
                break;
            }
            case "CustomsDocumentsDefinitionListService": {
                myResult = new CustomsDocumentsDefinitionListService_1.CustomsDocumentsDefinitionListService();
                break;
            }
            case "UIMessageListService": {
                myResult = new UIMessageListService_1.UIMessageListService();
                break;
            }
            case "UIMessageAdditionalListService": {
                myResult = new UIMessageAdditionalListService_1.UIMessageAdditionalListService();
                break;
            }
            case "Customs.PointerLevelListService": {
                myResult = new PointerLevelListService_1.PointerLevelListService();
                break;
            }
            case "PointerLevelListService": {
                myResult = new PointerLevelListService_1.PointerLevelListService();
                break;
            }
            case "CourierMasterListService": {
                myResult = new CourierMasterListService_1.CourierMasterListService();
                break;
            }
            case "CourierDeclarationStatusListService": {
                myResult = new CourierDeclarationStatusListService_1.CourierDeclarationStatusListService();
                break;
            }
            case "DeclarationCourierStatusListService": {
                myResult = new DeclarationCourierStatusListService_1.DeclarationCourierStatusListService();
                break;
            }
            case "DeclarationCargoSplitListService": {
                myResult = new DeclarationCargoSplitListService_1.DeclarationCargoSplitListService();
                break;
            }
            case "ActionCodeListService": {
                myResult = new ActionCodeListService_1.ActionCodeListService();
                break;
            }
            case "SplitOrMergeReasonListService": {
                myResult = new SplitOrMergeReasonListService_1.SplitOrMergeReasonListService();
                break;
            }
            case "CargoSplitRequestStatusListService": {
                myResult = new CargoSplitRequestStatusListService_1.CargoSplitRequestStatusListService();
                break;
            }
            case "TreatmentWayListService": {
                myResult = new TreatmentWayListService_1.TreatmentWayListService();
                break;
            }
            case "TPGFileTypeListService": {
                myResult = new TPGFileTypeListService_1.TPGFileTypeListService();
                break;
            }
            case "CustomsAirlineListService": {
                myResult = new CustomsAirlineListService_1.CustomsAirlineListService();
                break;
            }
            case "PendingErrorPlaceListService": {
                myResult = new PendingErrorPlaceListService_1.PendingErrorPlaceListService();
                break;
            }
            case "DecisionTypeListService": {
                myResult = new DecisionTypeListService_1.DecisionTypeListService();
                break;
            }
            case "SeizureMethodTypeListService": {
                myResult = new SeizureMethodTypeListService_1.SeizureMethodTypeListService();
                break;
            }
            case "SeizureFactorTypeListService": {
                myResult = new SeizureFactorTypeListService_1.SeizureFactorTypeListService();
                break;
            }
            case "RefundCustomerActivityTypeListService": {
                myResult = new RefundCustomerActivityTypeListService_1.RefundCustomerActivityTypeListService();
                break;
            }
            //#endregion                
            //#region StandardPMs
            case "ClaimPMService": {
                myResult = new ClaimPMService_1.ClaimPMService();
                break;
            }
            case "ClientPMService": {
                myResult = new ClientPMService_1.ClientPMService();
                break;
            }
            case "CustomBankPMService": {
                myResult = new CustomBankPMService_1.CustomBankPMService();
                break;
            }
            case "CustomsBookPMService": {
                myResult = new CustomsBookPMService_1.CustomsBookPMService();
                break;
            }
            case "CustomsClosedTablePMService": {
                myResult = new CustomsClosedTablePMService_1.CustomsClosedTablePMService();
                break;
            }
            case "CustomsCollateralPMService": {
                myResult = new CustomsCollateralPMService_1.CustomsCollateralPMService();
                break;
            }
            case "CustomsDocumentPMService": {
                myResult = new CustomsDocumentPMService_1.CustomsDocumentPMService();
                break;
            }
            case "CustomsDocumentsTicketPMService": {
                myResult = new CustomsDocumentsTicketPMService_1.CustomsDocumentsTicketPMService();
                break;
            }
            case "CustomsExchangeRatePMService": {
                myResult = new CustomsExchangeRatePMService_1.CustomsExchangeRatePMService();
                break;
            }
            case "CustomsHouseTypeAdditionalPMService": {
                myResult = new CustomsHouseTypeAdditionalPMService_1.CustomsHouseTypeAdditionalPMService();
                break;
            }
            case "CustomsHouseTypePMService": {
                myResult = new CustomsHouseTypePMService_1.CustomsHouseTypePMService();
                break;
            }
            case "CustomsItemPMService": {
                myResult = new CustomsItemPMService_1.CustomsItemPMService();
                break;
            }
            case "CustomsPartnersItemPMService": {
                myResult = new CustomsPartnersItemPMService_1.CustomsPartnersItemPMService();
                break;
            }
            case "CustomsRequestsSheetPMService": {
                myResult = new CustomsRequestsSheetPMService_1.CustomsRequestsSheetPMService();
                break;
            }
            case "CustomsRequiredFieldPMService": {
                myResult = new CustomsRequiredFieldPMService_1.CustomsRequiredFieldPMService();
                break;
            }
            case "CustomsSettingPMService": {
                myResult = new CustomsSettingPMService_1.CustomsSettingPMService();
                break;
            }
            case "CustomsVendorPMService": {
                myResult = new CustomsVendorPMService_1.CustomsVendorPMService();
                break;
            }
            case "DeclarationErrorMappingPMService": {
                myResult = new DeclarationErrorMappingPMService_1.DeclarationErrorMappingPMService();
                break;
            }
            case "DeclarationPaymentPMService": {
                myResult = new DeclarationPaymentPMService_1.DeclarationPaymentPMService();
                break;
            }
            case "DeclarationPMService": {
                myResult = new DeclarationPMService_1.DeclarationPMService();
                break;
            }
            case "DeficitConnFileParagraphTypePMService": {
                myResult = new DeficitConnFileParagraphTypePMService_1.DeficitConnFileParagraphTypePMService();
                break;
            }
            case "DeficitPMService": {
                myResult = new DeficitPMService_1.DeficitPMService();
                break;
            }
            case "DepositPMService": {
                myResult = new DepositPMService_1.DepositPMService();
                break;
            }
            case "GovernmentProcedureTypePMService": {
                myResult = new GovernmentProcedureTypePMService_1.GovernmentProcedureTypePMService();
                break;
            }
            case "GuaranteePMService": {
                myResult = new GuaranteePMService_1.GuaranteePMService();
                break;
            }
            case "ImporterDespositionPMService": {
                myResult = new ImporterDespositionPMService_1.ImporterDespositionPMService();
                break;
            }
            case "InterfaceManagementPMService": {
                myResult = new InterfaceManagementPMService_1.InterfaceManagementPMService();
                break;
            }
            case "InterfaceTenantDefinitionPMService": {
                myResult = new InterfaceTenantDefinitionPMService_1.InterfaceTenantDefinitionPMService();
                break;
            }
            case "NotificationDefinitionPMService": {
                myResult = new NotificationDefinitionPMService_1.NotificationDefinitionPMService();
                break;
            }
            case "NotificationPMService": {
                myResult = new NotificationPMService_1.NotificationPMService();
                break;
            }
            case "NotificationTenantDefinitionPMService": {
                myResult = new NotificationTenantDefinitionPMService_1.NotificationTenantDefinitionPMService();
                break;
            }
            case "PaymentOrderPMService": {
                myResult = new PaymentOrderPMService_1.PaymentOrderPMService();
                break;
            }
            case "PhysicalCheckPMService": {
                myResult = new PhysicalCheckPMService_1.PhysicalCheckPMService();
                break;
            }
            case "ProceduralFaultPMService": {
                myResult = new ProceduralFaultPMService_1.ProceduralFaultPMService();
                break;
            }
            case "ProceduralFaultsPMService": {
                myResult = new ProceduralFaultPMService_1.ProceduralFaultPMService();
                break;
            }
            case "SupplierInvoicePMService": {
                myResult = new SupplierInvoicePMService_1.SupplierInvoicePMService();
                break;
            }
            case "TapagConnectionTablePMService": {
                myResult = new TapagConnectionTablePMService_1.TapagConnectionTablePMService();
                break;
            }
            case "TapagPMService": {
                myResult = new TapagPMService_1.TapagPMService();
                break;
            }
            case "VehiclePMService": {
                myResult = new VehiclePMService_1.VehiclePMService();
                break;
            }
            case "VendorCommissionPMService": {
                myResult = new VendorCommissionPMService_1.VendorCommissionPMService();
                break;
            }
            case "CouriersVatPMService": {
                myResult = new CouriersVatPMService_1.CouriersVatPMService();
                break;
            }
            case "CourierPendingReasonPMService": {
                myResult = new CourierPendingReasonPMService_1.CourierPendingReasonPMService();
                break;
            }
            case "CourierMasterPMService": {
                myResult = new CourierMasterPMService_1.CourierMasterPMService();
                break;
            }
            case "CustomDocumentTypePMService": {
                myResult = new CustomDocumentTypePMService_1.CustomDocumentTypePMService();
                break;
            }
            case "InternalBorderSiteTypePMService": {
                myResult = new InternalBorderSiteTypePMService_1.custominternakl();
                break;
            }
            case "UIMessagePMService": {
                myResult = new UIMessagePMService_1.UIMessagePMService();
                break;
            }
            case "InternationalSitePMService": {
                myResult = new InternationalSitePMService_1.InternationalSitePMService();
                break;
            }
            case "DeclarationCargoSplitPMService": {
                myResult = new DeclarationCargoSplitPMService_1.DeclarationCargoSplitPMService();
                break;
            }
            case "CustomsAirlinePMService": {
                myResult = new CustomsAirlinePMService_1.CustomsAirlinePMService();
                break;
            }
            //#endregion
            //#region ExtendedLists
            case "CustomBankExtendedListService": {
                myResult = new CustomBankExtendedListService_1.CustomBankExtendedListService();
                break;
            }
            case "CustomsClosedTableExtendedListService": {
                myResult = new CustomsClosedTableExtendedListService_1.CustomsClosedTableExtendedListService();
                break;
            }
            case "CustomsRequestsSheetExtendedListService": {
                myResult = new CustomsRequestsSheetExtendedListService_1.CustomsRequestsSheetExtendedListService();
                break;
            }
            case "DeclarationExtendedListService": {
                myResult = new DeclarationExtendedListService_1.DeclarationExtendedListService();
                break;
            }
            case "SupplierInvoiceExtendedListService": {
                myResult = new SupplierInvoiceExtendedListService_1.SupplierInvoiceExtendedListService();
                break;
            }
            case "SupplierInvoiceFreightAmountExtendedListService": {
                myResult = new SupplierInvoiceFreightAmountExtendedListService_1.SupplierInvoiceFreightAmountExtendedListService();
                break;
            }
            case "SupplierInvoiceItemExtendedListService": {
                myResult = new SupplierInvoiceItemExtendedListService_1.SupplierInvoiceItemExtendedListService();
                break;
            }
            case "NotificationExtendedListService": {
                myResult = new NotificationExtendedListService_1.NotificationExtendedListService();
                break;
            }
            case "SupplierInvoiceItemsTaxExtendedListService": {
                myResult = new SupplierInvoiceItemsTaxExtendedListService_1.SupplierInvoiceItemsTaxExtendedListService();
                break;
            }
            case "SignStationExtendedListService": {
                myResult = new SignStationExtendedListService_1.SignStationExtendedListService();
                break;
            }
            case "DeclarationCourierStatusExtendedListService": {
                myResult = new DeclarationCourierStatusExtendedListService_1.DeclarationCourierStatusExtendedListService();
                break;
            }
            //case "SupplierInvoiceItemsExtendedList": { myResult = new SupplierInvoiceItemsExtendedList(); break; } 
            //#endregion
            //#region ExtendedPMs
            case "CustomBankCardExtendedPMService": {
                myResult = new CustomBankCardExtendedPMService_1.CustomBankCardExtendedPMService();
                break;
            }
            case "CustomsDocumentPointersExtendedPMService": {
                myResult = new CustomsDocumentPointersExtendedPMService_1.CustomsDocumentPointersExtendedPMService();
                break;
            }
            case "CustomsDocumentsTicketsExtendedService": {
                myResult = new CustomsDocumentsTicketsExtendedService_1.CustomsDocumentsTicketsExtendedService();
                break;
            }
            case "CustomsExchangeRateExtendedPMService": {
                myResult = new CustomsExchangeRateExtendedPMService_1.CustomsExchangeRateExtendedPMService();
                break;
            }
            case "CustomsHouseTypeExtendedPMService": {
                myResult = new CustomsHouseTypeExtendedPMService_1.CustomsHouseTypeExtendedPMService();
                break;
            }
            case "CustomsRequestSheetExtendedPMService": {
                myResult = new CustomsRequestSheetExtendedPMService_1.CustomsRequestSheetExtendedPMService();
                break;
            }
            case "PaymentOrderConnectionTableExtendedPMService": {
                myResult = new PaymentOrderConnectionTableExtendedPMService_1.PaymentOrderConnectionTableExtendedPMService();
                break;
            }
            case "SupplierInvoiceExtendedPMService": {
                myResult = new SupplierInvoiceExtendedPMService_1.SupplierInvoiceExtendedPMService();
                break;
            }
            //#endregion
            //#region WebServices
            case "ClientMessagesService": {
                myResult = new ClientMessagesService_1.ClientMessagesService();
                break;
            }
            case "CustDocMetaDataValuesWebService": {
                myResult = new CustDocMetaDataValuesWebService_1.CustDocMetaDataValuesWebService();
                break;
            }
            case "CustDocRelatedDocsWebService": {
                myResult = new CustDocRelatedDocsWebService_1.CustDocRelatedDocsWebService();
                break;
            }
            case "CustDocsTicketWebService": {
                myResult = new CustDocsTicketWebService_1.CustDocsTicketWebService();
                break;
            }
            case "DeclarationMessagesService": {
                myResult = new DeclarationMessagesService_1.DeclarationMessagesService();
                break;
            }
            case "DeclarationWebService": {
                myResult = new DeclarationWebService_1.DeclarationWebService();
                break;
            }
            case "IIGGeneralMessagesService": {
                myResult = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
                break;
            }
            case "PaymentMessagesService": {
                myResult = new PaymentMessagesService_1.PaymentMessagesService();
                break;
            }
            case "PaymentOrderWebService": {
                myResult = new PaymentOrderWebService_1.PaymentOrderWebService();
                break;
            }
            case "QuantityTypeMessageService": {
                myResult = new QuantityTypeMessageService_1.QuantityTypeMessageService();
                break;
            }
            case "TapagMessagesService": {
                myResult = new TapagMessagesService_1.TapagMessagesService();
                break;
            }
            case "VendorMessagesService": {
                myResult = new VendorMessagesService_1.VendorMessagesService();
                break;
            }
            case "LoadTestService": {
                myResult = new LoadTestService_1.LoadTestService();
                break;
            }
            //#endregion
            //Others
            case "GovernmentProcedureTypeDataChangeService": {
                myResult = new GovernmentProcedureTypeDataChangeService_1.GovernmentProcedureTypeDataChangeService();
                break;
            }
            case "CustomsDocumentPointerService": {
                myResult = new CustomsDocumentPointerService_1.CustomsDocumentPointerService();
                break;
            }
            case "CustomsRequestMenuService": {
                myResult = new CustomsRequestMenuService_1.CustomsRequestMenuService();
                break;
            }
            case "MultiCertificatesService": {
                myResult = new MultiCertificatesService_1.MultiCertificatesService();
                break;
            }
            case "SupplierInvoiceService": {
                myResult = new SupplierInvoiceService_1.SupplierInvoiceService();
                break;
            }
            //case "GITITEMCacheService": { myResult = new GITITEMCacheService(); break; }
            case "DeclarationMenuButtonsHandler": {
                myResult = new DeclarationMenuButtonsHandler_1.DeclarationMenuButtonsHandler();
                break;
            }
            case "VehicleMenuButtonsHandler": {
                myResult = new VehicleMenuButtonsHandler_1.VehicleMenuButtonsHandler();
                break;
            }
            case "PaymentOrderMenuButtonsHandler": {
                myResult = new PaymentOrderMenuButtonsHandler_1.PaymentOrderMenuButtonsHandler();
                break;
            }
            case "ClaimMenuButtonsHandler": {
                myResult = new ClaimMenuButtonsHandler_1.ClaimMenuButtonsHandler();
                break;
            }
            case "DeclarationEditComponentController": {
                myResult = new DeclarationEditComponentController_1.DeclarationEditComponentController();
                break;
            }
            case "VehicleEditComponentController": {
                myResult = new VehicleEditComponentController_1.VehicleEditComponentController();
                break;
            }
            case "PhysicalCheckMenuButtonsHandler": {
                myResult = new PhysicalCheckMenuButtonsHandler_1.PhysicalCheckMenuButtonsHandler();
                break;
            }
            case "VendorCommissionService": {
                myResult = new VendorCommissionService_1.VendorCommissionService();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map