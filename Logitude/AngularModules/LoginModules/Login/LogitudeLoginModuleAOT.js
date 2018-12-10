"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var forms_1 = require("@angular/forms");
var http_1 = require("@angular/http");
var RootComponentAOT_1 = require("./RootComponentAOT");
var ModuleDeclarations_1 = require("./ModuleDeclarations");
var LoginService_1 = require("./LoginService");
var PasswordChangeService_1 = require("./PasswordChangeService");
var LogitudeLoginModuleAOT = /** @class */ (function () {
    function LogitudeLoginModuleAOT() {
    }
    LogitudeLoginModuleAOT = __decorate([
        core_1.NgModule({
            imports: [platform_browser_1.BrowserModule, forms_1.FormsModule, forms_1.ReactiveFormsModule, http_1.HttpModule],
            declarations: [
                RootComponentAOT_1.RootComponentAOT
            ].concat(ModuleDeclarations_1.LoginComponents),
            //exports: [
            //    ...Pipes,
            //    ...Directives,
            //    ...InfrastructureControlsComponents,
            //    FormsModule,
            //    ReactiveFormsModule,
            //    LogitudeControlsModule,
            //],
            entryComponents: [
                RootComponentAOT_1.RootComponentAOT
            ].concat(ModuleDeclarations_1.LoginComponents),
            providers: [
                LoginService_1.LoginService,
                PasswordChangeService_1.PasswordChangeService
            ],
            bootstrap: [RootComponentAOT_1.RootComponentAOT]
        })
    ], LogitudeLoginModuleAOT);
    return LogitudeLoginModuleAOT;
}());
exports.LogitudeLoginModuleAOT = LogitudeLoginModuleAOT;
//# sourceMappingURL=LogitudeLoginModuleAOT.js.map