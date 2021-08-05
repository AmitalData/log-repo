import { ViewContainerRef, ComponentRef, Injector, Compiler, NgModuleFactory, Type, ComponentFactoryResolver } from '@angular/core';

export class DynamicLoader {
  public static Injector: Injector;
  public static Compiler: Compiler;
  public static LazyWidgets: { [key: string]: () => Promise<NgModuleFactory<any> | Type<any>> };
  public static ModulesBank: ModuleProfile[] = [];
  public static Load(myPath: string, location: ViewContainerRef): Promise<ComponentRef<any>> {
    if (!myPath || !location) {
      alert("Invalid DynamicLoader Load arguments");
    }

    var iPathParts: string[] = myPath.split("/");

    if (iPathParts[1] == "null") {
      alert("Client module name is not defined");
    }

    else {

      var iModuleName: string = this.GetWidgetModuleName(myPath);
      var iComponentName = iPathParts[iPathParts.length - 1];

      return new Promise((resolve, reject) => {

        this.GetModuleProfile(iModuleName).then((Profile: ModuleProfile) => {
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
        });
      });
    }
    }
    public static GetInstance(myPath: string , dontShowErrorAlert: boolean = false) {
    if (!myPath) {
      alert("Invalid DynamicLoader GetInstance arguments");
    }

    var iPathParts: string[] = myPath.split("/");

    if (iPathParts[1] == "null") {
      alert("Client module name is not defined");
    }

    else {
      var iModuleName: string = this.GetWidgetModuleName(myPath);
      var instanceName = iPathParts[iPathParts.length - 1];

      return new Promise((resolve, reject) => {

        this.GetModuleProfile(iModuleName).then((Profile: ModuleProfile) => {
          if (Profile) {
            let instance = Profile.Module.GetInstance(instanceName);
              resolve(instance);

            if (instance) {
              resolve(instance);
            }

            else {
                if (dontShowErrorAlert) resolve(null);
                else alert(instanceName + " is not declared in " + iModuleName);
            }
          }
        });
      });
    }
  }

  private static GetWidgetModuleName(myComponentPath: string) {
    var output: string;

    var iPathParts: string[] = myComponentPath.split("/");

    switch (iPathParts[1]) {
      case "InfrastructureModules":
        case "QuoteModules":
        case "QuoteOPModules":
      case "ShipmentModules":
      case "Logitude_Modules":
      case "CommonModules":
      case "InvoiceModules":
      case "CRMModules":
      case "CustomsModules":
        {
          output = iPathParts[2];

          if (output == "CustomsDeclarationModules") {
            output = iPathParts[3];
          }

          break;
        }

      default: {
        output = iPathParts[1];
        break;
      }
    }

    return output;
  }

  private static async GetModuleProfile(iModuleName: string) {

    let Profile = await this.ModulesBank.filter(f => f.ModuleName == iModuleName)[0];

    if (!Profile) {
      const tempModule = await this.LazyWidgets[iModuleName]();
      let moduleFactory;

      if (tempModule instanceof NgModuleFactory) {
        // For AOT
        //console.log('PART 11111111111111');
        moduleFactory = tempModule;
      }

      else {
        // For JIT
        //console.log('PART 22222222222222');
        moduleFactory = await this.Compiler.compileModuleAsync(tempModule);
      }

      if (!moduleFactory) {
        alert("Error resolving module factory");
      }

      else {
        let Module = (<any>moduleFactory.moduleType);
        let moduleRef = moduleFactory.create(this.Injector);

        Profile = new ModuleProfile(Module, iModuleName, moduleRef.componentFactoryResolver);
        this.ModulesBank.push(Profile);
      }
    }

    return Profile;
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


// https://dev.to/binarysort/manually-lazy-load-components-in-angular-8-ffi
