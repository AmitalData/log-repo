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
var Tools_1 = require("../../../../Infrastructure/Tools");
var ARInvoiceDocsInTabComponent = /** @class */ (function () {
    function ARInvoiceDocsInTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = null;
        this.ObjectTableName = "ARInvoice";
        this.DataContext = this;
        this.IsVisible = false;
    }
    ARInvoiceDocsInTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            if (this.EntityPM.StatusCode != "VD") {
                this.IsVisible = true;
            }
            var table = window.ObjectTables.filter(function (d) { return d.Name == _this.ObjectTableName; })[0];
            var entityObjectTable = null;
            if (this.EntityPM.InvoiceEntities.length > 1) {
                entityObjectTable = window.ObjectTables.filter(function (d) { return d.Name == "Master" && (d.Tenant == SessionLocator_1.SessionLocator.TenantPM.Id || d.Tenant == 0); })[0];
                this.currentInvoiceEntity = this.EntityPM.InvoiceEntities.filter(function (a) { return a.ObjectTableId == entityObjectTable.Id; })[0];
            }
            else if (this.EntityPM.InvoiceEntities.length == 1) {
                entityObjectTable = window.ObjectTables.filter(function (d) { return d.Name == "Shipment" && (d.Tenant == SessionLocator_1.SessionLocator.TenantPM.Id || d.Tenant == 0); })[0];
                this.currentInvoiceEntity = this.EntityPM.InvoiceEntities.filter(function (a) { return a.ObjectTableId == entityObjectTable.Id; })[0];
                if (this.currentInvoiceEntity == null) {
                    entityObjectTable = window.ObjectTables.filter(function (d) { return d.Name == "Master" && (d.Tenant == SessionLocator_1.SessionLocator.TenantPM.Id || d.Tenant == 0); })[0];
                    this.currentInvoiceEntity = this.EntityPM.InvoiceEntities.filter(function (a) { return a.ObjectTableId == entityObjectTable.Id; })[0];
                }
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.ObjectTableId)) {
                var ObjectTable = window.ObjectTables.filter(function (d) { return d.Name == "ARInvoice" && (d.Tenant == SessionLocator_1.SessionLocator.TenantPM.Id || d.Tenant == 0); })[0];
                if (ObjectTable != null) {
                    this.ObjectTableId = ObjectTable.Id;
                }
            }
            if (entityObjectTable != null) {
                this.ObjectTableId = entityObjectTable.Id;
            }
            var invoiceEntityId = "";
            if (this.currentInvoiceEntity != null) {
                invoiceEntityId = this.currentInvoiceEntity.EntityId;
            }
            this.EntityId = invoiceEntityId;
            if (table)
                this.ChildObjectTableId = table.Id;
            this.ChildEntityId = this.EntityPM.Id;
            this.ChildEntityReference = this.EntityPM.InvoiceNumber;
        }
    };
    ARInvoiceDocsInTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARInvoiceDocsInTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ARInvoiceDocsInTabComponent);
    return ARInvoiceDocsInTabComponent;
}());
exports.ARInvoiceDocsInTabComponent = ARInvoiceDocsInTabComponent;
//# sourceMappingURL=ARInvoiceDocsInTabComponent.js.map