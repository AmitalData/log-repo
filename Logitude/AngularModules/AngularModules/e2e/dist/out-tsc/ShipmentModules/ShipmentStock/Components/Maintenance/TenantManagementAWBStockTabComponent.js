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
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TenantManagementAWBStockTabComponent = /** @class */ (function () {
    function TenantManagementAWBStockTabComponent(entityArgs, entityResourceService) {
        this.entityArgs = entityArgs;
        this.entityResourceService = entityResourceService;
        this.ObjectTableName = "TenantManagement";
        this.IsEditingAllowed = false;
        this.EntityPM = this.entityArgs.EntityPM;
    }
    TenantManagementAWBStockTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.EntityPM != null) {
            this.entityResourceService.getEntityResourceByTableName("MessagingStock").subscribe(function (res1) {
                _this.IsEditingAllowed = _this.IsTenantManagementEditable();
                _this.BuilItemsSource();
            });
        }
    };
    TenantManagementAWBStockTabComponent.prototype.IsTenantManagementEditable = function () {
        var myResult = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("TenantManagement", "EnableTenantManagementEdit")) {
            myResult = true;
        }
        return myResult;
    };
    TenantManagementAWBStockTabComponent.prototype.BuilItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        var service = new ShipmentDomainService_1.ShipmentDomainService();
        service.GetMessagingStockListForTenantManagmentTab(this.EntityPM.Id).subscribe(function (result) {
            var allStocks = result.Result;
            allStocks.sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.StartDate) === Tools_1.DateTool.GetDateFromDate(b.StartDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.StartDate) > Tools_1.DateTool.GetDateFromDate(b.StartDate)) ? -1 : 1; }).forEach(function (item) {
                _this.ItemsSource.push(item);
            });
        });
    };
    TenantManagementAWBStockTabComponent.prototype.AddStockClicked = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = "New Messaging Stock";
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 600;
        var args = new StockArgs();
        args.IsNewEntity = true;
        args.FatherComponent = this;
        logitudeWindow.WindowArgs = args;
        logitudeWindow.Show('./ShipmentModules/ShipmentStock/Components/Maintenance/AddEditAWBStockComponent');
    };
    TenantManagementAWBStockTabComponent.prototype.EditStockClicked = function (item) {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = "Edit Messaging Stock";
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 600;
        var args = new StockArgs();
        args.IsNewEntity = false;
        args.FatherComponent = this;
        args.EntityId = item.Id;
        logitudeWindow.WindowArgs = args;
        logitudeWindow.Show('./ShipmentModules/ShipmentStock/Components/Maintenance/AddEditAWBStockComponent');
    };
    TenantManagementAWBStockTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TenantManagementAWBStockTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], TenantManagementAWBStockTabComponent);
    return TenantManagementAWBStockTabComponent;
}());
exports.TenantManagementAWBStockTabComponent = TenantManagementAWBStockTabComponent;
var StockArgs = /** @class */ (function () {
    function StockArgs() {
    }
    return StockArgs;
}());
exports.StockArgs = StockArgs;
//# sourceMappingURL=TenantManagementAWBStockTabComponent.js.map