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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var GlobalDomainService_1 = require("../../../../Common/Services/GlobalDomainService");
var QuickBooksComponent = /** @class */ (function () {
    function QuickBooksComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.MyCardsLabel = "My Cards";
        this.isWindowOpened = false;
        this.CustomersListFilterd = [];
        this.searchText = "";
        this.HelpText = "";
        this.isCustomer = false;
        this.isPaymentMethod = false;
        this.isVatType = false;
        this.isReceivablesChargesType = false;
        this.isPayablesChargesType = false;
        this.isPaymentTerm = false;
        this.isVendor = false;
        this.isAgent = false;
        this.isCurrency = false;
        this.LoadedForTheFirstTime = true;
        this.QuantityLabel = "My Cards";
        this.selectedItem = null;
        this.TempList = [];
        this.EntityPM = entityArgs.EntityPM;
        this.GlobalDomainService = new GlobalDomainService_1.GlobalDomainService();
    }
    Object.defineProperty(QuickBooksComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            if (this.searchText != newValue) {
                this.searchText = newValue;
                this.OnSearchTextChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    QuickBooksComponent.prototype.SetWindowArgs = function (args) {
        this.args = args;
    };
    Object.defineProperty(QuickBooksComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (item) { this.selectedItem = item; },
        enumerable: true,
        configurable: true
    });
    QuickBooksComponent.prototype.OnSearchTextChanged = function () {
        var _this = this;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        this.timerToken = setTimeout(function () { return _this.GetCustomersBySearch(); }, 500);
    };
    QuickBooksComponent.prototype.OkButtonClicked = function () {
        if (this.SelectedItem != null)
            this.args.SelectedEntity = this.SelectedItem;
        this.CurrentSession.CloseCurrentWindow();
    };
    QuickBooksComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    QuickBooksComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.args.CardName == "Account" || this.args.CardName == "AccountBankCredit") {
            this.isPayablesChargesType = true;
            this.QuantityLabel = "Payables Charges Types";
            if (this.args.CardName == "AccountBankCredit")
                this.QuantityLabel = "Payment Account Types";
        }
        else if (this.args.LogitudeCardName == "Customer")
            this.isCustomer = true;
        else if (this.args.LogitudeCardName == "VatType") {
            this.isVatType = true;
            this.QuantityLabel = "VAT Types";
        }
        else if (this.args.LogitudeCardName == "ChargesType") {
            this.isReceivablesChargesType = true;
            this.QuantityLabel = "Receivables Charges Types";
        }
        else if (this.args.LogitudeCardName == "PaymentTerm") {
            this.isPaymentTerm = true;
            this.QuantityLabel = "Payment Terms";
        }
        else if (this.args.LogitudeCardName == "Currency") {
            this.isCurrency = true;
            this.QuantityLabel = "Currencies";
        }
        else if (this.args.LogitudeCardName == "Vendor")
            this.isVendor = true;
        else if (this.args.LogitudeCardName == "Agent")
            this.isAgent = true;
        else if (this.args.CardName == "PaymentMethod") {
            this.isPaymentMethod = true;
            this.QuantityLabel = "Payment Methods";
        }
        setTimeout(function () { return _this.GetCustomersBySearch(); }, 1);
    };
    QuickBooksComponent.prototype.GetCustomersBySearch = function () {
        var _this = this;
        if (!this.LoadedForTheFirstTime && !this.args.ReceivableCard && !this.args.PayableCard && !this.isReceivablesChargesType && !this.isPayablesChargesType && !this.isPaymentMethod) {
            if (this.searchText == null)
                this.searchText = "";
            this.CustomersListFilterd = this.TempList.filter(function (f) { return f.nameField.toLowerCase().indexOf(_this.SearchText.toLowerCase()) > -1; });
            if (this.args.LogitudeCardName == "VatType")
                this.MyCardsLabel = "VAT Types (" + this.CustomersListFilterd.length + ")";
            else if (this.args.LogitudeCardName == "ChargesType")
                this.MyCardsLabel = "Charges Types (" + this.CustomersListFilterd.length + ")";
            else if (this.args.LogitudeCardName == "PaymentTerm")
                this.MyCardsLabel = "Payment Terms (" + this.CustomersListFilterd.length + ")";
            else if (this.args.LogitudeCardName == "Currency")
                this.MyCardsLabel = "Currencies (" + this.CustomersListFilterd.length + ")";
        }
        else {
            if (this.searchText == null)
                this.searchText = "";
            this.CurrentSession.StartBusyIndicator("Searching ..");
            this.GlobalDomainService.GetQuickBooksQueries(this.args, this.SearchText).subscribe(function (myResult) {
                _this.TempList = [];
                _this.CustomersListFilterd = [];
                var list = myResult.Result;
                if (list != null)
                    if (list.length > 20) {
                        _this.HelpText = "Please, use the search tool to find more " + SessionLocator_1.SessionLocator.AccountingSystemPM.Name + " " + _this.args.LogitudeCardName + "s.";
                        list.pop();
                    }
                if (list != null)
                    list.forEach(function (d) {
                        if (_this.isPaymentTerm) {
                            if (d.nameField != null && d.nameField != "")
                                if (d.itemsField["0"] == null)
                                    d.itemsField["0"] = "";
                            _this.TempList.push(d);
                        }
                        else if (_this.isCurrency || _this.isPaymentMethod) {
                            if (d.nameField != null && d.nameField != "")
                                _this.TempList.push(d);
                        }
                        else if (_this.isReceivablesChargesType) {
                            if (d.incomeAccountRefField != null)
                                _this.TempList.push(d);
                        }
                        else if (_this.isPayablesChargesType) {
                            if (d.nameField != null && d.nameField != "" && d.accountTypeField != "Accounts Payable" && d.accountSubTypeField != "AccountsPayable")
                                _this.TempList.push(d);
                        }
                        else {
                            _this.TempList.push(d);
                        }
                    });
                _this.CustomersListFilterd = _this.TempList;
                _this.MyCardsLabel = _this.QuantityLabel + " (" + _this.CustomersListFilterd.length + ")";
                _this.CurrentSession.StopBusyIndicator();
            });
        }
        this.LoadedForTheFirstTime = false;
    };
    QuickBooksComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './QuickBooksComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], QuickBooksComponent);
    return QuickBooksComponent;
}());
exports.QuickBooksComponent = QuickBooksComponent;
//# sourceMappingURL=QuickBooksComponent.js.map