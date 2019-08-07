"use strict";
/// <reference path="../../../infrastructure/datacontracts/serviceresponse.ts" />
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
//import 'rxjs/add/operator/map';
//import Rx from 'rxjs/Rx';
var Rx_1 = require("rxjs/Rx");
var HybridPartnerList_1 = require("../../EntityLists/HybridPartnerList");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var HybridPartnerExtendedListService = /** @class */ (function () {
    function HybridPartnerExtendedListService() {
        this.CachedData = [];
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/HybridPartnerExtendedList';
    }
    //  setServiceArgs(serviceArgs: ServiceArgs) {
    //      this._serviceArgs = serviceArgs;
    //      this._http = serviceArgs.http;
    //      this._apiUrl = logitude_url + 'api/HybridPartnerExtendedList';
    //this.CachedData = [];
    //  }
    HybridPartnerExtendedListService.prototype.GetHybridPartnerLists = function (tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetHybridPartnerLists/?' + 'tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity;
                        entity = _this.MapJsonToEntityList(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                return _mappedListsArray;
            });
        });
    };
    HybridPartnerExtendedListService.prototype.GetHybridPartnerListWithNoRequest = function (tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetHybridPartnerListWithNoRequest/?' + 'tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity;
                        entity = _this.MapJsonToEntityList(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                return _mappedListsArray;
            });
        });
    };
    HybridPartnerExtendedListService.prototype.GetAllowdHybridPartnerLists = function (hybridPartnerId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAllowdHybridPartnerLists/?' + 'hybridPartnerId=' + hybridPartnerId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = allLists;
                return pmresponse;
            });
        });
    };
    HybridPartnerExtendedListService.prototype.GetAllowingHybridPartnerLists = function (hybridPartnerId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAllowingHybridPartnerLists/?' + 'hybridPartnerId=' + hybridPartnerId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = allLists;
                return pmresponse;
            });
        });
    };
    HybridPartnerExtendedListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new HybridPartnerList_1.HybridPartnerList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    HybridPartnerExtendedListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], HybridPartnerExtendedListService);
    return HybridPartnerExtendedListService;
}());
exports.HybridPartnerExtendedListService = HybridPartnerExtendedListService;
//# sourceMappingURL=HybridPartnerExtendedListService.js.map