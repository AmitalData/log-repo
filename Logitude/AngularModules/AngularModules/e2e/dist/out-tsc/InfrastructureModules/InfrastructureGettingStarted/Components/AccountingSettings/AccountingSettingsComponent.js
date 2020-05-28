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
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var TenantPMService_1 = require("../../../../Common/Services/StandardPMs/TenantPMService");
var AccountingSettingPMService_1 = require("../../../../Common/Services/StandardPMs/AccountingSettingPMService");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ObjectsUpdater_1 = require("../../../../Infrastructure/Locators/ObjectsUpdater");
var AccountingSettingsComponent = /** @class */ (function (_super) {
    __extends(AccountingSettingsComponent, _super);
    function AccountingSettingsComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "AccountingSetting";
        _this.IsResourcesReady = false;
        _this.IsEnableMultiRateAPInvoicesVisible = false;
        _this.IsEnableMultiCurrencyAPPaymentsVisible = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsDemoTenant = false;
        _this.ShowRealSingleTaxCheckBox = false;
        _this.ShowDummySingleTaxCheckBox = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APInvoice", "EnableMultiRate")) {
            _this.IsEnableMultiRateAPInvoicesVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APPayment", "EnableMultiCurrency")) {
            _this.IsEnableMultiCurrencyAPPaymentsVisible = true;
        }
        _this.InitializeServices();
        _this.LoadData();
        return _this;
    }
    AccountingSettingsComponent.prototype.InitializeServices = function () {
        this.myTenantPMService = new TenantPMService_1.TenantPMService();
        this.myAccountingSettingPMService = new AccountingSettingPMService_1.AccountingSettingPMService();
    };
    AccountingSettingsComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.entityResourceService.getEntityResourceByTableName("AccountingSetting").subscribe(function (res2) {
            _this.myAccountingSettingPMService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result;
                    if (_this.EntityPM) {
                        _this.enableMultiPercentageVATTypes_Old = _this.EntityPM.EnableMultiPercentageVATTypes;
                        _this.IsResourcesReady = true;
                        _this.SetUIProperties();
                    }
                }
            });
        });
    };
    AccountingSettingsComponent.prototype.SetUIProperties = function () {
        var isShowDummySingleTaxCheckBox = false;
        if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
            if (SessionLocator_1.SessionLocator.AccountingSystemPM.IsSingleTaxPerInvoice) {
                isShowDummySingleTaxCheckBox = true;
            }
        }
        this.ShowRealSingleTaxCheckBox = !isShowDummySingleTaxCheckBox;
        this.ShowDummySingleTaxCheckBox = isShowDummySingleTaxCheckBox;
        var isDemoTenant = false;
        if (SessionLocator_1.SessionLocator.Tenant == 65) {
            isDemoTenant = true;
            if (SessionLocator_1.SessionLocator.LoggedUserPM.Email) {
                if (SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                    isDemoTenant = false;
                }
            }
        }
        this.IsDemoTenant = isDemoTenant;
        if (isDemoTenant) {
            this.UIProperties.SetEnabled("AllowVoidARI", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowVoidARP", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsVatNumberMandatoryInAR", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowMinusInvoicelines", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowPositiveAmountsInTheCreditNote", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowManualInvoiceNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsARInvoiceChronologicalDates", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsARPaymentChronologicalDates", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsARInvoicesTransferEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsARPaymentsTransferEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsSingleTaxPerInvoice", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowVoidAPI", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowVoidAPP", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsVatNumberMandatoryInAP", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllowClosureWithoutPayables", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("IsAPInvoicesTransferEnabled", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentTermId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("EnableMultiPercentageVATTypes", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("NotifyPastDateOnInvoiceEdit", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RegistryDateTypeCode", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("IsARInvoiceChronologicalDates", this.ObjectTableName, !this.AllowManualInvoiceNumber);
            var isARInvoicesTransferEnabled = false;
            var isAPInvoicesTransferEnabled = false;
            var isARPaymentsTransferEnabled = false;
            //var isSingleTaxPerInvoiceEnabled = false;
            if (SessionLocator_1.SessionLocator.AccountingSystemPM) {
                isARInvoicesTransferEnabled = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowARInvoicesTransfer;
                isAPInvoicesTransferEnabled = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowAPInvoicesTransfer;
                isARPaymentsTransferEnabled = SessionLocator_1.SessionLocator.AccountingSystemPM.AllowARPaymentsTransfer;
                //isSingleTaxPerInvoiceEnabled = !SessionLocator.AccountingSystemPM.IsSingleTaxPerInvoice;
            }
            this.UIProperties.SetEnabled("IsARInvoicesTransferEnabled", this.ObjectTableName, isARInvoicesTransferEnabled);
            this.UIProperties.SetEnabled("IsAPInvoicesTransferEnabled", this.ObjectTableName, isAPInvoicesTransferEnabled);
            this.UIProperties.SetEnabled("IsARPaymentsTransferEnabled", this.ObjectTableName, isARPaymentsTransferEnabled);
            //this.UIProperties.SetEnabled("IsSingleTaxPerInvoice", this.ObjectTableName, isSingleTaxPerInvoiceEnabled);
            if (SessionLocator_1.SessionLocator.TenantPM.CountryCode == "IL" && SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare == false) {
                this.UIProperties.SetEnabled("AllowVoidARI", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("AllowVoidARP", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("IsVatNumberMandatoryInAR", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("AllowManualInvoiceNumber", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("IsARInvoiceChronologicalDates", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("IsARPaymentChronologicalDates", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("AllowVoidAPI", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("AllowVoidAPP", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("IsVatNumberMandatoryInAP", this.ObjectTableName, false);
            }
        }
        this.UIProperties.SetEnabled("EnableInvoiceStocksManagement", this.ObjectTableName, !this.IsARInvoiceChronologicalDates);
        this.SetUIProperties_RegistryDate();
    };
    AccountingSettingsComponent.prototype.SetUIProperties_RegistryDate = function () {
        var isEnabled = false;
        if (SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
            isEnabled = true;
        }
        else if (FeatureLocator_1.FeatureLocator.IsPackage_DVMT()) {
            isEnabled = true;
        }
        this.UIProperties.SetEnabled("RegistryDateTypeCode", this.ObjectTableName, isEnabled);
    };
    Object.defineProperty(AccountingSettingsComponent.prototype, "AllowVoidARI", {
        // Accounting Receivable
        get: function () { return this.EntityPM.AllowVoidARI; },
        set: function (value) {
            if (this.EntityPM.AllowVoidARI != value) {
                this.EntityPM.AllowVoidARI = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "AllowVoidARP", {
        get: function () { return this.EntityPM.AllowVoidARP; },
        set: function (value) {
            if (this.EntityPM.AllowVoidARP != value) {
                this.EntityPM.AllowVoidARP = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "IsVatNumberMandatoryInAR", {
        get: function () { return this.EntityPM.IsVatNumberMandatoryInAR; },
        set: function (value) {
            if (this.EntityPM.IsVatNumberMandatoryInAR != value) {
                this.EntityPM.IsVatNumberMandatoryInAR = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "AllowMinusInvoicelines", {
        get: function () { return this.EntityPM.AllowMinusInvoicelines; },
        set: function (value) {
            if (this.EntityPM.AllowMinusInvoicelines != value) {
                this.EntityPM.AllowMinusInvoicelines = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "AllowPositiveAmountsInTheCreditNote", {
        get: function () { return this.EntityPM.AllowPositiveAmountsInTheCreditNote; },
        set: function (value) {
            if (this.EntityPM.AllowPositiveAmountsInTheCreditNote != value) {
                this.EntityPM.AllowPositiveAmountsInTheCreditNote = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "AllowManualInvoiceNumber", {
        get: function () { return this.EntityPM.AllowManualInvoiceNumber; },
        set: function (value) {
            if (this.EntityPM.AllowManualInvoiceNumber != value) {
                this.EntityPM.AllowManualInvoiceNumber = value;
                if (value) {
                    this.IsARInvoiceChronologicalDates = false;
                }
                this.UIProperties.SetEnabled("IsARInvoiceChronologicalDates", this.ObjectTableName, !this.AllowManualInvoiceNumber);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "IsARInvoiceChronologicalDates", {
        get: function () { return this.EntityPM.IsARInvoiceChronologicalDates; },
        set: function (value) {
            if (this.EntityPM.IsARInvoiceChronologicalDates != value) {
                this.EntityPM.IsARInvoiceChronologicalDates = value;
                if (value) {
                    this.EnableInvoiceStocksManagement = false;
                }
                this.UIProperties.SetEnabled("EnableInvoiceStocksManagement", this.ObjectTableName, !value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "IsARPaymentChronologicalDates", {
        get: function () { return this.EntityPM.IsARPaymentChronologicalDates; },
        set: function (value) {
            if (this.EntityPM.IsARPaymentChronologicalDates != value) {
                this.EntityPM.IsARPaymentChronologicalDates = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "IsARInvoicesTransferEnabled", {
        get: function () { return this.EntityPM.IsARInvoicesTransferEnabled; },
        set: function (value) {
            if (this.EntityPM.IsARInvoicesTransferEnabled != value) {
                this.EntityPM.IsARInvoicesTransferEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "IsARPaymentsTransferEnabled", {
        get: function () { return this.EntityPM.IsARPaymentsTransferEnabled; },
        set: function (value) {
            if (this.EntityPM.IsARPaymentsTransferEnabled != value) {
                this.EntityPM.IsARPaymentsTransferEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "IsSingleTaxPerInvoice", {
        get: function () { return this.EntityPM.IsSingleTaxPerInvoice; },
        set: function (value) {
            if (this.EntityPM.IsSingleTaxPerInvoice != value) {
                this.EntityPM.IsSingleTaxPerInvoice = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "AllowVoidAPI", {
        // Accounting Payables
        get: function () { return this.EntityPM.AllowVoidAPI; },
        set: function (value) {
            if (this.EntityPM.AllowVoidAPI != value) {
                this.EntityPM.AllowVoidAPI = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "AllowVoidAPP", {
        get: function () { return this.EntityPM.AllowVoidAPP; },
        set: function (value) {
            if (this.EntityPM.AllowVoidAPP != value) {
                this.EntityPM.AllowVoidAPP = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "IsVatNumberMandatoryInAP", {
        get: function () { return this.EntityPM.IsVatNumberMandatoryInAP; },
        set: function (value) {
            if (this.EntityPM.IsVatNumberMandatoryInAP != value) {
                this.EntityPM.IsVatNumberMandatoryInAP = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "AllowClosureWithoutPayables", {
        get: function () { return this.EntityPM.AllowClosureWithoutPayables; },
        set: function (value) {
            if (this.EntityPM.AllowClosureWithoutPayables != value) {
                this.EntityPM.AllowClosureWithoutPayables = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "IsAPInvoicesTransferEnabled", {
        get: function () { return this.EntityPM.IsAPInvoicesTransferEnabled; },
        set: function (value) {
            if (this.EntityPM.IsAPInvoicesTransferEnabled != value) {
                this.EntityPM.IsAPInvoicesTransferEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "EnableMultiRateAPInvoices", {
        get: function () { return this.EntityPM.EnableMultiRateAPInvoices; },
        set: function (value) {
            if (this.EntityPM.EnableMultiRateAPInvoices != value) {
                this.EntityPM.EnableMultiRateAPInvoices = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "VatNumber", {
        // Tenant Properties
        get: function () { return this.EntityPM.VatNumber; },
        set: function (value) {
            if (this.EntityPM.VatNumber != value) {
                this.EntityPM.VatNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "PaymentTermId", {
        get: function () { return this.EntityPM.PaymentTermId; },
        set: function (value) {
            if (this.EntityPM.PaymentTermId != value) {
                this.EntityPM.PaymentTermId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "EnableMultiPercentageVATTypes", {
        get: function () { return this.EntityPM.EnableMultiPercentageVATTypes; },
        set: function (value) {
            if (this.EntityPM.EnableMultiPercentageVATTypes != value) {
                this.EntityPM.EnableMultiPercentageVATTypes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "NotifyPastDateOnInvoiceEdit", {
        get: function () { return this.EntityPM.NotifyPastDateOnInvoiceEdit; },
        set: function (value) {
            if (this.EntityPM.NotifyPastDateOnInvoiceEdit != value) {
                this.EntityPM.NotifyPastDateOnInvoiceEdit = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "RegistryDateTypeCode", {
        get: function () { return this.EntityPM.RegistryDateTypeCode; },
        set: function (value) {
            if (this.EntityPM.RegistryDateTypeCode != value) {
                this.EntityPM.RegistryDateTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "EnableMultiCurrencyARPayments", {
        get: function () { return this.EntityPM.EnableMultiCurrencyARPayments; },
        set: function (value) {
            if (this.EntityPM.EnableMultiCurrencyARPayments != value) {
                this.EntityPM.EnableMultiCurrencyARPayments = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "EnableMultiCurrencyAPPayments", {
        get: function () { return this.EntityPM.EnableMultiCurrencyAPPayments; },
        set: function (value) {
            if (this.EntityPM.EnableMultiCurrencyAPPayments != value) {
                this.EntityPM.EnableMultiCurrencyAPPayments = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "EnableNegativeOffsetARPayments", {
        get: function () { return this.EntityPM.EnableNegativeOffsetARPayments; },
        set: function (value) {
            if (this.EntityPM.EnableNegativeOffsetARPayments != value) {
                this.EntityPM.EnableNegativeOffsetARPayments = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "EnableNegativeOffsetAPPayments", {
        get: function () { return this.EntityPM.EnableNegativeOffsetAPPayments; },
        set: function (value) {
            if (this.EntityPM.EnableNegativeOffsetAPPayments != value) {
                this.EntityPM.EnableNegativeOffsetAPPayments = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccountingSettingsComponent.prototype, "EnableInvoiceStocksManagement", {
        get: function () { return this.EntityPM.EnableInvoiceStocksManagement; },
        set: function (value) {
            if (this.EntityPM.EnableInvoiceStocksManagement != value) {
                this.EntityPM.EnableInvoiceStocksManagement = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //Commands 
    AccountingSettingsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AccountingSettingsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (this.EntityPM.IsDirty) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var isLoadingVatGroups = false;
            if (this.EnableMultiPercentageVATTypes != this.enableMultiPercentageVATTypes_Old) {
                if (this.EnableMultiPercentageVATTypes) {
                    isLoadingVatGroups = true;
                }
            }
            this.myAccountingSettingPMService.update(this.EntityPM).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    _this.EntityPM = myResponse.Result;
                    ObjectsUpdater_1.ObjectsUpdater.UpdateAccountingSettingPM(_this.EntityPM);
                    _this.myTenantPMService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse1) {
                        if (!myResponse1.HasError) {
                            InfraSettings_1.InfraSettings.TenantPM = myResponse1.Result;
                        }
                        if (isLoadingVatGroups) {
                            var myCommonDomain = new CommonDomainService_1.CommonDomainService();
                            myCommonDomain.GetAllVatTypesGroups().subscribe(function (myResponse) {
                                if (!myResponse.HasError) {
                                    SessionLocator_1.SessionLocator.AllVatTypesGroups = myResponse.Result;
                                }
                                _this.CurrentSession.CloseCurrentWindowEmit("OK");
                            });
                        }
                        else {
                            SessionLocator_1.SessionLocator.AllVatTypesGroups = [];
                            _this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                    });
                }
            });
        }
        else {
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    AccountingSettingsComponent.prototype.ViewAdvancedSettings = function () {
        var windowTitle = "Advanced Accounting Settings ";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Title = windowTitle;
        logWindow.DataContext = this;
        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/AccountingSettings/AccountingAdvancedSettingsComponent');
    };
    AccountingSettingsComponent.prototype.ManageStocksClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.IsFillScreen_90 = true;
        logWindow.IsShowCloseButton = true;
        logWindow.Title = "Invoice Stocks";
        logWindow.Show('./InvoiceModules/InvoiceStocks/Components/ManageStocksComponent');
    };
    AccountingSettingsComponent = __decorate([
        core_1.Component({
            selector: 'AccountingSettingsComponent',
            moduleId: module.id,
            templateUrl: './AccountingSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], AccountingSettingsComponent);
    return AccountingSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.AccountingSettingsComponent = AccountingSettingsComponent;
//# sourceMappingURL=AccountingSettingsComponent.js.map