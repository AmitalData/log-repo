"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var VehicleExtendedListService_1 = require("../../../../../../Customs/Services/ExtendedLists/VehicleExtendedListService");
var BaseComponent_1 = require("../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityResourceService_1 = require("../../../../../../Infrastructure/Services/EntityResourceService");
var ObservableCollection_1 = require("../../../../../../Infrastructure/Utilities/ObservableCollection");
var SessionLocator_1 = require("../../../../../../Infrastructure/Utilities/SessionLocator");
var VehiclesSearchComponent = /** @class */ (function (_super) {
    __extends(VehiclesSearchComponent, _super);
    function VehiclesSearchComponent(cd) {
        var _this = _super.call(this) || this;
        _this.cd = cd;
        _this.vehicleListService = new VehicleExtendedListService_1.VehicleExtendedListService();
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.vehicles = new ObservableCollection_1.ObservableCollection([]);
        _this.selectedVehicles = new ObservableCollection_1.ObservableCollection([]);
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.tempSelectedRows = [];
        _this.SelectedRows = [];
        return _this;
    }
    VehiclesSearchComponent.prototype.LaodVehicles = function () {
        var _this = this;
        // this.vahicles.Clear();
        this.tempSelectedRows = [];
        this.vehicleListService.GetVehiclesForSelection().subscribe(function (response) {
            if (response) {
                //response.Result.forEach((vehicle) => {
                //    this.vehicles.Insert(vehicle);
                //});
                _this.vehicles.InsertCollection(response.Result);
                _this.Parent.invoiceItemPM.SupplierInvoiceItemVehicles.forEach(function (vehicle) {
                    var item = _this.vehicles.Collection.filter(function (d) { return d.Id == vehicle.VehicleId; })[0];
                    if (item) {
                        if (!_this.selectedVehicles.Collection.includes(item)) {
                            _this.selectedVehicles.Collection.push(item);
                        }
                        _this.cd.detectChanges();
                    }
                });
                _this.tempSelectedRows = _this.selectedVehicles.Collection;
            }
        });
    };
    VehiclesSearchComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.Parent = args.Parent;
        this.entityResourceService.getEntityResourceByTableName("Customs.Vehicle").subscribe(function (response) {
            _this.LaodVehicles();
        });
    };
    VehiclesSearchComponent.prototype.OnRowSelected = function (items) {
        this.SelectedRows = items;
        //this.selectedVehicles.Collection.forEach((vehicle) => {
        //    var item = items.filter(d => d.Id == vehicle.Id)[0];
        //    if (!item) {
        //        this.selectedVehicles.Remove(vehicle);
        //    }
        //});
    };
    VehiclesSearchComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    VehiclesSearchComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
        this.Parent.SelectVehicleCompleted(this.SelectedRows);
    };
    VehiclesSearchComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VehiclesSearchComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], VehiclesSearchComponent);
    return VehiclesSearchComponent;
}(BaseComponent_1.BaseComponent));
exports.VehiclesSearchComponent = VehiclesSearchComponent;
//# sourceMappingURL=VehiclesSearchComponent.js.map