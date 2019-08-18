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
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var AccountingAdvancedSettingsComponent = /** @class */ (function (_super) {
    __extends(AccountingAdvancedSettingsComponent, _super);
    function AccountingAdvancedSettingsComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "AccountingSetting";
        _this.IsEnableMultiCurrencyARPaymentsVisible = false;
        _this.IsEnableInvoiceStocksManagementVisible = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARPayment", "EnableMultiCurrency")) {
            _this.IsEnableMultiCurrencyARPaymentsVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "ManageStocks")) {
            _this.IsEnableInvoiceStocksManagementVisible = true;
        }
        return _this;
    }
    AccountingAdvancedSettingsComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = this.DataContext.EntityPM;
        this.Clone();
    };
    //Commands 
    AccountingAdvancedSettingsComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AccountingAdvancedSettingsComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AccountingAdvancedSettingsComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('EnableMultiPercentageVATTypes');
        this.myCloner.AddField('NotifyPastDateOnInvoiceEdit');
        this.myCloner.AddField('RegistryDateTypeCode');
        this.myCloner.AddField('EnableMultiCurrencyARPayments');
        this.myCloner.AddField('EnableNegativeOffsetARPayments');
        this.myCloner.AddEntity(this.EntityPM);
    };
    AccountingAdvancedSettingsComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AccountingAdvancedSettingsComponent = __decorate([
        core_1.Component({
            selector: 'AccountingAdvancedSettingsComponent',
            moduleId: module.id,
            templateUrl: './AccountingAdvancedSettingsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AccountingAdvancedSettingsComponent);
    return AccountingAdvancedSettingsComponent;
}(BaseComponent_1.BaseComponent));
exports.AccountingAdvancedSettingsComponent = AccountingAdvancedSettingsComponent;
//# sourceMappingURL=AccountingAdvancedSettingsComponent.js.map