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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var CustomerBillingTabComponent = /** @class */ (function (_super) {
    __extends(CustomerBillingTabComponent, _super);
    function CustomerBillingTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Customer";
        _this.DataContext = _this;
        _this.HasCreditLimitFeature = false;
        _this.IsCreditLimitActivated = false;
        _this.DisplaySATSettings = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Retries = 0;
        _this.SessionEvent = null;
        _this.isLimitAmountLoaded = false;
        _this.CreditLimitLoadedAmount = null;
        _this.EntityPM = entityArgs.EntityPM;
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        _this.HasCreditLimitFeature = FeatureLocator_1.FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Module");
        if (_this.HasCreditLimitFeature) {
            _this.IsCreditLimitActivated = ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled;
            _this.SetLabels();
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            _this.DisplaySATSettings = true;
        }
        _this.Listen();
        return _this;
    }
    CustomerBillingTabComponent.prototype.ngOnInit = function () {
        this.IsAccountingActivated = SessionLocator_1.SessionLocator.TenantPM.AccountingActivated;
        this.SetUIProperties();
        this.RunComponent();
        this.LoadCreditLimitData();
    };
    CustomerBillingTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    CustomerBillingTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    CustomerBillingTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            _this.GeneratedComponent = cmpRef.instance;
            cmpRef.instance.LoadCompleted.subscribe(function (s) {
                _this.SetUIProperties_GeneratedComponent();
            });
            var screenCode = "Customer.BillingTabScreen";
            cmpRef.instance.Run(_this.EntityPM, _this.ObjectTableName, screenCode);
        });
    };
    CustomerBillingTabComponent.prototype.SetUIProperties_GeneratedComponent = function () {
        if (this.GeneratedComponent) {
            var enabled = true;
            if (SessionLocator_1.SessionLocator.TenantPM.IsHybrid && (this.EntityPM.CustomerStatusCode == "ACT" || this.EntityPM.CustomerStatusCode == "WAC")) {
                enabled = false;
            }
            this.GeneratedComponent.SetEnabled(enabled);
        }
    };
    CustomerBillingTabComponent.prototype.SetLabels = function () {
        this.CreditLimitAmountLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('Customer.F.CreditLimitAmount') + " (" + this.LocalCurrencyCode + ")";
        this.CreditLimitOpenBalanceLabel = TextCodeTranslator_1.TextCodeTranslator.Translate('Customer.F.CreditLimitOpenBalance') + " (" + this.LocalCurrencyCode + ")";
        this.CreditLimitActualBalanceLabel = "Actual Balance" + " (" + this.LocalCurrencyCode + "):";
    };
    CustomerBillingTabComponent.prototype.SetUIProperties = function () {
        var isFieldActivated = false;
        if (this.HasCreditLimitFeature) {
            if (this.IsCreditLimitActivated) {
                if (this.IsCreditLimitEnabled) {
                    isFieldActivated = true;
                }
            }
        }
        this.UIProperties.SetEnabled("IsCreditLimitEnabled", this.ObjectTableName, this.IsCreditLimitActivated);
        this.UIProperties.SetEnabled("CreditLimitAmount", this.ObjectTableName, isFieldActivated);
        this.UIProperties.SetEnabled("CreditLimitOpenBalance", this.ObjectTableName, isFieldActivated);
        this.UIProperties.SetEnabled("CreditLimitWarningPercentage", this.ObjectTableName, isFieldActivated);
        this.UIProperties.SetEnabled("CreditLimitActualBalance", this.ObjectTableName, isFieldActivated);
        this.UIProperties.SetEnabled("BlockNewInvoiceCreation", this.ObjectTableName, isFieldActivated);
        this.UIProperties.SetEnabled("BlockNewShipmentCreation", this.ObjectTableName, isFieldActivated);
        var isLimitAmountRequired = false;
        var isWarningPercentageRequired = false;
        var isWarningPercentageInvalid = false;
        if (isFieldActivated) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.CreditLimitAmount)) {
                isLimitAmountRequired = true;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.CreditLimitWarningPercentage)) {
                isWarningPercentageRequired = true;
            }
            else if (this.EntityPM.CreditLimitWarningPercentage < 0 || this.EntityPM.CreditLimitWarningPercentage > 100) {
                isWarningPercentageInvalid = true;
            }
        }
        this.UIProperties.SetRequired("CreditLimitAmount", this.ObjectTableName, isLimitAmountRequired);
        this.UIProperties.SetRequired("CreditLimitWarningPercentage", this.ObjectTableName, false);
        this.UIProperties.SetValidity("CreditLimitWarningPercentage", this.ObjectTableName, true, null);
        if (isWarningPercentageRequired) {
            this.UIProperties.SetRequired("CreditLimitWarningPercentage", this.ObjectTableName, isWarningPercentageRequired);
        }
        else if (isWarningPercentageInvalid) {
            this.UIProperties.SetValidity("CreditLimitWarningPercentage", this.ObjectTableName, false, "Credit limit warning percentage field must be more than 0 and less than 100");
        }
        this.SetUIProperties_GeneratedComponent();
    };
    CustomerBillingTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "EntityActivated") {
                    _this.SetUIProperties_GeneratedComponent();
                }
            });
        }
    };
    CustomerBillingTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
    };
    CustomerBillingTabComponent.prototype.LoadCreditLimitData = function () {
        var _this = this;
        if (this.HasCreditLimitFeature) {
            if (this.IsCreditLimitActivated) {
                if (this.IsCreditLimitEnabled) {
                    if (!this.isLimitAmountLoaded) {
                        this.isLimitAmountLoaded = true;
                        var myService = new PartnersDomainService_1.PartnersDomainService();
                        myService.GetCustomerCreditLimitActualAmount(this.EntityPM.Id).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                _this.CreditLimitLoadedAmount = myResponse.Result;
                                _this.ComputeActualBalance();
                            }
                        });
                    }
                }
            }
        }
    };
    CustomerBillingTabComponent.prototype.CreditLimitManagementClicked = function () {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = "Credit Limit Settings";
        logitudeWindow.Show('./Common/Components/Maintenance/CreditLimit/CreditLimitSettingsComponent');
        logitudeWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.IsCreditLimitActivated = ObjectsLocator_1.ObjectsLocator.CreditLimitSettingPM.IsCreditLimitEnabled;
                _this.SetUIProperties();
                _this.LoadCreditLimitData();
            }
        });
    };
    Object.defineProperty(CustomerBillingTabComponent.prototype, "IsCreditLimitEnabled", {
        get: function () { return this.EntityPM.IsCreditLimitEnabled; },
        set: function (value) {
            if (this.EntityPM.IsCreditLimitEnabled != value) {
                this.EntityPM.IsCreditLimitEnabled = value;
                this.SetUIProperties();
                this.LoadCreditLimitData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerBillingTabComponent.prototype, "CreditLimitAmount", {
        get: function () { return this.EntityPM.CreditLimitAmount; },
        set: function (value) {
            if (this.EntityPM.CreditLimitAmount != value) {
                this.EntityPM.CreditLimitAmount = Tools_1.AppTool.Round(value, 2);
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerBillingTabComponent.prototype, "CreditLimitOpenBalance", {
        get: function () { return this.EntityPM.CreditLimitOpenBalance; },
        set: function (value) {
            if (this.EntityPM.CreditLimitOpenBalance != value) {
                this.EntityPM.CreditLimitOpenBalance = Tools_1.AppTool.Round(value, 2);
                this.SetUIProperties();
                this.ComputeActualBalance();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerBillingTabComponent.prototype, "CreditLimitWarningPercentage", {
        get: function () { return this.EntityPM.CreditLimitWarningPercentage; },
        set: function (value) {
            if (this.EntityPM.CreditLimitWarningPercentage != value) {
                this.EntityPM.CreditLimitWarningPercentage = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerBillingTabComponent.prototype.ComputeActualBalance = function () {
        this.CreditLimitActualBalance = Tools_1.AppTool.AddAmounts(this.CreditLimitLoadedAmount, this.CreditLimitOpenBalance);
    };
    Object.defineProperty(CustomerBillingTabComponent.prototype, "CreditLimitActualBalance", {
        get: function () { return this.creditLimitActualBalance; },
        set: function (value) {
            if (this.creditLimitActualBalance != value) {
                this.creditLimitActualBalance = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerBillingTabComponent.prototype, "PaymentMethodCode", {
        get: function () { return this.EntityPM.PaymentMethodCode; },
        set: function (newValue) {
            if (this.EntityPM.PaymentMethodCode != newValue) {
                this.EntityPM.PaymentMethodCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerBillingTabComponent.prototype, "MetodoPagoCode", {
        get: function () { return this.EntityPM.MetodoPagoCode; },
        set: function (newValue) {
            if (this.EntityPM.MetodoPagoCode != newValue) {
                this.EntityPM.MetodoPagoCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerBillingTabComponent.prototype, "UsoCFDICode", {
        get: function () { return this.EntityPM.UsoCFDICode; },
        set: function (newValue) {
            if (this.EntityPM.UsoCFDICode != newValue) {
                this.EntityPM.UsoCFDICode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerBillingTabComponent.prototype, "SATForeignRFC", {
        get: function () { return this.EntityPM.SATForeignRFC; },
        set: function (newValue) {
            if (this.EntityPM.SATForeignRFC != newValue) {
                this.EntityPM.SATForeignRFC = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerBillingTabComponent.prototype, "BlockNewInvoiceCreation", {
        get: function () { return this.EntityPM.BlockNewInvoiceCreation; },
        set: function (value) {
            if (this.EntityPM.BlockNewInvoiceCreation != value) {
                this.EntityPM.BlockNewInvoiceCreation = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerBillingTabComponent.prototype, "BlockNewShipmentCreation", {
        get: function () { return this.EntityPM.BlockNewShipmentCreation; },
        set: function (value) {
            if (this.EntityPM.BlockNewShipmentCreation != value) {
                this.EntityPM.BlockNewShipmentCreation = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.ViewChild('BillingChild', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], CustomerBillingTabComponent.prototype, "viewContainerRef", void 0);
    CustomerBillingTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomerBillingTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CustomerBillingTabComponent);
    return CustomerBillingTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerBillingTabComponent = CustomerBillingTabComponent;
//# sourceMappingURL=CustomerBillingTabComponent.js.map