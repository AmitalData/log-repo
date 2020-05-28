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
require("rxjs/add/operator/map");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ExportDocumentService = /** @class */ (function () {
    function ExportDocumentService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ExportDocument';
    }
    ExportDocumentService.prototype.getDocumentPdfFile = function (documentTypeId, entityId, entityObjectTableId, childEntityId, childObjectTableId, documentOutId, tenant, documentTypeCopyId, userId) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.get(this._apiUrl + '?documentTypeId=' + documentTypeId + '&entityId=' + entityId + '&entityObjectTableId=' + entityObjectTableId + '&childEntityId=' + childEntityId + '&childObjectTableId=' + childObjectTableId + '&documentOutId=' + documentOutId + '&tenant=' + tenant + '&documentTypeCopyId=' + documentTypeCopyId + '&userId=' + userId, {
            headers: authHeader,
        })
            .map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    ExportDocumentService.prototype.PostReportStimulsoftViewer = function (filter) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.post(_this._apiUrl + '/postreportstimulsoftviewer', JSON.stringify(filter), {
                headers: authHeader,
            }).map(function (response) {
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = response.json();
                return pmresponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ExportDocumentService.prototype.GetUsedSpaceForTenant = function (tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.get(this._apiUrl + '?tenant=' + tenant, {
            headers: authHeader,
        }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    ExportDocumentService.prototype.GetResetEditableFields = function (documentoOutId) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.get(this._apiUrl + "/GetResetEditableFields" + '?documentoOutId=' + documentoOutId, {
            headers: authHeader,
        }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    ExportDocumentService.prototype.DownloadFileFromServer = function (documentId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.get(this._apiUrl + '?documentId=' + documentId + '&tenant=' + tenant, {
            headers: authHeader,
        }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    ExportDocumentService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ExportDocumentService);
    return ExportDocumentService;
}());
exports.ExportDocumentService = ExportDocumentService;
//# sourceMappingURL=ExportDocumentService.js.map