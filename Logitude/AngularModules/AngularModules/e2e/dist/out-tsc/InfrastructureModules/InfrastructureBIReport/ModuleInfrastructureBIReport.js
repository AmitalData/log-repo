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
var ag_grid_angular_1 = require("ag-grid-angular");
var AGGridCustomHeader_1 = require("./Components/TemplateRenderer/AGGridCustomHeader");
var ModuleInfrastructureBIReport = /** @class */ (function () {
    function ModuleInfrastructureBIReport() {
    }
    ModuleInfrastructureBIReport.GetComponent = function (name) {
        return ModuleDeclarations_1.ModuleDeclarations.Get(name);
    };
    ModuleInfrastructureBIReport = __decorate([
        core_1.NgModule({
            imports: [Module_INFR_1.InfrastructureModule, ag_grid_angular_1.AgGridModule.withComponents([AGGridCustomHeader_1.AGGridCustomHeader])],
            declarations: ModuleDeclarations_1.Components.concat([AGGridCustomHeader_1.AGGridCustomHeader]),
            entryComponents: ModuleDeclarations_1.Components.slice(),
        })
    ], ModuleInfrastructureBIReport);
    return ModuleInfrastructureBIReport;
}());
exports.ModuleInfrastructureBIReport = ModuleInfrastructureBIReport;
//# sourceMappingURL=ModuleInfrastructureBIReport.js.map