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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TwoFactorAuthenticationDeviceExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/TwoFactorAuthenticationDeviceExtendedPMService");
var TwoFactorAuthenticationDevicePMService_1 = require("../../../../Common/Services/StandardPMs/TwoFactorAuthenticationDevicePMService");
var DevicesTabComponent = /** @class */ (function (_super) {
    __extends(DevicesTabComponent, _super);
    function DevicesTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "TwoFactorAuthenticationDevice";
        _this.DataContext = _this;
        _this.ItemsSource = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = entityArgs.EntityPM;
        _this.twoFactorAuthenticationDeviceExtendedPMService = new TwoFactorAuthenticationDeviceExtendedPMService_1.TwoFactorAuthenticationDeviceExtendedPMService();
        _this.twoFactorAuthenticationDevicePMService = new TwoFactorAuthenticationDevicePMService_1.TwoFactorAuthenticationDevicePMService();
        _this.twoFactorAuthenticationDeviceExtendedPMService.GetDevicesByUser(_this.EntityPM.Id).subscribe(function (response) {
            if (!response.HasError) {
                _this.ItemsSource = response.Result;
            }
        });
        return _this;
    }
    DevicesTabComponent.prototype.DeviceActivation = function (device, isActive) {
        var _this = this;
        device.InActive = !isActive;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.twoFactorAuthenticationDevicePMService.update(device).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            if (response.HasError) {
            }
        });
    };
    DevicesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DevicesTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], DevicesTabComponent);
    return DevicesTabComponent;
}(BaseComponent_1.BaseComponent));
exports.DevicesTabComponent = DevicesTabComponent;
//# sourceMappingURL=DevicesTabComponent.js.map