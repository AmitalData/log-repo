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
var ARInvoiceStockPM_1 = require("../../../../Invoice/EntityPMs/ARInvoiceStockPM");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Args_1 = require("../../../../Invoice/Args");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ARInvoiceStockGeneralTabComponent = /** @class */ (function () {
    function ARInvoiceStockGeneralTabComponent(entityArgs, CD) {
        this.entityArgs = entityArgs;
        this.CD = CD;
        this.EntityPM = new ARInvoiceStockPM_1.ARInvoiceStockPM();
        this.ObjectTableName = "ARInvoiceStock";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.Retries = 0;
        this.EntityPM = entityArgs.EntityPM;
        this.RunComponent();
        this.Listen();
    }
    ARInvoiceStockGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.StockInputTemplate.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.StockInputTemplate.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
        }
    };
    ARInvoiceStockGeneralTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    ARInvoiceStockGeneralTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    ARInvoiceStockGeneralTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./InvoiceModules/InvoiceStocks/Components/ARInvoiceStockInputTemplate", this.viewContainerRef)
            .then(function (cmpRef) {
            _this.StockInputTemplate = cmpRef.instance;
            var args = new Args_1.InvoiceStockInputArgs();
            args.Stock = _this.EntityPM;
            args.IsEditMode = true;
            _this.StockInputTemplate.InitTemplate(args);
        });
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ARInvoiceStockGeneralTabComponent.prototype, "viewContainerRef", void 0);
    ARInvoiceStockGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARInvoiceStockGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], ARInvoiceStockGeneralTabComponent);
    return ARInvoiceStockGeneralTabComponent;
}());
exports.ARInvoiceStockGeneralTabComponent = ARInvoiceStockGeneralTabComponent;
//# sourceMappingURL=ARInvoiceStockGeneralTabComponent.js.map