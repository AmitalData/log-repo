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
var ServiceArgs_1 = require("../../../Infrastructure/DataContracts/ServiceArgs");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FullAccountingSettingPMService_1 = require("../../Services/StandardPMs/FullAccountingSettingPMService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
//import {AutomaticExternalRconcilMthodsPM}  '../../Services/StandardPMs/AutomaticExternalRconcilMthodsPM';
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var FullAccountingSettingsComponent = /** @class */ (function (_super) {
    __extends(FullAccountingSettingsComponent, _super);
    function FullAccountingSettingsComponent(serviceArgs, _entityResourceService, cd) {
        var _this = _super.call(this) || this;
        _this.serviceArgs = serviceArgs;
        _this._entityResourceService = _entityResourceService;
        _this.cd = cd;
        _this.DataContext = _this;
        //public myForm: ControlGroup;
        _this.ObjectTableName = "FullAccountingSetting";
        _this.isRTL = false;
        _this.fullAccountingSettingPMService = new FullAccountingSettingPMService_1.FullAccountingSettingPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region Tabs Code
        _this.TabsSource = [];
        _this.SelectedTab = "";
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.CurrentSession.StartBusyIndicatorLoading();
        _this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName(_this.ObjectTableName, 0).subscribe(function (response) { });
        });
        _this.fullAccountingSettingPMService.get(SessionLocator_1.SessionLocator.Tenant.toString()).subscribe(function (myResult) {
            _this.CurrentSession.StopBusyIndicator();
            _this.EntityPM = myResult.Result;
            if (_this.EntityPM == null || _this.EntityPM == undefined) {
                console.log("There is no F. Accounting setting found for tenant: " + SessionLocator_1.SessionLocator.Tenant);
            }
            else {
                _this.AccountingActivationDate = _this.EntityPM.AccountingActivationDate;
                _this.SetUIProperties();
            }
        });
        _this.UIProperties.SetEnabled("AccountingActivationDate", "Tenant", false);
        return _this;
    }
    ;
    FullAccountingSettingsComponent.prototype.ngOnInit = function () {
        this.BuildTabs();
    };
    FullAccountingSettingsComponent.prototype.ngAfterViewInit = function () {
        //this.SetUIProperties();
    };
    FullAccountingSettingsComponent.prototype.ReloadTenantPM = function () {
        SessionLocator_1.SessionLocator.TenantPM.AccountingActivated = this.AccountingActivated;
    };
    FullAccountingSettingsComponent.prototype.SetUIProperties = function () {
        var enableAllFields = false;
        if (this.AccountingActivated && this.AccountingActivationDate != null) {
            enableAllFields = true;
        }
        this.UIProperties.SetEnabled("DeductionFileNumber", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("ConsolidationVAT", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("DefaultVATTypeId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("PaymentTermId", "Tenant", enableAllFields);
        this.UIProperties.SetEnabled("VATInputsGLAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("VATOutputGLAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("AutomaticReconcileMethodId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("ExchangeRateDiffGLAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("RevenueExpenseGLAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("CustomerControlAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("VendorControlAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("FileControlAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("OceanExportJobControlAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("OceanImportJobControlAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("AirExportJobControlAccountId", this.ObjectTableName, enableAllFields);
        this.UIProperties.SetEnabled("AirImportJobControlAccountId", this.ObjectTableName, enableAllFields);
    };
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "AccountingActivationDate", {
        //#region Full Accounting Setting Properties
        get: function () { return this.EntityPM.AccountingActivationDate; },
        set: function (value) {
            if (this.EntityPM.AccountingActivationDate != value) {
                this.EntityPM.AccountingActivationDate = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "AccountingActivated", {
        get: function () { return this.EntityPM.AccountingActivated; },
        set: function (value) {
            if (this.EntityPM.AccountingActivated != value) {
                this.EntityPM.AccountingActivated = value;
                if (value == true) {
                    this.AccountingActivationDate = new Date();
                }
                else if (value == false) {
                    this.AccountingActivationDate = null;
                }
                this.ReloadTenantPM();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "IsPaymentChequesActivated", {
        get: function () { return this.EntityPM.IsPaymentChequesActivated; },
        set: function (value) {
            if (this.EntityPM.IsPaymentChequesActivated != value) {
                this.EntityPM.IsPaymentChequesActivated = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "DeductionFileNumber", {
        get: function () { return this.EntityPM.DeductionFileNumber; },
        set: function (value) {
            if (this.EntityPM.DeductionFileNumber != value) {
                this.EntityPM.DeductionFileNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "GLAccounterCounterLength", {
        get: function () { return this.EntityPM.GLAccounterCounterLength; },
        set: function (value) {
            if (this.EntityPM.GLAccounterCounterLength != value) {
                this.EntityPM.GLAccounterCounterLength = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "ExternalReconciliationDefault", {
        get: function () { return this.EntityPM.ExternalReconciliationDefault; },
        set: function (value) {
            if (this.EntityPM.ExternalReconciliationDefault != value) {
                this.EntityPM.ExternalReconciliationDefault = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "TaxWithholdingGLAccountId", {
        get: function () { return this.EntityPM.TaxWithholdingGLAccountId; },
        set: function (value) {
            if (this.EntityPM.TaxWithholdingGLAccountId != value) {
                this.EntityPM.TaxWithholdingGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "ConsolidationVAT", {
        get: function () { return this.EntityPM.ConsolidationVAT; },
        set: function (value) {
            if (this.EntityPM.ConsolidationVAT != value) {
                this.EntityPM.ConsolidationVAT = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "DefaultVATTypeId", {
        get: function () { return this.EntityPM.DefaultVATTypeId; },
        set: function (value) {
            if (this.EntityPM.DefaultVATTypeId != value) {
                this.EntityPM.DefaultVATTypeId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "PaymentTermId", {
        get: function () { return this.EntityPM.TenantPaymentTermId; },
        set: function (value) {
            if (this.EntityPM.TenantPaymentTermId != value) {
                this.EntityPM.TenantPaymentTermId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "DefaultTaxWithholdPercentage", {
        get: function () { return this.EntityPM.DefaultTaxWithholdPercentage; },
        set: function (value) {
            if (this.EntityPM.DefaultTaxWithholdPercentage != value) {
                this.EntityPM.DefaultTaxWithholdPercentage = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "VATInputsGLAccountId", {
        get: function () { return this.EntityPM.VATInputsGLAccountId; },
        set: function (value) {
            if (this.EntityPM.VATInputsGLAccountId != value) {
                this.EntityPM.VATInputsGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "VATOutputGLAccountId", {
        get: function () { return this.EntityPM.VATOutputGLAccountId; },
        set: function (value) {
            if (this.EntityPM.VATOutputGLAccountId != value) {
                this.EntityPM.VATOutputGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "AutomaticReconcileMethodId", {
        get: function () { return this.EntityPM.AutomaticReconcileMethodId; },
        set: function (value) {
            if (this.EntityPM.AutomaticReconcileMethodId != value) {
                this.EntityPM.AutomaticReconcileMethodId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "ExchangeRateDiffGLAccountId", {
        get: function () { return this.EntityPM.ExchangeRateDiffGLAccountId; },
        set: function (value) {
            if (this.EntityPM.ExchangeRateDiffGLAccountId != value) {
                this.EntityPM.ExchangeRateDiffGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "RevenueExpenseGLAccountId", {
        get: function () { return this.EntityPM.RevenueExpenseGLAccountId; },
        set: function (value) {
            if (this.EntityPM.RevenueExpenseGLAccountId != value) {
                this.EntityPM.RevenueExpenseGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "CustomerControlAccountId", {
        get: function () { return this.EntityPM.CustomerControlAccountId; },
        set: function (value) {
            if (this.EntityPM.CustomerControlAccountId != value) {
                this.EntityPM.CustomerControlAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "VendorControlAccountId", {
        get: function () { return this.EntityPM.VendorControlAccountId; },
        set: function (value) {
            if (this.EntityPM.VendorControlAccountId != value) {
                this.EntityPM.VendorControlAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "FileControlAccountId", {
        get: function () { return this.EntityPM.FileControlAccountId; },
        set: function (value) {
            if (this.EntityPM.FileControlAccountId != value) {
                this.EntityPM.FileControlAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "OceanExportJobControlAccountId", {
        get: function () { return this.EntityPM.OceanExportJobControlAccountId; },
        set: function (value) {
            if (this.EntityPM.OceanExportJobControlAccountId != value) {
                this.EntityPM.OceanExportJobControlAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "OceanImportJobControlAccountId", {
        get: function () { return this.EntityPM.OceanImportJobControlAccountId; },
        set: function (value) {
            if (this.EntityPM.OceanImportJobControlAccountId != value) {
                this.EntityPM.OceanImportJobControlAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "AirExportJobControlAccountId", {
        get: function () { return this.EntityPM.AirExportJobControlAccountId; },
        set: function (value) {
            if (this.EntityPM.AirExportJobControlAccountId != value) {
                this.EntityPM.AirExportJobControlAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "CustomsGLAccountId", {
        get: function () { return this.EntityPM.CustomsGLAccountId; },
        set: function (value) {
            if (this.EntityPM.CustomsGLAccountId != value) {
                this.EntityPM.CustomsGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "AirImportJobControlAccountId", {
        get: function () { return this.EntityPM.AirImportJobControlAccountId; },
        set: function (value) {
            if (this.EntityPM.AirImportJobControlAccountId != value) {
                this.EntityPM.AirImportJobControlAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "DefaultDifferencesGLAccountId", {
        get: function () { return this.EntityPM.DefaultDifferencesGLAccountId; },
        set: function (value) {
            if (this.EntityPM.DefaultDifferencesGLAccountId != value) {
                this.EntityPM.DefaultDifferencesGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "DefaultExternalDiffGLAccountId", {
        get: function () { return this.EntityPM.DefaultExternalDiffGLAccountId; },
        set: function (value) {
            if (this.EntityPM.DefaultExternalDiffGLAccountId != value) {
                this.EntityPM.DefaultExternalDiffGLAccountId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "RevenueExpenseGLAccount", {
        get: function () { return this.revenueExpenseGLAccount; },
        set: function (value) {
            if (this.revenueExpenseGLAccount != value) {
                this.revenueExpenseGLAccount = value;
                this.ValidateMulticurrencyAccounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "CustomerControlAccount", {
        get: function () { return this.customerControlAccount; },
        set: function (value) {
            if (this.customerControlAccount != value) {
                this.customerControlAccount = value;
                this.ValidateMulticurrencyAccounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "TaxWithholdingGLAccount", {
        get: function () { return this.taxWithholdingGLAccount; },
        set: function (value) {
            if (this.taxWithholdingGLAccount != value) {
                this.taxWithholdingGLAccount = value;
                this.ValidateMulticurrencyAccounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "SoftwareVersion", {
        get: function () { return this.EntityPM.SoftwareVersion; },
        set: function (value) {
            if (this.EntityPM.SoftwareVersion != value) {
                this.EntityPM.SoftwareVersion = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "VendorControlAccount", {
        get: function () { return this.vendorControlAccount; },
        set: function (value) {
            if (this.vendorControlAccount != value) {
                this.vendorControlAccount = value;
                this.ValidateMulticurrencyAccounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "FileControlAccount", {
        get: function () { return this.fileControlAccount; },
        set: function (value) {
            if (this.fileControlAccount != value) {
                this.fileControlAccount = value;
                this.ValidateMulticurrencyAccounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FullAccountingSettingsComponent.prototype, "JobControlAccount", {
        get: function () { return this.jobControlAccount; },
        set: function (value) {
            if (this.jobControlAccount != value) {
                this.jobControlAccount = value;
                this.ValidateMulticurrencyAccounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    //Commands
    FullAccountingSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    FullAccountingSettingsComponent.prototype.OkButtonClicked = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (errors.length == 0)
            this.ValidateMulticurrencyAccounts();
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
            this.SubmitChanges();
        }
    };
    FullAccountingSettingsComponent.prototype.SubmitChanges = function () {
        var _this = this;
        //console.log("EntityPM: ", this.EntityPM);
        this.fullAccountingSettingPMService.update(this.EntityPM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) { // Success
                _this.CurrentSession.CloseCurrentWindow();
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        }, function (error) {
            _this.CurrentSession.StopBusyIndicator();
            var dd = error;
            console.log(dd.text);
            _this.ValidationErrorsList = [];
            _this.ValidationErrorsList.push('Server Error!');
        });
    };
    FullAccountingSettingsComponent.prototype.ValidateMulticurrencyAccounts = function () {
        console.log("ValidateMulticurrencyAccounts");
        this.ValidationErrorsList = [];
        this.UIProperties.SetValidity("FileControlAccountId", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("RevenueExpenseGLAccountId", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("VendorControlAccountId", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("CustomerControlAccountId", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("JobControlAccountId", this.ObjectTableName, true, "");
        if (this.revenueExpenseGLAccount && !this.revenueExpenseGLAccount.IsMultiCurrency) {
            this.ValidationErrorsList.push("Revenue Expense Account must be multi currency");
            this.UIProperties.SetValidity("RevenueExpenseGLAccountId", this.ObjectTableName, false, "Revenue Expense Account must be multi currency");
        }
        if (this.customerControlAccount && !this.customerControlAccount.IsMultiCurrency) {
            this.ValidationErrorsList.push("Customer Control Account must be multi currency");
            this.UIProperties.SetValidity("CustomerControlAccountId", this.ObjectTableName, false, "Customer Control Account must be multi currency");
        }
        if (this.vendorControlAccount && !this.vendorControlAccount.IsMultiCurrency) {
            this.ValidationErrorsList.push("Vendor Control Account must be multi currency");
            this.UIProperties.SetValidity("VendorControlAccountId", this.ObjectTableName, false, "Vendor Control Account must be multi currency");
        }
        if (this.fileControlAccount && !this.fileControlAccount.IsMultiCurrency) {
            this.ValidationErrorsList.push("File Control Account must be multi currency");
            this.UIProperties.SetValidity("FileControlAccountId", this.ObjectTableName, false, "File Control Account must be multi currency");
        }
        if (this.jobControlAccount && !this.jobControlAccount.IsMultiCurrency) {
            this.ValidationErrorsList.push("Job Control Account must be multi currency");
            this.UIProperties.SetValidity("JobControlAccountId", this.ObjectTableName, false, "Job Control Account must be multi currency");
        }
    };
    FullAccountingSettingsComponent.prototype.BuildTabs = function () {
        this.SelectedTab = "FullAccoutingSetting";
        this.TabsSource.push({ Name: "FullAccoutingSetting", isSelected: true, Header: TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.General") }); //Accounting.O.FullAccountingSettings
        this.TabsSource.push({ Name: "ControlAccounts", isSelected: false, Header: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.ControlGLAccounts") });
    };
    FullAccountingSettingsComponent.prototype.SelectionChanged = function (tab) {
        this.TabsSource.forEach(function (item) {
            item.isSelected = false;
        });
        var index = this.TabsSource.indexOf(tab);
        if (index < 0) {
            console.log("The tab was not found, cant not delete it :( ", tab);
            return;
        }
        var item = this.TabsSource[index];
        item.isSelected = true;
        this.SelectedTab = item.Name;
    };
    FullAccountingSettingsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'FullAccountingSettingsComponent',
            templateUrl: './FullAccountingSettingsComponent.html',
            providers: [ServiceArgs_1.ServiceArgs]
        }),
        __metadata("design:paramtypes", [ServiceArgs_1.ServiceArgs, EntityResourceService_1.EntityResourceService, core_1.ChangeDetectorRef])
    ], FullAccountingSettingsComponent);
    return FullAccountingSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.FullAccountingSettingsComponent = FullAccountingSettingsComponent;
//# sourceMappingURL=FullAccountingSettingsComponent.js.map