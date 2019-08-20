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
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var EntityChangePM_1 = require("../../EntityPMs/EntityChangePM");
var EntityChangeExtendedPMService = /** @class */ (function () {
    function EntityChangeExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/EntityChangeExtended';
    }
    EntityChangeExtendedPMService.prototype.getEntityChangePMsByEntityIdAndObjectTable = function (entityId, objectTableId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/getentitychangepmsbyentityidandobjecttable" + '?entityId=' + entityId + '&objectTableId=' + objectTableId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    EntityChangeExtendedPMService.prototype.getEntityChangeAutomationsSummaryByEntityChangeId = function (entitychangeId, objectTableName, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/getentitychangeautomationssummarybyentitychangeId" + '?entitychangeId=' + entitychangeId + '&objectTableName=' + objectTableName + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    EntityChangeExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new EntityChangePM_1.EntityChangePM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    EntityChangeExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], EntityChangeExtendedPMService);
    return EntityChangeExtendedPMService;
}());
exports.EntityChangeExtendedPMService = EntityChangeExtendedPMService;
//# sourceMappingURL=EntityChangeExtendedPMService.js.map