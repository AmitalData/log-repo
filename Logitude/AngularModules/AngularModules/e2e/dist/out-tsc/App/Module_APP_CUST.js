"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var http_1 = require("@angular/http");
var router_1 = require("@angular/router");
var AppComponent_Cust_1 = require("./AppComponent_Cust");
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            imports: [platform_browser_1.BrowserModule, http_1.HttpModule],
            declarations: [AppComponent_Cust_1.AppComponent_Cust],
            providers: [
                core_1.SystemJsNgModuleLoader,
                router_1.provideRoutes([
                    // controls
                    { loadChildren: 'Controls/Module_CTRL#ControlsModule' },
                    // Infrastructure
                    { loadChildren: 'Infrastructure/Module_INFR#InfrastructureModule' },
                    { loadChildren: 'InfrastructureModules/InfrastructureAutomation/ModuleInfrastructureAutomation#ModuleInfrastructureAutomation' },
                    { loadChildren: 'InfrastructureModules/InfrastructureBatchService/ModuleInfrastructureBatchService#ModuleInfrastructureBatchService' },
                    { loadChildren: 'InfrastructureModules/InfrastructureCommunications/ModuleInfrastructureCommunications#ModuleInfrastructureCommunications' },
                    { loadChildren: 'InfrastructureModules/InfrastructureCustomization/ModuleInfrastructureCustomization#ModuleInfrastructureCustomization' },
                    { loadChildren: 'InfrastructureModules/InfrastructureDocuments/ModuleInfrastructureDocuments#ModuleInfrastructureDocuments' },
                    { loadChildren: 'InfrastructureModules/InfrastructureGettingStarted/ModuleInfrastructureGettingStarted#ModuleInfrastructureGettingStarted' },
                    { loadChildren: 'InfrastructureModules/InfrastructureHybrid/ModuleInfrastructureHybrid#ModuleInfrastructureHybrid' },
                    { loadChildren: 'InfrastructureModules/InfrastructureOthers/ModuleInfrastructureOthers#ModuleInfrastructureOthers' },
                    { loadChildren: 'InfrastructureModules/InfrastructureStimulsoft/ModuleInfrastructureStimulsoft#ModuleInfrastructureStimulsoft' },
                    { loadChildren: 'InfrastructureModules/InfrastructureTenantManagement/ModuleInfrastructureTenantManagement#ModuleInfrastructureTenantManagement' },
                    { loadChildren: 'InfrastructureModules/InfrastructureUser/ModuleInfrastructureUser#ModuleInfrastructureUser' },
                    { loadChildren: 'InfrastructureModules/InfrastructureComputingPartner/ModuleInfrastructureComputingPartner#ModuleInfrastructureComputingPartner' },
                    //Common Modules
                    { loadChildren: 'Common/Module_COMN#LogitudeCommonModule' },
                    { loadChildren: 'CommonModules/CommonAgent/ModuleCommonAgent#ModuleCommonAgent' },
                    { loadChildren: 'CommonModules/CommonAirline/ModuleCommonAirline#ModuleCommonAirline' },
                    { loadChildren: 'CommonModules/CommonCustomer/ModuleCommonCustomer#ModuleCommonCustomer' },
                    { loadChildren: 'CommonModules/CommonFilingInbox/ModuleCommonFilingInbox#ModuleCommonFilingInbox' },
                    { loadChildren: 'CommonModules/CommonFlightsSchedules/ModuleCommonFlightsSchedules#ModuleCommonFlightsSchedules' },
                    { loadChildren: 'CommonModules/CommonOthers/ModuleCommonOthers#ModuleCommonOthers' },
                    { loadChildren: 'CommonModules/CommonPartners/ModuleCommonPartners#ModuleCommonPartners' },
                    //{ loadChildren: 'Accounting/Module_ACCT#AccountingModule' },
                    //{ loadChildren: 'Booking/Module_BOOK#BookingModule' },
                    //{ loadChildren: 'CRM/Module_CRM#CRMModule' },
                    //{ loadChildren: 'Dashboard/Module_DASH#DashboardModule' },
                    //// Invoice Modules
                    //{ loadChildren: 'Invoice/Module_INVC#InvoiceModule' },
                    //{ loadChildren: 'InvoiceModules/APInvoice/ModuleAPInvoice#ModuleAPInvoice' },
                    //{ loadChildren: 'InvoiceModules/APPayment/ModuleAPPayment#ModuleAPPayment' },
                    //{ loadChildren: 'InvoiceModules/ARInvoice/ModuleARInvoice#ModuleARInvoice' },
                    //{ loadChildren: 'InvoiceModules/ARPayment/ModuleARPayment#ModuleARPayment' },
                    //{ loadChildren: 'InvoiceModules/Transfer/ModuleTransfer#ModuleTransfer' },
                    ////Quote Modules
                    //{ loadChildren: 'Quote/Module_QUOT#QuoteModule' },
                    //{ loadChildren: 'QuoteModules/QuoteCharges/ModuleQuoteCharges#ModuleQuoteCharges' },
                    //{ loadChildren: 'QuoteModules/QuoteOthers/ModuleQuoteOthers#ModuleQuoteOthers' },
                    //{ loadChildren: 'QuoteModules/QuoteTabs/ModuleQuoteTabs#ModuleQuoteTabs' },
                    //{ loadChildren: 'QuoteModules/QuoteTemplates/ModuleQuoteTemplates#ModuleQuoteTemplates' },
                    //{ loadChildren: 'Report/Module_REPO#ReportModule' },
                    //{ loadChildren: 'SharedLogistics/Module_SHRD#SharedLogisticsModule' },
                    ////Shipment Modules
                    //{ loadChildren: 'Shipment/Module_SHIP#Shipment_Module' },
                    //{ loadChildren: 'ShipmentModules/ShipmentAWB/ModuleShipmentAWB#ModuleShipmentAWB' },
                    //{ loadChildren: 'ShipmentModules/ShipmentINTTRA/ModuleShipmentINTTRA#ModuleShipmentINTTRA' },
                    //{ loadChildren: 'ShipmentModules/ShipmentOthers/ModuleShipmentOthers#ModuleShipmentOthers' },
                    //{ loadChildren: 'ShipmentModules/ShipmentStock/ModuleShipmentStock#ModuleShipmentStock' },
                    //{ loadChildren: 'ShipmentModules/ShipmentTabs/ModuleShipmentTabs#ModuleShipmentTabs' },
                    //{ loadChildren: 'ShipmentModules/ShipmentPackages/ModuleShipmentPackages#ModuleShipmentPackages' },
                    //{ loadChildren: 'ShipmentModules/ShipmentRouting/ModuleShipmentRouting#ModuleShipmentRouting' },
                    //{ loadChildren: 'ShipmentModules/ShipmentLogBox/ModuleShipmentLogBox#ModuleShipmentLogBox' },
                    //{ loadChildren: 'ShipmentModules/ShipmentSharedManifest/ModuleShipmentSharedManifest#ModuleShipmentSharedManifest' },
                    //{ loadChildren: 'Social/Module_SOCL#SocialModule' },
                    //{ loadChildren: 'TimeManagement/Module_TIME#TimeManagementModule' },
                    //{ loadChildren: 'Warehouse/Module_WARH#WarehouseModule' },
                    // Customs Module
                    { loadChildren: 'Customs/Module_CUST#CustomsModule' },
                    { loadChildren: 'CustomsModules/CustomsClaim/ModuleCustomsClaim#ModuleCustomsClaim' },
                    { loadChildren: 'CustomsModules/CustomsControls/ModuleCustomsControls#ModuleCustomsControls' },
                    { loadChildren: 'CustomsModules/CustomsClient/ModuleCustomsClient#ModuleCustomsClient' },
                    { loadChildren: 'CustomsModules/CustomsDeclarationModules/DeclarationTabs/ModuleDeclarationTabs#ModuleDeclarationTabs' },
                    { loadChildren: 'CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/ModuleDeclarationSupplierInvoice#ModuleDeclarationSupplierInvoice' },
                    { loadChildren: 'CustomsModules/CustomsDeclarationModules/DeclarationOthers/ModuleDeclarationOthers#ModuleDeclarationOthers' },
                    { loadChildren: 'CustomsModules/CustomsCourier/ModuleCustomsCourier#ModuleCustomsCourier' },
                    { loadChildren: 'CustomsModules/CustomsDeclarationCargoSplit/ModuleCustomsDeclarationCargoSplit#ModuleCustomsDeclarationCargoSplit' },
                    { loadChildren: 'CustomsModules/CustomsMaintenance/ModuleCustomsMaintenance#ModuleCustomsMaintenance' },
                    { loadChildren: 'CustomsModules/CustomsCollateral/ModuleCustomsCollateral#ModuleCustomsCollateral' },
                    { loadChildren: 'CustomsModules/CustomsPhysicalCheck/ModulePhysicalCheck#ModulePhysicalCheck' },
                    { loadChildren: 'CustomsModules/CustomsProceduralFault/ModuleProceduralFault#ModuleProceduralFault' },
                    { loadChildren: 'CustomsModules/CustomsVehicle/ModuleCustomsVehicle#ModuleCustomsVehicle' },
                    { loadChildren: 'CustomsModules/CustomsPaymentOrder/ModuleCustomsPaymentOrder#ModuleCustomsPaymentOrder' },
                    { loadChildren: 'CustomsModules/CustomsListTemplates/ModuleCustomsListTemplates#ModuleCustomsListTemplates' },
                    { loadChildren: 'CustomsModules/CustomsDocuments/ModuleCustomsDocuments#ModuleCustomsDocuments' },
                    { loadChildren: 'CustomsModules/CustomsRequests/ModuleCustomsRequests#ModuleCustomsRequests' },
                    { loadChildren: 'CustomsModules/CustomsGeneralRequests/ModuleCustomsGeneralRequests#ModuleCustomsGeneralRequests' },
                    { loadChildren: 'CustomsModules/CustomsVendor/ModuleCustomsVendor#ModuleCustomsVendor' },
                ])
            ],
            bootstrap: [AppComponent_Cust_1.AppComponent_Cust]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=Module_APP_CUST.js.map