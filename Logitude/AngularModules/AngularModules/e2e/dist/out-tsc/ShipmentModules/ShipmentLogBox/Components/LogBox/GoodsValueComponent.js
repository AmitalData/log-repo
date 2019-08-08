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
var GoodsValueComponent = /** @class */ (function () {
    function GoodsValueComponent() {
        this.DataContext = this;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    GoodsValueComponent.prototype.ngOnInit = function () {
    };
    GoodsValueComponent.prototype.ngAfterViewInit = function () {
    };
    GoodsValueComponent.prototype.SetWindowArgs = function (args) {
        this.AdditionalData = args.AdditionalData;
    };
    GoodsValueComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    Object.defineProperty(GoodsValueComponent.prototype, "SupAccount", {
        get: function () { return new CustomNumbersPipe_1.CustomNumbersPipe().transform(this.AdditionalData.SupAccount, 0); },
        set: function (newValue) { this.AdditionalData.SupAccount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GoodsValueComponent.prototype, "IncotermId", {
        get: function () { return this.AdditionalData.IncotermId; },
        set: function (newValue) { this.AdditionalData.IncotermId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GoodsValueComponent.prototype, "Value", {
        get: function () { return new CustomNumbersPipe_1.CustomNumbersPipe().transform(this.AdditionalData.Value, 0); },
        set: function (newValue) { this.AdditionalData.Value = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GoodsValueComponent.prototype, "CurrencyName", {
        get: function () { return this.AdditionalData.CurrencyName; },
        set: function (newValue) { this.AdditionalData.CurrencyName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GoodsValueComponent.prototype, "CountryName", {
        get: function () { return this.AdditionalData.CountryName; },
        set: function (newValue) { this.AdditionalData.CountryName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GoodsValueComponent.prototype, "SupplierName", {
        get: function () { return this.AdditionalData.SupplierName; },
        set: function (newValue) { this.AdditionalData.SupplierName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GoodsValueComponent.prototype, "SupplierFreight", {
        get: function () { return new CustomNumbersPipe_1.CustomNumbersPipe().transform(this.AdditionalData.SupplierFreight, 0); },
        set: function (newValue) { this.AdditionalData.SupplierFreight = newValue; },
        enumerable: true,
        configurable: true
    });
    GoodsValueComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GoodsValueComponent.html'
        }),
        __metadata("design:paramtypes", [])
    ], GoodsValueComponent);
    return GoodsValueComponent;
}());
exports.GoodsValueComponent = GoodsValueComponent;
//# sourceMappingURL=GoodsValueComponent.js.map