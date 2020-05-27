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
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var CustomsShipperFiltersMenuComponent = /** @class */ (function () {
    function CustomsShipperFiltersMenuComponent() {
    }
    CustomsShipperFiltersMenuComponent.prototype.NewDepositionFormClcik = function () {
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("CustomsShipper", "Deposition Link");
        var link = "https://forms.gov.il/globaldata/getsequence/getHtmlForm.aspx?formType=SOVE01_hasava@taxes.gov.il";
        var win = window.open(link, '_blank');
        win.focus();
    };
    CustomsShipperFiltersMenuComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsShipperFiltersMenuComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomsShipperFiltersMenuComponent);
    return CustomsShipperFiltersMenuComponent;
}());
exports.CustomsShipperFiltersMenuComponent = CustomsShipperFiltersMenuComponent;
//# sourceMappingURL=CustomsShipperFiltersMenuComponent.js.map