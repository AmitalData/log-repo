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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TenantPM_1 = require("../../../../Common/EntityPMs/TenantPM");
var AccountingSettingPM_1 = require("../../../../Common/EntityPMs/AccountingSettingPM");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var AccountingSettingPMService_1 = require("../../../../Common/Services/StandardPMs/AccountingSettingPMService");
var TenantPMService_1 = require("../../../../Common/Services/StandardPMs/TenantPMService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectsUpdater_1 = require("../../../../Infrastructure/Locators/ObjectsUpdater");
var InvoiceSettingsComponent = /** @class */ (function (_super) {
    __extends(InvoiceSettingsComponent, _super);
    function InvoiceSettingsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Tenant";
        _this.accountingSettings = new AccountingSettingPM_1.AccountingSettingPM();
        _this.IsVisible = false;
        _this.tenantPM = new TenantPM_1.TenantPM();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isTenantFinished = false;
        _this.isAccountingSettingFinished = false;
        return _this;
    }
    InvoiceSettingsComponent.prototype.ngOnInit = function () {
        this.GetTenant();
        this.GetAccountingSettings();
    };
    InvoiceSettingsComponent.prototype.SetIsVisible = function () {
        if (this.isTenantFinished && this.isAccountingSettingFinished) {
            this.IsVisible = true;
        }
    };
    InvoiceSettingsComponent.prototype.GetTenant = function () {
        var _this = this;
        var tenantService = new TenantPMService_1.TenantPMService();
        tenantService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResult) {
            if (!myResult.HasError) {
                _this.tenantPM = myResult.Result;
                _this.isTenantFinished = true;
                _this.SetIsVisible();
            }
        });
    };
    InvoiceSettingsComponent.prototype.GetAccountingSettings = function () {
        var _this = this;
        var accountingSettingPMService = new AccountingSettingPMService_1.AccountingSettingPMService();
        accountingSettingPMService.get(SessionLocator_1.SessionLocator.AccountingSettingPM.Id).subscribe(function (myResult) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResult.HasError) {
                _this.accountingSettings = myResult.Result;
                _this.isAccountingSettingFinished = true;
                _this.SetIsVisible();
            }
        });
    };
    Object.defineProperty(InvoiceSettingsComponent.prototype, "InvoiceAddress1", {
        get: function () { return this.tenantPM.InvoiceSection1; },
        set: function (value) { this.tenantPM.InvoiceSection1 = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceSettingsComponent.prototype, "InvoiceAddress2", {
        get: function () { return this.tenantPM.InvoiceSection2; },
        set: function (value) { this.tenantPM.InvoiceSection2 = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceSettingsComponent.prototype, "BankDetails", {
        get: function () { return this.tenantPM.BankDetails; },
        set: function (value) { this.tenantPM.BankDetails = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceSettingsComponent.prototype, "Voidinvoice", {
        get: function () {
            return this.accountingSettings.AllowVoidARI;
        },
        set: function (value) {
            this.accountingSettings.AllowVoidARI = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceSettingsComponent.prototype, "AllowManualInvoiceNumber", {
        get: function () {
            return this.accountingSettings.AllowManualInvoiceNumber;
        },
        set: function (value) {
            this.accountingSettings.AllowManualInvoiceNumber = value;
            if (value) {
                this.IsARInvoiceChronologicalDates = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceSettingsComponent.prototype, "IsVatNumberMandatoryInAR", {
        get: function () {
            return this.accountingSettings.IsVatNumberMandatoryInAR;
        },
        set: function (value) {
            this.accountingSettings.IsVatNumberMandatoryInAR = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceSettingsComponent.prototype, "IsARInvoiceChronologicalDates", {
        get: function () {
            return this.accountingSettings.IsARInvoiceChronologicalDates;
        },
        set: function (value) {
            this.accountingSettings.IsARInvoiceChronologicalDates = value;
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    InvoiceSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    InvoiceSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.tenantPM, this.DataContext.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
            var tenantService = new TenantPMService_1.TenantPMService();
            tenantService.update(this.tenantPM).subscribe(function (myResult) {
                if (!myResult.HasError) { // Success
                    _this.UpdateAccountingSettings();
                }
                else {
                    _this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
    };
    InvoiceSettingsComponent.prototype.UpdateAccountingSettings = function () {
        var _this = this;
        var accountingSettingPMService = new AccountingSettingPMService_1.AccountingSettingPMService();
        accountingSettingPMService.update(this.accountingSettings).subscribe(function (myResult) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResult.HasError) { // Success
                ObjectsUpdater_1.ObjectsUpdater.UpdateTenantPM(_this.tenantPM);
                ObjectsUpdater_1.ObjectsUpdater.UpdateAccountingSettingPM(_this.accountingSettings);
                _this.CurrentSession.CloseCurrentWindow();
            }
            else {
                _this.CurrentSession.CloseCurrentWindow();
            }
        });
    };
    InvoiceSettingsComponent = __decorate([
        core_1.Component({
            selector: 'InvoiceSettingsComponent',
            moduleId: module.id,
            templateUrl: './InvoiceSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], InvoiceSettingsComponent);
    return InvoiceSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.InvoiceSettingsComponent = InvoiceSettingsComponent;
//# sourceMappingURL=InvoiceSettingsComponent.js.map