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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var ShipmentListService_1 = require("../../../Shipment/Services/StandardLists/ShipmentListService");
var ShipmentDomainService_1 = require("../../../Shipment/Services/ShipmentDomainService");
var QuoteListService_1 = require("../../../Quote/Services/StandardLists/QuoteListService");
var ChooseEntityComponent = /** @class */ (function () {
    function ChooseEntityComponent() {
        this.ItemsSource = [];
        this.AllShipmentsCount = 0;
        this.AllQuotesCount = 0;
        this.ListTitle = "";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsShipment = false;
        this.IsQuote = false;
        this.EntityObjectTableName = "";
        this.IsFromTicket = false;
        this.searchText = null;
        // Filters
        this.mySelectedDirectionFilter = "";
        this.mySelectedTransportFilter = "";
        // Commands
        this.SelectedShipment = null;
        this.SelectedQuote = null;
        this.DomainService = new ShipmentDomainService_1.ShipmentDomainService();
        this.myShipmentListService = new ShipmentListService_1.ShipmentListService();
        this.myQuoteListService = new QuoteListService_1.QuoteListService();
    }
    ChooseEntityComponent.prototype.SetWindowArgs = function (args) {
        this.EntityObjectTableName = args.EntityObjectTableName;
        this.IsFromTicket = args.IsFromTicket;
        if (this.EntityObjectTableName == "Shipment" || this.EntityObjectTableName == "Master") {
            this.IsShipment = true;
            this.LoadShipmentsData();
            this.ListTitle = "All Shipments";
        }
        else if (this.EntityObjectTableName == "House") {
            this.IsShipment = true;
            this.EntityId = args.EntityId;
            this.LoadShipmentsData();
            this.ListTitle = "All Houses";
        }
        else {
            this.IsQuote = true;
            this.LoadQuotesData();
            this.ListTitle = "All Quotes";
        }
    };
    Object.defineProperty(ChooseEntityComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            if (this.searchText != newValue) {
                this.searchText = newValue;
                if (this.EntityObjectTableName == "Shipment" || this.EntityObjectTableName == "Master" || this.EntityObjectTableName == "House") {
                    this.LoadShipmentsData();
                }
                else {
                    this.LoadQuotesData();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ChooseEntityComponent.prototype.LoadShipmentsData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource = [];
        this.AllShipmentsCount = 0;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.GetAll = false;
        filters.GetCount = true;
        var searchValue = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            searchValue = Tools_1.AppTool.IsNullOrEmpty(this.SearchText.trim()) ? null : this.SearchText;
        }
        if (!this.IsFromTicket) {
            if (this.EntityObjectTableName == "Master") {
                filters.addAdditionalFilter("ShipmentLevelCode", "D,C", null, null, "InList", true, true, false, "string");
            }
            else if (this.EntityObjectTableName == "House") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityId)) {
                    filters.addAdditionalFilter("MasterShipmentDataId", this.EntityId, null, null, "Equals", false, false, false, "string");
                }
                filters.addAdditionalFilter("ShipmentLevelCode", "H", null, null, "Equals", false, true, false, "string");
                filters.addAdditionalFilter("MasterConnectedHouses", true, null, null, "Equals", true, false, false, "Boolean");
            }
            else {
                filters.addAdditionalFilter("ShipmentLevelCode", "D,H", null, null, "InList", true, true, false, "string");
            }
        }
        filters.addAdditionalFilter("SearchFields", searchValue, null, null, "Contains", false, false, false, "string");
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
        //this.DomainService.GetShipmentFullTextSearch(filters).subscribe((myResponse: ServiceResponse) => {
        //    if (!myResponse.HasError) {
        //        this.ItemsSource = myResponse.Result;
        //        this.AllShipmentsCount = this.ItemsSource.length;
        //    }
        //});
        this.myShipmentListService.getByFilters(filters).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.ItemsSource = myResponse.Result;
                _this.AllShipmentsCount = _this.ItemsSource.length;
            }
        });
    };
    ChooseEntityComponent.prototype.LoadQuotesData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource = [];
        this.AllQuotesCount = 0;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.GetAll = false;
        filters.GetCount = true;
        var searchValue = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            searchValue = Tools_1.AppTool.IsNullOrEmpty(this.SearchText.trim()) ? null : this.SearchText;
        }
        filters.addAdditionalFilter("SearchFields", searchValue, null, null, "Contains", false, false, false, "string");
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
        this.myQuoteListService.getByFilters(filters).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.ItemsSource = myResponse.Result;
                _this.AllQuotesCount = _this.ItemsSource.length;
            }
        });
    };
    Object.defineProperty(ChooseEntityComponent.prototype, "CurrentTransportModeId", {
        get: function () { return this.mySelectedDirectionFilter; },
        set: function (newValue) {
            if (this.mySelectedDirectionFilter != newValue) {
                this.mySelectedDirectionFilter = newValue;
                if (this.EntityObjectTableName == "Shipment" || this.EntityObjectTableName == "Master" || this.EntityObjectTableName == "House") {
                    this.LoadShipmentsData();
                }
                else {
                    this.LoadQuotesData();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ChooseEntityComponent.prototype, "CurrentDirectionId", {
        get: function () { return this.mySelectedTransportFilter; },
        set: function (newValue) {
            if (this.mySelectedTransportFilter != newValue) {
                this.mySelectedTransportFilter = newValue;
                if (this.EntityObjectTableName == "Shipment" || this.EntityObjectTableName == "Master" || this.EntityObjectTableName == "House") {
                    this.LoadShipmentsData();
                }
                else {
                    this.LoadQuotesData();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ChooseEntityComponent.prototype.Selecting = function (item) {
        this.SelectedShipment = item;
        this.Close();
    };
    ChooseEntityComponent.prototype.SelectingQuote = function (item) {
        this.SelectedQuote = item;
        this.Close();
    };
    ChooseEntityComponent.prototype.CloseButtonClicked = function () {
        this.Close();
    };
    ChooseEntityComponent.prototype.Close = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ChooseEntityComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ChooseEntityComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ChooseEntityComponent);
    return ChooseEntityComponent;
}());
exports.ChooseEntityComponent = ChooseEntityComponent;
//# sourceMappingURL=ChooseEntityComponent.js.map