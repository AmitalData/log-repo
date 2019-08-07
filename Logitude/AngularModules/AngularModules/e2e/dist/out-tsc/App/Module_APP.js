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
var AppComponent_1 = require("./AppComponent");
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            imports: [platform_browser_1.BrowserModule, http_1.HttpModule],
            declarations: [AppComponent_1.AppComponent],
            providers: [
                core_1.SystemJsNgModuleLoader,
                router_1.provideRoutes([
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
                    { loadChildren: 'InfrastructureModules/InfrastructureBusinessProcess/ModuleInfrastructureBusinessProcess#ModuleInfrastructureBusinessProcess' },
                    { loadChildren: 'InfrastructureModules/InfrastructureBIReport/ModuleInfrastructureBIReport#ModuleInfrastructureBIReport' },
                    //Common Modules
                    { loadChildren: 'Common/Module_COMN#LogitudeCommonModule' },
                    { loadChildren: 'CommonModules/CommonAgent/ModuleCommonAgent#ModuleCommonAgent' },
                    { loadChildren: 'CommonModules/CommonAirline/ModuleCommonAirline#ModuleCommonAirline' },
                    { loadChildren: 'CommonModules/CommonCustomer/ModuleCommonCustomer#ModuleCommonCustomer' },
                    { loadChildren: 'CommonModules/CommonFilingInbox/ModuleCommonFilingInbox#ModuleCommonFilingInbox' },
                    { loadChildren: 'CommonModules/CommonFlightsSchedules/ModuleCommonFlightsSchedules#ModuleCommonFlightsSchedules' },
                    { loadChildren: 'CommonModules/CommonOthers/ModuleCommonOthers#ModuleCommonOthers' },
                    { loadChildren: 'CommonModules/CommonPartners/ModuleCommonPartners#ModuleCommonPartners' },
                    { loadChildren: 'Accounting/Module_ACCT#AccountingModule' },
                    { loadChildren: 'Booking/Module_BOOK#BookingModule' },
                    //CRM
                    { loadChildren: 'CRM/Module_CRM#CRMModule' },
                    { loadChildren: 'CRMModules/CRMActivity/ModuleCRMActivity#ModuleCRMActivity' },
                    { loadChildren: 'CRMModules/CRMEmployeeGroup/ModuleCRMEmployeeGroup#ModuleCRMEmployeeGroup' },
                    { loadChildren: 'CRMModules/CRMInboundEmail/ModuleCRMInboundEmail#ModuleCRMInboundEmail' },
                    { loadChildren: 'CRMModules/CRMOpportunity/ModuleCRMOpportunity#ModuleCRMOpportunity' },
                    { loadChildren: 'CRMModules/CRMOthers/ModuleCRMOthers#ModuleCRMOthers' },
                    { loadChildren: 'CRMModules/CRMStages/ModuleCRMStages#ModuleCRMStages' },
                    { loadChildren: 'CRMModules/CRMTickets/ModuleCRMTickets#ModuleCRMTickets' },
                    { loadChildren: 'CRMModules/CRMOccasion/ModuleCRMOccasion#ModuleCRMOccasion' },
                    { loadChildren: 'Dashboard/Module_DASH#DashboardModule' },
                    // Invoice Modules
                    { loadChildren: 'Invoice/Module_INVC#InvoiceModule' },
                    { loadChildren: 'InvoiceModules/APInvoice/ModuleAPInvoice#ModuleAPInvoice' },
                    { loadChildren: 'InvoiceModules/APPayment/ModuleAPPayment#ModuleAPPayment' },
                    { loadChildren: 'InvoiceModules/ARInvoice/ModuleARInvoice#ModuleARInvoice' },
                    { loadChildren: 'InvoiceModules/ARPayment/ModuleARPayment#ModuleARPayment' },
                    { loadChildren: 'InvoiceModules/Transfer/ModuleTransfer#ModuleTransfer' },
                    { loadChildren: 'InvoiceModules/InvoiceStocks/ModuleInvoiceStocks#ModuleInvoiceStocks' },
                    //Quote Modules
                    { loadChildren: 'Quote/Module_QUOT#QuoteModule' },
                    { loadChildren: 'QuoteModules/QuoteCharges/ModuleQuoteCharges#ModuleQuoteCharges' },
                    { loadChildren: 'QuoteModules/QuoteOthers/ModuleQuoteOthers#ModuleQuoteOthers' },
                    { loadChildren: 'QuoteModules/QuoteTabs/ModuleQuoteTabs#ModuleQuoteTabs' },
                    { loadChildren: 'QuoteModules/QuoteTemplates/ModuleQuoteTemplates#ModuleQuoteTemplates' },
                    { loadChildren: 'Report/Module_REPO#ReportModule' },
                    { loadChildren: 'SharedLogistics/Module_SHRD#SharedLogisticsModule' },
                    //Shipment Modules
                    { loadChildren: 'Shipment/Module_SHIP#Shipment_Module' },
                    { loadChildren: 'ShipmentModules/ShipmentAWB/ModuleShipmentAWB#ModuleShipmentAWB' },
                    { loadChildren: 'ShipmentModules/ShipmentINTTRA/ModuleShipmentINTTRA#ModuleShipmentINTTRA' },
                    { loadChildren: 'ShipmentModules/ShipmentOthers/ModuleShipmentOthers#ModuleShipmentOthers' },
                    { loadChildren: 'ShipmentModules/ShipmentStock/ModuleShipmentStock#ModuleShipmentStock' },
                    { loadChildren: 'ShipmentModules/ShipmentTabs/ModuleShipmentTabs#ModuleShipmentTabs' },
                    { loadChildren: 'ShipmentModules/ShipmentPackages/ModuleShipmentPackages#ModuleShipmentPackages' },
                    { loadChildren: 'ShipmentModules/ShipmentRouting/ModuleShipmentRouting#ModuleShipmentRouting' },
                    { loadChildren: 'ShipmentModules/ShipmentLogBox/ModuleShipmentLogBox#ModuleShipmentLogBox' },
                    { loadChildren: 'ShipmentModules/ShipmentSharedManifest/ModuleShipmentSharedManifest#ModuleShipmentSharedManifest' },
                    { loadChildren: 'Social/Module_SOCL#SocialModule' },
                    { loadChildren: 'TimeManagement/Module_TIME#TimeManagementModule' },
                    { loadChildren: 'Warehouse/Module_WARH#WarehouseModule' },
                    { loadChildren: 'TariffModule/Module_Tariff#Tariff_Module' },
                ])
            ],
            bootstrap: [AppComponent_1.AppComponent]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=Module_APP.js.map