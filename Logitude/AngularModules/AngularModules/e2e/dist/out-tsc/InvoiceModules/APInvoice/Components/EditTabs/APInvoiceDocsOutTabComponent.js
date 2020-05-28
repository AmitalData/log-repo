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
var APInvoiceDocsOutTabComponent = /** @class */ (function () {
    function APInvoiceDocsOutTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = null;
        this.ObjectTableName = "APInvoice";
        this.DataContext = this;
        this.CustomFilterValue = "";
        this.CustomFilterOperation = "";
        this.EntityPM = entityArgs.EntityPM;
    }
    APInvoiceDocsOutTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        var invoiceObjectTable = window.ObjectTables.filter(function (d) { return d.Name == _this.ObjectTableName; })[0];
        if (this.EntityPM.IsMultipleEntities) {
            this.EntityId = this.EntityPM.Id;
            this.EntityReference = this.EntityPM.InvoiceNumber;
            this.EntityObjectTableId = invoiceObjectTable.Id;
            this.CustomFilterOperation = "Equal";
            this.CustomFilterValue = "999MP";
        }
        else {
            this.EntityId = this.EntityPM.MainEntityId;
            this.EntityReference = this.EntityPM.MainEntityReference;
            var invoiceEntitiy = this.EntityPM.InvoiceEntities.filter(function (a) { return a.EntityId == _this.EntityPM.MainEntityId; })[0];
            if (invoiceEntitiy) {
                this.EntityObjectTableId = invoiceEntitiy.ObjectTableId;
            }
            this.ChildEntityId = this.EntityPM.Id;
            this.ChildEntityReference = this.EntityPM.InvoiceNumber;
            this.ChildEntityObjectTableId = invoiceObjectTable.Id;
            this.CustomFilterOperation = "NotEqual";
            this.CustomFilterValue = "999MP";
        }
    };
    APInvoiceDocsOutTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './APInvoiceDocsOutTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], APInvoiceDocsOutTabComponent);
    return APInvoiceDocsOutTabComponent;
}());
exports.APInvoiceDocsOutTabComponent = APInvoiceDocsOutTabComponent;
//# sourceMappingURL=APInvoiceDocsOutTabComponent.js.map