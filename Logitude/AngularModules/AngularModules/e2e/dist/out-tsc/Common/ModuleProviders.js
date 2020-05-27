"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AccountingSettingListService_1 = require("./Services/StandardLists/AccountingSettingListService");
var AccountingSystemListService_1 = require("./Services/StandardLists/AccountingSystemListService");
var CustomsInterfaceSettingListService_1 = require("./Services/StandardLists/CustomsInterfaceSettingListService");
var CustomsInterfaceListService_1 = require("./Services/StandardLists/CustomsInterfaceListService");
var AdditionalServiceListService_1 = require("./Services/StandardLists/AdditionalServiceListService");
var AddressListService_1 = require("./Services/StandardLists/AddressListService");
var AgentListService_1 = require("./Services/StandardLists/AgentListService");
var AirlineListService_1 = require("./Services/StandardLists/AirlineListService");
var AirlineStatisticsListService_1 = require("./Services/StandardLists/AirlineStatisticsListService");
var AutomationResultEmailRecipientListService_1 = require("./Services/StandardLists/AutomationResultEmailRecipientListService");
var BranchListService_1 = require("./Services/StandardLists/BranchListService");
var BusinessUnitListService_1 = require("./Services/StandardLists/BusinessUnitListService");
var CardListService_1 = require("./Services/StandardLists/CardListService");
var CarrierListService_1 = require("./Services/StandardLists/CarrierListService");
var ChargesTypeListService_1 = require("./Services/StandardLists/ChargesTypeListService");
var CommodityListService_1 = require("./Services/StandardLists/CommodityListService");
var CommunicationLogListService_1 = require("./Services/StandardLists/CommunicationLogListService");
var CommunicationLogTypeListService_1 = require("./Services/StandardLists/CommunicationLogTypeListService");
var CommunicationStatusTypeListService_1 = require("./Services/StandardLists/CommunicationStatusTypeListService");
var CompetitorListService_1 = require("./Services/StandardLists/CompetitorListService");
var ComputingPartnerListService_1 = require("./Services/StandardLists/ComputingPartnerListService");
var ContactDoneMethodListService_1 = require("./Services/StandardLists/ContactDoneMethodListService");
var ContactListService_1 = require("./Services/StandardLists/ContactListService");
var CountryCityListService_1 = require("./Services/StandardLists/CountryCityListService");
var CountryListService_1 = require("./Services/StandardLists/CountryListService");
var CurrencyListService_1 = require("./Services/StandardLists/CurrencyListService");
var CustomAgentListService_1 = require("./Services/StandardLists/CustomAgentListService");
var TemplateFormatListService_1 = require("./Services/StandardLists/TemplateFormatListService");
var CustomerTenantAccessListService_1 = require("./Services/StandardLists/CustomerTenantAccessListService");
var CustomsShipperListService_1 = require("./Services/StandardLists/CustomsShipperListService");
//import {CustomerAdditionalServiceListService} from './Services/StandardLists/CustomerAdditionalServiceListService';
var CustomerListService_1 = require("./Services/StandardLists/CustomerListService");
//import {CustomerProductActualDataListService} from './Services/StandardLists/CustomerProductActualDataListService';
//import {CustomerProductListService} from './Services/StandardLists/CustomerProductListService';
//import {CustomerProductLocationListService} from './Services/StandardLists/CustomerProductLocationListService';
var CustomerSizeListService_1 = require("./Services/StandardLists/CustomerSizeListService");
var CustomerStatusListService_1 = require("./Services/StandardLists/CustomerStatusListService");
var DepartmentListService_1 = require("./Services/StandardLists/DepartmentListService");
var DimensionsUnitListService_1 = require("./Services/StandardLists/DimensionsUnitListService");
var DistributorListService_1 = require("./Services/StandardLists/DistributorListService");
var DocumentFolderListService_1 = require("./Services/StandardLists/DocumentFolderListService");
var DocumentsFilingListService_1 = require("./Services/StandardLists/DocumentsFilingListService");
var DocumentTypeCategoryListService_1 = require("./Services/StandardLists/DocumentTypeCategoryListService");
var DocumentTypeListService_1 = require("./Services/StandardLists/DocumentTypeListService");
var DocumentTypeTemplateListService_1 = require("./Services/StandardLists/DocumentTypeTemplateListService");
var DueTypeListService_1 = require("./Services/StandardLists/DueTypeListService");
var GlobalZoneListService_1 = require("./Services/StandardLists/GlobalZoneListService");
var HybridPartnerListService_1 = require("./Services/StandardLists/HybridPartnerListService");
//import {HybridTenantStateListService} from './Services/StandardLists/HybridTenantStateListService';
//import {HybridTenantThresholdListService} from './Services/StandardLists/HybridTenantThresholdListService';
var IncotermListService_1 = require("./Services/StandardLists/IncotermListService");
var IndustryListService_1 = require("./Services/StandardLists/IndustryListService");
var LeadSourceListService_1 = require("./Services/StandardLists/LeadSourceListService");
var LogitudeMessagesTransmissionLogListService_1 = require("./Services/StandardLists/LogitudeMessagesTransmissionLogListService");
var MeasurementListService_1 = require("./Services/StandardLists/MeasurementListService");
var PackageListService_1 = require("./Services/StandardLists/PackageListService");
var PackageTypeListService_1 = require("./Services/StandardLists/PackageTypeListService");
var ParticipantListService_1 = require("./Services/StandardLists/ParticipantListService");
var PartnerTypeListService_1 = require("./Services/StandardLists/PartnerTypeListService");
var PasswordPolicyListService_1 = require("./Services/StandardLists/PasswordPolicyListService");
var PaymentTermListService_1 = require("./Services/StandardLists/PaymentTermListService");
var WarehouseExtendedListService_1 = require("./Services/ExtendedLists/WarehouseExtendedListService");
var PortService_1 = require("./Services/ExtendedLists/PortService");
var PortListService_1 = require("./Services/StandardLists/PortListService");
var ProductPeriodListService_1 = require("./Services/StandardLists/ProductPeriodListService");
var ProductTypeListService_1 = require("./Services/StandardLists/ProductTypeListService");
var RankListService_1 = require("./Services/StandardLists/RankListService");
var RateClassListService_1 = require("./Services/StandardLists/RateClassListService");
var RegionListService_1 = require("./Services/StandardLists/RegionListService");
var ReportListService_1 = require("./Services/StandardLists/ReportListService");
//import {RestrictionListService} from './Services/StandardLists/RestrictionListService';
var RoleListService_1 = require("./Services/StandardLists/RoleListService");
var ShippingAgentListService_1 = require("./Services/StandardLists/ShippingAgentListService");
var ShippingLineListService_1 = require("./Services/StandardLists/ShippingLineListService");
var StateListService_1 = require("./Services/StandardLists/StateListService");
//import {TarrifChargeListService} from './Services/StandardLists/TarrifChargeListService';
//import {TarrifFromToListService} from './Services/StandardLists/TarrifFromToListService';
var TarrifFromToTypeListService_1 = require("./Services/StandardLists/TarrifFromToTypeListService");
var TarrifHeaderListService_1 = require("./Services/StandardLists/TarrifHeaderListService");
var TarrifStepListService_1 = require("./Services/StandardLists/TarrifStepListService");
var TarrifTypeListService_1 = require("./Services/StandardLists/TarrifTypeListService");
var TenantListService_1 = require("./Services/StandardLists/TenantListService");
//import {TermsofUseSignatureListService} from './Services/StandardLists/TermsofUseSignatureListService';
var TruckerListService_1 = require("./Services/StandardLists/TruckerListService");
var UserListService_1 = require("./Services/StandardLists/UserListService");
var VatMandatoryTypeListService_1 = require("./Services/StandardLists/VatMandatoryTypeListService");
var VatTypeListService_1 = require("./Services/StandardLists/VatTypeListService");
var VatUniqueTypeListService_1 = require("./Services/StandardLists/VatUniqueTypeListService");
var VendorListService_1 = require("./Services/StandardLists/VendorListService");
var VesselListService_1 = require("./Services/StandardLists/VesselListService");
var WarehouseListService_1 = require("./Services/StandardLists/WarehouseListService");
var WeightUnitListService_1 = require("./Services/StandardLists/WeightUnitListService");
var AgentSharedManifestListService_1 = require("./Services/StandardLists/AgentSharedManifestListService");
var VatFormatTypeListService_1 = require("./Services/StandardLists/VatFormatTypeListService");
var TwoFactorAuthenticationDeviceListService_1 = require("./Services/StandardLists/TwoFactorAuthenticationDeviceListService");
var LoginPolicyListService_1 = require("./Services/StandardLists/LoginPolicyListService");
var MetodoPagoListService_1 = require("./Services/StandardLists/MetodoPagoListService");
var CustomerTenantAccessPMService_1 = require("./Services/StandardPMs/CustomerTenantAccessPMService");
var UsoCFDIListService_1 = require("./Services/StandardLists/UsoCFDIListService");
var ReportsTemplateListService_1 = require("./Services/StandardLists/ReportsTemplateListService");
var ReportsTemplatesVersionListService_1 = require("./Services/StandardLists/ReportsTemplatesVersionListService");
var AccountingSettingPMService_1 = require("./Services/StandardPMs/AccountingSettingPMService");
//import {AccountingSystemPMService} from './Services/StandardPMs/AccountingSystemPMService';
var AdditionalServicePMService_1 = require("./Services/StandardPMs/AdditionalServicePMService");
var AddressPMService_1 = require("./Services/StandardPMs/AddressPMService");
var AgentPMService_1 = require("./Services/StandardPMs/AgentPMService");
var AirlinePMService_1 = require("./Services/StandardPMs/AirlinePMService");
var AirlineStatisticsPMService_1 = require("./Services/StandardPMs/AirlineStatisticsPMService");
var AutomationPMService_1 = require("./Services/StandardPMs/AutomationPMService");
var AutomationResultEmailRecipientPMService_1 = require("./Services/StandardPMs/AutomationResultEmailRecipientPMService");
var BranchPMService_1 = require("./Services/StandardPMs/BranchPMService");
var BusinessUnitPMService_1 = require("./Services/StandardPMs/BusinessUnitPMService");
var CardPMService_1 = require("./Services/StandardPMs/CardPMService");
var ChargesTypePMService_1 = require("./Services/StandardPMs/ChargesTypePMService");
var CommodityPMService_1 = require("./Services/StandardPMs/CommodityPMService");
var CommunicationLogPMService_1 = require("./Services/StandardPMs/CommunicationLogPMService");
//import {CommunicationLogTypePMService} from './Services/StandardPMs/CommunicationLogTypePMService';
//import {CommunicationStatusTypePMService} from './Services/StandardPMs/CommunicationStatusTypePMService';
var CompetitorPMService_1 = require("./Services/StandardPMs/CompetitorPMService");
//import {ContactDoneMethodPMService} from './Services/StandardPMs/ContactDoneMethodPMService';
var ContactPMService_1 = require("./Services/StandardPMs/ContactPMService");
var CountryCityPMService_1 = require("./Services/StandardPMs/CountryCityPMService");
var CountryPMService_1 = require("./Services/StandardPMs/CountryPMService");
var CurrencyPMService_1 = require("./Services/StandardPMs/CurrencyPMService");
var CustomAgentPMService_1 = require("./Services/StandardPMs/CustomAgentPMService");
var CustomerPMService_1 = require("./Services/StandardPMs/CustomerPMService");
//import {CustomerProductActualDataPMService} from './Services/StandardPMs/CustomerProductActualDataPMService';
//import {CustomerProductLocationPMService} from './Services/StandardPMs/CustomerProductLocationPMService';
//import {CustomerProductPMService} from './Services/StandardPMs/CustomerProductPMService';
var CustomerSizePMService_1 = require("./Services/StandardPMs/CustomerSizePMService");
//import {CustomerStatusPMService} from './Services/StandardPMs/CustomerStatusPMService';
var DepartmentPMService_1 = require("./Services/StandardPMs/DepartmentPMService");
//import {DimensionsUnitPMService} from './Services/StandardPMs/DimensionsUnitPMService';
var DistributorPMService_1 = require("./Services/StandardPMs/DistributorPMService");
var DocumentFolderPMService_1 = require("./Services/StandardPMs/DocumentFolderPMService");
var DocumentsFilingPMService_1 = require("./Services/StandardPMs/DocumentsFilingPMService");
//import {DocumentTypeCategoryPMService} from './Services/StandardPMs/DocumentTypeCategoryPMService';
var DocumentTypePMService_1 = require("./Services/StandardPMs/DocumentTypePMService");
var DocumentTypeTemplatePMService_1 = require("./Services/StandardPMs/DocumentTypeTemplatePMService");
//import {DueTypePMService} from './Services/StandardPMs/DueTypePMService';
var GlobalZonePMService_1 = require("./Services/StandardPMs/GlobalZonePMService");
var HybridPartnerPMService_1 = require("./Services/StandardPMs/HybridPartnerPMService");
//import {HybridTenantStatePMService} from './Services/StandardPMs/HybridTenantStatePMService';
//import {HybridTenantThresholdPMService} from './Services/StandardPMs/HybridTenantThresholdPMService';
var IncotermPMService_1 = require("./Services/StandardPMs/IncotermPMService");
var IndustryPMService_1 = require("./Services/StandardPMs/IndustryPMService");
var LeadSourcePMService_1 = require("./Services/StandardPMs/LeadSourcePMService");
var LogitudeMessagesTransmissionLogPMService_1 = require("./Services/StandardPMs/LogitudeMessagesTransmissionLogPMService");
var MAWBStackPMService_1 = require("./Services/StandardPMs/MAWBStackPMService");
var MeasurementPMService_1 = require("./Services/StandardPMs/MeasurementPMService");
//import {PackageConnectedPackagePMService} from './Services/StandardPMs/PackageConnectedPackagePMService';
var PackagePMService_1 = require("./Services/StandardPMs/PackagePMService");
var PackageTypePMService_1 = require("./Services/StandardPMs/PackageTypePMService");
var ParticipantPMService_1 = require("./Services/StandardPMs/ParticipantPMService");
//import {PartnerTypePMService} from './Services/StandardPMs/PartnerTypePMService';
//import {PasswordPolicyPMService} from './Services/StandardPMs/PasswordPolicyPMService';
var PaymentTermPMService_1 = require("./Services/StandardPMs/PaymentTermPMService");
var PortPMService_1 = require("./Services/StandardPMs/PortPMService");
//import {ProductPeriodPMService} from './Services/StandardPMs/ProductPeriodPMService';
var ProductTypePMService_1 = require("./Services/StandardPMs/ProductTypePMService");
var RankPMService_1 = require("./Services/StandardPMs/RankPMService");
//import {RateClassPMService} from './Services/StandardPMs/RateClassPMService';
var RegionPMService_1 = require("./Services/StandardPMs/RegionPMService");
var ReportPMService_1 = require("./Services/StandardPMs/ReportPMService");
//import {RestrictionPMService} from './Services/StandardPMs/RestrictionPMService';
var RolePMService_1 = require("./Services/StandardPMs/RolePMService");
var ShippingAgentPMService_1 = require("./Services/StandardPMs/ShippingAgentPMService");
var ShippingLinePMService_1 = require("./Services/StandardPMs/ShippingLinePMService");
var StatePMService_1 = require("./Services/StandardPMs/StatePMService");
//import {TarrifChargePMService} from './Services/StandardPMs/TarrifChargePMService';
//import {TarrifFromToPMService} from './Services/StandardPMs/TarrifFromToPMService';
//import {TarrifFromToTypePMService} from './Services/StandardPMs/TarrifFromToTypePMService';
var TarrifHeaderPMService_1 = require("./Services/StandardPMs/TarrifHeaderPMService");
var TarrifStepPMService_1 = require("./Services/StandardPMs/TarrifStepPMService");
//import {TarrifTypePMService} from './Services/StandardPMs/TarrifTypePMService';
var TenantPMService_1 = require("./Services/StandardPMs/TenantPMService");
var TermsofUseSignaturePMService_1 = require("./Services/StandardPMs/TermsofUseSignaturePMService");
var TruckerPMService_1 = require("./Services/StandardPMs/TruckerPMService");
//import {UserLicensePMService} from './Services/StandardPMs/UserLicensePMService';
var UserPMService_1 = require("./Services/StandardPMs/UserPMService");
var VatTypePMService_1 = require("./Services/StandardPMs/VatTypePMService");
var VendorPMService_1 = require("./Services/StandardPMs/VendorPMService");
var VesselPMService_1 = require("./Services/StandardPMs/VesselPMService");
var WarehousePMService_1 = require("./Services/StandardPMs/WarehousePMService");
var AgentSharedManifestPMService_1 = require("./Services/StandardPMs/AgentSharedManifestPMService");
//import {WeightUnitPMService} from './Services/StandardPMs/WeightUnitPMService';
var CustomsShipperPMService_1 = require("./Services/StandardPMs/CustomsShipperPMService");
var CustomerMenuButtonsHandler_1 = require("./Components/MenuButtons/CustomerMenuButtonsHandler");
var UserMenuButtonsHandler_1 = require("./Components/MenuButtons/UserMenuButtonsHandler");
var CommunicationLogMenuButtonsHandler_1 = require("./Components/MenuButtons/CommunicationLogMenuButtonsHandler");
var CustomerTenantAccessMenuButtonsHandler_1 = require("./Components/MenuButtons/CustomerTenantAccessMenuButtonsHandler");
var ReportsTemplatePMService_1 = require("./Services/StandardPMs/ReportsTemplatePMService");
var ReportsTemplatesVersionPMService_1 = require("./Services/StandardPMs/ReportsTemplatesVersionPMService");
var ContactMenuButtonsHandler_1 = require("./Components/MenuButtons/ContactMenuButtonsHandler");
var DocumentsFilingExtendedPMService_1 = require("./Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var PaymentTermDateTypeListService_1 = require("./Services/StandardLists/PaymentTermDateTypeListService");
var NumberFormatListService_1 = require("./Services/StandardLists/NumberFormatListService");
// Extended Lists
var CarrierExtendedListService_1 = require("./Services/ExtendedLists/CarrierExtendedListService");
var PortExtendedListService_1 = require("./Services/ExtendedLists/PortExtendedListService");
var CustomerFieldsUpdateSettingListService_1 = require("./Services/StandardLists/CustomerFieldsUpdateSettingListService");
var CustomerFieldsUpdateSettingPMService_1 = require("./Services/StandardPMs/CustomerFieldsUpdateSettingPMService");
var TenantLoginPolicyPMService_1 = require("./Services/StandardPMs/TenantLoginPolicyPMService");
var TwoFactorAuthenticationDeviceExtendedPMService_1 = require("./Services/ExtendedPMs/TwoFactorAuthenticationDeviceExtendedPMService");
var FeaturePackageTypeListService_1 = require("./Services/StandardLists/FeaturePackageTypeListService");
var ComputingPartnerPMService_1 = require("./Services/StandardPMs/ComputingPartnerPMService");
var ComputingPartnerTranslationPMService_1 = require("./Services/StandardPMs/ComputingPartnerTranslationPMService");
var TenantLoginPolicyListService_1 = require("./Services/StandardLists/TenantLoginPolicyListService");
var RegistryDateTypeListService_1 = require("./Services/StandardLists/RegistryDateTypeListService");
var WarehouseTypeListService_1 = require("./Services/StandardLists/WarehouseTypeListService");
var TemperatureUnitListService_1 = require("./Services/StandardLists/TemperatureUnitListService");
var DocumentFilingBackupSettingListService_1 = require("./Services/StandardLists/DocumentFilingBackupSettingListService");
var DocumentFilingBackupBatchListService_1 = require("./Services/StandardLists/DocumentFilingBackupBatchListService");
var DocumentFilingBackupSettingPMService_1 = require("./Services/StandardPMs/DocumentFilingBackupSettingPMService");
var DocumentFilingBackupBatchPMService_1 = require("./Services/StandardPMs/DocumentFilingBackupBatchPMService");
var CheckDigitControlAlgorithmListService_1 = require("./Services/StandardLists/CheckDigitControlAlgorithmListService");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            case "AccountingSettingListService": {
                myResult = new AccountingSettingListService_1.AccountingSettingListService();
                break;
            }
            case "AccountingSystemListService": {
                myResult = new AccountingSystemListService_1.AccountingSystemListService();
                break;
            }
            case "CustomsInterfaceSettingListService": {
                myResult = new CustomsInterfaceSettingListService_1.CustomsInterfaceSettingListService();
                break;
            }
            case "CustomsInterfaceListService": {
                myResult = new CustomsInterfaceListService_1.CustomsInterfaceListService();
                break;
            }
            case "AdditionalServiceListService": {
                myResult = new AdditionalServiceListService_1.AdditionalServiceListService();
                break;
            }
            case "AddressListService": {
                myResult = new AddressListService_1.AddressListService();
                break;
            }
            case "AgentListService": {
                myResult = new AgentListService_1.AgentListService();
                break;
            }
            case "AirlineListService": {
                myResult = new AirlineListService_1.AirlineListService();
                break;
            }
            case "AirlineStatisticsListService": {
                myResult = new AirlineStatisticsListService_1.AirlineStatisticsListService();
                break;
            }
            case "AutomationResultEmailRecipientListService": {
                myResult = new AutomationResultEmailRecipientListService_1.AutomationResultEmailRecipientListService();
                break;
            }
            case "BranchListService": {
                myResult = new BranchListService_1.BranchListService();
                break;
            }
            case "BusinessUnitListService": {
                myResult = new BusinessUnitListService_1.BusinessUnitListService();
                break;
            }
            case "CardListService": {
                myResult = new CardListService_1.CardListService();
                break;
            }
            case "CarrierListService": {
                myResult = new CarrierListService_1.CarrierListService();
                break;
            }
            case "ChargesTypeListService": {
                myResult = new ChargesTypeListService_1.ChargesTypeListService();
                break;
            }
            case "CommodityListService": {
                myResult = new CommodityListService_1.CommodityListService();
                break;
            }
            case "CommunicationLogListService": {
                myResult = new CommunicationLogListService_1.CommunicationLogListService();
                break;
            }
            case "CommunicationLogTypeListService": {
                myResult = new CommunicationLogTypeListService_1.CommunicationLogTypeListService();
                break;
            }
            case "TemplateFormatListService": {
                myResult = new TemplateFormatListService_1.TemplateFormatListService();
                break;
            }
            case "CommunicationStatusTypeListService": {
                myResult = new CommunicationStatusTypeListService_1.CommunicationStatusTypeListService();
                break;
            }
            case "CompetitorListService": {
                myResult = new CompetitorListService_1.CompetitorListService();
                break;
            }
            case "ComputingPartnerListService": {
                myResult = new ComputingPartnerListService_1.ComputingPartnerListService();
                break;
            }
            case "ContactDoneMethodListService": {
                myResult = new ContactDoneMethodListService_1.ContactDoneMethodListService();
                break;
            }
            case "ContactListService": {
                myResult = new ContactListService_1.ContactListService();
                break;
            }
            case "CountryCityListService": {
                myResult = new CountryCityListService_1.CountryCityListService();
                break;
            }
            case "CountryListService": {
                myResult = new CountryListService_1.CountryListService();
                break;
            }
            case "CurrencyListService": {
                myResult = new CurrencyListService_1.CurrencyListService();
                break;
            }
            case "CustomAgentListService": {
                myResult = new CustomAgentListService_1.CustomAgentListService();
                break;
            }
            //case "CustomerAdditionalServiceListService": { myResult = new CustomerAdditionalServiceListService(); break; }
            case "CustomerListService": {
                myResult = new CustomerListService_1.CustomerListService();
                break;
            }
            //case "CustomerProductActualDataListService": { myResult = new CustomerProductActualDataListService(); break; }
            //case "CustomerProductListService": { myResult = new CustomerProductListService(); break; }
            //case "CustomerProductLocationListService": { myResult = new CustomerProductLocationListService(); break; }
            case "CustomerSizeListService": {
                myResult = new CustomerSizeListService_1.CustomerSizeListService();
                break;
            }
            case "CustomerStatusListService": {
                myResult = new CustomerStatusListService_1.CustomerStatusListService();
                break;
            }
            case "DepartmentListService": {
                myResult = new DepartmentListService_1.DepartmentListService();
                break;
            }
            case "DimensionsUnitListService": {
                myResult = new DimensionsUnitListService_1.DimensionsUnitListService();
                break;
            }
            case "DistributorListService": {
                myResult = new DistributorListService_1.DistributorListService();
                break;
            }
            case "DocumentFolderListService": {
                myResult = new DocumentFolderListService_1.DocumentFolderListService();
                break;
            }
            case "DocumentsFilingListService": {
                myResult = new DocumentsFilingListService_1.DocumentsFilingListService();
                break;
            }
            case "DocumentTypeCategoryListService": {
                myResult = new DocumentTypeCategoryListService_1.DocumentTypeCategoryListService();
                break;
            }
            case "DocumentTypeListService": {
                myResult = new DocumentTypeListService_1.DocumentTypeListService();
                break;
            }
            case "DocumentTypeTemplateListService": {
                myResult = new DocumentTypeTemplateListService_1.DocumentTypeTemplateListService();
                break;
            }
            case "DueTypeListService": {
                myResult = new DueTypeListService_1.DueTypeListService();
                break;
            }
            case "GlobalZoneListService": {
                myResult = new GlobalZoneListService_1.GlobalZoneListService();
                break;
            }
            case "HybridPartnerListService": {
                myResult = new HybridPartnerListService_1.HybridPartnerListService();
                break;
            }
            //case "HybridTenantStateListService": { myResult = new HybridTenantStateListService(); break; }
            //case "HybridTenantThresholdListService": { myResult = new HybridTenantThresholdListService(); break; }
            case "IncotermListService": {
                myResult = new IncotermListService_1.IncotermListService();
                break;
            }
            case "IndustryListService": {
                myResult = new IndustryListService_1.IndustryListService();
                break;
            }
            case "LeadSourceListService": {
                myResult = new LeadSourceListService_1.LeadSourceListService();
                break;
            }
            case "LogitudeMessagesTransmissionLogListService": {
                myResult = new LogitudeMessagesTransmissionLogListService_1.LogitudeMessagesTransmissionLogListService();
                break;
            }
            case "MeasurementListService": {
                myResult = new MeasurementListService_1.MeasurementListService();
                break;
            }
            case "PackageListService": {
                myResult = new PackageListService_1.PackageListService();
                break;
            }
            case "PackageTypeListService": {
                myResult = new PackageTypeListService_1.PackageTypeListService();
                break;
            }
            case "ParticipantListService": {
                myResult = new ParticipantListService_1.ParticipantListService();
                break;
            }
            case "PartnerTypeListService": {
                myResult = new PartnerTypeListService_1.PartnerTypeListService();
                break;
            }
            case "PasswordPolicyListService": {
                myResult = new PasswordPolicyListService_1.PasswordPolicyListService();
                break;
            }
            case "PaymentTermListService": {
                myResult = new PaymentTermListService_1.PaymentTermListService();
                break;
            }
            case "PortService": {
                myResult = new PortService_1.PortService();
                break;
            }
            case "WarehouseExtendedListService": {
                myResult = new WarehouseExtendedListService_1.WarehouseExtendedListService();
                break;
            }
            case "PortListService": {
                myResult = new PortListService_1.PortListService();
                break;
            }
            case "ProductPeriodListService": {
                myResult = new ProductPeriodListService_1.ProductPeriodListService();
                break;
            }
            case "ProductTypeListService": {
                myResult = new ProductTypeListService_1.ProductTypeListService();
                break;
            }
            case "RankListService": {
                myResult = new RankListService_1.RankListService();
                break;
            }
            case "RateClassListService": {
                myResult = new RateClassListService_1.RateClassListService();
                break;
            }
            case "RegionListService": {
                myResult = new RegionListService_1.RegionListService();
                break;
            }
            case "ReportListService": {
                myResult = new ReportListService_1.ReportListService();
                break;
            }
            //case "RestrictionListService": { myResult = new RestrictionListService(); break; }
            case "RoleListService": {
                myResult = new RoleListService_1.RoleListService();
                break;
            }
            case "ShippingAgentListService": {
                myResult = new ShippingAgentListService_1.ShippingAgentListService();
                break;
            }
            case "ShippingLineListService": {
                myResult = new ShippingLineListService_1.ShippingLineListService();
                break;
            }
            case "StateListService": {
                myResult = new StateListService_1.StateListService();
                break;
            }
            //case "TarrifChargeListService": { myResult = new TarrifChargeListService(); break; }
            //case "TarrifFromToListService": { myResult = new TarrifFromToListService(); break; }
            case "TarrifFromToTypeListService": {
                myResult = new TarrifFromToTypeListService_1.TarrifFromToTypeListService();
                break;
            }
            case "TarrifHeaderListService": {
                myResult = new TarrifHeaderListService_1.TarrifHeaderListService();
                break;
            }
            case "TarrifStepListService": {
                myResult = new TarrifStepListService_1.TarrifStepListService();
                break;
            }
            case "TarrifTypeListService": {
                myResult = new TarrifTypeListService_1.TarrifTypeListService();
                break;
            }
            case "TenantListService": {
                myResult = new TenantListService_1.TenantListService();
                break;
            }
            //case "TermsofUseSignatureListService": { myResult = new TermsofUseSignatureListService(); break; }
            case "TruckerListService": {
                myResult = new TruckerListService_1.TruckerListService();
                break;
            }
            case "UserListService": {
                myResult = new UserListService_1.UserListService();
                break;
            }
            case "VatMandatoryTypeListService": {
                myResult = new VatMandatoryTypeListService_1.VatMandatoryTypeListService();
                break;
            }
            case "VatTypeListService": {
                myResult = new VatTypeListService_1.VatTypeListService();
                break;
            }
            case "VatUniqueTypeListService": {
                myResult = new VatUniqueTypeListService_1.VatUniqueTypeListService();
                break;
            }
            case "VendorListService": {
                myResult = new VendorListService_1.VendorListService();
                break;
            }
            case "VesselListService": {
                myResult = new VesselListService_1.VesselListService();
                break;
            }
            case "WarehouseListService": {
                myResult = new WarehouseListService_1.WarehouseListService();
                break;
            }
            case "WeightUnitListService": {
                myResult = new WeightUnitListService_1.WeightUnitListService();
                break;
            }
            case "AgentSharedManifestListService": {
                myResult = new AgentSharedManifestListService_1.AgentSharedManifestListService();
                break;
            }
            case "VatFormatTypeListService": {
                myResult = new VatFormatTypeListService_1.VatFormatTypeListService();
                break;
            }
            case "CustomsShipperListService": {
                myResult = new CustomsShipperListService_1.CustomsShipperListService();
                break;
            }
            case "CustomerTenantAccessPMService": {
                myResult = new CustomerTenantAccessPMService_1.CustomerTenantAccessPMService();
                break;
            }
            case "AccountingSettingPMService": {
                myResult = new AccountingSettingPMService_1.AccountingSettingPMService();
                break;
            }
            //case "AccountingSystemPMService": { myResult = new AccountingSystemPMService(); break; }
            case "AdditionalServicePMService": {
                myResult = new AdditionalServicePMService_1.AdditionalServicePMService();
                break;
            }
            case "AddressPMService": {
                myResult = new AddressPMService_1.AddressPMService();
                break;
            }
            case "AgentPMService": {
                myResult = new AgentPMService_1.AgentPMService();
                break;
            }
            case "AirlinePMService": {
                myResult = new AirlinePMService_1.AirlinePMService();
                break;
            }
            case "AirlineStatisticsPMService": {
                myResult = new AirlineStatisticsPMService_1.AirlineStatisticsPMService();
                break;
            }
            case "AutomationPMService": {
                myResult = new AutomationPMService_1.AutomationPMService();
                break;
            }
            case "AutomationResultEmailRecipientPMService": {
                myResult = new AutomationResultEmailRecipientPMService_1.AutomationResultEmailRecipientPMService();
                break;
            }
            case "BranchPMService": {
                myResult = new BranchPMService_1.BranchPMService();
                break;
            }
            case "BusinessUnitPMService": {
                myResult = new BusinessUnitPMService_1.BusinessUnitPMService();
                break;
            }
            case "CardPMService": {
                myResult = new CardPMService_1.CardPMService();
                break;
            }
            case "ChargesTypePMService": {
                myResult = new ChargesTypePMService_1.ChargesTypePMService();
                break;
            }
            case "CommodityPMService": {
                myResult = new CommodityPMService_1.CommodityPMService();
                break;
            }
            case "CommunicationLogPMService": {
                myResult = new CommunicationLogPMService_1.CommunicationLogPMService();
                break;
            }
            //case "CommunicationLogTypePMService": { myResult = new CommunicationLogTypePMService(); break; }
            //case "CommunicationStatusTypePMService": { myResult = new CommunicationStatusTypePMService(); break; }
            case "CompetitorPMService": {
                myResult = new CompetitorPMService_1.CompetitorPMService();
                break;
            }
            //case "ContactDoneMethodPMService": { myResult = new ContactDoneMethodPMService(); break; }
            case "ContactPMService": {
                myResult = new ContactPMService_1.ContactPMService();
                break;
            }
            case "CountryCityPMService": {
                myResult = new CountryCityPMService_1.CountryCityPMService();
                break;
            }
            case "CountryPMService": {
                myResult = new CountryPMService_1.CountryPMService();
                break;
            }
            case "CurrencyPMService": {
                myResult = new CurrencyPMService_1.CurrencyPMService();
                break;
            }
            case "CustomAgentPMService": {
                myResult = new CustomAgentPMService_1.CustomAgentPMService();
                break;
            }
            case "CustomerPMService": {
                myResult = new CustomerPMService_1.CustomerPMService();
                break;
            }
            //case "CustomerProductActualDataPMService": { myResult = new CustomerProductActualDataPMService(); break; }
            //case "CustomerProductLocationPMService": { myResult = new CustomerProductLocationPMService(); break; }
            //case "CustomerProductPMService": { myResult = new CustomerProductPMService(); break; }
            case "CustomerSizePMService": {
                myResult = new CustomerSizePMService_1.CustomerSizePMService();
                break;
            }
            //case "CustomerStatusPMService": { myResult = new CustomerStatusPMService(); break; }
            case "DepartmentPMService": {
                myResult = new DepartmentPMService_1.DepartmentPMService();
                break;
            }
            //case "DimensionsUnitPMService": { myResult = new DimensionsUnitPMService(); break; }
            case "DistributorPMService": {
                myResult = new DistributorPMService_1.DistributorPMService();
                break;
            }
            case "DocumentFolderPMService": {
                myResult = new DocumentFolderPMService_1.DocumentFolderPMService();
                break;
            }
            case "DocumentsFilingPMService": {
                myResult = new DocumentsFilingPMService_1.DocumentsFilingPMService();
                break;
            }
            //case "DocumentTypeCategoryPMService": { myResult = new DocumentTypeCategoryPMService(); break; }
            case "DocumentTypePMService": {
                myResult = new DocumentTypePMService_1.DocumentTypePMService();
                break;
            }
            case "DocumentTypeTemplatePMService": {
                myResult = new DocumentTypeTemplatePMService_1.DocumentTypeTemplatePMService();
                break;
            }
            //case "DueTypePMService": { myResult = new DueTypePMService(); break; }
            case "GlobalZonePMService": {
                myResult = new GlobalZonePMService_1.GlobalZonePMService();
                break;
            }
            case "HybridPartnerPMService": {
                myResult = new HybridPartnerPMService_1.HybridPartnerPMService();
                break;
            }
            //case "HybridTenantStatePMService": { myResult = new HybridTenantStatePMService(); break; }
            //case "HybridTenantThresholdPMService": { myResult = new HybridTenantThresholdPMService(); break; }
            case "IncotermPMService": {
                myResult = new IncotermPMService_1.IncotermPMService();
                break;
            }
            case "IndustryPMService": {
                myResult = new IndustryPMService_1.IndustryPMService();
                break;
            }
            case "LeadSourcePMService": {
                myResult = new LeadSourcePMService_1.LeadSourcePMService();
                break;
            }
            case "LogitudeMessagesTransmissionLogPMService": {
                myResult = new LogitudeMessagesTransmissionLogPMService_1.LogitudeMessagesTransmissionLogPMService();
                break;
            }
            case "MAWBStackPMService": {
                myResult = new MAWBStackPMService_1.MAWBStackPMService();
                break;
            }
            case "MeasurementPMService": {
                myResult = new MeasurementPMService_1.MeasurementPMService();
                break;
            }
            //case "PackageConnectedPackagePMService": { myResult = new PackageConnectedPackagePMService(); break; }
            case "PackagePMService": {
                myResult = new PackagePMService_1.PackagePMService();
                break;
            }
            case "PackageTypePMService": {
                myResult = new PackageTypePMService_1.PackageTypePMService();
                break;
            }
            case "ParticipantPMService": {
                myResult = new ParticipantPMService_1.ParticipantPMService();
                break;
            }
            //case "PartnerTypePMService": { myResult = new PartnerTypePMService(); break; }
            //case "PasswordPolicyPMService": { myResult = new PasswordPolicyPMService(); break; }
            case "PaymentTermPMService": {
                myResult = new PaymentTermPMService_1.PaymentTermPMService();
                break;
            }
            case "PortPMService": {
                myResult = new PortPMService_1.PortPMService();
                break;
            }
            //case "ProductPeriodPMService": { myResult = new ProductPeriodPMService(); break; }
            case "ProductTypePMService": {
                myResult = new ProductTypePMService_1.ProductTypePMService();
                break;
            }
            case "RankPMService": {
                myResult = new RankPMService_1.RankPMService();
                break;
            }
            //case "RateClassPMService": { myResult = new RateClassPMService(); break; }
            case "RegionPMService": {
                myResult = new RegionPMService_1.RegionPMService();
                break;
            }
            case "ReportPMService": {
                myResult = new ReportPMService_1.ReportPMService();
                break;
            }
            //case "RestrictionPMService": { myResult = new RestrictionPMService(); break; }
            case "RolePMService": {
                myResult = new RolePMService_1.RolePMService();
                break;
            }
            case "ShippingAgentPMService": {
                myResult = new ShippingAgentPMService_1.ShippingAgentPMService();
                break;
            }
            case "ShippingLinePMService": {
                myResult = new ShippingLinePMService_1.ShippingLinePMService();
                break;
            }
            case "StatePMService": {
                myResult = new StatePMService_1.StatePMService();
                break;
            }
            //case "TarrifChargePMService": { myResult = new TarrifChargePMService(); break; }
            //case "TarrifFromToPMService": { myResult = new TarrifFromToPMService(); break; }
            //case "TarrifFromToTypePMService": { myResult = new TarrifFromToTypePMService(); break; }
            case "TarrifHeaderPMService": {
                myResult = new TarrifHeaderPMService_1.TarrifHeaderPMService();
                break;
            }
            case "TarrifStepPMService": {
                myResult = new TarrifStepPMService_1.TarrifStepPMService();
                break;
            }
            //case "TarrifTypePMService": { myResult = new TarrifTypePMService(); break; }
            case "TenantPMService": {
                myResult = new TenantPMService_1.TenantPMService();
                break;
            }
            case "TermsofUseSignaturePMService": {
                myResult = new TermsofUseSignaturePMService_1.TermsofUseSignaturePMService();
                break;
            }
            case "TruckerPMService": {
                myResult = new TruckerPMService_1.TruckerPMService();
                break;
            }
            //case "UserLicensePMService": { myResult = new UserLicensePMService(); break; }
            case "UserPMService": {
                myResult = new UserPMService_1.UserPMService();
                break;
            }
            case "VatTypePMService": {
                myResult = new VatTypePMService_1.VatTypePMService();
                break;
            }
            case "VendorPMService": {
                myResult = new VendorPMService_1.VendorPMService();
                break;
            }
            case "VesselPMService": {
                myResult = new VesselPMService_1.VesselPMService();
                break;
            }
            case "WarehousePMService": {
                myResult = new WarehousePMService_1.WarehousePMService();
                break;
            }
            case "CustomsShipperPMService": {
                myResult = new CustomsShipperPMService_1.CustomsShipperPMService();
                break;
            }
            //case "WeightUnitPMService": { myResult = new WeightUnitPMService(); break; }
            case "CustomerMenuButtonsHandler": {
                myResult = new CustomerMenuButtonsHandler_1.CustomerMenuButtonsHandler();
                break;
            }
            case "UserMenuButtonsHandler": {
                myResult = new UserMenuButtonsHandler_1.UserMenuButtonsHandler();
                break;
            }
            case "CommunicationLogMenuButtonsHandler": {
                myResult = new CommunicationLogMenuButtonsHandler_1.CommunicationLogMenuButtonsHandler();
                break;
            }
            case "ContactMenuButtonsHandler": {
                myResult = new ContactMenuButtonsHandler_1.ContactMenuButtonsHandler();
                break;
            }
            case "DocumentsFilingExtendedPMService": {
                myResult = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
                break;
            }
            case "CarrierExtendedListService": {
                myResult = new CarrierExtendedListService_1.CarrierExtendedListService();
                break;
            }
            case "PortExtendedListService": {
                myResult = new PortExtendedListService_1.PortExtendedListService();
                break;
            }
            case "PaymentTermDateTypeListService": {
                myResult = new PaymentTermDateTypeListService_1.PaymentTermDateTypeListService();
                break;
            }
            case "AgentSharedManifestPMService": {
                myResult = new AgentSharedManifestPMService_1.AgentSharedManifestPMService();
                break;
            }
            case "AgentSharedManifestListService": {
                myResult = new AgentSharedManifestListService_1.AgentSharedManifestListService();
                break;
            } //
            case "CustomerFieldsUpdateSettingPMService": {
                myResult = new CustomerFieldsUpdateSettingPMService_1.CustomerFieldsUpdateSettingPMService();
                break;
            } //
            case "CustomerFieldsUpdateSettingListService": {
                myResult = new CustomerFieldsUpdateSettingListService_1.CustomerFieldsUpdateSettingListService();
                break;
            } //
            case "TenantLoginPolicyPMService": {
                myResult = new TenantLoginPolicyPMService_1.TenantLoginPolicyPMService();
                break;
            } //
            case "TwoFactorAuthenticationDeviceExtendedPMService": {
                myResult = new TwoFactorAuthenticationDeviceExtendedPMService_1.TwoFactorAuthenticationDeviceExtendedPMService();
                break;
            } //
            case "LoginPolicyListService": {
                myResult = new LoginPolicyListService_1.LoginPolicyListService();
                break;
            }
            case "TwoFactorAuthenticationDeviceListService": {
                myResult = new TwoFactorAuthenticationDeviceListService_1.TwoFactorAuthenticationDeviceListService();
                break;
            }
            case "FeaturePackageTypeListService": {
                myResult = new FeaturePackageTypeListService_1.FeaturePackageTypeListService();
                break;
            }
            case "MetodoPagoListService": {
                myResult = new MetodoPagoListService_1.MetodoPagoListService();
                break;
            }
            case "UsoCFDIListService": {
                myResult = new UsoCFDIListService_1.UsoCFDIListService();
                break;
            }
            case "ComputingPartnerPMService": {
                myResult = new ComputingPartnerPMService_1.ComputingPartnerPMService();
                break;
            }
            case "ComputingPartnerTranslationPMService": {
                myResult = new ComputingPartnerTranslationPMService_1.ComputingPartnerTranslationPMService();
                break;
            }
            case "CustomerTenantAccessMenuButtonsHandler": {
                myResult = new CustomerTenantAccessMenuButtonsHandler_1.CustomerTenantAccessMenuButtonsHandler();
                break;
            }
            case "CustomerTenantAccessListService": {
                myResult = new CustomerTenantAccessListService_1.CustomerTenantAccessListService();
                break;
            }
            case "TenantLoginPolicyListService": {
                myResult = new TenantLoginPolicyListService_1.TenantLoginPolicyListService();
                break;
            } //
            case "RegistryDateTypeListService": {
                myResult = new RegistryDateTypeListService_1.RegistryDateTypeListService();
                break;
            } //                
            case "WarehouseTypeListService": {
                myResult = new WarehouseTypeListService_1.WarehouseTypeListService();
                break;
            }
            case "ReportsTemplateListService": {
                myResult = new ReportsTemplateListService_1.ReportsTemplateListService();
                break;
            } //                
            case "ReportsTemplatesVersionListService": {
                myResult = new ReportsTemplatesVersionListService_1.ReportsTemplatesVersionListService();
                break;
            }
            case "ReportsTemplatePMService": {
                myResult = new ReportsTemplatePMService_1.ReportsTemplatePMService();
                break;
            }
            case "ReportsTemplatesVersionPMService": {
                myResult = new ReportsTemplatesVersionPMService_1.ReportsTemplatesVersionPMService();
                break;
            } //  
            case "DocumentFilingBackupSettingListService": {
                myResult = new DocumentFilingBackupSettingListService_1.DocumentFilingBackupSettingListService();
                break;
            }
            case "DocumentFilingBackupBatchListService": {
                myResult = new DocumentFilingBackupBatchListService_1.DocumentFilingBackupBatchListService();
                break;
            }
            case "DocumentFilingBackupSettingPMService": {
                myResult = new DocumentFilingBackupSettingPMService_1.DocumentFilingBackupSettingPMService();
                break;
            }
            case "DocumentFilingBackupBatchPMService": {
                myResult = new DocumentFilingBackupBatchPMService_1.DocumentFilingBackupBatchPMService();
                break;
            }
            case "ReportsTemplatesVersionPMService": {
                myResult = new ReportsTemplatesVersionPMService_1.ReportsTemplatesVersionPMService();
                break;
            } //                
            case "TemperatureUnitListService": {
                myResult = new TemperatureUnitListService_1.TemperatureUnitListService();
                break;
            } //
            case "NumberFormatListService": {
                myResult = new NumberFormatListService_1.NumberFormatListService();
                break;
            }
            case "CheckDigitControlAlgorithmListService": {
                myResult = new CheckDigitControlAlgorithmListService_1.CheckDigitControlAlgorithmListService();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map