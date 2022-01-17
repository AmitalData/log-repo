import { NgModuleFactory, Type } from '@angular/core';

export const LazyWidgets: { path: string, loadChildren: () => Promise<NgModuleFactory<any> | Type<any>> }[] = [
  { loadChildren: () => import('../../Controls/Module_CTRL').then(m => m.ControlsModule), path: 'Controls' },

   // Infrastructure
   { loadChildren: () => import('../../Infrastructure/Module_INFR').then(m => m.InfrastructureModule), path: 'Infrastructure' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureAutomation/ModuleInfrastructureAutomation').then(m => m.ModuleInfrastructureAutomation), path: 'InfrastructureAutomation' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureBatchService/ModuleInfrastructureBatchService').then(m => m.ModuleInfrastructureBatchService), path: 'InfrastructureBatchService' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureCommunications/ModuleInfrastructureCommunications').then(m => m.ModuleInfrastructureCommunications), path: 'InfrastructureCommunications' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureCustomization/ModuleInfrastructureCustomization').then(m => m.ModuleInfrastructureCustomization), path: 'InfrastructureCustomization' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureDocuments/ModuleInfrastructureDocuments').then(m => m.ModuleInfrastructureDocuments), path: 'InfrastructureDocuments' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureGettingStarted/ModuleInfrastructureGettingStarted').then(m => m.ModuleInfrastructureGettingStarted), path: 'InfrastructureGettingStarted' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureHybrid/ModuleInfrastructureHybrid').then(m => m.ModuleInfrastructureHybrid), path: 'InfrastructureHybrid' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureOthers/ModuleInfrastructureOthers').then(m => m.ModuleInfrastructureOthers), path: 'InfrastructureOthers' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureStimulsoft/ModuleInfrastructureStimulsoft').then(m => m.ModuleInfrastructureStimulsoft), path: 'InfrastructureStimulsoft' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureTenantManagement/ModuleInfrastructureTenantManagement').then(m => m.ModuleInfrastructureTenantManagement), path: 'InfrastructureTenantManagement' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureUser/ModuleInfrastructureUser').then(m => m.ModuleInfrastructureUser), path: 'InfrastructureUser' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureComputingPartner/ModuleInfrastructureComputingPartner').then(m => m.ModuleInfrastructureComputingPartner), path: 'InfrastructureComputingPartner' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureBusinessProcess/ModuleInfrastructureBusinessProcess').then(m => m.ModuleInfrastructureBusinessProcess), path: 'InfrastructureBusinessProcess' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureBIReport/ModuleInfrastructureBIReport').then(m => m.ModuleInfrastructureBIReport), path: 'InfrastructureBIReport' },
   { loadChildren: () => import('../../InfrastructureModules/InfrastructureHelpResource/ModuleInfrastructureHelpResource').then(m => m.ModuleInfrastructureHelpResource), path: 'InfrastructureHelpResource' },
 
   // Common Modules
   { loadChildren: () => import('../../Common/Module_COMN').then(m => m.LogitudeCommonModule), path: 'Common' },
   { loadChildren: () => import('../../CommonModules/CommonAgent/ModuleCommonAgent').then(m => m.ModuleCommonAgent), path: 'CommonAgent' },
   { loadChildren: () => import('../../CommonModules/CommonAirline/ModuleCommonAirline').then(m => m.ModuleCommonAirline), path: 'CommonAirline' },
   { loadChildren: () => import('../../CommonModules/CommonCustomer/ModuleCommonCustomer').then(m => m.ModuleCommonCustomer), path: 'CommonCustomer' },
   { loadChildren: () => import('../../CommonModules/CommonFilingInbox/ModuleCommonFilingInbox').then(m => m.ModuleCommonFilingInbox), path: 'CommonFilingInbox' },
   { loadChildren: () => import('../../CommonModules/CommonFlightsSchedules/ModuleCommonFlightsSchedules').then(m => m.ModuleCommonFlightsSchedules), path: 'CommonFlightsSchedules' },
   { loadChildren: () => import('../../CommonModules/CommonOthers/ModuleCommonOthers').then(m => m.ModuleCommonOthers), path: 'CommonOthers' },
   { loadChildren: () => import('../../CommonModules/CommonPartners/ModuleCommonPartners').then(m => m.ModuleCommonPartners), path: 'CommonPartners' },
 
  // Customs Modules
  { loadChildren: () => import('../../Customs/Module_CUST').then(m => m.CustomsModule), path: 'Customs' },
  { loadChildren: () => import('../../CustomsModules/CustomsClaim/ModuleCustomsClaim').then(m => m.ModuleCustomsClaim), path: 'CustomsClaim' },
  { loadChildren: () => import('../../CustomsModules/CustomsControls/ModuleCustomsControls').then(m => m.ModuleCustomsControls), path: 'CustomsControls' },
  { loadChildren: () => import('../../CustomsModules/CustomsClient/ModuleCustomsClient').then(m => m.ModuleCustomsClient), path: 'CustomsClient' },
  { loadChildren: () => import('../../CustomsModules/CustomsDeclarationModules/DeclarationTabs/ModuleDeclarationTabs').then(m => m.ModuleDeclarationTabs), path: 'DeclarationTabs' },
  { loadChildren: () => import('../../CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/ModuleDeclarationSupplierInvoice').then(m => m.ModuleDeclarationSupplierInvoice), path: 'DeclarationSupplierInvoice' },
  { loadChildren: () => import('../../CustomsModules/CustomsDeclarationModules/DeclarationOthers/ModuleDeclarationOthers').then(m => m.ModuleDeclarationOthers), path: 'DeclarationOthers' },
  { loadChildren: () => import('../../CustomsModules/CustomsCourier/ModuleCustomsCourier').then(m => m.ModuleCustomsCourier), path: 'CustomsCourier' },
  { loadChildren: () => import('../../CustomsModules/CustomsDeclarationCargoSplit/ModuleCustomsDeclarationCargoSplit').then(m => m.ModuleCustomsDeclarationCargoSplit), path: 'CustomsDeclarationCargoSplit' },
  { loadChildren: () => import('../../CustomsModules/CustomsMaintenance/ModuleCustomsMaintenance').then(m => m.ModuleCustomsMaintenance), path: 'CustomsMaintenance' },
  { loadChildren: () => import('../../CustomsModules/CustomsReferant/ModuleCustomsReferant').then(m => m.ModuleCustomsReferant), path: 'CustomsReferant' },
  { loadChildren: () => import('../../CustomsModules/CustomsCollateral/ModuleCustomsCollateral').then(m => m.ModuleCustomsCollateral), path: 'CustomsCollateral' },
  { loadChildren: () => import('../../CustomsModules/CustomsPhysicalCheck/ModulePhysicalCheck').then(m => m.ModulePhysicalCheck), path: 'PhysicalCheck' },
    { loadChildren: () => import('../../CustomsModules/CustomsPhysicalCheck/ModulePhysicalCheck').then(m => m.ModulePhysicalCheck), path: 'CustomsPhysicalCheck' },
    { loadChildren: () => import('../../CustomsModules/CustomsProceduralFault/ModuleProceduralFault').then(m => m.ModuleProceduralFault), path: 'CustomsProceduralFault' },
  { loadChildren: () => import('../../CustomsModules/CustomsVehicle/ModuleCustomsVehicle').then(m => m.ModuleCustomsVehicle), path: 'CustomsVehicle' },
  { loadChildren: () => import('../../CustomsModules/CustomsPaymentOrder/ModuleCustomsPaymentOrder').then(m => m.ModuleCustomsPaymentOrder), path: 'CustomsPaymentOrder' },
  { loadChildren: () => import('../../CustomsModules/CustomsListTemplates/ModuleCustomsListTemplates').then(m => m.ModuleCustomsListTemplates), path: 'CustomsListTemplates' },
  { loadChildren: () => import('../../CustomsModules/CustomsDocuments/ModuleCustomsDocuments').then(m => m.ModuleCustomsDocuments), path: 'CustomsDocuments' },
  { loadChildren: () => import('../../CustomsModules/CustomsRequests/ModuleCustomsRequests').then(m => m.ModuleCustomsRequests), path: 'CustomsRequests' },
  { loadChildren: () => import('../../CustomsModules/CustomsGeneralRequests/ModuleCustomsGeneralRequests').then(m => m.ModuleCustomsGeneralRequests), path: 'CustomsGeneralRequests' },
  { loadChildren: () => import('../../CustomsModules/CustomsVendor/ModuleCustomsVendor').then(m => m.ModuleCustomsVendor), path: 'CustomsVendor' },
    { loadChildren: () => import('../../CustomsModules/InvoiceQueue/ModuleInvoiceQueue').then(m => m.ModuleInvoiceQueue), path: 'InvoiceQueue' },
    { loadChildren: () => import('../../CustomsModules/CustomsExportStorage/export-storage.module').then(m => m.ExportStorageModule), path: 'CustomsExportStorage' },
    
    { loadChildren: () => import('../../CustomsModules/CustomsContainerization/ModuleCustomsContainerization').then(m => m.ModuleCustomsContainerization), path: 'CustomsContainerization' },
    { loadChildren: () => import('../../CustomsModules/CustomsReport/ModuleCustomsReports').then(m => m.ModuleCustomsReports), path: 'CustomsReport' },



    // QuoteOPM Modules
    { loadChildren: () => import('../../QuoteOPM/Module_QUPM').then(m => m.QuoteModule), path: 'QuoteOPM' },
    { loadChildren: () => import('../../QuoteOPModules/QuoteCharges/ModuleQuoteCharges').then(m => m.ModuleQuoteCharges), path: 'QuoteCharges' },
    { loadChildren: () => import('../../QuoteOPModules/QuoteOthers/ModuleQuoteOthers').then(m => m.ModuleQuoteOthers), path: 'QuoteOthers' },
    { loadChildren: () => import('../../QuoteOPModules/QuoteTabs/ModuleQuoteTabs').then(m => m.ModuleQuoteTabs), path: 'QuoteTabs' },
    { loadChildren: () => import('../../QuoteOPModules/QuoteTemplates/ModuleQuoteTemplates').then(m => m.ModuleQuoteOPTemplates), path: 'QuoteTemplates' },

];

export function LazyArrayToObjects() {
  const result = {};

  for (const w of LazyWidgets) {
    result[w.path] = w.loadChildren;
  }

  return result;
}
