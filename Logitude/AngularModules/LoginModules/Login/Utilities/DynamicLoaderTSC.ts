import {ViewContainerRef, ComponentRef, Compiler, ModuleWithComponentFactories, ComponentFactoryResolver} from '@angular/core';

export class DynamicLoaderTSC {
    public static Compiler: Compiler;
    public static Resolver: ComponentFactoryResolver
    public static Load(myComponentPath: string, location: ViewContainerRef): Promise<ComponentRef<any>> {
        if (myComponentPath != null) {

            var stringParts: string[] = myComponentPath.split("/");
            let myModuleName = "Logitude" + stringParts[1] + "Module";
            let myModulePath = "./" + stringParts[1] + "/" + myModuleName;

            let myComponentName: any = null;
            if (myComponentPath != null) {
                var urlParts: string[] = myComponentPath.split("/");
                myComponentName = urlParts[urlParts.length - 1];
            }

            return new Promise(resolve => {

                (<any>window).System.import(myModulePath)
                    .then((m: any) => m[myModuleName])

                    .then((type: any) => {
                        return this.Compiler.compileModuleAndAllComponentsAsync(type)
                    })

                    .then((moduleWithFactories: ModuleWithComponentFactories<any>) => {
                        const factory: any = moduleWithFactories.componentFactories.find(x => x.componentType.name === myComponentName);

                        if (factory) {
                            let componentRef = location.createComponent(factory);
                            resolve(componentRef);
                        }

                        else {
                            alert("(TSC) " + myComponentName + " is not declared in " + myModuleName);
                        }
                    });
            });
        }
    }

    public static GetInstance(myPath: string) {
        if (myPath != null) {

            myPath = myPath.replace("CommunicationStatusTypesList", "CommunicationStatusTypeList");

            var urlParts: string[] = myPath.split("/");
            let myModuleName = "Logitude" + urlParts[1] + "Module";
            let instanceName = urlParts[urlParts.length - 1];

            return new Promise((resolve, reject) => {                
                (<any>window).System.import(myPath)
                    .then(m => {
                        let instanceType = m[instanceName];

                        if (instanceType) {
                            let instance = Object.create(instanceType.prototype);
                            instance.constructor.apply(instance);
                            resolve(instance);
                        }

                        else {
                            alert("(TSC) " + instanceName + " is not declared in " + myModuleName);
                        }
                    });
            });
        }

        else {
            alert("Instance Path is null");
        }
    }
}