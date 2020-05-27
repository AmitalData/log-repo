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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var QuoteVATDetailsComponent = /** @class */ (function () {
    function QuoteVATDetailsComponent() {
        this.LocalCurrencyCode = null;
        this.SaleCurrencyCode = null;
        this.SelectedCurrencyCode = null;
        this.IsByLocalCurrency = false;
        this.IsCurrencyFilterVisible = false;
        this.ItemsSource = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
    }
    QuoteVATDetailsComponent.prototype.SetWindowArgs = function (args) {
        this.IsByLocalCurrency = args['IsLocalCurrency'];
        this.SaleCurrencyCode = args['SaleCurrencyCode'];
        this.IsCurrencyFilterVisible = args['IsCurrencyFilterVisible'];
        this.SelectedCurrencyCode = this.IsByLocalCurrency ? this.LocalCurrencyCode : this.SaleCurrencyCode;
        this.ItemsSource = args['TotalVATs'];
    };
    QuoteVATDetailsComponent.prototype.OnSelectCurrency = function (myCurrencyCode) {
        this.SelectedCurrencyCode = myCurrencyCode;
        if (myCurrencyCode == this.LocalCurrencyCode) {
            this.IsByLocalCurrency = true;
        }
        else {
            this.IsByLocalCurrency = false;
        }
    };
    QuoteVATDetailsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    QuoteVATDetailsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './QuoteVATDetailsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], QuoteVATDetailsComponent);
    return QuoteVATDetailsComponent;
}());
exports.QuoteVATDetailsComponent = QuoteVATDetailsComponent;
//# sourceMappingURL=QuoteVATDetailsComponent.js.map