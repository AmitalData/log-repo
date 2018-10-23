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
var core_1 = require('@angular/core');
var platform_browser_1 = require('@angular/platform-browser');
var forms_1 = require('@angular/forms');
var http_1 = require('@angular/http');
var RootComponent_1 = require('./RootComponent');
var ModuleDeclarations_1 = require('./ModuleDeclarations');
var LoginService_1 = require('./LoginService');
var PasswordChangeService_1 = require('./PasswordChangeService');
var LogitudeLoginModule = (function () {
    function LogitudeLoginModule() {
    }
    LogitudeLoginModule = __decorate([
        core_1.NgModule({
            imports: [platform_browser_1.BrowserModule, forms_1.FormsModule, forms_1.ReactiveFormsModule, http_1.HttpModule],
            declarations: ModuleDeclarations_1.LoginComponents.slice(),
            //exports:
            //[
            //    ...Pipes,
            //    ...Directives,
            //    ...InfrastructureControlsComponents,
            //    FormsModule,
            //    ReactiveFormsModule,
            //    LogitudeControlsModule,
            //],
            providers: [
                LoginService_1.LoginService,
                PasswordChangeService_1.PasswordChangeService
            ],
            bootstrap: [RootComponent_1.RootComponent]
        }), 
        __metadata('design:paramtypes', [])
    ], LogitudeLoginModule);
    return LogitudeLoginModule;
}());
exports.LogitudeLoginModule = LogitudeLoginModule;
//# sourceMappingURL=LogitudeLoginModule.js.map