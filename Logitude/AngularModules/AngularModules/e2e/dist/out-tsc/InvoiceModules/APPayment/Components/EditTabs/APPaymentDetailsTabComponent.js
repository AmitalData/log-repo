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
var Tools_1 = require("../../../../Infrastructure/Tools");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var AddressListService_1 = require("../../../../Common/Services/StandardLists/AddressListService");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var APInvoiceListService_1 = require("../../../../Invoice/Services/StandardLists/APInvoiceListService");
var APPaymentInvoicePM_1 = require("../../../../Invoice/EntityPMs/APPaymentInvoicePM");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var AccountingPaymentMethodListService_1 = require("../../../../Invoice/Services/StandardLists/AccountingPaymentMethodListService");
var GLAccountWithholdingTaxExtendedService_1 = require("../../../../Accounting/Services/Others/GLAccountWithholdingTaxExtendedService");
var BankAccountPMService_1 = require("../../../../Accounting/Services/StandardPMs/BankAccountPMService");
var FullAccountingSettingPM_1 = require("../../../../Accounting/EntityPMs/FullAccountingSettingPM");
var FullAccountingSettingPMService_1 = require("../../../../Accounting/Services/StandardPMs/FullAccountingSettingPMService");
var PaymentChequeExtendedPMService_1 = require("../../../../Accounting/Services/ExtendedPMs/PaymentChequeExtendedPMService");
var GLAccountListService_1 = require("../../../../Accounting/Services/StandardLists/GLAccountListService");
var APPaymentDetailsTabComponent = /** @class */ (function (_super) {
    __extends(APPaymentDetailsTabComponent, _super);
    function APPaymentDetailsTabComponent(entityArgs, _entityResourceService, cd) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._entityResourceService = _entityResourceService;
        _this.cd = cd;
        _this.EntityPM = null;
        _this.ObjectTableName = "APPayment";
        _this.DataContext = _this;
        _this.APPaymentsDetailsService = new APInvoiceListService_1.APInvoiceListService();
        _this.EnableNegativeOffsetAPPayments = false;
        _this.IsEditExchangeRateVisible = false;
        _this.isRTL = false;
        _this.IsFullAccounting = false;
        _this.IsNoVendorTax = false;
        _this.PaymentChequeActivated = true;
        _this.IsMultiCurrency = false;
        _this.LocalCurrencyCode = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.fullAccountingSettingPMService = new FullAccountingSettingPMService_1.FullAccountingSettingPMService();
        _this.PaymentChequePMService = new PaymentChequeExtendedPMService_1.PaymentChequeExtendedPMService();
        _this.IsChequeLinkVisibile = false;
        _this.ShowSplitButton = false;
        _this.SplitTooltip = "";
        _this.FullAccountingSetting = new FullAccountingSettingPM_1.FullAccountingSettingPM();
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsTaxUpdated = false;
        _this._GLAccountListService = new GLAccountListService_1.GLAccountListService();
        _this.RateIsEnabled = false;
        _this.IsSplitButtonVisibile = false;
        //LoadCurrencyRates
        _this.LastRatesList = [];
        // Load Data
        _this.IsDataLoaded = false;
        _this.searchText = "";
        _this.ConnectedList = [];
        _this.IsMatchedList = [];
        _this.myRelativeRateDate = null;
        //Payment Line Properties
        _this.PaymentMethodAddedManually = false;
        _this.BankAccountIdVisibility = false;
        _this.CreditCardTypeIdVisibility = false;
        _this.AllCurrencies = [];
        _this.AllMethods = [];
        // Summary
        _this.Summary_Amount = 0;
        _this.Summary_AmountPaid = 0;
        _this.Summary_AmountPaidColor = "#282E30";
        _this.RequestedCommandCode = null;
        _this.RequestedCommandParam = null;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting) {
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
            _this.IsSplitComponentOpened = _this.isRTL;
        }
        _this.IsFullAccounting = SessionLocator_1.SessionLocator.TenantPM.AccountingActivated;
        _this.EntityPM = entityArgs.EntityPM;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.EnableNegativeOffsetAPPayments = ObjectsLocator_1.ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetAPPayments;
        _this.GetFullAccountingSettings();
        if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.ChequeOrPaymentRef) && _this.EntityPM.AutomaticPaymentCheque) {
            _this.IsChequeLinkVisibile = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(_this.ObjectTableName, "EnableMultiCurrency")) {
            if (ObjectsLocator_1.ObjectsLocator.AccountingSettingPM.EnableMultiCurrencyAPPayments) {
                _this.IsMultiCurrency = true;
            }
        }
        _this.InitializeServices();
        _this.ApplyViewModel();
        _this.Listen();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            _this.IsEditExchangeRateVisible = true;
        }
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        return _this;
    }
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "IsNegativeAmountEnabled", {
        get: function () { return this.EnableNegativeOffsetAPPayments == true && this.PaymentMethodCode == "FS" ? true : false; },
        enumerable: true,
        configurable: true
    });
    APPaymentDetailsTabComponent.prototype.SplitButtonClicked = function () {
        this.IsSplitComponentOpened = !this.IsSplitComponentOpened;
        if (!this.IsSplitComponentOpened) {
            this.AutomaticPaymentCheque = !this.isRTL;
            this.PaymentChequeActivated = !this.isRTL;
            this.SplitTooltip = this.isRTL ? TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.PaymentChequeAutomaticOption") : "";
            if (Tools_1.AppTool.IsNullOrEmpty(this.ChequeOrPaymentRef)) {
                this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, this.isRTL);
            }
        }
        else {
            this.PaymentChequeActivated = this.isRTL;
            this.AutomaticPaymentCheque = this.isRTL;
            this.SplitTooltip = this.isRTL ? "" : TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.PaymentChequeAutomaticOption");
            this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, !this.isRTL);
            this.ChequeOrPaymentRef = null;
        }
        this.ShowSplitButton = true;
        // this.cd.detectChanges();
    };
    APPaymentDetailsTabComponent.prototype.ViewPaymentCheque = function () {
        var _this = this;
        this.PaymentChequePMService.getPaymentChequeByChequeNumber(this.EntityPM.ChequeOrPaymentRef).subscribe(function (myResult) {
            var myResponse = myResult;
            if (myResponse != null) {
                var res = myResponse.Result;
                var entityId = res.Id;
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: "PaymentCheque", BackButtonLabel: "PaymentCheque" });
                    cmpRef.instance.BackCompleted.subscribe(function ($event) {
                        _this.BuildScreenData();
                    });
                });
            }
        });
    };
    APPaymentDetailsTabComponent.prototype.GetFullAccountingSettings = function () {
        var _this = this;
        this.fullAccountingSettingPMService.get(SessionLocator_1.SessionLocator.TenantPM.Id.toString()).subscribe(function (myResult) {
            var myResponse = myResult;
            if (myResponse != null) {
                var res = myResponse.Result;
                _this.FullAccountingSetting = res;
                _this.PaymentChequeActivated = _this.FullAccountingSetting.IsPaymentChequesActivated && _this.IsFullAccounting;
            }
        });
    };
    APPaymentDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.RefreshScreen();
                }
                if (_this.RequestedCommandCode) {
                    _this.ApplyRequestedCommand();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.RefreshScreen();
                }
            });
        }
    };
    APPaymentDetailsTabComponent.prototype.ngOnInit = function () {
        this.LoadPaymentMethods();
        this.LoadCurrencies();
    };
    APPaymentDetailsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    APPaymentDetailsTabComponent.prototype.LoadTaxPercentage = function () {
        var _this = this;
        if (this.IsFullAccounting) {
            this.GLAccountWithholdingService.GetDeductionPercentage(this.VendorId, this.EntityPM.RegisterDate).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id) || _this.IsTaxUpdated) {
                        _this.IsNoVendorTax = myResponse.Result.IsDefault;
                        _this.TaxDeductionPercentage = myResponse.Result.Percentage;
                    }
                    else {
                        _this.IsNoVendorTax = myResponse.Result.IsDefault;
                    }
                }
                else {
                    _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    };
    APPaymentDetailsTabComponent.prototype.ApplyViewModel = function () {
        this.RefreshFields();
        this.SetUIProperties();
        this.BuildScreenData();
    };
    APPaymentDetailsTabComponent.prototype.InitializeServices = function () {
        this.CardListService = new CardListService_1.CardListService();
        this.PartnersDomainService = new PartnersDomainService_1.PartnersDomainService();
        this.AddressListService = new AddressListService_1.AddressListService();
        this.GLAccountWithholdingService = new GLAccountWithholdingTaxExtendedService_1.GLAccountWithholdingTaxExtendedPMService();
        this.BankAccountPMService = new BankAccountPMService_1.BankAccountPMService();
    };
    //RefreshFields
    APPaymentDetailsTabComponent.prototype.RefreshFields = function () {
        this.ComputeRelativeRateDate();
        this.GetRateIsEnabled();
    };
    APPaymentDetailsTabComponent.prototype.GetRateIsEnabled = function () {
        this.SetUIProperties_ExchangeRate();
    };
    // SetUIProperties
    APPaymentDetailsTabComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_Cheque();
        this.SetUIProperties_Invoices();
        this.SetUIProperties_CreditCard();
        if (!this.IsScreenEnabled) {
            this.UIProperties.SetEnabled("AccountingPaymentMethodId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AmountInPaymentCurrency", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RegisterDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ValueDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ChequeOrPaymentRef", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Bank", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BankBranch", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Account", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CreditCardTypeId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, false);
            if (this.IsFullAccounting) {
                this.UIProperties.SetEnabled("BankAccountId", this.ObjectTableName, false);
            }
        }
        else {
            this.UIProperties.SetEnabled("AccountingPaymentMethodId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("AmountInPaymentCurrency", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("RegisterDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ValueDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ChequeOrPaymentRef", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("Bank", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("BankBranch", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("Account", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PrintNotes", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("CreditCardTypeId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, true);
            if (this.IsFullAccounting) {
                this.UIProperties.SetEnabled("BankAccountId", this.ObjectTableName, true);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VendorId)) {
                this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, false);
            }
            if (this.EntityPM.StatusCode == "VD") {
                this.UIProperties.SetEnabled("PrintNotes", this.ObjectTableName, false);
            }
        }
        this.SetUIProperties_FullAccounting();
    };
    APPaymentDetailsTabComponent.prototype.SetUIProperties_Invoices = function () {
        if (!this.IsScreenEnabled) {
            this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, true);
            if (this.EntityPM.PaymentInvoices.length > 0) {
                this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
            }
        }
        this.SetUIProperties_ExchangeRate();
    };
    APPaymentDetailsTabComponent.prototype.SetUIProperties_ExchangeRate = function () {
        var isEnabled = false;
        if (this.IsScreenEnabled) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APPayment", "APPaymentEditExchangeRate")) {
                if (this.PaymentCurrencyId) {
                    if (this.PaymentCurrencyId != SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                        if (this.EntityPM.PaymentInvoices.length == 0) {
                            isEnabled = true;
                        }
                    }
                }
            }
        }
        this.RateIsEnabled = isEnabled;
        this.UIProperties.SetEnabled("PaymentCurrencyExchangeRate", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("ExchangeRateDate", this.ObjectTableName, isEnabled);
    };
    APPaymentDetailsTabComponent.prototype.SetUIProperties_Cheque = function () {
        this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("Bank", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("Account", this.ObjectTableName, false);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PaymentMethodCode)) {
            if (this.PaymentMethodCode == "CH" && !this.PaymentChequeActivated) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.ChequeOrPaymentRef)) {
                    this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, true);
                }
            }
            if (this.PaymentMethodCode == "CH" || this.PaymentMethodAddedManually) {
                this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
            }
        }
    };
    APPaymentDetailsTabComponent.prototype.SetUIProperties_CreditCard = function () {
        this.UIProperties.SetVisibility("CreditCardTypeId", this.ObjectTableName, false);
        this.CreditCardTypeIdVisibility = false;
        this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, false);
        if (this.PaymentMethodCode == "CC") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.CreditCardTypeId)) {
                this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, true);
            }
            this.UIProperties.SetVisibility("CreditCardTypeId", this.ObjectTableName, true);
            this.CreditCardTypeIdVisibility = true;
        }
    };
    // BuildScreenData
    APPaymentDetailsTabComponent.prototype.BuildScreenData = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
            this.LoadCurrencyRates();
        }
        else {
            this.LoadData();
        }
    };
    //Refresh Screen
    APPaymentDetailsTabComponent.prototype.RefreshScreen = function () {
        this.SetUIProperties();
        this.LoadData();
        this.CollapseSplitButton();
    };
    APPaymentDetailsTabComponent.prototype.CollapseSplitButton = function () {
        if (this.PaymentMethodCode == "CH" && this.AutomaticPaymentCheque) {
            this.IsSplitButtonVisibile = false;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ChequeOrPaymentRef) && this.EntityPM.AutomaticPaymentCheque) {
            this.IsChequeLinkVisibile = true;
        }
    };
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "IsScreenEnabled", {
        get: function () {
            var result = true;
            if (this.EntityPM != null) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
                    result = true;
                }
                else {
                    result = false;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    APPaymentDetailsTabComponent.prototype.LoadCurrencyRates = function () {
        var _this = this;
        var loadingDate = this.RegisterDate;
        if (loadingDate == null) {
            loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        var myService = new CurrencyRatesService_1.CurrencyRatesService();
        myService.GetCurrenciesExchangeRateByValueDate(SessionLocator_1.SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.LastRatesList = myResponse.Result;
            }
            _this.LoadData();
        });
    };
    APPaymentDetailsTabComponent.prototype.UpdateCurrencyRates = function () {
        var _this = this;
        var loadingDate = this.RegisterDate;
        if (loadingDate == null) {
            loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        var myService = new CurrencyRatesService_1.CurrencyRatesService();
        myService.GetCurrenciesExchangeRateByValueDate(SessionLocator_1.SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.LastRatesList = myResponse.Result;
                _this.SetCurrencyRateData();
            }
        });
    };
    APPaymentDetailsTabComponent.prototype.SetCurrencyRateData = function () {
        var _this = this;
        var myRate = null;
        var myRateDate = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PaymentCurrencyId)) {
            if (this.PaymentCurrencyId == SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                myRate = 1;
            }
            else {
                var lastRate = this.LastRatesList.filter(function (d) { return d.ForeignCurrencyId == _this.PaymentCurrencyId; })[0];
                if (lastRate != null) {
                    myRate = lastRate.Rate;
                    myRateDate = lastRate.ValueDate;
                }
            }
        }
        this.PaymentCurrencyExchangeRate = myRate;
        this.ExchangeRateDate = myRateDate;
    };
    APPaymentDetailsTabComponent.prototype.GetCurrencyRate = function (currencyId) {
        var result = null;
        if (Tools_1.AppTool.IsNullOrEmpty(currencyId)) {
            result = null;
        }
        else {
            if (currencyId == SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                result = 1;
            }
            else {
                if (this.LastRatesList) {
                    var lastRate = this.LastRatesList.filter(function (d) { return d.ForeignCurrencyId == currencyId; })[0];
                    if (lastRate != null) {
                        result = lastRate.Rate;
                    }
                }
            }
        }
        return result;
    };
    APPaymentDetailsTabComponent.prototype.GetCurrencyRateDate = function (currencyId) {
        var result = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(currencyId)) {
            if (currencyId == SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                result = null;
            }
            else {
                if (this.LastRatesList) {
                    var lastRate = this.LastRatesList.filter(function (d) { return d.ForeignCurrencyId == currencyId; })[0];
                    if (lastRate != null) {
                        result = lastRate.ValueDate;
                    }
                }
            }
        }
        return result;
    };
    APPaymentDetailsTabComponent.prototype.SearchTextKeyUp = function (args) {
        this.searchText = args;
        this.LoadData();
    };
    APPaymentDetailsTabComponent.prototype.LoadData = function () {
        this.ItemsSource.Clear();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VendorId) && this.EntityPM.StatusCode != "VD") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                this.LoadPaymentInvoices_IsMatched();
            }
            else {
                this.LoadPaymentInvoices_Connected();
            }
        }
        else {
            this.UpdateSummary();
            this.IsDataLoaded = true;
        }
    };
    APPaymentDetailsTabComponent.prototype.LoadPaymentInvoices_Connected = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1000;
        filters.SortBy = "DueDate";
        filters.SortDirection = "Descending";
        var searchValue = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.searchText)) {
            searchValue = Tools_1.AppTool.IsNullOrEmpty(this.searchText.trim()) ? null : this.searchText;
        }
        filters.addAdditionalFilter("APPaymentInvoicesSearch", searchValue, null, null, "Contains", true, false, false, "string");
        filters.addAdditionalFilter("APPaymentInvoicesConnected", this.EntityPM.Id, null, null, "Contains", true, false, false, "string");
        this.APPaymentsDetailsService.getByFilters(filters).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ConnectedList = myResponse.Result;
                if (_this.EntityPM.IsClosed) {
                    _this.FillBaselist();
                }
                else {
                    _this.LoadPaymentInvoices_IsMatched();
                }
            }
        });
    };
    APPaymentDetailsTabComponent.prototype.LoadPaymentInvoices_IsMatched = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1000;
        filters.SortBy = "DueDate";
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("VendorId", this.EntityPM.VendorId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("StatusCode", "WA,AD,PP,PD", null, null, "InList", false, true, false, "string");
        filters.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
        var searchValue = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.searchText)) {
            searchValue = Tools_1.AppTool.IsNullOrEmpty(this.searchText.trim()) ? null : this.searchText;
        }
        filters.addAdditionalFilter("APPaymentInvoicesSearch", searchValue, null, null, "Contains", true, false, false, "string");
        this.APPaymentsDetailsService.getByFilters(filters).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.IsMatchedList = myResponse.Result;
            }
            _this.FillBaselist();
        });
    };
    APPaymentDetailsTabComponent.prototype.FillBaselist = function () {
        var _this = this;
        this.ItemsSource.Clear();
        var connectedList = [];
        var unConnectedMatchedList = [];
        var unConnectedListNotMatched = [];
        var itemsCollection = [];
        if (this.ConnectedList.length > 0) {
            this.ConnectedList.forEach(function (item) {
                if (_this.EntityPM.PaymentInvoices.filter(function (d) { return d.APInvoiceId == item.Id; })[0]) {
                    if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) {
                        if (_this.EntityPM.AmountInPaymentCurrency > 0) {
                            var value = _this.EntityPM.AmountInPaymentCurrency;
                            if (item.AmountDue > value) {
                                item.AmountDue = item.AmountDue - value;
                            }
                            else {
                                item.AmountDue = 0;
                            }
                        }
                    }
                    connectedList.push(new APPaymentInvoiceArgs(item, _this));
                }
            });
            connectedList.sort(function (a, b) { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1; }).forEach(function (item) {
                itemsCollection.push(item);
            });
        }
        if (!this.EntityPM.IsClosed) {
            this.IsMatchedList.forEach(function (item) {
                if (connectedList.filter(function (f) { return f.Id == item.Id; }).length == 0) {
                    if (!item.IsClosed) {
                        var isCurrencyMatched = false;
                        if (_this.PaymentCurrencyId == item.InvoiceCurrencyId) {
                            isCurrencyMatched = true;
                        }
                        else if (_this.IsMultiCurrency) {
                            if (_this.PaymentCurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
                                isCurrencyMatched = true;
                            }
                            else if (item.InvoiceCurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
                                isCurrencyMatched = true;
                            }
                        }
                        if (isCurrencyMatched == false || item.StatusCode == "WA") {
                            unConnectedListNotMatched.push(new APPaymentInvoiceArgs(item, _this));
                        }
                        else {
                            unConnectedMatchedList.push(new APPaymentInvoiceArgs(item, _this));
                        }
                    }
                }
            });
            unConnectedMatchedList.filter(function (f) { return f.CurrencyId == _this.PaymentCurrencyId; }).sort(function (a, b) { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1; }).forEach(function (item) {
                itemsCollection.push(item);
            });
            unConnectedMatchedList.filter(function (f) { return f.CurrencyId != _this.PaymentCurrencyId; }).sort(function (a, b) { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1; }).forEach(function (item) {
                itemsCollection.push(item);
            });
            unConnectedListNotMatched.sort(function (a, b) { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1; }).forEach(function (item) {
                itemsCollection.push(item);
            });
        }
        this.ItemsSource.InsertCollection(itemsCollection);
        this.UpdateSummary();
        this.IsDataLoaded = true;
    };
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "VendorId", {
        // Vendor Properties
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.VendorId;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.VendorId != value) {
                    this.EntityPM.VendorId = value;
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VendorId)) {
                        this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, false);
                    }
                    else {
                        this.UIProperties.SetEnabled("VendorAddressId", this.ObjectTableName, true);
                    }
                    this.GetCardProperties();
                    this.LoadData();
                    this.IsTaxUpdated = true;
                    this.LoadTaxPercentage();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    APPaymentDetailsTabComponent.prototype.GetCardProperties = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VendorId)) {
            this.FillDataFromCardList(null);
            this.EntityPM.VendorPartnerTypeId = null;
        }
        else {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.CardListService.getSingle(this.EntityPM.VendorId).subscribe(function (myResult) {
                var myResponse = myResult;
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    var cardList = myResponse.Result;
                    if (cardList != null) {
                        _this.FillDataFromCardList(cardList);
                    }
                }
            });
        }
    };
    APPaymentDetailsTabComponent.prototype.FillDataFromCardList = function (list) {
        if (list == null) {
            this.GLAccountId = null;
            this.vendorGLAccount = null;
            this.deductionFileNumber = null;
            this.VendorAddressId = null;
            this.PaymentCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
        }
        else {
            this.GLAccountId = list.GLAccountId;
            if (!Tools_1.AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                this.PaymentCurrencyId = list.InvoiceCurrencyId;
            }
            if (SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal) {
                this.EntityPM.VendorName = list.EnglishName;
            }
            else {
                this.EntityPM.VendorName = list.LocalName;
            }
            this.EntityPM.VendorPartnerTypeId = list.PartnerTypeId;
            this.LoadAddress();
            if (this.IsFullAccounting)
                this.GetConnectedGLAccount();
        }
    };
    APPaymentDetailsTabComponent.prototype.GetConnectedGLAccount = function () {
        var _this = this;
        if (this.GLAccountId) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this._GLAccountListService.getSingle(this.GLAccountId).subscribe(function (myResult) {
                console.log("[_GLAccountListService.getSingle]", myResult);
                _this.CurrentSession.StopBusyIndicator();
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var gla = myResponse.Result;
                    _this.vendorGLAccount = gla;
                    _this.deductionFileNumber = gla ? gla.DeductionFileNumber : null;
                }
            });
        }
        else {
            this.vendorGLAccount = null;
            this.deductionFileNumber = null;
        }
    };
    APPaymentDetailsTabComponent.prototype.LoadAddress = function () {
        var _this = this;
        this.PartnersDomainService.GetBillingOrMainAddressListByCardId(this.EntityPM.VendorId).subscribe(function (resp) {
            if (resp != null) {
                var address = resp;
                if (address != null) {
                    _this.VendorAddressId = address.Id;
                }
                else {
                    _this.VendorAddressId = null;
                }
            }
        });
    };
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "VendorAddressId", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.VendorAddressId;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.VendorAddressId != value) {
                    this.EntityPM.VendorAddressId = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "PaymentCurrencyId", {
        // Currency Properties
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.PaymentCurrencyId;
        },
        set: function (value) {
            var _this = this;
            if (this.EntityPM != null) {
                if (this.EntityPM.PaymentCurrencyId != value) {
                    this.EntityPM.PaymentCurrencyId = value;
                    this.PaymentCurrencyExchangeRate = this.GetCurrencyRate(value);
                    this.ExchangeRateDate = this.GetCurrencyRateDate(value);
                    if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                        this.PaymentCurrencyCode = null;
                    }
                    else {
                        var myService = new CurrencyListService_1.CurrencyListService();
                        myService.getSingleFromCache(value).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var list = myResponse.Result;
                                if (list) {
                                    _this.PaymentCurrencyCode = list.Code;
                                }
                            }
                        });
                    }
                    this.SetUIProperties_ExchangeRate();
                    this.ItemsSource.Collection.forEach(function (item) {
                        item.SetUIProperties();
                        item.InitExchangeRate();
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "PaymentCurrencyExchangeRate", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.PaymentCurrencyExchangeRate;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.PaymentCurrencyExchangeRate != value) {
                    this.EntityPM.PaymentCurrencyExchangeRate = Tools_1.AppTool.Round(value, 5);
                    this.GetRateIsEnabled();
                    this.ComputeLocalAmount();
                    this.ItemsSource.Collection.forEach(function (item) {
                        item.InitExchangeRate();
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "ExchangeRateDate", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.PaymentCurrencyExchangeRateDate;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.PaymentCurrencyExchangeRateDate != value) {
                    this.EntityPM.PaymentCurrencyExchangeRateDate = value;
                    this.ComputeRelativeRateDate();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "AutomaticPaymentCheque", {
        get: function () { return this.EntityPM.AutomaticPaymentCheque; },
        set: function (value) {
            if (this.EntityPM.AutomaticPaymentCheque != value) {
                this.EntityPM.AutomaticPaymentCheque = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "RelativeRateDate", {
        get: function () { return this.myRelativeRateDate; },
        set: function (value) {
            if (this.myRelativeRateDate != value) {
                this.myRelativeRateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    APPaymentDetailsTabComponent.prototype.ComputeRelativeRateDate = function () {
        this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(this.RegisterDate, this.ExchangeRateDate, "old");
    };
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "TaxDeductionPercentage", {
        // Properties
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.TaxDeductionPercentage;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.TaxDeductionPercentage != value) {
                    this.EntityPM.TaxDeductionPercentage = value;
                    this.CalculateTaxDeductionLocalAmount();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "TaxDeductionLocalAmount", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.TaxDeductionLocalAmount;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.TaxDeductionLocalAmount != value) {
                    this.EntityPM.TaxDeductionLocalAmount = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    APPaymentDetailsTabComponent.prototype.CalculateTaxDeductionLocalAmount = function () {
        if (this.IsFullAccounting && this.TaxDeductionPercentage != null && this.AmountInLocalCurrency != null) {
            this.TaxDeductionLocalAmount = (this.TaxDeductionPercentage * this.AmountInLocalCurrency) / 100;
            this.SetUIProperties_FullAccounting_Tax();
        }
    };
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "RegisterDate", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.RegisterDate;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.RegisterDate != value) {
                    this.EntityPM.RegisterDate = value;
                    this.UpdateCurrencyRates();
                    this.IsTaxUpdated = true;
                    this.LoadTaxPercentage();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "AccountingPaymentMethodId", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.AccountingPaymentMethodId;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.AccountingPaymentMethodId != value) {
                    this.EntityPM.AccountingPaymentMethodId = value;
                    this.RefreshPaymentMethodFields();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "PaymentMethodCode", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.PaymentMethodCode;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.PaymentMethodCode != value) {
                    this.EntityPM.PaymentMethodCode = value;
                    this.SetAutomaticPaymentCheque(value);
                    this.SetUIProperties_FullAccounting(); // for BankAccountId
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    APPaymentDetailsTabComponent.prototype.SetAutomaticPaymentCheque = function (value) {
        if (this.PaymentMethodCode == "CH" && this.PaymentChequeActivated) {
            this.EntityPM.AutomaticPaymentCheque = true;
            this.IsSplitButtonVisibile = true;
        }
        else {
            this.EntityPM.AutomaticPaymentCheque = false;
            this.IsSplitButtonVisibile = false;
        }
    };
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "BranchId", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.BranchId;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.BranchId != value) {
                    this.EntityPM.BranchId = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    APPaymentDetailsTabComponent.prototype.RefreshPaymentMethodFields = function () {
        var _this = this;
        this.Bank = null;
        this.BankBranch = null;
        this.Account = null;
        this.ChequeOrPaymentRef = null;
        this.ValueDate = null;
        this.CreditCardTypeId = null;
        if (this.IsFullAccounting) {
            this.BankAccountId = null;
        }
        var lists = this.AllMethods.filter(function (d) { return d.Id == _this.AccountingPaymentMethodId; });
        if (lists) {
            var list = lists[0];
            if (list) {
                this.PaymentMethodCode = list.Code;
                this.PaymentMethodAddedManually = list.AddedManually;
            }
        }
        this.ValueDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        if (this.PaymentMethodCode == "CA" || this.PaymentMethodCode == "FS") {
            // 
        }
        this.SetUIProperties_Cheque();
        this.SetUIProperties_CreditCard();
        if (this.IsFullAccounting) {
            this.SetUIProperties_FullAccounting();
        }
    };
    APPaymentDetailsTabComponent.prototype.SetUIProperties_FullAccounting = function () {
        if (this.IsFullAccounting) {
            this.SetUIProperties_FullAccounting_Tax();
            if (this.EntityPM.Id != null) {
                this.ShowSplitButton = false;
                this.PaymentChequeActivated = false;
            }
            if (this.PaymentMethodCode == "CH" || this.PaymentMethodCode == "BT" || this.PaymentMethodCode == "CC") {
                this.BankAccountIdVisibility = true;
            }
            else {
                this.BankAccountIdVisibility = false;
            }
            if (this.PaymentMethodCode == "CH" || this.PaymentMethodCode == "BT" || this.PaymentMethodCode == "CC") {
                this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
                this.UIProperties.SetEnabled("Bank", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("BankBranch", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("Account", this.ObjectTableName, false);
                this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, !this.BankAccountId);
            }
            else {
                this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, false);
            }
        }
    };
    APPaymentDetailsTabComponent.prototype.SetUIProperties_FullAccounting_Tax = function () {
        this.UIProperties.SetEnabled("TaxDeductionLocalAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TaxDeductionPercentage", this.ObjectTableName, false);
        this.UIProperties.SetRequired("TaxDeductionLocalAmount", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.TaxDeductionLocalAmount));
        this.UIProperties.SetRequired("TaxDeductionPercentage", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.TaxDeductionPercentage));
    };
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "PaymentMethodDetailsLabel", {
        get: function () {
            var _this = this;
            var result = "";
            if (this.PaymentMethodCode == "CH") {
                result = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.S.Details.Cheque");
            }
            else if (this.PaymentMethodCode == "BT") {
                result = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.S.Details.BankTransfer");
            }
            else if (this.PaymentMethodCode == "CC") {
                result = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.S.Details.CreditCard");
            }
            else {
                var method = this.AllMethods.filter(function (d) { return d.Code == _this.PaymentMethodCode; })[0];
                if (method != null) {
                    result = method.Name;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "ChequePaymentRefLabel", {
        get: function () {
            var result = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.S.Details.PaymentRef");
            if (this.PaymentMethodCode == "CH") {
                result = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.S.Details.ChequeRef");
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "VisibleIfCash", {
        get: function () {
            var result = false;
            if (Tools_1.AppTool.IsNullOrEmpty(this.PaymentMethodCode) || this.PaymentMethodCode == "CA" || this.PaymentMethodCode == "FS") {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "CollapsedIfCash", {
        get: function () {
            var result = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.PaymentMethodCode)) {
                if (this.PaymentMethodCode == "CA" || this.PaymentMethodCode == "FS") {
                    result = false;
                }
                else {
                    result = true;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "Bank", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.Bank;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.Bank != value) {
                    this.EntityPM.Bank = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "BankBranch", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.BankBranch;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.BankBranch != value) {
                    this.EntityPM.BankBranch = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "Account", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.Account;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.Account != value) {
                    this.EntityPM.Account = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "ValueDate", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.ValueDate;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.ValueDate != value) {
                    this.EntityPM.ValueDate = value;
                    if (value != null) {
                        this.UIProperties.SetRequired("ValueDate", this.ObjectTableName, false);
                    }
                    else {
                        this.UIProperties.SetRequired("ValueDate", this.ObjectTableName, true);
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "ChequeOrPaymentRef", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.ChequeOrPaymentRef;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.ChequeOrPaymentRef != value) {
                    this.EntityPM.ChequeOrPaymentRef = value;
                    if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                        this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, false);
                    }
                    else {
                        this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, true);
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "CreditCardTypeId", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.CreditCardTypeId;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.CreditCardTypeId != value) {
                    this.EntityPM.CreditCardTypeId = value;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, true);
                }
            }
            if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "PaymentCurrencyCode", {
        get: function () { return this.EntityPM.PaymentCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.PaymentCurrencyCode != value) {
                this.EntityPM.PaymentCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    APPaymentDetailsTabComponent.prototype.LoadCurrencies = function () {
        var _this = this;
        var myService = new CurrencyListService_1.CurrencyListService();
        myService.getAll().subscribe(function (response) {
            if (response != null) {
                _this.AllCurrencies = response.Result;
            }
        });
    };
    APPaymentDetailsTabComponent.prototype.LoadPaymentMethods = function () {
        var _this = this;
        var myService = new AccountingPaymentMethodListService_1.AccountingPaymentMethodListService();
        myService.getAll().subscribe(function (response) {
            if (response != null) {
                _this.AllMethods = response.Result;
            }
        });
    };
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "AmountInPaymentCurrency", {
        get: function () { return this.EntityPM.AmountInPaymentCurrency; },
        set: function (value) {
            if (this.EntityPM.AmountInPaymentCurrency != value) {
                this.EntityPM.AmountInPaymentCurrency = Tools_1.AppTool.Round(value, 2);
                this.ComputeLocalAmount();
                this.ComputeOpenAmount();
                this.UpdateSummary();
                this.ItemsSource.Collection.forEach(function (item) {
                    item.SetUIProperties();
                });
                //this.ItemsSource.Collection.forEach(item => {
                //    item.RefreshMatchProperties();
                //});
                this.CalculateTaxDeductionLocalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "AmountInLocalCurrency", {
        get: function () { return this.EntityPM.AmountInLocalCurrency; },
        set: function (value) {
            if (this.EntityPM.AmountInLocalCurrency != value) {
                this.EntityPM.AmountInLocalCurrency = Tools_1.AppTool.Round(value, 2);
                this.CalculateTaxDeductionLocalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "OpenAmount", {
        get: function () { return this.EntityPM.OpenAmount == null ? 0 : this.EntityPM.OpenAmount; },
        set: function (value) {
            if (this.EntityPM.OpenAmount != value) {
                this.EntityPM.OpenAmount = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "PrintNotes", {
        get: function () { return this.EntityPM.PrintNotes; },
        set: function (value) {
            if (this.EntityPM.PrintNotes != value) {
                this.EntityPM.PrintNotes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentDetailsTabComponent.prototype, "BankAccountId", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.BankAccountId;
        },
        set: function (value) {
            var _this = this;
            if (this.EntityPM != null) {
                if (this.EntityPM.BankAccountId != value) {
                    this.EntityPM.BankAccountId = value;
                    this.BankAccountPMService.get(this.EntityPM.BankAccountId).subscribe(function (res) {
                        if (res) {
                            if (res.Result) {
                                var bank = res.Result;
                                _this.BankBranch = bank.BranchNumber;
                                _this.Account = bank.AccountNumber;
                                _this.Bank = bank.BankCode;
                            }
                        }
                    });
                    this.SetUIProperties_FullAccounting();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    APPaymentDetailsTabComponent.prototype.ComputeOpenAmount = function () {
        this.OpenAmount = this.AmountInPaymentCurrency - this.Summary_AmountPaid;
    };
    APPaymentDetailsTabComponent.prototype.ComputeLocalAmount = function () {
        this.AmountInLocalCurrency = this.AmountInPaymentCurrency * this.PaymentCurrencyExchangeRate;
    };
    APPaymentDetailsTabComponent.prototype.UpdateSummary = function () {
        var Amount = 0;
        var AmountPaid = 0;
        var AmountPaidColor = "#282E30";
        if (this.AmountInPaymentCurrency) {
            Amount = Tools_1.AppTool.Round(this.AmountInPaymentCurrency, 2);
        }
        if (this.EntityPM.PaymentInvoices.length > 0) {
            AmountPaid = Tools_1.ArrayTool.Sum(this.EntityPM.PaymentInvoices, "PaymentAmount");
            AmountPaid = Tools_1.AppTool.Round(AmountPaid, 2);
        }
        if (this.IsNegativeAmountEnabled == false) {
            if (AmountPaid < 0) {
                AmountPaidColor = "#E53030";
            }
        }
        if (AmountPaid > Amount) {
            AmountPaidColor = "#E53030";
        }
        this.Summary_Amount = Amount;
        this.Summary_AmountPaid = AmountPaid;
        this.Summary_AmountPaidColor = AmountPaidColor;
    };
    APPaymentDetailsTabComponent.prototype.ViewEntity = function (args) {
        if (args) {
            this.RequestedCommandCode = "ViewInvoice";
            this.RequestedCommandParam = args.Id;
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    APPaymentDetailsTabComponent.prototype.UpdateCurrencyRateMethod = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("RatesTable", 0).subscribe(function (response) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("RatesTable.O.UpdateCurrencyRate");
            logWindow.WindowArgs = { CurrencyId: _this.EntityPM.PaymentCurrencyId, CurrencyCode: _this.EntityPM.PaymentCurrencyCode, Rate: _this.EntityPM.PaymentCurrencyExchangeRate, Date: _this.RegisterDate };
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.PaymentCurrencyExchangeRate = comp.Rate;
                        _this.ExchangeRateDate = comp.RateDate;
                    }
                });
            });
            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
        });
    };
    APPaymentDetailsTabComponent.prototype.UpdatePaymentInvoicesErrors = function () {
        var haserrors = false;
        if (this.ItemsSource.Collection.filter(function (d) { return d.InputHasError; })[0]) {
            haserrors = true;
        }
        this.EntityPM.HasInvoicesErrors = haserrors;
    };
    APPaymentDetailsTabComponent.prototype.ApplyRequestedCommand = function () {
        var _this = this;
        if (this.RequestedCommandCode == "ViewInvoice") {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this.RequestedCommandParam, ObjectTableName: 'APInvoice' });
                _this.RequestedCommandParam = null;
                var isEditComponentSaved = false;
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    if (isEditComponentSaved) {
                        _this.entityArgs.EditComponent.ReloadEntityPM();
                    }
                });
                cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });
                cmpRef.instance.SaveAndCloseCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });
            });
        }
        this.RequestedCommandCode = null;
    };
    APPaymentDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './APPaymentDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService, core_1.ChangeDetectorRef])
    ], APPaymentDetailsTabComponent);
    return APPaymentDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.APPaymentDetailsTabComponent = APPaymentDetailsTabComponent;
var APPaymentInvoiceArgs = /** @class */ (function (_super) {
    __extends(APPaymentInvoiceArgs, _super);
    function APPaymentInvoiceArgs(item, trigger) {
        var _this = _super.call(this) || this;
        _this.trigger = trigger;
        _this.EntityPM = null;
        _this.Invoice = null;
        _this.PaymentPM = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "APInvoice";
        _this.SortingValue = 0;
        _this.LocalCurrencyId = null;
        _this.IsMultiCurrency = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Id = null;
        _this.DueDate = null;
        _this.Status = null;
        _this.InvoiceNumber = null;
        _this.TransferStatus = null;
        _this.ShipmentNumber = null;
        _this.CurrencyId = null;
        _this.CurrencyCode = null;
        _this.InvoiceAmount = 0;
        _this.TransferStatusCode = null;
        _this.ExchangeRate = 0;
        _this.OtherPaymentsAmount = 0;
        _this.isControlEnabled = false;
        _this.isCurrencyMatched = false;
        _this.isAllowedToConnect = true;
        _this.CheckBoxVisibility = false;
        _this.NotMatchedVisibility = false;
        _this.ConnectFeature = false;
        _this.DissconectFeature = false;
        _this.IsAdvancedButtonVisible = false;
        _this.CheckBoxEnabled = true;
        _this.NoPermision = "";
        _this.CellColor = "#282E30";
        _this.CellBackground = "transparent";
        _this.CurrencyCodeColor = "transparent";
        _this.CurrencyCodeBackground = "transparent";
        _this.StatusBackground = "transparent";
        _this.isConnected = false;
        _this.ConnectedAmount_INV = 0;
        _this.ConnectedAmount_PAY = 0;
        _this.amountDue = 0;
        _this.Invoice = item;
        _this.PaymentPM = _this.trigger.EntityPM;
        _this.LocalCurrencyId = SessionLocator_1.SessionLocator.LocalCurrencyId;
        _this.IsMultiCurrency = _this.trigger.IsMultiCurrency;
        if (item) {
            if (item.DueDate) {
                _this.SortingValue = Tools_1.DateTool.GetDateParts(item.DueDate).DateTicks;
            }
        }
        _this.EntityPM = _this.PaymentPM.PaymentInvoices.filter(function (d) { return d.APInvoiceId == _this.Invoice.Id; })[0];
        if (_this.EntityPM) {
            _this.isConnected = true;
            _this.ExchangeRate = _this.EntityPM.ExchangeRate;
            _this.ConnectedAmount_INV = _this.EntityPM.ForeignAmount;
            _this.ConnectedAmount_PAY = _this.EntityPM.PaymentAmount;
        }
        _this.InitProperties();
        _this.InitAllAmounts();
        _this.InitExchangeRate();
        _this.SetUIProperties();
        return _this;
    }
    APPaymentInvoiceArgs.prototype.InitProperties = function () {
        this.Id = this.Invoice.Id;
        this.DueDate = this.Invoice.DueDate;
        this.Status = this.Invoice.StatusName;
        this.InvoiceNumber = this.Invoice.InvoiceNumber;
        this.TransferStatus = this.Invoice.TransferStatusName;
        this.CurrencyId = this.Invoice.InvoiceCurrencyId;
        this.CurrencyCode = this.Invoice.InvoiceCurrencyCode;
        this.InvoiceAmount = this.Invoice.AmountInInvoiceCurrency == null ? 0 : this.Invoice.AmountInInvoiceCurrency;
        this.ShipmentNumber = this.Invoice.IsMultipleEntities ? "List" : this.Invoice.MainEntityReference;
        this.TransferStatusCode = this.Invoice.TransferStatusCode;
    };
    APPaymentInvoiceArgs.prototype.InitAllAmounts = function () {
        var myInvoiceAmount = 0;
        var myAmountDue = 0;
        var myOtherAmounts = 0;
        if (this.Invoice.AmountInInvoiceCurrency != null) {
            myInvoiceAmount = this.Invoice.AmountInInvoiceCurrency;
        }
        if (this.Invoice.AmountDue != null) {
            myAmountDue = this.Invoice.AmountDue;
        }
        myOtherAmounts = myInvoiceAmount - myAmountDue;
        if (this.EntityPM) {
            myOtherAmounts -= this.EntityPM.ForeignAmount;
            this.Invoice.AmountPaid = this.EntityPM.ForeignAmount;
        }
        this.InvoiceAmount = myInvoiceAmount;
        this.AmountDue = myAmountDue;
        this.OtherPaymentsAmount = Tools_1.AppTool.Round(myOtherAmounts, 2);
    };
    APPaymentInvoiceArgs.prototype.InitExchangeRate = function () {
        var myExchangeRate = null;
        if (this.EntityPM) {
            myExchangeRate = this.EntityPM.ExchangeRate;
        }
        else {
            if (this.CurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
                myExchangeRate = this.PaymentPM.PaymentCurrencyExchangeRate;
            }
            else {
                myExchangeRate = this.Invoice.InvoiceCurrencyExchangeRate;
            }
        }
        this.ExchangeRate = myExchangeRate;
    };
    APPaymentInvoiceArgs.prototype.SetUIProperties = function () {
        this.isControlEnabled = false;
        this.CheckBoxVisibility = false;
        this.NotMatchedVisibility = false;
        this.IsAdvancedButtonVisible = false;
        this.CheckBoxEnabled = true;
        this.SetUIProperties_CurrencyMatched();
        this.SetUIProperties_AllowedToConnect();
        if (this.isAllowedToConnect) {
            this.isControlEnabled = true;
            if (this.trigger.EntityPM.OpenAmount <= 0) {
                this.isControlEnabled = false;
            }
        }
        if (this.isConnected) {
            this.isControlEnabled = true;
            this.CheckBoxVisibility = true;
            this.NotMatchedVisibility = false;
        }
        else {
            if (!this.isAllowedToConnect) {
                this.CheckBoxVisibility = false;
                this.NotMatchedVisibility = true;
                this.IsAdvancedButtonVisible = false;
            }
            else if (this.trigger.EntityPM.OpenAmount <= 0) {
                this.NotMatchedVisibility = false;
            }
            else {
                this.CheckBoxVisibility = true;
            }
        }
        this.SetUIProperties_AmountPaidEnabled();
        this.SetLineColors();
        var featureExist = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APPayment", "APPaymentConnectInvoices")) {
            this.ConnectFeature = true;
            featureExist = true;
        }
        else {
            if (this.isConnected) {
                this.CheckBoxVisibility = true;
                this.ConnectFeature = true;
            }
            else {
                this.CheckBoxVisibility = false;
                this.ConnectFeature = false;
                this.UIProperties.SetEnabled("AmountPaid", this.ObjectTableName, false);
            }
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APPayment", "APPaymentDissconectInvoices")) {
            this.DissconectFeature = true;
        }
        else {
            if (this.isConnected) {
                this.CheckBoxEnabled = false;
                this.NoPermision = "You have no permission to disconnect invoices";
                this.UIProperties.SetEnabled("AmountPaid", this.ObjectTableName, false);
            }
            else {
                this.CheckBoxEnabled = true;
                this.NoPermision = null;
            }
        }
    };
    APPaymentInvoiceArgs.prototype.SetUIProperties_CurrencyMatched = function () {
        this.isCurrencyMatched = false;
        if (this.PaymentPM.PaymentCurrencyId == this.CurrencyId) {
            this.isCurrencyMatched = true;
        }
        else {
            if (this.trigger.IsMultiCurrency) {
                if (this.PaymentPM.PaymentCurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
                    this.isCurrencyMatched = true;
                    this.IsAdvancedButtonVisible = true;
                }
                else if (this.CurrencyId == SessionLocator_1.SessionLocator.LocalCurrencyId) {
                    this.isCurrencyMatched = true;
                    this.IsAdvancedButtonVisible = true;
                }
            }
        }
    };
    APPaymentInvoiceArgs.prototype.SetUIProperties_AllowedToConnect = function () {
        this.isAllowedToConnect = true;
        if (this.IsConnected == false) {
            if (this.Invoice.StatusCode == "WA") {
                this.isAllowedToConnect = false;
            }
            if (this.isCurrencyMatched == false) {
                this.isAllowedToConnect = false;
            }
            if (Tools_1.AppTool.IsNullOrZero(this.AmountDue)) {
                this.isAllowedToConnect = false;
            }
        }
    };
    APPaymentInvoiceArgs.prototype.SetUIProperties_AmountPaidEnabled = function () {
        var isEnabled = true;
        if (this.Invoice.StatusCode == "WA") {
            isEnabled = false;
        }
        else if (this.isCurrencyMatched == false) {
            isEnabled = false;
        }
        this.UIProperties.SetEnabled("AmountPaid", this.ObjectTableName, isEnabled);
    };
    APPaymentInvoiceArgs.prototype.SetLineColors = function () {
        this.CellBackground = "transparent";
        this.CurrencyCodeColor = this.CellColor;
        this.CurrencyCodeBackground = "transparent";
        this.StatusBackground = "transparent";
        if (this.IsConnected) {
            this.CellBackground = "rgba(208, 224, 234, 0.4)";
        }
        else {
            if (!this.isCurrencyMatched) {
                this.CurrencyCodeColor = "rgb(255, 94,0)";
                this.CurrencyCodeBackground = "rgba(255, 171,3, 0.6)";
            }
            if (this.Invoice.StatusCode == "WA") {
                this.StatusBackground = "rgba(255, 171,3, 0.6)";
            }
        }
    };
    Object.defineProperty(APPaymentInvoiceArgs.prototype, "IsConnected", {
        get: function () { return this.isConnected; },
        set: function (value) {
            var _this = this;
            if (this.isConnected != value) {
                this.isConnected = value;
                if (value == true) {
                    this.GetSmallestAmount();
                    this.Connect();
                }
                else {
                    var itemPM = this.PaymentPM.PaymentInvoices.filter(function (d) { return d.APInvoiceId == _this.Id; })[0];
                    if (itemPM) {
                        this.PaymentPM.RemoveAPPaymentInvoicePM(itemPM);
                        this.Disconnect();
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    APPaymentInvoiceArgs.prototype.GetSmallestAmount = function () {
        var invoiceAmount = this.AmountDue;
        var paymentAmount = this.PaymentPM.OpenAmount;
        var ConnectedAmountOfInvoice = 0;
        var ConnectedAmountOfPayment = 0;
        if (this.IsMultiCurrency && this.CurrencyId != this.PaymentPM.PaymentCurrencyId) {
            var invoiceAmountInPayment = 0;
            var paymentAmountInInvoice = 0;
            if (this.CurrencyId == this.LocalCurrencyId) {
                invoiceAmountInPayment = invoiceAmount / this.PaymentPM.PaymentCurrencyExchangeRate;
                paymentAmountInInvoice = paymentAmount * this.PaymentPM.PaymentCurrencyExchangeRate;
            }
            else {
                invoiceAmountInPayment = invoiceAmount * this.Invoice.InvoiceCurrencyExchangeRate;
                paymentAmountInInvoice = paymentAmount / this.Invoice.InvoiceCurrencyExchangeRate;
            }
            if (invoiceAmountInPayment <= paymentAmount) {
                ConnectedAmountOfInvoice = invoiceAmount;
                ConnectedAmountOfPayment = invoiceAmountInPayment;
            }
            else {
                ConnectedAmountOfInvoice = paymentAmountInInvoice;
                ConnectedAmountOfPayment = paymentAmount;
            }
        }
        else {
            if (invoiceAmount <= paymentAmount) {
                ConnectedAmountOfInvoice = invoiceAmount;
                ConnectedAmountOfPayment = invoiceAmount;
            }
            else {
                ConnectedAmountOfInvoice = paymentAmount;
                ConnectedAmountOfPayment = paymentAmount;
            }
        }
        this.ConnectedAmount_INV = ConnectedAmountOfInvoice;
        this.ConnectedAmount_PAY = ConnectedAmountOfPayment;
    };
    APPaymentInvoiceArgs.prototype.GetConnectedAmount_PAY = function (invoiceAmount) {
        var myResult = 0;
        if (!Tools_1.AppTool.IsNullOrZero(invoiceAmount)) {
            if (this.IsMultiCurrency && this.CurrencyId != this.PaymentPM.PaymentCurrencyId) {
                if (this.CurrencyId == this.LocalCurrencyId) {
                    myResult = invoiceAmount / this.ExchangeRate;
                }
                else {
                    myResult = invoiceAmount * this.ExchangeRate;
                }
            }
            else {
                myResult = invoiceAmount;
            }
        }
        return myResult;
    };
    Object.defineProperty(APPaymentInvoiceArgs.prototype, "AmountDue", {
        get: function () { return this.amountDue == null ? 0 : this.amountDue; },
        set: function (value) {
            if (this.amountDue != value) {
                this.amountDue = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APPaymentInvoiceArgs.prototype, "AmountPaid", {
        get: function () { return this.Invoice.AmountPaid == null ? 0 : this.Invoice.AmountPaid; },
        set: function (value) {
            var _this = this;
            if (this.Invoice.AmountPaid != value || this.InputHasError) {
                this.Invoice.AmountPaid = value;
                var inputErrors = 0;
                var inputEntry = value == null ? 0 : Tools_1.AppTool.Round(value, 2);
                var allowedAmount = Tools_1.AppTool.Round(this.InvoiceAmount - this.OtherPaymentsAmount, 2);
                this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, true, "");
                if (this.InvoiceAmount < 0) {
                    if (inputEntry > 0) {
                        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.M.OnlyMinusValue");
                        this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                        inputErrors++;
                    }
                    else if (inputEntry < allowedAmount) {
                        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.M.AmountPaidNotLess") + " " + allowedAmount;
                        this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                        inputErrors++;
                    }
                }
                else {
                    if (inputEntry < 0) {
                        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.M.CantPayMinusValue");
                        this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                        inputErrors++;
                    }
                    else if (inputEntry > allowedAmount) {
                        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.M.AmountPaidLessOrEqual") + " " + allowedAmount;
                        this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                        inputErrors++;
                    }
                }
                this.InputHasError = (inputErrors > 0);
                this.trigger.UpdatePaymentInvoicesErrors();
                if (this.InputHasError) {
                    return;
                }
                else {
                    if (!this.IsConnected) {
                        if (inputEntry != 0 && inputEntry != null) {
                            this.ConnectedAmount_INV = inputEntry;
                            this.ConnectedAmount_PAY = this.GetConnectedAmount_PAY(inputEntry);
                            this.Connect();
                        }
                    }
                    else {
                        if (inputEntry == 0 || inputEntry == null) {
                            this.IsConnected = false;
                        }
                        else {
                            this.Invoice.AmountPaid = inputEntry;
                            this.AmountDue = this.InvoiceAmount - this.OtherPaymentsAmount - inputEntry;
                            var itemPM = this.PaymentPM.PaymentInvoices.filter(function (d) { return d.APInvoiceId == _this.Id; })[0];
                            if (itemPM) {
                                this.ConnectedAmount_INV = inputEntry;
                                this.ConnectedAmount_PAY = this.GetConnectedAmount_PAY(inputEntry);
                                itemPM.ForeignAmount = this.ConnectedAmount_INV;
                                itemPM.PaymentAmount = this.ConnectedAmount_PAY;
                                if (this.CurrencyId == this.LocalCurrencyId) {
                                    itemPM.LocalAmount = itemPM.ForeignAmount;
                                }
                                else {
                                    itemPM.LocalAmount = Tools_1.AppTool.Round(itemPM.ForeignAmount * itemPM.ExchangeRate, 2);
                                }
                            }
                            this.UpdatePayment();
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    APPaymentInvoiceArgs.prototype.Connect = function () {
        this.isConnected = true;
        var itemPM = new APPaymentInvoicePM_1.APPaymentInvoicePM(this.PaymentPM);
        itemPM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
        itemPM.APInvoiceId = this.Id;
        itemPM.APPaymentId = this.PaymentPM.Id;
        itemPM.APInvoiceNumber = this.InvoiceNumber;
        itemPM.ForeignCurrencyId = this.CurrencyId;
        itemPM.ExchangeRate = this.ExchangeRate;
        itemPM.ForeignAmount = this.ConnectedAmount_INV == null ? 0 : Tools_1.AppTool.Round(this.ConnectedAmount_INV, 2);
        itemPM.PaymentAmount = this.ConnectedAmount_PAY == null ? 0 : Tools_1.AppTool.Round(this.ConnectedAmount_PAY, 2);
        itemPM.APInvoiceTransferStatusCode = this.TransferStatusCode;
        if (this.CurrencyId == this.LocalCurrencyId) {
            itemPM.LocalAmount = itemPM.ForeignAmount;
        }
        else {
            itemPM.LocalAmount = Tools_1.AppTool.Round(itemPM.ForeignAmount * itemPM.ExchangeRate, 2);
        }
        this.PaymentPM.AddAPPaymentInvoicePM(itemPM);
        var invoiceAmount = this.InvoiceAmount;
        var invoiceAmountPaid = itemPM.ForeignAmount;
        var invoiceAmountDue = invoiceAmount - this.OtherPaymentsAmount - invoiceAmountPaid;
        if (invoiceAmount < 0) {
            if (invoiceAmountDue > 0) {
                invoiceAmountDue = invoiceAmountDue * -1;
            }
            if (invoiceAmountPaid > 0) {
                invoiceAmountPaid = invoiceAmountPaid * -1;
            }
        }
        this.AmountDue = Tools_1.AppTool.Round(invoiceAmountDue, 2);
        this.Invoice.AmountPaid = Tools_1.AppTool.Round(invoiceAmountPaid, 2);
        this.SetLineColors();
        this.UpdatePayment();
    };
    APPaymentInvoiceArgs.prototype.Disconnect = function () {
        this.AmountDue = Tools_1.AppTool.Round(this.InvoiceAmount - this.OtherPaymentsAmount, 2);
        this.Invoice.AmountPaid = 0;
        this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, true, "");
        this.InputHasError = false;
        this.ConnectedAmount_INV = 0;
        this.ConnectedAmount_PAY = 0;
        this.SetLineColors();
        this.UpdatePayment();
    };
    APPaymentInvoiceArgs.prototype.UpdatePayment = function () {
        var _this = this;
        var paymentAmount = this.PaymentPM.AmountInPaymentCurrency == null ? 0 : this.PaymentPM.AmountInPaymentCurrency;
        var allPaidAmounts = 0;
        this.PaymentPM.PaymentInvoices.forEach(function (item) {
            var itemPaidAmount = 0;
            if (item.ForeignCurrencyId == _this.PaymentPM.PaymentCurrencyId) {
                itemPaidAmount = item.ForeignAmount;
            }
            else {
                itemPaidAmount = item.LocalAmount / item.ExchangeRate;
            }
            allPaidAmounts += itemPaidAmount;
        });
        var openAmount = paymentAmount - allPaidAmounts;
        this.trigger.EntityPM.OpenAmount = (openAmount == null) ? 0 : Tools_1.AppTool.Round(openAmount, 2);
        this.trigger.UpdatePaymentInvoicesErrors();
        this.trigger.UpdateSummary();
        this.trigger.ComputeOpenAmount();
        this.trigger.SetUIProperties_Invoices();
        this.trigger.ItemsSource.Collection.forEach(function (item) {
            item.SetUIProperties();
        });
    };
    APPaymentInvoiceArgs.prototype.AdvancedClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 300;
        logWindow.Height = 185;
        logWindow.Title = "Edit Amount to Pay";
        logWindow.WindowArgs = this;
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.ExchangeRate = comp['ExchangeRate'];
                    var inputEntry_INV = comp['ForeignAmount'];
                    var inputEntry_PAY = comp['PaymentAmount'];
                    _this.InputHasError = false;
                    _this.trigger.UpdatePaymentInvoicesErrors();
                    if (!_this.IsConnected) {
                        if (inputEntry_INV != 0 && inputEntry_INV != null) {
                            _this.ConnectedAmount_INV = inputEntry_INV;
                            _this.ConnectedAmount_PAY = inputEntry_PAY;
                            _this.Connect();
                        }
                    }
                    else {
                        if (inputEntry_INV == 0 || inputEntry_INV == null) {
                            _this.IsConnected = false;
                        }
                        else {
                            _this.ConnectedAmount_PAY = inputEntry_PAY;
                            _this.Invoice.AmountPaid = inputEntry_INV;
                            _this.AmountDue = _this.InvoiceAmount - _this.OtherPaymentsAmount - inputEntry_INV;
                            var itemPM = _this.PaymentPM.PaymentInvoices.filter(function (d) { return d.APInvoiceId == _this.Id; })[0];
                            if (itemPM) {
                                itemPM.ForeignAmount = inputEntry_INV;
                                itemPM.PaymentAmount = inputEntry_PAY;
                                itemPM.ExchangeRate = _this.ExchangeRate;
                                if (_this.CurrencyId == _this.LocalCurrencyId) {
                                    itemPM.LocalAmount = itemPM.ForeignAmount;
                                }
                                else {
                                    itemPM.LocalAmount = Tools_1.AppTool.Round(itemPM.ForeignAmount * itemPM.ExchangeRate, 2);
                                }
                            }
                            _this.UpdatePayment();
                        }
                    }
                }
            });
        });
        logWindow.Show('./InvoiceModules/APPayment/Components/EditTabs/APEditMultiCurrency');
    };
    return APPaymentInvoiceArgs;
}(BaseComponent_1.BaseComponent));
exports.APPaymentInvoiceArgs = APPaymentInvoiceArgs;
//# sourceMappingURL=APPaymentDetailsTabComponent.js.map