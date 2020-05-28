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
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../../Shipment/Tools");
var FreightChargesTabComponent = /** @class */ (function (_super) {
    __extends(FreightChargesTabComponent, _super);
    function FreightChargesTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.LabelColumnWidth = 150;
        // SetUIProperties
        _this.IsEditingEnabled = false;
        // Validate    
        _this.ShowWarning_AWBCurrencyId = false;
        _this.ShowWarning_RateClassCode = false;
        _this.ShowWarning_AWBChargeRate = false;
        _this.ShowWarning_ChargeableWeight = false;
        _this.ShowWarning_AWBChargeAmount = false;
        _this.ShowWarning_AWBChargesCodeCode = false;
        return _this;
    }
    FreightChargesTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.SetLabels();
        this.Listen();
        this.Validate();
        this.SetUIProperties();
    };
    FreightChargesTabComponent.prototype.RefreshTab = function () {
        this.Validate();
        this.SetUIProperties();
    };
    FreightChargesTabComponent.prototype.Listen = function () {
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
    FreightChargesTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.UIProperties.SetEnabled("AWBCurrencyId", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AWBChargesCodeCode", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AsAgreedFreight", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("RateClassCode", this.ObjectTableName, !this.EntityPM.IsMultipleCommodities && this.IsEditingEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AWBFreightAmountPrepaid", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AWBFreightAmountCollect", this.ObjectTableName, false);
        this.SetRateClassUIProperties();
    };
    FreightChargesTabComponent.prototype.SetRateClassUIProperties = function () {
        if (this.EntityPM.IsMultipleCommodities) {
            this.UIProperties.SetEnabled("AWBChargeRate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AWBChargeAmount", this.ObjectTableName, false);
        }
        else {
            var rateClassGroupCode = Tools_2.ShipmentTool.GetRateClassGroupCode(this.EntityPM.RateClassCode);
            if (rateClassGroupCode == "S") {
                this.AWBChargeRate = null;
                this.UIProperties.SetEnabled("AWBChargeRate", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("AWBChargeAmount", this.ObjectTableName, this.IsEditingEnabled);
            }
            else {
                this.UIProperties.SetEnabled("AWBChargeRate", this.ObjectTableName, this.IsEditingEnabled);
                this.UIProperties.SetEnabled("AWBChargeAmount", this.ObjectTableName, false);
            }
        }
    };
    FreightChargesTabComponent.prototype.SetLabels = function () {
        this.ChargeableWeightLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace('%ChargWeightCode', this.EntityPM.ChargeableWeightUnitCode);
    };
    FreightChargesTabComponent.prototype.FireWizardEvent = function () {
        this.Wizard.ValidateScreen_PAC();
        this.Wizard.ValidateScreen_FRE();
    };
    FreightChargesTabComponent.prototype.Validate = function () {
        if (!this.Wizard.IsImportWizard) {
            var isShowWarning_AWBCurrencyId = false;
            var isShowWarning_RateClassCode = false;
            var isShowWarning_AWBChargeRate = false;
            var isShowWarning_ChargeableWeight = false;
            var isShowWarning_AWBChargeAmount = false;
            var isShowWarning_AWBChargesCodeCode = false;
            if (this.Wizard.IsFWB) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.AWBCurrencyId)) {
                    isShowWarning_AWBCurrencyId = true;
                }
                if (Tools_1.AppTool.IsNullOrEmpty(this.AWBChargesCodeCode)) {
                    isShowWarning_AWBChargesCodeCode = true;
                }
                if (!this.EntityPM.IsMultipleCommodities) {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.RateClassCode)) {
                        isShowWarning_RateClassCode = true;
                    }
                    if (Tools_1.AppTool.IsNullOrZero(this.ChargeableWeight)) {
                        isShowWarning_ChargeableWeight = true;
                    }
                    if (!this.AsAgreedFreight) {
                        var rateClassGroupCode = Tools_2.ShipmentTool.GetRateClassGroupCode(this.RateClassCode);
                        if (rateClassGroupCode != "S") {
                            if (Tools_1.AppTool.IsNullOrZero(this.AWBChargeRate)) {
                                isShowWarning_AWBChargeRate = true;
                            }
                        }
                        if (Tools_1.AppTool.IsNullOrZero(this.AWBChargeAmount)) {
                            isShowWarning_AWBChargeAmount = true;
                        }
                    }
                }
            }
            else {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBChargeRate"; })[0];
                if (!Tools_2.ShipmentTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBChargeRate)) {
                    isShowWarning_AWBChargeRate = true;
                }
            }
            this.ShowWarning_AWBCurrencyId = isShowWarning_AWBCurrencyId;
            this.ShowWarning_RateClassCode = isShowWarning_RateClassCode;
            this.ShowWarning_AWBChargeRate = isShowWarning_AWBChargeRate;
            this.ShowWarning_ChargeableWeight = isShowWarning_ChargeableWeight;
            this.ShowWarning_AWBChargeAmount = isShowWarning_AWBChargeAmount;
            this.ShowWarning_AWBChargesCodeCode = isShowWarning_AWBChargesCodeCode;
        }
    };
    Object.defineProperty(FreightChargesTabComponent.prototype, "AWBCurrencyId", {
        // Properties
        get: function () { return this.EntityPM.AWBCurrencyId; },
        set: function (newValue) {
            if (this.EntityPM.AWBCurrencyId != newValue) {
                this.EntityPM.AWBCurrencyId = newValue;
                this.Validate();
                this.FireWizardEvent();
                this.UpdateOtherCharges();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FreightChargesTabComponent.prototype, "RateClassCode", {
        get: function () { return this.EntityPM.RateClassCode; },
        set: function (newValue) {
            if (this.EntityPM.RateClassCode != newValue) {
                this.EntityPM.RateClassCode = newValue;
                this.Validate();
                this.FireWizardEvent();
                this.ComputeAWBChargeAmount();
                this.SetRateClassUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FreightChargesTabComponent.prototype, "AWBChargeRate", {
        get: function () { return this.EntityPM.AWBChargeRate; },
        set: function (newValue) {
            if (this.EntityPM.AWBChargeRate != newValue) {
                this.EntityPM.AWBChargeRate = Tools_1.AppTool.Round(newValue, 3);
                this.Validate();
                this.FireWizardEvent();
                this.ComputeAWBChargeAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FreightChargesTabComponent.prototype, "AsAgreedFreight", {
        get: function () { return this.EntityPM.AsAgreedFreight; },
        set: function (newValue) {
            if (this.EntityPM.AsAgreedFreight != newValue) {
                this.EntityPM.AsAgreedFreight = newValue;
                this.Validate();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FreightChargesTabComponent.prototype, "ChargeableWeight", {
        get: function () { return Tools_1.AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? 0 : this.EntityPM.ChargeableWeight; },
        set: function (newValue) {
            if (this.EntityPM.ChargeableWeight != newValue) {
                this.EntityPM.ChargeableWeight = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeAWBChargeAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FreightChargesTabComponent.prototype, "AWBChargeAmount", {
        get: function () { return this.EntityPM.AWBChargeAmount; },
        set: function (newValue) {
            if (this.EntityPM.AWBChargeAmount != newValue) {
                this.EntityPM.AWBChargeAmount = Tools_1.AppTool.Round(newValue, 3);
                this.ComputeAWBFrieghtAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FreightChargesTabComponent.prototype, "AWBChargesCodeCode", {
        get: function () { return this.EntityPM.AWBChargesCodeCode; },
        set: function (newValue) {
            if (this.EntityPM.AWBChargesCodeCode != newValue) {
                this.EntityPM.AWBChargesCodeCode = newValue;
                this.Validate();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FreightChargesTabComponent.prototype, "FreightPrepaidCollectId", {
        get: function () { return this.EntityPM.FreightPrepaidCollectId; },
        set: function (newValue) {
            if (this.EntityPM.FreightPrepaidCollectId != newValue) {
                this.EntityPM.FreightPrepaidCollectId = newValue;
                Tools_2.ShipmentTool.BuildAWBChargesCodeCode(this.EntityPM);
                this.ComputeAWBFrieghtAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FreightChargesTabComponent.prototype, "AWBFreightAmountPrepaid", {
        get: function () { return this.EntityPM.AWBFreightAmountPrepaid; },
        set: function (newValue) {
            if (this.EntityPM.AWBFreightAmountPrepaid != newValue) {
                this.EntityPM.AWBFreightAmountPrepaid = newValue;
                this.Validate();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FreightChargesTabComponent.prototype, "AWBFreightAmountCollect", {
        get: function () { return this.EntityPM.AWBFreightAmountCollect; },
        set: function (newValue) {
            if (this.EntityPM.AWBFreightAmountCollect != newValue) {
                this.EntityPM.AWBFreightAmountCollect = newValue;
                this.Validate();
                this.FireWizardEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    FreightChargesTabComponent.prototype.UpdateOtherCharges = function () {
        //foreach(ShipmentPayablePM item in shipmentPM.ShipmentPayables.Where(d => d.ChargesGroupCode != "FRT"))
        //{
        //    if (string.IsNullOrEmpty(item.IATACodeId) || string.IsNullOrEmpty(item.PrepaidCollectId) || string.IsNullOrEmpty(item.DueTypeCode) || item.CurrencyId != AWBCurrencyId) {
        //        item.AWBPrint = false;
        //    }
        //}
        //foreach(ShipmentReceivablePM item in shipmentPM.ShipmentReceivables.Where(d => d.ChargesGroupCode != "FRT"))
        //{
        //    if (string.IsNullOrEmpty(item.IATACodeId) || string.IsNullOrEmpty(item.PrepaidCollectId) || string.IsNullOrEmpty(item.DueTypeCode) || item.CurrencyId != AWBCurrencyId) {
        //        item.AWBPrint = false;
        //    }
        //}
    };
    FreightChargesTabComponent.prototype.ComputeAWBChargeAmount = function () {
        this.AWBChargeAmount = Tools_2.ShipmentTool.ComputeAWBChargeAmount(this.EntityPM.RateClassCode, this.EntityPM.AWBChargeRate, this.EntityPM.ChargeableWeight);
    };
    FreightChargesTabComponent.prototype.ComputeAWBFrieghtAmount = function () {
        var computedAmount = this.AWBChargeAmount;
        var totaAmount = this.AWBFreightAmountPrepaid + this.AWBFreightAmountCollect;
        var recompute = true;
        var isPrepaidHasAmount = (this.AWBFreightAmountPrepaid != 0 && this.AWBFreightAmountPrepaid != null);
        var isCollectHasAmount = (this.AWBFreightAmountCollect != 0 && this.AWBFreightAmountCollect != null);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FreightPrepaidCollectId)) {
            if (isPrepaidHasAmount && isCollectHasAmount && (computedAmount == totaAmount)) {
                recompute = false;
            }
        }
        if (recompute) {
            if (this.FreightPrepaidCollectId == "P") {
                this.AWBFreightAmountCollect = 0;
                this.AWBFreightAmountPrepaid = computedAmount == null ? 0 : computedAmount;
            }
            else if (this.FreightPrepaidCollectId == "C") {
                this.AWBFreightAmountPrepaid = 0;
                this.AWBFreightAmountCollect = computedAmount == null ? 0 : computedAmount;
            }
        }
        this.Validate();
        this.FireWizardEvent();
    };
    FreightChargesTabComponent.prototype.SetFreightPrepaidCollect = function (newValue) {
        this.FreightPrepaidCollectId = newValue;
    };
    FreightChargesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'FreightChargesTabComponent',
            templateUrl: './FreightChargesTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], FreightChargesTabComponent);
    return FreightChargesTabComponent;
}(BaseComponent_1.BaseComponent));
exports.FreightChargesTabComponent = FreightChargesTabComponent;
//# sourceMappingURL=FreightChargesTabComponent.js.map