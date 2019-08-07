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
var CustomsExchangeRatePM_1 = require("../../EntityPMs/CustomsExchangeRatePM");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var CustomsExchangeRateExtendedPMService = /** @class */ (function () {
    function CustomsExchangeRateExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CustomsExchangeRatesExtended';
    }
    CustomsExchangeRateExtendedPMService.prototype.GetCustomsExchangeRateForCurrencyAndDate = function (currencyTypeCodes, date) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCustomsExchangeRateForCurrencyAndDate/?' + 'currencyTypeCode=' + currencyTypeCodes + '&date=' + date, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity;
                        entity = _this.MapJsonToEntityPM(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CustomsExchangeRateExtendedPMService.prototype.GetCustomsExchangeRateForDate = function (date) {
        var _this = this;
        var authHeader = new http_1.Headers();
        var stringDate = date;
        if (date instanceof Object) {
            stringDate = date.toJSON();
        }
        ///var stringDate = date.toJSON();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCustomsExchangeRateForDate/?' + 'date=' + stringDate, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity;
                        entity = _this.MapJsonToEntityPM(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CustomsExchangeRateExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new CustomsExchangeRatePM_1.CustomsExchangeRatePM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    CustomsExchangeRateExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CustomsExchangeRateExtendedPMService);
    return CustomsExchangeRateExtendedPMService;
}());
exports.CustomsExchangeRateExtendedPMService = CustomsExchangeRateExtendedPMService;
//# sourceMappingURL=CustomsExchangeRateExtendedPMService.js.map