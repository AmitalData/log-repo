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
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var StockHistoryComponent = /** @class */ (function () {
    function StockHistoryComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ItemsSource = [];
    }
    StockHistoryComponent.prototype.SetWindowArgs = function (stockId) {
        if (stockId != null) {
            this.LoadData(stockId);
        }
    };
    StockHistoryComponent.prototype.LoadData = function (stockId) {
        var _this = this;
        this.ItemsSource = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        if (this.myDomainService == null) {
            this.myDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        }
        this.myDomainService.GetLoggedTenantMessagingStockUsageHistoryLists(stockId).subscribe(function (myResult) {
            _this.ItemsSource = myResult;
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    StockHistoryComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    StockHistoryComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './StockHistoryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], StockHistoryComponent);
    return StockHistoryComponent;
}());
exports.StockHistoryComponent = StockHistoryComponent;
//# sourceMappingURL=StockHistoryComponent.js.map