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
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var InvoiceDomainService_1 = require("../../../Services/InvoiceDomainService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var TransferHeaderDetailsTabComponent = /** @class */ (function (_super) {
    __extends(TransferHeaderDetailsTabComponent, _super);
    function TransferHeaderDetailsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "AccountingTransferHeader";
        _this.DataContext = _this;
        _this.ItemsSource = [];
        _this.SelectedItem = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ColumnHeader_Date = null;
        _this.ColumnHeader_Number = null;
        _this.ColumnHeader_Partner = null;
        _this.ColumnHeader_Status = null;
        _this.ColumnHeader_Amount = null;
        _this.EntityPM = entityArgs.EntityPM;
        _this.ItemsSource = _this.EntityPM.TransferLines;
        _this.invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
        _this.SetColumnHeaders();
        return _this;
    }
    TransferHeaderDetailsTabComponent.prototype.SetColumnHeaders = function () {
        switch (this.EntityPM.AccountingTransferTypeCode) {
            case "ARIN": {
                this.ColumnHeader_Date = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.CH.InvoiceDateListLable");
                this.ColumnHeader_Number = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.CH.InvoiceNumberListLable");
                this.ColumnHeader_Partner = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.CH.BillToNameListLable");
                this.ColumnHeader_Status = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.CH.StatusNameRateListLable");
                this.ColumnHeader_Amount = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.CH.AmountInInvoiceCurrencyListLable");
                break;
            }
            case "APIN": {
                this.ColumnHeader_Date = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.CH.InvoiceDateListLable");
                this.ColumnHeader_Number = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.CH.InvoiceNumberListLable");
                this.ColumnHeader_Partner = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.CH.VendorNameListLable");
                this.ColumnHeader_Status = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.CH.StatusNameListLable");
                this.ColumnHeader_Amount = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.CH.AmountInInvoiceCurrencyListLable");
                break;
            }
            case "ARPA": {
                this.ColumnHeader_Date = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.CH.RegisterDateListLable");
                this.ColumnHeader_Number = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.CH.PaymentNoListLable");
                this.ColumnHeader_Partner = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.CH.BillToNameListLable");
                this.ColumnHeader_Status = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.CH.StatusNameListLable");
                this.ColumnHeader_Amount = TextCodeTranslator_1.TextCodeTranslator.Translate("ARPayment.CH.AmountInPaymentCurrencyListLable");
                break;
            }
            case "APPA": {
                this.ColumnHeader_Date = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.CH.RegisterDateListLable");
                this.ColumnHeader_Number = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.CH.PaymentNoListLable");
                this.ColumnHeader_Partner = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.CH.VendorNameListLable");
                this.ColumnHeader_Status = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.CH.StatusNameListLable");
                this.ColumnHeader_Amount = TextCodeTranslator_1.TextCodeTranslator.Translate("APPayment.CH.AmountInPaymentCurrencyListLable");
                break;
            }
        }
    };
    TransferHeaderDetailsTabComponent.prototype.RebuildClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Rebuilding ...");
        this.invoiceDomainService.RebuildTransferFile(this.EntityPM.Id).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    TransferHeaderDetailsTabComponent.prototype.DownloadClicked = function () {
        DownloadManager_1.DownloadManager.DownloadTransferHeaderFile(this.EntityPM.FileName);
    };
    TransferHeaderDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TransferHeaderDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], TransferHeaderDetailsTabComponent);
    return TransferHeaderDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.TransferHeaderDetailsTabComponent = TransferHeaderDetailsTabComponent;
//# sourceMappingURL=TransferHeaderDetailsTabComponent.js.map