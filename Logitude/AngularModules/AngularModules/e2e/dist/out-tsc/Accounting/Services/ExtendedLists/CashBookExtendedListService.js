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
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var CashBookList_1 = require("../../EntityLists/CashBookList");
var CashBookStatusChart_1 = require("../../DataContracts/CashBookStatusChart");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var CashBookExtendedListService = /** @class */ (function () {
    function CashBookExtendedListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CashBookViews';
    }
    CashBookExtendedListService.prototype.GetCashBookStatusChartData = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetCashBookStatusChartData';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var _mappedListsArray = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {
                        var entity;
                        entity = _this.MapJsonToCashBookStatusChart(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CashBookExtendedListService.prototype.GetCashBookSummary = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCashBookSummary?', {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                return allLists;
            });
        });
    };
    CashBookExtendedListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new CashBookList_1.CashBookList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    CashBookExtendedListService.prototype.MapJsonToCashBookStatusChart = function (jsonList) {
        var entityList;
        entityList = new CashBookStatusChart_1.CashBookStatusChart();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    CashBookExtendedListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CashBookExtendedListService);
    return CashBookExtendedListService;
}());
exports.CashBookExtendedListService = CashBookExtendedListService;
//# sourceMappingURL=CashBookExtendedListService.js.map