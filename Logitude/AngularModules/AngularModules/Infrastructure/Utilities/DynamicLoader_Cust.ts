import { ViewContainerRef, ComponentRef, Compiler, ModuleWithComponentFactories, ComponentFactoryResolver, SystemJsNgModuleLoader, NgModuleFactory, Injector, NgModuleRef } from '@angular/core';

export class DynamicLoader_Cust {
  public static Injector: Injector;
  public static Compiler: Compiler;
  public static ModuleLoader: SystemJsNgModuleLoader;
  public static Resolver: ComponentFactoryResolver
  public static InfrastructureModuleRef: NgModuleRef<any>;
  public static ModulesBank: ModuleProfile[] = [];
  public static Load(myComponentPath: string, location: ViewContainerRef): Promise<ComponentRef<any>> {
    if (myComponentPath != null) {

      var iPathParts: string[] = myComponentPath.split("/");

      if (iPathParts[1] == "null") {
        alert("Client module name is not defined");
      }

      else {
        var iModuleName: string = null;
        var iComponentName = iPathParts[iPathParts.length - 1];

        switch (iPathParts[1]) {
          case "InfrastructureModules":
          case "QuoteModules":
          case "ShipmentModules":
          case "Logitude_Modules":
          case "CommonModules":
          case "InvoiceModules":


            {

              iModuleName = iPathParts[1] + "/" + iPathParts[2];
              break;
            }

          case "CustomsModules":

            {
              if (iPathParts[2] == "CustomsDeclarationModules") {
                iModuleName = iPathParts[1] + "/" + iPathParts[3];
              }
              else {
                iModuleName = iPathParts[1] + "/" + iPathParts[2];
              }
              break;
            }

          default: {
            iModuleName = iPathParts[1];
            break;
          }
        }
      }

      //let myModuleName = stringParts[1] + "Module";
      //if (stringParts[1] == "Common") {
      //    myModuleName = "Logitude" + stringParts[1] + "Module";
      //}

      //let myComponentName: any = null;
      //if (myComponentPath != null) {
      //    var urlParts: string[] = myComponentPath.split("/");
      //    myComponentName = urlParts[urlParts.length - 1];
      //}

      return new Promise(resolve => {

        var Profile = this.ModulesBank.filter(f => f.ModuleName == iModuleName)[0];
        if (Profile) {
          let Component = Profile.Module.GetComponent(iComponentName);

          if (Component) {
            let compFactory = Profile.ModuleResolver.resolveComponentFactory(Component);
            let componentRef = location.createComponent(compFactory);
            resolve(componentRef);
          }

          else {
            alert(iComponentName + " is not declared in " + iModuleName);
          }
        }

        else {
          var moduleLoaderPath = this.GetModuleLoaderPath(iModuleName);

          this.ModuleLoader.load(moduleLoaderPath).then((moduleFactory: NgModuleFactory<any>) => {
            let Module = (<any>moduleFactory.moduleType);
            let Component = Module.GetComponent(iComponentName);

            if (Component) {
              const moduleRef = moduleFactory.create(this.Injector);
              const compFactory = moduleRef.componentFactoryResolver.resolveComponentFactory(Component);
              let componentRef = location.createComponent(compFactory);

              this.ModulesBank.push(new ModuleProfile(Module, iModuleName, moduleRef.componentFactoryResolver));
              resolve(componentRef);
            }

            else {
              alert(iComponentName + " is not declared in " + iModuleName);
            }
          });
        }
      });
    }
  }

  public static GetInstance(myPath: string) {
    if (myPath) {

      var stringParts: string[] = myPath.split("/");

      if (stringParts[1] == "null") {
        alert("Client module name is not defined");
      }

      else {

        let myModuleName = stringParts[1] + "Module";
        let instanceName = stringParts[stringParts.length - 1];

        if (stringParts[1] == "Common") {
          myModuleName = "Logitude" + myModuleName;
        }

        return new Promise((resolve, reject) => {

          var Profile = this.ModulesBank.filter(f => f.ModuleName == stringParts[1])[0];
          if (Profile) {
            let instance = Profile.Module.GetInstance(instanceName);

            if (instance) {
              resolve(instance);
            }

            else {
               alert(instanceName + " is not declared in " + myModuleName);
            }
          }

          else {
            var moduleLoaderPath = this.GetModuleLoaderPath(stringParts[1]);

            this.ModuleLoader.load(moduleLoaderPath).then((moduleFactory: NgModuleFactory<any>) => {

              let Module = (<any>moduleFactory.moduleType);
              let instance = Module.GetInstance(instanceName);

              if (instance) {
                resolve(instance);
              }

              else {
                 alert(instanceName + " is not declared in " + myModuleName);
              }
            });
          }
        });
      }
    }
  }

  private static GetModuleLoaderPath(moduleName: string) {
    var myResult: string;

    switch (moduleName) {

      case "Controls": { myResult = "Controls/Module_CTRL#ControlsModule"; break; }

      // Infrastructure
      case "Infrastructure": { myResult = "Infrastructure/Module_INFR#InfrastructureModule"; break; }
      case "InfrastructureModules/InfrastructureAutomation": { myResult = "InfrastructureModules/InfrastructureAutomation/ModuleInfrastructureAutomation#ModuleInfrastructureAutomation"; break; }
      case "InfrastructureModules/InfrastructureBatchService": { myResult = "InfrastructureModules/InfrastructureBatchService/ModuleInfrastructureBatchService#ModuleInfrastructureBatchService"; break; }
      case "InfrastructureModules/InfrastructureCommunications": { myResult = "InfrastructureModules/InfrastructureCommunications/ModuleInfrastructureCommunications#ModuleInfrastructureCommunications"; break; }
      case "InfrastructureModules/InfrastructureCustomization": { myResult = "InfrastructureModules/InfrastructureCustomization/ModuleInfrastructureCustomization#ModuleInfrastructureCustomization"; break; }
      case "InfrastructureModules/InfrastructureDocuments": { myResult = "InfrastructureModules/InfrastructureDocuments/ModuleInfrastructureDocuments#ModuleInfrastructureDocuments"; break; }
      case "InfrastructureModules/InfrastructureGettingStarted": { myResult = "InfrastructureModules/InfrastructureGettingStarted/ModuleInfrastructureGettingStarted#ModuleInfrastructureGettingStarted"; break; }
      case "InfrastructureModules/InfrastructureHybrid": { myResult = "InfrastructureModules/InfrastructureHybrid/ModuleInfrastructureHybrid#ModuleInfrastructureHybrid"; break; }
      case "InfrastructureModules/InfrastructureOthers": { myResult = "InfrastructureModules/InfrastructureOthers/ModuleInfrastructureOthers#ModuleInfrastructureOthers"; break; }
      case "InfrastructureModules/InfrastructureStimulsoft": { myResult = "InfrastructureModules/InfrastructureStimulsoft/ModuleInfrastructureStimulsoft#ModuleInfrastructureStimulsoft"; break; }
      case "InfrastructureModules/InfrastructureTenantManagement": { myResult = "InfrastructureModules/InfrastructureTenantManagement/ModuleInfrastructureTenantManagement#ModuleInfrastructureTenantManagement"; break; }
      case "InfrastructureModules/InfrastructureUser": { myResult = "InfrastructureModules/InfrastructureUser/ModuleInfrastructureUser#ModuleInfrastructureUser"; break; }
      case "InfrastructureModules/InfrastructureComputingPartner": { myResult = "InfrastructureModules/InfrastructureComputingPartner/ModuleInfrastructureComputingPartner#ModuleInfrastructureComputingPartner"; break; }

      //Common
      case "Common": { myResult = "Common/Module_COMN#LogitudeCommonModule"; break; }
      case "CommonModules/CommonAgent": { myResult = "CommonModules/CommonAgent/ModuleCommonAgent#ModuleCommonAgent"; break; }
      case "CommonModules/CommonAirline": { myResult = "CommonModules/CommonAirline/ModuleCommonAirline#ModuleCommonAirline"; break; }
      case "CommonModules/CommonCustomer": { myResult = "CommonModules/CommonCustomer/ModuleCommonCustomer#ModuleCommonCustomer"; break; }
      case "CommonModules/CommonFilingInbox": { myResult = "CommonModules/CommonFilingInbox/ModuleCommonFilingInbox#ModuleCommonFilingInbox"; break; }
      case "CommonModules/CommonFlightsSchedules": { myResult = "CommonModules/CommonFlightsSchedules/ModuleCommonFlightsSchedules#ModuleCommonFlightsSchedules"; break; }
      case "CommonModules/CommonOthers": { myResult = "CommonModules/CommonOthers/ModuleCommonOthers#ModuleCommonOthers"; break; }
      case "CommonModules/CommonPartners": { myResult = "CommonModules/CommonPartners/ModuleCommonPartners#ModuleCommonPartners"; break; }

      //case "Accounting": { myResult = "Accounting/Module_ACCT#AccountingModule"; break; }
      //case "Booking": { myResult = "Booking/Module_BOOK#BookingModule"; break; }
      //case "CRM": { myResult = "CRM/Module_CRM#CRMModule"; break; }


      //Customs
      case "Customs": { myResult = "Customs/Module_CUST#CustomsModule"; break; }// must comment if in main dev mode.
      case "CustomsModules/CustomsClaim": { myResult = "CustomsModules/CustomsClaim/ModuleCustomsClaim#ModuleCustomsClaim"; break; }
      case "CustomsModules/CustomsControls": { myResult = "CustomsModules/CustomsControls/ModuleCustomsControls#ModuleCustomsControls"; break; }
      case "CustomsModules/CustomControls": { myResult = "CustomsModules/CustomsControls/ModuleCustomsControls#ModuleCustomsControls"; break; }
      case "CustomsModules/CustomsClient": { myResult = "CustomsModules/CustomsClient/ModuleCustomsClient#ModuleCustomsClient"; break; }
      case "CustomsModules/CustomsCourier": { myResult = "CustomsModules/CustomsCourier/ModuleCustomsCourier#ModuleCustomsCourier"; break; }
      case "CustomsModules/CustomsMaintenance": { myResult = "CustomsModules/CustomsMaintenance/ModuleCustomsMaintenance#ModuleCustomsMaintenance"; break; }
      case "CustomsModules/DeclarationTabs": { myResult = "CustomsModules/CustomsDeclarationModules/DeclarationTabs/ModuleDeclarationTabs#ModuleDeclarationTabs"; break; }
      case "CustomsModules/DeclarationSupplierInvoice": { myResult = "CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/ModuleDeclarationSupplierInvoice#ModuleDeclarationSupplierInvoice"; break; }
      case "CustomsModules/DeclarationOthers": { myResult = "CustomsModules/CustomsDeclarationModules/DeclarationOthers/ModuleDeclarationOthers#ModuleDeclarationOthers"; break; }
      case "CustomsModules/CustomsCollateral": { myResult = "CustomsModules/CustomsCollateral/ModuleCustomsCollateral#ModuleCustomsCollateral"; break; }
      case "CustomsModules/CustomsVehicle": { myResult = "CustomsModules/CustomsVehicle/ModuleCustomsVehicle#ModuleCustomsVehicle"; break; }
      case "CustomsModules/CustomsPhysicalCheck": { myResult = "CustomsModules/CustomsPhysicalCheck/ModulePhysicalCheck#ModulePhysicalCheck"; break; }
      case "CustomsModules/CustomsProceduralFault": { myResult = "CustomsModules/CustomsProceduralFault/ModuleProceduralFault#ModuleProceduralFault"; break; }
      case "CustomsModules/CustomsDeclarationCargoSplit": { myResult = "CustomsModules/CustomsDeclarationCargoSplit/ModuleCustomsDeclarationCargoSplit#ModuleCustomsDeclarationCargoSplit"; break; }
      case "CustomsModules/CustomsListTemplates": { myResult = "CustomsModules/CustomsListTemplates/ModuleCustomsListTemplates#ModuleCustomsListTemplates"; break; }
      case "CustomsModules/CustomsDocuments": { myResult = "CustomsModules/CustomsDocuments/ModuleCustomsDocuments#ModuleCustomsDocuments"; break; }
      case "CustomsModules/CustomsPaymentOrder": { myResult = "CustomsModules/CustomsPaymentOrder/ModuleCustomsPaymentOrder#ModuleCustomsPaymentOrder"; break; }
      case "CustomsModules/CustomsRequests": { myResult = "CustomsModules/CustomsRequests/ModuleCustomsRequests#ModuleCustomsRequests"; break; }
      case "CustomsModules/CustomsGeneralRequests": { myResult = "CustomsModules/CustomsGeneralRequests/ModuleCustomsGeneralRequests#ModuleCustomsGeneralRequests"; break; }
      case "CustomsModules/CustomsVendor": { myResult = "CustomsModules/CustomsVendor/ModuleCustomsVendor#ModuleCustomsVendor"; break; }





      //case "Dashboard": { myResult = "Dashboard/Module_DASH#DashboardModule"; break; }

      ////Invoice
      //case "Invoice": { myResult = "Invoice/Module_INVC#InvoiceModule"; break; }
      //case "InvoiceModules/APInvoice": { myResult = "InvoiceModules/APInvoice/ModuleAPInvoice#ModuleAPInvoice"; break; }
      //case "InvoiceModules/APPayment": { myResult = "InvoiceModules/APPayment/ModuleAPPayment#ModuleAPPayment"; break; }
      //case "InvoiceModules/ARInvoice": { myResult = "InvoiceModules/ARInvoice/ModuleARInvoice#ModuleARInvoice"; break; }
      //case "InvoiceModules/ARPayment": { myResult = "InvoiceModules/ARPayment/ModuleARPayment#ModuleARPayment"; break; }
      //case "InvoiceModules/Transfer": { myResult = "InvoiceModules/Transfer/ModuleTransfer#ModuleTransfer"; break; }

      ////Quote
      //case "Quote": { myResult = "Quote/Module_QUOT#QuoteModule"; break; }
      //case "QuoteModules/QuoteCharges": { myResult = "QuoteModules/QuoteCharges/ModuleQuoteCharges#ModuleQuoteCharges"; break; }
      //case "QuoteModules/QuoteOthers": { myResult = "QuoteModules/QuoteOthers/ModuleQuoteOthers#ModuleQuoteOthers"; break; }
      //case "QuoteModules/QuoteTabs": { myResult = "QuoteModules/QuoteTabs/ModuleQuoteTabs#ModuleQuoteTabs"; break; }
      //case "QuoteModules/QuoteTemplates": { myResult = "QuoteModules/QuoteTemplates/ModuleQuoteTemplates#ModuleQuoteTemplates"; break; }

      //case "Report": { myResult = "Report/Module_REPO#ReportModule"; break; }
      //case "SharedLogistics": { myResult = "SharedLogistics/Module_SHRD#SharedLogisticsModule"; break; }

      //// Shipments 
      //case "Shipment": { myResult = "Shipment/Module_SHIP#Shipment_Module"; break; }
      //case "ShipmentModules/ShipmentAWB": { myResult = "ShipmentModules/ShipmentAWB/ModuleShipmentAWB#ModuleShipmentAWB"; break; }
      //case "ShipmentModules/ShipmentINTTRA": { myResult = "ShipmentModules/ShipmentINTTRA/ModuleShipmentINTTRA#ModuleShipmentINTTRA"; break; }
      //case "ShipmentModules/ShipmentOthers": { myResult = "ShipmentModules/ShipmentOthers/ModuleShipmentOthers#ModuleShipmentOthers"; break; }
      //case "ShipmentModules/ShipmentStock": { myResult = "ShipmentModules/ShipmentStock/ModuleShipmentStock#ModuleShipmentStock"; break; }
      //case "ShipmentModules/ShipmentTabs": { myResult = "ShipmentModules/ShipmentTabs/ModuleShipmentTabs#ModuleShipmentTabs"; break; }
      //case "ShipmentModules/ShipmentPackages": { myResult = "ShipmentModules/ShipmentPackages/ModuleShipmentPackages#ModuleShipmentPackages"; break; }
      //case "ShipmentModules/ShipmentRouting": { myResult = "ShipmentModules/ShipmentRouting/ModuleShipmentRouting#ModuleShipmentRouting"; break; }
      //case "ShipmentModules/ShipmentLogBox": { myResult = "ShipmentModules/ShipmentLogBox/ModuleShipmentLogBox#ModuleShipmentLogBox"; break; }
      //case "ShipmentModules/ShipmentSharedManifest": { myResult = "ShipmentModules/ShipmentSharedManifest/ModuleShipmentSharedManifest#ModuleShipmentSharedManifest"; break; }

      //case "Social": { myResult = "Social/Module_SOCL#SocialModule"; break; }
      //case "TimeManagement": { myResult = "TimeManagement/Module_TIME#TimeManagementModule"; break; }
      //case "Warehouse": { myResult = "Warehouse/Module_WARH#WarehouseModule"; break; }
    }

    return myResult;
  }
}

export class ModuleProfile {
  public Module: any;
  public ModuleName: string;
  public ModuleResolver: ComponentFactoryResolver;
  constructor(module: any, moduleName: string, moduleResolver: ComponentFactoryResolver) {
    this.Module = module;
    this.ModuleName = moduleName;
    this.ModuleResolver = moduleResolver;
  }
}
