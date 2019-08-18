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
var HarmonizeCodeListService_1 = require("../../../../../Shipment/Services/StandardLists/HarmonizeCodeListService");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var HarmonizesComponent = /** @class */ (function () {
    function HarmonizesComponent() {
        this.ItemsSource = [];
        this.HarmonizesCount = 0;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.searchText = null;
        this.myService = new HarmonizeCodeListService_1.HarmonizeCodeListService();
        this.LoadAllData();
    }
    HarmonizesComponent.prototype.SetWindowArgs = function (args) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
    };
    Object.defineProperty(HarmonizesComponent.prototype, "SearchText", {
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
    HarmonizesComponent.prototype.LoadAllData = function () {
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
            _this.HarmonizesCount = _this.ItemsSource == null ? 0 : _this.ItemsSource.length;
        });
    };
    HarmonizesComponent.prototype.Selecting = function (item) {
        this.SetField(item);
        if (item == null) {
            this.SetField(null);
        }
        else {
            this.SetField(item);
        }
        this.Close();
    };
    HarmonizesComponent.prototype.SetField = function (item) {
        var iCode = null;
        if (item) {
            iCode = item.Code;
        }
        if (this.Entity[this.FieldName] != iCode) {
            this.Entity[this.FieldName] = iCode;
        }
    };
    HarmonizesComponent.prototype.CloseButtonClicked = function () {
        this.Close();
    };
    HarmonizesComponent.prototype.Close = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    HarmonizesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './HarmonizesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], HarmonizesComponent);
    return HarmonizesComponent;
}());
exports.HarmonizesComponent = HarmonizesComponent;
//# sourceMappingURL=HarmonizesComponent.js.map