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
var DocumentOutPM_1 = require("../../EntityPMs/DocumentOutPM");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var DocumentOutPMService = /** @class */ (function () {
    function DocumentOutPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/DocumentOutExtended';
    }
    DocumentOutPMService.prototype.GetEntityPartners = function (entityId, objectTableName, childEntityId, childobjectTableName) {
        if (childEntityId === void 0) { childEntityId = ""; }
        if (childobjectTableName === void 0) { childobjectTableName = ""; }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getentitypartners/?' + 'entityId=' + entityId + '&objectTableName=' + objectTableName + '&childEntityId=' + childEntityId + '&childobjectTableName=' + childobjectTableName, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentOutPMService.prototype.getDocumentOutsByEntityIdAndObjectTable = function (entityId, childEntityId, objectTableId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getdocumentoutsbyentityidandobjecttable/?' + 'entityId=' + entityId + '&childEntityId=' + childEntityId + '&objectTableId=' + objectTableId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var DocumentOutPMLists;
            DocumentOutPMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                DocumentOutPMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = DocumentOutPMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentOutPMService.prototype.getCreateDocumentOut = function (documentTypeId, entityId, childEntityId, childReference, objectTableId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getcreatedocumentout/?' + 'documentTypeId=' + documentTypeId + '&entityId=' + entityId + '&childEntityId=' + childEntityId + '&childReference=' + childReference + '&objectTableId=' + objectTableId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            entity = _this.MapJsonToEntityPM(result);
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        //var authHeader = new Headers();
        //authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        //return this._http.get(logitude_url + 'api/DocumentOutExtended' + '?documentTypeId=' + documentTypeId + '&entityId=' + entityId + '&childEntityId=' + childEntityId + '&childReference=' + childReference + '&objectTableId=' + objectTableId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
        //    var result = response.json();
        //    var entity: DocumentOutPM;
        //    entity = this.MapJsonToEntityPM(result);
        //    return entity;
        //});
    };
    DocumentOutPMService.prototype.getSingleDocumentOutPM = function (id, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getsingledocumentoutpm/?' + 'id=' + id + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            entity = _this.MapJsonToEntityPM(result);
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentOutPMService.prototype.GetCalculatedFileNameForDocumentOutCopy = function (documentOutId, documentTypeCopyId) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetCalculatedFileNameForDocumentOutCopy/?' + 'documentOutId=' + documentOutId + '&documentTypeCopyId=' + documentTypeCopyId, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentOutPMService.prototype.putDocumentOut = function (entityPM) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.put(_this._apiUrl, JSON.stringify(entityPM), {
                headers: authHeader,
            }).map(function (response) {
                var result = response.json();
                var entity;
                entity = _this.MapJsonToEntityPM(result);
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = entity;
                return pmresponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DocumentOutPMService.prototype.getDocumentOutByDocumentTypeEntityAndChild = function (entityId, tenant, childEntityId, documentTypeId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getDocumentOutByDocumentTypeEntityAndChild/?' + 'entityId=' + entityId + '&tenant=' + tenant + '&childEntityId=' + childEntityId + '&documentTypeId=' + documentTypeId, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            entity = result;
            if (result) {
                entity = _this.MapJsonToEntityPM(result);
            }
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentOutPMService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new DocumentOutPM_1.DocumentOutPM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    DocumentOutPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], DocumentOutPMService);
    return DocumentOutPMService;
}());
exports.DocumentOutPMService = DocumentOutPMService;
//# sourceMappingURL=DocumentOutPMService.js.map