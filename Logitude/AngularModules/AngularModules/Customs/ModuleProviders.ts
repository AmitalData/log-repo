/*
 * [Note] To enable Typescript regions, install Web Essential extension: http://vswebessentials.com/
 */

//#region StandardList
import { AddressContactStateListService } from './Services/StandardLists/AddressContactStateListService';
import { AddressPurposeListService } from './Services/StandardLists/AddressPurposeListService';
import { AmendmentFieldReasonTypeListService } from './Services/StandardLists/AmendmentFieldReasonTypeListService';
import { AmendmentRequestStatusListService } from './Services/StandardLists/AmendmentRequestStatusListService';
import { AssigneeNotificationTypeListService } from './Services/StandardLists/AssigneeNotificationTypeListService';
import { AttachmentTypeListService } from './Services/StandardLists/AttachmentTypeListService';
import { AuthorityListService } from './Services/StandardLists/AuthorityListService';
import { AuthorizedSignerPermitListService } from './Services/StandardLists/AuthorizedSignerPermitListService';
import { AutonomyTypeListService } from './Services/StandardLists/AutonomyTypeListService';
import { BankListService } from './Services/StandardLists/BankListService';
import { CargoIdentifireTypeListService } from './Services/StandardLists/CargoIdentifireTypeListService';
import { CargoIdentityQualifierListService } from './Services/StandardLists/CargoIdentityQualifierListService';
import { CertificateExemptionTypeListService } from './Services/StandardLists/CertificateExemptionTypeListService';
import { CertificatesStatusListService } from './Services/StandardLists/CertificatesStatusListService';
import { CheckEntityTypeListService } from './Services/StandardLists/CheckEntityTypeListService';
import { CheckEssenceLookupListService } from './Services/StandardLists/CheckEssenceLookupListService';
import { CheckQueueTypeListService } from './Services/StandardLists/CheckQueueTypeListService';
import { CheckRepresentativeTypeListService } from './Services/StandardLists/CheckRepresentativeTypeListService';
import { CheckTypeLookupListService } from './Services/StandardLists/CheckTypeLookupListService';
import { CityListService } from './Services/StandardLists/CityListService';
import { AgentTalkBackTypeListService } from './Services/StandardLists/AgentTalkBackTypeListService';
import { AcceptanceStatusListService } from './Services/StandardLists/AcceptanceStatusListService';
import { ClaimEntityListService } from './Services/StandardLists/ClaimEntityListService';
import { ClaimExplanationCodeListService } from './Services/StandardLists/ClaimExplanationCodeListService';
import { ClaimListService } from './Services/StandardLists/ClaimListService';
import { ClientListService } from './Services/StandardLists/ClientListService';
import { ClosedTableStatusListService } from './Services/StandardLists/ClosedTableStatusListService';
import { CollateralAnswerStatusListService } from './Services/StandardLists/CollateralAnswerStatusListService';
import { CollateralAnswerTypeListService } from './Services/StandardLists/CollateralAnswerTypeListService';
import { CollateralRequestStatusListService } from './Services/StandardLists/CollateralRequestStatusListService';
import { CollateralTypeListService } from './Services/StandardLists/CollateralTypeListService';
import { CommercialSaleListService } from './Services/StandardLists/CommercialSaleListService';
import { CommunicationTypeListService } from './Services/StandardLists/CommunicationTypeListService';
import { ConfirmationTypeListService } from './Services/StandardLists/ConfirmationTypeListService';
import { ConstraintApprovalDecisionListService } from './Services/StandardLists/ConstraintApprovalDecisionListService';
import { ConstraintProcessTypeListService } from './Services/StandardLists/ConstraintProcessTypeListService';
import { ConstraintStatusListService } from './Services/StandardLists/ConstraintStatusListService';
import { ConstraintTypeListService } from './Services/StandardLists/ConstraintTypeListService';
import { ContactRoleTypeListService } from './Services/StandardLists/ContactRoleTypeListService';
import { ContinuousMessagesTypeCodeListService } from './Services/StandardLists/ContinuousMessagesTypeCodeListService';
import { ConverterTypeListService } from './Services/StandardLists/ConverterTypeListService';
import { CountryGroupListService } from './Services/StandardLists/CountryGroupListService';
import { CourtInstanceListService } from './Services/StandardLists/CourtInstanceListService';
import { CurrencyTypeListService } from './Services/StandardLists/CurrencyTypeListService';
import { CustomBankListService } from './Services/StandardLists/CustomBankListService';
import { CustomDocumentTypeListService } from './Services/StandardLists/CustomDocumentTypeListService';
import { CustomDocumentTypeMetaDataListService } from './Services/StandardLists/CustomDocumentTypeMetaDataListService';
import { CustomerActivityTypeListService } from './Services/StandardLists/CustomerActivityTypeListService';
import { CustomerIdentifyTypeListService } from './Services/StandardLists/CustomerIdentifyTypeListService';
import { CustomerRoleTypeListService } from './Services/StandardLists/CustomerRoleTypeListService';
import { CustomerTypeGeneralListService } from './Services/StandardLists/CustomerTypeGeneralListService';
import { CustomMetaDataTypeListService } from './Services/StandardLists/CustomMetaDataTypeListService';
import { CustomsAddressTypeListService } from './Services/StandardLists/CustomsAddressTypeListService';
import { CustomsBookListService } from './Services/StandardLists/CustomsBookListService';
import { CustomsBookTypeListService } from './Services/StandardLists/CustomsBookTypeListService';
import { CustomsBranchListService } from './Services/StandardLists/CustomsBranchListService';
import { CustomsClosedTableListService } from './Services/StandardLists/CustomsClosedTableListService';
import { CustomsCollateralListService } from './Services/StandardLists/CustomsCollateralListService';
import { CustomsCountryListService } from './Services/StandardLists/CustomsCountryListService';
import { CustomsDocumentListService } from './Services/StandardLists/CustomsDocumentListService';
import { CustomsDocumentStatusTypeListService } from './Services/StandardLists/CustomsDocumentStatusTypeListService';
import { CustomsDocumentsTicketListService } from './Services/StandardLists/CustomsDocumentsTicketListService';
import { CustomsEnvoirmentTypeListService } from './Services/StandardLists/CustomsEnvoirmentTypeListService';
import { CustomsExchangeRateListService } from './Services/StandardLists/CustomsExchangeRateListService';
import { CustomsHouseTypeAdditionalListService } from './Services/StandardLists/CustomsHouseTypeAdditionalListService';
import { CustomsHouseTypeListService } from './Services/StandardLists/CustomsHouseTypeListService';
import { CustomsInsuranceCompanyListService } from './Services/StandardLists/CustomsInsuranceCompanyListService';
import { CustomsItemListService } from './Services/StandardLists/CustomsItemListService';
import { CustomsPartnersItemListService } from './Services/StandardLists/CustomsPartnersItemListService';
import { CustomsPaymentTermListService } from './Services/StandardLists/CustomsPaymentTermsListService';
import { CustomsRequestsSheetListService } from './Services/StandardLists/CustomsRequestsSheetListService';
import { CustomsRequestsSheetStatusListService } from './Services/StandardLists/CustomsRequestsSheetStatusListService';
import { CustomsRequiredFieldListService } from './Services/StandardLists/CustomsRequiredFieldListService';
import { CustomsSettingListService } from './Services/StandardLists/CustomsSettingListService';
import { CustomsTransportModeListService } from './Services/StandardLists/CustomsTransportModeListService';
import { CustomsVendorListService } from './Services/StandardLists/CustomsVendorListService';
import { CustomsVerificationStatusTypeListService } from './Services/StandardLists/CustomsVerificationStatusTypeListService';
import { DangerousGoodsPackingReqListService } from './Services/StandardLists/DangerousGoodsPackingReqListService';
import { DebtNotificationTypeListService } from './Services/StandardLists/DebtNotificationTypeListService';
import { DeclarationErrorMappingListService } from './Services/StandardLists/DeclarationErrorMappingListService';
import { DeclarationListService } from './Services/StandardLists/DeclarationListService';
import { DeclarationPaymentListService } from './Services/StandardLists/DeclarationPaymentListService';
import { DeclarationStatementTypeListService } from './Services/StandardLists/DeclarationStatementTypeListService';
import { DeclarationStatusTypeListService } from './Services/StandardLists/DeclarationStatusTypeListService';
import { DeficitConnFileParagraphTypeListService } from './Services/StandardLists/DeficitConnFileParagraphTypeListService';
import { DeficitListService } from './Services/StandardLists/DeficitsListService';
import { DeliverySiteTypeListService } from './Services/StandardLists/DeliverySiteTypeListService';
import { DemanderTypeListService } from './Services/StandardLists/DemanderTypeListService';
import { DepositCustomerActivityListService } from './Services/StandardLists/DepositCustomerActivityListService';
import { DepositEssenceTypeListService } from './Services/StandardLists/DepositEssenceTypeListService';
import { DepositFileTypeListService } from './Services/StandardLists/DepositFileTypeListService';
import { DepositListService } from './Services/StandardLists/DepositListService';
import { EntitlementTypeListService } from './Services/StandardLists/EntitlementTypeListService';
import { EntityTypeLookupListService } from './Services/StandardLists/EntityTypeLookupListService';
import { FacilitationTypeListService } from './Services/StandardLists/FacilitationTypeListService';
import { FaultInspectionTypeListService } from './Services/StandardLists/FaultInspectionTypeListService';
import { FuelTypeListService } from './Services/StandardLists/FuelTypeListService';
import { GenderListService } from './Services/StandardLists/GenderListService';
import { GovernmentProcedureTypeListService } from './Services/StandardLists/GovernmentProcedureTypeListService';
import { GuaranteeCertificateTypeListService } from './Services/StandardLists/GuaranteeCertificateTypeListService';
import { GuaranteeCustomerActivityListService } from './Services/StandardLists/GuaranteeCustomerActivityListService';
import { GuaranteeListService } from './Services/StandardLists/GuaranteeListService';
import { ImporterDeclarationTypeListService } from './Services/StandardLists/ImporterDeclarationTypeListService';
import { ImporterDespositionListService } from './Services/StandardLists/ImporterDespositionListService';
import { ImporterPeriodicDeclarStatusListService } from './Services/StandardLists/ImporterPeriodicDeclarStatusListService';
import { ImporterTypeForClaimListService } from './Services/StandardLists/ImporterTypeForClaimListService';
import { InterfaceManagementListService } from './Services/StandardLists/InterfaceManagementListService';
import { InterfaceSendOptionListService } from './Services/StandardLists/InterfaceSendOptionListService';
import { InterfaceTenantDefinitionListService } from './Services/StandardLists/InterfaceTenantDefinitionListService';
import { InternalBorderSiteTypeListService } from './Services/StandardLists/InternalBorderSiteTypeListService';
import { InternationalSiteListService } from './Services/StandardLists/InternationalSiteListService';
import { InvoiceTypeListService } from './Services/StandardLists/InvoiceTypeListService';
import { ItemGovernmentProcedureTypeListService } from './Services/StandardLists/ItemGovernmentProcedureTypeListService';
import { LastReleaseFromWarehouseListService } from './Services/StandardLists/LastReleaseFromWarehouseListService';
import { LeadDocumentExceptionTypeListService } from './Services/StandardLists/LeadDocumentExceptionTypeListService';
import { LeadDocumentTypeListService } from './Services/StandardLists/LeadDocumentTypeListService';
import { MeasureQualifierListService } from './Services/StandardLists/MeasureQualifierListService';
import { MeasurmentUnitListService } from './Services/StandardLists/MeasurmentUnitListService';
import { ModificationAndDiscountTypeListService } from './Services/StandardLists/ModificationAndDiscountTypeListService';
import { MorningMessageTypeListService } from './Services/StandardLists/MorningMessageTypeListService';
import { NotificationDefinitionListService } from './Services/StandardLists/NotificationDefinitionListService';
import { NotificationListService } from './Services/StandardLists/NotificationListService';
import { NotificationTenantDefinitionListService } from './Services/StandardLists/NotificationTenantDefinitionListService';
import { NotificationTypeListService } from './Services/StandardLists/NotificationTypeListService';
import { OrganizationUnitTypeListService } from './Services/StandardLists/OrganizationUnitTypeListService';
import { PackageMeasureQualifierListService } from './Services/StandardLists/PackageMeasureQualifierListService';
import { PackingTypeListService } from './Services/StandardLists/PackingTypeListService';
import { ParagraphTypeListService } from './Services/StandardLists/ParagraphTypeListService';
import { PassportTypeListService } from './Services/StandardLists/PassportTypeListService';
import { PayerActivityTypeListService } from './Services/StandardLists/PayerActivityTypeListService';
import { PayerTypeListService } from './Services/StandardLists/PayerTypeListService';
import { PaymentMethodStatusListService } from './Services/StandardLists/PaymentMethodStatusListService';
import { PaymentMethodTypeListService } from './Services/StandardLists/PaymentMethodTypeListService';
import { PaymentOrderListService } from './Services/StandardLists/PaymentOrderListService';
import { PaymentOrderStatusListService } from './Services/StandardLists/PaymentOrderStatusListService';
import { PaymentOrderTypeListService } from './Services/StandardLists/PaymentOrderTypeListService';
import { PaymentProcessListService } from './Services/StandardLists/PaymentProcessListService';
import { PaymentProtestTypeListService } from './Services/StandardLists/PaymentProtestTypeListService';
import { PaymentTypeListService } from './Services/StandardLists/PaymentTypeListService';
import { PhysicalCheckListService } from './Services/StandardLists/PhysicalCheckListService';
import { PhysicalCheckOperationListService } from './Services/StandardLists/PhysicalCheckOperationListService';
import { PhysicalCheckStatusMessageListService } from './Services/StandardLists/PhysicalCheckStatusMessageListService';
import { ProceduralFaultInProcessTypeListService } from './Services/StandardLists/ProceduralFaultInProcessTypeListService';
import { ProceduralFaultInSourceTypeListService } from './Services/StandardLists/ProceduralFaultInSourceTypeListService';
import { ProceduralFaultListService } from './Services/StandardLists/ProceduralFaultListService';
import { ProceduralFaultStatusListService } from './Services/StandardLists/ProceduralFaultStatusListService';
import { ProceduralFaultTypeListService } from './Services/StandardLists/ProceduralFaultTypeListService';
import { ProcessingReasonListService } from './Services/StandardLists/ProcessingReasonListService';
import { ProductIdentificationTypeListService } from './Services/StandardLists/ProductIdentificationTypeListService';
import { ProductNameTypeListService } from './Services/StandardLists/ProductNameTypeListService';
import { RansomViolationTypeListService } from './Services/StandardLists/RansomViolationTypeListService';
import { RegisteredWarehouseSiteTypeListService } from './Services/StandardLists/RegisteredWarehouseSiteTypeListService';
import { RequestStatusListService } from './Services/StandardLists/RequestStatusListService';
import { ReturnConditionListService } from './Services/StandardLists/ReturnConditionListService';
import { SalesTaxExemptionTypeListService } from './Services/StandardLists/SalesTaxExemptionTypeListService';
import { SignatureTypeListService } from './Services/StandardLists/SignatureTypeListService';
import { SiteLookupListService } from './Services/StandardLists/SiteLookupListService';
import { SiteTypeListService } from './Services/StandardLists/SiteTypeListService';
import { SpecialActionDescriptionTypeListService } from './Services/StandardLists/SpecialActionDescriptionTypeListService';
import { SpecializationTypeListService } from './Services/StandardLists/SpecializationTypeListService';
import { StorageMessageTypeListService } from './Services/StandardLists/StorageMessageTypeListService';
import { SubCountryListService } from './Services/StandardLists/SubCountryListService';
import { SupplierInvoiceListService } from './Services/StandardLists/SupplierInvoiceListService';
import { TapagConnectionTableListService } from './Services/StandardLists/TapagConnectionTableListService';
import { TapagListService } from './Services/StandardLists/TapagListService';
import { TapagTypeListService } from './Services/StandardLists/TapagTypeListService';
import { TermsOfSaleTypeListService } from './Services/StandardLists/TermsOfSaleTypeListService';
import { TradeAgreementListService } from './Services/StandardLists/TradeAgreementListService';
import { TradeLevyExamptTypeListService } from './Services/StandardLists/TradeLevyExamptTypeListService';
import { UnloadingSiteTypeListService } from './Services/StandardLists/UnloadingSiteTypeListService';
import { ValidCustomsItemListService } from './Services/StandardLists/ValidCustomsItemListService';
import { VehicleListService } from './Services/StandardLists/VehicleListService';
import { VehicleManufacturerListService } from './Services/StandardLists/VehicleManufacturerListService';
import { VehiclePoolTypeListService } from './Services/StandardLists/VehiclePoolTypeListService';
import { VehiclePriceListTypeListService } from './Services/StandardLists/VehiclePriceListTypeListService';
import { VehicleReductionTypeListService } from './Services/StandardLists/VehicleReductionTypeListService';
import { VehicleSafeAccessoryInstlTypeListService } from './Services/StandardLists/VehicleSafeAccessoryInstlTypeListService';
import { VehicleSafetyAccessoryTypeListService } from './Services/StandardLists/VehicleSafetyAccessoryTypeListService';
import { VehicleStatusListService } from './Services/StandardLists/VehicleStatusListService';
import { VehicleTecnologyTypeListService } from './Services/StandardLists/VehicleTecnologyTypeListService';
import { VehicleTypeListService } from './Services/StandardLists/VehicleTypeListService';
import { VendorCommissionListService } from './Services/StandardLists/VendorCommissionListService';
import { VendorStatusListService } from './Services/StandardLists/VendorStatusListService';
import { VendorTransactionTypeListService } from './Services/StandardLists/VendorTransactionTypeListService';
import { VendorTypeListService } from './Services/StandardLists/VendorTypeListService';
import { AccumalationStateListService } from './Services/StandardLists/AccumalationStateListService';
import { CouriersVatListService } from './Services/StandardLists/CouriersVatListService';
import { CourierPendingReasonListService } from './Services/StandardLists/CourierPendingReasonListService';
import { StorageStatusListService } from './Services/StandardLists/StorageStatusListService';
import { FreightPaymentMethodListService } from './Services/StandardLists/FreightPaymentMethodListService';
import { CustomsDocumentsDefinitionListService } from './Services/StandardLists/CustomsDocumentsDefinitionListService';
import { UIMessageListService } from './Services/StandardLists/UIMessageListService';
import { UIMessageAdditionalListService } from './Services/StandardLists/UIMessageAdditionalListService';
import { PointerLevelListService } from './Services/StandardLists/PointerLevelListService';
import { CourierMasterListService} from './Services/StandardLists/CourierMasterListService';
import { CourierDeclarationStatusListService} from './Services/StandardLists/CourierDeclarationStatusListService';
import { DeclarationCourierStatusListService} from './Services/StandardLists/DeclarationCourierStatusListService';
import { DeclarationCargoSplitListService } from './Services/StandardLists/DeclarationCargoSplitListService';
import { ActionCodeListService } from './Services/StandardLists/ActionCodeListService';
import { SplitOrMergeReasonListService } from './Services/StandardLists/SplitOrMergeReasonListService';
import { CargoSplitRequestStatusListService } from './Services/StandardLists/CargoSplitRequestStatusListService';
import { TreatmentWayListService } from './Services/StandardLists/TreatmentWayListService';
import { TPGFileTypeListService } from './Services/StandardLists/TPGFileTypeListService';
import { CustomsAirlineListService } from './Services/StandardLists/CustomsAirlineListService';
import { PendingErrorPlaceListService } from './Services/StandardLists/PendingErrorPlaceListService';
import { DecisionTypeListService } from './Services/StandardLists/DecisionTypeListService';
import { SeizureMethodTypeListService } from './Services/StandardLists/SeizureMethodTypeListService';
import { SeizureFactorTypeListService } from './Services/StandardLists/SeizureFactorTypeListService';
import { RefundCustomerActivityTypeListService } from './Services/StandardLists/RefundCustomerActivityTypeListService';
import { TransferCargoMethodTypeListService } from './Services/StandardLists/TransferCargoMethodTypeListService';
import { UpdateCodeListService } from './Services/StandardLists/UpdateCodeListService';
import { GatepassReturnCodeListService } from './Services/StandardLists/GatepassReturnCodeListService';
import { CourierCustomStatusListService } from './Services/StandardLists/CourierCustomStatusListService';
import { PendingByKeywordListService } from './Services/StandardLists/PendingByKeywordListService';



//#endregion

//#region StandardPMs
import { ClaimPMService } from './Services/StandardPMs/ClaimPMService';
import { ClientPMService } from './Services/StandardPMs/ClientPMService';
import { CustomBankPMService } from './Services/StandardPMs/CustomBankPMService';
import { CustomsBookPMService } from './Services/StandardPMs/CustomsBookPMService';
import { CustomsClosedTablePMService } from './Services/StandardPMs/CustomsClosedTablePMService';
import { CustomsCollateralPMService } from './Services/StandardPMs/CustomsCollateralPMService';
import { CustomsDocumentPMService } from './Services/StandardPMs/CustomsDocumentPMService';
import { CustomsDocumentsTicketPMService } from './Services/StandardPMs/CustomsDocumentsTicketPMService';
import { CustomsExchangeRatePMService } from './Services/StandardPMs/CustomsExchangeRatePMService';
import { CustomsHouseTypeAdditionalPMService } from './Services/StandardPMs/CustomsHouseTypeAdditionalPMService';
import { CustomsHouseTypePMService } from './Services/StandardPMs/CustomsHouseTypePMService';
import { CustomsItemPMService } from './Services/StandardPMs/CustomsItemPMService';
import { CustomsPartnersItemPMService } from './Services/StandardPMs/CustomsPartnersItemPMService';
import { CustomsRequestsSheetPMService } from './Services/StandardPMs/CustomsRequestsSheetPMService';
import { CustomsRequiredFieldPMService } from './Services/StandardPMs/CustomsRequiredFieldPMService';
import { CustomsSettingPMService } from './Services/StandardPMs/CustomsSettingPMService';
import { CustomsVendorPMService } from './Services/StandardPMs/CustomsVendorPMService';
import { DeclarationErrorMappingPMService } from './Services/StandardPMs/DeclarationErrorMappingPMService';
import { DeclarationPaymentPMService } from './Services/StandardPMs/DeclarationPaymentPMService';
import { DeclarationPMService } from './Services/StandardPMs/DeclarationPMService';
import { DeficitConnFileParagraphTypePMService } from './Services/StandardPMs/DeficitConnFileParagraphTypePMService';
import { DeficitPMService } from './Services/StandardPMs/DeficitPMService';
import { DepositPMService } from './Services/StandardPMs/DepositPMService';
import { GovernmentProcedureTypePMService } from './Services/StandardPMs/GovernmentProcedureTypePMService';
import { GuaranteePMService } from './Services/StandardPMs/GuaranteePMService';
import { ImporterDespositionPMService } from './Services/StandardPMs/ImporterDespositionPMService';
import { InterfaceManagementPMService } from './Services/StandardPMs/InterfaceManagementPMService';
import { InterfaceTenantDefinitionPMService } from './Services/StandardPMs/InterfaceTenantDefinitionPMService';
import { NotificationDefinitionPMService } from './Services/StandardPMs/NotificationDefinitionPMService';
import { NotificationPMService } from './Services/StandardPMs/NotificationPMService';
import { NotificationTenantDefinitionPMService } from './Services/StandardPMs/NotificationTenantDefinitionPMService';
import { PaymentOrderPMService } from './Services/StandardPMs/PaymentOrderPMService';
import { PhysicalCheckPMService } from './Services/StandardPMs/PhysicalCheckPMService';
import { ProceduralFaultPMService } from './Services/StandardPMs/ProceduralFaultPMService';
import { SupplierInvoicePMService } from './Services/StandardPMs/SupplierInvoicePMService';
import { TapagConnectionTablePMService } from './Services/StandardPMs/TapagConnectionTablePMService';
import { TapagPMService } from './Services/StandardPMs/TapagPMService';
import { VehiclePMService } from './Services/StandardPMs/VehiclePMService';
import { VendorCommissionPMService } from './Services/StandardPMs/VendorCommissionPMService';
import { CouriersVatPMService } from './Services/StandardPMs/CouriersVatPMService';
import { CourierPendingReasonPMService } from './Services/StandardPMs/CourierPendingReasonPMService';
import { CourierMasterPMService } from './Services/StandardPMs/CourierMasterPMService';
import { CustomDocumentTypePMService } from './Services/StandardPMs/CustomDocumentTypePMService';
import { UIMessagePMService } from './Services/StandardPMs/UIMessagePMService';
import { InternationalSitePMService } from './Services/StandardPMs/InternationalSitePMService';
import { DeclarationCargoSplitPMService } from './Services/StandardPMs/DeclarationCargoSplitPMService';
import { CustomsAirlinePMService } from './Services/StandardPMs/CustomsAirlinePMService';
import { CustomsCountryPMService } from './Services/StandardPMs/CustomsCountryPMService';
import { PendingByKeywordPMService } from './Services/StandardPMs/PendingByKeywordPMService';

//#endregion

//#region ExtendedLists
import { CustomBankExtendedListService } from './Services/ExtendedLists/CustomBankExtendedListService';
import { CustomsClosedTableExtendedListService } from './Services/ExtendedLists/CustomsClosedTableExtendedListService';
import { CustomsRequestsSheetExtendedListService } from './Services/ExtendedLists/CustomsRequestsSheetExtendedListService';
import { DeclarationExtendedListService } from './Services/ExtendedLists/DeclarationExtendedListService';
import { SupplierInvoiceExtendedListService } from './Services/ExtendedLists/SupplierInvoiceExtendedListService';
import { SupplierInvoiceFreightAmountExtendedListService } from './Services/ExtendedLists/SupplierInvoiceFreightAmountExtendedListService';
import { SupplierInvoiceItemExtendedListService } from './Services/ExtendedLists/SupplierInvoiceItemExtendedListService';
//import { SupplierInvoiceItemsExtendedList } from './Services/ExtendedLists/SupplierInvoiceItemsExtendedList';
import { NotificationExtendedListService } from './Services/ExtendedLists/NotificationExtendedListService';
import { SupplierInvoiceItemsTaxExtendedListService } from './Services/ExtendedLists/SupplierInvoiceItemsTaxExtendedListService';
import { SignStationExtendedListService } from './Services/ExtendedLists/SignStationExtendedListService';
import { DeclarationCourierStatusExtendedListService } from './Services/ExtendedLists/DeclarationCourierStatusExtendedListService';
import { RecallClientsForCutoms } from '../CustomsModules/CustomsGeneralRequests/Components/RecallClientsForCutoms';
//#endregion

//#region ExtendedPMs
import { CustomBankCardExtendedPMService } from './Services/ExtendedPMs/CustomBankCardExtendedPMService';
import { CustomsDocumentPointersExtendedPMService } from './Services/ExtendedPMs/CustomsDocumentPointersExtendedPMService';
import { CustomsDocumentsTicketsExtendedService } from './Services/ExtendedPMs/CustomsDocumentsTicketsExtendedService';
import { CustomsExchangeRateExtendedPMService } from './Services/ExtendedPMs/CustomsExchangeRateExtendedPMService';
import { CustomsHouseTypeExtendedPMService } from './Services/ExtendedPMs/CustomsHouseTypeExtendedPMService';
import { CustomsRequestSheetExtendedPMService } from './Services/ExtendedPMs/CustomsRequestSheetExtendedPMService';
import { PaymentOrderConnectionTableExtendedPMService } from './Services/ExtendedPMs/PaymentOrderConnectionTableExtendedPMService';
import { SupplierInvoiceExtendedPMService } from './Services/ExtendedPMs/SupplierInvoiceExtendedPMService';
///????  import { CustomsRequestSheetExtendedPMService } from './Services/ExtendedPMs/CustomsRequestSheetExtendedPMService';


//#endregion

//#region WebServices
import { ClientMessagesService } from './Services/WebServices/ClientMessagesService';
import { CustDocMetaDataValuesWebService } from './Services/WebServices/CustDocMetaDataValuesWebService';
import { CustDocRelatedDocsWebService } from './Services/WebServices/CustDocRelatedDocsWebService';
import { CustDocsTicketWebService } from './Services/WebServices/CustDocsTicketWebService';
import { DeclarationMessagesService } from './Services/WebServices/DeclarationMessagesService';
import { DeclarationWebService } from './Services/WebServices/DeclarationWebService';
import { IIGGeneralMessagesService } from './Services/WebServices/IIGGeneralMessagesService';
import { PaymentMessagesService } from './Services/WebServices/PaymentMessagesService';
import { PaymentOrderWebService } from './Services/WebServices/PaymentOrderWebService';
import { QuantityTypeMessageService } from './Services/WebServices/QuantityTypeMessageService';
import { TapagMessagesService } from './Services/WebServices/TapagMessagesService';
import { VendorMessagesService } from './Services/WebServices/VendorMessagesService';
import { LoadTestService } from './Services/WebServices/LoadTestService';
//#endregion

// Others
import { GovernmentProcedureTypeDataChangeService } from './Services/DataChange/GovernmentProcedureTypeDataChangeService';
import { CustomsDocumentPointerService } from './Services/Others/CustomsDocumentPointerService';
import { CustomsRequestMenuService } from './Services/Others/CustomsRequestMenuService';
import { MultiCertificatesService } from './Services/Others/MultiCertificatesService';
import { SupplierInvoiceService } from './Services/Others/SupplierInvoiceService';
import { GITITEMCacheService } from './Services/Others/GITITEMCacheService';


import { DeclarationMenuButtonsHandler } from './Components/MenuButtons/DeclarationMenuButtonsHandler';
import { VehicleMenuButtonsHandler } from './Components/MenuButtons/VehicleMenuButtonsHandler';
import { PaymentOrderMenuButtonsHandler } from './Components/MenuButtons/PaymentOrderMenuButtonsHandler';
import { ClaimMenuButtonsHandler } from './Components/MenuButtons/ClaimMenuButtonsHandler';
import { PhysicalCheckMenuButtonsHandler } from './Components/MenuButtons/PhysicalCheckMenuButtonsHandler';
import { DeclarationEditComponentController } from './Controller/DeclarationEditComponentController'
import { VehicleEditComponentController } from './Controller/VehicleEditComponentController'
import { VendorCommissionService } from './Services/WebServices/VendorCommissionService'


export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {

            //#region StandardList
            case "AddressContactStateListService": { myResult = new AddressContactStateListService(); break; }
            case "AddressPurposeListService": { myResult = new AddressPurposeListService(); break; }
            case "AmendmentFieldReasonTypeListService": { myResult = new AmendmentFieldReasonTypeListService(); break; }
            case "AmendmentRequestStatusListService": { myResult = new AmendmentRequestStatusListService(); break; }
            case "AssigneeNotificationTypeListService": { myResult = new AssigneeNotificationTypeListService(); break; }
            case "AttachmentTypeListService": { myResult = new AttachmentTypeListService(); break; }
            case "AuthorityListService": { myResult = new AuthorityListService(); break; }
            case "AuthorizedSignerPermitListService": { myResult = new AuthorizedSignerPermitListService(); break; }
            case "AutonomyTypeListService": { myResult = new AutonomyTypeListService(); break; }
            case "BankListService": { myResult = new BankListService(); break; }
            case "CargoIdentifireTypeListService": { myResult = new CargoIdentifireTypeListService(); break; }
            case "CargoIdentityQualifierListService": { myResult = new CargoIdentityQualifierListService(); break; }
            case "CertificateExemptionTypeListService": { myResult = new CertificateExemptionTypeListService(); break; }
            case "CertificatesStatusListService": { myResult = new CertificatesStatusListService(); break; }
            case "CheckEntityTypeListService": { myResult = new CheckEntityTypeListService(); break; }
            case "CheckEssenceLookupListService": { myResult = new CheckEssenceLookupListService(); break; }
            case "CheckQueueTypeListService": { myResult = new CheckQueueTypeListService(); break; }
            case "CheckRepresentativeTypeListService": { myResult = new CheckRepresentativeTypeListService(); break; }
            case "CheckTypeLookupListService": { myResult = new CheckTypeLookupListService(); break; }
            case "CityListService": { myResult = new CityListService(); break; }
            case "AgentTalkBackTypeListService": { myResult = new AgentTalkBackTypeListService(); break; }
            case "AcceptanceStatusListService": { myResult = new AcceptanceStatusListService(); break; } 
            case "ClaimEntityListService": { myResult = new ClaimEntityListService(); break; }
            case "ClaimExplanationCodeListService": { myResult = new ClaimExplanationCodeListService(); break; }
            case "ClaimListService": { myResult = new ClaimListService(); break; }
            case "ClientListService": { myResult = new ClientListService(); break; }
            case "ClosedTableStatusListService": { myResult = new ClosedTableStatusListService(); break; }
            case "CollateralAnswerStatusListService": { myResult = new CollateralAnswerStatusListService(); break; }
            case "CollateralAnswerTypeListService": { myResult = new CollateralAnswerTypeListService(); break; }
            case "CollateralRequestStatusListService": { myResult = new CollateralRequestStatusListService(); break; }
            case "CollateralTypeListService": { myResult = new CollateralTypeListService(); break; }
            case "CommercialSaleListService": { myResult = new CommercialSaleListService(); break; }
            case "CommunicationTypeListService": { myResult = new CommunicationTypeListService(); break; }
            case "ConfirmationTypeListService": { myResult = new ConfirmationTypeListService(); break; }
            case "ConstraintApprovalDecisionListService": { myResult = new ConstraintApprovalDecisionListService(); break; }
            case "ConstraintProcessTypeListService": { myResult = new ConstraintProcessTypeListService(); break; }
            case "ConstraintStatusListService": { myResult = new ConstraintStatusListService(); break; }
            case "ConstraintTypeListService": { myResult = new ConstraintTypeListService(); break; }
            case "ContactRoleTypeListService": { myResult = new ContactRoleTypeListService(); break; }
            case "ContinuousMessagesTypeCodeListService": { myResult = new ContinuousMessagesTypeCodeListService(); break; }
            case "ConverterTypeListService": { myResult = new ConverterTypeListService(); break; }
            case "CountryGroupListService": { myResult = new CountryGroupListService(); break; }
            case "CourtInstanceListService": { myResult = new CourtInstanceListService(); break; }
            case "CurrencyTypeListService": { myResult = new CurrencyTypeListService(); break; }
            case "CustomBankListService": { myResult = new CustomBankListService(); break; }
            case "CustomDocumentTypeListService": { myResult = new CustomDocumentTypeListService(); break; }
            case "CustomDocumentTypeMetaDataListService": { myResult = new CustomDocumentTypeMetaDataListService(); break; }
            case "CustomerActivityTypeListService": { myResult = new CustomerActivityTypeListService(); break; }
            case "CustomerIdentifyTypeListService": { myResult = new CustomerIdentifyTypeListService(); break; }
            case "CustomerRoleTypeListService": { myResult = new CustomerRoleTypeListService(); break; }
            case "CustomerTypeGeneralListService": { myResult = new CustomerTypeGeneralListService(); break; }
            case "CustomMetaDataTypeListService": { myResult = new CustomMetaDataTypeListService(); break; }
            case "CustomsAddressTypeListService": { myResult = new CustomsAddressTypeListService(); break; }
            case "CustomsBookListService": { myResult = new CustomsBookListService(); break; }
            case "CustomsBookTypeListService": { myResult = new CustomsBookTypeListService(); break; }
            case "CustomsBranchListService": { myResult = new CustomsBranchListService(); break; }
            case "CustomsClosedTableListService": { myResult = new CustomsClosedTableListService(); break; }
            case "CustomsCollateralListService": { myResult = new CustomsCollateralListService(); break; }
            case "CustomsCountryListService": { myResult = new CustomsCountryListService(); break; }
            case "CustomsDocumentListService": { myResult = new CustomsDocumentListService(); break; }
            case "CustomsDocumentStatusTypeListService": { myResult = new CustomsDocumentStatusTypeListService(); break; }
            case "CustomsDocumentsTicketListService": { myResult = new CustomsDocumentsTicketListService(); break; }
            case "CustomsEnvoirmentTypeListService": { myResult = new CustomsEnvoirmentTypeListService(); break; }
            case "CustomsExchangeRateListService": { myResult = new CustomsExchangeRateListService(); break; }
            case "CustomsHouseTypeAdditionalListService": { myResult = new CustomsHouseTypeAdditionalListService(); break; }
            case "CustomsHouseTypeListService": { myResult = new CustomsHouseTypeListService(); break; }
            case "CustomsInsuranceCompanyListService": { myResult = new CustomsInsuranceCompanyListService(); break; }
            case "CustomsItemListService": { myResult = new CustomsItemListService(); break; }
            case "CustomsPartnersItemListService": { myResult = new CustomsPartnersItemListService(); break; }
            case "CustomsPaymentTermListService": { myResult = new CustomsPaymentTermListService(); break; }
            case "CustomsRequestsSheetListService": { myResult = new CustomsRequestsSheetListService(); break; }
            case "CustomsRequestsSheetStatusListService": { myResult = new CustomsRequestsSheetStatusListService(); break; }
            case "CustomsRequiredFieldListService": { myResult = new CustomsRequiredFieldListService(); break; }
            case "CustomsSettingListService": { myResult = new CustomsSettingListService(); break; }
            case "CustomsTransportModeListService": { myResult = new CustomsTransportModeListService(); break; }
            case "CustomsVendorListService": { myResult = new CustomsVendorListService(); break; }
            case "CustomsVerificationStatusTypeListService": { myResult = new CustomsVerificationStatusTypeListService(); break; }
            case "DangerousGoodsPackingReqListService": { myResult = new DangerousGoodsPackingReqListService(); break; }
            case "DebtNotificationTypeListService": { myResult = new DebtNotificationTypeListService(); break; }
            case "DeclarationErrorMappingListService": { myResult = new DeclarationErrorMappingListService(); break; }
            case "DeclarationListService": { myResult = new DeclarationListService(); break; }
            case "DeclarationPaymentListService": { myResult = new DeclarationPaymentListService(); break; }
            case "DeclarationStatementTypeListService": { myResult = new DeclarationStatementTypeListService(); break; }
            case "DeclarationStatusTypeListService": { myResult = new DeclarationStatusTypeListService(); break; }
            case "DeficitConnFileParagraphTypeListService": { myResult = new DeficitConnFileParagraphTypeListService(); break; }
            case "DeficitListService": { myResult = new DeficitListService(); break; }
            case "DeliverySiteTypeListService": { myResult = new DeliverySiteTypeListService(); break; }
            case "DemanderTypeListService": { myResult = new DemanderTypeListService(); break; }
            case "DepositCustomerActivityListService": { myResult = new DepositCustomerActivityListService(); break; }
            case "DepositEssenceTypeListService": { myResult = new DepositEssenceTypeListService(); break; }
            case "DepositFileTypeListService": { myResult = new DepositFileTypeListService(); break; }
            case "DepositListService": { myResult = new DepositListService(); break; }
            case "EntitlementTypeListService": { myResult = new EntitlementTypeListService(); break; }
            case "EntityTypeLookupListService": { myResult = new EntityTypeLookupListService(); break; }
            case "FacilitationTypeListService": { myResult = new FacilitationTypeListService(); break; }
            case "FaultInspectionTypeListService": { myResult = new FaultInspectionTypeListService(); break; }
            case "FuelTypeListService": { myResult = new FuelTypeListService(); break; }
            case "GenderListService": { myResult = new GenderListService(); break; }
            case "GovernmentProcedureTypeListService": { myResult = new GovernmentProcedureTypeListService(); break; }
            case "GuaranteeCertificateTypeListService": { myResult = new GuaranteeCertificateTypeListService(); break; }
            case "GuaranteeCustomerActivityListService": { myResult = new GuaranteeCustomerActivityListService(); break; }
            case "GuaranteeListService": { myResult = new GuaranteeListService(); break; }
            case "ImporterDeclarationTypeListService": { myResult = new ImporterDeclarationTypeListService(); break; }
            case "ImporterDespositionListService": { myResult = new ImporterDespositionListService(); break; }
            case "ImporterPeriodicDeclarStatusListService": { myResult = new ImporterPeriodicDeclarStatusListService(); break; }
            case "ImporterTypeForClaimListService": { myResult = new ImporterTypeForClaimListService(); break; }
            case "InterfaceManagementListService": { myResult = new InterfaceManagementListService(); break; }
            case "InterfaceSendOptionListService": { myResult = new InterfaceSendOptionListService(); break; }
            case "InterfaceTenantDefinitionListService": { myResult = new InterfaceTenantDefinitionListService(); break; }
            case "InternalBorderSiteTypeListService": { myResult = new InternalBorderSiteTypeListService(); break; }
            case "InternationalSiteListService": { myResult = new InternationalSiteListService(); break; }
            case "InvoiceTypeListService": { myResult = new InvoiceTypeListService(); break; }
            case "ItemGovernmentProcedureTypeListService": { myResult = new ItemGovernmentProcedureTypeListService(); break; }
            case "LastReleaseFromWarehouseListService": { myResult = new LastReleaseFromWarehouseListService(); break; }
            case "LeadDocumentExceptionTypeListService": { myResult = new LeadDocumentExceptionTypeListService(); break; }
            case "LeadDocumentTypeListService": { myResult = new LeadDocumentTypeListService(); break; }
            case "MeasureQualifierListService": { myResult = new MeasureQualifierListService(); break; }
            case "MeasurmentUnitListService": { myResult = new MeasurmentUnitListService(); break; }
            case "ModificationAndDiscountTypeListService": { myResult = new ModificationAndDiscountTypeListService(); break; }
            case "MorningMessageTypeListService": { myResult = new MorningMessageTypeListService(); break; }
            case "NotificationDefinitionListService": { myResult = new NotificationDefinitionListService(); break; }
            case "NotificationListService": { myResult = new NotificationListService(); break; }
            case "NotificationTenantDefinitionListService": { myResult = new NotificationTenantDefinitionListService(); break; }
            case "NotificationTypeListService": { myResult = new NotificationTypeListService(); break; }
            case "OrganizationUnitTypeListService": { myResult = new OrganizationUnitTypeListService(); break; }
            case "PackageMeasureQualifierListService": { myResult = new PackageMeasureQualifierListService(); break; }
            case "PackingTypeListService": { myResult = new PackingTypeListService(); break; }
            case "ParagraphTypeListService": { myResult = new ParagraphTypeListService(); break; }
            case "PassportTypeListService": { myResult = new PassportTypeListService(); break; }
            case "PayerActivityTypeListService": { myResult = new PayerActivityTypeListService(); break; }
            case "PayerTypeListService": { myResult = new PayerTypeListService(); break; }
            case "PaymentMethodStatusListService": { myResult = new PaymentMethodStatusListService(); break; }
            case "PaymentMethodTypeListService": { myResult = new PaymentMethodTypeListService(); break; }
            case "PaymentOrderListService": { myResult = new PaymentOrderListService(); break; }
            case "PaymentOrderStatusListService": { myResult = new PaymentOrderStatusListService(); break; }
            case "PaymentOrderTypeListService": { myResult = new PaymentOrderTypeListService(); break; }
            case "PaymentProcessListService": { myResult = new PaymentProcessListService(); break; }
            case "PaymentProtestTypeListService": { myResult = new PaymentProtestTypeListService(); break; }
            case "PaymentTypeListService": { myResult = new PaymentTypeListService(); break; }
            case "PhysicalCheckListService": { myResult = new PhysicalCheckListService(); break; }
            case "PhysicalCheckOperationListService": { myResult = new PhysicalCheckOperationListService(); break; }
            case "PhysicalCheckStatusMessageListService": { myResult = new PhysicalCheckStatusMessageListService(); break; }
            case "ProceduralFaultInProcessTypeListService": { myResult = new ProceduralFaultInProcessTypeListService(); break; }
            case "ProceduralFaultInSourceTypeListService": { myResult = new ProceduralFaultInSourceTypeListService(); break; }
            case "ProceduralFaultListService": { myResult = new ProceduralFaultListService(); break; }
            case "ProceduralFaultStatusListService": { myResult = new ProceduralFaultStatusListService(); break; }
            case "ProceduralFaultTypeListService": { myResult = new ProceduralFaultTypeListService(); break; }
            case "ProcessingReasonListService": { myResult = new ProcessingReasonListService(); break; }
            case "ProductIdentificationTypeListService": { myResult = new ProductIdentificationTypeListService(); break; }
            case "ProductNameTypeListService": { myResult = new ProductNameTypeListService(); break; }
            case "RansomViolationTypeListService": { myResult = new RansomViolationTypeListService(); break; }
            case "RegisteredWarehouseSiteTypeListService": { myResult = new RegisteredWarehouseSiteTypeListService(); break; }
            case "RequestStatusListService": { myResult = new RequestStatusListService(); break; }
            case "ReturnConditionListService": { myResult = new ReturnConditionListService(); break; }
            case "SalesTaxExemptionTypeListService": { myResult = new SalesTaxExemptionTypeListService(); break; }
            case "SignatureTypeListService": { myResult = new SignatureTypeListService(); break; }
            case "SiteLookupListService": { myResult = new SiteLookupListService(); break; }
            case "SiteTypeListService": { myResult = new SiteTypeListService(); break; }
            case "SpecialActionDescriptionTypeListService": { myResult = new SpecialActionDescriptionTypeListService(); break; }
            case "SpecializationTypeListService": { myResult = new SpecializationTypeListService(); break; }
            case "StorageMessageTypeListService": { myResult = new StorageMessageTypeListService(); break; }
            case "SubCountryListService": { myResult = new SubCountryListService(); break; }
            case "SupplierInvoiceListService": { myResult = new SupplierInvoiceListService(); break; }
            case "TapagConnectionTableListService": { myResult = new TapagConnectionTableListService(); break; }
            case "TapagListService": { myResult = new TapagListService(); break; }
            case "TapagTypeListService": { myResult = new TapagTypeListService(); break; }
            case "TermsOfSaleTypeListService": { myResult = new TermsOfSaleTypeListService(); break; }
            case "TradeAgreementListService": { myResult = new TradeAgreementListService(); break; }
            case "TradeLevyExamptTypeListService": { myResult = new TradeLevyExamptTypeListService(); break; }
            case "UnloadingSiteTypeListService": { myResult = new UnloadingSiteTypeListService(); break; }
            case "ValidCustomsItemListService": { myResult = new ValidCustomsItemListService(); break; }
            case "VehicleListService": { myResult = new VehicleListService(); break; }
            case "VehicleManufacturerListService": { myResult = new VehicleManufacturerListService(); break; }
            case "VehiclePoolTypeListService": { myResult = new VehiclePoolTypeListService(); break; }
            case "VehiclePriceListTypeListService": { myResult = new VehiclePriceListTypeListService(); break; }
            case "VehicleReductionTypeListService": { myResult = new VehicleReductionTypeListService(); break; }
            case "VehicleSafeAccessoryInstlTypeListService": { myResult = new VehicleSafeAccessoryInstlTypeListService(); break; }
            case "VehicleSafetyAccessoryTypeListService": { myResult = new VehicleSafetyAccessoryTypeListService(); break; }
            case "VehicleStatusListService": { myResult = new VehicleStatusListService(); break; }
            case "VehicleTecnologyTypeListService": { myResult = new VehicleTecnologyTypeListService(); break; }
            case "VehicleTypeListService": { myResult = new VehicleTypeListService(); break; }
            case "VendorCommissionListService": { myResult = new VendorCommissionListService(); break; }
            case "VendorStatusListService": { myResult = new VendorStatusListService(); break; }
            case "VendorTransactionTypeListService": { myResult = new VendorTransactionTypeListService(); break; }
            case "VendorTypeListService": { myResult = new VendorTypeListService(); break; }
            case "AccumalationStateListService": { myResult = new AccumalationStateListService(); break; }
            case "CouriersVatListService": { myResult = new CouriersVatListService(); break; }
            case "CourierPendingReasonListService": { myResult = new CourierPendingReasonListService(); break; }
            case "StorageStatusListService": { myResult = new StorageStatusListService(); break; }
            case "FreightPaymentMethodListService": { myResult = new FreightPaymentMethodListService(); break; }
            case "CustomsDocumentsDefinitionListService": { myResult = new CustomsDocumentsDefinitionListService(); break; }
            case "UIMessageListService": { myResult = new UIMessageListService(); break; }
            case "UIMessageAdditionalListService": { myResult = new UIMessageAdditionalListService(); break; }
            case "Customs.PointerLevelListService": { myResult = new PointerLevelListService(); break; }
            case "PointerLevelListService": { myResult = new PointerLevelListService(); break; }
            case "CourierMasterListService": { myResult = new CourierMasterListService(); break; }
            case "CourierDeclarationStatusListService": { myResult = new CourierDeclarationStatusListService(); break; }
            case "DeclarationCourierStatusListService": { myResult = new DeclarationCourierStatusListService(); break; }
            case "DeclarationCargoSplitListService": { myResult = new DeclarationCargoSplitListService(); break; }
            case "ActionCodeListService": { myResult = new ActionCodeListService(); break; }
            case "SplitOrMergeReasonListService": { myResult = new SplitOrMergeReasonListService(); break; }
            case "CargoSplitRequestStatusListService": { myResult = new CargoSplitRequestStatusListService(); break; }
            case "TreatmentWayListService": { myResult = new TreatmentWayListService(); break; }
            case "TPGFileTypeListService": { myResult = new TPGFileTypeListService(); break; }  
            case "CustomsAirlineListService": { myResult = new CustomsAirlineListService(); break; }
            case "PendingErrorPlaceListService": { myResult = new PendingErrorPlaceListService(); break; }
            case "DecisionTypeListService": { myResult = new DecisionTypeListService(); break; }
            case "SeizureMethodTypeListService": { myResult = new SeizureMethodTypeListService(); break; }
            case "SeizureFactorTypeListService": { myResult = new SeizureFactorTypeListService(); break; }
            case "RefundCustomerActivityTypeListService": { myResult = new RefundCustomerActivityTypeListService(); break; }
            case "TransferCargoMethodTypeListService": { myResult = new TransferCargoMethodTypeListService(); break; }
            case "UpdateCodeListService": { myResult = new UpdateCodeListService(); break; }
            case "GatepassReturnCodeListService": { myResult = new GatepassReturnCodeListService(); break; }
            case "PendingByKeywordListService": { myResult = new PendingByKeywordListService(); break; }
                
            case "CourierCustomStatusListService": { myResult = new CourierCustomStatusListService(); break; }
            //#endregion                

            //#region StandardPMs
            case "ClaimPMService": { myResult = new ClaimPMService(); break; }
            case "ClientPMService": { myResult = new ClientPMService(); break; }
            case "CustomBankPMService": { myResult = new CustomBankPMService(); break; }
            case "CustomsBookPMService": { myResult = new CustomsBookPMService(); break; }
            case "CustomsClosedTablePMService": { myResult = new CustomsClosedTablePMService(); break; }
            case "CustomsCollateralPMService": { myResult = new CustomsCollateralPMService(); break; }
            case "CustomsDocumentPMService": { myResult = new CustomsDocumentPMService(); break; }
            case "CustomsDocumentsTicketPMService": { myResult = new CustomsDocumentsTicketPMService(); break; }
            case "CustomsExchangeRatePMService": { myResult = new CustomsExchangeRatePMService(); break; }
            case "CustomsHouseTypeAdditionalPMService": { myResult = new CustomsHouseTypeAdditionalPMService(); break; }
            case "CustomsHouseTypePMService": { myResult = new CustomsHouseTypePMService(); break; }
            case "CustomsItemPMService": { myResult = new CustomsItemPMService(); break; }
            case "CustomsPartnersItemPMService": { myResult = new CustomsPartnersItemPMService(); break; }
            case "CustomsRequestsSheetPMService": { myResult = new CustomsRequestsSheetPMService(); break; }
            case "CustomsRequiredFieldPMService": { myResult = new CustomsRequiredFieldPMService(); break; }
            case "CustomsSettingPMService": { myResult = new CustomsSettingPMService(); break; }
            case "CustomsVendorPMService": { myResult = new CustomsVendorPMService(); break; }
            case "DeclarationErrorMappingPMService": { myResult = new DeclarationErrorMappingPMService(); break; }
            case "DeclarationPaymentPMService": { myResult = new DeclarationPaymentPMService(); break; }
            case "DeclarationPMService": { myResult = new DeclarationPMService(); break; }
            case "DeficitConnFileParagraphTypePMService": { myResult = new DeficitConnFileParagraphTypePMService(); break; }
            case "DeficitPMService": { myResult = new DeficitPMService(); break; }
            case "DepositPMService": { myResult = new DepositPMService(); break; }
            case "GovernmentProcedureTypePMService": { myResult = new GovernmentProcedureTypePMService(); break; }
            case "GuaranteePMService": { myResult = new GuaranteePMService(); break; }
            case "ImporterDespositionPMService": { myResult = new ImporterDespositionPMService(); break; }
            case "InterfaceManagementPMService": { myResult = new InterfaceManagementPMService(); break; }
            case "InterfaceTenantDefinitionPMService": { myResult = new InterfaceTenantDefinitionPMService(); break; }
            case "NotificationDefinitionPMService": { myResult = new NotificationDefinitionPMService(); break; }
            case "NotificationPMService": { myResult = new NotificationPMService(); break; }
            case "NotificationTenantDefinitionPMService": { myResult = new NotificationTenantDefinitionPMService(); break; }
            case "PaymentOrderPMService": { myResult = new PaymentOrderPMService(); break; }
            case "PhysicalCheckPMService": { myResult = new PhysicalCheckPMService(); break; }
            case "ProceduralFaultPMService": { myResult = new ProceduralFaultPMService(); break; }
            case "ProceduralFaultsPMService": { myResult = new ProceduralFaultPMService(); break; }
            case "SupplierInvoicePMService": { myResult = new SupplierInvoicePMService(); break; }
            case "TapagConnectionTablePMService": { myResult = new TapagConnectionTablePMService(); break; }
            case "TapagPMService": { myResult = new TapagPMService(); break; }
            case "VehiclePMService": { myResult = new VehiclePMService(); break; }
            case "VendorCommissionPMService": { myResult = new VendorCommissionPMService(); break; }
            case "CouriersVatPMService": { myResult = new CouriersVatPMService(); break; }
            case "CourierPendingReasonPMService": { myResult = new CourierPendingReasonPMService(); break; }
            case "CourierMasterPMService": { myResult = new CourierMasterPMService(); break; }
            case "CustomDocumentTypePMService": { myResult = new CustomDocumentTypePMService(); break; }
            case "UIMessagePMService": { myResult = new UIMessagePMService(); break; }
            case "InternationalSitePMService": { myResult = new InternationalSitePMService(); break; }
            case "DeclarationCargoSplitPMService": { myResult = new DeclarationCargoSplitPMService(); break; }
            case "CustomsAirlinePMService": { myResult = new CustomsAirlinePMService(); break; }
            case "CustomsCountryPMService": { myResult = new CustomsCountryPMService(); break; }
            case "PendingByKeywordPMService": { myResult = new PendingByKeywordPMService(); break; }
            //#endregion

            //#region ExtendedLists
            case "CustomBankExtendedListService": { myResult = new CustomBankExtendedListService(); break; }
            case "CustomsClosedTableExtendedListService": { myResult = new CustomsClosedTableExtendedListService(); break; }
            case "CustomsRequestsSheetExtendedListService": { myResult = new CustomsRequestsSheetExtendedListService(); break; }
            case "DeclarationExtendedListService": { myResult = new DeclarationExtendedListService(); break; }
            case "SupplierInvoiceExtendedListService": { myResult = new SupplierInvoiceExtendedListService(); break; }
            case "SupplierInvoiceFreightAmountExtendedListService": { myResult = new SupplierInvoiceFreightAmountExtendedListService(); break; }
            case "SupplierInvoiceItemExtendedListService": { myResult = new SupplierInvoiceItemExtendedListService(); break; }
            case "NotificationExtendedListService": { myResult = new NotificationExtendedListService(); break; }
            case "SupplierInvoiceItemsTaxExtendedListService": { myResult = new SupplierInvoiceItemsTaxExtendedListService(); break; }
            case "SignStationExtendedListService": { myResult = new SignStationExtendedListService(); break; }
            case "DeclarationCourierStatusExtendedListService": { myResult = new DeclarationCourierStatusExtendedListService(); break; }
            case "RecallClientsForCutoms": { myResult = new RecallClientsForCutoms(); break; } 
            //#endregion

            //#region ExtendedPMs
            case "CustomBankCardExtendedPMService": { myResult = new CustomBankCardExtendedPMService(); break; }
            case "CustomsDocumentPointersExtendedPMService": { myResult = new CustomsDocumentPointersExtendedPMService(); break; }
            case "CustomsDocumentsTicketsExtendedService": { myResult = new CustomsDocumentsTicketsExtendedService(); break; }
            case "CustomsExchangeRateExtendedPMService": { myResult = new CustomsExchangeRateExtendedPMService(); break; }
            case "CustomsHouseTypeExtendedPMService": { myResult = new CustomsHouseTypeExtendedPMService(); break; }
            case "CustomsRequestSheetExtendedPMService": { myResult = new CustomsRequestSheetExtendedPMService(); break; }
            case "PaymentOrderConnectionTableExtendedPMService": { myResult = new PaymentOrderConnectionTableExtendedPMService(); break; }
            case "SupplierInvoiceExtendedPMService": { myResult = new SupplierInvoiceExtendedPMService(); break; } 
            //#endregion

            //#region WebServices
            case "ClientMessagesService": { myResult = new ClientMessagesService(); break; }
            case "CustDocMetaDataValuesWebService": { myResult = new CustDocMetaDataValuesWebService(); break; }
            case "CustDocRelatedDocsWebService": { myResult = new CustDocRelatedDocsWebService(); break; }
            case "CustDocsTicketWebService": { myResult = new CustDocsTicketWebService(); break; }
            case "DeclarationMessagesService": { myResult = new DeclarationMessagesService(); break; }
            case "DeclarationWebService": { myResult = new DeclarationWebService(); break; }
            case "IIGGeneralMessagesService": { myResult = new IIGGeneralMessagesService(); break; }
            case "PaymentMessagesService": { myResult = new PaymentMessagesService(); break; }
            case "PaymentOrderWebService": { myResult = new PaymentOrderWebService(); break; }
            case "QuantityTypeMessageService": { myResult = new QuantityTypeMessageService(); break; }
            case "TapagMessagesService": { myResult = new TapagMessagesService(); break; }
            case "VendorMessagesService": { myResult = new VendorMessagesService(); break; }
            case "LoadTestService": { myResult = new LoadTestService(); break; }
                
            //#endregion

            //Others
            case "GovernmentProcedureTypeDataChangeService": { myResult = new GovernmentProcedureTypeDataChangeService(); break; }
            case "CustomsDocumentPointerService": { myResult = new CustomsDocumentPointerService(); break; }
            case "CustomsRequestMenuService": { myResult = new CustomsRequestMenuService(); break; }
            case "MultiCertificatesService": { myResult = new MultiCertificatesService(); break; }
          case "SupplierInvoiceService": { myResult = new SupplierInvoiceService(); break; }
          //case "GITITEMCacheService": { myResult = new GITITEMCacheService(); break; }
            

            case "DeclarationMenuButtonsHandler": { myResult = new DeclarationMenuButtonsHandler(); break; }
            case "VehicleMenuButtonsHandler": { myResult = new VehicleMenuButtonsHandler(); break; }
            case "PaymentOrderMenuButtonsHandler": { myResult = new PaymentOrderMenuButtonsHandler(); break; }
            case "ClaimMenuButtonsHandler": { myResult = new ClaimMenuButtonsHandler(); break; }
            case "DeclarationEditComponentController": { myResult = new DeclarationEditComponentController(); break; }
            case "VehicleEditComponentController": { myResult = new VehicleEditComponentController(); break; }
            case "PhysicalCheckMenuButtonsHandler": { myResult = new PhysicalCheckMenuButtonsHandler(); break; }
            case "VendorCommissionService": { myResult = new VendorCommissionService(); break; }
          
                

        }

        return myResult;
    }
}
