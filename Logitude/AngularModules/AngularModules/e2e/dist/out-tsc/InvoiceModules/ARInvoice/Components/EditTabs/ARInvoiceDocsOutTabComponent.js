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
var ARInvoiceDocsOutTabComponent = /** @class */ (function () {
    function ARInvoiceDocsOutTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = null;
        this.ObjectTableName = "ARInvoice";
        this.DataContext = this;
        this.IsVisible = false;
        this.CustomFilterOperation = "";
        this.CustomFilterValue = "";
        this.ChildEntityId = "";
    }
    ARInvoiceDocsOutTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(function (d) { return d.Name == _this.ObjectTableName; })[0];
            this.ChildEntityId = this.EntityPM.Id;
            if (table)
                this.ObjectTableId = table.Id;
            if (this.EntityPM.IsConsolidationInvoice) {
                this.IsVisible = true;
                if (table)
                    this.ObjectTableId = table.Id;
                this.EntityId = this.EntityPM.Id;
                var myReference = null;
                if (this.EntityPM.InvoiceNumber == null) {
                    myReference = "Draft: " + this.EntityPM.DraftNumber;
                }
                else {
                    myReference = this.EntityPM.InvoiceNumber;
                }
                this.EntityReference = myReference;
                this.CustomFilterOperation = "Equal";
                this.CustomFilterValue = "999C";
            }
            else if (this.EntityPM.IsGeneralInvoice) {
                this.IsVisible = true;
                if (table)
                    this.ObjectTableId = table.Id;
                this.EntityId = this.EntityPM.Id;
                var myReference = null;
                if (this.EntityPM.InvoiceNumber == null) {
                    myReference = "Draft: " + this.EntityPM.DraftNumber;
                }
                else {
                    myReference = this.EntityPM.InvoiceNumber;
                }
                this.EntityReference = myReference;
                this.CustomFilterOperation = "Equal";
                this.CustomFilterValue = "999G";
            }
            else if (this.EntityPM.StatusCode != "VD") {
                this.IsVisible = true;
                var entityObjectTable = null;
                if (this.EntityPM.InvoiceEntities.length > 1) {
                    entityObjectTable = window.ObjectTables.filter(function (d) { return d.Name == "Master"; })[0];
                    this.currentInvoiceEntity = this.EntityPM.InvoiceEntities.filter(function (a) { return a.ObjectTableId == entityObjectTable.Id; })[0];
                }
                else if (this.EntityPM.InvoiceEntities.length == 1) {
                    entityObjectTable = window.ObjectTables.filter(function (d) { return d.Name == "Shipment"; })[0];
                    this.currentInvoiceEntity = this.EntityPM.InvoiceEntities.filter(function (a) { return a.ObjectTableId == entityObjectTable.Id; })[0];
                    if (this.currentInvoiceEntity == null) {
                        entityObjectTable = window.ObjectTables.filter(function (d) { return d.Name == "Master"; })[0];
                        this.currentInvoiceEntity = this.EntityPM.InvoiceEntities.filter(function (a) { return a.ObjectTableId == entityObjectTable.Id; })[0];
                    }
                }
                if (entityObjectTable != null) {
                    this.ObjectTableId = entityObjectTable.Id;
                }
                var invoiceEntityId = "";
                if (this.currentInvoiceEntity != null) {
                    invoiceEntityId = this.currentInvoiceEntity.EntityId;
                }
                else {
                    invoiceEntityId = this.EntityPM.MainEntityId;
                }
                this.EntityId = invoiceEntityId;
                if (table)
                    this.ChildObjectTableId = table.Id;
                this.CustomFilterOperation = "NotEqual";
                this.CustomFilterValue = "999C";
            }
        }
    };
    ARInvoiceDocsOutTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARInvoiceDocsOutTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ARInvoiceDocsOutTabComponent);
    return ARInvoiceDocsOutTabComponent;
}());
exports.ARInvoiceDocsOutTabComponent = ARInvoiceDocsOutTabComponent;
//# sourceMappingURL=ARInvoiceDocsOutTabComponent.js.map