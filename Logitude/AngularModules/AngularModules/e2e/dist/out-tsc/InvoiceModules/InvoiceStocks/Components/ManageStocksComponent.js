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
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ARInvoiceStockListService_1 = require("../../../Invoice/Services/StandardLists/ARInvoiceStockListService");
var ManageStocksComponent = /** @class */ (function () {
    function ManageStocksComponent() {
        var _this = this;
        this.ObjectTableName = "ARInvoiceStock";
        this.ItemsSource = [];
        this.IsVisibile = false;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("ARInvoiceStockLine", 0).subscribe(function (response) {
                _this.IsVisibile = true;
                _this.ARInvoiceStockListService = new ARInvoiceStockListService_1.ARInvoiceStockListService();
                _this.LoadData();
            });
        });
    }
    ManageStocksComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ARInvoiceStockListService.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.BuildItemsSource(myResponse.Result);
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    ManageStocksComponent.prototype.BuildItemsSource = function (items) {
        var itemsSource = [];
        if (items != null) {
            itemsSource = items;
        }
        this.ItemsSource = itemsSource;
    };
    ManageStocksComponent.prototype.EditStock = function (item) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Edit AR Invoice Stock";
        logWindow.IsFillScreen = true;
        logWindow.ShowEditComponent(item.Id, "ARInvoiceStock");
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                _this.LoadData();
            });
        });
    };
    ManageStocksComponent.prototype.NewStockClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "New Invoice Stock";
        logWindow.Width = 900;
        logWindow.Height = 600;
        logWindow.WindowArgs = { IsNew: true, EntityPM: null };
        logWindow.Show('./InvoiceModules/InvoiceStocks/Components/NewEntity/NewARInvoiceStockComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s == "OK") {
                _this.LoadData();
            }
        });
    };
    ManageStocksComponent.prototype.CloseClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ManageStocksComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ManageStocksComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ManageStocksComponent);
    return ManageStocksComponent;
}());
exports.ManageStocksComponent = ManageStocksComponent;
//# sourceMappingURL=ManageStocksComponent.js.map