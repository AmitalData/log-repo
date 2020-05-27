"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Module_CTRL_1 = require("../Controls/Module_CTRL");
var ModuleDeclarations_1 = require("./ModuleDeclarations");
var ModuleProviders_1 = require("./ModuleProviders");
// Services
var EntityArgs_1 = require("./DataContracts/EntityArgs");
var ServiceArgs_1 = require("./DataContracts/ServiceArgs");
var LoginService_1 = require("./Services/LoginService");
var EntityListService_1 = require("./Services/EntityListService");
var EntityPMService_1 = require("./Services/EntityPMService");
var IndexedDbService_1 = require("./Services/IndexedDbService");
var EntityResourceService_1 = require("./Services/EntityResourceService");
var EntityLastActivityService_1 = require("./Services/EntityLastActivityService");
var TotangoService_1 = require("./Services/WebServices/TotangoService");
var LogitudeErrorHandler_1 = require("./Utilities/LogitudeErrorHandler");
var core_2 = require("@angular/core");
var InfrastructureModule = /** @class */ (function () {
    function InfrastructureModule() {
    }
    InfrastructureModule.GetComponent = function (name) {
        return ModuleDeclarations_1.ModuleDeclarations.Get(name);
    };
    InfrastructureModule.GetInstance = function (name) {
        return ModuleProviders_1.ModuleProviders.GetInstance(name);
    };
    InfrastructureModule = __decorate([
        core_1.NgModule({
            imports: [Module_CTRL_1.ControlsModule],
            declarations: ModuleDeclarations_1.Pipes.concat(ModuleDeclarations_1.Directives, ModuleDeclarations_1.Components, ModuleDeclarations_1.ControlsComponents),
            exports: ModuleDeclarations_1.Pipes.concat(ModuleDeclarations_1.Directives, ModuleDeclarations_1.ControlsComponents, [Module_CTRL_1.ControlsModule]),
            entryComponents: ModuleDeclarations_1.Components.concat(ModuleDeclarations_1.ControlsComponents),
            providers: [
                EntityArgs_1.EntityArgs,
                ServiceArgs_1.ServiceArgs,
                LoginService_1.LoginService,
                EntityListService_1.EntityListService,
                EntityPMService_1.EntityPMService,
                IndexedDbService_1.IndexedDbService,
                EntityResourceService_1.EntityResourceService,
                EntityLastActivityService_1.EntityLastActivityService,
                TotangoService_1.TotangoService,
                //Islam: if you comment it dont check in!
                { provide: core_2.ErrorHandler, useClass: LogitudeErrorHandler_1.LogitudeErrorHandler }
            ],
        })
    ], InfrastructureModule);
    return InfrastructureModule;
}());
exports.InfrastructureModule = InfrastructureModule;
//# sourceMappingURL=Module_INFR.js.map