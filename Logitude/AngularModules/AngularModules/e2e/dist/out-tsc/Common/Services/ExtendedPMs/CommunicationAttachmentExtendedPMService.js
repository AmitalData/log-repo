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
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var CommunicationAttachmentPM_1 = require("../../EntityPMs/CommunicationAttachmentPM");
var CommunicationAttachmentExtendedPMService = /** @class */ (function () {
    function CommunicationAttachmentExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CommunicationAttachmentExtended';
    }
    CommunicationAttachmentExtendedPMService.prototype.getCommunicationAttachmentsByTenant = function (tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var communicationAttachmentPMLists;
            communicationAttachmentPMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                communicationAttachmentPMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = communicationAttachmentPMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    CommunicationAttachmentExtendedPMService.prototype.getCommunicationAttachmentsByCommunicationLogId = function (communicationLogId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?communicationLogId=' + communicationLogId + "&tenant=" + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var communicationAttachmentPMLists;
            communicationAttachmentPMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                communicationAttachmentPMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = communicationAttachmentPMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    CommunicationAttachmentExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new CommunicationAttachmentPM_1.CommunicationAttachmentPM(null);
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    CommunicationAttachmentExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CommunicationAttachmentExtendedPMService);
    return CommunicationAttachmentExtendedPMService;
}());
exports.CommunicationAttachmentExtendedPMService = CommunicationAttachmentExtendedPMService;
//# sourceMappingURL=CommunicationAttachmentExtendedPMService.js.map