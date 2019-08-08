"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var forms_1 = require("@angular/forms");
var ModuleDeclarations_1 = require("./ModuleDeclarations");
var ModuleProviders_1 = require("./ModuleProviders");
var ControlsModule = /** @class */ (function () {
    function ControlsModule() {
    }
    ControlsModule.GetComponent = function (name) {
        return ModuleDeclarations_1.ModuleDeclarations.Get(name);
    };
    ControlsModule.GetInstance = function (name) {
        return ModuleProviders_1.ModuleProviders.GetInstance(name);
    };
    ControlsModule = __decorate([
        core_1.NgModule({
            imports: [common_1.CommonModule, forms_1.FormsModule, forms_1.ReactiveFormsModule],
            declarations: ModuleDeclarations_1.Components.concat([ModuleDeclarations_1.Pipes]),
            exports: ModuleDeclarations_1.Components.concat(ModuleDeclarations_1.Pipes, [common_1.CommonModule, forms_1.FormsModule, forms_1.ReactiveFormsModule]),
            entryComponents: ModuleDeclarations_1.Components.slice(),
        })
    ], ControlsModule);
    return ControlsModule;
}());
exports.ControlsModule = ControlsModule;
//# sourceMappingURL=Module_CTRL.js.map