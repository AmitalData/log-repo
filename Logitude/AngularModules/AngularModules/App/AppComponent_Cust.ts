
import { Component, OnInit, ViewChild, ViewContainerRef, Compiler, ComponentFactoryResolver, Injector, SystemJsNgModuleLoader, NgModuleFactory, isDevMode } from '@angular/core';
import { Http } from '@angular/http';
import { environment } from '../environments/environment';

@Component({
    selector: 'AppComponent',
    //<img *ngIf="!_FinishLogin" class="CenterCenter" src="./_Resources/Images/Gif/Bluespin.gif" />
    template:
    `
    <div class="MediaFillRelative">
        <div #Child></div>
    </div>
    `,
})

export class AppComponent_Cust implements OnInit {
    @ViewChild("Child", { read: ViewContainerRef }) location: ViewContainerRef;
    constructor(private compiler: Compiler, private resolver: ComponentFactoryResolver, private http: Http, private moduleLoader: SystemJsNgModuleLoader, private injector: Injector) {
        //console.log("isDevMode: " + isDevMode);
        //console.log("environment: " + environment.production);
    }

    ngOnInit() {
        this.moduleLoader.load('Infrastructure/Module_INFR#InfrastructureModule').then((moduleFactory: NgModuleFactory<any>) => {

            let Module = (<any>moduleFactory.moduleType);
            let Component = Module.GetComponent("RootComponent_Cust");

            if (Component) {
                const moduleRef = moduleFactory.create(this.injector);
                const compFactory = moduleRef.componentFactoryResolver.resolveComponentFactory(Component);
                let cmpRef: any = this.location.createComponent(compFactory);
                cmpRef.instance.Boot({ Compiler: this.compiler, Resolver: this.resolver, Injector: this.injector, ModuleLoader: this.moduleLoader, Http: this.http });
            }
        });
    }
}