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
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var TenantPMService_1 = require("../../../../Common/Services/StandardPMs/TenantPMService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var IntegrationSystemsSetting = /** @class */ (function (_super) {
    __extends(IntegrationSystemsSetting, _super);
    function IntegrationSystemsSetting() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ExportQuotationsToIntegratedSystem = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (_this.tenantPMService == null) {
            _this.tenantPMService = new TenantPMService_1.TenantPMService();
        }
        _this.LoadCurrentTenant();
        return _this;
    }
    IntegrationSystemsSetting.prototype.ngOnInit = function () {
    };
    IntegrationSystemsSetting.prototype.LoadCurrentTenant = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.tenantPMService.get(SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.myTenantPM = myResult;
                    if (_this.myTenantPM) {
                        _this.ExportQuotationsToIntegratedSystem = _this.myTenantPM.ExportQuotationsToIntegratedSystem;
                    }
                }
            }
        });
    };
    IntegrationSystemsSetting.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    IntegrationSystemsSetting.prototype.SaveButtonClicked = function () {
        var _this = this;
        if (this.myTenantPM) {
            if (this.ExportQuotationsToIntegratedSystem != this.myTenantPM.ExportQuotationsToIntegratedSystem) {
                this.myTenantPM.ExportQuotationsToIntegratedSystem = this.ExportQuotationsToIntegratedSystem;
                this.CurrentSession.StartBusyIndicatorSaving();
                this.tenantPMService.update(this.myTenantPM).subscribe(function (res) {
                    _this.CurrentSession.StopBusyIndicator();
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        SessionLocator_1.SessionLocator.TenantPM.ExportQuotationsToIntegratedSystem = _this.ExportQuotationsToIntegratedSystem;
                        _this.CloseButtonClicked();
                    }
                });
            }
            else
                this.CloseButtonClicked();
        }
    };
    IntegrationSystemsSetting = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'IntegrationSystemsSetting',
            templateUrl: './IntegrationSystemsSetting.html',
        }),
        __metadata("design:paramtypes", [])
    ], IntegrationSystemsSetting);
    return IntegrationSystemsSetting;
}(BaseComponent_1.BaseComponent));
exports.IntegrationSystemsSetting = IntegrationSystemsSetting;
//# sourceMappingURL=IntegrationSystemsSetting.js.map