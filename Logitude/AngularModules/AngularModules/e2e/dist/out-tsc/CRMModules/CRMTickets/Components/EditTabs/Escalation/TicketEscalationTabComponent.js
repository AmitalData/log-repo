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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var CRMDomainService_1 = require("../../../../../CRM/Services/CRMDomainService");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var TicketEscalationTabComponent = /** @class */ (function () {
    function TicketEscalationTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = null;
        this.ObjectTableName = "Ticket";
        this.EntityPM = entityArgs.EntityPM;
        this.TicketEscalationsList = new ObservableCollection_1.ObservableCollection([]);
        this.GetEscalationsList();
    }
    TicketEscalationTabComponent.prototype.GetEscalationsList = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetTicketEscalationListsByTicketId(this.EntityPM.Id).subscribe(function (resp) {
            if (resp != null && !resp.HasError) {
                //this.TicketEscalationsList = [];
                _this.TicketEscalationsList.InsertCollection(resp.Result.sort(function (a, b) { return (Tools_1.DateTool.GetDateParts(a.DueDate) === Tools_1.DateTool.GetDateParts(b.DueDate)) ? 0 : (Tools_1.DateTool.GetDateParts(a.DueDate) > Tools_1.DateTool.GetDateParts(b.DueDate)) ? 1 : -1; }));
            }
        });
    };
    TicketEscalationTabComponent = __decorate([
        core_1.Component({
            selector: 'TicketEscalationTabComponent',
            moduleId: module.id,
            templateUrl: './TicketEscalationTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], TicketEscalationTabComponent);
    return TicketEscalationTabComponent;
}());
exports.TicketEscalationTabComponent = TicketEscalationTabComponent;
//# sourceMappingURL=TicketEscalationTabComponent.js.map