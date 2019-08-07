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
var http_1 = require("@angular/http");
require("rxjs/add/operator/map");
var Rx_1 = require("rxjs/Rx");
var QuoteList_1 = require("../../EntityLists/QuoteList");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var QuoteFollowUpListService = /** @class */ (function () {
    function QuoteFollowUpListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuoteFollowUpsViews';
    }
    QuoteFollowUpListService.prototype.setServiceArgs = function (serviceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        //this._apiUrl = logitude_url + 'api/FollowUpsViews';
    };
    QuoteFollowUpListService.prototype.getCount = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '?tenant=' + SessionLocator_1.SessionLocator.Tenant.toString(), {
                headers: authHeader
            }).map(function (response) {
                return response.json();
            });
        });
    };
    QuoteFollowUpListService.prototype.getSingle = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getsingle/?' + 'id=' + id, {
                headers: authHeader
            }).map(function (response) {
                var list = response.json();
                var entity;
                entity = _this.MapJsonToEntityList(list);
                return list;
            });
        });
    };
    QuoteFollowUpListService.prototype.getAll = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getall', {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    _mappedListsArray.push(entity);
                }
                return _mappedListsArray;
            });
        });
    };
    QuoteFollowUpListService.prototype.getByFilters = function (filters) {
        var _this = this;
        var urlparameters = '/getbyfilters?';
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];
            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");
            if (!urlparameters.endsWith('?')) {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter)
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);
        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var callUrl = this._apiUrl.concat(urlparameters);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                var viewResponse = response.json();
                //viewResponse = response.json();
                //var allLists = response.json();
                var _mappedListsArray = [];
                for (var key in viewResponse.Data) {
                    var entity;
                    entity = _this.MapJsonToEntityList(viewResponse.Data[key]);
                    _mappedListsArray.push(entity);
                }
                viewResponse.Data = _mappedListsArray;
                return viewResponse; //{Data: _mappedListsArray,DataCount:response.headers };
            });
        });
    };
    QuoteFollowUpListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new QuoteList_1.QuoteList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    QuoteFollowUpListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], QuoteFollowUpListService);
    return QuoteFollowUpListService;
}());
exports.QuoteFollowUpListService = QuoteFollowUpListService;
//# sourceMappingURL=QuoteFollowUpListService.js.map