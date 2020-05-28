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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var QuoteListService_1 = require("../../../../Quote/Services/StandardLists/QuoteListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var QuoteDomainService_1 = require("../../../../Quote/Services/QuoteDomainService");
var QuotesWindowComponent = /** @class */ (function (_super) {
    __extends(QuotesWindowComponent, _super);
    function QuotesWindowComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Opportunity";
        _this.DataContext = _this;
        _this.ObsList = [];
        _this.searchText = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.NoConnectedQuotes = false;
        _this.selectedItem = null;
        _this.quoteListService = new QuoteListService_1.QuoteListService();
        _this.quoteDomainService = new QuoteDomainService_1.QuoteDomainService();
        return _this;
    }
    Object.defineProperty(QuotesWindowComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            this.searchText = newValue;
            this.OnSearchTextChanged();
        },
        enumerable: true,
        configurable: true
    });
    QuotesWindowComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args;
        this.LoadQuotesList();
    };
    QuotesWindowComponent.prototype.LoadQuotesList = function () {
        var _this = this;
        this.ObsList = [];
        if (this.EntityPM != null) {
            var filters = new ApiQueryFilters_1.ApiQueryFilters;
            filters.PageSize = 25;
            filters.PageIndex = 0;
            var SearchedValue = null;
            if (this.SearchText != null)
                SearchedValue = this.SearchText.trim();
            filters.addAdditionalFilter("CustomerId", this.EntityPM.CustomerId, null, null, "Equals", false, false, false, "string");
            filters.addAdditionalFilter("NotConnectedOpportunity", true, null, null, "Equals", true, true, false, "string");
            filters.addAdditionalFilter("SearchFields", SearchedValue, null, null, "Contains", false, false, false, "string");
            this.quoteListService.getByFilters(filters).subscribe(function (result) {
                var QuoteList = result.Result.reverse();
                if (QuoteList.length == 0)
                    _this.NoConnectedQuotes = true;
                else
                    _this.NoConnectedQuotes = false;
                QuoteList.forEach(function (item) {
                    _this.ObsList.push(new QuoteItemClass(item, _this));
                });
            });
        }
    };
    Object.defineProperty(QuotesWindowComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (item) { this.selectedItem = item; },
        enumerable: true,
        configurable: true
    });
    QuotesWindowComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    QuotesWindowComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var myConnectedQuotes = "";
        this.CurrentSession.StartBusyIndicatorSaving();
        this.ObsList.filter(function (p) { return p.IsChecked; }).forEach(function (item) { myConnectedQuotes += item.QuoteId + ":"; });
        if (myConnectedQuotes.length > 0) {
            this.quoteDomainService.ConnectQuotesToOpportunity(this.EntityPM.Id, myConnectedQuotes).subscribe(function (p) {
                _this.CurrentSession.StopBusyIndicator();
                _this.CurrentSession.CloseCurrentWindowEmit("ok");
            });
        }
        else {
            this.CurrentSession.CloseCurrentWindowEmit("cancel");
        }
    };
    QuotesWindowComponent.prototype.OnSearchTextChanged = function () {
        var _this = this;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        this.timerToken = setTimeout(function () { return _this.LoadQuotesList(); }, 500);
    };
    Object.defineProperty(QuotesWindowComponent.prototype, "SearchTextLabel", {
        get: function () { return TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.SearchFields"); },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuotesWindowComponent.prototype, "OKButtonEnabled", {
        get: function () {
            var myResult = false;
            if (this.ObsList.filter(function (d) { return d.IsChecked; })[0]) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    QuotesWindowComponent = __decorate([
        core_1.Component({
            selector: 'QuotesWindowComponent',
            moduleId: module.id,
            templateUrl: './QuotesWindowComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], QuotesWindowComponent);
    return QuotesWindowComponent;
}(BaseComponent_1.BaseComponent));
exports.QuotesWindowComponent = QuotesWindowComponent;
var QuoteItemClass = /** @class */ (function (_super) {
    __extends(QuoteItemClass, _super);
    function QuoteItemClass(item, trigger) {
        var _this = _super.call(this) || this;
        _this.entityList = item;
        _this.trigger = trigger;
        _this.QuoteId = item.Id;
        return _this;
    }
    Object.defineProperty(QuoteItemClass.prototype, "QuoteId", {
        get: function () { return this.quoteId; },
        set: function (value) { this.quoteId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItemClass.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            if (this.isChecked != value) {
                this.isChecked = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItemClass.prototype, "QuoteNumber", {
        get: function () { return this.entityList.QuoteNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItemClass.prototype, "OpenDate", {
        get: function () { return this.entityList.OpenDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItemClass.prototype, "ExpirationDate", {
        get: function () { return this.entityList.ExpirationDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItemClass.prototype, "StageName", {
        get: function () { return this.entityList.StageName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItemClass.prototype, "QuoteTypeName", {
        get: function () { return this.entityList.QuoteTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItemClass.prototype, "CarrierName", {
        get: function () { return this.entityList.CarrierName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItemClass.prototype, "FromCountryCode", {
        get: function () { return this.entityList.FromCountryCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItemClass.prototype, "FromPortCountry", {
        get: function () { return this.entityList.FromPortCountry; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItemClass.prototype, "ToCountryCode", {
        get: function () { return this.entityList.ToCountryCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteItemClass.prototype, "ToPortCountry", {
        get: function () { return this.entityList.ToPortCountry; },
        enumerable: true,
        configurable: true
    });
    return QuoteItemClass;
}(BaseComponent_1.BaseComponent));
//# sourceMappingURL=QuotesWindowComponent.js.map