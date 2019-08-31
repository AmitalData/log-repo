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
var ReconciliationExtendedPMService_1 = require("./../../../../Accounting/Services/ExtendedPMs/ReconciliationExtendedPMService");
var GLAccountListService_1 = require("./../../../../Accounting/Services/StandardLists/GLAccountListService");
var AccountingEntityHelper_1 = require("./../../../../Accounting/Utilities/AccountingEntityHelper");
var LedgerTransactionExtendedListService_1 = require("./../../../../Accounting/Services/ExtendedLists/LedgerTransactionExtendedListService");
var core_1 = require("@angular/core");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CurrencyListService_1 = require("../../../../Common/Services/StandardLists/CurrencyListService");
var AddressListService_1 = require("../../../../Common/Services/StandardLists/AddressListService");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var ARInvoiceListService_1 = require("../../../../Invoice/Services/StandardLists/ARInvoiceListService");
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
var ARPaymentDetailsFullAccountingTab = /** @class */ (function (_super) {
    __extends(ARPaymentDetailsFullAccountingTab, _super);
    function ARPaymentDetailsFullAccountingTab(entityArgs, _entityResourceService) {
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
        _this._LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService_1.LedgerTransactionExtendedListService();
        _this._ReconciliationExtendedPMService = new ReconciliationExtendedPMService_1.ReconciliationExtendedPMService();
        _this._glaService = new GLAccountListService_1.GLAccountListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.paymentAmountTotal = 0;
        _this.amount2reconcileTotal = 0;
        _this.paymentReconciledAmountTotal = 0;
        _this._loading = false;
        //#endregion
        //#region old code
        _this.AllMethods = [];
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        // UIProperties
        _this.RateIsEnabled = true;
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
        console.log("[FULL ACCOUNING ARPayment]");
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting) {
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
        _this.EntityPM = entityArgs.EntityPM;
        _this.FullAccounting = SessionLocator_1.SessionLocator.TenantPM.AccountingActivated;
        _this.originalPaymentOpenAmount = _this.EntityPM.OpenAmount;
        _this.paymentAmountTotal = _this.EntityPM.AmountInPaymentCurrency;
        _this.TransactionsList = new ObservableCollection_1.ObservableCollection([]);
        //#region old
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
        //#endregion
        _this.GetData();
        return _this;
        // this.UIProperties.SetEnabled("AmountToReconcile","LedgerTransaction",!this.IsGridReadOnly);
    }
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "IsNegativeAmountEnabled", {
        get: function () { return this.EnableNegativeOffsetARPayments == true && this.AccountingPaymentMethodCode == "FS" ? true : false; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "TextStore", {
        get: function () {
            return TextStore;
        },
        enumerable: true,
        configurable: true
    });
    ARPaymentDetailsFullAccountingTab.prototype.ngOnInit = function () {
        this.LoadPaymentMethods();
    };
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "IsGridReadOnly", {
        // IsEntityValid: boolean = true;
        get: function () {
            // return this.EntityPM.StatusCode == 'CL' || this.EntityPM.StatusCode == 'VD' || this.EntityPM.OpenAmount == 0;
            return this.EntityPM.StatusCode == 'CL' || this.EntityPM.StatusCode == 'VD';
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "IsEntityValid", {
        get: function () {
            var _valid = true;
            _valid = this.TransactionsList.Collection.every(function (d) { return d.isLineValid == true; });
            return _valid;
        },
        enumerable: true,
        configurable: true
    });
    ARPaymentDetailsFullAccountingTab.prototype.SetEntityValidity = function () {
        this.CurrentSession.CurrentEditComponent.IsEditValid = this.IsEntityValid;
    };
    ARPaymentDetailsFullAccountingTab.prototype.ReloadGLAccount = function () {
        var _this = this;
        //1- get glaccount
        this.fetchBillToCard().then(function (res) {
            var card = res;
            _this.fetchGLAccount().then(function (response) {
                var glaccount = response;
                if (glaccount) {
                    _this.EntityPM.GLAccountId = glaccount.Id;
                    console.log("GLAccount reloaded: " + _this.EntityPM.GLAccountId);
                    _this.GetData();
                }
            });
        });
    };
    ARPaymentDetailsFullAccountingTab.prototype.GetData = function () {
        var _this = this;
        if (this.EntityPM.GLAccountId) {
            console.log(">>> Getting transactions for Account: ", this.EntityPM.GLAccountId);
            this.TransactionsList.Clear();
            this.EntityPM.InvoicesLedgerTransactions = [];
            this._loading = true;
            // setTimeout(() => {
            this._LedgerTransactionExtendedListService.getTransactionsForARPayment(this.EntityPM.Id, this.EntityPM.GLAccountId).subscribe(function (myResult) {
                _this._loading = false;
                var mm = myResult;
                if (!mm.HasError) {
                    var transactions = mm.Result.Result;
                    var tempItemSource = [];
                    if (transactions != null) {
                        for (var i = 0; i < transactions.length; i++) {
                            var line = new TransactionLineModel(transactions[i], _this);
                            // var line = transactions;
                            tempItemSource.push(line);
                        }
                        var sortedTransactions = _this.sortTransactionsByStatus(tempItemSource);
                        // tempItemSource = tempItemSource.sort((a: TransactionLineModel, b: TransactionLineModel) =>
                        // {
                        //     if(a.Status == TextStore.Closed)
                        //         return -1;
                        //     else if(a.Status == TextStore.open)
                        //         return 0;
                        //     else
                        //         return 1;
                        //     // return (a.ReconciledAmount === b.ReconciledAmount) ? 0 : (a.ReconciledAmount > b.ReconciledAmount) ? -1 : 1;
                        // });
                        _this.TransactionsList.InsertCollection(sortedTransactions);
                    }
                }
                else {
                }
                _this.CalculateTotals();
            });
            // }, 6000);
        }
        else {
            console.error("No GLAccount for this payment ", this.EntityPM);
        }
    };
    ARPaymentDetailsFullAccountingTab.prototype.fetchGLAccount = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var _glaId = _this.billtoCard.GLAccountId;
            _this.CurrentSession.StartBusyIndicatorLoading();
            _this._glaService.getSingle(_glaId)
                .subscribe(function (response) {
                var res = response;
                if (!res.HasError) {
                    var glaccount = res.Result;
                    resolve(glaccount);
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    reject();
                    console.error(res.ErrorsArray);
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        });
    };
    ARPaymentDetailsFullAccountingTab.prototype.fetchBillToCard = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var myService = new CardListService_1.CardListService();
            myService.getSingle(_this.BillToId).subscribe(function (resp) {
                if (resp != null) {
                    if (!resp.HasError) {
                        var cardList = resp.Result;
                        if (cardList != null) {
                            _this.billtoCard = cardList;
                            resolve(cardList);
                        }
                    }
                }
            });
        });
    };
    ARPaymentDetailsFullAccountingTab.prototype.CalculateTotals = function () {
        // Reconciliation amount
        var _linesAmount2reco = 0;
        var _linespaymentReconciledAmount = 0;
        this.TransactionsList.Collection.forEach(function (line) {
            if (line && line.AmountToReconcile >= 0) {
                _linesAmount2reco += line.AmountToReconcile;
                _linespaymentReconciledAmount += line.PaymentReconciledAmount;
            }
        });
        this.amount2reconcileTotal = _linesAmount2reco;
        this.paymentReconciledAmountTotal = _linespaymentReconciledAmount;
        if (this.EntityPM.InvoicesLedgerTransactions.length == 0) {
            // this.EntityPM.OpenAmount = this.originalPaymentOpenAmount;
            this.EntityPM.IsDirty = false;
        }
        else {
            // Open Amount
            var _openAmount = this.paymentAmountTotal - _linesAmount2reco;
            if (this.EntityPM.OpenAmount != _openAmount) {
                // this.EntityPM.OpenAmount = _openAmount < 0 ? 0 : _openAmount;
            }
        }
    };
    ARPaymentDetailsFullAccountingTab.prototype.OpenSource = function (id, sourceTypeCode) {
        var tableName = AccountingEntityHelper_1.AccountingEntityHelper.getEntityObjectTableName(sourceTypeCode);
        ;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityId: id,
                ObjectTableName: tableName
            });
        });
    };
    ARPaymentDetailsFullAccountingTab.prototype.OpenJournal = function (id) {
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal', BackButtonLabel: 'Back' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                });
            });
        }
    };
    ARPaymentDetailsFullAccountingTab.prototype.OpenReco = function (recoNumber) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(recoNumber)) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this._ReconciliationExtendedPMService.getByNumber(recoNumber)
                .subscribe(function (myResult) {
                _this.CurrentSession.StopBusyIndicator();
                var mm = myResult;
                if (!mm.HasError) {
                    var reco = mm.Result;
                    var recoId = reco.Id;
                    // this.showAlert = false;
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: recoId, ObjectTableName: 'Reconciliation' });
                        cmpRef.instance.BackCompleted.subscribe(function (bk) {
                            // this.CurrentSession.CloseCurrentWindow();
                            _this.GetData();
                            _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        });
                    });
                }
                else {
                }
            });
        }
    };
    ARPaymentDetailsFullAccountingTab.prototype.PushTransaction = function (trans) {
        if (trans != null) {
            var index = this.EntityPM.InvoicesLedgerTransactions.indexOf(trans);
            if (index == -1) {
                this.EntityPM.IsDirty = true;
                this.EntityPM.InvoicesLedgerTransactions.push(trans);
            }
        }
    };
    ARPaymentDetailsFullAccountingTab.prototype.PopTransaction = function (trans) {
        if (trans != null) {
            var index = this.EntityPM.InvoicesLedgerTransactions.indexOf(trans);
            if (index > -1) {
                // this.EntityPM.IsDirty = true;
                this.EntityPM.InvoicesLedgerTransactions.splice(index, 1);
            }
        }
    };
    ARPaymentDetailsFullAccountingTab.prototype.LoadPaymentMethods = function () {
        var _this = this;
        var myService = new AccountingPaymentMethodListService_1.AccountingPaymentMethodListService();
        myService.getAll().subscribe(function (response) {
            if (response != null) {
                _this.AllMethods = response.Result;
            }
        });
    };
    ARPaymentDetailsFullAccountingTab.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.GetData();
                }
                // if (this.RequestedCommandCode) {
                //     this.ApplyRequestedCommand();
                // }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.GetData();
                }
            });
        }
    };
    ARPaymentDetailsFullAccountingTab.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "IsScreenEnabled", {
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
    ARPaymentDetailsFullAccountingTab.prototype.SetUIProperties = function () {
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
        this.SetBankRequired();
    };
    ARPaymentDetailsFullAccountingTab.prototype.SetBankRequired = function () {
        if (this.EntityPM.AccountingPaymentMethodCode == "CH" && this.Bank == null) {
            this.UIProperties.SetRequired("Bank", this.ObjectTableName, true);
        }
    };
    ARPaymentDetailsFullAccountingTab.prototype.SetUIProperties_Invoices = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.SetUIProperties_ExchangeRate = function () {
        var isEnabled = false;
        if (this.IsScreenEnabled) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "ARPaymentEditExchangeRate")) {
                if (this.EntityPM.PaymentInvoices.length > 0) {
                    isEnabled = false;
                }
                else {
                    isEnabled = true;
                }
                if (this.PaymentCurrencyId == SessionLocator_1.SessionLocator.TenantPM.CurrencyId) {
                    isEnabled = false;
                }
                else {
                    isEnabled = true;
                }
            }
        }
        this.UIProperties.SetEnabled("PaymentCurrencyExchangeRate", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("ExchangeRateDate", this.ObjectTableName, isEnabled);
    };
    ARPaymentDetailsFullAccountingTab.prototype.SetUIProperties_Cheque = function () {
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
                if (Tools_1.AppTool.IsNullOrEmpty(this.Bank)) {
                    this.UIProperties.SetRequired("Bank", this.ObjectTableName, true);
                }
                this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
                this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
            }
        }
    };
    ARPaymentDetailsFullAccountingTab.prototype.SetUIProperties_CreditCard = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.SetUIProperties_BankTransfer = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.GetRateIsEnabled = function () {
        var result = true;
        if (this.EntityPM != null) {
            if (this.EntityPM.PaymentCurrencyId == SessionLocator_1.SessionLocator.TenantPM.CurrencyId || this.EntityPM.PaymentCurrencyId == null || SessionLocator_1.SessionLocator.TenantPM.CurrencyId == null) {
                result = false;
            }
        }
        this.SetUIProperties_ExchangeRate();
        this.RateIsEnabled = result;
    };
    ARPaymentDetailsFullAccountingTab.prototype.SearchTextKeyUp = function (args) {
        this.searchText = args;
        this.LoadData();
    };
    ARPaymentDetailsFullAccountingTab.prototype.LoadData = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.LoadPaymentInvoices_Created = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.LoadPaymentInvoices_Connected = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.LoadPaymentInvoices_IsMatched = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.FillBaselist = function () {
        // this.ItemsSource.Clear();
        // var connectedList: ARPaymentInvoiceArgs[] = [];
        // var unConnectedMatchedList: ARPaymentInvoiceArgs[] = [];
        // var unConnectedListNotMatched: ARPaymentInvoiceArgs[] = [];
        // var itemsCollection: ARPaymentInvoiceArgs[] = [];
        // if (this.ConnectedList.length > 0) {
        //     this.ConnectedList.forEach(item => {
        //         if (this.EntityPM.PaymentInvoices.filter(d => d.ARInvoiceId == item.Id)[0]) {
        //             if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
        //                 if (this.EntityPM.AmountInPaymentCurrency > 0) {
        //                     var value = this.EntityPM.AmountInPaymentCurrency;
        //                     if (item.AmountDue > value) {
        //                         item.AmountDue = item.AmountDue - value;
        //                     }
        //                     else {
        //                         item.AmountDue = 0;
        //                     }
        //                 }
        //             }
        //             connectedList.push(new ARPaymentInvoiceArgs(item, this));
        //         }
        //     });
        //     connectedList.sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
        //         itemsCollection.push(item);
        //     });
        // }
        // if (!this.EntityPM.IsClosed) {
        //     this.IsMatchedList.forEach(item => {
        //         if (connectedList.filter(f => f.Id == item.Id).length == 0) {
        //             if (!item.IsClosed) {
        //                 var isCurrencyMatched: boolean = false;
        //                 if (this.PaymentCurrencyId == item.InvoiceCurrencyId) {
        //                     isCurrencyMatched = true;
        //                 }
        //                 else if (this.IsMultiCurrency) {
        //                     if (this.PaymentCurrencyId == SessionLocator.LocalCurrencyId) {
        //                         isCurrencyMatched = true;
        //                     }
        //                     else if (item.InvoiceCurrencyId == SessionLocator.LocalCurrencyId) {
        //                         isCurrencyMatched = true;
        //                     }
        //                 }
        //                 if (isCurrencyMatched == false || item.StatusCode == "DR") {
        //                     unConnectedListNotMatched.push(new ARPaymentInvoiceArgs(item, this));
        //                 }
        //                 else {
        //                     unConnectedMatchedList.push(new ARPaymentInvoiceArgs(item, this));
        //                 }
        //             }
        //         }
        //     });
        //     unConnectedMatchedList.filter(f => f.CurrencyId == this.PaymentCurrencyId).sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
        //         itemsCollection.push(item);
        //     });
        //     unConnectedMatchedList.filter(f => f.CurrencyId != this.PaymentCurrencyId).sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
        //         itemsCollection.push(item);
        //     });
        //     unConnectedListNotMatched.sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
        //         itemsCollection.push(item);
        //     });
        // }
        // this.ItemsSource.InsertCollection(itemsCollection);
        // this.UpdateSummary();
        // this.IsDataLoaded = true;
    };
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "BillToId", {
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
                    if (value)
                        this.ReloadGLAccount();
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "BillToAddressId", {
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
    ARPaymentDetailsFullAccountingTab.prototype.LoadAddress = function () {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "PaymentCurrencyId", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "PaymentCurrencyCode", {
        get: function () { return this.EntityPM.PaymentCurrencyCode; },
        set: function (value) {
            if (this.EntityPM.PaymentCurrencyCode != value) {
                this.EntityPM.PaymentCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "PaymentCurrencyExchangeRate", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "ExchangeRateDate", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "RelativeRateDate", {
        get: function () { return this.myRelativeRateDate; },
        set: function (value) {
            if (this.myRelativeRateDate != value) {
                this.myRelativeRateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARPaymentDetailsFullAccountingTab.prototype.ComputeRelativeRateDate = function () {
        this.RelativeRateDate = Tools_1.DateTool.GetRelativeRateDate(this.RegisterDate, this.ExchangeRateDate, "old");
    };
    ARPaymentDetailsFullAccountingTab.prototype.LoadCurrencyRates = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.UpdateCurrencyRates = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.SetCurrencyRateData = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.GetCurrencyRate = function (currencyId) {
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
    ARPaymentDetailsFullAccountingTab.prototype.GetCurrencyRateDate = function (currencyId) {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "RegisterDate", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "AccountingPaymentMethodId", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "AccountingPaymentMethodCode", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "SATPaymentMethodCode", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "BranchId", {
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
    ARPaymentDetailsFullAccountingTab.prototype.CheckARPaymentCashBook = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.RefreshPaymentMethodFields = function () {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "PaymentMethodDetailsLabel", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "ChequePaymentRefLabel", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "VisibleIfCash", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "CollapsedIfCash", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "Bank", {
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
                    if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                        this.UIProperties.SetRequired("Bank", this.ObjectTableName, false);
                    }
                    else {
                        this.UIProperties.SetRequired("Bank", this.ObjectTableName, true);
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "BankBranch", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "Account", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "ValueDate", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "ChequeOrPaymentRef", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "CreditCardTypeId", {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "BankAccountId", {
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
    ARPaymentDetailsFullAccountingTab.prototype.CheckGLAccountCurrencyId = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.EditGLAccount = function (arg) {
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
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "AmountInPaymentCurrency", {
        // Amounts
        get: function () { return this.EntityPM.AmountInPaymentCurrency; },
        set: function (value) {
            if (this.EntityPM.AmountInPaymentCurrency != value) {
                this.EntityPM.AmountInPaymentCurrency = Tools_1.AppTool.Round(value, 2);
                this.ComputeLocalAmount();
                this.ComputeOpenAmount();
                this.UpdateSummary();
                this.originalPaymentOpenAmount = this.EntityPM.AmountInPaymentCurrency;
                this.paymentAmountTotal = this.EntityPM.AmountInPaymentCurrency;
                this.CalculateTotals();
                this.ItemsSource.Collection.forEach(function (item) {
                    item.SetUIProperties();
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "AmountInLocalCurrency", {
        get: function () { return this.EntityPM.AmountInLocalCurrency; },
        set: function (value) {
            if (this.EntityPM.AmountInLocalCurrency != value) {
                this.EntityPM.AmountInLocalCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "OpenAmount", {
        get: function () { return this.EntityPM.OpenAmount == null ? 0 : this.EntityPM.OpenAmount; },
        set: function (value) {
            if (this.EntityPM.OpenAmount != value) {
                this.EntityPM.OpenAmount = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARPaymentDetailsFullAccountingTab.prototype, "PrintNotes", {
        get: function () { return this.EntityPM.PrintNotes; },
        set: function (value) {
            if (this.EntityPM.PrintNotes != value) {
                this.EntityPM.PrintNotes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARPaymentDetailsFullAccountingTab.prototype.ComputeOpenAmount = function () {
        this.OpenAmount = this.AmountInPaymentCurrency - this.Summary_AmountPaid;
    };
    ARPaymentDetailsFullAccountingTab.prototype.ComputeLocalAmount = function () {
        this.AmountInLocalCurrency = this.AmountInPaymentCurrency * this.PaymentCurrencyExchangeRate;
    };
    ARPaymentDetailsFullAccountingTab.prototype.UpdateSummary = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.ViewEntity = function (args) {
        if (args) {
            this.RequestedCommandCode = "ViewInvoice";
            this.RequestedCommandParam = args.Id;
            this.entityArgs.EditComponent.SaveChanges();
        }
    };
    ARPaymentDetailsFullAccountingTab.prototype.UpdateCurrencyRateMethod = function () {
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
    ARPaymentDetailsFullAccountingTab.prototype.UpdatePaymentInvoicesErrors = function () {
        var haserrors = false;
        if (this.ItemsSource.Collection.filter(function (d) { return d.InputHasError; })[0]) {
            haserrors = true;
        }
        this.EntityPM.HasInvoicesErrors = haserrors;
    };
    ARPaymentDetailsFullAccountingTab.prototype.ApplyRequestedCommand = function () {
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
    //#endregion
    ARPaymentDetailsFullAccountingTab.prototype.sortTransactionsByStatus = function (transactions) {
        var closedTransactions = transactions.filter(function (d) { return d.Status == TextStore.Closed; });
        var partiallyOpenedTransactions = transactions.filter(function (d) { return d.Status == TextStore.partiallyOpened; });
        var openedTransactions = transactions.filter(function (d) { return d.Status == TextStore.open; });
        var sortedTransactions = [];
        sortedTransactions = closedTransactions.concat(partiallyOpenedTransactions).concat(openedTransactions);
        return sortedTransactions;
    };
    ARPaymentDetailsFullAccountingTab = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARPaymentDetailsFullAccountingTab.html',
            styleUrls: ['./ARPaymentDetailsFullAccountingTab.css']
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ARPaymentDetailsFullAccountingTab);
    return ARPaymentDetailsFullAccountingTab;
}(BaseComponent_1.BaseComponent));
exports.ARPaymentDetailsFullAccountingTab = ARPaymentDetailsFullAccountingTab;
var TextStore = /** @class */ (function () {
    function TextStore() {
    }
    TextStore.open = TextCodeTranslator_1.TextCodeTranslator.Translate('Accounting.O.ARP.Open');
    TextStore.Closed = TextCodeTranslator_1.TextCodeTranslator.Translate('Accounting.O.ARP.Closed');
    TextStore.partiallyOpened = TextCodeTranslator_1.TextCodeTranslator.Translate('Accounting.O.ARP.partiallyOpened');
    TextStore.ErrorsInSelectedLines = TextCodeTranslator_1.TextCodeTranslator.Translate('Reconciliations.O.ErrorsInSelectedLines');
    TextStore.invoiceAmount2reconcileMSG = TextCodeTranslator_1.TextCodeTranslator.Translate('Accounting.O.ARP.invoiceAmount2reconcileMSG');
    return TextStore;
}());
exports.TextStore = TextStore;
var TransactionLineModel = /** @class */ (function (_super) {
    __extends(TransactionLineModel, _super);
    function TransactionLineModel(ledgerTransaction, parent) {
        var _this = _super.call(this) || this;
        _this.ledgerTransaction = ledgerTransaction;
        _this.parent = parent;
        _this.DataContext = _this;
        _this.LedgerTransactionPM = null;
        _this.ObjectTableName = "LedgerTransaction";
        _this.isRTL = false;
        _this.isLineValid = true;
        _this.RecociliationNumbers = [];
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.EntityPM = _this.parent.EntityPM;
        _this.LedgerTransactionPM = ledgerTransaction;
        _this.originalOpenAmount = _this.OpenAmount;
        _this.CalculateFields();
        _this.UIProperties.SetEnabled("AmountToReconcile", "LedgerTransaction", _this.Status != TextStore.Closed && !_this.parent.IsGridReadOnly);
        return _this;
    }
    TransactionLineModel.prototype.CalculateFields = function () {
        this.IconCode = AccountingEntityHelper_1.AccountingEntityHelper.getEntityIcon(this.LedgerTransactionPM.SourceTypeCode);
        this.OriginalAmount = this.CalculateOriginalAmount();
        this.OriginalAmountCurrency = this.CalculatOriginalCurruncy();
        this.Status = this.GetStatus();
        this.RecociliationNumbers = this.getRecoLinkList();
    };
    Object.defineProperty(TransactionLineModel.prototype, "OriginalAmount", {
        get: function () {
            return this._OriginalAmount;
        },
        set: function (v) {
            this._OriginalAmount = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "OriginalAmountCurrency", {
        get: function () {
            return this._OriginalAmountCurrency;
        },
        set: function (v) {
            this._OriginalAmountCurrency = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "IsChecked", {
        get: function () {
            if (this.IsReconciled) {
                return true;
            }
            else {
                return this._isChecked;
            }
        },
        set: function (v) {
            this._isChecked = v;
            if (v) {
                if (this.AmountToReconcile == null || this.AmountToReconcile == 0)
                    this.AmountToReconcile = this.OpenAmount;
                this.parent.PushTransaction(this.ledgerTransaction);
            }
            else {
                this.AmountToReconcile = 0;
                this.parent.PopTransaction(this.ledgerTransaction);
                if (this.EntityPM.InvoicesLedgerTransactions.length == 0) {
                    this.EntityPM.IsDirty = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "IsReconciled", {
        get: function () { return this.LedgerTransactionPM.IsReconciled; },
        set: function (value) {
            if (this.LedgerTransactionPM.IsReconciled != value) {
                this.LedgerTransactionPM.IsReconciled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "AmountToReconcile", {
        get: function () { return this.LedgerTransactionPM.AmountToReconcile; },
        set: function (value) {
            if (this.LedgerTransactionPM.AmountToReconcile != value) {
                this.LedgerTransactionPM.AmountToReconcile = value;
                if (value == 0 || !value)
                    this.IsChecked = false;
                else
                    this.IsChecked = true;
                this.setAmounts();
                if (this.IsChecked)
                    this.validateLine();
                this.parent.CalculateTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    TransactionLineModel.prototype.OnAmountToReconcileLostFocus = function (logCellTemplate, classificationTextBox) {
        if (this.IsChecked)
            this.validateLine();
    };
    Object.defineProperty(TransactionLineModel.prototype, "OpenAmount", {
        get: function () { return this.LedgerTransactionPM.OpenAmount; },
        set: function (value) {
            if (this.LedgerTransactionPM.OpenAmount != value) {
                this.LedgerTransactionPM.OpenAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Status", {
        get: function () {
            return this._Status;
        },
        set: function (v) {
            this._Status = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "ReconciledAmount", {
        get: function () {
            return this.OriginalAmount - this.originalOpenAmount;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "NewReconciliationAmount", {
        get: function () {
            return this.AmountToReconcile + this.ReconciledAmount;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "PaymentReconciledAmount", {
        get: function () {
            return this.LedgerTransactionPM.PaymentReconciledAmount;
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    TransactionLineModel.prototype.validateLine = function () {
        //validate line
        if (this.AmountToReconcile >= 0 && this.AmountToReconcile <= this.originalOpenAmount) {
            this.UIProperties.SetValidity("AmountToReconcile", this.ObjectTableName, true, "valid");
            this.isLineValid = true;
            this.parent.SetEntityValidity();
            // SessionLocator.SustainFocusOnCell = false;
        }
        else {
            this.UIProperties.SetValidity("AmountToReconcile", this.ObjectTableName, false, TextStore.invoiceAmount2reconcileMSG);
            this.isLineValid = false;
            this.parent.SetEntityValidity();
            // SessionLocator.SustainFocusOnCell = true;
            // SessionLocator.SelectedSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: classificationTextBox.InputId });
        }
    };
    TransactionLineModel.prototype.setAmounts = function () {
        //set amount
        if (this.AmountToReconcile >= 0 && this.AmountToReconcile <= this.originalOpenAmount) {
            this.OpenAmount = this.originalOpenAmount - this.AmountToReconcile;
        }
        else {
            this.OpenAmount = this.originalOpenAmount;
        }
    };
    Object.defineProperty(TransactionLineModel.prototype, "Id", {
        //#region Other Properties
        get: function () { return this.LedgerTransactionPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Tenant", {
        get: function () { return this.LedgerTransactionPM.Tenant; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "AccountingDate", {
        get: function () { return this.LedgerTransactionPM.AccountingDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "DocumentDate", {
        get: function () { return this.LedgerTransactionPM.DocumentDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "JournalNumber", {
        get: function () { return this.LedgerTransactionPM.JournalNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Source", {
        get: function () { return this.LedgerTransactionPM.Source; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "SourceType", {
        get: function () { return this.LedgerTransactionPM.SourceType; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "SourceId", {
        get: function () { return this.LedgerTransactionPM.SourceId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "DueDate", {
        get: function () { return this.LedgerTransactionPM.DueDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "LocalAmountCredit", {
        get: function () { return this.LedgerTransactionPM.LocalAmountCredit; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "LocalAmountDebit", {
        get: function () { return this.LedgerTransactionPM.LocalAmountDebit; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "ForeignAmountCredit", {
        get: function () { return this.LedgerTransactionPM.ForeignAmountCredit; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "ForeignAmountDebit", {
        get: function () { return this.LedgerTransactionPM.ForeignAmountDebit; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "OpenAmountCurrencyCode", {
        get: function () { return this.LedgerTransactionPM.OpenAmountCurrencyCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "OpenAmountCurrencySign", {
        get: function () { return this.LedgerTransactionPM.OpenAmountCurrencySign; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "CurrencyId", {
        get: function () { return this.LedgerTransactionPM.CurrencyId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Reference1", {
        get: function () { return this.LedgerTransactionPM.Reference1; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Reference2", {
        get: function () { return this.LedgerTransactionPM.Reference2; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Reference3", {
        get: function () { return this.LedgerTransactionPM.Reference3; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "Notes", {
        get: function () { return this.LedgerTransactionPM.Notes; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "IsPartial", {
        get: function () { return this.OpenAmount != this.AmountToReconcile; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "OpenAmountCurrencyId", {
        get: function () { return this.LedgerTransactionPM.OpenAmountCurrencyId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "SourceTypeCode", {
        get: function () { return this.LedgerTransactionPM.SourceTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "SourceNumber", {
        get: function () { return this.LedgerTransactionPM.SourceNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "GroupNumber", {
        get: function () { return this.LedgerTransactionPM.GroupHash; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "RecoNumber", {
        get: function () { return this.LedgerTransactionPM.RecoNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "ReconciliationId", {
        get: function () { return this.LedgerTransactionPM.ReconciliationId; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    TransactionLineModel.prototype.GetStatus = function () {
        var __s = "";
        if (this.OriginalAmount == this.originalOpenAmount)
            __s = TextStore.open;
        else if (0 == this.originalOpenAmount)
            __s = TextStore.Closed;
        else
            __s = TextStore.partiallyOpened;
        return __s;
    };
    TransactionLineModel.prototype.GetStatusColor = function () {
        var _color = 'black';
        if (this.OriginalAmount == this.originalOpenAmount)
            _color = 'green';
        else if (0 == this.originalOpenAmount)
            _color = 'black';
        else
            _color = 'orange';
        return _color;
    };
    TransactionLineModel.prototype.CalculateOriginalAmount = function () {
        var transaction = this.LedgerTransactionPM;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.parent.EntityPM.GLAccountRecoMethodCode)) {
            if (this.parent.EntityPM.GLAccountRecoMethodCode == "0") { // 0-local currency
                if (transaction['LocalAmountCredit'] == 0) {
                    return transaction['LocalAmountDebit'];
                }
                else {
                    return -1 * transaction['LocalAmountCredit'];
                }
            }
            else if (this.parent.EntityPM.GLAccountRecoMethodCode == "1") { // 1-foreign currency
                if (transaction['ForeignAmountCredit'] == 0) {
                    return transaction['ForeignAmountDebit'];
                }
                else {
                    return -1 * transaction['ForeignAmountCredit'];
                }
            }
        }
    };
    TransactionLineModel.prototype.CalculatOriginalCurruncy = function () {
        //
        // [i] copied from list template
        //
        if (!Tools_1.AppTool.IsNullOrEmpty(this.parent.EntityPM.GLAccountRecoMethodCode)) {
            // this code was copied to reconcile window, if it need change, please chenge it in reconcile window too
            if (this.parent.EntityPM.GLAccountRecoMethodCode == "0") { // 0-local currency
                // local
                return SessionLocator_1.SessionLocator.TenantPM.CurrencySign;
            }
            else if (this.parent.EntityPM.GLAccountRecoMethodCode == "1") { // 1-foreign currency
                // foreign
                return this.ledgerTransaction.CurrencySign;
            }
        }
    };
    TransactionLineModel.prototype.getRecoLinkList = function () {
        var res = [];
        if (this.RecoNumber) {
            res = this.RecoNumber.split(',');
        }
        return res;
    };
    return TransactionLineModel;
}(BaseComponent_1.BaseComponent));
exports.TransactionLineModel = TransactionLineModel;
//# sourceMappingURL=ARPaymentDetailsFullAccountingTab.js.map