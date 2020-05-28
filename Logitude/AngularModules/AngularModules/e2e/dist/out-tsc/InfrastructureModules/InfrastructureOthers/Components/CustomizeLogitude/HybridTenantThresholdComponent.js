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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var HybridTenantThresholdPM_1 = require("../../../../Common/EntityPMs/HybridTenantThresholdPM");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var HybridTenantThresholdPMService_1 = require("../../../../Common/Services/StandardPMs/HybridTenantThresholdPMService");
var HybridTenantThresholdComponent = /** @class */ (function (_super) {
    __extends(HybridTenantThresholdComponent, _super);
    function HybridTenantThresholdComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Isupdate = false;
        _this.DataLoaded = false;
        _this.LoadHybridTenantThreshold();
        return _this;
    }
    HybridTenantThresholdComponent.prototype.LoadHybridTenantThreshold = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var service = new CommonDomainService_1.CommonDomainService();
        service.GetHybridTenantThresholdByIdTenant().subscribe(function (res) {
            if (!res.HasError) {
                _this.EntityPM = res.Result;
                if (_this.EntityPM == null) {
                    _this.EntityPM = new HybridTenantThresholdPM_1.HybridTenantThresholdPM();
                    _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    _this.EntityPM.WaitingThresold = 10;
                    _this.EntityPM.FailedThresold = 10;
                    _this.Isupdate = false;
                }
                else {
                    _this.Isupdate = true;
                }
            }
            _this.CurrentSession.StopBusyIndicator();
            _this.DataLoaded = true;
        });
    };
    Object.defineProperty(HybridTenantThresholdComponent.prototype, "WaitingThresold", {
        get: function () {
            var waitingThresold = 0;
            if (this.EntityPM != null) {
                waitingThresold = this.EntityPM.WaitingThresold;
            }
            return waitingThresold;
        },
        set: function (value) {
            if (value != this.EntityPM.WaitingThresold) {
                this.EntityPM.WaitingThresold = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridTenantThresholdComponent.prototype, "FailedThresold", {
        get: function () {
            var failedThresold = 0;
            if (this.EntityPM != null) {
                failedThresold = this.EntityPM.FailedThresold;
            }
            return failedThresold;
        },
        set: function (value) {
            if (value != this.EntityPM.FailedThresold) {
                this.EntityPM.FailedThresold = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    HybridTenantThresholdComponent.prototype.CloseButtonClicked = function () { this.CurrentSession.CloseCurrentWindow(); };
    HybridTenantThresholdComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        var service = new HybridTenantThresholdPMService_1.HybridTenantThresholdPMService();
        if (!this.Isupdate) {
            service.insert(this.EntityPM).subscribe(function (res) {
                _this.CurrentSession.CloseCurrentWindow();
            });
        }
        else {
            service.update(this.EntityPM).subscribe(function (res) {
                _this.CurrentSession.CloseCurrentWindow();
            });
        }
    };
    HybridTenantThresholdComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'HybridTenantThresholdComponent',
            templateUrl: './HybridTenantThresholdComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], HybridTenantThresholdComponent);
    return HybridTenantThresholdComponent;
}(BaseComponent_1.BaseComponent));
exports.HybridTenantThresholdComponent = HybridTenantThresholdComponent;
//# sourceMappingURL=HybridTenantThresholdComponent.js.map