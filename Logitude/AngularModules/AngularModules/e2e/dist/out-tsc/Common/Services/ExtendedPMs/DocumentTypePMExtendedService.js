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
var DocumentTypePM_1 = require("../../EntityPMs/DocumentTypePM");
var DocumentTypePMExtendedService = /** @class */ (function () {
    function DocumentTypePMExtendedService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeExtended';
    }
    DocumentTypePMExtendedService.prototype.GetSinglePMWithOutInclude = function (id, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetSinglePMWithOutInclude/?' + 'id=' + id + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity = _this.MapJsonToEntityPM(result);
            var pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypePMExtendedService.prototype.getDocumentTypesByEnityIdAndtransportModeId = function (transportModeId, shipmentLevelCode, objecttableId, tenant, childrenObjectTableIds) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getdocumenttypesbyenityidandtransportmodeid/?' + 'transportModeId=' + transportModeId + '&shipmentLevelCode=' + shipmentLevelCode + '&objecttableId=' + objecttableId + '&tenant=' + tenant + '&childrenObjectTableIds=' + childrenObjectTableIds, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var documentTypePMLists;
            documentTypePMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                documentTypePMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = documentTypePMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypePMExtendedService.prototype.GetFollowUpDocumentTypeByEntityId = function (entityId, objectTableName, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetFollowUpDocumentTypeByEntityId/?' + 'entityId=' + entityId + '&objectTableName=' + objectTableName + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var documentTypePMLists;
            documentTypePMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                documentTypePMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = documentTypePMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypePMExtendedService.prototype.GetShareDocumentByObjectTableAndEntityIdAndshipmentLevel = function (entityId, agentId, agentReference, objecttableId, shipmentLevelCode, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetShareDocumentByObjectTableAndEntityIdAndshipmentLevel/?' + 'entityId=' + entityId + '&agentId=' + agentId + '&agentReference=' + agentReference + '&objecttableId=' + objecttableId + '&shipmentLevelCode=' + shipmentLevelCode + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypePMExtendedService.prototype.GetDocumentTypeCopiesByDocumentTypeId = function (id, documentOutId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetDocumentTypeCopiesByDocumentTypeId/?' + 'id=' + id + '&documentOutId=' + documentOutId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypePMExtendedService.prototype.GetDocumentTypesPMByObjectTableIdForDocumentPremissions = function (objecttableId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetDocumentTypesPMByObjectTableIdForDocumentPremissions/?' + 'objecttableId=' + objecttableId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var documentTypePMLists;
            documentTypePMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                documentTypePMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = documentTypePMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypePMExtendedService.prototype.GetDocumentTypesByObjectTableAndTenant = function (objecttableId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getdocumenttypesbyobjecttableandtenant/?' + 'objecttableId=' + objecttableId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var documentTypePMLists;
            documentTypePMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                documentTypePMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = documentTypePMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypePMExtendedService.prototype.GetDoesDocumentTypeCodeExist = function (code, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getdoesdocumenttypecodeexist/?' + 'code=' + code + '&tenant=' + tenant + '&x=' + 1, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypePMExtendedService.prototype.getSingleDocumentType = function (id, documentOutId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getsingledocumenttype/?' + 'id=' + id + '&documentOutId=' + documentOutId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            entity = _this.MapJsonToEntityPM(result);
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypePMExtendedService.prototype.putDocumentType = function (entityPM) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.put(_this._apiUrl + '/putdocumenttype', JSON.stringify(entityPM), {
                headers: authHeader,
            }).map(function (response) {
                var pm = response.json();
                var entity;
                entity = _this.MapJsonToEntityPM(pm);
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = entity;
                return pmresponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DocumentTypePMExtendedService.prototype.update = function (eventTypePMLists) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.put(_this._apiUrl + '/putupdatedocumenttypepmlists', JSON.stringify(eventTypePMLists), { headers: authHeader }).map(function (res) {
                var pm = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DocumentTypePMExtendedService.prototype.GetDocumentTypeByCode = function (code, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getdocumenttypebycode/?' + 'code=' + code + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            if (result)
                entity = _this.MapJsonToEntityPM(result);
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypePMExtendedService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new DocumentTypePM_1.DocumentTypePM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    DocumentTypePMExtendedService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], DocumentTypePMExtendedService);
    return DocumentTypePMExtendedService;
}());
exports.DocumentTypePMExtendedService = DocumentTypePMExtendedService;
//# sourceMappingURL=DocumentTypePMExtendedService.js.map