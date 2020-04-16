import { NgModule, SystemJsNgModuleLoader } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { provideRoutes } from '@angular/router';
import { AppComponent } from './AppComponent';


@NgModule({
    imports: [BrowserModule, HttpClientModule],
    declarations: [AppComponent],

    providers: [
        SystemJsNgModuleLoader,

        provideRoutes([
            { loadChildren: () => import('../Controls/Module_CTRL').then(m => m.ControlsModule), path: 'Controls' },

          // Infrastructure
          { loadChildren: () => import('../Infrastructure/Module_INFR').then(m => m.InfrastructureModule), path: 'Infrastructure' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureAutomation/ModuleInfrastructureAutomation').then(m => m.ModuleInfrastructureAutomation), path: 'InfrastructureAutomation' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureBatchService/ModuleInfrastructureBatchService').then(m => m.ModuleInfrastructureBatchService), path: 'InfrastructureBatchService' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureCommunications/ModuleInfrastructureCommunications').then(m => m.ModuleInfrastructureCommunications), path: 'InfrastructureCommunications' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureCustomization/ModuleInfrastructureCustomization').then(m => m.ModuleInfrastructureCustomization), path: 'InfrastructureCustomization' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureDocuments/ModuleInfrastructureDocuments').then(m => m.ModuleInfrastructureDocuments), path: 'InfrastructureDocuments' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureGettingStarted/ModuleInfrastructureGettingStarted').then(m => m.ModuleInfrastructureGettingStarted), path: 'InfrastructureGettingStarted' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureHybrid/ModuleInfrastructureHybrid').then(m => m.ModuleInfrastructureHybrid), path: 'InfrastructureHybrid' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureOthers/ModuleInfrastructureOthers').then(m => m.ModuleInfrastructureOthers), path: 'InfrastructureOthers' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureStimulsoft/ModuleInfrastructureStimulsoft').then(m => m.ModuleInfrastructureStimulsoft), path: 'InfrastructureStimulsoft' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureTenantManagement/ModuleInfrastructureTenantManagement').then(m => m.ModuleInfrastructureTenantManagement), path: 'InfrastructureTenantManagement' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureUser/ModuleInfrastructureUser').then(m => m.ModuleInfrastructureUser), path: 'InfrastructureUser' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureComputingPartner/ModuleInfrastructureComputingPartner').then(m => m.ModuleInfrastructureComputingPartner), path: 'InfrastructureComputingPartner' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureBusinessProcess/ModuleInfrastructureBusinessProcess').then(m => m.ModuleInfrastructureBusinessProcess), path: 'InfrastructureBusinessProcess' },
          { loadChildren: () => import('../InfrastructureModules/InfrastructureBIReport/ModuleInfrastructureBIReport').then(m => m.ModuleInfrastructureBIReport), path: 'InfrastructureBIReport' },

          //Common Modules
          { loadChildren: () => import('../Common/Module_COMN').then(m => m.LogitudeCommonModule), path: 'Common' },
          { loadChildren: () => import('../CommonModules/CommonAgent/ModuleCommonAgent').then(m => m.ModuleCommonAgent), path: 'CommonAgent' },
          { loadChildren: () => import('../CommonModules/CommonAirline/ModuleCommonAirline').then(m => m.ModuleCommonAirline), path: 'CommonAirline' },
          { loadChildren: () => import('../CommonModules/CommonCustomer/ModuleCommonCustomer').then(m => m.ModuleCommonCustomer), path: 'CommonCustomer' },
          { loadChildren: () => import('../CommonModules/CommonFilingInbox/ModuleCommonFilingInbox').then(m => m.ModuleCommonFilingInbox), path: 'CommonFilingInbox' },
          { loadChildren: () => import('../CommonModules/CommonFlightsSchedules/ModuleCommonFlightsSchedules').then(m => m.ModuleCommonFlightsSchedules), path: 'CommonFlightsSchedules' },
          { loadChildren: () => import('../CommonModules/CommonOthers/ModuleCommonOthers').then(m => m.ModuleCommonOthers), path: 'CommonOthers' },
          { loadChildren: () => import('../CommonModules/CommonPartners/ModuleCommonPartners').then(m => m.ModuleCommonPartners), path: 'CommonPartners' },

          // Shipment Modules
          { loadChildren: () => import('../Shipment/Module_SHIP').then(m => m.Shipment_Module), path: 'Shipment' },
          { loadChildren: () => import('../ShipmentModules/ShipmentAWB/ModuleShipmentAWB').then(m => m.ModuleShipmentAWB), path: 'ShipmentAWB' },
          { loadChildren: () => import('../ShipmentModules/ShipmentINTTRA/ModuleShipmentINTTRA').then(m => m.ModuleShipmentINTTRA), path: 'ShipmentINTTRA' },
          { loadChildren: () => import('../ShipmentModules/ShipmentOthers/ModuleShipmentOthers').then(m => m.ModuleShipmentOthers), path: 'ShipmentOthers' },
          { loadChildren: () => import('../ShipmentModules/ShipmentStock/ModuleShipmentStock').then(m => m.ModuleShipmentStock), path: 'ShipmentStock' },
          { loadChildren: () => import('../ShipmentModules/ShipmentTabs/ModuleShipmentTabs').then(m => m.ModuleShipmentTabs), path: 'ShipmentTabs' },
          { loadChildren: () => import('../ShipmentModules/ShipmentPackages/ModuleShipmentPackages').then(m => m.ModuleShipmentPackages), path: 'ShipmentPackages' },
          { loadChildren: () => import('../ShipmentModules/ShipmentRouting/ModuleShipmentRouting').then(m => m.ModuleShipmentRouting), path: 'ShipmentRouting' },
          { loadChildren: () => import('../ShipmentModules/ShipmentLogBox/ModuleShipmentLogBox').then(m => m.ModuleShipmentLogBox), path: 'ShipmentLogBox' },
          { loadChildren: () => import('../ShipmentModules/ShipmentSharedManifest/ModuleShipmentSharedManifest').then(m => m.ModuleShipmentSharedManifest), path: 'ShipmentSharedManifest' },
          { loadChildren: () => import('../ShipmentModules/ShipmentAMANAC/ModuleShipmentAMANAC').then(m => m.ModuleShipmentAMANAC), path: 'ShipmentAMANAC' },


                      //Shipment Modules






            //{ loadChildren: 'Accounting/Module_ACCT#AccountingModule' },
            //{ loadChildren: 'Booking/Module_BOOK#BookingModule' },

            //CRM
            //{ loadChildren: 'CRM/Module_CRM#CRMModule' },
            //{ loadChildren: 'CRMModules/CRMActivity/ModuleCRMActivity#ModuleCRMActivity' },
            //{ loadChildren: 'CRMModules/CRMEmployeeGroup/ModuleCRMEmployeeGroup#ModuleCRMEmployeeGroup' },
            //{ loadChildren: 'CRMModules/CRMInboundEmail/ModuleCRMInboundEmail#ModuleCRMInboundEmail' },
            //{ loadChildren: 'CRMModules/CRMOpportunity/ModuleCRMOpportunity#ModuleCRMOpportunity' },
            //{ loadChildren: 'CRMModules/CRMOthers/ModuleCRMOthers#ModuleCRMOthers' },
            //{ loadChildren: 'CRMModules/CRMStages/ModuleCRMStages#ModuleCRMStages' },
            //{ loadChildren: 'CRMModules/CRMTickets/ModuleCRMTickets#ModuleCRMTickets' },
            //{ loadChildren: 'CRMModules/CRMOccasion/ModuleCRMOccasion#ModuleCRMOccasion' },
            //{ loadChildren: 'Dashboard/Module_DASH#DashboardModule' },

            // Invoice Modules
            //{ loadChildren: 'Invoice/Module_INVC#InvoiceModule' },
            //{ loadChildren: 'InvoiceModules/APInvoice/ModuleAPInvoice#ModuleAPInvoice' },
            //{ loadChildren: 'InvoiceModules/APPayment/ModuleAPPayment#ModuleAPPayment' },
            //{ loadChildren: 'InvoiceModules/ARInvoice/ModuleARInvoice#ModuleARInvoice' },
            //{ loadChildren: 'InvoiceModules/ARPayment/ModuleARPayment#ModuleARPayment' },
            //{ loadChildren: 'InvoiceModules/Transfer/ModuleTransfer#ModuleTransfer' },
            //{ loadChildren: 'InvoiceModules/InvoiceStocks/ModuleInvoiceStocks#ModuleInvoiceStocks' },

            //Quote Modules
            //{ loadChildren: 'Quote/Module_QUOT#QuoteModule' },
            //{ loadChildren: 'QuoteModules/QuoteCharges/ModuleQuoteCharges#ModuleQuoteCharges' },
            //{ loadChildren: 'QuoteModules/QuoteOthers/ModuleQuoteOthers#ModuleQuoteOthers' },
            //{ loadChildren: 'QuoteModules/QuoteTabs/ModuleQuoteTabs#ModuleQuoteTabs' },
            //{ loadChildren: 'QuoteModules/QuoteTemplates/ModuleQuoteTemplates#ModuleQuoteTemplates' },

            //{ loadChildren: 'Report/Module_REPO#ReportModule' },
            //{ loadChildren: 'SharedLogistics/Module_SHRD#SharedLogisticsModule' },



            //{ loadChildren: 'Social/Module_SOCL#SocialModule' },
            //{ loadChildren: 'TimeManagement/Module_TIME#TimeManagementModule' },
            //{ loadChildren: 'Warehouse/Module_WARH#WarehouseModule' },
            //{ loadChildren: 'TariffModule/Module_Tariff#Tariff_Module' },

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
