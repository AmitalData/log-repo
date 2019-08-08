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
var InvoiceDomainService_1 = require("../../../Services/InvoiceDomainService");
var RecalculateExternalsComponent = /** @class */ (function () {
    function RecalculateExternalsComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.entityCode = null;
        this.stepCount = 10;
        this.sentCount = 0;
        this.allEntitiesCount = 0;
        this.entitiesIdsList = [];
        this.CountText = "";
        this.RecalculateButtonIsEnabled = false;
        this.isRecalculateButtonClicked = false;
        this.invoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
    }
    RecalculateExternalsComponent.prototype.SetWindowArgs = function (args) {
        this.entityCode = args;
        this.LoadData();
    };
    RecalculateExternalsComponent.prototype.LoadData = function () {
        var _this = this;
        this.sentCount = 0;
        this.entitiesIdsList = [];
        if (this.entityCode == "ARInvoice") {
            this.invoiceDomainService.GetNotReadyARInvoicesIds().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.OnLoadDataCompleted(myResponse.Result, " Invoices");
                }
            });
        }
        else if (this.entityCode == "APInvoice") {
            this.invoiceDomainService.GetNotReadyAPInvoicesIds().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.OnLoadDataCompleted(myResponse.Result, " Invoices");
                }
            });
        }
        else if (this.entityCode == "ARPayment") {
            this.invoiceDomainService.GetNotReadyARPaymentsIds().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.OnLoadDataCompleted(myResponse.Result, " Payments");
                }
            });
        }
        else if (this.entityCode == "APPayment") {
            this.invoiceDomainService.GetNotReadyAPPaymentsIds().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.OnLoadDataCompleted(myResponse.Result, " Payments");
                }
            });
        }
    };
    RecalculateExternalsComponent.prototype.OnLoadDataCompleted = function (ids, text) {
        this.entitiesIdsList = ids;
        this.allEntitiesCount = this.entitiesIdsList.length;
        this.CountText = this.allEntitiesCount + text;
        this.RecalculateButtonIsEnabled = this.entitiesIdsList.length > 0 ? true : false;
    };
    RecalculateExternalsComponent.prototype.RecalculateClicked = function () {
        if (!this.isRecalculateButtonClicked) {
            this.Recalculate();
            this.isRecalculateButtonClicked = true;
        }
    };
    RecalculateExternalsComponent.prototype.Recalculate = function () {
        var _this = this;
        if (this.entitiesIdsList.length == 0) {
            this.isRecalculateButtonClicked = false;
            this.CurrentSession.StopBusyIndicator();
            this.LoadData();
            this.CurrentSession.FireEvent("Accounting_T");
        }
        else {
            var idsList = [];
            this.entitiesIdsList.forEach(function (id) {
                if (idsList.length < _this.stepCount) {
                    idsList.push(id);
                }
            });
            idsList.forEach(function (id) {
                var indexOfId = _this.entitiesIdsList.indexOf(id);
                if (indexOfId > -1) {
                    _this.entitiesIdsList.splice(indexOfId, 1);
                }
            });
            this.sentCount = this.sentCount + idsList.length;
            this.CurrentSession.StartBusyIndicator("Recalculating " + this.sentCount + " from " + this.allEntitiesCount);
            this.invoiceDomainService.GetRecalculateTransfer(idsList, this.entityCode).subscribe(function (myResponse) {
                _this.Recalculate();
            });
        }
    };
    RecalculateExternalsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    RecalculateExternalsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './RecalculateExternalsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], RecalculateExternalsComponent);
    return RecalculateExternalsComponent;
}());
exports.RecalculateExternalsComponent = RecalculateExternalsComponent;
//# sourceMappingURL=RecalculateExternalsComponent.js.map