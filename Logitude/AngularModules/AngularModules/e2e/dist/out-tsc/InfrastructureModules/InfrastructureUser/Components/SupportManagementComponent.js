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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TenantManagementPMService_1 = require("../../../Infrastructure/Services/StandardPMs/TenantManagementPMService");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var SupportManagementComponent = /** @class */ (function (_super) {
    __extends(SupportManagementComponent, _super);
    function SupportManagementComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "TenantManagement";
        _this.EntityPM = null;
        _this.IsResourcesReady = false;
        _this.ValidationErrorsList = [];
        _this.SystemSupportEnabledKey = "";
        _this.DistributorSupportEnabledKey = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsSystemSupportEnabledCheck = false;
        _this.IsDistributorSupportEnabledCheck = false;
        _this.CurrentSession.StartBusyIndicatorLoading();
        _this.iService = new TenantManagementPMService_1.TenantManagementPMService();
        _this.iService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                _this.EntityPM = myResponse.Result;
                _this.IsSystemSupportEnabledCheck = _this.EntityPM.IsSystemSupportEnabled;
                _this.IsDistributorSupportEnabledCheck = _this.EntityPM.IsDistributorSupportEnabled;
                _this.IsResourcesReady = true;
            }
            _this.CurrentSession.StopBusyIndicator();
        });
        _this.SystemSupportEnabledKey = Guid_1.Guid.newGuid();
        _this.DistributorSupportEnabledKey = Guid_1.Guid.newGuid();
        return _this;
    }
    Object.defineProperty(SupportManagementComponent.prototype, "IsSystemSupportEnabled", {
        get: function () { return this.EntityPM.IsSystemSupportEnabled; },
        set: function (value) {
            if (this.EntityPM.IsSystemSupportEnabled != value) {
                this.EntityPM.IsSystemSupportEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupportManagementComponent.prototype, "IsDistributorSupportEnabled", {
        get: function () { return this.EntityPM.IsDistributorSupportEnabled; },
        set: function (value) {
            if (this.EntityPM.IsDistributorSupportEnabled != value) {
                this.EntityPM.IsDistributorSupportEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    SupportManagementComponent.prototype.SystemSupportEnabledChecked = function () {
        this.IsSystemSupportEnabledCheck = this.IsSystemSupportEnabledCheck;
    };
    SupportManagementComponent.prototype.DistributorSupportEnabledChecked = function () {
        this.IsDistributorSupportEnabledCheck = this.IsDistributorSupportEnabledCheck;
    };
    SupportManagementComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SupportManagementComponent.prototype.SaveButtonClicked = function () {
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("SupportManagement", "Edit");
        if ((!this.IsSystemSupportEnabledCheck && this.IsSystemSupportEnabled) || (!this.IsDistributorSupportEnabledCheck && this.IsDistributorSupportEnabled)) {
            this.ShowConfirmationWindow();
        }
        else {
            this.SaveChanges();
        }
    };
    SupportManagementComponent.prototype.ShowConfirmationWindow = function () {
        var _this = this;
        var confirmMsg = "Please note that when the system/distributor support is not allowed, we cannot provide online customer support.\nIn case of a problem, please refer to your Logitude account manager.";
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Title = "Customer Care Deactivation";
        confirmWindow.Width = 400;
        confirmWindow.Height = 180;
        confirmWindow.YesButtonText = "OK";
        confirmWindow.NoButtonText = "Cancel";
        confirmWindow.Show(confirmMsg);
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.SaveChanges();
            }
        });
    };
    SupportManagementComponent.prototype.SaveChanges = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicatorSaving();
        this.EntityPM.IsSystemSupportEnabled = this.IsSystemSupportEnabledCheck;
        this.EntityPM.IsDistributorSupportEnabled = this.IsDistributorSupportEnabledCheck;
        this.iService.update(this.EntityPM).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                _this.CurrentSession.CloseCurrentWindow();
            }
        });
    };
    SupportManagementComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SupportManagement',
            templateUrl: './SupportManagementComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SupportManagementComponent);
    return SupportManagementComponent;
}(BaseComponent_1.BaseComponent));
exports.SupportManagementComponent = SupportManagementComponent;
//# sourceMappingURL=SupportManagementComponent.js.map