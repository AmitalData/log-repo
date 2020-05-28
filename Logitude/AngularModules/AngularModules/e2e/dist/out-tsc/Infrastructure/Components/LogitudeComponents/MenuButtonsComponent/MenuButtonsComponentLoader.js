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
var ObjectTablePM_1 = require("../../../EntityPMs/ObjectTablePM");
var MenuButtonPM_1 = require("../../../EntityPMs/MenuButtonPM");
var SessionLocator_1 = require("../../../Utilities/SessionLocator");
var MenuButtonsComponentLoader = /** @class */ (function () {
    function MenuButtonsComponentLoader() {
    }
    MenuButtonsComponentLoader.prototype.ngOnInit = function () {
        var _this = this;
        if (this.MenuButton.HtmlComponentPath) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load(this.MenuButton.HtmlComponentPath, this.ComponentViewContainerRef).then(function (cmpRef) {
                cmpRef.instance.Run({ EntityPM: _this.EntityPM, ObjectTable: _this.ObjectTable });
            });
        }
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", MenuButtonPM_1.MenuButtonPM)
    ], MenuButtonsComponentLoader.prototype, "MenuButton", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", ObjectTablePM_1.ObjectTablePM)
    ], MenuButtonsComponentLoader.prototype, "ObjectTable", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], MenuButtonsComponentLoader.prototype, "EntityPM", void 0);
    __decorate([
        core_1.ViewChild('MenuButtonComponent', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], MenuButtonsComponentLoader.prototype, "ComponentViewContainerRef", void 0);
    MenuButtonsComponentLoader = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'MenuButtonsComponentLoader',
            templateUrl: "./MenuButtonsComponentLoader.html",
        }),
        __metadata("design:paramtypes", [])
    ], MenuButtonsComponentLoader);
    return MenuButtonsComponentLoader;
}());
exports.MenuButtonsComponentLoader = MenuButtonsComponentLoader;
//# sourceMappingURL=MenuButtonsComponentLoader.js.map