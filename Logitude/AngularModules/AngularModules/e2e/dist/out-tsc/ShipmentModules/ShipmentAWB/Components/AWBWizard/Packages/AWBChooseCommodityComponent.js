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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var CommodityListService_1 = require("../../../../../Common/Services/StandardLists/CommodityListService");
var CommonDomainService_1 = require("../../../../../Common/Services/CommonDomainService");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var EntityPMService_1 = require("../../../../../Infrastructure/Services/EntityPMService");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var AWBChooseCommodityComponent = /** @class */ (function () {
    function AWBChooseCommodityComponent() {
        this.ItemsSource = [];
        this.ItemsSource_TenantZero = [];
        this.MyCommoditiesCount = 0;
        this.AllCommoditiesCount = 0;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.searchText = null;
        this.myService = new CommodityListService_1.CommodityListService();
        this.LoadAllData();
    }
    AWBChooseCommodityComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args['EntityPM'];
        this.FieldName = args['FieldName'];
        this.NameProperty = args['NameProperty'];
    };
    Object.defineProperty(AWBChooseCommodityComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            if (this.searchText != newValue) {
                this.searchText = newValue;
                this.LoadAllData();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBChooseCommodityComponent.prototype.LoadAllData = function () {
        this.LoadTenantData();
        this.LoadTenantZeroData();
    };
    AWBChooseCommodityComponent.prototype.LoadTenantData = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.SortBy = "Code";
        filters.SortDirection = "Descending";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");
        }
        this.myService.getByFilters(filters).subscribe(function (myResponse) {
            if (myResponse == null) {
                _this.ItemsSource = [];
            }
            else {
                if (!myResponse.HasError) {
                    _this.ItemsSource = myResponse.Result;
                }
            }
            _this.MyCommoditiesCount = _this.ItemsSource == null ? 0 : _this.ItemsSource.length;
        });
    };
    AWBChooseCommodityComponent.prototype.LoadTenantZeroData = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.SortBy = "Code";
        filters.SortDirection = "Descending";
        filters.Tenant = 0;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");
        }
        this.myService.getByFilters(filters).subscribe(function (myResponse) {
            if (myResponse == null) {
                _this.ItemsSource_TenantZero = [];
            }
            else {
                if (!myResponse.HasError) {
                    _this.ItemsSource_TenantZero = myResponse.Result;
                }
            }
            _this.AllCommoditiesCount = _this.ItemsSource_TenantZero == null ? 0 : _this.ItemsSource_TenantZero.length;
        });
    };
    AWBChooseCommodityComponent.prototype.Selecting = function (item) {
        this.SetField(item);
        if (item == null) {
            this.SetField(null);
        }
        else {
            this.SetField(item);
        }
        this.Close();
    };
    AWBChooseCommodityComponent.prototype.SelectingTenantZero = function (item) {
        this.SetField(item);
        if (item == null) {
            this.Close();
        }
        else {
            this.CopyToMyTenant(item.Id);
        }
    };
    AWBChooseCommodityComponent.prototype.SetField = function (item) {
        var myCommodityCode = null;
        var myCommodityName = null;
        if (item) {
            myCommodityCode = item.Code;
            myCommodityName = item.Name;
        }
        if (this.EntityPM[this.FieldName] != myCommodityCode) {
            this.EntityPM[this.FieldName] = myCommodityCode;
        }
        if (this.EntityPM[this.NameProperty] != myCommodityName) {
            this.EntityPM[this.NameProperty] = myCommodityName;
        }
    };
    AWBChooseCommodityComponent.prototype.CopyToMyTenant = function (tenantZeroId) {
        var _this = this;
        if (this.DomainService == null) {
            this.DomainService = new CommonDomainService_1.CommonDomainService();
        }
        this.DomainService.GetCopyCommodityToTenant(tenantZeroId).subscribe(function (myResult) {
            _this.Close();
        });
    };
    AWBChooseCommodityComponent.prototype.CloseButtonClicked = function () {
        this.Close();
    };
    AWBChooseCommodityComponent.prototype.Close = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AWBChooseCommodityComponent.prototype.AddCommodityClicked = function () {
        var _this = this;
        var myService = new EntityPMService_1.EntityPMService();
        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        myService.getNewEntity("Commodity").then(function (response) {
            var args = new EntityArgs_1.EntityArgs();
            args.EntityPM = response;
            args.ObjectTableName = "Commodity";
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate("Commodity"));
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewEntityWindowClosed($event); });
            logWindow.Show(componentPath);
        });
    };
    AWBChooseCommodityComponent.prototype.OnNewEntityWindowClosed = function ($event) {
        console.log($event);
        if ($event && $event != "event") {
            this.LoadTenantData();
        }
    };
    AWBChooseCommodityComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AWBChooseCommodityComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AWBChooseCommodityComponent);
    return AWBChooseCommodityComponent;
}());
exports.AWBChooseCommodityComponent = AWBChooseCommodityComponent;
//# sourceMappingURL=AWBChooseCommodityComponent.js.map