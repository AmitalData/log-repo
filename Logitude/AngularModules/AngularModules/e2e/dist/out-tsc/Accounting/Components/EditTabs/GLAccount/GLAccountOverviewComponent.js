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
var Tools_1 = require("./../../../../Infrastructure/Tools");
var CardListService_1 = require("./../../../../Common/Services/StandardLists/CardListService");
var AccountingNotePMService_1 = require("./../../../Services/StandardPMs/AccountingNotePMService");
var MessageWindow_1 = require("./../../../../Controls/Windows/MessageWindow");
var AccountingNoteExtendedListService_1 = require("./../../../Services/ExtendedLists/AccountingNoteExtendedListService");
var AccountingEntityHelper_1 = require("./../../../Utilities/AccountingEntityHelper");
var EntityResourceService_1 = require("./../../../../Infrastructure/Services/EntityResourceService");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var GLAccountMoreDataListService_1 = require("../../../Services/StandardLists/GLAccountMoreDataListService");
var Tools_2 = require("../../../../Infrastructure/Tools");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var GLAccountExtendedListService_1 = require("../../../Services/ExtendedLists/GLAccountExtendedListService");
var LedgerTransactionExtendedListService_1 = require("../../../Services/ExtendedLists/LedgerTransactionExtendedListService");
var ReconcileEventManager_1 = require("../../../Utilities/ReconcileEventManager");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var AgingReportParameters_1 = require("../../../DataContracts/AgingReportParameters");
var GLAccountOverviewComponent = /** @class */ (function (_super) {
    __extends(GLAccountOverviewComponent, _super);
    function GLAccountOverviewComponent(entityArgs, CD) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CD = CD;
        // Const
        _this.DataContext = _this;
        _this.ObjectTableName = "GLAccount";
        _this.txtcode_Amount = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Amount");
        _this.txtcode_AgingDetails = TextCodeTranslator_1.TextCodeTranslator.Translate("GLAccounts.O.AgingDetails");
        // Variables
        _this.AccountPM = null;
        _this.GLAccountMoreData = null;
        _this.isRTL = false;
        _this.showLocal = false;
        _this.isUsedOutside = false; // when view tab inside customer ..
        //Services
        _this._EntityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._GLAccountMoreDataListService = new GLAccountMoreDataListService_1.GLAccountMoreDataListService();
        _this._LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService_1.LedgerTransactionExtendedListService();
        _this._GLAccountExtendedListService = new GLAccountExtendedListService_1.GLAccountExtendedListService();
        _this._AccountingNoteExtendedListService = new AccountingNoteExtendedListService_1.AccountingNoteExtendedListService();
        _this._AccountingNotePMService = new AccountingNotePMService_1.AccountingNotePMService();
        _this._CardListService = new CardListService_1.CardListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.TabSelectedEvent = null;
        //#region Balance Section
        _this.GLAccountOpenTransactionsCount = 0.0;
        //#endregion
        //#region Accounting Notes
        _this.accountingNotesList = [];
        _this.isNotesLoading = false;
        _this.txt_updatedBy = TextCodeTranslator_1.TextCodeTranslator.Translate("AccountingNote.F.UpdatedByUserName");
        //#endregion
        //#region Credit limit
        _this.creditPercentage = 0;
        _this.accountTotal = 0;
        _this.creditStatusAmount = 0;
        //
        //#endregion
        //#region Aging Details
        _this.chartId = "";
        // Filter Methods
        _this.FilterSelectedValue = '3mo';
        // Filter Methods
        _this.DateFilterSelectedValue = 'filter_Collecting';
        //Chart Code
        _this.barChartLabels = [];
        _this.barChartData = [{ data: [], label: '', scaleShowVerticalLines: false, }];
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        if (SessionLocator_1.SessionLocator.LoggedUserPM)
            _this.showLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        //Resources
        _this._EntityResourceService.getEntityResourceByTableName("AccountingNote").subscribe(function (response) { });
        _this._EntityResourceService.getEntityResourceByTableName("Reconciliation").subscribe(function (response) { });
        _this._EntityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe(function (response) { });
        // Set Entity
        if (entityArgs && entityArgs.ObjectTableName == "GLAccount") {
            _this.AccountPM = entityArgs.EntityPM;
            _this.LoadAllData();
        }
        else {
            _this.isUsedOutside = true;
        }
        _this.chartId = "CustomerOverview_" + _this.CurrentSession.GetChartId();
        _this.Listen();
        return _this;
    }
    GLAccountOverviewComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.LoadAllData();
                    }
                });
            }
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                        _this.LoadAllData();
                    }
                });
            }
            //
            if (this.TabSelectedEvent == null) {
                this.TabSelectedEvent = this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                    if (tabCode == "GAOV") {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.LoadAllData();
                    }
                });
            }
        }
    };
    GLAccountOverviewComponent.prototype.LoadAllData = function () {
        this.GetDefaultValues();
        this.GetLastTransactions();
        this.GetAccountingNotes();
        this.LoadChartData();
        this.SetUIProperties();
        this.LoadCreditDetailsData();
    };
    GLAccountOverviewComponent.prototype.GetDefaultValues = function () {
        var _this = this;
        // Get GLAccountMoreData
        this.CurrentSession.StartBusyIndicatorLoading();
        this._GLAccountMoreDataListService.getSingle(this.AccountPM.Id).subscribe(function (myResult) {
            _this.CurrentSession.StopBusyIndicator();
            console.log("_GLAccountMoreDataListService.getSingle", myResult);
            var mm = myResult;
            if (!mm.HasError) {
                _this.GLAccountMoreData = mm.Result;
                _this.LoadCreditDetailsData();
            }
            else {
            }
        });
        // Get GLAccount Open Transactions Count
        this._GLAccountExtendedListService.GetAccountOpenTransactionsCount(this.AccountPM.Id).subscribe(function (myResult) {
            console.log("GetAccountOpenTransactionsCount", myResult);
            var result = myResult;
            if (!result.HasError) {
                _this.GLAccountOpenTransactionsCount = result.Result;
            }
            else {
            }
        });
        // Get connect card
        this._CardListService.getSingle(this.AccountPM.CardId).subscribe(function (myResult) {
            console.log("_CardListService.getSingle", myResult);
            var result = myResult;
            if (!result.HasError) {
                _this.accountCardlist = result.Result;
                _this.LoadCreditDetailsData();
            }
            else {
                console.log("[!] cannot get glaccount card");
            }
        });
        // Get tenant currency
        this.TenantCurrency = SessionLocator_1.SessionLocator.TenantPM.CurrencyCode;
    };
    GLAccountOverviewComponent.prototype.SetUIProperties = function () {
        //if (!this.AccountPM || this.DisableGLAccount)
        //    return;
        //if (this.AccountPM.IsMultiCurrency) {
        //    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
        //}
        //if (this.AccountPM.ChartOfAccountsTypeCode) {
        //    this.ChartOfAccountsTypeCode = this.AccountPM.ChartOfAccountsTypeCode;
        //}
        //if (this.AccountPM.ChartOfAccountsId) {
        //    this.ChartOfAccountsId = this.AccountPM.ChartOfAccountsId;
        //    this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, true, "");
        //}
    };
    GLAccountOverviewComponent.prototype.DisplayTransactionsLinkClicked = function () {
        var _this = this;
        if (this.isUsedOutside) {
            var editWindow = new LogitudeWindow_1.LogitudeWindow();
            editWindow.ShowHeaderButtons = true;
            //editWindow.Title = windowTitle;
            editWindow.Height = 770;
            editWindow.Width = 1500;
            editWindow.IsHideHeader = true;
            editWindow.ShowEditComponent(this.AccountPM.Id, "GLAccount", "GATR");
            editWindow.WindowClosed.subscribe(function (res) {
                _this.LoadAllData();
            });
        }
        else {
            this.CurrentSession.CurrentEditComponent.SetSelectedTabByCode("GATR");
        }
    };
    GLAccountOverviewComponent.prototype.ReconcileLinkClicked = function () {
        this.ReconcileButtonClicked();
    };
    GLAccountOverviewComponent.prototype.ReconcileButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var screenWidth = this.getScreenWidth();
        var screenHeight = this.getScreenHeight();
        this._LedgerTransactionExtendedListService.GetFirstLedgerTransaction(this.AccountPM.Id).subscribe(function (serviceResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (serviceResponse.Result) {
                var result = serviceResponse.Result;
                var transaction = result.Result; // get the data
                var openAmountCurrency = transaction.OpenAmountCurrencySign;
                // original amount currency
                var originalAmountCurrency;
                if (_this.AccountPM.ReconcileMethodCode == "0")
                    originalAmountCurrency = SessionLocator_1.SessionLocator.TenantPM.CurrencySign;
                else if (_this.AccountPM.ReconcileMethodCode == "1")
                    originalAmountCurrency = transaction.CurrencySign;
                var windowArgs = {};
                windowArgs.GLAccountPM = _this.AccountPM;
                windowArgs.openAmountCurrency = openAmountCurrency;
                windowArgs.originalAmountCurrency = originalAmountCurrency;
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.Width = (screenWidth > 1024) ? (screenWidth > 1200 ? 1500 : screenWidth - 20) : 900;
                logitudeWindow.Height = (screenHeight > 768) ? (screenHeight > 800 ? 700 : screenHeight - 70) : screenHeight - 70;
                logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Reconcile"); //"Reconcile";
                logitudeWindow.WindowArgs = windowArgs;
                logitudeWindow.Show('./Accounting/Components/Others/ReconcileComponent');
                logitudeWindow.WindowClosed.subscribe(function ($event) {
                    // this.LoadAllData();
                    _this.GetNonReconciledTransactionsCount();
                });
            }
        });
    };
    GLAccountOverviewComponent.prototype.GetNonReconciledTransactionsCount = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this._GLAccountExtendedListService.GetAccountReconcilesCount(this.AccountPM.Id).subscribe(function (myResult) {
            if (!Tools_2.AppTool.IsNullOrEmpty(myResult)) {
                _this.CurrentSession.CurrentEditComponent.EntityPM.ReconcilationCount = myResult;
                _this.CurrentSession.CurrentEditComponent.SaveChanges();
                _this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function ($event) {
                    if ($event == true) {
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
        });
    };
    GLAccountOverviewComponent.prototype.getScreenHeight = function () {
        if (self.innerHeight) {
            return self.innerHeight;
        }
        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }
        if (document.body) {
            return document.body.clientHeight;
        }
    };
    GLAccountOverviewComponent.prototype.getScreenWidth = function () {
        if (self.innerWidth) {
            return self.innerWidth;
        }
        if (document.documentElement && document.documentElement.clientWidth) {
            return document.documentElement.clientWidth;
        }
        if (document.body) {
            return document.body.clientWidth;
        }
    };
    GLAccountOverviewComponent.prototype.GetAccountingNotes = function () {
        var _this = this;
        if (this.AccountPM.CardId) {
            this.accountingNotesList = [];
            this.isNotesLoading = true;
            // setTimeout(() => {
            this._AccountingNoteExtendedListService.GetNotesByCard(this.AccountPM.CardId)
                .subscribe(function (res) {
                _this.isNotesLoading = false;
                if (res.HasError) {
                    var msg = new MessageWindow_1.MessageWindow();
                    msg.Show("Get Accounting Note error: " + res.ErrorsArray[0]);
                }
                else {
                    var notesList = res.Result;
                    _this.accountingNotesList = notesList;
                }
            });
            // }, 2000);
        }
    };
    GLAccountOverviewComponent.prototype.OpenAccountingNote = function (notePM) {
        var _this = this;
        var windowArgs = {};
        windowArgs.AccountPM = this.AccountPM;
        windowArgs.AccountingNotePM = notePM;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 320;
        logitudeWindow.Title = notePM ? '' : TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.NewAccountingNote");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Accounting/Components/Others/AccountingNoteComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            _this.GetAccountingNotes();
        });
    };
    GLAccountOverviewComponent.prototype.ItemEditButton = function (_noteList) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this._AccountingNotePMService.get(_noteList.Id)
            .subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                var _notePM = mm.Result;
                _this.OpenAccountingNote(_notePM);
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    GLAccountOverviewComponent.prototype.ItemDeleteButton = function (item) {
        var _this = this;
        this._AccountingNoteExtendedListService.DeleteNote(item.Id)
            .subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                var res = mm.Result;
                _this.CurrentSession.StopBusyIndicator();
                _this.GetAccountingNotes();
            }
            else {
                var msg = new MessageWindow_1.MessageWindow();
                msg.Show(mm.ErrorsArray[0]);
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    GLAccountOverviewComponent.prototype.GetNoteTitle = function (note) {
        var result = "";
        if (note) {
            var myFormats = Tools_1.DateTool.GetDateFormats(note.UpdateDate);
            var formatedDate = myFormats.DateString + " " + myFormats.ShortTimeString;
            result = this.txt_updatedBy + ' ' + note.UpdatedByUserName + ' (' + formatedDate + ') ';
        }
        return result;
    };
    GLAccountOverviewComponent.prototype.GetAmountLabel = function () {
        var msg = this.txtcode_Amount + " (" + SessionLocator_1.SessionLocator.TenantPM.CurrencyCode + ")";
        return msg;
    };
    GLAccountOverviewComponent.prototype.GetTransAmount = function (transaction) {
        return transaction.LocalAmountDebit ? transaction.LocalAmountDebit : transaction.LocalAmountCredit;
    };
    GLAccountOverviewComponent.prototype.GetIconText = function (line) {
        var iconTxt = AccountingEntityHelper_1.AccountingEntityHelper.getEntityIcon(line.SourceTypeCode);
        return iconTxt;
    };
    GLAccountOverviewComponent.prototype.GetReferencesText = function (transaction) {
        var reference = "";
        reference = this.pushText(reference, transaction.Reference1);
        reference = this.pushText(reference, transaction.Reference2);
        reference = this.pushText(reference, transaction.Reference3);
        return reference;
    };
    GLAccountOverviewComponent.prototype.pushText = function (txt, target) {
        return (target ? (!target.includes(txt) ? (target += ' / ' + txt) : target) : txt);
    };
    GLAccountOverviewComponent.prototype.GetLastTransactions = function () {
        var _this = this;
        this._LedgerTransactionExtendedListService.getLast10TransactionsForAccount(this.AccountPM.Id).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                _this.lastTransactionsList = mm.Result.Result;
            }
            else {
            }
        });
    };
    GLAccountOverviewComponent.prototype.Abs = function (number) {
        return number < 0 ? number * -1 : number;
    };
    GLAccountOverviewComponent.prototype.OpenSource = function (transaction, id) {
        // Type:    SourceTypeCode
        // Id:      SourceId
        // Display: SourceNumber
        var tableName = "Journal";
        switch (transaction.SourceTypeCode) {
            // 1-Journal
            case '1': {
                tableName = "Journal";
                break;
            }
            // 2-ARInvoice
            case '2': {
                tableName = "ARInvoice";
                break;
            }
            // 3-ARPayment
            case '3': {
                tableName = "ARPayment";
                break;
            }
            // 4-APInvoice
            case '4': {
                tableName = "APInvoice";
                break;
            }
            // 5-APPayment
            case '5': {
                tableName = "APPayment";
                break;
            }
            // 6-Cheque Deposit
            case '6': {
                tableName = "BankDeposit";
                break;
            }
            // 7-Cash Deposit
            case '7': {
                tableName = "BankDeposit";
                break;
            }
            // 8-Revaluation
            case '8': {
                tableName = "Revaluation";
                break;
            }
            // 9-PaymentCheque
            case '9': {
                tableName = "PaymentCheque";
                break;
            }
        }
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityId: id,
                ObjectTableName: tableName
            });
        });
    };
    GLAccountOverviewComponent.prototype.OpenJournal = function (id) {
        if (!Tools_2.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                });
            });
        }
    };
    GLAccountOverviewComponent.prototype.CalculateOriginalAmount = function (transaction) {
        if (!Tools_2.AppTool.IsNullOrEmpty(ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode)) {
            if (ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode == "0") { // 0-local currency
                if (transaction['LocalAmountCredit'] == 0) {
                    return transaction['LocalAmountDebit'];
                }
                else {
                    return -1 * transaction['LocalAmountCredit'];
                }
            }
            else if (ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode == "1") { // 1-foreign currency
                if (transaction['ForeignAmountCredit'] == 0) {
                    return transaction['ForeignAmountDebit'];
                }
                else {
                    return -1 * transaction['ForeignAmountCredit'];
                }
            }
        }
    };
    GLAccountOverviewComponent.prototype.GetIndicatorText = function (transaction) {
        var showLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        if (transaction['OpenAmount'] != this.CalculateOriginalAmount(transaction))
            return showLocal ? 'סכום פתוח חלקית' : 'Partial transaction';
        else
            return showLocal ? 'סכום פתוח ' : 'Open transaction';
    };
    GLAccountOverviewComponent.prototype.LoadCreditDetailsData = function () {
        console.log("LoadCreditDetailsData");
        // Calculate credit percentage
        var percentage = 0;
        if (this.GLAccountMoreData && this.accountCardlist) {
            percentage =
                (this.GLAccountMoreData.BalanceInLocalCurrency ? this.GLAccountMoreData.BalanceInLocalCurrency : 0)
                    + (this.GLAccountMoreData.TotalOpenChequesInLocalCur ? this.GLAccountMoreData.TotalOpenChequesInLocalCur : 0)
                    + (this.GLAccountMoreData.TotFutureOpenChequesInLocalCur ? this.GLAccountMoreData.TotFutureOpenChequesInLocalCur : 0);
            //+ (this.accountCardlist.Total?this.accountCardlist.Total:0 Open shipments)
            this.accountTotal = percentage;
            if (this.accountCardlist.CreditLimitAmount && this.accountCardlist.CreditLimitAmount != 0)
                percentage = percentage / (this.accountCardlist.CreditLimitAmount ? this.accountCardlist.CreditLimitAmount : 0);
            else
                percentage = 0;
            this.creditStatusAmount = (this.accountCardlist.CreditLimitAmount ? this.accountCardlist.CreditLimitAmount : 0) - this.accountTotal;
        }
        if (!percentage)
            percentage = 0;
        if (percentage < 0)
            percentage = 0;
        percentage = percentage * 100;
        this.creditPercentage = percentage;
    };
    GLAccountOverviewComponent.prototype.DisplayChequelistClicked = function () {
    };
    GLAccountOverviewComponent.prototype.CardIndexClicked = function () {
        this.DisplayTransactionsLinkClicked();
    };
    GLAccountOverviewComponent.prototype.IsOverCredit = function () {
        var total = (this.GLAccountMoreData.BalanceInLocalCurrency ? this.GLAccountMoreData.BalanceInLocalCurrency : 0)
            + (this.GLAccountMoreData.TotalOpenChequesInLocalCur ? this.GLAccountMoreData.TotalOpenChequesInLocalCur : 0)
            + (this.GLAccountMoreData.TotFutureOpenChequesInLocalCur ? this.GLAccountMoreData.TotFutureOpenChequesInLocalCur : 0);
        //+ (this.accountCardlist.Total?this.accountCardlist.Total:0 Open shipments)
        return (total > this.accountCardlist.CreditLimitAmount);
    };
    GLAccountOverviewComponent.prototype.IsCreditNotDefined = function () {
        return this.accountCardlist.CreditLimitAmount == null;
    };
    GLAccountOverviewComponent.prototype.GetAgingHeader = function () {
        var txt = this.txtcode_AgingDetails;
        txt += " (" + SessionLocator_1.SessionLocator.TenantPM.CurrencyCode + ")";
        return txt;
    };
    GLAccountOverviewComponent.prototype.FilterItemClicked = function (itemValue) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            this.FilterLines();
        }
    };
    GLAccountOverviewComponent.prototype.FilterLines = function () {
        this.LoadChartData();
    };
    GLAccountOverviewComponent.prototype.DateFilterItemClicked = function (itemValue) {
        if (this.DateFilterSelectedValue != itemValue) {
            this.DateFilterSelectedValue = itemValue;
            this.FilterLines();
        }
    };
    GLAccountOverviewComponent.prototype.LoadChartData = function () {
        var _this = this;
        //if (AppTool.IsNullOrEmpty(this.accSettings)) return;
        var numberOfmonthsbackwards = 3;
        switch (this.FilterSelectedValue) {
            case '3mo': {
                numberOfmonthsbackwards = 3;
                break;
            }
            case '6mo': {
                numberOfmonthsbackwards = 6;
                break;
            }
            case '9mo': {
                numberOfmonthsbackwards = 9;
                break;
            }
            case '12mo': {
                numberOfmonthsbackwards = 12;
                break;
            }
        }
        var args = new AgingReportParameters_1.AgingReportParameters();
        args.Tenant = SessionLocator_1.SessionLocator.Tenant,
            args.AgingForDate = new Date();
        args.NumberOfmonthsbackwards = numberOfmonthsbackwards == null ? 3 : numberOfmonthsbackwards;
        //args.VendorCustomerId = AppTool.IsNullOrEmpty(this.accSettings) ? "" : this.accSettings.CustomerControlAccountId;
        args.VendorCustomerId = this.AccountPM.Id;
        //args.Category1Id = "";
        //args.Category2Id = "";
        //args.Category3Id = "";
        //args.Category4Id = "";
        //args.Category5Id = "";
        //args.CollectorId = SessionLocator.LoggedUserId;
        //args.SalesmanId = "";
        args.IsCustomer = true;
        args.GroupByDate = this.DateFilterSelectedValue == "filter_Accounting" ? "AccountingDate" : "DueDate";
        this.CurrentSession.StartBusyIndicatorLoading();
        this._GLAccountExtendedListService.GetAgingReport(args).subscribe(function (myResponse) {
            console.log("GetAgingReport: ", periods);
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                var res = myResponse.Result;
                if (res != null && res.length > 0) {
                    var periods;
                    periods = res;
                    _this.LoadChart(periods);
                }
            }
        });
    };
    GLAccountOverviewComponent.prototype.OrganizeData = function (data) {
        //var oData: any[];
        //var takeNumber = 5; // Number of columns to show in the graph , execlude "Others" column
        //if (!AppTool.IsNullOrEmpty(data)) {
        //    if (data.length > takeNumber) {
        //         1-Take first section
        //        oData = data.slice(0, takeNumber);
        //         2-Calculate "Others" Column
        //        var othersColumn = new GLAccountList();
        //        othersColumn.TotalAmount = 0;
        //        othersColumn.GLAccountTypeCode = "-1"; // manual entry "Others"
        //        for (var i = takeNumber; i < data.length; i++) {
        //            var amount = data[i].TotalAmount;
        //            amount = this.ConvertToLocal(amount, data[i].CurrencyId);
        //            othersColumn.TotalAmount += ((AppTool.IsNullOrEmpty(amount)) ? 0 : amount);
        //        }
        //        oData.push(othersColumn);
        //    } else {
        //        return data
        //    }
        //}
        // 3-Return data
        //return oData;
    };
    GLAccountOverviewComponent.prototype.LoadChart = function (data) {
        var _this = this;
        //#region Graph metadata
        var max = 0;
        var DataProvider = [];
        var i = 0;
        var index = 0;
        this.barChartData[0].data = [];
        this.barChartLabels = [];
        this.barChartData = [{
                data: [], label: '', scaleShowVerticalLines: false,
            }];
        //Graph Properties
        var Graphs = Graphs = [{
                "balloonText": "Amount: <br>[[value]]",
                "fillAlphas": 1,
                "id": "AmGraph-10" + i,
                "title": "Aging",
                "type": "column",
                "valueField": "dataCol",
                "fillColors": ["#BADFE8", "#7AC2D4", "#73BFD2", "#7AC2D4", "#BADFE8",],
                "gradientOrientation": "horizontal",
                "borderAlpha": 0,
                "lineColor": "#fff",
                "fixedColumnWidth": this.FilterSelectedValue == "9mo" ? 30 : 40,
            }];
        //#endregion
        data.forEach(function (element) {
            //if (element.Total <= 0) return;
            _this.barChartData[0].label = "Amount";
            // Amount
            var value = element.Total; // + (Math.floor((Math.random() * 2500) + 1));
            _this.barChartData[0].data[i] = value.toString();
            // Labels
            var label = element.PeriodName.replace("b4", _this.showLocal ? "עד" : "Before"); // replace 'b4' with 'Before'
            label = label.replace("/20", "/"); // minimize year in 'Before' Column
            _this.barChartLabels[i] = label;
            // Data
            DataProvider[i] = { "category": _this.barChartLabels[i], "dataCol": _this.barChartData[0].data[i] };
            if (value > max)
                max = value;
            i++;
        });
        var poisition = this.isRTL == true ? "right" : "left";
        makeAmBarChart(this.chartId, Graphs, DataProvider, max, null, null, null, null, poisition);
    };
    GLAccountOverviewComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GLAccountOverviewComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], GLAccountOverviewComponent);
    return GLAccountOverviewComponent;
}(BaseComponent_1.BaseComponent));
exports.GLAccountOverviewComponent = GLAccountOverviewComponent;
//# sourceMappingURL=GLAccountOverviewComponent.js.map