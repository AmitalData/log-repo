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
var CountryCityListService_1 = require("../../../../Common/Services/StandardLists/CountryCityListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var CitySelectionComponent = /** @class */ (function () {
    function CitySelectionComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.ItemsSource = [];
        this.IsNoData = false;
        this.SelectedCity = null;
        this.ObjectTableName = "CountryCity";
        this.IsResourcesReady = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.searchText = null;
        this.myService = new CountryCityListService_1.CountryCityListService();
        this.ItemsSource = new Array();
    }
    CitySelectionComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.args = args;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.IsResourcesReady = true;
            _this.LoadData();
        });
    };
    Object.defineProperty(CitySelectionComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            if (this.searchText != newValue) {
                this.searchText = newValue;
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    CitySelectionComponent.prototype.LoadData = function () {
        var _this = this;
        this.IsNoData = false;
        this.ItemsSource = [];
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 50;
        filters.SortBy = "EnglishName";
        filters.SortDirection = "Descending";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");
        }
        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, true, false, "Boolean");
        if (!Tools_1.AppTool.IsNullOrEmpty(this.args.CountryId)) {
            filters.addAdditionalFilter("CountryId", this.args.CountryId, null, null, "Equals", false, true, false, "Boolean");
        }
        this.myService.getByFilters(filters).subscribe(function (myResponse) {
            if (myResponse == null) {
                _this.IsNoData = true;
                _this.ItemsSource = [];
            }
            else {
                _this.ItemsSource = myResponse.Result;
                if (_this.ItemsSource.length == 0) {
                    _this.IsNoData = true;
                }
            }
        });
    };
    CitySelectionComponent.prototype.Selecting = function (item) {
        this.SelectedCity = item;
        if (item == null) {
            this.args.CityName = null;
            this.args.CityLocalName = null;
            this.args.CountryId = null;
            this.args.StateId = null;
            this.args.IsCitySelected = false;
        }
        else {
            this.args.CityName = item.EnglishName;
            this.args.CityLocalName = item.LocalName;
            this.args.CountryId = item.CountryId;
            this.args.StateId = item.StateId;
            this.args.IsCitySelected = true;
        }
        this.Close();
    };
    CitySelectionComponent.prototype.Close = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CitySelectionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CitySelectionComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], CitySelectionComponent);
    return CitySelectionComponent;
}());
exports.CitySelectionComponent = CitySelectionComponent;
//# sourceMappingURL=CitySelectionComponent.js.map