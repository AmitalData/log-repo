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
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var RatesTablePM_1 = require("../../Infrastructure/EntityPMs/RatesTablePM");
var TenantPMService_1 = require("./StandardPMs/TenantPMService");
var Rx_1 = require("rxjs/Rx");
var CurrencyRatesService = /** @class */ (function () {
    function CurrencyRatesService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/currencyrates';
    }
    CurrencyRatesService.prototype.getAll = function (baseCurrencyId, date) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/getall?baseCurrencyId=' + baseCurrencyId + '&dateString=' + ServiceHelper_1.ServiceHelper.GetDateString(date);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    _mappedListsArray.push(entity);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CurrencyRatesService.prototype.GetCurrenciesExchangeRateByValueDate = function (currencyId, loadingDate) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCurrenciesExchangeRateByValueDate?currencyId=' + currencyId + '&dateString=' + ServiceHelper_1.ServiceHelper.GetDateString(loadingDate);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    _mappedListsArray.push(entity);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CurrencyRatesService.prototype.GetRatesByValueDate = function (currencyId, date) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetRatesByValueDate?currencyId=' + currencyId + '&dateString=' + ServiceHelper_1.ServiceHelper.GetDateString(date);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToRatesTableList(allLists[key]);
                    _mappedListsArray.push(entity);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CurrencyRatesService.prototype.UpdateAccountingCurrency = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapJsonToAccountingCurrencyHelper(entityPM, false);
            return _this._http.put(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var myPMService = new TenantPMService_1.TenantPMService();
                var mappedResult;
                if (myJsonResult) {
                    mappedResult = myPMService.MapJsonToEntityPM(myJsonResult);
                }
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    //InsertListOfRatesTable(ratesTables: LastRate[]) {
    //    var authHeader = new Headers();
    //    authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
    //    var url = this._apiUrl + '/GetInsertListOfRatesTable?ratesTables=' + ratesTables;
    //    return Observable.defer(() => {
    //        return this._http.get(url, { headers: authHeader }).map(response => {
    //            return response.json();
    //        }).catch(ServiceHelper.HandleServiceError);
    //    });
    //}
    CurrencyRatesService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new LastRate();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    CurrencyRatesService.prototype.MapJsonToRatesTableList = function (jsonList) {
        var entityList;
        entityList = new RatesTablePM_1.RatesTablePM();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    CurrencyRatesService.prototype.MapJsonToAccountingCurrencyHelper = function (jsonPM, getCallMap, entity) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entity === void 0) { entity = null; }
        if (!entity) {
            entity = new AccountingCurrencyHelper();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "TenantPM") {
                var myPMService = new TenantPMService_1.TenantPMService();
                entity.TenantPM = myPMService.MapJsonToEntityPM(jsonPM[property], getCallMap);
            }
            else if (property === "LastRates") {
                entity.LastRates = new Array();
                for (var item in jsonPM.LastRates) {
                    var jItem = jsonPM.LastRates[item];
                    var newItemPM = this.MapJsonToEntityList(jItem);
                    entity.LastRates.push(newItemPM);
                }
            }
            else {
                entity[property] = jsonPM[property];
            }
        }
        return entity;
    };
    CurrencyRatesService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CurrencyRatesService);
    return CurrencyRatesService;
}());
exports.CurrencyRatesService = CurrencyRatesService;
var LastRate = /** @class */ (function () {
    function LastRate() {
    }
    return LastRate;
}());
exports.LastRate = LastRate;
var AccountingCurrencyHelper = /** @class */ (function () {
    function AccountingCurrencyHelper() {
    }
    return AccountingCurrencyHelper;
}());
exports.AccountingCurrencyHelper = AccountingCurrencyHelper;
//# sourceMappingURL=CurrencyRatesService.js.map