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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var AWBStackDomainService_1 = require("../../../../Common/Services/AWBStackDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var CustomerAWBStockTabComponent = /** @class */ (function () {
    function CustomerAWBStockTabComponent(entityArgs) {
        var _this = this;
        this.entityArgs = entityArgs;
        this.ObjectTableName = "Customer";
        this.ItemsCount = 0;
        this.ItemsSource = [];
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.IsVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._entityResourceService.getEntityResourceByTableName("MAWBStack").subscribe(function (response) {
            _this.IsVisible = true;
            _this.EntityPM = entityArgs.EntityPM;
            _this.StackDomainService = new AWBStackDomainService_1.AWBStackDomainService();
            _this.LoadData();
        });
    }
    CustomerAWBStockTabComponent.prototype.LoadData = function () {
        var _this = this;
        this.ItemsCount = 0;
        this.ItemsSource = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.StackDomainService.GetCustomerStockSeries(this.EntityPM.Id).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myClass = myResponse.Result;
                    myClass.StockSeriesList.forEach(function (item) {
                        _this.ItemsSource.push(item);
                        _this.ItemsCount += item.Total;
                    });
                }
            }
        });
    };
    CustomerAWBStockTabComponent.prototype.AddStockClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("MAWBStack.O.NewAirWayBillNumbers");
        logWindow.WindowArgs = { CustomerId: this.EntityPM.Id, IsCustomerMode: true };
        logWindow.Show('./Common/Components/Partners/AWBStock/NewStackComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s == "OK") {
                _this.LoadData();
            }
        });
    };
    CustomerAWBStockTabComponent.prototype.AssignClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Assign to shipper";
        logWindow.WindowArgs = { CustomerId: this.EntityPM.Id, IsCustomerMode: true };
        logWindow.Show('./Common/Components/Partners/AWBStock/AssignComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s == "OK") {
                _this.LoadData();
            }
        });
    };
    CustomerAWBStockTabComponent.prototype.UnassignClicked = function (item) {
        var _this = this;
        if (item != null) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.Title = "Unassign Series";
            confirmWindow.YesButtonText = "Unassign";
            confirmWindow.NoButtonText = "Cancel";
            confirmWindow.Show("Are you sure you want to Unassign this series?");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.CurrentSession.StartBusyIndicatorSaving();
                    _this.StackDomainService.UnAssignStockSeriesToUser(item.From, item.To, item.AirlineId).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (myResponse != null) {
                            if (!myResponse.HasError) {
                                _this.LoadData();
                            }
                        }
                    });
                }
            });
        }
    };
    CustomerAWBStockTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomerAWBStockTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CustomerAWBStockTabComponent);
    return CustomerAWBStockTabComponent;
}());
exports.CustomerAWBStockTabComponent = CustomerAWBStockTabComponent;
//# sourceMappingURL=CustomerAWBStockTabComponent.js.map