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
var LogBoxSignatureClientService = /** @class */ (function () {
    function LogBoxSignatureClientService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/LogBoxSignatureClient';
    }
    LogBoxSignatureClientService.prototype.GetSignRequestReceived = function (entityPM) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        authHeader.append('Content-Type', 'application/json');
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.put(_this._apiUrl, JSON.stringify(entityPM), {
                headers: authHeader
            }).map(function (response) {
                //var pm = response.json();
                //var entity: AgentSharedManifestPM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}
                var entity = response.json();
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = entity;
                return pmresponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LogBoxSignatureClientService.prototype.GetMultiSignRequestReceived = function (Ids) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            // Prepare parameters
            var IdsParameterString = "";
            if (Ids && Ids.length > 0) {
                Ids.forEach(function (el) {
                    IdsParameterString += 'Ids=' + el + '&';
                });
            }
            else {
                console.log("[ERROR] cannot Archive Shipments without Ids!", Ids);
                return;
            }
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetMultiSignRequestReceived/?" + IdsParameterString, { headers: authHeader }).map(function (response) {
                //var res = response.json();
                var entity = response.json();
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = entity;
                return pmresponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LogBoxSignatureClientService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], LogBoxSignatureClientService);
    return LogBoxSignatureClientService;
}());
exports.LogBoxSignatureClientService = LogBoxSignatureClientService;
//# sourceMappingURL=LogBoxSignatureClientService.js.map