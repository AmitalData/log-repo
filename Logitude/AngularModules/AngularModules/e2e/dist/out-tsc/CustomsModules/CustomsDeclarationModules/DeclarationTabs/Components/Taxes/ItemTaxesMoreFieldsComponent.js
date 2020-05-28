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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var ItemTaxesMoreFieldsComponent = /** @class */ (function () {
    function ItemTaxesMoreFieldsComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.ObjectTableName = "Customs.SupplierInvoiceItemsTax";
        this.DataContext = this;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //ngOnInit() {
        //    this.EntityPM = this.entityArgs.EntityPM;
        //}
        this._IsVisible = false;
    }
    ItemTaxesMoreFieldsComponent.prototype.SetWindowArgs = function (windowArgs) {
        ///console.warn(windowArgs);
        this.EntityPM = windowArgs;
        this._IsVisible = true;
    };
    ItemTaxesMoreFieldsComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    Object.defineProperty(ItemTaxesMoreFieldsComponent.prototype, "DefinedPerUnitMeasure", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.DefinedPerUnitMeasure; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemTaxesMoreFieldsComponent.prototype, "AlternateDefinedPerUnitMeasure", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.AlternateDefinedPerUnitMeasure; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemTaxesMoreFieldsComponent.prototype, "AlternateRate", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.AlternateRate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemTaxesMoreFieldsComponent.prototype, "DefinedPerUnitQuantity", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.DefinedPerUnitQuantity; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemTaxesMoreFieldsComponent.prototype, "AlternateDefinedPerUnitQuant", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.AlternateDefinedPerUnitQuant; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemTaxesMoreFieldsComponent.prototype, "TradeLevyNumber", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.TradeLevyNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemTaxesMoreFieldsComponent.prototype, "MeasurementUnitCode", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.MeasurementUnitCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemTaxesMoreFieldsComponent.prototype, "AlternateMeasurementUnitCode", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.AlternateMeasurementUnitCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemTaxesMoreFieldsComponent.prototype, "TotalBtlCoverageNIS", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.TotalBtlCoverageNIS; },
        enumerable: true,
        configurable: true
    });
    ItemTaxesMoreFieldsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ItemTaxesMoreFieldsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ItemTaxesMoreFieldsComponent);
    return ItemTaxesMoreFieldsComponent;
}());
exports.ItemTaxesMoreFieldsComponent = ItemTaxesMoreFieldsComponent;
//# sourceMappingURL=ItemTaxesMoreFieldsComponent.js.map