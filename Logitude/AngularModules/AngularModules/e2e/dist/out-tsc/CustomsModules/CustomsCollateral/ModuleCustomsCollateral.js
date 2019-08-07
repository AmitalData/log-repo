"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Module_INFR_1 = require("../../Infrastructure/Module_INFR");
var ModuleDeclarations_1 = require("./ModuleDeclarations");
var ModuleCustomsControls_1 = require("../CustomsControls/ModuleCustomsControls");
var ModuleCustomsCollateral = /** @class */ (function () {
    function ModuleCustomsCollateral() {
    }
    ModuleCustomsCollateral.GetComponent = function (name) {
        return ModuleDeclarations_1.ModuleDeclarations.Get(name);
    };
    ModuleCustomsCollateral = __decorate([
        core_1.NgModule({
            imports: [Module_INFR_1.InfrastructureModule, ModuleCustomsControls_1.ModuleCustomsControls],
            exports: [ModuleCustomsControls_1.ModuleCustomsControls],
            declarations: ModuleDeclarations_1.Components.slice(),
            entryComponents: ModuleDeclarations_1.Components.slice(),
        })
    ], ModuleCustomsCollateral);
    return ModuleCustomsCollateral;
}());
exports.ModuleCustomsCollateral = ModuleCustomsCollateral;
//# sourceMappingURL=ModuleCustomsCollateral.js.map