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
var Tools_1 = require("../../../Tools");
var Tools_2 = require("../../../../Infrastructure/Tools");
var GeneralDetailsTabComponent = /** @class */ (function (_super) {
    __extends(GeneralDetailsTabComponent, _super);
    function GeneralDetailsTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsBookingProductVisibile = false;
        _this.ShowWarning_BookingProduct = false;
        _this.ShowWarning_TarrifReference = false;
        _this.ShowWarning_AWBSpecialHandlingCodes = false;
        _this.ShowWarning_SpecialServicesRequest = false;
        _this.ShowWarning_OtherServicesInformation = false;
        return _this;
    }
    GeneralDetailsTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.SetUIProperties();
        this.Validate();
    };
    GeneralDetailsTabComponent.prototype.RefreshTab = function () {
        this.SetUIProperties();
        this.Validate();
    };
    GeneralDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.RefreshTab();
                }
            });
            this.Wizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.RefreshTab();
                }
            });
        }
    };
    GeneralDetailsTabComponent.prototype.SetUIProperties = function () {
        this.isEditingEnabled_Others = Tools_1.BookingTool.IsEditingFieldsEnabled_Others(this.EntityPM);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId1", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId2", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId3", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId4", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId5", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId6", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId7", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId8", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBSpecialHandlingCodeId9", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("SpecialServicesRequest", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("OtherServicesInformation", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.isEditingEnabled_Others);
        this.UIProperties.SetEnabled("AWBCarrierTarrifReference", this.ObjectTableName, this.isEditingEnabled_Others);
        this.SetUIProperties_Product();
    };
    GeneralDetailsTabComponent.prototype.SetUIProperties_Product = function () {
        if (this.EntityPM.TenantZeroIsManagingProduct) {
            this.UIProperties.SetVisibility("BookingProductId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("BookingProductId", this.ObjectTableName, this.isEditingEnabled_Others);
            this.IsBookingProductVisibile = true;
        }
        else {
            this.UIProperties.SetVisibility("BookingProductId", this.ObjectTableName, false);
            this.IsBookingProductVisibile = false;
        }
    };
    GeneralDetailsTabComponent.prototype.FireWizardEvent = function () {
        this.Wizard.ValidateScreen_GEN();
    };
    GeneralDetailsTabComponent.prototype.Validate = function () {
        this.Validate_BookingProduct();
        this.Validate_AWBCarrierTarrifReference();
        this.Validate_AWBSpecialHandlingCodes();
        this.Validate_SpecialServicesRequest();
        this.Validate_OtherServicesInformation();
    };
    GeneralDetailsTabComponent.prototype.Validate_BookingProduct = function () {
        if (this.EntityPM.TenantZeroIsProductMandatory) {
            if (Tools_2.AppTool.IsNullOrEmpty(this.BookingProductId)) {
                this.ShowWarning_BookingProduct = true;
            }
            else {
                this.ShowWarning_BookingProduct = false;
            }
        }
        else {
            this.ShowWarning_BookingProduct = false;
        }
    };
    GeneralDetailsTabComponent.prototype.Validate_AWBCarrierTarrifReference = function () {
        var isValid = true;
        var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBCarrierTarrifReference"; })[0];
        if (!Tools_2.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBCarrierTarrifReference)) {
            isValid = false;
        }
        this.ShowWarning_TarrifReference = !isValid;
    };
    GeneralDetailsTabComponent.prototype.Validate_AWBSpecialHandlingCodes = function () {
        var isValid = true;
        if (this.EntityPM.IsTemperatureSensitive) {
            if (Tools_2.AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId1) && Tools_2.AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId2) && Tools_2.AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId3)
                && Tools_2.AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId4) && Tools_2.AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId5) && Tools_2.AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId6)
                && Tools_2.AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId7) && Tools_2.AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId8) && Tools_2.AppTool.IsNullOrEmpty(this.AWBSpecialHandlingCodeId9)) {
                isValid = false;
            }
        }
        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId1"; })[0];
            if (!Tools_2.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId1)) {
                isValid = false;
            }
        }
        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId2"; })[0];
            if (!Tools_2.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId2)) {
                isValid = false;
            }
        }
        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId3"; })[0];
            if (!Tools_2.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId3)) {
                isValid = false;
            }
        }
        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId4"; })[0];
            if (!Tools_2.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId4)) {
                isValid = false;
            }
        }
        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId5"; })[0];
            if (!Tools_2.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId5)) {
                isValid = false;
            }
        }
        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId6"; })[0];
            if (!Tools_2.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId6)) {
                isValid = false;
            }
        }
        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId7"; })[0];
            if (!Tools_2.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId7)) {
                isValid = false;
            }
        }
        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId8"; })[0];
            if (!Tools_2.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId8)) {
                isValid = false;
            }
        }
        if (isValid) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "AWBSpecialHandlingCodeId9"; })[0];
            if (!Tools_2.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBSpecialHandlingCodeId9)) {
                isValid = false;
            }
        }
        this.ShowWarning_AWBSpecialHandlingCodes = !isValid;
    };
    GeneralDetailsTabComponent.prototype.Validate_SpecialServicesRequest = function () {
        var isValid = true;
        var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "SpecialServicesRequest"; })[0];
        if (!Tools_2.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.SpecialServicesRequest)) {
            isValid = false;
        }
        this.ShowWarning_SpecialServicesRequest = !isValid;
    };
    GeneralDetailsTabComponent.prototype.Validate_OtherServicesInformation = function () {
        var isValid = true;
        var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "OtherServicesInformation"; })[0];
        if (!Tools_2.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.OtherServicesInformation)) {
            isValid = false;
        }
        this.ShowWarning_OtherServicesInformation = !isValid;
    };
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "TenantZeroAirlineId", {
        // Properties
        get: function () { return this.EntityPM.TenantZeroAirlineId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "TenantZeroIsManagingProduct", {
        get: function () { return this.EntityPM.TenantZeroIsManagingProduct; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "BookingProductId", {
        get: function () { return this.EntityPM.BookingProductId; },
        set: function (newValue) {
            if (this.EntityPM.BookingProductId != newValue) {
                this.EntityPM.BookingProductId = newValue;
                this.FireWizardEvent();
                this.Validate_BookingProduct();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBCarrierTarrifReference", {
        get: function () { return this.EntityPM.AWBCarrierTarrifReference; },
        set: function (newValue) {
            if (this.EntityPM.AWBCarrierTarrifReference != newValue) {
                this.EntityPM.AWBCarrierTarrifReference = newValue;
                this.FireWizardEvent();
                this.Validate_AWBCarrierTarrifReference();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "SpecialServicesRequest", {
        get: function () { return this.EntityPM.SpecialServicesRequest; },
        set: function (newValue) {
            if (this.EntityPM.SpecialServicesRequest != newValue) {
                this.EntityPM.SpecialServicesRequest = newValue;
                this.FireWizardEvent();
                this.Validate_SpecialServicesRequest();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "OtherServicesInformation", {
        get: function () { return this.EntityPM.OtherServicesInformation; },
        set: function (newValue) {
            if (this.EntityPM.OtherServicesInformation != newValue) {
                this.EntityPM.OtherServicesInformation = newValue;
                this.FireWizardEvent();
                this.Validate_OtherServicesInformation();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId1", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId1; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId1 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId1 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId2", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId2; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId2 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId2 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId3", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId3; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId3 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId3 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId4", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId4; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId4 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId4 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId5", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId5; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId5 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId5 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId6", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId6; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId6 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId6 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId7", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId7; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId7 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId7 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId8", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId8; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId8 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId8 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GeneralDetailsTabComponent.prototype, "AWBSpecialHandlingCodeId9", {
        get: function () { return this.EntityPM.AWBSpecialHandlingCodeId9; },
        set: function (newValue) {
            if (this.EntityPM.AWBSpecialHandlingCodeId9 != newValue) {
                this.EntityPM.AWBSpecialHandlingCodeId9 = newValue;
                this.FireWizardEvent();
                this.Validate_AWBSpecialHandlingCodes();
            }
        },
        enumerable: true,
        configurable: true
    });
    GeneralDetailsTabComponent = __decorate([
        core_1.Component({
            selector: 'GeneralDetailsTabComponent',
            moduleId: module.id,
            templateUrl: './GeneralDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], GeneralDetailsTabComponent);
    return GeneralDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.GeneralDetailsTabComponent = GeneralDetailsTabComponent;
//# sourceMappingURL=GeneralDetailsTabComponent.js.map