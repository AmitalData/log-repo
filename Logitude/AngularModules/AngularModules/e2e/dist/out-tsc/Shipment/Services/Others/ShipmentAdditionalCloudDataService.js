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
/// <reference path="../../../common/entitypms/agentsharedmanifestpm.ts" />
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var PerformanceLogger_1 = require("../../../Infrastructure/Utilities/PerformanceLogger");
var ShipmentAdditionalCloudDataService = /** @class */ (function () {
    function ShipmentAdditionalCloudDataService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ShipmentAdditionalCloudData';
    }
    ShipmentAdditionalCloudDataService.prototype.get = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getsingle?' + 'id=' + id, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                //var entity: AgentSharedManifestPM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = pm;
                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger_1.PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ShipmentAdditionalCloudData", "GetSinglePM", 'id=' + id);
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentAdditionalCloudDataService.prototype.getsingledata = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSingleData?' + 'id=' + id, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                //var entity: AgentSharedManifestPM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = pm;
                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger_1.PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ShipmentAdditionalCloudData", "GetSinglePM", 'id=' + id);
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentAdditionalCloudDataService.prototype.update = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var response;
            response = new ServiceResponse_1.ServiceResponse();
            //errorsArray = [];
            var shipString;
            shipString = JSON.stringify(entityPM);
            //console.log(shipString);
            return _this._http.put(_this._apiUrl, shipString, { headers: authHeader }).map(function (res) {
                var pm = res.json();
                response.Result = pm;
                return response;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentAdditionalCloudDataService.prototype.getSingleWithoutToken = function (id, Tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        //authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSingleWithoutToken?' + 'id=' + id + '&tenant=' + Tenant, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                //var entity: AgentSharedManifestPM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = pm;
                var servertime = response.headers.get('ServerExecutionTime');
                //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ShipmentAdditionalCloudData", "GetSinglePM", 'id=' + id);
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ShipmentAdditionalCloudDataService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ShipmentAdditionalCloudDataService);
    return ShipmentAdditionalCloudDataService;
}());
exports.ShipmentAdditionalCloudDataService = ShipmentAdditionalCloudDataService;
//# sourceMappingURL=ShipmentAdditionalCloudDataService.js.map