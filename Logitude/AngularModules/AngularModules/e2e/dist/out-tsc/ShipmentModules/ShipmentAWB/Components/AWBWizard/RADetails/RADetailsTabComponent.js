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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../../Shipment/Tools");
var Tools_2 = require("../../../../../Infrastructure/Tools");
var RADetailsTabComponent = /** @class */ (function (_super) {
    __extends(RADetailsTabComponent, _super);
    function RADetailsTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.LabelColumnWidth = 150;
        _this.ControlColumnWidth = 200;
        _this.IsEditingEnabled = false;
        return _this;
    }
    RADetailsTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.SetUIProperties();
    };
    RADetailsTabComponent.prototype.RefreshTab = function () {
    };
    RADetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.SetUIProperties();
                }
            });
            this.Wizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.SetUIProperties();
                }
            });
        }
    };
    RADetailsTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_1.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.UIProperties.SetEnabled('IsKnownCargo', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('RegulatedAgentRANumber', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('KnownConsignorNumber', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('ColoaderRANumber', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('AWBPrintingSecurityStatusId', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('AWBPrintingRANumber', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('AdditionalHandlingInfo', this.ObjectTableName, this.IsEditingEnabled);
    };
    // Regualted Agent Field Changed
    RADetailsTabComponent.prototype.RAFieldChanged = function () {
        Tools_1.ShipmentTool.OnRegulatedAgentFieldChanged(this.EntityPM);
    };
    Object.defineProperty(RADetailsTabComponent.prototype, "TenantZeroAirlineId", {
        get: function () { return this.EntityPM.TenantZeroAirlineId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RADetailsTabComponent.prototype, "IsKnownCargo", {
        // RA Properties
        get: function () { return this.EntityPM.IsKnownCargo; },
        set: function (newValue) {
            if (this.EntityPM.IsKnownCargo != newValue) {
                this.EntityPM.IsKnownCargo = newValue;
                this.RAFieldChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RADetailsTabComponent.prototype, "RegulatedAgentRANumber", {
        get: function () { return this.EntityPM.RegulatedAgentRANumber; },
        set: function (newValue) {
            if (this.EntityPM.RegulatedAgentRANumber != newValue) {
                this.EntityPM.RegulatedAgentRANumber = newValue;
                this.RAFieldChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RADetailsTabComponent.prototype, "KnownConsignorNumber", {
        get: function () { return this.EntityPM.KnownConsignorNumber; },
        set: function (newValue) {
            if (this.EntityPM.KnownConsignorNumber != newValue) {
                this.EntityPM.KnownConsignorNumber = newValue;
                this.RAFieldChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RADetailsTabComponent.prototype, "KCExpirationDate", {
        get: function () { return this.EntityPM.KCExpirationDate; },
        set: function (newValue) {
            if (this.EntityPM.KCExpirationDate != newValue) {
                this.EntityPM.KCExpirationDate = newValue;
                this.RAFieldChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RADetailsTabComponent.prototype, "KCExpirationDateForeground", {
        get: function () {
            var myResult = "#282E30";
            if (this.KCExpirationDate != null) {
                var todayDate = Tools_2.DateTool.GetCurrentDateAsUtc();
                if (Tools_2.DateTool.TruncateTime(this.KCExpirationDate).valueOf() < Tools_2.DateTool.TruncateTime(todayDate).valueOf()) {
                    myResult = "#E53030";
                }
                else if (this.EntityPM.MainCarriageETD != null) {
                    var date = Tools_2.DateTool.AddDays(this.EntityPM.MainCarriageETD, 7);
                    if (Tools_2.DateTool.TruncateTime(this.KCExpirationDate).valueOf() < Tools_2.DateTool.TruncateTime(date).valueOf()) {
                        myResult = "Orange";
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RADetailsTabComponent.prototype, "ColoaderRANumber", {
        get: function () { return this.EntityPM.ColoaderRANumber; },
        set: function (newValue) {
            if (this.EntityPM.ColoaderRANumber != newValue) {
                this.EntityPM.ColoaderRANumber = newValue;
                this.RAFieldChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RADetailsTabComponent.prototype, "AWBPrintingRANumber", {
        // Printing Properties
        get: function () { return this.EntityPM.AWBPrintingRANumber; },
        set: function (newValue) {
            if (this.EntityPM.AWBPrintingRANumber != newValue) {
                this.EntityPM.AWBPrintingRANumber = newValue;
            }
            if (newValue == Tools_1.ShipmentTool.ComputeAWBPrintingRANumber(this.EntityPM)) {
                this.AWBPrintingRANumberEdited = false;
            }
            else {
                this.AWBPrintingRANumberEdited = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RADetailsTabComponent.prototype, "AdditionalHandlingInfo", {
        get: function () { return this.EntityPM.AdditionalHandlingInfo; },
        set: function (newValue) {
            if (this.EntityPM.AdditionalHandlingInfo != newValue) {
                var myOldValue = this.EntityPM.AdditionalHandlingInfo;
                this.EntityPM.AdditionalHandlingInfo = newValue;
                Tools_1.ShipmentTool.OnAdditionalHandlingInfoChanged(this.EntityPM, myOldValue, newValue);
            }
            if (newValue == Tools_1.ShipmentTool.ComputeAdditionalHandlingInfo(this.EntityPM)) {
                this.AdditionalHandlingInfoEdited = false;
            }
            else {
                this.AdditionalHandlingInfoEdited = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RADetailsTabComponent.prototype, "AWBPrintingSecurityStatusId", {
        get: function () { return this.EntityPM.AWBPrintingSecurityStatusId; },
        set: function (newValue) {
            if (this.EntityPM.AWBPrintingSecurityStatusId != newValue) {
                this.EntityPM.AWBPrintingSecurityStatusId = newValue;
                Tools_1.ShipmentTool.OnSecurityCodeChanged(this.EntityPM);
            }
            if (newValue == Tools_1.ShipmentTool.ComputeAWBPrintingSecurityStatus(this.EntityPM)) {
                this.AWBPrintingSecurityStatusEdited = false;
            }
            else {
                this.AWBPrintingSecurityStatusEdited = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RADetailsTabComponent.prototype, "AWBPrintingRANumberEdited", {
        get: function () { return this.EntityPM.AWBPrintingRANumberEdited; },
        set: function (newValue) {
            if (this.EntityPM.AWBPrintingRANumberEdited != newValue) {
                this.EntityPM.AWBPrintingRANumberEdited = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RADetailsTabComponent.prototype, "AdditionalHandlingInfoEdited", {
        get: function () { return this.EntityPM.AdditionalHandlingInfoEdited; },
        set: function (newValue) {
            if (this.EntityPM.AdditionalHandlingInfoEdited != newValue) {
                this.EntityPM.AdditionalHandlingInfoEdited = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RADetailsTabComponent.prototype, "AWBPrintingSecurityStatusEdited", {
        get: function () { return this.EntityPM.AWBPrintingSecurityStatusEdited; },
        set: function (newValue) {
            if (this.EntityPM.AWBPrintingSecurityStatusEdited != newValue) {
                this.EntityPM.AWBPrintingSecurityStatusEdited = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Reset Commands
    RADetailsTabComponent.prototype.ResetAWBPrintingRANumber = function () {
        this.EntityPM.AWBPrintingRANumber = Tools_1.ShipmentTool.ComputeAWBPrintingRANumber(this.EntityPM);
        this.EntityPM.AWBPrintingRANumberEdited = false;
    };
    RADetailsTabComponent.prototype.ResetAdditionalHandling = function () {
        var myOldValue = this.EntityPM.AdditionalHandlingInfo;
        this.EntityPM.AdditionalHandlingInfo = Tools_1.ShipmentTool.ComputeAdditionalHandlingInfo(this.EntityPM);
        this.EntityPM.AdditionalHandlingInfoEdited = false;
        Tools_1.ShipmentTool.OnAdditionalHandlingInfoChanged(this.EntityPM, myOldValue, this.EntityPM.AdditionalHandlingInfo);
    };
    RADetailsTabComponent.prototype.ResetAWBPrintingSecurity = function () {
        this.EntityPM.AWBPrintingSecurityStatusId = Tools_1.ShipmentTool.ComputeAWBPrintingSecurityStatus(this.EntityPM);
        this.EntityPM.AWBPrintingSecurityStatusEdited = false;
        Tools_1.ShipmentTool.OnSecurityCodeChanged(this.EntityPM);
    };
    RADetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'RADetailsTabComponent',
            templateUrl: './RADetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], RADetailsTabComponent);
    return RADetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.RADetailsTabComponent = RADetailsTabComponent;
//# sourceMappingURL=RADetailsTabComponent.js.map