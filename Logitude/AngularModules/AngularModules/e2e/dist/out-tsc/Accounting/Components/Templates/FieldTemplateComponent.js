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
var core_1 = require("@angular/core");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var JournalExtendedListService_1 = require("../../Services/ExtendedLists/JournalExtendedListService");
var ARPaymentExtendedListService_1 = require("../../../Invoice/Services/ExtendedLists/ARPaymentExtendedListService");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var FieldTemplateComponent = /** @class */ (function () {
    function FieldTemplateComponent() {
        this.Entity = null;
        this.FieldName = null;
        this.FieldValue = null;
        this.ObjectTableName = null;
        this.IsHeaderScreenTemplate = false;
        this.SpotlightDataTemplate = null;
        this.IsSpotLightTemplate = false;
        this._JournalExtendedListService = new JournalExtendedListService_1.JournalExtendedListService();
        this._ARPaymentExtendedListService = new ARPaymentExtendedListService_1.ARPaymentExtendedListService();
        this.isRTL = false;
        this.showLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        this.tenantCurrency = "";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.TenantCurrencySign = SessionLocator_1.SessionLocator.TenantPM.CurrencySign;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.tenantCurrency = SessionLocator_1.SessionLocator.TenantPM.CurrencyCode;
    }
    FieldTemplateComponent.prototype.Run = function (args) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsHeaderScreenTemplate = args['IsHeaderScreenTemplate'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];
        if (this.Entity != null && this.FieldName != null) {
            this.FieldValue = this.Entity[this.FieldName];
        }
        if (this.ObjectTableName == "Revaluation" && this.FieldName == "Status") {
            if (this.FieldValue == "Done") {
                this.fontColor = "green";
            }
            else if (this.FieldValue == "In Progress") {
                this.fontColor = "orange";
            }
            else
                this.fontColor = "black";
        }
        if (this.ObjectTableName == "PaymentCheque" && this.FieldName == "PaymentChequeStatusCode") {
            if (this.FieldValue == "1") {
                this.textColor = "orange";
            }
            else if (this.FieldValue == "2") {
                this.textColor = "green";
            }
            else if (this.FieldValue == "4") {
                this.textColor = "red";
            }
        }
        if (this.ObjectTableName == "PaymentCheque" && this.FieldName == "PaymentChequeStatusName") {
            if (this.Entity.PaymentChequeStatusCode == "1") {
                this.textColor = "orange";
            }
            else if (this.Entity.PaymentChequeStatusCode == "2") {
                this.textColor = "green";
            }
            else if (this.Entity.PaymentChequeStatusCode == "4") {
                this.textColor = "red";
            }
        }
        if (this.ObjectTableName == "PaymentCheque" && this.FieldName == "PaymentChequeStatusName") {
            if (SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal) {
                this.FieldValue = this.Entity.StatusEnglishName;
            }
            else {
                this.FieldValue = this.Entity.PaymentChequeStatusName;
            }
        }
        if (this.ObjectTableName == "TaxDeductionReport" && this.FieldName == "Status") {
            if (SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal) {
                this.FieldValue = this.Entity.Status;
            }
            else {
                this.FieldValue = this.Entity.StatusLocalName;
            }
            if (this.Entity.StatusTypeCode == "2") {
                this.textColor = "orange";
            }
            else if (this.Entity.StatusTypeCode == "3") {
                this.textColor = "green";
            }
            else if (this.Entity.StatusTypeCode == "4") {
                this.textColor = "red";
            }
        }
        if (this.ObjectTableName == "OpenFormatReport" && this.FieldName == "Status") {
            if (SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal) {
                this.FieldValue = this.Entity.Status;
            }
            else {
                this.FieldValue = this.Entity.StatusLocalName;
            }
        }
        if (this.ObjectTableName == "OpenFormatReport" && this.FieldName == "CreatedByUserName") {
            if (SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal) {
                this.FieldValue = this.Entity.CreatedByUserName;
            }
            else {
                this.FieldValue = this.Entity.UserLocalName;
            }
        }
        if (this.ObjectTableName == "TaxReport" && this.FieldName == "StatusEnglishName") {
            if (SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal) {
                this.FieldValue = this.Entity.StatusEnglishName;
            }
            else {
                this.FieldValue = this.Entity.StatusLocalName;
            }
        }
    };
    FieldTemplateComponent.prototype.Abs = function (num) {
        if (!Tools_1.AppTool.IsNullOrEmpty(num)) {
            return num > 0 ? num : num * -1;
        }
    };
    FieldTemplateComponent.prototype.OpenCashBook = function (id) {
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'CashBook' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                });
            });
        }
    };
    FieldTemplateComponent.prototype.OpenJournal = function (id) {
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
    FieldTemplateComponent.prototype.OpenSource = function (id) {
        // Type:    AccountingEntityCode
        // Id:      AccountingEntityId
        // Display: AccountingEntityReference
        var tableName = "Journal";
        switch (this.Entity.AccountingEntityCode) {
            // 1-Journal
            case '1': {
                return;
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
                BackButtonLabel: TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.SourceJournal"),
            });
        });
    };
    FieldTemplateComponent.prototype.OpenGLAccount = function (id) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'GLAccount' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                });
            });
        }
    };
    FieldTemplateComponent.prototype.OpenBankAccount = function (id) {
        if (!Tools_1.AppTool.IsNullOrEmpty(id)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'BankAccount', BackButtonLabel: 'Deposit' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                });
            });
        }
    };
    FieldTemplateComponent.prototype.VatNumberClicked = function () {
    };
    FieldTemplateComponent.prototype.GetAccountingIntegrityCheckStatus = function () {
        var color = "black";
        switch (this.Entity['StatusCode']) {
            case '2': // 2- in progress
                color = '#0043ff';
                break;
            case '3':
                color = '#d69f03';
                break;
            case '4':
                color = 'green';
                break;
            case '5':
                color = 'red';
                break;
            default:
                break;
        }
        return color;
    };
    FieldTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FieldTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], FieldTemplateComponent);
    return FieldTemplateComponent;
}());
exports.FieldTemplateComponent = FieldTemplateComponent;
//# sourceMappingURL=FieldTemplateComponent.js.map