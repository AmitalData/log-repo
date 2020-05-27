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
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ShipmentAdditionalCloudDataService_1 = require("../../../../Shipment/Services/Others/ShipmentAdditionalCloudDataService");
var DenyReasonComponent = /** @class */ (function (_super) {
    __extends(DenyReasonComponent, _super);
    function DenyReasonComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._ShipmentAdditionalCloudDataService = new ShipmentAdditionalCloudDataService_1.ShipmentAdditionalCloudDataService();
        return _this;
    }
    DenyReasonComponent.prototype.ngOnInit = function () {
    };
    DenyReasonComponent.prototype.ngAfterViewInit = function () {
    };
    DenyReasonComponent.prototype.SetWindowArgs = function (args) {
        this.AdditionalData = args.AdditionalData;
    };
    DenyReasonComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DenyReasonComponent.prototype.SendButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.DenyReason)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "DenyReason"));
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            this.AdditionalData.IsImporterApprovalRequried = false;
            this.DenyReason = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName + ", " + SessionLocator_1.SessionLocator.LoggedUserPM.LocalName + ", " + SessionLocator_1.SessionLocator.LoggedUserPM.Email + ", " + this.DenyReason + ", " + this.AdditionalData.VersionApproved;
            this._ShipmentAdditionalCloudDataService.update(this.AdditionalData).subscribe(function (AdditionalResult) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                _this.CurrentSession.CloseCurrentWindowEmit("Denied");
            });
        }
    };
    Object.defineProperty(DenyReasonComponent.prototype, "DenyReason", {
        get: function () { return this.AdditionalData.DenyReason; },
        set: function (newValue) { this.AdditionalData.DenyReason = newValue; },
        enumerable: true,
        configurable: true
    });
    DenyReasonComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DenyReasonComponent.html'
        }),
        __metadata("design:paramtypes", [])
    ], DenyReasonComponent);
    return DenyReasonComponent;
}(BaseComponent_1.BaseComponent));
exports.DenyReasonComponent = DenyReasonComponent;
//# sourceMappingURL=DenyReasonComponent.js.map