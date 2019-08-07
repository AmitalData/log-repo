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
var AgentSharedManifestPM_1 = require("../../../Common/EntityPMs/AgentSharedManifestPM");
var PerformanceLogger_1 = require("../../../Infrastructure/Utilities/PerformanceLogger");
var SharedAgentManifestService = /** @class */ (function () {
    function SharedAgentManifestService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/SharedAgentManifest';
    }
    SharedAgentManifestService.prototype.getSharedAgentManifestTransLateIdByCode = function (code, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getSharedAgentManifestTransLateIdByCode?' + 'code=' + code + '&tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var result = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    SharedAgentManifestService.prototype.GetCheckIfAnyShipmentHaveMasterNumber = function (master, longMaster, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCheckIfAnyShipmentHaveMasterNumber?' + 'master=' + master + '&longMaster=' + longMaster + '&tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var result = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    SharedAgentManifestService.prototype.GetCheckIfMasterShipmentHaveHouseWithOtherAgent = function (entityId, agentId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCheckIfMasterShipmentHaveHouseWithOtherAgent?' + 'entityId=' + entityId + '&agentId=' + agentId + '&tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var result = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    SharedAgentManifestService.prototype.ShareAgentManifest = function (shipmentId, isUpdateAgent) {
        var _this = this;
        if (isUpdateAgent === void 0) { isUpdateAgent = false; }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSharedAgentManifest?' + 'shipmentId=' + shipmentId + '&isUpdateAgent=' + isUpdateAgent + '&tenant=' + SessionInfo_1.SessionInfo.LoggedUserTenant, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = _this.MapJsonToEntityPM(pm);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return response.json();
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    SharedAgentManifestService.prototype.getAgentSharedManifesRefShipmentListsByIds = function (agentManifestSharedRefListIds) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.post(_this._apiUrl + '/postagentsharedmanifesrefshipmentListsbyids', JSON.stringify(agentManifestSharedRefListIds), {
                headers: authHeader,
            }).map(function (response) {
                var result = response.json();
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    SharedAgentManifestService.prototype.getAgentSharedManifestsWorkspaceSummary = function () {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetAgentSharedManifestsWorkspaceSummary', { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    SharedAgentManifestService.prototype.GetIsAgentSharedManifests = function (agentId, entityid) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetIsAgentSharedManifests?' + 'agentId=' + agentId + '&entityid=' + entityid, {
                headers: authHeader
            }).map(function (response) {
                var result = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    SharedAgentManifestService.prototype.get = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getsingle?' + 'id=' + id, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = _this.MapJsonToEntityPM(pm);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger_1.PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "AgentSharedManifest", "GetSinglePM", 'id=' + id);
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    SharedAgentManifestService.prototype.GetAgentSharedManifestsForDashBoard = function (lastMonths, lastDays, selectedIndex) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAgentSharedManifestsForDashBoard?' + 'lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&selectedIndex=' + selectedIndex, {
                headers: authHeader
            }).map(function (response) {
                var result = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    SharedAgentManifestService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new AgentSharedManifestPM_1.AgentSharedManifestPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    SharedAgentManifestService.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    SharedAgentManifestService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], SharedAgentManifestService);
    return SharedAgentManifestService;
}());
exports.SharedAgentManifestService = SharedAgentManifestService;
//# sourceMappingURL=SharedAgentManifestService.js.map