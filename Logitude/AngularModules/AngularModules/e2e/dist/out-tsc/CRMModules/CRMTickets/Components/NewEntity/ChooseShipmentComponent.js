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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var TicketPM_1 = require("../../../../CRM/EntityPMs/TicketPM");
var ShipmentListService_1 = require("../../../../Shipment/Services/StandardLists/ShipmentListService");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var ChooseShipmentComponent = /** @class */ (function () {
    function ChooseShipmentComponent() {
        this.EntityPM = new TicketPM_1.TicketPM();
        this.ItemsSource = [];
        this.AllShipmentsCount = 0;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.searchText = null;
        // Filters
        this.mySelectedDirectionFilter = "";
        this.mySelectedTransportFilter = "";
        // Commands
        this.SelectedShipment = null;
        this.DomainService = new ShipmentDomainService_1.ShipmentDomainService();
        this.myService = new ShipmentListService_1.ShipmentListService();
    }
    ChooseShipmentComponent.prototype.SetWindowArgs = function (entityPM) {
        this.EntityPM = entityPM;
        this.LoadShipmentsData();
    };
    Object.defineProperty(ChooseShipmentComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            if (this.searchText != newValue) {
                this.searchText = newValue;
                this.LoadShipmentsData();
            }
        },
        enumerable: true,
        configurable: true
    });
    ChooseShipmentComponent.prototype.LoadShipmentsData = function () {
        var _this = this;
        this.ItemsSource = [];
        this.AllShipmentsCount = 0;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.GetAll = true;
        filters.GetCount = true;
        var searchValue = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            searchValue = Tools_1.AppTool.IsNullOrEmpty(this.SearchText.trim()) ? null : this.SearchText;
        }
        filters.addAdditionalFilter("SearchFields", searchValue, null, null, "Contains", false, false, false, "string");
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CompanyId)) {
            filters.addAdditionalFilter("CustomerId", this.EntityPM.CompanyId, null, null, "Equals", false, false, false, "string");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CurrentDirectionId)) {
            if (this.CurrentDirectionId == "All")
                this.CurrentDirectionId = "";
            else {
                filters.addAdditionalFilter("DirectionId", this.CurrentDirectionId, null, null, "Equals", false, true, false, "string");
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CurrentTransportModeId)) {
            if (this.CurrentTransportModeId == "All")
                this.CurrentTransportModeId = "";
            else {
                filters.addAdditionalFilter("TransportModeId", this.CurrentTransportModeId, null, null, "Equals", false, true, false, "string");
            }
        }
        //this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
        //    if (!myResponse.HasError) {
        //        this.ItemsSource = myResponse.Result;
        //        this.AllShipmentsCount = this.ItemsSource.length;
        //    }
        //});
        this.DomainService.GetShipmentFullTextSearch(filters).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ItemsSource = myResponse.Result;
                _this.AllShipmentsCount = _this.ItemsSource.length;
            }
        });
    };
    Object.defineProperty(ChooseShipmentComponent.prototype, "CurrentTransportModeId", {
        get: function () { return this.mySelectedDirectionFilter; },
        set: function (newValue) {
            if (this.mySelectedDirectionFilter != newValue) {
                this.mySelectedDirectionFilter = newValue;
                this.LoadShipmentsData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChooseShipmentComponent.prototype, "CurrentDirectionId", {
        get: function () { return this.mySelectedTransportFilter; },
        set: function (newValue) {
            if (this.mySelectedTransportFilter != newValue) {
                this.mySelectedTransportFilter = newValue;
                this.LoadShipmentsData();
            }
        },
        enumerable: true,
        configurable: true
    });
    ChooseShipmentComponent.prototype.Selecting = function (item) {
        this.SelectedShipment = item;
        this.Close();
    };
    ChooseShipmentComponent.prototype.CloseButtonClicked = function () {
        this.Close();
    };
    ChooseShipmentComponent.prototype.Close = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ChooseShipmentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ChooseShipmentComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ChooseShipmentComponent);
    return ChooseShipmentComponent;
}());
exports.ChooseShipmentComponent = ChooseShipmentComponent;
//# sourceMappingURL=ChooseShipmentComponent.js.map