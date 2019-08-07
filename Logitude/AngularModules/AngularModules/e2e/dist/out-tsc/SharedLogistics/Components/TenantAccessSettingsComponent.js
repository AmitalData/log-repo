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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var TenantPMService_1 = require("../../Common/Services/StandardPMs/TenantPMService");
var BaseComponent_1 = require("../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../Infrastructure/Tools");
var TenantAccessSettingsComponent = /** @class */ (function (_super) {
    __extends(TenantAccessSettingsComponent, _super);
    function TenantAccessSettingsComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    Object.defineProperty(TenantAccessSettingsComponent.prototype, "LogBoxAdminUserId", {
        get: function () {
            return this.EntityPM != null ? this.EntityPM.LogBoxAdminUserId : null;
        },
        set: function (value) {
            if (this.EntityPM.LogBoxAdminUserId != value)
                this.EntityPM.LogBoxAdminUserId = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantAccessSettingsComponent.prototype, "IsCustomerTenantShareEnable", {
        get: function () {
            return this.EntityPM != null ? this.EntityPM.IsCustomerTenantShare == true ? false : true : null;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TenantAccessSettingsComponent.prototype, "IsCustomerTenantShare", {
        get: function () {
            return this.EntityPM != null ? this.EntityPM.IsCustomerTenantShare : null;
        },
        set: function (value) {
            if (this.EntityPM.IsCustomerTenantShare != value) {
                this.EntityPM.IsCustomerTenantShare = value;
                this.Parent.RefreshTenantScreenData();
            }
        },
        enumerable: true,
        configurable: true
    });
    TenantAccessSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    TenantAccessSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (this.EntityPM.IsCustomerTenantShare && !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.LogBoxAdminUserId)) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var service = new TenantPMService_1.TenantPMService();
            service.update(this.EntityPM).subscribe(function (res) {
                _this.CurrentSession.StopBusyIndicator();
                SessionLocator_1.SessionLocator.TenantPM = _this.EntityPM;
                _this.CurrentSession.CloseCurrentWindow();
            });
        }
        else {
            this.ValidationErrorsList.push("LogBox Administrator User is required");
        }
    };
    TenantAccessSettingsComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.EntityPM;
        this.Parent = args.Parent;
    };
    TenantAccessSettingsComponent = __decorate([
        core_1.Component({
            moduleId: './SharedLogistics/Components/',
            selector: 'TenantAccessSettingsComponent',
            templateUrl: 'TenantAccessSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], TenantAccessSettingsComponent);
    return TenantAccessSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.TenantAccessSettingsComponent = TenantAccessSettingsComponent;
//# sourceMappingURL=TenantAccessSettingsComponent.js.map