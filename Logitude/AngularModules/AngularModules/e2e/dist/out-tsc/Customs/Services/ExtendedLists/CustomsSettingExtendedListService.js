"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var CustomsSettingList_1 = require("../../EntityLists/CustomsSettingList");
var CustomsSettingExtendedListService = /** @class */ (function () {
    function CustomsSettingExtendedListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CustomsSettingExtended';
    }
    CustomsSettingExtendedListService.prototype.GetSettingByTenant = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSettingByTenant/?', { headers: authHeader }).map(function (response) {
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
    CustomsSettingExtendedListService.prototype.GetAmitalRestrictOwnerModel = function (getFromCache) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAmitalRestrictOwnerModel/?getFromCache=' + getFromCache.toString(), { headers: authHeader }).map(function (response) {
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
    CustomsSettingExtendedListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new CustomsSettingList_1.CustomsSettingList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    CustomsSettingExtendedListService.prototype.GetSkipAutoInsurancePromise = function (customerCode, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSkipAutoInsurance/?customerCode=' + customerCode.toString() + '&tenant=' + tenant, { headers: authHeader })
                .map(function (response) {
                var obj = response.json();
                //var entity: CustomsSettingList;
                //if (list) {
                //    entity = this.MapJsonToEntityList(list);
                //}
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = obj;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CustomsSettingExtendedListService.prototype.GetInsurancePercentDefault = function (customerCode, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetInsurancePercentDefault/?customerCode=' + customerCode.toString() + '&tenant=' + tenant, { headers: authHeader })
                .map(function (response) {
                var obj = response.json();
                //var entity: CustomsSettingList;
                //if (list) {
                //    entity = this.MapJsonToEntityList(list);
                //}
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = obj;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CustomsSettingExtendedListService.prototype.GetDefault = function (DISTRID, DEFID, BRANCHID, CARDID, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        DISTRID = DISTRID || "ISRAEL";
        BRANCHID = BRANCHID || "NON";
        CARDID = CARDID || "NON";
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDefault/?' +
                'DISTRID=' + DISTRID.toString() +
                '&DEFID=' + DEFID.toString() +
                '&BRANCHID=' + BRANCHID.toString() +
                '&CARDID=' + CARDID.toString() +
                '&tenant=' + tenant, { headers: authHeader })
                .map(function (response) {
                var obj = response.json();
                //var entity: CustomsSettingList;
                //if (list) {
                //    entity = this.MapJsonToEntityList(list);
                //}
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = obj;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    return CustomsSettingExtendedListService;
}());
exports.CustomsSettingExtendedListService = CustomsSettingExtendedListService;
//# sourceMappingURL=CustomsSettingExtendedListService.js.map