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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CustomNumbersPipe_1 = require("../../../../Infrastructure/Pipes/CustomNumbersPipe");
var TaxScreenComponent = /** @class */ (function () {
    function TaxScreenComponent() {
        this.DataContext = this;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.totalTaxBasis = 0;
        this.totalTaxToPay = 0;
        this.totalAmount = 0;
        this.totalPostboned = 0;
    }
    TaxScreenComponent.prototype.ngOnInit = function () {
    };
    TaxScreenComponent.prototype.ngAfterViewInit = function () {
    };
    TaxScreenComponent.prototype.SetWindowArgs = function (args) {
        this.AdditionalData = args.AdditionalData;
        var ammount = 0;
        var basis = 0;
        var topay = 0;
        var post = 0;
        this.AdditionalData.TaxesDetails.forEach(function (item, key) {
            ammount += +(item.TaxAmount);
            basis += +(item.TaxBasis);
            topay += +(item.TaxToPay);
            post += +(item.TaxPostponed);
        });
        this.TotalAmount = ammount;
        this.TotalTaxToPay = topay;
        this.TotalTaxBasis = basis;
        this.TotalPostboned = post;
    };
    TaxScreenComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    Object.defineProperty(TaxScreenComponent.prototype, "Taxtypename", {
        get: function () { return this.AdditionalData.Taxtypename; },
        set: function (newValue) { this.AdditionalData.Taxtypename = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxScreenComponent.prototype, "TaxBasis", {
        get: function () { return new CustomNumbersPipe_1.CustomNumbersPipe().transform(this.AdditionalData.TaxBasis, 0); },
        set: function (newValue) { this.AdditionalData.TaxBasis = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxScreenComponent.prototype, "TaxToPay", {
        get: function () { return new CustomNumbersPipe_1.CustomNumbersPipe().transform(this.AdditionalData.TaxToPay, 0); },
        set: function (newValue) { this.AdditionalData.TaxToPay = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxScreenComponent.prototype, "TaxAmount", {
        get: function () { return new CustomNumbersPipe_1.CustomNumbersPipe().transform(this.AdditionalData.TaxAmount, 0); },
        set: function (newValue) { this.AdditionalData.TaxAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxScreenComponent.prototype, "TaxPostponed", {
        get: function () { return new CustomNumbersPipe_1.CustomNumbersPipe().transform(this.AdditionalData.TaxPostponed, 0); },
        set: function (newValue) { this.AdditionalData.TaxPostponed = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxScreenComponent.prototype, "TotalTaxBasis", {
        get: function () { return this.totalTaxBasis; },
        set: function (newValue) { this.totalTaxBasis = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxScreenComponent.prototype, "TotalTaxToPay", {
        get: function () { return this.totalTaxToPay; },
        set: function (newValue) { this.totalTaxToPay = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxScreenComponent.prototype, "TotalAmount", {
        get: function () { return this.totalAmount; },
        set: function (newValue) { this.totalAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TaxScreenComponent.prototype, "TotalPostboned", {
        get: function () { return this.totalPostboned; },
        set: function (newValue) { this.totalPostboned = newValue; },
        enumerable: true,
        configurable: true
    });
    TaxScreenComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TaxScreenComponent.html'
        }),
        __metadata("design:paramtypes", [])
    ], TaxScreenComponent);
    return TaxScreenComponent;
}());
exports.TaxScreenComponent = TaxScreenComponent;
//# sourceMappingURL=TaxScreenComponent.js.map