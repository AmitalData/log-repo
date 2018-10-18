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
var DocumentTypeTemplatePM_1 = require("../../EntityPMs/DocumentTypeTemplatePM");
var DocumentTypeTemplatePMExtendedService = (function () {
    function DocumentTypeTemplatePMExtendedService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeTemplateExtended';
    }
    DocumentTypeTemplatePMExtendedService.prototype.GetSingleDocumentTypeTemplate = function (id, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getsingledocumenttypetemplate/?' + 'id=' + id + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            entity = _this.MapJsonToEntityPM(result);
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypeTemplatePMExtendedService.prototype.GetDocumentTypeTemplatesPMForDocumentType = function (documentTypeId, templateType, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?documentTypeId=' + documentTypeId + '&templateType=' + templateType + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var DocumentTypeTemplatePMLists;
            DocumentTypeTemplatePMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                DocumentTypeTemplatePMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = DocumentTypeTemplatePMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypeTemplatePMExtendedService.prototype.GetTemplateBodyByDocumentTemplateId = function (documentTypeTemplateId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetTemplateBodyByDocumentTemplateId?documentTypeTemplateId=' + documentTypeTemplateId + "&tenant=" + tenant, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypeTemplatePMExtendedService.prototype.GetTemplateBodyhtmlOrJsonByDocumentTemplateId = function (documentTyptemplateId, tenant, isHtml, pageType) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '?documentTyptemplateId=' + documentTyptemplateId + "&tenant=" + tenant + "&isHtml=" + isHtml + "&pagetype=" + pageType, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    //string documentTypeId, int tenant
    DocumentTypeTemplatePMExtendedService.prototype.getDocumentTypeTemplatesByDocumentTypeIdForAutomations = function (documentTypeId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getdocumenttypetemplatesbydocumenttypeidforautomations/?' + 'documentTypeId=' + documentTypeId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var DocumentTypeTemplatePMLists;
            DocumentTypeTemplatePMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                DocumentTypeTemplatePMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = DocumentTypeTemplatePMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypeTemplatePMExtendedService.prototype.SaveDocumentTemplate = function (filter) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.put(_this._apiUrl + '/PutSaveDocumentTypeTemplate', JSON.stringify(filter), {
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
    DocumentTypeTemplatePMExtendedService.prototype.ConvertXmalByteTojosnObject = function (filter) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.put(_this._apiUrl + '/PutConvertXmalByteTojosnObject', JSON.stringify(filter), {
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
    DocumentTypeTemplatePMExtendedService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new DocumentTypeTemplatePM_1.DocumentTypeTemplatePM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    return DocumentTypeTemplatePMExtendedService;
}());
DocumentTypeTemplatePMExtendedService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [])
], DocumentTypeTemplatePMExtendedService);
exports.DocumentTypeTemplatePMExtendedService = DocumentTypeTemplatePMExtendedService;
//# sourceMappingURL=DocumentTypeTemplatePMExtendedService.js.map