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
var AWBStackDomainService_1 = require("../../../Services/AWBStackDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var AssignComponent = /** @class */ (function () {
    function AssignComponent() {
        this.AirlineId = null;
        this.CustomerId = null;
        this.IsCustomerMode = false;
        this.ItemsCount = 0;
        this.ItemsSource = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isReloadingData = false;
        this.StackDomainService = new AWBStackDomainService_1.AWBStackDomainService();
    }
    AssignComponent.prototype.SetWindowArgs = function (args) {
        this.AirlineId = args['AirlineId'];
        this.CustomerId = args['CustomerId'];
        this.IsCustomerMode = args['IsCustomerMode'];
        this.LoadData();
    };
    AssignComponent.prototype.LoadData = function () {
        var _this = this;
        this.ItemsCount = 0;
        this.ItemsSource = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        if (this.IsCustomerMode) {
            this.StackDomainService.GetAllAvailableStockSeries().subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var myClass = myResponse.Result;
                    myClass.StockSeriesList.forEach(function (item) {
                        _this.ItemsSource.push(item);
                        _this.ItemsCount += item.Total;
                    });
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
        else {
            this.StackDomainService.GetAirlineAvailableStockSeries(this.AirlineId).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var myClass = myResponse.Result;
                    myClass.StockSeriesList.forEach(function (item) {
                        _this.ItemsSource.push(item);
                        _this.ItemsCount += item.Total;
                    });
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    AssignComponent.prototype.AssignClicked = function (item) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Assign to shipper";
        logWindow.Width = 700;
        logWindow.Height = 450;
        logWindow.WindowArgs = { ShipperId: this.CustomerId, IsCustomerMode: this.IsCustomerMode, StockSeriesItem: item };
        logWindow.Show('./Common/Components/Partners/AWBStock/AssignToShipperComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s == "OK") {
                _this.LoadData();
                _this.isReloadingData = true;
            }
        });
    };
    AssignComponent.prototype.CloseClicked = function () {
        if (this.isReloadingData) {
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    AssignComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AssignComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AssignComponent);
    return AssignComponent;
}());
exports.AssignComponent = AssignComponent;
//# sourceMappingURL=AssignComponent.js.map