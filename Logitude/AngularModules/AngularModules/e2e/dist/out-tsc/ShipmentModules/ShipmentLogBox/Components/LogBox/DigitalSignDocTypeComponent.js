"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DocumentTypePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var DocumentTypePMService_1 = require("../../../../Common/Services/StandardPMs/DocumentTypePMService");
var DocumentTypePM_1 = require("../../../../Common/EntityPMs/DocumentTypePM");
var DigitalSignDocTypeComponent = /** @class */ (function (_super) {
    __extends(DigitalSignDocTypeComponent, _super);
    function DigitalSignDocTypeComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.UpdateDocTypes = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._DocumentTypeListService = new DocumentTypePMExtendedService_1.DocumentTypePMExtendedService();
        _this._DocumentTypePMService = new DocumentTypePMService_1.DocumentTypePMService();
        return _this;
    }
    DigitalSignDocTypeComponent.prototype.ngOnInit = function () {
        var _this = this;
        var objectTablePm = window.ObjectTables.filter(function (d) { return d.Name == "Shipment"; })[0];
        this._DocumentTypeListService.GetDocumentTypesByObjectTableAndTenant(objectTablePm.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            _this.DocTypes = res.Result;
            _this.DocTypes = _this.DocTypes.sort(function (a, b) { return (a.OrderBy === b.OrderBy) ? 0 : (a.OrderBy < b.OrderBy) ? -1 : 1; });
        });
    };
    DigitalSignDocTypeComponent.prototype.ngAfterViewInit = function () {
    };
    DigitalSignDocTypeComponent.prototype.SetWindowArgs = function (args) {
        //this.AdditionalData = args.AdditionalData;
    };
    DigitalSignDocTypeComponent.prototype.onCheckBoxChecked = function (Type) {
        if (this.UpdateDocTypes.filter(function (a) { return a.Id == Type.Id; }).length > 0) {
            this.UpdateDocTypes = this.UpdateDocTypes.filter(function (a) { return a.Id != Type.Id; });
        }
        this.UpdateDocTypes.push(this.CustomMapJsonToEntityPM(Type));
    };
    DigitalSignDocTypeComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DigitalSignDocTypeComponent.prototype.OKButtonClicked = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ..");
        //var itemsProcessed = 0;
        //this.UpdateDocTypes.forEach((Type) => {
        this._DocumentTypeListService.update(this.UpdateDocTypes).subscribe(function (myResult) {
            //itemsProcessed++;
            //if (itemsProcessed === this.UpdateDocTypes.length) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            _this.CurrentSession.CloseCurrentWindow();
            //}
        });
        //});
    };
    DigitalSignDocTypeComponent.prototype.CustomMapJsonToEntityPM = function (jsonPM, entityPM) {
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new DocumentTypePM_1.DocumentTypePM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    DigitalSignDocTypeComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DigitalSignDocTypeComponent.html'
        }),
        __metadata("design:paramtypes", [])
    ], DigitalSignDocTypeComponent);
    return DigitalSignDocTypeComponent;
}(BaseComponent_1.BaseComponent));
exports.DigitalSignDocTypeComponent = DigitalSignDocTypeComponent;
//# sourceMappingURL=DigitalSignDocTypeComponent.js.map