import { NgModule, SystemJsNgModuleLoader } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { provideRoutes } from '@angular/router';
import { AppComponent } from './AppComponent';


@NgModule({
    imports: [BrowserModule, HttpModule],
    declarations: [AppComponent],

    providers: [
        SystemJsNgModuleLoader,

        provideRoutes([
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
            { loadChildren: 'ShipmentModules/ShipmentAMANAC/ModuleShipmentAMANAC#ModuleShipmentAMANAC' },

            { loadChildren: 'Social/Module_SOCL#SocialModule' },
            { loadChildren: 'TimeManagement/Module_TIME#TimeManagementModule' },
            { loadChildren: 'Warehouse/Module_WARH#WarehouseModule' },
            { loadChildren: 'TariffModule/Module_Tariff#Tariff_Module' },

            // Customs Module
            // { loadChildren: 'Customs/Module_CUST#CustomsModule' }, //this should be only on customs.
            // { loadChildren: 'CustomsModules/CustomsClaim/ModuleCustomsClaim#ModuleCustomsClaim' },
            // { loadChildren: 'CustomsModules/CustomsControls/ModuleCustomsControls#ModuleCustomsControls' },
            // { loadChildren: 'CustomsModules/CustomsClient/ModuleCustomsClient#ModuleCustomsClient' },
            // { loadChildren: 'CustomsModules/CustomsDeclarationModules/DeclarationTabs/ModuleDeclarationTabs#ModuleDeclarationTabs' },
            // { loadChildren: 'CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/ModuleDeclarationSupplierInvoice#ModuleDeclarationSupplierInvoice' },
            // { loadChildren: 'CustomsModules/CustomsDeclarationModules/DeclarationOthers/ModuleDeclarationOthers#ModuleDeclarationOthers' },
            // { loadChildren: 'CustomsModules/CustomsCourier/ModuleCustomsCourier#ModuleCustomsCourier' },
            // { loadChildren: 'CustomsModules/CustomsDeclarationCargoSplit/ModuleCustomsDeclarationCargoSplit#ModuleCustomsDeclarationCargoSplit' },
            // { loadChildren: 'CustomsModules/CustomsMaintenance/ModuleCustomsMaintenance#ModuleCustomsMaintenance' },
            // { loadChildren: 'CustomsModules/CustomsCollateral/ModuleCustomsCollateral#ModuleCustomsCollateral' },
            // { loadChildren: 'CustomsModules/CustomsPhysicalCheck/ModulePhysicalCheck#ModulePhysicalCheck' },
            // { loadChildren: 'CustomsModules/CustomsProceduralFault/ModuleProceduralFault#ModuleProceduralFault' },
            // { loadChildren: 'CustomsModules/CustomsVehicle/ModuleCustomsVehicle#ModuleCustomsVehicle' },
            // { loadChildren: 'CustomsModules/CustomsPaymentOrder/ModuleCustomsPaymentOrder#ModuleCustomsPaymentOrder' },
            // { loadChildren: 'CustomsModules/CustomsListTemplates/ModuleCustomsListTemplates#ModuleCustomsListTemplates' },
            // { loadChildren: 'CustomsModules/CustomsDocuments/ModuleCustomsDocuments#ModuleCustomsDocuments' },
            // { loadChildren: 'CustomsModules/CustomsRequests/ModuleCustomsRequests#ModuleCustomsRequests' },
            // { loadChildren: 'CustomsModules/CustomsGeneralRequests/ModuleCustomsGeneralRequests#ModuleCustomsGeneralRequests' },
            // { loadChildren: 'CustomsModules/CustomsVendor/ModuleCustomsVendor#ModuleCustomsVendor'},

        ])
    ],

    bootstrap: [AppComponent]
})

export class AppModule { }
