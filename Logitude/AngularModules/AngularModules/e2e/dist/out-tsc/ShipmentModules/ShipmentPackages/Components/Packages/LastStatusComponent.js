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
var INTRAWebService_1 = require("../../../../Shipment/Services/INTRAWebService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var LastStatusComponent = /** @class */ (function () {
    function LastStatusComponent() {
        this.ShipmentId = null;
        this.ContainerId = null;
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
    }
    LastStatusComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.ShipmentId = args["ShipmentId"];
        this.ContainerId = args["ContainerId"];
        var myService = new INTRAWebService_1.INTRAWebService();
        myService.GetContainerStatuses(this.ShipmentId, this.ContainerId).subscribe(function (myResponse) {
            var itemsSource = [];
            var itemsCollection = [];
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                var items = myResponse.Result;
                items.forEach(function (item) {
                    itemsSource.push(new LastStatusItem(item));
                });
                itemsSource.sort(function (a, b) { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1; }).forEach(function (item) {
                    itemsCollection.push(item);
                });
                _this.ItemsSource.InsertCollection(itemsCollection);
            }
        });
    };
    LastStatusComponent.prototype.CloseClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    LastStatusComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './LastStatusComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], LastStatusComponent);
    return LastStatusComponent;
}());
exports.LastStatusComponent = LastStatusComponent;
var LastStatusItem = /** @class */ (function () {
    function LastStatusItem(item) {
        this.SortingValue = 0;
        if (item) {
            this.StatusName = item.StatusName;
            this.EventDate = item.EventDate;
            this.ReceivingDate = item.ReceivingDate;
            this.LocationCode = item.LocationCode;
            this.DepartureDate = item.DepartureDate;
            this.ArrivalDate = item.ArrivalDate;
            this.VesselName = item.VesselName;
            this.VoyageNumber = item.VoyageNumber;
            if (this.DepartureDate && item.TimeOfDepartureInfo) {
                this.DepartureDateInfo = item.TimeOfDepartureInfo == "E" ? "(expected)" : "(actual)";
            }
            if (this.ArrivalDate && item.TimeOfArrivalInfo) {
                this.ArrivalDateInfo = item.TimeOfArrivalInfo == "E" ? "(expected)" : "(actual)";
            }
            if (this.EventDate) {
                this.SortingValue = Tools_1.DateTool.GetDateParts(this.EventDate).DateTicks;
            }
        }
    }
    return LastStatusItem;
}());
//# sourceMappingURL=LastStatusComponent.js.map