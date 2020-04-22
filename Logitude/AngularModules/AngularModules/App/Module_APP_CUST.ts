import { NgModule, SystemJsNgModuleLoader } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { provideRoutes } from '@angular/router';
import { AppComponent_Cust } from './AppComponent_Cust';

@NgModule({
    imports: [BrowserModule, HttpClientModule],
    declarations: [AppComponent_Cust],

    providers: [
        SystemJsNgModuleLoader,

      provideRoutes([

        // controls
        //{ loadChildren: 'Controls/Module_CTRL#ControlsModule' },

        // Infrastructure
        //{ loadChildren: 'Infrastructure/Module_INFR#InfrastructureModule' },
        //{ loadChildren: 'InfrastructureModules/InfrastructureAutomation/ModuleInfrastructureAutomation#ModuleInfrastructureAutomation' },
        //{ loadChildren: 'InfrastructureModules/InfrastructureBatchService/ModuleInfrastructureBatchService#ModuleInfrastructureBatchService' },
        //{ loadChildren: 'InfrastructureModules/InfrastructureCommunications/ModuleInfrastructureCommunications#ModuleInfrastructureCommunications' },
        //{ loadChildren: 'InfrastructureModules/InfrastructureCustomization/ModuleInfrastructureCustomization#ModuleInfrastructureCustomization' },
        //{ loadChildren: 'InfrastructureModules/InfrastructureDocuments/ModuleInfrastructureDocuments#ModuleInfrastructureDocuments' },
        //{ loadChildren: 'InfrastructureModules/InfrastructureGettingStarted/ModuleInfrastructureGettingStarted#ModuleInfrastructureGettingStarted' },
        //{ loadChildren: 'InfrastructureModules/InfrastructureHybrid/ModuleInfrastructureHybrid#ModuleInfrastructureHybrid' },
        //{ loadChildren: 'InfrastructureModules/InfrastructureOthers/ModuleInfrastructureOthers#ModuleInfrastructureOthers' },
        //{ loadChildren: 'InfrastructureModules/InfrastructureStimulsoft/ModuleInfrastructureStimulsoft#ModuleInfrastructureStimulsoft' },
        //{ loadChildren: 'InfrastructureModules/InfrastructureTenantManagement/ModuleInfrastructureTenantManagement#ModuleInfrastructureTenantManagement' },
        //{ loadChildren: 'InfrastructureModules/InfrastructureUser/ModuleInfrastructureUser#ModuleInfrastructureUser' },
        //{ loadChildren: 'InfrastructureModules/InfrastructureComputingPartner/ModuleInfrastructureComputingPartner#ModuleInfrastructureComputingPartner' },

        //Common Modules
        //{ loadChildren: 'Common/Module_COMN#LogitudeCommonModule' },
        //{ loadChildren: 'CommonModules/CommonAgent/ModuleCommonAgent#ModuleCommonAgent' },
        //{ loadChildren: 'CommonModules/CommonAirline/ModuleCommonAirline#ModuleCommonAirline' },
        //{ loadChildren: 'CommonModules/CommonCustomer/ModuleCommonCustomer#ModuleCommonCustomer' },
        //{ loadChildren: 'CommonModules/CommonFilingInbox/ModuleCommonFilingInbox#ModuleCommonFilingInbox' },
        //{ loadChildren: 'CommonModules/CommonFlightsSchedules/ModuleCommonFlightsSchedules#ModuleCommonFlightsSchedules' },
        //{ loadChildren: 'CommonModules/CommonOthers/ModuleCommonOthers#ModuleCommonOthers' },
        //{ loadChildren: 'CommonModules/CommonPartners/ModuleCommonPartners#ModuleCommonPartners' },

        // Customs Module
        //{ loadChildren: 'Customs/Module_CUST#CustomsModule' },
        //{ loadChildren: 'CustomsModules/CustomsClaim/ModuleCustomsClaim#ModuleCustomsClaim' },
        //{ loadChildren: 'CustomsModules/CustomsControls/ModuleCustomsControls#ModuleCustomsControls' },
        //{ loadChildren: 'CustomsModules/CustomsClient/ModuleCustomsClient#ModuleCustomsClient' },
        //{ loadChildren: 'CustomsModules/CustomsDeclarationModules/DeclarationTabs/ModuleDeclarationTabs#ModuleDeclarationTabs' },
        //{ loadChildren: 'CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/ModuleDeclarationSupplierInvoice#ModuleDeclarationSupplierInvoice' },
        //{ loadChildren: 'CustomsModules/CustomsDeclarationModules/DeclarationOthers/ModuleDeclarationOthers#ModuleDeclarationOthers' },
        //{ loadChildren: 'CustomsModules/CustomsCourier/ModuleCustomsCourier#ModuleCustomsCourier' },
        //{ loadChildren: 'CustomsModules/CustomsDeclarationCargoSplit/ModuleCustomsDeclarationCargoSplit#ModuleCustomsDeclarationCargoSplit' },
        //{ loadChildren: 'CustomsModules/CustomsMaintenance/ModuleCustomsMaintenance#ModuleCustomsMaintenance' },
        //{ loadChildren: 'CustomsModules/CustomsCollateral/ModuleCustomsCollateral#ModuleCustomsCollateral' },
        //{ loadChildren: 'CustomsModules/CustomsPhysicalCheck/ModulePhysicalCheck#ModulePhysicalCheck' },
        //{ loadChildren: 'CustomsModules/CustomsProceduralFault/ModuleProceduralFault#ModuleProceduralFault' },
        //{ loadChildren: 'CustomsModules/CustomsVehicle/ModuleCustomsVehicle#ModuleCustomsVehicle' },
        //{ loadChildren: 'CustomsModules/CustomsPaymentOrder/ModuleCustomsPaymentOrder#ModuleCustomsPaymentOrder' },
        //{ loadChildren: 'CustomsModules/CustomsListTemplates/ModuleCustomsListTemplates#ModuleCustomsListTemplates' },
        //{ loadChildren: 'CustomsModules/CustomsDocuments/ModuleCustomsDocuments#ModuleCustomsDocuments' },
        //{ loadChildren: 'CustomsModules/CustomsRequests/ModuleCustomsRequests#ModuleCustomsRequests' },
        //{ loadChildren: 'CustomsModules/CustomsGeneralRequests/ModuleCustomsGeneralRequests#ModuleCustomsGeneralRequests' },
        //{ loadChildren: 'CustomsModules/CustomsVendor/ModuleCustomsVendor#ModuleCustomsVendor' },
        ])
    ],

    bootstrap: [AppComponent_Cust]
})

export class AppModule { }
