"use strict";
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
var AccountingEntityHelper_1 = require("./../../Utilities/AccountingEntityHelper");
var SessionLocator_1 = require("./../../../Infrastructure/Utilities/SessionLocator");
var core_1 = require("@angular/core");
var core_2 = require("@angular/core");
var JournalExtendedListService_1 = require("../../Services/ExtendedLists/JournalExtendedListService");
var ARPaymentExtendedListService_1 = require("../../../Invoice/Services/ExtendedLists/ARPaymentExtendedListService");
var Tools_1 = require("../../../Infrastructure/Tools");
var ReconcileEventManager_1 = require("../../Utilities/ReconcileEventManager");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var GlAccountLedgerTransactionsListTemplate = /** @class */ (function () {
    function GlAccountLedgerTransactionsListTemplate(CD) {
        this.CD = CD;
        this._JournalExtendedListService = new JournalExtendedListService_1.JournalExtendedListService();
        this._ARPaymentExtendedListService = new ARPaymentExtendedListService_1.ARPaymentExtendedListService();
        this.CheckBoxChecked = new core_2.EventEmitter();
        this.Changed = new core_2.EventEmitter();
        this.isRTL = false;
        this.showLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.checkBoxState = false;
        this.TenantCurrencySign = SessionLocator_1.SessionLocator.TenantPM.CurrencySign;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";
    }
    Object.defineProperty(GlAccountLedgerTransactionsListTemplate.prototype, "CheckBoxState", {
        get: function () {
            return this.checkBoxState;
        },
        set: function (isChecked) {
            console.log("Changed to: ", isChecked);
            this.checkBoxState = isChecked;
        },
        enumerable: true,
        configurable: true
    });
    GlAccountLedgerTransactionsListTemplate.prototype.setVariables = function (rowData, fieldName, MyAdditionalData) {
        this.rowData = rowData;
        if (this.rowData.IsChecked == true) {
            console.log("Oh Yea True");
        }
        else {
            console.log("Else " + this.rowData.IsChecked);
        }
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;
        //#region Set Icons
        this.IconCode = AccountingEntityHelper_1.AccountingEntityHelper.getEntityIcon(this.rowData.SourceTypeCode);
        //#endregion
        var isDestroyed = this.CD["destroyed"];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    GlAccountLedgerTransactionsListTemplate.prototype.Abs = function (number) {
        return number < 0 ? number * -1 : number;
    };
    GlAccountLedgerTransactionsListTemplate.prototype.OpenSource = function (id) {
        // Type:    SourceTypeCode
        // Id:      SourceId
        // Display: SourceNumber
        var tableName = "Journal";
        switch (this.rowData.SourceTypeCode) {
            // 1-Journal
            case "1": {
                tableName = "Journal";
                break;
            }
            // 2-ARInvoice
            case "2": {
                tableName = "ARInvoice";
                break;
            }
            // 3-ARPayment
            case "3": {
                tableName = "ARPayment";
                break;
            }
            // 4-APInvoice
            case "4": {
                tableName = "APInvoice";
                break;
            }
            // 5-APPayment
            case "5": {
                tableName = "APPayment";
                break;
            }
            // 6-Cheque Deposit
            case "6": {
                tableName = "BankDeposit";
                break;
            }
            // 7-Cash Deposit
            case "7": {
                tableName = "BankDeposit";
                break;
            }
            // 8-Revaluation
            case "8": {
                tableName = "Revaluation";
                break;
            }
            // 9-PaymentCheque
            case "9": {
                tableName = "PaymentCheque";
                break;
            }
        }
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditComponent", this.CurrentSession.SessionLocation.viewContainerRef).then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityId: id,
                ObjectTableName: tableName
            });
        });
    };
    GlAccountLedgerTransactionsListTemplate.prototype.OpenJournal = function (id) {
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditComponent", this.CurrentSession.SessionLocation.viewContainerRef).then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id,
                    ObjectTableName: "Journal"
                });
                cmpRef.instance.BackCompleted.subscribe(function (bk) { });
            });
        }
    };
    GlAccountLedgerTransactionsListTemplate.prototype.CheckBoxClicked = function (checked) {
        //console.log("clicked: ", checked);
        //this.rowData['IsChecked'] = checked;
        ReconcileEventManager_1.ReconcileEventManager.CheckBoxChecked.emit({
            line: this.rowData,
            isChecked: checked,
            RowIndex: this.AdditionalData.rowIndex
        });
        //ReconcileEventManager.RowUnselected.subscribe(($event) => {
        //    this.rowData = ro
        //});
    };
    GlAccountLedgerTransactionsListTemplate.prototype.CalculateOriginalAmount = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode)) {
            if (ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode == "0") {
                // 0-local currency
                if (this.rowData["LocalAmountCredit"] == 0) {
                    return this.rowData["LocalAmountDebit"];
                }
                else {
                    return this.rowData["LocalAmountCredit"]; // -1 *
                }
            }
            else if (ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode == "1") {
                // 1-foreign currency
                if (this.rowData["ForeignAmountCredit"] == 0) {
                    return this.rowData["ForeignAmountDebit"];
                }
                else {
                    return this.rowData["ForeignAmountCredit"]; // -1 *
                }
            }
        }
    };
    GlAccountLedgerTransactionsListTemplate.prototype.CalculatOriginalCurruncy = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode)) {
            // this code was copied to reconcile window, if it need change, please chenge it in reconcile window too
            if (ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode == "0") {
                // 0-local currency
                // local
                return SessionLocator_1.SessionLocator.TenantPM.CurrencySign;
            }
            else if (ReconcileEventManager_1.ReconcileEventManager.GLAccountReconcileMethodCode == "1") {
                // 1-foreign currency
                // foreign
                return this.rowData["CurrencySign"];
            }
        }
    };
    GlAccountLedgerTransactionsListTemplate.prototype.GetIndicatorText = function () {
        if (this.rowData["OpenAmount"] != this.CalculateOriginalAmount())
            return this.showLocal ? "סכום פתוח חלקית" : "Partial transaction";
        else
            return this.showLocal ? "סכום פתוח " : "Open transaction";
    };
    GlAccountLedgerTransactionsListTemplate.prototype.OpenGLAccount = function () {
        var account2open = this.rowData["OppositeAccountId"];
        var tableName = "GLAccount";
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditComponent", this.CurrentSession.SessionLocation.viewContainerRef).then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityId: account2open,
                ObjectTableName: tableName
            });
        });
    };
    __decorate([
        core_2.Output(),
        __metadata("design:type", Object)
    ], GlAccountLedgerTransactionsListTemplate.prototype, "CheckBoxChecked", void 0);
    __decorate([
        core_2.Output(),
        __metadata("design:type", core_2.EventEmitter)
    ], GlAccountLedgerTransactionsListTemplate.prototype, "Changed", void 0);
    GlAccountLedgerTransactionsListTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "./GlAccountLedgerTransactionsListTemplate.html"
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], GlAccountLedgerTransactionsListTemplate);
    return GlAccountLedgerTransactionsListTemplate;
}());
exports.GlAccountLedgerTransactionsListTemplate = GlAccountLedgerTransactionsListTemplate;
//# sourceMappingURL=GlAccountLedgerTransactionsListTemplate.js.map