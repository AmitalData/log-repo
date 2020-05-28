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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
//import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
//import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
//import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
//import {Guid} from '../../../Infrastructure/Utilities/Guid';
var DWFilterSettings = /** @class */ (function (_super) {
    __extends(DWFilterSettings, _super);
    function DWFilterSettings() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "TenantManagement";
        _this.DataContext = _this;
        _this.IsResourcesReady = false;
        _this.ValidationErrorsList = [];
        _this.SystemSupportEnabledKey = "";
        _this.DistributorSupportEnabledKey = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsMandatoryFilter = false;
        _this.IsSetDefaults = false;
        return _this;
    }
    DWFilterSettings.prototype.SetWindowArgs = function (args) {
        this.IsSetDefaults = args.IsSetDefaults;
        this.IsMandatoryFilter = args.IsMandatoryFilter;
    };
    DWFilterSettings.prototype.MandatoryFilterChecked = function (value) {
        this.IsMandatoryFilter = value;
    };
    DWFilterSettings.prototype.SetDefaultsChecked = function (value) {
        this.IsSetDefaults = value;
    };
    DWFilterSettings.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DWFilterSettings.prototype.SaveButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit(this.IsMandatoryFilter + "," + this.IsSetDefaults);
        //if ((!this.IsSystemSupportEnabledCheck && this.IsSystemSupportEnabled) || (!this.IsDistributorSupportEnabledCheck && this.IsDistributorSupportEnabled)) {
        //    this.ShowConfirmationWindow();
        //}
        //else {
        //    this.SaveChanges();
        //}            
    };
    DWFilterSettings = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DWFilterSettings',
            templateUrl: './DWFilterSettings.html',
        }),
        __metadata("design:paramtypes", [])
    ], DWFilterSettings);
    return DWFilterSettings;
}(BaseComponent_1.BaseComponent));
exports.DWFilterSettings = DWFilterSettings;
//# sourceMappingURL=DWFilterSettings.js.map