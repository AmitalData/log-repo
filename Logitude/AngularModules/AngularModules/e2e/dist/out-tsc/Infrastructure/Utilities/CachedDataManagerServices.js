"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BluesnapContractListService_1 = require("../Services/StandardLists/BluesnapContractListService");
var ChargesGroupListService_1 = require("../Services/StandardLists/ChargesGroupListService");
var CustomPickListListService_1 = require("../Services/StandardLists/CustomPickListListService");
var EntityStatusListService_1 = require("../Services/StandardLists/EntityStatusListService");
var EventTypeListService_1 = require("../Services/StandardLists/EventTypeListService");
var IATACodeListService_1 = require("../Services/StandardLists/IATACodeListService");
var MoveTypeListService_1 = require("../Services/StandardLists/MoveTypeListService");
var RatesTableListService_1 = require("../Services/StandardLists/RatesTableListService");
var BookingProductListService_1 = require("../../Booking/Services/StandardLists/BookingProductListService");
var QuoteStageListService_1 = require("../../Quote/Services/StandardLists/QuoteStageListService");
var AWBSpecialHandlingCodeListService_1 = require("../../Shipment/Services/StandardLists/AWBSpecialHandlingCodeListService");
var APPaymentMethodListService_1 = require("../../Invoice/Services/StandardLists/APPaymentMethodListService");
var AccountingPaymentMethodListService_1 = require("../../Invoice/Services/StandardLists/AccountingPaymentMethodListService");
var CreditCardTypeListService_1 = require("../../Invoice/Services/StandardLists/CreditCardTypeListService");
var EmployeeGroupListService_1 = require("../../CRM/Services/StandardLists/EmployeeGroupListService");
var OpportunityClosingReasonListService_1 = require("../../CRM/Services/StandardLists/OpportunityClosingReasonListService");
var OpportunityTypeListService_1 = require("../../CRM/Services/StandardLists/OpportunityTypeListService");
var StageListService_1 = require("../../CRM/Services/StandardLists/StageListService");
var TicketClassificationListService_1 = require("../../CRM/Services/StandardLists/TicketClassificationListService");
var TicketSeverityListService_1 = require("../../CRM/Services/StandardLists/TicketSeverityListService");
var TicketStageListService_1 = require("../../CRM/Services/StandardLists/TicketStageListService");
var TicketTypeListService_1 = require("../../CRM/Services/StandardLists/TicketTypeListService");
var AccountingSettingListService_1 = require("../../Common/Services/StandardLists/AccountingSettingListService");
var AdditionalServiceListService_1 = require("../../Common/Services/StandardLists/AdditionalServiceListService");
var AirlineListService_1 = require("../../Common/Services/StandardLists/AirlineListService");
var BranchListService_1 = require("../../Common/Services/StandardLists/BranchListService");
var CarrierListService_1 = require("../../Common/Services/StandardLists/CarrierListService");
var ChargesTypeListService_1 = require("../../Common/Services/StandardLists/ChargesTypeListService");
var CountryListService_1 = require("../../Common/Services/StandardLists/CountryListService");
var CountryCityListService_1 = require("../../Common/Services/StandardLists/CountryCityListService");
var CurrencyListService_1 = require("../../Common/Services/StandardLists/CurrencyListService");
var CustomerSizeListService_1 = require("../../Common/Services/StandardLists/CustomerSizeListService");
var DepartmentListService_1 = require("../../Common/Services/StandardLists/DepartmentListService");
var DocumentTypeListService_1 = require("../../Common/Services/StandardLists/DocumentTypeListService");
var GlobalZoneListService_1 = require("../../Common/Services/StandardLists/GlobalZoneListService");
var HybridPartnerListService_1 = require("../../Common/Services/StandardLists/HybridPartnerListService");
var IncotermListService_1 = require("../../Common/Services/StandardLists/IncotermListService");
var IndustryListService_1 = require("../../Common/Services/StandardLists/IndustryListService");
var LeadSourceListService_1 = require("../../Common/Services/StandardLists/LeadSourceListService");
var MeasurementListService_1 = require("../../Common/Services/StandardLists/MeasurementListService");
var PackageListService_1 = require("../../Common/Services/StandardLists/PackageListService");
var PackageTypeListService_1 = require("../../Common/Services/StandardLists/PackageTypeListService");
var PaymentTermListService_1 = require("../../Common/Services/StandardLists/PaymentTermListService");
var PortListService_1 = require("../../Common/Services/StandardLists/PortListService");
var ProductTypeListService_1 = require("../../Common/Services/StandardLists/ProductTypeListService");
var RankListService_1 = require("../../Common/Services/StandardLists/RankListService");
var RegionListService_1 = require("../../Common/Services/StandardLists/RegionListService");
var ShippingLineListService_1 = require("../../Common/Services/StandardLists/ShippingLineListService");
var StateListService_1 = require("../../Common/Services/StandardLists/StateListService");
var TruckerListService_1 = require("../../Common/Services/StandardLists/TruckerListService");
var UserListService_1 = require("../../Common/Services/StandardLists/UserListService");
var VatTypeListService_1 = require("../../Common/Services/StandardLists/VatTypeListService");
var VesselListService_1 = require("../../Common/Services/StandardLists/VesselListService");
var WarehouseListService_1 = require("../../Common/Services/StandardLists/WarehouseListService");
var JournalActionTypeListService_1 = require("../../Accounting/Services/StandardLists/JournalActionTypeListService");
var BluesnapContractTypeListService_1 = require("../Services/StandardLists/BluesnapContractTypeListService");
//customs
var InternationalSiteListService_1 = require("../../Customs/Services/StandardLists/InternationalSiteListService");
var CustomDocumentTypeListService_1 = require("../../Customs/Services/StandardLists/CustomDocumentTypeListService");
var CustomsRequiredFieldListService_1 = require("../../Customs/Services/StandardLists/CustomsRequiredFieldListService");
var InterfaceManagementListService_1 = require("../../Customs/Services/StandardLists/InterfaceManagementListService");
var UIMessageAdditionalListService_1 = require("../../Customs/Services/StandardLists/UIMessageAdditionalListService");
var CustomBankListService_1 = require("../../Customs/Services/StandardLists/CustomBankListService");
var CustomsHouseTypeListService_1 = require("../../Customs/Services/StandardLists/CustomsHouseTypeListService");
var CustomsSettingListService_1 = require("../../Customs/Services/StandardLists/CustomsSettingListService");
var CustomsHouseTypeAdditionalListService_1 = require("../../Customs/Services/StandardLists/CustomsHouseTypeAdditionalListService");
var GovernmentProcedureTypeListService_1 = require("../../Customs/Services/StandardLists/GovernmentProcedureTypeListService");
// Business Process 
var BusinessRoleListService_1 = require("../Services/StandardLists/BusinessRoleListService");
var BusinessProcessQueueListService_1 = require("../Services/StandardLists/BusinessProcessQueueListService");
var TeamListService_1 = require("../Services/StandardLists/TeamListService");
var ARPaymentMethodListService_1 = require("../../Invoice/Services/StandardLists/ARPaymentMethodListService");
var TMBudgetListService_1 = require("../../TimeManagement/Services/StandardLists/TMBudgetListService");
var TMProjectCategoryListService_1 = require("../../TimeManagement/Services/StandardLists/TMProjectCategoryListService");
var SprintListService_1 = require("../../TimeManagement/Services/StandardLists/SprintListService");
var TenantManagmentPrivateLabelsListService_1 = require("../../Infrastructure/Services/StandardLists/TenantManagmentPrivateLabelsListService");
var BIReportsTypeListService_1 = require("../Services/StandardLists/BIReportsTypeListService");
var FeatureToggleListService_1 = require("../Services/StandardLists/FeatureToggleListService");
// Tariff Module
var TariffListService_1 = require("../../TariffModule/Services/StandardLists/TariffListService");
var TariffTypeListService_1 = require("../../TariffModule/Services/StandardLists/TariffTypeListService");
var OccasionTypeListService_1 = require("../../CRM/Services/StandardLists/OccasionTypeListService");
var CachedDataManagerServices = /** @class */ (function () {
    function CachedDataManagerServices() {
    }
    CachedDataManagerServices.prototype.getAllFromCache = function (objectTableName, filters) {
        var serviceName = objectTableName + "ListService";
        var service = this.GetServiceInstance(serviceName);
        return new Promise(function (resolve, reject) {
            try {
                resolve(service.getAllFromCache(filters));
            }
            catch (e) {
                reject(new Error("service.getAllFromCache is not a function: " + service._apiUrl));
            }
        });
    };
    CachedDataManagerServices.prototype.GetServiceInstance = function (name) {
        var myResult = null;
        switch (name) {
            case "BluesnapContractListService": {
                myResult = new BluesnapContractListService_1.BluesnapContractListService();
                break;
            }
            case "ChargesGroupListService": {
                myResult = new ChargesGroupListService_1.ChargesGroupListService();
                break;
            }
            case "CustomPickListListService": {
                myResult = new CustomPickListListService_1.CustomPickListListService();
                break;
            }
            case "EntityStatusListService": {
                myResult = new EntityStatusListService_1.EntityStatusListService();
                break;
            }
            case "EventTypeListService": {
                myResult = new EventTypeListService_1.EventTypeListService();
                break;
            }
            case "IATACodeListService": {
                myResult = new IATACodeListService_1.IATACodeListService();
                break;
            }
            case "MoveTypeListService": {
                myResult = new MoveTypeListService_1.MoveTypeListService();
                break;
            }
            case "RatesTableListService": {
                myResult = new RatesTableListService_1.RatesTableListService();
                break;
            }
            case "BookingProductListService": {
                myResult = new BookingProductListService_1.BookingProductListService();
                break;
            }
            case "QuoteStageListService": {
                myResult = new QuoteStageListService_1.QuoteStageListService();
                break;
            }
            case "AWBSpecialHandlingCodeListService": {
                myResult = new AWBSpecialHandlingCodeListService_1.AWBSpecialHandlingCodeListService();
                break;
            }
            case "APPaymentMethodListService": {
                myResult = new APPaymentMethodListService_1.APPaymentMethodListService();
                break;
            }
            case "AccountingPaymentMethodListService": {
                myResult = new AccountingPaymentMethodListService_1.AccountingPaymentMethodListService();
                break;
            }
            case "CreditCardTypeListService": {
                myResult = new CreditCardTypeListService_1.CreditCardTypeListService();
                break;
            }
            case "EmployeeGroupListService": {
                myResult = new EmployeeGroupListService_1.EmployeeGroupListService();
                break;
            }
            case "OpportunityClosingReasonListService": {
                myResult = new OpportunityClosingReasonListService_1.OpportunityClosingReasonListService();
                break;
            }
            case "OpportunityTypeListService": {
                myResult = new OpportunityTypeListService_1.OpportunityTypeListService();
                break;
            }
            case "StageListService": {
                myResult = new StageListService_1.StageListService();
                break;
            }
            case "TicketClassificationListService": {
                myResult = new TicketClassificationListService_1.TicketClassificationListService();
                break;
            }
            case "TicketSeverityListService": {
                myResult = new TicketSeverityListService_1.TicketSeverityListService();
                break;
            }
            case "TicketStageListService": {
                myResult = new TicketStageListService_1.TicketStageListService();
                break;
            }
            case "TicketTypeListService": {
                myResult = new TicketTypeListService_1.TicketTypeListService();
                break;
            }
            case "AccountingSettingListService": {
                myResult = new AccountingSettingListService_1.AccountingSettingListService();
                break;
            }
            case "AdditionalServiceListService": {
                myResult = new AdditionalServiceListService_1.AdditionalServiceListService();
                break;
            }
            case "AirlineListService": {
                myResult = new AirlineListService_1.AirlineListService();
                break;
            }
            case "BranchListService": {
                myResult = new BranchListService_1.BranchListService();
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
            case "CountryListService": {
                myResult = new CountryListService_1.CountryListService();
                break;
            }
            case "CountryCityListService": {
                myResult = new CountryCityListService_1.CountryCityListService();
                break;
            }
            case "CurrencyListService": {
                myResult = new CurrencyListService_1.CurrencyListService();
                break;
            }
            case "CustomerSizeListService": {
                myResult = new CustomerSizeListService_1.CustomerSizeListService();
                break;
            }
            case "DepartmentListService": {
                myResult = new DepartmentListService_1.DepartmentListService();
                break;
            }
            case "DocumentTypeListService": {
                myResult = new DocumentTypeListService_1.DocumentTypeListService();
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
            case "PaymentTermListService": {
                myResult = new PaymentTermListService_1.PaymentTermListService();
                break;
            }
            case "PortListService": {
                myResult = new PortListService_1.PortListService();
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
            case "RegionListService": {
                myResult = new RegionListService_1.RegionListService();
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
            case "TruckerListService": {
                myResult = new TruckerListService_1.TruckerListService();
                break;
            }
            case "UserListService": {
                myResult = new UserListService_1.UserListService();
                break;
            }
            case "VatTypeListService": {
                myResult = new VatTypeListService_1.VatTypeListService();
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
            case "Customs.InternationalSiteListService": {
                myResult = new InternationalSiteListService_1.InternationalSiteListService();
                break;
            }
            case "Customs.CustomDocumentTypeListService": {
                myResult = new CustomDocumentTypeListService_1.CustomDocumentTypeListService();
                break;
            }
            case "Customs.CustomsRequiredFieldListService": {
                myResult = new CustomsRequiredFieldListService_1.CustomsRequiredFieldListService();
                break;
            }
            case "Customs.InterfaceManagementListService": {
                myResult = new InterfaceManagementListService_1.InterfaceManagementListService();
                break;
            }
            case "Customs.UIMessageAdditionalListService": {
                myResult = new UIMessageAdditionalListService_1.UIMessageAdditionalListService();
                break;
            }
            case "Customs.CustomBankListService": {
                myResult = new CustomBankListService_1.CustomBankListService();
                break;
            }
            case "Customs.CustomsHouseTypeListService": {
                myResult = new CustomsHouseTypeListService_1.CustomsHouseTypeListService();
                break;
            }
            case "Customs.CustomsSettingListService": {
                myResult = new CustomsSettingListService_1.CustomsSettingListService();
                break;
            }
            case "Customs.CustomsHouseTypeAdditionalListService": {
                myResult = new CustomsHouseTypeAdditionalListService_1.CustomsHouseTypeAdditionalListService();
                break;
            }
            case "Customs.GovernmentProcedureTypeListService": {
                myResult = new GovernmentProcedureTypeListService_1.GovernmentProcedureTypeListService();
                break;
            }
            case "JournalActionTypeListService": {
                myResult = new JournalActionTypeListService_1.JournalActionTypeListService();
                break;
            }
            case "BusinessRoleListService": {
                myResult = new BusinessRoleListService_1.BusinessRoleListService();
                break;
            }
            case "BusinessProcessQueueListService": {
                myResult = new BusinessProcessQueueListService_1.BusinessProcessQueueListService();
                break;
            }
            case "TeamListService": {
                myResult = new TeamListService_1.TeamListService();
                break;
            }
            case "ARPaymentMethodListService": {
                myResult = new ARPaymentMethodListService_1.ARPaymentMethodListService();
                break;
            }
            case "TMBudgetListService": {
                myResult = new TMBudgetListService_1.TMBudgetListService();
                break;
            }
            case "TMProjectCategoryListService": {
                myResult = new TMProjectCategoryListService_1.TMProjectCategoryListService();
                break;
            }
            case "SprintListService": {
                myResult = new SprintListService_1.SprintListService();
                break;
            }
            case "TenantManagmentPrivateLabelsListService": {
                myResult = new TenantManagmentPrivateLabelsListService_1.TenantManagmentPrivateLabelsListService();
                break;
            }
            case "BIReportsTypeListService": {
                myResult = new BIReportsTypeListService_1.BIReportsTypeListService();
                break;
            }
            case "FeatureToggleListService": {
                myResult = new FeatureToggleListService_1.FeatureToggleListService();
                break;
            }
            case "BluesnapContractTypeListService": {
                myResult = new BluesnapContractTypeListService_1.BluesnapContractTypeListService();
                break;
            }
            case "TariffListService": {
                myResult = new TariffListService_1.TariffListService();
                break;
            }
            case "TariffTypeListService": {
                myResult = new TariffTypeListService_1.TariffTypeListService();
                break;
            }
            case "OccasionTypeListService": {
                myResult = new OccasionTypeListService_1.OccasionTypeListService();
                break;
            }
            default: {
                alert(name + " is not declared in CachedDataManagerServices");
                break;
            }
        }
        return myResult;
    };
    return CachedDataManagerServices;
}());
exports.CachedDataManagerServices = CachedDataManagerServices;
//# sourceMappingURL=CachedDataManagerServices.js.map