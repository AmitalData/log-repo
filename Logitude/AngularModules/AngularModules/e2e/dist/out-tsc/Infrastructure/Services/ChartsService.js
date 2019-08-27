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
require("rxjs/add/operator/catch");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../Utilities/ServiceHelper");
var List_1 = require("../DataContracts/Dashboard/List");
var DashBoardClass_1 = require("../DataContracts/Dashboard/DashBoardClass");
var ChartsService = /** @class */ (function () {
    function ChartsService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        //this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
    }
    ChartsService.prototype.GetMoneyStatusForTenant = function (ActivityType, months, days, tenant, index, currency) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetMoneyStatusForTenant?type=' + ActivityType + '&months=' + months + '&days=' + days + '&tenant=' + tenant + '&index=' + index + '&currency=' + currency, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    ChartsService.prototype.GetMoneyOutStatusForTenant = function (months, days, tenant, index, currency) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/InvoiceDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetMoneyOutStatusForTenant?months=' + months + '&days=' + days + '&tenant=' + tenant + '&index=' + index + '&currency=' + currency, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    ChartsService.prototype.GetActivityStatusByType = function (ActivityType, fromDate, toDate, currentTenant, customerid, directionId, transportmodeId) {
        var _this = this;
        if (customerid === void 0) { customerid = null; }
        if (directionId === void 0) { directionId = null; }
        if (transportmodeId === void 0) { transportmodeId = null; }
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetActivityStatusByType?type=' + ActivityType + '&FromDate=' + ServiceHelper_1.ServiceHelper.GetDateString(fromDate) + '&ToDate=' + ServiceHelper_1.ServiceHelper.GetDateString(toDate) + '&currentTenant=' + currentTenant + '&customerid=' + customerid + '&directionid=' + directionId + '&transportmodeId=' + transportmodeId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new List_1.List();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });
    };
    ChartsService.prototype.GetActivityStatus = function (ActivityType, lastMonths, lastDays, currentTenant, customerid) {
        var _this = this;
        if (customerid === void 0) { customerid = null; }
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetActivityStatus?type=' + ActivityType + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&currentTenant=' + currentTenant + '&customerid=' + customerid, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new List_1.List();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });
    };
    ChartsService.prototype.GetShipmentByDirectionAndTransmode = function (type, lastMonths, lastDays, currentTenant, customerid) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetShipmentByDirectionAndTransmode?type=' + type + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&currentTenant=' + currentTenant + '&customerid=' + customerid, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new List_1.List();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });
    };
    ChartsService.prototype.GetShipmentsByTop10CountriesDashBoard = function (type, lastMonths, lastDays, measurment, currentTenant, top, includeOthers, customerid, directionId, transmodeId) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetShipmentsByTop10CountriesDashBoard?type=' + type + '&lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&measurment=' + measurment + '&currentTenant=' + currentTenant + '&top=' + top + '&includeOthers=' + includeOthers + '&customerid=' + customerid + '&directionId=' + directionId + '&transmodeId=' + transmodeId, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new List_1.List();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.add(entity);
                }
                return myList;
            });
        });
    };
    ChartsService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new DashBoardClass_1.DashBoardClass();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    ChartsService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ChartsService);
    return ChartsService;
}());
exports.ChartsService = ChartsService;
//# sourceMappingURL=ChartsService.js.map