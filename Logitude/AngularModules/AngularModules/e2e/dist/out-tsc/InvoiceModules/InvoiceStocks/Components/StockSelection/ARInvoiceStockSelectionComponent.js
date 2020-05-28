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
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var InvoiceDomainService_1 = require("../../../../Invoice/Services/InvoiceDomainService");
var ARInvoiceStockSelectionComponent = /** @class */ (function () {
    function ARInvoiceStockSelectionComponent() {
        var _this = this;
        this.ObjectTableName = "ARInvoiceStock";
        this.StocksHeaderList = [];
        this.StockLines = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.invoiceStockSelectedItem = null;
        this.stockLineSelectedItem = null;
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("ARInvoiceStockLine", 0).subscribe(function (response) {
                _this.InitializeServices();
                _this.LoadData();
            });
        });
    }
    ARInvoiceStockSelectionComponent.prototype.InitializeServices = function () {
        this.InvoiceDomainService = new InvoiceDomainService_1.InvoiceDomainService();
    };
    ARInvoiceStockSelectionComponent.prototype.LoadData = function () {
        var _this = this;
        this.StocksHeaderList = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.InvoiceDomainService.GetListOfARInvoiceStockPM().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var stockList = myResponse.Result;
                stockList.forEach(function (item) {
                    _this.StocksHeaderList.push(new StockHeaderData(item));
                });
                _this.InvoiceStockSelectedItem = _this.StocksHeaderList != null ? _this.StocksHeaderList[0] : null;
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    Object.defineProperty(ARInvoiceStockSelectionComponent.prototype, "InvoiceStockSelectedItem", {
        get: function () {
            return this.invoiceStockSelectedItem;
        },
        set: function (value) {
            if (this.invoiceStockSelectedItem != value) {
                this.invoiceStockSelectedItem = value;
                this.FillStockLines(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceStockSelectionComponent.prototype.FillStockLines = function (stock) {
        var _this = this;
        if (stock != null) {
            this.StockLines = [];
            this.StockLineSelectedItem = null;
            stock.StockHeader.ARInvoiceStockLines.forEach(function (item) {
                _this.StockLines.push(item);
            });
            this.StocksLineCount = this.StockLines.length;
        }
    };
    Object.defineProperty(ARInvoiceStockSelectionComponent.prototype, "StockLineSelectedItem", {
        get: function () {
            return this.stockLineSelectedItem;
        },
        set: function (value) {
            if (this.stockLineSelectedItem != value) {
                this.stockLineSelectedItem = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ARInvoiceStockSelectionComponent.prototype.EditStockClicked = function (item) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Edit AR Invoice Stock";
        logWindow.IsFillScreen = true;
        logWindow.ShowEditComponent(item.StockHeader.Id, "ARInvoiceStock");
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                _this.LoadData();
            });
        });
    };
    ARInvoiceStockSelectionComponent.prototype.NewStockClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "New Invoice Stock";
        logWindow.WindowArgs = { IsNew: true, EntityPM: null };
        logWindow.Show('./InvoiceModules/InvoiceStocks/Components/NewEntity/NewARInvoiceStockComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s == "OK") {
                _this.LoadData();
            }
        });
    };
    ARInvoiceStockSelectionComponent.prototype.CancelClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    ARInvoiceStockSelectionComponent.prototype.OkClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    };
    ARInvoiceStockSelectionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARInvoiceStockSelectionComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ARInvoiceStockSelectionComponent);
    return ARInvoiceStockSelectionComponent;
}());
exports.ARInvoiceStockSelectionComponent = ARInvoiceStockSelectionComponent;
var StockHeaderData = /** @class */ (function () {
    function StockHeaderData(entity) {
        this.StockHeader = entity;
    }
    Object.defineProperty(StockHeaderData.prototype, "LinesCount", {
        get: function () { return this.StockHeader.LinesCount; },
        set: function (newValue) {
            if (this.StockHeader.LinesCount != newValue) {
                this.StockHeader.LinesCount = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockHeaderData.prototype, "Name", {
        get: function () { return this.StockHeader.Name; },
        set: function (newValue) {
            if (this.StockHeader.Name != newValue) {
                this.StockHeader.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockHeaderData.prototype, "EndDate", {
        get: function () { return this.StockHeader.EndDate; },
        set: function (newValue) {
            if (this.StockHeader.EndDate != newValue) {
                this.StockHeader.EndDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockHeaderData.prototype, "Description", {
        get: function () { return this.StockHeader.Description; },
        set: function (newValue) {
            if (this.StockHeader.Description != newValue) {
                this.StockHeader.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    return StockHeaderData;
}());
exports.StockHeaderData = StockHeaderData;
//# sourceMappingURL=ARInvoiceStockSelectionComponent.js.map