"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var AppComponent_Cust = /** @class */ (function () {
    function AppComponent_Cust(compiler, resolver, http, moduleLoader, injector) {
        this.compiler = compiler;
        this.resolver = resolver;
        this.http = http;
        this.moduleLoader = moduleLoader;
        this.injector = injector;
        //console.log("isDevMode: " + isDevMode);
        //console.log("environment: " + environment.production);
    }
    AppComponent_Cust.prototype.ngOnInit = function () {
        var _this = this;
        this.moduleLoader.load('Infrastructure/Module_INFR#InfrastructureModule').then(function (moduleFactory) {
            var Module = moduleFactory.moduleType;
            var Component = Module.GetComponent("RootComponent_Cust");
            if (Component) {
                var moduleRef = moduleFactory.create(_this.injector);
                var compFactory = moduleRef.componentFactoryResolver.resolveComponentFactory(Component);
                var cmpRef = _this.location.createComponent(compFactory);
                cmpRef.instance.Boot({ Compiler: _this.compiler, Resolver: _this.resolver, Injector: _this.injector, ModuleLoader: _this.moduleLoader, Http: _this.http });
            }
        });
    };
    __decorate([
        core_1.ViewChild("Child", { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], AppComponent_Cust.prototype, "location", void 0);
    AppComponent_Cust = __decorate([
        core_1.Component({
            selector: 'AppComponent',
            //<img *ngIf="!_FinishLogin" class="CenterCenter" src="./_Resources/Images/Gif/Bluespin.gif" />
            template: "\n    <div class=\"MediaFillRelative\">\n        <div #Child></div>\n    </div>\n    ",
        }),
        __metadata("design:paramtypes", [core_1.Compiler, core_1.ComponentFactoryResolver, http_1.Http, core_1.SystemJsNgModuleLoader, core_1.Injector])
    ], AppComponent_Cust);
    return AppComponent_Cust;
}());
exports.AppComponent_Cust = AppComponent_Cust;
//# sourceMappingURL=AppComponent_Cust.js.map