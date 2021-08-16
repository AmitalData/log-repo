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

  // Shipment Modules
  { loadChildren: () => import('../../Shipment/Module_SHIP').then(m => m.Shipment_Module), path: 'Shipment' },
  { loadChildren: () => import('../../ShipmentModules/ShipmentAWB/ModuleShipmentAWB').then(m => m.ModuleShipmentAWB), path: 'ShipmentAWB' },
  { loadChildren: () => import('../../ShipmentModules/ShipmentINTTRA/ModuleShipmentINTTRA').then(m => m.ModuleShipmentINTTRA), path: 'ShipmentINTTRA' },
  { loadChildren: () => import('../../ShipmentModules/ShipmentOthers/ModuleShipmentOthers').then(m => m.ModuleShipmentOthers), path: 'ShipmentOthers' },
  { loadChildren: () => import('../../ShipmentModules/ShipmentStock/ModuleShipmentStock').then(m => m.ModuleShipmentStock), path: 'ShipmentStock' },
  { loadChildren: () => import('../../ShipmentModules/ShipmentTabs/ModuleShipmentTabs').then(m => m.ModuleShipmentTabs), path: 'ShipmentTabs' },
  { loadChildren: () => import('../../ShipmentModules/ShipmentPackages/ModuleShipmentPackages').then(m => m.ModuleShipmentPackages), path: 'ShipmentPackages' },
  { loadChildren: () => import('../../ShipmentModules/ShipmentRouting/ModuleShipmentRouting').then(m => m.ModuleShipmentRouting), path: 'ShipmentRouting' },
  { loadChildren: () => import('../../ShipmentModules/ShipmentLogBox/ModuleShipmentLogBox').then(m => m.ModuleShipmentLogBox), path: 'ShipmentLogBox' },
  { loadChildren: () => import('../../ShipmentModules/ShipmentSharedManifest/ModuleShipmentSharedManifest').then(m => m.ModuleShipmentSharedManifest), path: 'ShipmentSharedManifest' },
  { loadChildren: () => import('../../ShipmentModules/ShipmentAMANAC/ModuleShipmentAMANAC').then(m => m.ModuleShipmentAMANAC), path: 'ShipmentAMANAC' },
  { loadChildren: () => import('../../ShipmentModules/ShipmentContainers/ModuleShipmentContainers').then(m => m.ModuleShipmentContainers), path: 'ShipmentContainers' },

    
  // Invoice Modules
  { loadChildren: () => import('../../Invoice/Module_INVC').then(m => m.InvoiceModule), path: 'Invoice' },
  { loadChildren: () => import('../../InvoiceModules/APInvoice/ModuleAPInvoice').then(m => m.ModuleAPInvoice), path: 'APInvoice' },
  { loadChildren: () => import('../../InvoiceModules/APPayment/ModuleAPPayment').then(m => m.ModuleAPPayment), path: 'APPayment' },
  { loadChildren: () => import('../../InvoiceModules/ARInvoice/ModuleARInvoice').then(m => m.ModuleARInvoice), path: 'ARInvoice' },
  { loadChildren: () => import('../../InvoiceModules/ARPayment/ModuleARPayment').then(m => m.ModuleARPayment), path: 'ARPayment' },
  { loadChildren: () => import('../../InvoiceModules/Transfer/ModuleTransfer').then(m => m.ModuleTransfer), path: 'Transfer' },
  { loadChildren: () => import('../../InvoiceModules/InvoiceStocks/ModuleInvoiceStocks').then(m => m.ModuleInvoiceStocks), path: 'InvoiceStocks' },

  // CRM
  { loadChildren: () => import('../../CRM/Module_CRM').then(m => m.CRMModule), path: 'CRM' },
  { loadChildren: () => import('../../CRMModules/CRMActivity/ModuleCRMActivity').then(m => m.ModuleCRMActivity), path: 'CRMActivity' },
  { loadChildren: () => import('../../CRMModules/CRMEmployeeGroup/ModuleCRMEmployeeGroup').then(m => m.ModuleCRMEmployeeGroup), path: 'CRMEmployeeGroup' },
  { loadChildren: () => import('../../CRMModules/CRMInboundEmail/ModuleCRMInboundEmail').then(m => m.ModuleCRMInboundEmail), path: 'CRMInboundEmail' },
  { loadChildren: () => import('../../CRMModules/CRMOpportunity/ModuleCRMOpportunity').then(m => m.ModuleCRMOpportunity), path: 'CRMOpportunity' },
  { loadChildren: () => import('../../CRMModules/CRMOthers/ModuleCRMOthers').then(m => m.ModuleCRMOthers), path: 'CRMOthers' },
  { loadChildren: () => import('../../CRMModules/CRMStages/ModuleCRMStages').then(m => m.ModuleCRMStages), path: 'CRMStages' },
  { loadChildren: () => import('../../CRMModules/CRMTickets/ModuleCRMTickets').then(m => m.ModuleCRMTickets), path: 'CRMTickets' },
  { loadChildren: () => import('../../CRMModules/CRMOccasion/ModuleCRMOccasion').then(m => m.ModuleCRMOccasion), path: 'CRMOccasion' },

  // Quote Modules
  { loadChildren: () => import('../../Quote/Module_QUOT').then(m => m.QuoteModule), path: 'Quote' },
  { loadChildren: () => import('../../QuoteModules/QuoteCharges/ModuleQuoteCharges').then(m => m.ModuleQuoteCharges), path: 'QuoteCharges' },
  { loadChildren: () => import('../../QuoteModules/QuoteOthers/ModuleQuoteOthers').then(m => m.ModuleQuoteOthers), path: 'QuoteOthers' },
  { loadChildren: () => import('../../QuoteModules/QuoteTabs/ModuleQuoteTabs').then(m => m.ModuleQuoteTabs), path: 'QuoteTabs' },
  { loadChildren: () => import('../../QuoteModules/QuoteTemplates/ModuleQuoteTemplates').then(m => m.ModuleQuoteTemplates), path: 'QuoteTemplates' },

  { loadChildren: () => import('../../Accounting/Module_ACCT').then(m => m.AccountingModule), path: 'Accounting' },
  { loadChildren: () => import('../../Booking/Module_BOOK').then(m => m.BookingModule), path: 'Booking' },
  { loadChildren: () => import('../../Dashboard/Module_DASH').then(m => m.DashboardModule), path: 'Dashboard' },
  { loadChildren: () => import('../../Report/Module_REPO').then(m => m.ReportModule), path: 'Report' },
  { loadChildren: () => import('../../SharedLogistics/Module_SHRD').then(m => m.SharedLogisticsModule), path: 'SharedLogistics' },
  { loadChildren: () => import('../../Social/Module_SOCL').then(m => m.SocialModule), path: 'Social' },
  { loadChildren: () => import('../../TariffModule/Module_Tariff').then(m => m.Tariff_Module), path: 'TariffModule' },
  { loadChildren: () => import('../../TimeManagement/Module_TIME').then(m => m.TimeManagementModule), path: 'TimeManagement' },
  { loadChildren: () => import('../../Warehouse/Module_WARH').then(m => m.WarehouseModule), path: 'Warehouse' },


  { loadChildren: () => import('../../TasksApp/ModuleTasksApp').then(m => m.ModuleTasksApp), path: 'TasksApp' },

  //shipment order
  { loadChildren: () => import('../../ShipmentOrder/Module_SHIPOR').then(m => m.ShipmentOrderModule), path: 'ShipmentOrder' },

];

export function LazyArrayToObjects() {
  const result = {};

  for (const w of LazyWidgets) {
    result[w.path] = w.loadChildren;
  }

  return result;
}
