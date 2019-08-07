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
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var RatesTableList_1 = require("../../EntityLists/RatesTableList");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var RatesTableExtendedListService = /** @class */ (function () {
    function RatesTableExtendedListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ratestableviews';
    }
    RatesTableExtendedListService.prototype.getClosestRate = function (baseCurrenyId, foreignCurrencyId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetClosestRate/?' + 'baseCurrenyId=' + baseCurrenyId + '&foreignCurrencyId=' + foreignCurrencyId, { headers: authHeader }).map(function (response) {
                var list = response.json();
                var entity;
                if (list) {
                    entity = _this.MapJsonToEntityList(list);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    RatesTableExtendedListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new RatesTableList_1.RatesTableList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    RatesTableExtendedListService.CachedData = [];
    RatesTableExtendedListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], RatesTableExtendedListService);
    return RatesTableExtendedListService;
}());
exports.RatesTableExtendedListService = RatesTableExtendedListService;
//# sourceMappingURL=RatesTableExtendedListService.js.map