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
var CreditLimitSettingPM_1 = require("../../../EntityPMs/CreditLimitSettingPM");
var CreditLimitSettingPMService_1 = require("../../../Services/StandardPMs/CreditLimitSettingPMService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var CreditLimitSettingsComponent = /** @class */ (function (_super) {
    __extends(CreditLimitSettingsComponent, _super);
    function CreditLimitSettingsComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ObjectTableName = "CreditLimitSetting";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.myService = new CreditLimitSettingPMService_1.CreditLimitSettingPMService();
        return _this;
    }
    CreditLimitSettingsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.myService.get(SessionLocator_1.SessionLocator.Tenant + "").subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    if (myResponse.Result) {
                        _this.EntityPM = myResponse.Result;
                    }
                    else {
                        _this.EntityPM = new CreditLimitSettingPM_1.CreditLimitSettingPM();
                        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        _this.EntityPM.IsDirty = false;
                    }
                    _this.SetUIProperties();
                    _this.IsResourcesReady = true;
                }
            });
        });
    };
    CreditLimitSettingsComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled('InvoiceCreationBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('InvoiceCreationWarning', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('ShipmentCreationBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
    };
    Object.defineProperty(CreditLimitSettingsComponent.prototype, "IsCreditLimitEnabled", {
        get: function () { return this.EntityPM.IsCreditLimitEnabled; },
        set: function (value) {
            if (this.EntityPM.IsCreditLimitEnabled != value) {
                this.EntityPM.IsCreditLimitEnabled = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CreditLimitSettingsComponent.prototype, "InvoiceCreationBlock", {
        get: function () { return this.EntityPM.InvoiceCreationBlock; },
        set: function (value) {
            if (this.EntityPM.InvoiceCreationBlock != value) {
                this.EntityPM.InvoiceCreationBlock = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CreditLimitSettingsComponent.prototype, "InvoiceCreationWarning", {
        get: function () { return this.EntityPM.InvoiceCreationWarning; },
        set: function (value) {
            if (this.EntityPM.InvoiceCreationWarning != value) {
                this.EntityPM.InvoiceCreationWarning = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CreditLimitSettingsComponent.prototype, "ShipmentCreationBlock", {
        get: function () { return this.EntityPM.ShipmentCreationBlock; },
        set: function (value) {
            if (this.EntityPM.ShipmentCreationBlock != value) {
                this.EntityPM.ShipmentCreationBlock = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CreditLimitSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CreditLimitSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (this.EntityPM.IsDirty) {
            this.CurrentSession.StartBusyIndicatorSaving();
            if (this.EntityPM.Id == null) {
                this.myService.insert(this.EntityPM).subscribe(function (myRespone) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!myRespone.HasError) {
                        ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM = _this.EntityPM;
                        _this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                    else {
                        _this.ValidationErrorsList = myRespone.ErrorsArray;
                    }
                });
            }
            else {
                this.myService.update(this.EntityPM).subscribe(function (myRespone) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!myRespone.HasError) {
                        ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM = _this.EntityPM;
                        _this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                    else {
                        _this.ValidationErrorsList = myRespone.ErrorsArray;
                    }
                });
            }
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    CreditLimitSettingsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CreditLimitSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], CreditLimitSettingsComponent);
    return CreditLimitSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.CreditLimitSettingsComponent = CreditLimitSettingsComponent;
//# sourceMappingURL=CreditLimitSettingsComponent.js.map