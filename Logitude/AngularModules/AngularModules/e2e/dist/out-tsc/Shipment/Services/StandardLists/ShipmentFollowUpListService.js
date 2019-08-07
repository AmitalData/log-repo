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
var ShipmentList_1 = require("../../EntityLists/ShipmentList");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ShipmentFollowUpListService = /** @class */ (function () {
    function ShipmentFollowUpListService() {
        console.log("constructing ShipmentFollowUpsListService");
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/FollowUpsViews';
    }
    ShipmentFollowUpListService.prototype.setServiceArgs = function (serviceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        //this._apiUrl = logitude_url + 'api/FollowUpsViews';
    };
    ShipmentFollowUpListService.prototype.getCount = function () {
        var _this = this;
        console.log('--------------------------------------> calling getSingle:');
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
    ShipmentFollowUpListService.prototype.getSingle = function (id) {
        var _this = this;
        console.log('--------------------------------------> calling getSingle:');
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
    ShipmentFollowUpListService.prototype.getAll = function () {
        var _this = this;
        console.log('--------------------------------------> calling getAllEntityListsFromServer:');
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
    ShipmentFollowUpListService.prototype.getByFilters = function (filters) {
        var _this = this;
        //console.log('--------------------------------------> calling the server with filters:');
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
        //console.log("addtionalFiltersValues", addtionalFiltersValues);
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var callUrl = this._apiUrl.concat(urlparameters);
        //console.log("Calling Url:" + callUrl);
        //console.log("abol 3abed");
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                //console.log("i'm the response yo ", response);
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
                //console.log(_mappedListsArray);
                return viewResponse; //{Data: _mappedListsArray,DataCount:response.headers };
            });
        });
    };
    ShipmentFollowUpListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new ShipmentList_1.ShipmentList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    ShipmentFollowUpListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ShipmentFollowUpListService);
    return ShipmentFollowUpListService;
}());
exports.ShipmentFollowUpListService = ShipmentFollowUpListService;
//# sourceMappingURL=ShipmentFollowUpListService.js.map