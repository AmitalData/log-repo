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
var ARInvoiceListService_1 = require("../../../../Invoice/Services/StandardLists/ARInvoiceListService");
var ARPaymentInvoicePM_1 = require("../../../../Invoice/EntityPMs/ARPaymentInvoicePM");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var BankAccountPMService_1 = require("../../../../Accounting/Services/StandardPMs/BankAccountPMService");
var InvoiceDomainService_1 = require("../../../../Invoice/Services/InvoiceDomainService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var AccountingPaymentMethodListService_1 = require("../../../../Invoice/Services/StandardLists/AccountingPaymentMethodListService");
var GLAccountPMService_1 = require("../../../../Accounting/Services/StandardPMs/GLAccountPMService");
var ARPaymentDetailsTabComponent = /** @class */ (function (_super) {
    __extends(ARPaymentDetailsTabComponent, _super);
    function ARPaymentDetailsTabComponent(entityArgs, _entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._entityResourceService = _entityResourceService;
        _this.ObjectTableName = "ARPayment";
        _this.DataContext = _this;
        _this.FullAccounting = false;
        _this.DisplaySATSettings = false;
        _this.IsMultiCurrency = false;
        _this.TransferStatusVisibilityColumn = false;
        _this.EnableNegativeOffsetARPayments = false;
        _this.IsEditExchangeRateVisible = false;
        _this.isRTL = false;
        _this.ARPaymentChequeStatus = "";
        _this.ARPaymentChequeStatusColor = "black";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.AllMethods = [];
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.RateIsEnabled = false;
        // Load Data
        _this.IsDataLoaded = false;
        _this.searchText = "";
        _this.ConnectedList = [];
        _this.IsMatchedList = [];
        _this.myRelativeRateDate = null;
        _this.LastRatesList = [];
        _this.IsCashBookValid = false;
        _this.CreditCardTypeIdVisibility = false;
        _this.BankAccountIdVisibility = false;
        _this.GLAccountNumber = "";
        _this.IsGLAccountCurrencyDifferent = false;
        // Summary
        _this.Summary_Amount = 0;
        _this.Summary_AmountPaid = 0;
        _this.Summary_AmountPaidColor = "#282E30";
        _this.RequestedCommandCode = null;
        _this.RequestedCommandParam = null;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting) {
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
        _this.EntityPM = entityArgs.EntityPM;
        _this.FullAccounting = SessionLocator_1.SessionLocator.TenantPM.AccountingActivated;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.EnableNegativeOffsetARPayments = ObjectsLocator_1.ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetARPayments;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(_this.ObjectTableName, "EnableMultiCurrency")) {
            if (ObjectsLocator_1.ObjectsLocator.AccountingSettingPM.EnableMultiCurrencyARPayments) {
                _this.IsMultiCurrency = true;
            }
        }
        if (SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator_1.SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
            _this.TransferStatusVisibilityColumn = true;
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            _this.DisplaySATSettings = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            _this.IsEditExchangeRateVisible = true;
        }
        _this.SetUIProperties();
        _this.ComputeRelativeRateDate();
        _this.Listen();
        _this.CheckARPaymentCashBook();
        if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.StatusCode) || _this.EntityPM.StatusCode == "DR") {
            _this.LoadCurrencyRates();
        }
        else {
            _this.LoadData();
        }
        return _this;
    }
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "IsNegativeAmountEnabled", {
        get: function () { return this.EnableNegativeOffsetARPayments == true && this.AccountingPaymentMethodCode == "FS" ? true : false; },
        enumerable: true,
        configurable: true
    });
    ARPaymentDetailsTabComponent.prototype.ngOnInit = function () {
        this.LoadPaymentMethods();
    };
    ARPaymentDetailsTabComponent.prototype.LoadPaymentMethods = function () {
        var _this = this;
        var myService = new AccountingPaymentMethodListService_1.AccountingPaymentMethodListService();
        myService.getAll().subscribe(function (response) {
            if (response != null) {
                _this.AllMethods = response.Result;
            }
        });
    };
    ARPaymentDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.LoadData();
                }
                if (_this.RequestedCommandCode) {
                    _this.ApplyRequestedCommand();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.LoadData();
                }
            });
        }
    };
    ARPaymentDetailsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "IsScreenEnabled", {
        // UIProperties
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
    ARPaymentDetailsTabComponent.prototype.SetUIProperties = function () {
        this.SetUIProperties_Cheque();
        this.SetUIProperties_Invoices();
        this.SetUIProperties_CreditCard();
        this.SetUIProperties_BankTransfer();
        this.GetRateIsEnabled();
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
            if (this.FullAccounting) {
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
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BillToId)) {
                this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
            }
            if (this.EntityPM.StatusCode == "VD") {
                this.UIProperties.SetEnabled("PrintNotes", this.ObjectTableName, false);
            }
            if (this.FullAccounting) {
                this.UIProperties.SetEnabled("BankAccountId", this.ObjectTableName, true);
            }
        }
    };
    ARPaymentDetailsTabComponent.prototype.SetUIProperties_Invoices = function () {
        if (!this.IsScreenEnabled) {
            this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, true);
            if (this.EntityPM.PaymentInvoices.length > 0) {
                this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
            }
        }
        this.SetUIProperties_ExchangeRate();
    };
    ARPaymentDetailsTabComponent.prototype.SetUIProperties_ExchangeRate = function () {
        var isEnabled = false;
        if (this.IsScreenEnabled) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARPayment", "ARPaymentEditExchangeRate")) {
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
    ARPaymentDetailsTabComponent.prototype.SetUIProperties_Cheque = function () {
        var _this = this;
        if (this.FullAccounting && this.AccountingPaymentMethodCode == "CH") {
            this.UIProperties.SetRequired("BankBranch", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.BankBranch));
            this.UIProperties.SetRequired("Account", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.Account));
            var service = new InvoiceDomainService_1.InvoiceDomainService();
            service.GetStatusOfARPaymentCheques(this.EntityPM.Id).subscribe(function (myResponse) {
                if (myResponse != null && !myResponse.HasError) {
                    _this.ARPaymentChequeStatus = myResponse.Result;
                    if (_this.ARPaymentChequeStatus == "בקופה" || _this.ARPaymentChequeStatus == "משמרת" || _this.ARPaymentChequeStatus == "הופקד- טרם נפרע") {
                        _this.ARPaymentChequeStatusColor = "orange";
                    }
                    else if (_this.ARPaymentChequeStatus == "הוחזר ללקוח") {
                        _this.ARPaymentChequeStatusColor = "red";
                    }
                    else if (_this.ARPaymentChequeStatus == "נפרע") {
                        _this.ARPaymentChequeStatusColor = "green";
                    }
                }
            });
        }
        else {
            this.UIProperties.SetRequired("BankBranch", this.ObjectTableName, false);
            this.UIProperties.SetRequired("Account", this.ObjectTableName, false);
        }
        this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, false);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AccountingPaymentMethodCode)) {
            if (this.AccountingPaymentMethodCode == "CH") {
                if (Tools_1.AppTool.IsNullOrEmpty(this.ChequeOrPaymentRef)) {
                    this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, true);
                }
                this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
            }
        }
    };
    ARPaymentDetailsTabComponent.prototype.SetUIProperties_CreditCard = function () {
        this.UIProperties.SetVisibility("CreditCardTypeId", this.ObjectTableName, false);
        this.CreditCardTypeIdVisibility = false;
        this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, false);
        if (this.AccountingPaymentMethodCode == "CC") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.CreditCardTypeId)) {
                this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, true);
            }
            this.UIProperties.SetVisibility("CreditCardTypeId", this.ObjectTableName, true);
            this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
            this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
            this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
            this.CreditCardTypeIdVisibility = true;
        }
    };
    ARPaymentDetailsTabComponent.prototype.SetUIProperties_BankTransfer = function () {
        this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, false);
        if (this.FullAccounting == true && this.AccountingPaymentMethodCode == "BT") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.BankAccountId)) {
                this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, true);
            }
            this.UIProperties.SetVisibility("BankAccountId", this.ObjectTableName, true);
            if (!this.FullAccounting) {
                this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
            }
            else {
                this.UIProperties.SetVisibility("Bank", this.ObjectTableName, false);
                this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, false);
                this.UIProperties.SetVisibility("Account", this.ObjectTableName, false);
            }
            this.BankAccountIdVisibility = true;
        }
        else {
            this.UIProperties.SetVisibility("BankAccountId", this.ObjectTableName, false);
            this.BankAccountIdVisibility = false;
        }
    };
    ARPaymentDetailsTabComponent.prototype.GetRateIsEnabled = function () {
        this.SetUIProperties_ExchangeRate();
    };
    ARPaymentDetailsTabComponent.prototype.SearchTextKeyUp = function (args) {
        this.searchText = args;
        this.LoadData();
    };
    ARPaymentDetailsTabComponent.prototype.LoadData = function () {
        this.ItemsSource.Clear();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BillToId) && this.EntityPM.StatusCode != "VD") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (this.EntityPM.PaymentInvoices.length == 1) {
                    this.LoadPaymentInvoices_Created();
                    //var iConnectedItem = new ARInvoiceList();
                    //iConnectedItem.Id = this.EntityPM.PaymentInvoices[0].ARInvoiceId;
                    //iConnectedItem.InvoiceCurrencyId = this.EntityPM.PaymentInvoices[0].ForeignCurrencyId;
                    //iConnectedItem.InvoiceCurrencyExchangeRate = this.EntityPM.PaymentInvoices[0].ExchangeRate;
                    //iConnectedItem.AmountInInvoiceCurrency = this.EntityPM.PaymentInvoices[0].ForeignAmount;
                    //iConnectedItem.AmountInLocalCurrency = this.EntityPM.PaymentInvoices[0].LocalAmount;
                    //iConnectedItem.Id = this.EntityPM.PaymentInvoices[0].ARInvoiceId;
                    //iConnectedItem.MetodoPagoCode = this.EntityPM.PaymentInvoices[0].ARInvoiceMetodoPagoCode;
                    //this.ConnectedList.push(iConnectedItem);
                }
                else {
                    this.LoadPaymentInvoices_IsMatched();
                }
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
    ARPaymentDetailsTabComponent.prototype.LoadPaymentInvoices_Created = function () {
        var _this = this;
        var invoiceId = this.EntityPM.PaymentInvoices[0].ARInvoiceId;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1;
        filters.SortBy = "InvoiceDate";
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("Id", invoiceId, null, null, "Equals", false, false, false, "string");
        var myService = new ARInvoiceListService_1.ARInvoiceListService();
        myService.getByFilters(filters).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ConnectedList = myResponse.Result;
                _this.LoadPaymentInvoices_IsMatched();
            }
        });
    };
    ARPaymentDetailsTabComponent.prototype.LoadPaymentInvoices_Connected = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1000;
        filters.SortBy = "InvoiceDate";
        filters.SortDirection = "Descending";
        var searchValue = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.searchText)) {
            searchValue = Tools_1.AppTool.IsNullOrEmpty(this.searchText.trim()) ? null : this.searchText;
        }
        filters.addAdditionalFilter("ARPaymentInvoicesSearch", searchValue, null, null, "Contains", true, false, false, "string");
        filters.addAdditionalFilter("ARPaymentInvoicesConnected", this.EntityPM.Id, null, null, "Contains", true, false, false, "string");
        var myService = new ARInvoiceListService_1.ARInvoiceListService();
        myService.getByFilters(filters).subscribe(function (myResponse) {
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
    ARPaymentDetailsTabComponent.prototype.LoadPaymentInvoices_IsMatched = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1000;
        filters.SortBy = "InvoiceDate";
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("BillToId", this.EntityPM.BillToId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("StatusCode", "DR,AD,PP,PD", null, null, "InList", false, true, false, "string");
        filters.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("IsConstituentInvoice", false, null, null, "Equals", false, false, false, "Boolean");
        var searchValue = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.searchText)) {
            searchValue = Tools_1.AppTool.IsNullOrEmpty(this.searchText.trim()) ? null : this.searchText;
        }
        filters.addAdditionalFilter("ARPaymentInvoicesSearch", searchValue, null, null, "Contains", true, false, false, "string");
        var myService = new ARInvoiceListService_1.ARInvoiceListService();
        myService.getByFilters(filters).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.IsMatchedList = myResponse.Result;
            }
            _this.FillBaselist();
        });
    };
    ARPaymentDetailsTabComponent.prototype.FillBaselist = function () {
        var _this = this;
        this.ItemsSource.Clear();
        var connectedList = [];
        var unConnectedMatchedList = [];
        var unConnectedListNotMatched = [];
        var itemsCollection = [];
        if (this.ConnectedList.length > 0) {
            this.ConnectedList.forEach(function (item) {
                if (_this.EntityPM.PaymentInvoices.filter(function (d) { return d.ARInvoiceId == item.Id; })[0]) {
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
                    connectedList.push(new ARPaymentInvoiceArgs(item, _this));
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
                        if (isCurrencyMatched == false || item.StatusCode == "DR") {
                            unConnectedListNotMatched.push(new ARPaymentInvoiceArgs(item, _this));
                        }
                        else {
                            unConnectedMatchedList.push(new ARPaymentInvoiceArgs(item, _this));
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
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "BillToId", {
        // BillTo
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.BillToId;
        },
        set: function (value) {
            var _this = this;
            if (this.EntityPM != null) {
                if (this.EntityPM.BillToId != value) {
                    this.EntityPM.BillToId = value;
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BillToId)) {
                        this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
                    }
                    else {
                        this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, true);
                    }
                    this.LoadData();
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BillToId)) {
                        this.BillToAddressId = null;
                        this.EntityPM.BillToName = null;
                        this.EntityPM.BillToPartnerTypeId = null;
                        this.PaymentCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
                    }
                    else {
                        var myService = new CardListService_1.CardListService();
                        myService.getSingle(this.EntityPM.BillToId).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var list = myResponse.Result;
                                if (list) {
                                    _this.EntityPM.BillToName = list.EnglishName;
                                    _this.EntityPM.BillToPartnerTypeId = list.PartnerTypeId;
                                    if (!Tools_1.AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                                        _this.PaymentCurrencyId = list.InvoiceCurrencyId;
                                    }
                                    _this.LoadAddress();
                                    if (_this.FullAccounting) {
                                        if (!Tools_1.AppTool.IsNullOrEmpty(list.GLAccountId)) {
                                            var myGLAccountPMService = new GLAccountPMService_1.GLAccountPMService();
                                            myGLAccountPMService.get(list.GLAccountId).subscribe(function (myResponse) {
                                                if (!myResponse.HasError) {
                                                    var glaccount = myResponse.Result;
                                                    if (glaccount != null && !glaccount.IsMultiCurrency) {
                                                        _this.PaymentCurrencyId = glaccount.CurrencyId;
                                                    }
                                                }
                                            });
                                        }
                                    }
                                }
                            }
                        });
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "BillToAddressId", {
        get: function () { return this.EntityPM.BillToAddressId; },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.BillToAddressId != value) {
                    this.EntityPM.BillToAddressId = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ARPaymentDetailsTabComponent.prototype.LoadAddress = function () {
        var _this = this;
        var myService = new PartnersDomainService_1.PartnersDomainService();
        myService.GetBillingAddressListByCardId(this.EntityPM.BillToId).subscribe(function (resp) {
            if (resp != null) {
                var billingAddress = resp;
                if (billingAddress != null) {
                    _this.BillToAddressId = billingAddress.Id;
                }
                else {
                    var myService = new AddressListService_1.AddressListService();
                    myService.getSingle(_this.EntityPM.BillToId).subscribe(function (myResult) {
                        var myResponse = myResult;
                        if (!myResponse.HasError) {
                            var billingAddress = myResponse.Result;
                            if (billingAddress != null) {
                                _this.BillToAddressId = billingAddress.Id;
                            }
                            else {
                                var myService = new PartnersDomainService_1.PartnersDomainService();
                                myService.GetMainAddressListByCardId(_this.EntityPM.BillToId).subscribe(function (resp) {
                                    if (resp != null) {
                                        var mainAddress = resp;
                                        if (mainAddress != null) {
                                            _this.BillToAddressId = mainAddress.Id;
                                        }
                                        else {
                                            _this.BillToAddressId = null;
                                        }
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    };
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "PaymentCurrencyId", {
        // Currency 
        get: function () { return this.EntityPM.PaymentCurrencyId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.PaymentCurrencyId != value) {
                this.EntityPM.PaymentCurrencyId = value;
                this.CheckARPaymentCashBook();
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
                if (this.FullAccounting == true && this.AccountingPaymentMethodCode == "BT") {
                    this.CheckGLAccountCurrencyId();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "PaymentCurrencyCode", {
        get: function () { return this.EntityPM.PaymentCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.PaymentCurrencyCode != value) {
                this.EntityPM.PaymentCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "PaymentCurrencyExchangeRate", {
        get: function () { return this.EntityPM.PaymentCurrencyExchangeRate; },
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
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "ExchangeRateDate", {
        get: function () { return this.EntityPM.ExchangeRateDate; },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.ExchangeRateDate != value) {
                    this.EntityPM.ExchangeRateDate = value;
                    this.ComputeRelativeRateDate();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "RelativeRateDate", {
        get: function () { return this.myRelativeRateDate; },
        set: function (value) {
            if (this.myRelativeRateDate != value) {
                this.myRelativeRateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARPaymentDetailsTabComponent.prototype.ComputeRelativeRateDate = function () {
        this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(this.RegisterDate, this.ExchangeRateDate, "old");
    };
    ARPaymentDetailsTabComponent.prototype.LoadCurrencyRates = function () {
        var _this = this;
        var loadingDate = this.EntityPM.RegisterDate;
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
    ARPaymentDetailsTabComponent.prototype.UpdateCurrencyRates = function () {
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
    ARPaymentDetailsTabComponent.prototype.SetCurrencyRateData = function () {
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
    ARPaymentDetailsTabComponent.prototype.GetCurrencyRate = function (currencyId) {
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
    ARPaymentDetailsTabComponent.prototype.GetCurrencyRateDate = function (currencyId) {
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
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "RegisterDate", {
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
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "AccountingPaymentMethodId", {
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
                    //this.CheckARPaymentCashBook();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "AccountingPaymentMethodCode", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.AccountingPaymentMethodCode;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.AccountingPaymentMethodCode != value) {
                    this.EntityPM.AccountingPaymentMethodCode = value;
                    this.CheckARPaymentCashBook();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "SATPaymentMethodCode", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.SATPaymentMethodCode;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.SATPaymentMethodCode != value) {
                    this.EntityPM.SATPaymentMethodCode = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "BranchId", {
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
                    this.CheckARPaymentCashBook();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ARPaymentDetailsTabComponent.prototype.CheckARPaymentCashBook = function () {
        var _this = this;
        this.IsCashBookValid = false;
        if (this.FullAccounting && (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "CH")) {
            var service = new InvoiceDomainService_1.InvoiceDomainService();
            service.CheckARPaymentCashBook(this.AccountingPaymentMethodCode, this.PaymentCurrencyId, this.BranchId).subscribe(function (myResponse) {
                if (myResponse != null && !myResponse.HasError) {
                    var cashbook = myResponse.Result;
                    if (cashbook != null && cashbook.length > 0) {
                        if (cashbook.length == 1) {
                            var data = cashbook[0];
                            _this.CashBookName = data.EnglishName;
                            _this.BranchGLAccountNumber = data.AccountNumber;
                            _this.BranchGLAccountId = data.AccountId;
                            _this.IsCashBookValid = true;
                            if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.CashbookId)) {
                                _this.EntityPM.CashbookId = data.Id;
                            }
                            _this.UIProperties.SetValidity("BranchId", _this.ObjectTableName, true, "");
                        }
                        else {
                            var branchesText = "";
                            cashbook.forEach(function (item) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(item.EnglishName)) {
                                    branchesText += item.EnglishName + ",";
                                }
                            });
                            branchesText = branchesText.replace(/,\s*$/, "");
                            var msg = "There is no cashbook for this branch, Cashbooks for branches: " + branchesText + "  was found, change the branch please";
                            _this.UIProperties.SetValidity("BranchId", _this.ObjectTableName, false, msg);
                        }
                    }
                    else {
                        var msg = "There is no cashbook that compatible to this ARPayment, create one please";
                        _this.UIProperties.SetValidity("BranchId", _this.ObjectTableName, false, msg);
                    }
                }
            });
        }
    };
    //Payment Line Properties
    ARPaymentDetailsTabComponent.prototype.RefreshPaymentMethodFields = function () {
        var _this = this;
        this.Bank = null;
        this.BankBranch = null;
        this.Account = null;
        this.ChequeOrPaymentRef = null;
        this.ValueDate = null;
        this.CreditCardTypeId = null;
        var lists = this.AllMethods.filter(function (d) { return d.Id == _this.AccountingPaymentMethodId; });
        if (lists) {
            var list = lists[0];
            if (list) {
                this.AccountingPaymentMethodCode = list.Code;
            }
        }
        if (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "FS") {
            this.ValueDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        this.SetUIProperties_Cheque();
        this.SetUIProperties_CreditCard();
        this.SetUIProperties_BankTransfer();
    };
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "PaymentMethodDetailsLabel", {
        get: function () {
            var _this = this;
            var result = "";
            if (this.AccountingPaymentMethodCode == "CH") {
                result = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.S.Details.Cheque");
            }
            else if (this.AccountingPaymentMethodCode == "FS") {
                result = "Offsetting";
            }
            else if (this.AccountingPaymentMethodCode == "BT") {
                result = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.S.Details.BankTransfer");
            }
            else if (this.AccountingPaymentMethodCode == "CC") {
                result = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.S.Details.CreditCard");
            }
            else {
                var method = this.AllMethods.filter(function (d) { return d.Code == _this.AccountingPaymentMethodCode; })[0];
                if (method != null) {
                    result = method.Name;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "ChequePaymentRefLabel", {
        get: function () {
            var result = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.S.Details.PaymentRef");
            if (this.AccountingPaymentMethodCode == "CH") {
                result = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.S.Details.ChequeRef");
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "VisibleIfCash", {
        get: function () {
            var result = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.AccountingPaymentMethodCode)) {
                if (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "FS") {
                    result = true;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "CollapsedIfCash", {
        get: function () {
            var result = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.AccountingPaymentMethodCode)) {
                if (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "FS") {
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
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "Bank", {
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
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "BankBranch", {
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
                    if (this.FullAccounting && this.AccountingPaymentMethodCode == "CH") {
                        if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                            this.UIProperties.SetRequired("BankBranch", this.ObjectTableName, false);
                        }
                        else {
                            this.UIProperties.SetRequired("BankBranch", this.ObjectTableName, true);
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "Account", {
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
                    if (this.FullAccounting && this.AccountingPaymentMethodCode == "CH") {
                        if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                            this.UIProperties.SetRequired("Account", this.ObjectTableName, false);
                        }
                        else {
                            this.UIProperties.SetRequired("Account", this.ObjectTableName, true);
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "ValueDate", {
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
                    //if (value != null) {
                    //    this.UIProperties.SetRequired("ValueDate", this.ObjectTableName, false);
                    //    this.UIProperties.SetValidity("ValueDate", this.ObjectTableName, false, null);
                    //}
                    //else {
                    //    this.UIProperties.SetRequired("ValueDate", this.ObjectTableName, true);
                    //    this.UIProperties.SetValidity("ValueDate", this.ObjectTableName, true, null);
                    //}
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "ChequeOrPaymentRef", {
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
                    this.SetUIProperties_Cheque();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "CreditCardTypeId", {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "BankAccountId", {
        get: function () {
            if (this.EntityPM == null) {
                return null;
            }
            return this.EntityPM.BankAccountId;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                if (this.EntityPM.BankAccountId != value) {
                    this.EntityPM.BankAccountId = value;
                    this.CheckGLAccountCurrencyId();
                    if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                        this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, false);
                    }
                    else {
                        this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, true);
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ARPaymentDetailsTabComponent.prototype.CheckGLAccountCurrencyId = function () {
        var _this = this;
        var service = new BankAccountPMService_1.BankAccountPMService();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BankAccountId)) {
            service.get(this.BankAccountId).subscribe(function (myResponse) {
                if (myResponse != null && !myResponse.HasError) {
                    _this.bankAccount = myResponse.Result;
                    if (_this.bankAccount != null && _this.bankAccount.GLAccountCurrencyId != null && _this.bankAccount.GLAccountCurrencyId != "multi") {
                        if (_this.bankAccount.GLAccountCurrencyId != _this.PaymentCurrencyId) {
                            _this.GLAccountNumber = _this.bankAccount.GLAccountNumber;
                            _this.IsGLAccountCurrencyDifferent = true;
                            var msg = "The currency of the bank account GLAccount (" + _this.GLAccountNumber + ") is different from ARPayment curreny";
                            _this.UIProperties.SetValidity("BankAccountId", _this.ObjectTableName, false, msg);
                        }
                        else {
                            _this.IsGLAccountCurrencyDifferent = false;
                            _this.UIProperties.SetValidity("BankAccountId", _this.ObjectTableName, true, "");
                        }
                    }
                    else {
                        _this.IsGLAccountCurrencyDifferent = false;
                        _this.UIProperties.SetValidity("BankAccountId", _this.ObjectTableName, true, "");
                    }
                }
            });
        }
        else {
            this.IsGLAccountCurrencyDifferent = false;
            this.UIProperties.SetValidity("BankAccountId", this.ObjectTableName, true, "");
        }
    };
    ARPaymentDetailsTabComponent.prototype.EditGLAccount = function (arg) {
        var _this = this;
        if (arg == "BA") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.bankAccount.GLAccountId)) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: _this.bankAccount.GLAccountId, ObjectTableName: 'GLAccount' });
                });
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.BranchGLAccountId)) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: _this.BranchGLAccountId, ObjectTableName: 'GLAccount' });
                });
            }
        }
    };
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "AmountInPaymentCurrency", {
        // Amounts
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
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "AmountInLocalCurrency", {
        get: function () { return this.EntityPM.AmountInLocalCurrency; },
        set: function (value) {
            if (this.EntityPM.AmountInLocalCurrency != value) {
                this.EntityPM.AmountInLocalCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "OpenAmount", {
        get: function () { return this.EntityPM.OpenAmount == null ? 0 : this.EntityPM.OpenAmount; },
        set: function (value) {
            if (this.EntityPM.OpenAmount != value) {
                this.EntityPM.OpenAmount = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsTabComponent.prototype, "PrintNotes", {
        get: function () { return this.EntityPM.PrintNotes; },
        set: function (value) {
            if (this.EntityPM.PrintNotes != value) {
                this.EntityPM.PrintNotes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARPaymentDetailsTabComponent.prototype.ComputeOpenAmount = function () {
        this.OpenAmount = this.AmountInPaymentCurrency - this.Summary_AmountPaid;
    };
    ARPaymentDetailsTabComponent.prototype.ComputeLocalAmount = function () {
        this.AmountInLocalCurrency = this.AmountInPaymentCurrency * this.PaymentCurrencyExchangeRate;
    };
    ARPaymentDetailsTabComponent.prototype.UpdateSummary = function () {
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
    ARPaymentDetailsTabComponent.prototype.ViewEntity = function (args) {
        if (args) {
            this.RequestedCommandCode = "ViewInvoice";
            this.RequestedCommandParam = args.Id;
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    ARPaymentDetailsTabComponent.prototype.UpdateCurrencyRateMethod = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("RatesTable", 0).subscribe(function (response) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Update Currency Rate";
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
    ARPaymentDetailsTabComponent.prototype.UpdatePaymentInvoicesErrors = function () {
        var haserrors = false;
        if (this.ItemsSource.Collection.filter(function (d) { return d.InputHasError; })[0]) {
            haserrors = true;
        }
        this.EntityPM.HasInvoicesErrors = haserrors;
    };
    ARPaymentDetailsTabComponent.prototype.ApplyRequestedCommand = function () {
        var _this = this;
        if (this.RequestedCommandCode == "ViewInvoice") {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this.RequestedCommandParam, ObjectTableName: 'ARInvoice' });
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
    ARPaymentDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARPaymentDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ARPaymentDetailsTabComponent);
    return ARPaymentDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ARPaymentDetailsTabComponent = ARPaymentDetailsTabComponent;
var ARPaymentInvoiceArgs = /** @class */ (function (_super) {
    __extends(ARPaymentInvoiceArgs, _super);
    function ARPaymentInvoiceArgs(item, trigger) {
        var _this = _super.call(this) || this;
        _this.trigger = trigger;
        _this.EntityPM = null;
        _this.Invoice = null;
        _this.PaymentPM = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "ARInvoice";
        _this.SortingValue = 0;
        _this.LocalCurrencyId = null;
        _this.IsMultiCurrency = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Id = null;
        _this.DueDate = null;
        _this.Status = null;
        _this.InvoiceNumber = null;
        _this.TransferStatus = null;
        _this.SATTransferStatus = null;
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
        _this.IsAdvancedButtonVisible = false;
        _this.CheckBoxEnabled = true;
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
        _this.EntityPM = _this.PaymentPM.PaymentInvoices.filter(function (d) { return d.ARInvoiceId == _this.Invoice.Id; })[0];
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
    ARPaymentInvoiceArgs.prototype.InitProperties = function () {
        this.Id = this.Invoice.Id;
        this.DueDate = this.Invoice.DueDate;
        this.Status = this.Invoice.StatusName;
        this.InvoiceNumber = this.Invoice.InvoiceNumber;
        this.TransferStatus = this.Invoice.TransferStatusName;
        this.CurrencyId = this.Invoice.InvoiceCurrencyId;
        this.CurrencyCode = this.Invoice.InvoiceCurrencyCode;
        this.InvoiceAmount = this.Invoice.AmountInInvoiceCurrency == null ? 0 : this.Invoice.AmountInInvoiceCurrency;
        this.ShipmentNumber = this.Invoice.IsConsolidationInvoice ? "List" : this.Invoice.MainEntityReference;
        this.SATTransferStatus = this.Invoice.SATInvoiceStatusName;
        this.TransferStatusCode = this.Invoice.TransferStatusCode;
    };
    ARPaymentInvoiceArgs.prototype.InitAllAmounts = function () {
        var myInvoiceAmount = 0;
        var myAmountDue = 0;
        var myOtherAmounts = 0;
        if (this.Invoice.AmountInInvoiceCurrency != null) {
            myInvoiceAmount = this.Invoice.AmountInInvoiceCurrency;
        }
        if (this.Invoice.AmountDue != null) {
            myAmountDue = this.Invoice.AmountDue;
        }
        if (this.Invoice.ARInvoiceTypeCode == "CD" || this.Invoice.ARInvoiceTypeCode == "CC") {
            if (myInvoiceAmount > 0) {
                myInvoiceAmount = myInvoiceAmount * -1;
            }
            if (myAmountDue > 0) {
                myAmountDue = myAmountDue * -1;
            }
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
    ARPaymentInvoiceArgs.prototype.InitExchangeRate = function () {
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
    ARPaymentInvoiceArgs.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("AmountToPay", this.ObjectTableName, true);
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
                if (this.Invoice.ARInvoiceTypeCode == "IN") {
                    this.isControlEnabled = false;
                }
            }
        }
        if (this.isConnected) {
            this.isControlEnabled = true;
            this.CheckBoxVisibility = true;
            this.NotMatchedVisibility = false;
            if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE" && this.PaymentPM.SATTransferStatusCode != undefined && this.PaymentPM.SATTransferStatusCode != "NT" && this.PaymentPM.SATTransferStatusCode != "TE" &&
                !(this.PaymentPM.SATTransferStatusCode == "TD" && (this.PaymentPM.StatusCode == "DR"))) {
                this.CheckBoxEnabled = false;
            }
        }
        else {
            if (!this.isAllowedToConnect) {
                this.CheckBoxVisibility = false;
                this.NotMatchedVisibility = true;
                this.IsAdvancedButtonVisible = false;
            }
            else if (this.trigger.EntityPM.OpenAmount <= 0) {
                if (this.Invoice.ARInvoiceTypeCode == "CD" || this.Invoice.ARInvoiceTypeCode == "CC") {
                    this.CheckBoxVisibility = true;
                }
                if (this.Invoice.ARInvoiceTypeCode == "IN") {
                    this.NotMatchedVisibility = false;
                }
            }
            else {
                this.CheckBoxVisibility = true;
            }
        }
        this.SetUIProperties_AmountPaidEnabled();
        this.SetLineColors();
    };
    ARPaymentInvoiceArgs.prototype.SetUIProperties_CurrencyMatched = function () {
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
    ARPaymentInvoiceArgs.prototype.SetUIProperties_AllowedToConnect = function () {
        this.isAllowedToConnect = true;
        if (this.IsConnected == false) {
            if (this.Invoice.StatusCode == "DR") {
                this.isAllowedToConnect = false;
            }
            if (this.isCurrencyMatched == false) {
                this.isAllowedToConnect = false;
            }
            if (Tools_1.AppTool.IsNullOrZero(this.AmountDue)) {
                this.isAllowedToConnect = false;
            }
            if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
                if ((this.Invoice.MetodoPagoCode != this.PaymentPM.MetodoPagoCode && !Tools_1.AppTool.IsNullOrEmpty(this.PaymentPM.MetodoPagoCode)) || (this.PaymentPM.MetodoPagoCode == "PUE" && this.Invoice.StatusCode != "AD") && this.PaymentPM.OpenAmount >= this.Invoice.AmountPaid) {
                    this.isAllowedToConnect = false;
                }
            }
        }
    };
    ARPaymentInvoiceArgs.prototype.SetUIProperties_AmountPaidEnabled = function () {
        var isEnabled = true;
        if (this.Invoice.StatusCode == "DR") {
            isEnabled = false;
        }
        else if (this.isCurrencyMatched == false) {
            isEnabled = false;
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            if (this.Invoice.MetodoPagoCode == this.PaymentPM.MetodoPagoCode && (this.PaymentPM.MetodoPagoCode == "PUE")) {
                //this.UIProperties.SetEnabled("AmountToPay", this.ObjectTableName, false);
                isEnabled = false;
            }
        }
        this.UIProperties.SetEnabled("AmountPaid", this.ObjectTableName, isEnabled);
    };
    ARPaymentInvoiceArgs.prototype.SetLineColors = function () {
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
            if (this.Invoice.StatusCode == "DR") {
                this.StatusBackground = "rgba(255, 171,3, 0.6)";
            }
        }
    };
    Object.defineProperty(ARPaymentInvoiceArgs.prototype, "IsConnected", {
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
                    var itemPM = this.PaymentPM.PaymentInvoices.filter(function (d) { return d.ARInvoiceId == _this.Id; })[0];
                    if (itemPM) {
                        this.PaymentPM.RemoveARPaymentInvoicePM(itemPM);
                        this.Disconnect();
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ARPaymentInvoiceArgs.prototype.GetSmallestAmount = function () {
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
    ARPaymentInvoiceArgs.prototype.GetConnectedAmount_PAY = function (invoiceAmount) {
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
    Object.defineProperty(ARPaymentInvoiceArgs.prototype, "AmountDue", {
        get: function () { return this.amountDue == null ? 0 : this.amountDue; },
        set: function (value) {
            if (this.amountDue != value) {
                this.amountDue = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentInvoiceArgs.prototype, "AmountPaid", {
        get: function () { return this.Invoice.AmountPaid == null ? 0 : this.Invoice.AmountPaid; },
        set: function (value) {
            var _this = this;
            if (this.Invoice.AmountPaid != value || this.InputHasError) {
                this.Invoice.AmountPaid = value;
                var inputErrors = 0;
                var inputEntry = value == null ? 0 : Tools_1.AppTool.Round(value, 2);
                var allowedAmount = Tools_1.AppTool.Round(this.InvoiceAmount - this.OtherPaymentsAmount, 2);
                this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, true, "");
                if (this.Invoice.ARInvoiceTypeCode == "CD" || this.Invoice.ARInvoiceTypeCode == "CC") {
                    if (inputEntry > 0) {
                        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.OnlyMinusValue");
                        this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                        inputErrors++;
                    }
                    else if (inputEntry < allowedAmount) {
                        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.AmountPaidNotLess") + " " + allowedAmount;
                        this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                        inputErrors++;
                    }
                }
                else {
                    if (inputEntry < 0) {
                        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.CantPayMinusValue");
                        this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, false, msg);
                        inputErrors++;
                    }
                    else if (inputEntry > allowedAmount) {
                        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.M.AmountPaidLessOrEqual") + " " + allowedAmount;
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
                            var itemPM = this.PaymentPM.PaymentInvoices.filter(function (d) { return d.ARInvoiceId == _this.Id; })[0];
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
    ARPaymentInvoiceArgs.prototype.Connect = function () {
        this.isConnected = true;
        var itemPM = new ARPaymentInvoicePM_1.ARPaymentInvoicePM(this.PaymentPM);
        itemPM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
        itemPM.ARInvoiceId = this.Id;
        itemPM.ARPaymentId = this.PaymentPM.Id;
        itemPM.ARInvoiceNumber = this.InvoiceNumber;
        itemPM.ForeignCurrencyId = this.CurrencyId;
        itemPM.ExchangeRate = this.ExchangeRate;
        itemPM.ForeignAmount = this.ConnectedAmount_INV == null ? 0 : Tools_1.AppTool.Round(this.ConnectedAmount_INV, 2);
        itemPM.PaymentAmount = this.ConnectedAmount_PAY == null ? 0 : Tools_1.AppTool.Round(this.ConnectedAmount_PAY, 2);
        itemPM.ARInvoiceMetodoPagoCode = this.Invoice.MetodoPagoCode;
        itemPM.ARInvoiceTransferStatusCode = this.TransferStatusCode;
        if (this.CurrencyId == this.LocalCurrencyId) {
            itemPM.LocalAmount = itemPM.ForeignAmount;
        }
        else {
            itemPM.LocalAmount = Tools_1.AppTool.Round(itemPM.ForeignAmount * itemPM.ExchangeRate, 2);
        }
        this.PaymentPM.AddARPaymentInvoicePM(itemPM);
        var invoiceAmount = this.InvoiceAmount;
        var invoiceAmountPaid = itemPM.ForeignAmount;
        var invoiceAmountDue = invoiceAmount - this.OtherPaymentsAmount - invoiceAmountPaid;
        if (this.Invoice.ARInvoiceTypeCode == "CD" || this.Invoice.ARInvoiceTypeCode == "CC") {
            if (invoiceAmountDue > 0) {
                invoiceAmountDue = invoiceAmountDue * -1;
            }
            if (invoiceAmountPaid > 0) {
                invoiceAmountPaid = invoiceAmountPaid * -1;
            }
        }
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            if (this.Invoice.MetodoPagoCode == this.PaymentPM.MetodoPagoCode && (this.PaymentPM.MetodoPagoCode == "PUE" && this.Invoice.StatusCode == "AD")) {
                this.AmountPaid = invoiceAmountPaid = this.AmountDue;
                invoiceAmountDue = 0;
            }
        }
        this.AmountDue = Tools_1.AppTool.Round(invoiceAmountDue, 2);
        this.Invoice.AmountPaid = Tools_1.AppTool.Round(invoiceAmountPaid, 2);
        this.SetLineColors();
        this.UpdatePayment();
    };
    ARPaymentInvoiceArgs.prototype.Disconnect = function () {
        this.AmountDue = Tools_1.AppTool.Round(this.InvoiceAmount - this.OtherPaymentsAmount, 2);
        this.Invoice.AmountPaid = 0;
        this.UIProperties.SetValidity("AmountPaid", this.ObjectTableName, true, "");
        this.InputHasError = false;
        this.ConnectedAmount_INV = 0;
        this.ConnectedAmount_PAY = 0;
        this.SetLineColors();
        this.UpdatePayment();
    };
    ARPaymentInvoiceArgs.prototype.UpdatePayment = function () {
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
        if (this.PaymentPM.PaymentInvoices.length > 0) {
            this.PaymentPM.UIProperties.SetEnabled("MetodoPagoCode", "ARPayment", false);
        }
        else {
            this.PaymentPM.UIProperties.SetEnabled("MetodoPagoCode", "ARPayment", true);
        }
    };
    ARPaymentInvoiceArgs.prototype.AdvancedClicked = function () {
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
                            var itemPM = _this.PaymentPM.PaymentInvoices.filter(function (d) { return d.ARInvoiceId == _this.Id; })[0];
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
        logWindow.Show('./InvoiceModules/ARPayment/Components/EditTabs/EditMultiCurrency');
    };
    return ARPaymentInvoiceArgs;
}(BaseComponent_1.BaseComponent));
exports.ARPaymentInvoiceArgs = ARPaymentInvoiceArgs;
//# sourceMappingURL=ARPaymentDetailsTabComponent.js.map