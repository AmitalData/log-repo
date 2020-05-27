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
var ARInvoiceStockPMInitService_1 = require("../../../../Invoice/EntityPMInitServices/ARInvoiceStockPMInitService");
var ARInvoiceStockPMService_1 = require("../../../../Invoice/Services/StandardPMs/ARInvoiceStockPMService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var Args_1 = require("../../../../Invoice/Args");
var NewARInvoiceStockComponent = /** @class */ (function () {
    function NewARInvoiceStockComponent() {
        this.DataContext = this;
        this.ObjectTableName = "ARInvoiceStock";
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.Retries = 0;
        this.RunComponent();
        this.stockPMService = new ARInvoiceStockPMService_1.ARInvoiceStockPMService();
    }
    NewARInvoiceStockComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewARInvoiceStockComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewARInvoiceStockComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./InvoiceModules/InvoiceStocks/Components/ARInvoiceStockInputTemplate", this.viewContainerRef)
            .then(function (cmpRef) {
            _this.StockInputTemplate = cmpRef.instance;
            var args = new Args_1.InvoiceStockInputArgs();
            args.Stock = _this.EntityPM;
            args.IsEditMode = false;
            _this.StockInputTemplate.InitTemplate(args);
        });
    };
    NewARInvoiceStockComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            if (args.Stock != null) {
                this.EntityPM = args.Stock;
            }
            else {
                this.EntityPM = new ARInvoiceStockPM_1.ARInvoiceStockPM();
                ARInvoiceStockPMInitService_1.ARInvoiceStockPMInitService.InitValues(this.EntityPM, true);
            }
        }
        else {
            this.EntityPM = new ARInvoiceStockPM_1.ARInvoiceStockPM();
            ARInvoiceStockPMInitService_1.ARInvoiceStockPMInitService.InitValues(this.EntityPM, true);
        }
        this.Clone();
    };
    NewARInvoiceStockComponent.prototype.CancelClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    NewARInvoiceStockComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('StartDate');
        this.myCloner.AddField('EndDate');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(this.EntityPM);
    };
    NewARInvoiceStockComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    NewARInvoiceStockComponent.prototype.OkClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.EntityPM.StartDate > this.EntityPM.EndDate) {
            errors.push("Start Date cannot be greater than End Date");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.EntityPM.IsDirty) {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.stockPMService.insert(this.EntityPM).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        _this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                    else {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                });
            }
            else {
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], NewARInvoiceStockComponent.prototype, "viewContainerRef", void 0);
    NewARInvoiceStockComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewARInvoiceStockComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewARInvoiceStockComponent);
    return NewARInvoiceStockComponent;
}());
exports.NewARInvoiceStockComponent = NewARInvoiceStockComponent;
//# sourceMappingURL=NewARInvoiceStockComponent.js.map