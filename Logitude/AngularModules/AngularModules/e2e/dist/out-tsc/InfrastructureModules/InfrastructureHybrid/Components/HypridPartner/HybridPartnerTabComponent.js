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
var HybridPartnerPM_1 = require("../../../../Common/EntityPMs/HybridPartnerPM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var HybridPartnerPMService_1 = require("../../../../Common/Services/StandardPMs/HybridPartnerPMService");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var WebFreightDomainService_1 = require("../../../../Infrastructure/Services/WebFreightDomainService");
var HybridPartnerTabComponent = /** @class */ (function (_super) {
    __extends(HybridPartnerTabComponent, _super);
    function HybridPartnerTabComponent(entityArgs, CD) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CD = CD;
        _this.DataContext = _this;
        _this.myentityPM = new HybridPartnerPM_1.HybridPartnerPM();
        _this.Source = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._HybridPartnerPMService = new HybridPartnerPMService_1.HybridPartnerPMService();
        _this.myentityPM = entityArgs.EntityPM;
        var myService = new WebFreightDomainService_1.WebFreightDomainService();
        myService.getHypridPartnerLogo(_this.myentityPM.LogoId).subscribe(function (myResult) {
            if (myResult) {
                _this.Source = "data:image/JPEG;base64," + myResult;
                var isDestroyed = _this.CD['destroyed'];
                if (!isDestroyed) {
                    _this.CD.detectChanges();
                }
            }
        });
        return _this;
    }
    HybridPartnerTabComponent.prototype.SetWindowArgs = function (args) {
        alert("Yeaaaa");
    };
    Object.defineProperty(HybridPartnerTabComponent.prototype, "Name", {
        get: function () { return this.myentityPM.Name; },
        set: function (newValue) { this.myentityPM.Name = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridPartnerTabComponent.prototype, "LocalName", {
        get: function () { return this.myentityPM.LocalName; },
        set: function (newValue) { this.myentityPM.LocalName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridPartnerTabComponent.prototype, "PartnerTenant", {
        get: function () { return this.myentityPM.PartnerTenant; },
        set: function (newValue) { this.myentityPM.PartnerTenant = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridPartnerTabComponent.prototype, "IsMislakaActivated", {
        get: function () { return this.myentityPM.IsMislakaActivated; },
        set: function (newValue) { this.myentityPM.IsMislakaActivated = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridPartnerTabComponent.prototype, "IsExternalPartner", {
        get: function () { return this.myentityPM.IsExternalPartner; },
        set: function (newValue) { this.myentityPM.IsExternalPartner = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridPartnerTabComponent.prototype, "ReceiveAllStatuses", {
        get: function () { return this.myentityPM.ReceiveAllStatuses; },
        set: function (newValue) { this.myentityPM.ReceiveAllStatuses = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridPartnerTabComponent.prototype, "AllowSendingDocsToAgent", {
        get: function () { return this.myentityPM.AllowSendingDocsToAgent; },
        set: function (newValue) { this.myentityPM.AllowSendingDocsToAgent = newValue; },
        enumerable: true,
        configurable: true
    });
    HybridPartnerTabComponent.prototype.onIsMislakaActivated = function (event) {
        this.IsMislakaActivated = event;
    };
    HybridPartnerTabComponent.prototype.onIsExternalPartner = function (event) {
        this.IsExternalPartner = event;
    };
    HybridPartnerTabComponent.prototype.onReceiveAllStatuses = function (event) {
        this.ReceiveAllStatuses = event;
    };
    HybridPartnerTabComponent.prototype.onAllowSendingDocsToAgent = function (event) {
        this.AllowSendingDocsToAgent = event;
    };
    HybridPartnerTabComponent.prototype.OpenUpLoadLogo = function () {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        var windowArgs = {};
        windowArgs.EntityPM = this.myentityPM;
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Width = 750;
        logitudeWindow.Height = 500;
        logitudeWindow.Title = "";
        logitudeWindow.Show('./InfrastructureModules/InfrastructureHybrid/Components/HypridPartner/HybridPartnerUploadLogoComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            var myService = new WebFreightDomainService_1.WebFreightDomainService();
            myService.getHypridPartnerLogo(_this.myentityPM.LogoId).subscribe(function (myResult) {
                if (myResult) {
                    _this.Source = "data:image/JPEG;base64," + myResult;
                    var isDestroyed = _this.CD['destroyed'];
                    if (!isDestroyed) {
                        _this.CD.detectChanges();
                    }
                }
            });
        });
    };
    HybridPartnerTabComponent.prototype.SaveChanges = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.Name)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Name"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.PartnerTenant)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "Partner Tenant"));
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ..");
            this._HybridPartnerPMService.insert(this.myentityPM).subscribe(function (myResult) {
                if (!myResult.HasError) {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    _this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
    };
    HybridPartnerTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './HybridPartnerTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], HybridPartnerTabComponent);
    return HybridPartnerTabComponent;
}(BaseComponent_1.BaseComponent));
exports.HybridPartnerTabComponent = HybridPartnerTabComponent;
//# sourceMappingURL=HybridPartnerTabComponent.js.map