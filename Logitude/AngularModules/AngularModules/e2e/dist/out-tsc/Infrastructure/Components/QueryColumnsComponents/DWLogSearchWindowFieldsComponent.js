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
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var DWLogSearchWindowFieldsComponent = /** @class */ (function () {
    function DWLogSearchWindowFieldsComponent(CD, _entityListService) {
        this.CD = CD;
        this._entityListService = _entityListService;
        this.PartnerTypes = [];
    }
    DWLogSearchWindowFieldsComponent.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    DWLogSearchWindowFieldsComponent.prototype.ngOnInit = function () {
    };
    DWLogSearchWindowFieldsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DWLogSearchWindowFieldsComponent',
            templateUrl: './DWLogSearchWindowFieldsComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], DWLogSearchWindowFieldsComponent);
    return DWLogSearchWindowFieldsComponent;
}());
exports.DWLogSearchWindowFieldsComponent = DWLogSearchWindowFieldsComponent;
//# sourceMappingURL=DWLogSearchWindowFieldsComponent.js.map