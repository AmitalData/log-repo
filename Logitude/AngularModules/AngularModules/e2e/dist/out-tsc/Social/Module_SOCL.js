"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Module_INFR_1 = require("../Infrastructure/Module_INFR");
var ModuleDeclarations_1 = require("./ModuleDeclarations");
var ModuleProviders_1 = require("./ModuleProviders");
var SocialModule = /** @class */ (function () {
    function SocialModule() {
    }
    SocialModule.GetComponent = function (name) {
        return ModuleDeclarations_1.ModuleDeclarations.Get(name);
    };
    SocialModule.GetInstance = function (name) {
        return ModuleProviders_1.ModuleProviders.GetInstance(name);
    };
    SocialModule = __decorate([
        core_1.NgModule({
            imports: [Module_INFR_1.InfrastructureModule],
            declarations: ModuleDeclarations_1.Components.concat([ModuleDeclarations_1.ControlsComponents]),
            entryComponents: ModuleDeclarations_1.Components.concat([ModuleDeclarations_1.ControlsComponents]),
        })
    ], SocialModule);
    return SocialModule;
}());
exports.SocialModule = SocialModule;
//# sourceMappingURL=Module_SOCL.js.map