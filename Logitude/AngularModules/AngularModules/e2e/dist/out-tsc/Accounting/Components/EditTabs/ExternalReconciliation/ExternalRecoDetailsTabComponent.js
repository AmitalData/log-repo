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
var AccountingEntityHelper_1 = require("./../../../Utilities/AccountingEntityHelper");
var core_1 = require("@angular/core");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var ReconcileEventManager_1 = require("../../../Utilities/ReconcileEventManager");
//Services
var ExternalReconciliationPMService_1 = require("../../../Services/StandardPMs/ExternalReconciliationPMService");
var ExternalReconciliationExtendedPMService_1 = require("../../../Services/ExtendedPMs/ExternalReconciliationExtendedPMService");
var LedgerTransactionExtendedListService_1 = require("../../../Services/ExtendedLists/LedgerTransactionExtendedListService");
var ExternalReconciliationExtendedListService_1 = require("../../../Services/ExtendedLists/ExternalReconciliationExtendedListService");
var ReconcileExternalPageExtendedListService_1 = require("../../../Services/ExtendedLists/ReconcileExternalPageExtendedListService");
var CurrencyPMService_1 = require("../../../../Common/Services/StandardPMs/CurrencyPMService");
var ExternalRecoDetailsTabComponent = /** @class */ (function (_super) {
    __extends(ExternalRecoDetailsTabComponent, _super);
    function ExternalRecoDetailsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "ExternalReconciliation";
        _this.DataContext = _this;
        _this.TotalSum = 0;
        _this.NoRows = false;
        _this.searchText = "";
        _this.isRTL = false;
        _this.openAmountCurrency = "";
        _this.entityListService = new EntityListService_1.EntityListService();
        _this.ledgerTransactionExtendedListService = new LedgerTransactionExtendedListService_1.LedgerTransactionExtendedListService();
        _this._ExternalReconciliationExtendedListService = new ExternalReconciliationExtendedListService_1.ExternalReconciliationExtendedListService();
        _this._ReconcileExternalPageExtendedListService = new ReconcileExternalPageExtendedListService_1.ReconcileExternalPageExtendedListService();
        _this.externalReconciliationExtendedPMService = new ExternalReconciliationExtendedPMService_1.ExternalReconciliationExtendedPMService();
        _this.externalReconciliationPMService = new ExternalReconciliationPMService_1.ExternalReconciliationPMService();
        _this._CurrencyPMService = new CurrencyPMService_1.CurrencyPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.bankAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate('ReconcileExternalPageLine.F.Amount');
        _this.ledgerAmountHeader = TextCodeTranslator_1.TextCodeTranslator.Translate('LedgerTransaction.F.OpenAmount');
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.EntityPM = entityArgs.EntityPM;
        _this.ExternalRecoPM = entityArgs.EntityPM;
        _this.TransactionsLines = new ObservableCollection_1.ObservableCollection([]);
        _this.BankPageLines = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    ExternalRecoDetailsTabComponent.prototype.ngOnInit = function () {
        this.GetDefaults();
        this.FillGrids();
    };
    ExternalRecoDetailsTabComponent.prototype.GetDefaults = function () {
        var _this = this;
        var glAccountCurrencyId = this.ExternalRecoPM.AccountCurrencyId;
        //Get currency
        if (!glAccountCurrencyId || glAccountCurrencyId == "multi") {
            this.ledgerTransactionExtendedListService.GetFirstLedgerTransaction(this.EntityPM.GLAccountId).subscribe(function (serviceResponse) {
                if (serviceResponse.Result) {
                    var result = serviceResponse.Result;
                    var transaction = result.Result; // get the data
                    _this.openAmountCurrency = transaction ? transaction.CurrencyCode : "";
                    _this.ledgerAmountHeader += " (" + _this.openAmountCurrency + ")";
                    _this.bankAmountHeader += " (" + _this.openAmountCurrency + ")";
                }
            });
        }
        else {
            this._CurrencyPMService.get(glAccountCurrencyId).subscribe(function (myResult) {
                var currency = myResult.Result;
                _this.openAmountCurrency = currency ? currency.Code : "";
                _this.ledgerAmountHeader += " (" + _this.openAmountCurrency + ")";
                _this.bankAmountHeader += " (" + _this.openAmountCurrency + ")";
            });
        }
    };
    //#region Data
    ExternalRecoDetailsTabComponent.prototype.FillGrids = function () {
        //#region 1- prepare Ids list
        var transactionsLinesIds = [];
        var bankPageLinesIds = [];
        if (this.ExternalRecoPM) {
            for (var _i = 0, _a = this.ExternalRecoPM.ExternalReconciliationLines; _i < _a.length; _i++) {
                var line = _a[_i];
                if (line.LedgerTransactionId)
                    transactionsLinesIds.push(line.LedgerTransactionId);
                else if (line.ExternalPageLineId)
                    bankPageLinesIds.push(line.ExternalPageLineId);
                else
                    console.log("What type are you !!!!!!!", this.ExternalRecoPM, line);
            }
        }
        else {
            console.log("No ExternalRecoPM entity !!");
        }
        //#endregion
        // 2- get lines
        this.CurrentSession.StartBusyIndicatorLoading();
        if (bankPageLinesIds.length > 0) {
            this.GetBankLines(bankPageLinesIds, transactionsLinesIds); // then get ledger lines
        }
        else {
            console.log("No bank lines!");
            this.GetLedgerLines(transactionsLinesIds);
        }
    };
    ExternalRecoDetailsTabComponent.prototype.GetBankLines = function (bankPageLinesIds, transactionsLinesIds) {
        var _this = this;
        this._ReconcileExternalPageExtendedListService.getBankPageLinesByIds(bankPageLinesIds).subscribe(function (myResult) {
            var result = myResult.Result;
            var list = result.Result;
            // Incapsulate transactions
            var pageLines = [];
            for (var _i = 0, list_1 = list; _i < list_1.length; _i++) {
                var line = list_1[_i];
                var pageLine = new BankLineModel(line, _this, -1);
                pageLines.push(pageLine);
            }
            //
            // 3- fil group hash
            for (var _a = 0, _b = _this.ExternalRecoPM.ExternalReconciliationLines; _a < _b.length; _a++) {
                var recoLine = _b[_a];
                if (recoLine.ExternalPageLineId) {
                    var item = pageLines.find(function (d) { return d.Id == recoLine.ExternalPageLineId; });
                    item.GroupHash = recoLine.GroupNumber;
                }
            }
            // 4- sort
            list.sort(function (a, b) { return (a.GroupHash === b.GroupHash) ? 0 : (a.GroupHash < b.GroupHash) ? -1 : 1; });
            _this.BankPageLines.InsertCollection(pageLines);
            _this.GetLedgerLines(transactionsLinesIds);
        });
    };
    ExternalRecoDetailsTabComponent.prototype.GetLedgerLines = function (transactionsLinesIds) {
        var _this = this;
        // 2- get ledger transactions lines
        if (transactionsLinesIds.length > 0) {
            this.ledgerTransactionExtendedListService.getLedgerTransactionsByIds(transactionsLinesIds).subscribe(function (myResult) {
                var result = myResult.Result;
                var list = result.Result;
                // Incapsulate transactions
                var transactionsItems = [];
                for (var _i = 0, list_2 = list; _i < list_2.length; _i++) {
                    var line = list_2[_i];
                    var item = new TransactionLineModel(line, _this, -1);
                    transactionsItems.push(item);
                }
                //
                // 3- fil group hash
                for (var _a = 0, _b = _this.ExternalRecoPM.ExternalReconciliationLines; _a < _b.length; _a++) {
                    var xxsxx = _b[_a];
                    if (xxsxx.LedgerTransactionId) {
                        var vfvv = transactionsItems.find(function (d) { return d.Id == xxsxx.LedgerTransactionId; });
                        vfvv.GroupHash = xxsxx.GroupNumber;
                    }
                }
                // 4- sort
                transactionsItems.sort(function (a, b) { return (a.GroupHash === b.GroupHash) ? 0 : (a.GroupHash < b.GroupHash) ? -1 : 1; });
                _this.TransactionsLines.InsertCollection(transactionsItems);
                _this.FillGroupHash();
                _this.CurrentSession.StopBusyIndicator();
            });
        }
        else {
            console.log("No ledger lines!");
            this.CurrentSession.StopBusyIndicator();
        }
    };
    ExternalRecoDetailsTabComponent.prototype.FillGroupHash = function () {
        //// 3- fil group hash
        //for (var line of this.ExternalRecoPM.ExternalReconciliationLines) {
        //    if (line.LedgerTransactionId) {
        //        var item = this.TransactionsLines.Collection.find(d => d.Id == line.LedgerTransactionId);
        //        item.GroupHash = line.GroupNumber;
        //    }
        //    else if (line.ExternalPageLineId) {
        //        var item = this.BankPageLines.Collection.find(d => d.Id == line.ExternalPageLineId);
        //        item.GroupHash = line.GroupNumber;
        //    }
        //}
    };
    //#endregion
    ExternalRecoDetailsTabComponent.prototype.OpenSource = function (id, type) {
        // Type:    SourceTypeCode
        // Id:      SourceId
        // Display: SourceNumber
        var tableName = "Journal";
        switch (type) {
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
                ObjectTableName: tableName,
                BackButtonLabel: 'GLAccount'
            });
        });
    };
    ExternalRecoDetailsTabComponent.prototype.OpenJournal = function (id) {
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                });
            });
        }
    };
    ExternalRecoDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ExternalRecoDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ExternalRecoDetailsTabComponent);
    return ExternalRecoDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ExternalRecoDetailsTabComponent = ExternalRecoDetailsTabComponent;
var TransactionLineModel = /** @class */ (function (_super) {
    __extends(TransactionLineModel, _super);
    function TransactionLineModel(ledgerTransaction, parent, myRowIndex) {
        var _this = _super.call(this) || this;
        _this.ledgerTransaction = ledgerTransaction;
        _this.parent = parent;
        _this.myRowIndex = myRowIndex;
        _this.LedgerTransactionPM = null;
        _this.ObjectTableName = "LedgerTransaction";
        _this.DataContext = _this;
        _this.isRTL = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.EntityPM = _this.parent.EntityPM;
        _this.LedgerTransactionPM = ledgerTransaction;
        //this.OriginalAmount = parent.CalculateOriginalAmount(this);
        //if (!this.AmountToReconcile)
        //    this.AmountToReconcile = this.ledgerTransaction.OpenAmount;
        _this.RowIndex = myRowIndex;
        //this.OddEven = this.ColorMe();
        //#region Set Icons
        var iconTxt = AccountingEntityHelper_1.AccountingEntityHelper.getEntityIcon(_this.LedgerTransactionPM.SourceTypeCode);
        _this.IconCode = iconTxt;
        return _this;
        //#endregion
    }
    Object.defineProperty(TransactionLineModel.prototype, "IsCredit", {
        get: function () {
            return this.LedgerTransactionPM.ForeignAmountCredit != 0;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "GroupHash", {
        get: function () { return this.LedgerTransactionPM.GroupHash; },
        set: function (value) { this.LedgerTransactionPM.GroupHash = value; },
        enumerable: true,
        configurable: true
    });
    ;
    ;
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
    Object.defineProperty(TransactionLineModel.prototype, "ForeignAmount", {
        get: function () { return this.LedgerTransactionPM.ForeignAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TransactionLineModel.prototype, "OpenAmount", {
        get: function () { return this.LedgerTransactionPM.OpenAmount; },
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
    Object.defineProperty(TransactionLineModel.prototype, "OpenAmountCurrencyId", {
        //get IsPartial() { return this.OpenAmount != this.AmountToReconcile; }
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
    //#endregion
    //#region Row Coloring
    TransactionLineModel.prototype.ColorMe = function () {
        //if (AppTool.IsNullOrEmpty(this.parent.lastGroupNumber))
        //    this.parent.lastGroupNumber = this.GroupHash;
        //if (this.parent.lastGroupNumber == this.GroupHash) {
        //    return this.parent.lastColorOperation == true;
        //} else {
        //    this.parent.lastGroupNumber = this.GroupHash;
        //    this.parent.lastColorOperation = !this.parent.lastColorOperation;
        //    return this.parent.lastColorOperation == true;
        //}
    };
    //#endregion
    TransactionLineModel.prototype.CalculatOriginalCurruncy = function () {
        //
        // [i] copied from list template
        //
        if (!Tools_1.AppTool.IsNullOrEmpty(ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode)) {
            // this code was copied to reconcile window, if it need change, please chenge it in reconcile window too
            if (ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode == "0") { // 0-local currency
                // local
                return SessionLocator_1.SessionLocator.TenantPM.CurrencySign;
            }
            else if (ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode == "1") { // 1-foreign currency
                // foreign
                return this.ledgerTransaction.CurrencySign;
            }
        }
    };
    return TransactionLineModel;
}(BaseComponent_1.BaseComponent));
var BankLineModel = /** @class */ (function (_super) {
    __extends(BankLineModel, _super);
    function BankLineModel(pageLine, parent, myRowIndex) {
        var _this = _super.call(this) || this;
        _this.pageLine = pageLine;
        _this.parent = parent;
        _this.myRowIndex = myRowIndex;
        _this.PageLinePM = null;
        _this.ObjectTableName = "ReconcileExternalPageLine";
        _this.DataContext = _this;
        _this.isRTL = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        _this.EntityPM = _this.parent.EntityPM;
        _this.PageLinePM = pageLine;
        _this.RowIndex = myRowIndex;
        return _this;
    }
    Object.defineProperty(BankLineModel.prototype, "IsCredit", {
        get: function () {
            return this.PageLinePM.CreditAmount != 0;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankLineModel.prototype, "GroupHash", {
        get: function () { return this.pageLine.GroupHash; },
        set: function (value) { this.pageLine.GroupHash = value; },
        enumerable: true,
        configurable: true
    });
    ;
    ;
    Object.defineProperty(BankLineModel.prototype, "Id", {
        //#region Properties
        get: function () { return this.PageLinePM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankLineModel.prototype, "Amount", {
        get: function () { return this.PageLinePM.Amount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankLineModel.prototype, "Reference", {
        get: function () { return this.PageLinePM.Reference; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankLineModel.prototype, "ReferenceDate", {
        get: function () { return this.PageLinePM.ReferenceDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankLineModel.prototype, "Notes", {
        get: function () { return this.PageLinePM.Notes; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BankLineModel.prototype, "LineNumber", {
        get: function () { return this.PageLinePM.LineNumber; },
        enumerable: true,
        configurable: true
    });
    return BankLineModel;
}(BaseComponent_1.BaseComponent));
//# sourceMappingURL=ExternalRecoDetailsTabComponent.js.map