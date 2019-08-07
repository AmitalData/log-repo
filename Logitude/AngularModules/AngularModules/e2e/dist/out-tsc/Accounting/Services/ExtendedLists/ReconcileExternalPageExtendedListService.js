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
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var LedgerTransactionList_1 = require("../../EntityLists/LedgerTransactionList");
var ReconcileExternalPageExtendedListService = /** @class */ (function () {
    function ReconcileExternalPageExtendedListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ReconcileExternalPagesExtended';
    }
    ReconcileExternalPageExtendedListService.prototype.getExternalReoncilioationsByFilter = function (bankAccountId, filters) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + "/getExternalReoncilioationsByFilter";
        var urlparameters = '?bankAccountId=' + bankAccountId;
        // Parse Filters into URI
        var mykeys = Object.keys(filters);
        var addtionalFiltersValues = null;
        var callTime = new Date();
        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = filters[propName];
            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");
            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }
            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }
            if (propName == "AdditionalFilters" && propValue.length > 0)
                addtionalFiltersValues = JSON.stringify(propValue);
        }
        if (addtionalFiltersValues) {
            urlparameters = urlparameters.concat("&AdditionalFilters=").concat(addtionalFiltersValues);
        }
        // End Parse
        var callUrl = url.concat(urlparameters);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(callUrl, {
                headers: authHeader
            }).map(function (response) {
                var serviceResponse;
                serviceResponse = response.json();
                console.log("serviceResponse: ", serviceResponse);
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ReconcileExternalPageExtendedListService.prototype.getBankPageLinesByIds = function (Ids) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var params = "";
        for (var _i = 0, Ids_1 = Ids; _i < Ids_1.length; _i++) {
            var id = Ids_1[_i];
            params += "Ids[]=" + id + "&";
        }
        var url = this._apiUrl + '/getBankPageLinesByIds?' + params;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ReconcileExternalPageExtendedListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new LedgerTransactionList_1.LedgerTransactionList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    ReconcileExternalPageExtendedListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ReconcileExternalPageExtendedListService);
    return ReconcileExternalPageExtendedListService;
}());
exports.ReconcileExternalPageExtendedListService = ReconcileExternalPageExtendedListService;
//# sourceMappingURL=ReconcileExternalPageExtendedListService.js.map