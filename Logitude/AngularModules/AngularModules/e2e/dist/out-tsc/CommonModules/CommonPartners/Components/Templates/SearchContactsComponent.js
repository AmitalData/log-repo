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
var ContactListService_1 = require("../../../../Common/Services/StandardLists/ContactListService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var SearchContactsComponent = /** @class */ (function () {
    function SearchContactsComponent() {
        this.ItemsSource = [];
        this.MyContactsCount = 0;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.searchText = null;
        this.myService = new ContactListService_1.ContactListService();
        this.LoadAllData();
    }
    SearchContactsComponent.prototype.SetWindowArgs = function (args) {
        this.inputTemplate = args;
    };
    Object.defineProperty(SearchContactsComponent.prototype, "SearchText", {
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
    SearchContactsComponent.prototype.LoadAllData = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 500;
        filters.SortBy = "EnglishName";
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
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
            _this.MyContactsCount = _this.ItemsSource == null ? 0 : _this.ItemsSource.length;
        });
    };
    SearchContactsComponent.prototype.Selecting = function (item) {
        if (this.inputTemplate != null) {
            if (item != null) {
                this.inputTemplate.Email = item.Email;
                this.inputTemplate.EmailLostFocus(item.Email);
            }
        }
        this.Close();
    };
    SearchContactsComponent.prototype.CloseButtonClicked = function () {
        this.Close();
    };
    SearchContactsComponent.prototype.Close = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SearchContactsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SearchContactsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SearchContactsComponent);
    return SearchContactsComponent;
}());
exports.SearchContactsComponent = SearchContactsComponent;
//# sourceMappingURL=SearchContactsComponent.js.map