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
var DocumentTypeTemplateListExtendedService = /** @class */ (function () {
    function DocumentTypeTemplateListExtendedService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeTemplateExtended';
    }
    DocumentTypeTemplateListExtendedService.prototype.getDocumentTypeTemplateListsForDocumentType = function (documentTypeId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getdocumenttypetemplatelistsfordocumenttype/?' + 'documentTypeId=' + documentTypeId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            //return this._http.get(this._apiUrl + '?documentTypeId=' + documentTypeId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypeTemplateListExtendedService.prototype.GetDocumentTypeTemplatesFromLibraryByDocumentTypeId = function (tenant, documentTypeId, isfilter, mytenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getdocumentTypetemplatesfromlibrarybydocumenttypeid/?' + 'tenant=' + tenant + '&documentTypeId=' + documentTypeId + '&isfilter=' + isfilter + '&mytenant=' + mytenant, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypeTemplateListExtendedService.prototype.GetDocumentTypeTemplatesFromLibrary = function (objecttableid, tenant, isfilter, transportModeId, shipmentLevelCode) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getdocumentTypetemplatesfromlibrary/?' + 'objecttableid=' + objecttableid + '&tenant=' + tenant + '&isfilter=' + isfilter + '&transportModeId=' + transportModeId + '&shipmentLevelCode=' + shipmentLevelCode, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypeTemplateListExtendedService.prototype.CopyDocumentTypeAndDocumentTypTemplate = function (docmentTypeTemplateId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var x = 1;
        return this._http.get(this._apiUrl + '/getcopydocumenttypeanddocumenttyptemplate/?' + 'docmentTypeTemplateId=' + docmentTypeTemplateId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    DocumentTypeTemplateListExtendedService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], DocumentTypeTemplateListExtendedService);
    return DocumentTypeTemplateListExtendedService;
}());
exports.DocumentTypeTemplateListExtendedService = DocumentTypeTemplateListExtendedService;
//# sourceMappingURL=DocumentTypeTemplateListExtendedService.js.map