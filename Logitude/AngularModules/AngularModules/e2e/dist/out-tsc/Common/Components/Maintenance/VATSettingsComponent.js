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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TenantPM_1 = require("../../../Common/EntityPMs/TenantPM");
var TenantPMService_1 = require("../../../Common/Services/StandardPMs/TenantPMService");
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var Tools_1 = require("../../../Infrastructure/Tools");
var VatFormatTypeListService_1 = require("../../../Common/Services/StandardLists/VatFormatTypeListService");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var VATSettingsComponent = /** @class */ (function (_super) {
    __extends(VATSettingsComponent, _super);
    function VATSettingsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Tenant";
        _this.QuestionnaireList = [];
        _this.IsVisibile = false;
        _this.IsShowAreaDefaultQuestionnaire = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CurrentSession.StartBusyIndicator("Loading...");
        _this.QuestionnaireList = [];
        _this.tenantPM = new TenantPM_1.TenantPM();
        _this.LoadTenantPMMethod();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "QUESTIONNAIRE")) {
            _this.IsShowAreaDefaultQuestionnaire = true;
        }
        return _this;
    }
    // Load Tenant 
    VATSettingsComponent.prototype.LoadTenantPMMethod = function () {
        var _this = this;
        var myService = new TenantPMService_1.TenantPMService();
        myService.get(SessionLocator_1.SessionLocator.TenantPM.Id).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            if (!response.HasError) {
                _this.tenantPM = response.Result;
                if (Tools_1.AppTool.IsNullOrEmpty(_this.VatFormatTypeCode)) {
                    _this.getVatFormatList();
                }
                else {
                    _this.isAlphaNumeric = !_this.IsNumeric;
                    _this.applyVATForCustomers = !_this.ApplyVATForAllPartners;
                    _this.SetUIProperties();
                    _this.IsVisibile = true;
                }
            }
        });
    };
    VATSettingsComponent.prototype.getVatFormatList = function () {
        var _this = this;
        var service = new VatFormatTypeListService_1.VatFormatTypeListService();
        service.getAllFromCache().subscribe(function (response) {
            if (!response.HasError) {
                var list = response.Result;
                if (list != null) {
                    var noFormatVat = list.filter(function (d) { return d.Code == "NOF"; })[0];
                    if (noFormatVat != null) {
                        _this.VatFormatTypeCode = noFormatVat.Code;
                    }
                }
                _this.isAlphaNumeric = !_this.IsNumeric;
                _this.applyVATForCustomers = !_this.ApplyVATForAllPartners;
                _this.SetUIProperties();
                _this.IsVisibile = true;
            }
        });
    };
    //SetUIProperties
    VATSettingsComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_VatUnique();
        this.SetUIProperties_VatMandatory();
        this.SetUIProperties_VatFormat();
    };
    Object.defineProperty(VATSettingsComponent.prototype, "VatFormatTypeCode", {
        //VAT format by country
        get: function () { return this.tenantPM.VatFormatTypeCode; },
        set: function (value) {
            if (this.tenantPM.VatFormatTypeCode != value) {
                this.tenantPM.VatFormatTypeCode = value;
                this.SetUIProperties_VatFormat();
                this.tenantPM.VatFormatCountryId = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VATSettingsComponent.prototype, "VatFormatCountryId", {
        get: function () { return this.tenantPM.VatFormatCountryId; },
        set: function (value) {
            if (this.tenantPM.VatFormatCountryId != value) {
                this.tenantPM.VatFormatCountryId = value;
                this.SetUIProperties_VatFormat();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VATSettingsComponent.prototype, "IsNumeric", {
        get: function () { return this.tenantPM.IsNumeric; },
        set: function (value) {
            if (this.tenantPM.IsNumeric != value) {
                this.isAlphaNumeric = !value;
                this.tenantPM.IsNumeric = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VATSettingsComponent.prototype, "IsAlphaNumeric", {
        get: function () { return this.isAlphaNumeric; },
        set: function (value) {
            if (this.isAlphaNumeric != value) {
                this.isAlphaNumeric = value;
                this.tenantPM.IsNumeric = !value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VATSettingsComponent.prototype, "VatSize", {
        get: function () { return this.tenantPM.VatSize; },
        set: function (value) {
            if (this.tenantPM.VatSize != value) {
                this.tenantPM.VatSize = value;
                this.SetUIProperties_VatFormat();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VATSettingsComponent.prototype, "IsNumericEnabled", {
        get: function () { return this.isNumericEnabled; },
        set: function (value) {
            if (this.isNumericEnabled != value) {
                this.isNumericEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    VATSettingsComponent.prototype.SetUIProperties_VatFormat = function () {
        this.UIProperties.SetEnabled("CheckDigitControlAlgorithmCode", this.ObjectTableName, this.VatFormatTypeCode == "NOF" ? false : true);
        if (this.VatFormatTypeCode == "FSC") {
            this.UIProperties.SetEnabled("VatFormatCountryId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("VatSize", this.ObjectTableName, true);
            this.IsNumericEnabled = true;
            this.UIProperties.SetRequired("VatFormatCountryId", this.ObjectTableName, false);
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatFormatCountryId)) {
                this.UIProperties.SetRequired("VatFormatCountryId", this.ObjectTableName, true);
            }
        }
        else if (this.VatFormatTypeCode == "FAC") {
            this.UIProperties.SetRequired("VatFormatCountryId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatFormatCountryId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatSize", this.ObjectTableName, true);
            this.IsNumericEnabled = true;
        }
        else {
            this.UIProperties.SetRequired("VatFormatCountryId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatFormatCountryId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatSize", this.ObjectTableName, false);
            this.IsNumericEnabled = false;
        }
        if (this.VatSize != null) {
            if (this.VatSize > 20) {
                this.UIProperties.SetValidity("VatSize", this.ObjectTableName, false, "VAT Size should not exceed 20");
            }
            else {
                this.UIProperties.SetValidity("VatSize", this.ObjectTableName, true, "");
            }
        }
    };
    Object.defineProperty(VATSettingsComponent.prototype, "VatUniqueTypeCode", {
        //VAT# is unique by countr
        get: function () { return this.tenantPM.VatUniqueTypeCode; },
        set: function (value) {
            if (this.tenantPM.VatUniqueTypeCode != value) {
                this.tenantPM.VatUniqueTypeCode = value;
                this.SetUIProperties_VatUnique();
                this.tenantPM.VatUniqueCountryId = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VATSettingsComponent.prototype, "VatUniqueCountryId", {
        get: function () { return this.tenantPM.VatUniqueCountryId; },
        set: function (value) {
            if (this.tenantPM.VatUniqueCountryId != value) {
                this.tenantPM.VatUniqueCountryId = value;
                this.SetUIProperties_VatUnique();
            }
        },
        enumerable: true,
        configurable: true
    });
    VATSettingsComponent.prototype.SetUIProperties_VatUnique = function () {
        var isEnabled = false;
        var isRequired = false;
        if (this.VatUniqueTypeCode == "USC") {
            isEnabled = true;
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatUniqueCountryId)) {
                isRequired = true;
            }
        }
        this.UIProperties.SetEnabled("VatUniqueCountryId", this.ObjectTableName, isEnabled);
        this.UIProperties.SetRequired("VatUniqueCountryId", this.ObjectTableName, isRequired);
    };
    Object.defineProperty(VATSettingsComponent.prototype, "VatMandatoryTypeCode", {
        //VAT# is mandatory for customers
        get: function () { return this.tenantPM.VatMandatoryTypeCode; },
        set: function (value) {
            if (this.tenantPM.VatMandatoryTypeCode != value) {
                this.tenantPM.VatMandatoryTypeCode = value;
                this.SetUIProperties_VatMandatory();
                this.tenantPM.VatMandatoryCountryId = null;
                if (value == "MNT") {
                    this.VatMandatoryForPotentialCustomers = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VATSettingsComponent.prototype, "VatMandatoryCountryId", {
        get: function () { return this.tenantPM.VatMandatoryCountryId; },
        set: function (value) {
            if (this.tenantPM.VatMandatoryCountryId != value) {
                this.tenantPM.VatMandatoryCountryId = value;
                this.SetUIProperties_VatMandatory();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VATSettingsComponent.prototype, "VatMandatoryForPotentialCustomers", {
        get: function () { return this.tenantPM.VatMandatoryForPotentialCustomers; },
        set: function (value) {
            if (this.tenantPM.VatMandatoryForPotentialCustomers != value) {
                this.tenantPM.VatMandatoryForPotentialCustomers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    VATSettingsComponent.prototype.SetUIProperties_VatMandatory = function () {
        var isEnabled = false;
        var isRequired = false;
        if (this.VatMandatoryTypeCode == "MSC") {
            isEnabled = true;
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatMandatoryCountryId)) {
                isRequired = true;
            }
        }
        this.UIProperties.SetEnabled("VatMandatoryCountryId", this.ObjectTableName, isEnabled);
        this.UIProperties.SetRequired("VatMandatoryCountryId", this.ObjectTableName, isRequired);
        this.UIProperties.SetEnabled("VatMandatoryForPotentialCustomers", this.ObjectTableName, this.VatMandatoryTypeCode != "MNT");
    };
    Object.defineProperty(VATSettingsComponent.prototype, "CheckDigitControlAlgorithmCode", {
        get: function () { return this.tenantPM.CheckDigitControlAlgorithmCode; },
        set: function (value) {
            if (this.tenantPM.CheckDigitControlAlgorithmCode != value) {
                this.tenantPM.CheckDigitControlAlgorithmCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands 
    VATSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    VATSettingsComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.tenantPM, this.DataContext.ObjectTableName, errors);
        if (this.VatFormatTypeCode == "FSC") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatFormatCountryId)) {
                errors.push("VAT Format Country is required");
            }
        }
        if (this.VatUniqueTypeCode == "USC") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatUniqueCountryId)) {
                errors.push("VAT Unique Country is required");
            }
        }
        if (this.VatMandatoryTypeCode == "MSC") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.VatMandatoryCountryId)) {
                errors.push("VAT Mandatory Country is required");
            }
        }
        if (this.CheckDigitControlAlgorithmCode == "LUHN") {
            if (!this.IsNumeric) {
                if (this.VatFormatTypeCode != "NOF") {
                    errors.push("Luhn Algorithm cannot be activated on Alpha-Numeric VAT numbers");
                }
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("CompanyDefaults", "Edit");
            this.SubmitTenantChanges();
        }
    };
    VATSettingsComponent.prototype.SubmitTenantChanges = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Saving...");
        var myService = new TenantPMService_1.TenantPMService();
        myService.update(this.tenantPM).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    InfraSettings_1.InfraSettings.TenantPM = _this.tenantPM;
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    VATSettingsComponent.prototype.SetCustomerActivationRadio = function (code) {
        if (code == 'A') {
            this.IsNumeric = false;
        }
        else {
            this.IsAlphaNumeric = false;
        }
    };
    Object.defineProperty(VATSettingsComponent.prototype, "ApplyVATForAllPartners", {
        get: function () { return this.tenantPM.ApplyVATForAllPartners; },
        set: function (value) {
            if (this.tenantPM.ApplyVATForAllPartners != value) {
                this.applyVATForCustomers = !value;
                this.tenantPM.ApplyVATForAllPartners = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VATSettingsComponent.prototype, "ApplyVATForCustomers", {
        get: function () { return this.applyVATForCustomers; },
        set: function (value) {
            if (this.applyVATForCustomers != value) {
                this.applyVATForCustomers = value;
                this.tenantPM.ApplyVATForAllPartners = !value;
            }
        },
        enumerable: true,
        configurable: true
    });
    VATSettingsComponent.prototype.SetApplyVAT = function (code) {
        if (code == 'C') {
            this.ApplyVATForAllPartners = false;
        }
        else {
            this.ApplyVATForCustomers = false;
        }
    };
    VATSettingsComponent = __decorate([
        core_1.Component({
            selector: 'VATSettingsComponent',
            moduleId: module.id,
            templateUrl: './VATSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], VATSettingsComponent);
    return VATSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.VATSettingsComponent = VATSettingsComponent;
//# sourceMappingURL=VATSettingsComponent.js.map