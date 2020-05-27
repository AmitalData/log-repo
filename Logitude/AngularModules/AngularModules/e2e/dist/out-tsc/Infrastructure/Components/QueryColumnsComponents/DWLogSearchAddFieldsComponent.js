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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var DWLogSearchAddFieldsComponent = /** @class */ (function () {
    function DWLogSearchAddFieldsComponent(cd, _entityListService) {
        this.cd = cd;
        this._entityListService = _entityListService;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    DWLogSearchAddFieldsComponent.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
    };
    DWLogSearchAddFieldsComponent.prototype.ngOnInit = function () {
    };
    DWLogSearchAddFieldsComponent.prototype.AddField = function (item) {
        if (item) {
            this.Destroyed();
            this.CurrentSession.SessionEvent.emit({ Item: item, ComponentName: "DWLogSearchAddFieldsComponent", IsFirstRequest: true });
        }
    };
    DWLogSearchAddFieldsComponent.prototype.Destroyed = function () {
        var isDestroyed = this.cd['destroyed'];
        if (!isDestroyed) {
            this.cd.detectChanges();
        }
    };
    DWLogSearchAddFieldsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DWLogSearchAddFieldsComponent',
            templateUrl: './DWLogSearchAddFieldsComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], DWLogSearchAddFieldsComponent);
    return DWLogSearchAddFieldsComponent;
}());
exports.DWLogSearchAddFieldsComponent = DWLogSearchAddFieldsComponent;
//# sourceMappingURL=DWLogSearchAddFieldsComponent.js.map