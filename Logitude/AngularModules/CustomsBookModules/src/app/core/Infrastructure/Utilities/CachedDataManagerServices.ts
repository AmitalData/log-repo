// import {ApiQueryFilters} from '../DataContracts/ApiQueryFilters';
// import {BluesnapContractListService} from '../Services/StandardLists/BluesnapContractListService';
// import {ChargesGroupListService} from '../Services/StandardLists/ChargesGroupListService';
// import {CustomPickListListService} from '../Services/StandardLists/CustomPickListListService';
// import {EntityStatusListService} from '../Services/StandardLists/EntityStatusListService';
// import {EventTypeListService} from '../Services/StandardLists/EventTypeListService';
// import {IATACodeListService} from '../Services/StandardLists/IATACodeListService';
// import {MoveTypeListService} from '../Services/StandardLists/MoveTypeListService';
// import {RatesTableListService} from '../Services/StandardLists/RatesTableListService';
// import {BookingProductListService} from '../../Booking/Services/StandardLists/BookingProductListService';
// import { QuoteStageListService } from '../../Quote/Services/StandardLists/QuoteStageListService';
// import { QuoteClosingReasonListService } from '../../Quote/Services/StandardLists/QuoteClosingReasonListService';
// import {AWBSpecialHandlingCodeListService} from '../../Shipment/Services/StandardLists/AWBSpecialHandlingCodeListService';
// import {AccountingPaymentMethodListService} from '../../Invoice/Services/StandardLists/AccountingPaymentMethodListService';
// import {CreditCardTypeListService} from '../../Invoice/Services/StandardLists/CreditCardTypeListService';
// import {EmployeeGroupListService} from '../../CRM/Services/StandardLists/EmployeeGroupListService';
// import {OpportunityClosingReasonListService} from '../../CRM/Services/StandardLists/OpportunityClosingReasonListService';
// import {OpportunityTypeListService} from '../../CRM/Services/StandardLists/OpportunityTypeListService';
// import {StageListService} from '../../CRM/Services/StandardLists/StageListService';
// import {TicketClassificationListService} from '../../CRM/Services/StandardLists/TicketClassificationListService';
// import {TicketSeverityListService} from '../../CRM/Services/StandardLists/TicketSeverityListService';
// import {TicketStageListService} from '../../CRM/Services/StandardLists/TicketStageListService';
// import {TicketTypeListService} from '../../CRM/Services/StandardLists/TicketTypeListService';
// import {AccountingSettingListService} from '../../Common/Services/StandardLists/AccountingSettingListService';
// import {AdditionalServiceListService} from '../../Common/Services/StandardLists/AdditionalServiceListService';
// import { AirlineListService } from '../../Common/Services/StandardLists/AirlineListService';
// import { AirlineMessagingRuleListService } from '../../Common/Services/StandardLists/AirlineMessagingRuleListService';
// import {BranchListService} from '../../Common/Services/StandardLists/BranchListService';
// import {CarrierListService} from '../../Common/Services/StandardLists/CarrierListService';
// import {ChargesTypeListService} from '../../Common/Services/StandardLists/ChargesTypeListService';
// import {CountryListService} from '../../Common/Services/StandardLists/CountryListService';
// import {CountryCityListService} from '../../Common/Services/StandardLists/CountryCityListService';
// import {CurrencyListService} from '../../Common/Services/StandardLists/CurrencyListService';
// import {CustomerSizeListService} from '../../Common/Services/StandardLists/CustomerSizeListService';
// import {DepartmentListService} from '../../Common/Services/StandardLists/DepartmentListService';
// import {DocumentTypeListService} from '../../Common/Services/StandardLists/DocumentTypeListService';
// import {GlobalZoneListService} from '../../Common/Services/StandardLists/GlobalZoneListService';
// import {HybridPartnerListService} from '../../Common/Services/StandardLists/HybridPartnerListService';
// import {IncotermListService} from '../../Common/Services/StandardLists/IncotermListService';
// import {IndustryListService} from '../../Common/Services/StandardLists/IndustryListService';
// import {LeadSourceListService} from '../../Common/Services/StandardLists/LeadSourceListService';
// import {MeasurementListService} from '../../Common/Services/StandardLists/MeasurementListService';
// import {PackageListService} from '../../Common/Services/StandardLists/PackageListService';
// import {PackageTypeListService} from '../../Common/Services/StandardLists/PackageTypeListService';
// import {PaymentTermListService} from '../../Common/Services/StandardLists/PaymentTermListService';
// import {PortListService} from '../../Common/Services/StandardLists/PortListService';
// import {ProductTypeListService} from '../../Common/Services/StandardLists/ProductTypeListService';
// import {RankListService} from '../../Common/Services/StandardLists/RankListService';
// import {RegionListService} from '../../Common/Services/StandardLists/RegionListService';
// import {ShippingLineListService} from '../../Common/Services/StandardLists/ShippingLineListService';
// import {StateListService} from '../../Common/Services/StandardLists/StateListService';
// import {TruckerListService} from '../../Common/Services/StandardLists/TruckerListService';
// import {UserListService} from '../../Common/Services/StandardLists/UserListService';
// import {VatTypeListService} from '../../Common/Services/StandardLists/VatTypeListService';
// import {VesselListService} from '../../Common/Services/StandardLists/VesselListService';
// import {WarehouseListService} from '../../Common/Services/StandardLists/WarehouseListService';
// import { JournalActionTypeListService } from '../../Accounting/Services/StandardLists/JournalActionTypeListService';
// import { BluesnapContractTypeListService } from '../Services/StandardLists/BluesnapContractTypeListService';
// import { HorseListService } from '../../Common/Services/StandardLists/HorseListService';

// //customs
// import {InternationalSiteListService} from '../../Customs/Services/StandardLists/InternationalSiteListService';
// import {CustomDocumentTypeListService} from '../../Customs/Services/StandardLists/CustomDocumentTypeListService';
// import {CustomsRequiredFieldListService} from '../../Customs/Services/StandardLists/CustomsRequiredFieldListService';
// import {InterfaceManagementListService} from '../../Customs/Services/StandardLists/InterfaceManagementListService';
// import {UIMessageAdditionalListService} from '../../Customs/Services/StandardLists/UIMessageAdditionalListService';
// import {CustomBankListService} from '../../Customs/Services/StandardLists/CustomBankListService';
// import {CustomsHouseTypeListService} from '../../Customs/Services/StandardLists/CustomsHouseTypeListService';
// import {CustomsSettingListService} from '../../Customs/Services/StandardLists/CustomsSettingListService';
// import {CustomsHouseTypeAdditionalListService} from '../../Customs/Services/StandardLists/CustomsHouseTypeAdditionalListService';
// import { GovernmentProcedureTypeListService } from '../../Customs/Services/StandardLists/GovernmentProcedureTypeListService';
// import { CourierPendingReasonListService } from '../../Customs/Services/StandardLists/CourierPendingReasonListService';
// import { InterfaceTenantDefinitionListService } from '../../Customs/Services/StandardLists/InterfaceTenantDefinitionListService';
// import { CustomsItemListService } from '../../Customs/Services/StandardLists/CustomsItemListService';
// import { CurrencyTypeListService } from '../../Customs/Services/StandardLists/CurrencyTypeListService';
// import { StorageStatusTableListService } from '../../Customs/Services/StandardLists/StorageStatusTableListService';
// import { NbcDeclarationTypeListService } from '../../Customs/Services/StandardLists/NbcDeclarationTypeListService';
// import { PartyRelationshipTypeListService } from '../../Customs/Services/StandardLists/PartyRelationshipTypeListService';
// import { AmountTypeListService } from '../../Customs/Services/StandardLists/AmountTypeListService';
// import { ClaimReasonTypeListService } from '../../Customs/Services/StandardLists/ClaimReasonTypeListService';
// import { ClassificationTypeListService } from '../../Customs/Services/StandardLists/ClassificationTypeListService';
// import { TransactionNatureTypeListService } from '../../Customs/Services/StandardLists/TransactionNatureTypeListService';
// import { LogisticActionResponseReqSListService } from '../../Customs/Services/StandardLists/LogisticActionResponseReqSListService';
// //import { AutonomyRegionTypeListService } from '../../Customs/Services/StandardLists/AutonomyRegionTypeListService';

// // Business Process
// import {BusinessRoleListService} from '../Services/StandardLists/BusinessRoleListService';
// import {BusinessProcessQueueListService } from '../Services/StandardLists/BusinessProcessQueueListService';
// import {TeamListService} from '../Services/StandardLists/TeamListService';

// import {TMBudgetListService} from '../../TimeManagement/Services/StandardLists/TMBudgetListService';
// import {TMProjectCategoryListService} from '../../TimeManagement/Services/StandardLists/TMProjectCategoryListService';
// import {SprintListService} from '../../TimeManagement/Services/StandardLists/SprintListService';
// import { TenantManagmentPrivateLabelsListService } from '../../Infrastructure/Services/StandardLists/TenantManagmentPrivateLabelsListService';
// import { BIReportsTypeListService } from  '../Services/StandardLists/BIReportsTypeListService';
// import { FeatureToggleListService } from '../Services/StandardLists/FeatureToggleListService';

// // Tariff Module
// import { TariffListService } from '../../TariffModule/Services/StandardLists/TariffListService';
// import { TariffTypeListService } from '../../TariffModule/Services/StandardLists/TariffTypeListService';
// import { TariffProductListService } from '../../TariffModule/Services/StandardLists/TariffProductListService';

// //Occasions
// import { OccasionStatusListService } from '../../CRM/Services/StandardLists/OccasionStatusListService';
// import { OccasionTypeListService } from '../../CRM/Services/StandardLists/OccasionTypeListService';

// import { AWBAdditionalHandlingInfoListService } from '../../Shipment/Services/StandardLists/AWBAdditionalHandlingInfoListService';

// import { ShipmentSubTypeListService } from '../../Shipment/services/standardlists/shipmentsubtypelistservice';

// import { SessionLocator } from './SessionLocator';
// import { ObjectsLocator } from '../Locators/ObjectsLocator';
// import { AppTool } from '../Tools';
// import { ExceptionReasonListService } from '../../Customs/Services/StandardLists/ExceptionReasonListService';
// import { CustomerRoleTypeListService } from '../../Customs/Services/StandardLists/CustomerRoleTypeListService';
// import { AutonomyRegionTypeListService } from '../../Customs/Services/StandardLists/AutonomyRegionTypeListService';
//  import { CancellationRequestStatusListService } from '../../Customs/Services/StandardLists/CancellationRequestStatusListService';
// import { CancellationReasonRequestTypeListService } from '../../Customs/Services/StandardLists/CancellationReasonRequestTypeListService';
// import { ReferantTeamListService } from '../../Customs/Services/StandardLists/ReferantTeamListService';
// import { ReferenceStatusListService } from '../../Customs/Services/StandardLists/ReferenceStatusListService';
// import { ReferenceInputTypeListService } from '../../Customs/Services/StandardLists/ReferenceInputTypeListService';
// import { LogisticsReferenceTypeListService } from '../../Customs/Services/StandardLists/LogisticsReferenceTypeListService';
// import { CancelRequestRejectReasonTypeListService } from '../../Customs/Services/StandardLists/CancelRequestRejectReasonTypeListService';
// import { BuyerRoleTypeListService } from '../../Customs/Services/StandardLists/BuyerRoleTypeListService';
// import { CurrencyTypeTenantListService } from 'Customs/Services/StandardLists/CurrencyTypeTenantListService';

// export class CachedDataManagerServices {
//     public getAllFromCache(objectTableName: string, filters: ApiQueryFilters) {

//         var serviceName: string = objectTableName + "ListService";
//         var service: any = this.GetServiceInstance(serviceName);

//         return new Promise((resolve, reject) => {
//             try {
//                 resolve(service.getAllFromCache(filters));
//             }

//             catch (e) {
//                 reject(new Error("service.getAllFromCache is not a function: " + service._apiUrl));
//             }
//         });
//     }
//     private GetServiceInstance(name: string) {

//         var myResult: any = null;

//         switch (name) {
//             case "BluesnapContractListService": { myResult = new BluesnapContractListService(); break; }
//             case "ChargesGroupListService": { myResult = new ChargesGroupListService(); break; }
//             case "CustomPickListListService": { myResult = new CustomPickListListService(); break; }
//             case "EntityStatusListService": { myResult = new EntityStatusListService(); break; }
//             case "EventTypeListService": { myResult = new EventTypeListService(); break; }
//             case "IATACodeListService": { myResult = new IATACodeListService(); break; }
//             case "MoveTypeListService": { myResult = new MoveTypeListService(); break; }
//             case "RatesTableListService": { myResult = new RatesTableListService(); break; }
//             case "BookingProductListService": { myResult = new BookingProductListService(); break; }
//             case "QuoteStageListService": { myResult = new QuoteStageListService(); break; }
//             case "QuoteClosingReasonListService": { myResult = new QuoteClosingReasonListService(); break; }
//             case "AWBSpecialHandlingCodeListService": { myResult = new AWBSpecialHandlingCodeListService(); break; }
//             case "AccountingPaymentMethodListService": { myResult = new AccountingPaymentMethodListService(); break; }
//             case "CreditCardTypeListService": { myResult = new CreditCardTypeListService(); break; }
//             case "EmployeeGroupListService": { myResult = new EmployeeGroupListService(); break; }
//             case "OpportunityClosingReasonListService": { myResult = new OpportunityClosingReasonListService(); break; }
//             case "OpportunityTypeListService": { myResult = new OpportunityTypeListService(); break; }
//             case "StageListService": { myResult = new StageListService(); break; }
//             case "TicketClassificationListService": { myResult = new TicketClassificationListService(); break; }
//             case "TicketSeverityListService": { myResult = new TicketSeverityListService(); break; }
//             case "TicketStageListService": { myResult = new TicketStageListService(); break; }
//             case "TicketTypeListService": { myResult = new TicketTypeListService(); break; }
//             case "AccountingSettingListService": { myResult = new AccountingSettingListService(); break; }
//             case "AdditionalServiceListService": { myResult = new AdditionalServiceListService(); break; }
//             case "AirlineListService": { myResult = new AirlineListService(); break; }
//             case "AirlineMessagingRuleListService": { myResult = new AirlineMessagingRuleListService(); break; }
//             case "BranchListService": { myResult = new BranchListService(); break; }
//             case "CarrierListService": { myResult = new CarrierListService(); break; }
//             case "ChargesTypeListService": { myResult = new ChargesTypeListService(); break; }
//             case "CountryListService": { myResult = new CountryListService(); break; }
//             case "CountryCityListService": { myResult = new CountryCityListService(); break; }
//             case "CurrencyListService": { myResult = new CurrencyListService(); break; }
//             case "CustomerSizeListService": { myResult = new CustomerSizeListService(); break; }
//             case "DepartmentListService": { myResult = new DepartmentListService(); break; }
//             case "DocumentTypeListService": { myResult = new DocumentTypeListService(); break; }
//             case "GlobalZoneListService": { myResult = new GlobalZoneListService(); break; }
//             case "HybridPartnerListService": { myResult = new HybridPartnerListService(); break; }
//             case "IncotermListService": { myResult = new IncotermListService(); break; }
//             case "IndustryListService": { myResult = new IndustryListService(); break; }
//             case "LeadSourceListService": { myResult = new LeadSourceListService(); break; }
//             case "MeasurementListService": { myResult = new MeasurementListService(); break; }
//             case "PackageListService": { myResult = new PackageListService(); break; }
//             case "PackageTypeListService": { myResult = new PackageTypeListService(); break; }
//             case "PaymentTermListService": { myResult = new PaymentTermListService(); break; }
//             case "PortListService": { myResult = new PortListService(); break; }
//             case "ProductTypeListService": { myResult = new ProductTypeListService(); break; }
//             case "RankListService": { myResult = new RankListService(); break; }
//             case "RegionListService": { myResult = new RegionListService(); break; }
//             case "ShippingLineListService": { myResult = new ShippingLineListService(); break; }
//             case "StateListService": { myResult = new StateListService(); break; }
//             case "TruckerListService": { myResult = new TruckerListService(); break; }
//             case "UserListService": { myResult = new UserListService(); break; }
//             case "VatTypeListService": { myResult = new VatTypeListService(); break; }
//             case "VesselListService": { myResult = new VesselListService(); break; }
//             case "WarehouseListService": { myResult = new WarehouseListService(); break; }
//             case "Customs.InternationalSiteListService": { myResult = new InternationalSiteListService(); break; }
//             case "Customs.CustomDocumentTypeListService": { myResult = new CustomDocumentTypeListService(); break; }
//             case "Customs.CustomsRequiredFieldListService": { myResult = new CustomsRequiredFieldListService(); break; }
//             case "Customs.InterfaceManagementListService": { myResult = new InterfaceManagementListService(); break; }
//             case "Customs.UIMessageAdditionalListService": { myResult = new UIMessageAdditionalListService(); break; }
//             case "Customs.InterfaceTenantDefinitionListService": { myResult = new InterfaceTenantDefinitionListService(); break; }
//             case "Customs.CustomsItemListService": { myResult = new CustomsItemListService(); break; }

//             case "Customs.CustomBankListService": { myResult = new CustomBankListService(); break; }
//             case "Customs.CustomsHouseTypeListService": { myResult = new CustomsHouseTypeListService(); break; }
//             case "Customs.CurrencyTypeListService": { myResult = new CurrencyTypeListService(); break; }
//             case "Customs.StorageStatusTableListService": { myResult = new StorageStatusTableListService(); break; }
//             case "Customs.CustomsSettingListService": { myResult = new CustomsSettingListService(); break; }
//             case "Customs.CustomsHouseTypeAdditionalListService": { myResult = new CustomsHouseTypeAdditionalListService(); break; }
//             case "Customs.GovernmentProcedureTypeListService": { myResult = new GovernmentProcedureTypeListService(); break; }
//             case "Customs.CourierPendingReasonListService": { myResult = new CourierPendingReasonListService(); break; }
//             case "Customs.ExceptionReasonListService": { myResult = new ExceptionReasonListService(); break; }
//             case "JournalActionTypeListService": { myResult = new JournalActionTypeListService(); break; }
//             case "BusinessRoleListService": { myResult = new BusinessRoleListService(); break; }
//             case "BusinessProcessQueueListService": { myResult = new BusinessProcessQueueListService(); break; }
//             case "TeamListService": { myResult = new TeamListService(); break; }
//             case "TMBudgetListService": { myResult = new TMBudgetListService(); break; }
//             case "TMProjectCategoryListService": { myResult = new TMProjectCategoryListService(); break; }
//             case "SprintListService": { myResult = new SprintListService(); break; }
//             case "TenantManagmentPrivateLabelsListService": { myResult = new TenantManagmentPrivateLabelsListService(); break; }
//             case "BIReportsTypeListService": { myResult = new BIReportsTypeListService(); break; }
//             case "FeatureToggleListService": { myResult = new FeatureToggleListService(); break; }
//             case "BluesnapContractTypeListService": { myResult = new BluesnapContractTypeListService(); break; }
//             case "TariffListService": { myResult = new TariffListService(); break; }
//             case "TariffTypeListService": { myResult = new TariffTypeListService(); break; }
//             case "OccasionTypeListService": { myResult = new OccasionTypeListService(); break; }
//             case "Customs.TransactionNatureTypeListService": { myResult = new TransactionNatureTypeListService(); break; }
//             case "Customs.ClassificationTypeListService": { myResult = new ClassificationTypeListService(); break; }
//             case "Customs.ClaimReasonTypeListService": { myResult = new ClaimReasonTypeListService(); break; }
//             case "Customs.AmountTypeListService": { myResult = new AmountTypeListService(); break; }
//             case "Customs.PartyRelationshipTypeListService": { myResult = new PartyRelationshipTypeListService(); break; }
//             case "Customs.NbcDeclarationTypeListService": { myResult = new NbcDeclarationTypeListService(); break; }
//             case "Customs.CustomerRoleTypeListService": { myResult = new CustomerRoleTypeListService() ; break; }
//             case "Customs.AutonomyRegionTypeListService": { myResult = new AutonomyRegionTypeListService(); break; }
//             case "Customs.CustomerRoleTypeListService": { myResult = new CustomerRoleTypeListService(); break; }
//             case "Customs.CancellationReasonRequestTypeListService": { myResult = new CancellationReasonRequestTypeListService(); break; }
//             case "Customs.CancellationRequestStatusListService": { myResult = new CancellationRequestStatusListService(); break; }
//             case "Customs.ReferantTeamListService": { myResult = new ReferantTeamListService(); break; }
//             case "Customs.CancelRequestRejectReasonTypeListService": { myResult = new CancelRequestRejectReasonTypeListService(); break; }
//             case "Customs.LogisticsReferenceTypeListService": { myResult = new LogisticsReferenceTypeListService(); break; }
//             case "Customs.ReferenceInputTypeListService": { myResult = new ReferenceInputTypeListService(); break; }
//             case "Customs.ReferenceStatusListService": { myResult = new ReferenceStatusListService(); break; }
//             case "Customs.BuyerRoleTypeListService": { myResult = new BuyerRoleTypeListService(); break; }

//             case "AWBAdditionalHandlingInfoListService": { myResult = new AWBAdditionalHandlingInfoListService(); break; }

//             case "TariffProductListService": { myResult = new TariffProductListService(); break; }
//             case "AWBAdditionalHandlingInfoListService": { myResult = new AWBAdditionalHandlingInfoListService(); break; }
//             case "TariffProductListService": { myResult = new TariffProductListService(); break; }
//             case "ShipmentSubTypeListService": { myResult = new ShipmentSubTypeListService(); break; }
//             case "HorseListService": { myResult = new HorseListService(); break; }
//             case "LogisticActionResponseReqSListService": { myResult = new LogisticActionResponseReqSListService(); break; }
//             case "CurrencyTypeTenantListService": { myResult = new CurrencyTypeTenantListService(); break; }

//             default: {

//                 if (ObjectsLocator.GlobalSetting && ObjectsLocator.GlobalSetting.WorkEnvironment == "customs") {
//                     console.error(name + " is not declared in CachedDataManagerServices")
//                 } else {
//                     alert(name + " is not declared in CachedDataManagerServices");
//                 }
//                 break;
//             }
//         }

//         return myResult;
//     }

//     // select * from ObjectTables where Tenant = 0 and CacheOnClient = 1 and IsClosed = 0 and ClientModuleName is not null order by Name
//     // 54

// /*
// select ClientModuleName, count( *)
// from ObjectTables where Tenant = 0 and CacheOnClient = 1 and IsClosed = 0 and ClientModuleName is not null
// group by ClientModuleName

// Booking	1
// Common	32
// CRM	8
// Infrastructure	8
// Invoice	3
// Quote	1
// Shipment	1
// */

// /*
// AccountingSetting
// AdditionalService
// Airline
// AWBSpecialHandlingCode
// BluesnapContract
// BookingProduct
// Branch
// Carrier
// ChargesGroup
// ChargesType
// Country
// CountryCity
// CreditCardType
// Currency
// CustomerSize
// CustomPickList
// Department
// DocumentType
// EmployeeGroup
// EntityStatus
// EventType
// GlobalZone
// HybridPartner
// IATACode
// Incoterm
// Industry
// LeadSource
// Measurement
// MoveType
// OpportunityClosingReason
// OpportunityType
// Package
// PackageType
// PaymentTerm
// Port
// ProductType
// QuoteStage
// Rank
// RatesTable
// Region
// ShippingLine
// Stage
// State
// TicketClassification
// TicketSeverity
// TicketStage
// TicketType
// Trucker
// User
// VatType
// Vessel
// Warehouse

//     */
// }
