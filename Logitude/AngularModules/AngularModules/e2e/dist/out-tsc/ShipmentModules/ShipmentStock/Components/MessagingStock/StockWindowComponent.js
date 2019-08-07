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
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Tools_1 = require("../../../../Infrastructure/Tools");
var StockWindowComponent = /** @class */ (function () {
    function StockWindowComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SelectedFilter = "ALL";
        this.ItemsSource = [];
        this.LoadData();
    }
    StockWindowComponent.prototype.LoadData = function () {
        var _this = this;
        this.DataSource = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        if (this.myDomainService == null) {
            this.myDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        }
        this.myDomainService.GetLoggedTenantMessagingStockLists().subscribe(function (myResult) {
            _this.DataSource = myResult;
            _this.BuildItemsSource();
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    StockWindowComponent.prototype.FilterChanged = function (filterCode) {
        this.SelectedFilter = filterCode;
        this.BuildItemsSource();
    };
    StockWindowComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        if (this.DataSource != null) {
            if (this.SelectedFilter == "ALL") {
                this.DataSource.forEach(function (item) {
                    _this.ItemsSource.push(new MessagingStockListItem(item));
                });
            }
            else if (this.SelectedFilter == "ACT") {
                this.DataSource.filter(function (f) { return f.Status == "New" || f.Status == "Active"; }).forEach(function (item) {
                    _this.ItemsSource.push(new MessagingStockListItem(item));
                });
            }
            else if (this.SelectedFilter == "INA") {
                this.DataSource.filter(function (f) { return f.Status != "New" && f.Status != "Active"; }).forEach(function (item) {
                    _this.ItemsSource.push(new MessagingStockListItem(item));
                });
            }
        }
    };
    StockWindowComponent.prototype.ViewHistory = function (item) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Messaging Stock History";
        logWindow.WindowArgs = item.Id;
        logWindow.Show("./ShipmentModules/ShipmentStock/Components/MessagingStock/StockHistoryComponent");
    };
    StockWindowComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    StockWindowComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './StockWindowComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], StockWindowComponent);
    return StockWindowComponent;
}());
exports.StockWindowComponent = StockWindowComponent;
var MessagingStockListItem = /** @class */ (function () {
    function MessagingStockListItem(entity) {
        this.entity = entity;
        this.SetForegrounds();
    }
    Object.defineProperty(MessagingStockListItem.prototype, "Id", {
        get: function () { return this.entity.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MessagingStockListItem.prototype, "StartDate", {
        get: function () { return this.entity.StartDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MessagingStockListItem.prototype, "EndDate", {
        get: function () { return this.entity.EndDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MessagingStockListItem.prototype, "Amount", {
        get: function () { return this.entity.Amount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MessagingStockListItem.prototype, "Remaining", {
        get: function () { return this.entity.Remaining; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MessagingStockListItem.prototype, "Status", {
        get: function () { return this.entity.Status; },
        enumerable: true,
        configurable: true
    });
    MessagingStockListItem.prototype.SetForegrounds = function () {
        var _black = Tools_1.FontTool.Black;
        var _green = Tools_1.FontTool.Green;
        var _red = Tools_1.FontTool.Red;
        var endDateForeground = _black;
        var remainingForeground = _black;
        var statusForeground = _black;
        if (this.Status == "New" || this.Status == "Active") {
            statusForeground = _green;
        }
        if (this.Status == "Cancelled" || this.Status == "Used" || this.Status == "Expired") {
            endDateForeground = _black;
            remainingForeground = _black;
        }
        else {
            if (this.Remaining < 10) {
                remainingForeground = _red;
            }
            var isRedColor = true;
            if (this.EndDate != null) {
                var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                if (this.EndDate.valueOf() > todayDate.valueOf()) {
                    var myTotalDays = Tools_1.DateTool.GetDaysBetweenDates(this.EndDate, todayDate);
                    if (myTotalDays > 14) {
                        isRedColor = false;
                    }
                }
            }
            if (isRedColor) {
                endDateForeground = _red;
            }
        }
        this.EndDateForeground = endDateForeground;
        this.RemainingForeground = remainingForeground;
        this.StatusForeground = statusForeground;
    };
    return MessagingStockListItem;
}());
//# sourceMappingURL=StockWindowComponent.js.map