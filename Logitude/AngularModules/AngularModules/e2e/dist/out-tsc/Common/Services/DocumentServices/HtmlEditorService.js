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
var HtmlEditorService = /** @class */ (function () {
    function HtmlEditorService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/HtmlEditor';
    }
    HtmlEditorService.prototype.getEditorHtmlData = function (docOutId, entityId, objecttableId, childEntityId, childEntityObjectTableId, tenant, userId, theIsSendMail, documentTemplateId, subject, mode, from, replyTo, cc) {
        if (mode === void 0) { mode = null; }
        if (from === void 0) { from = null; }
        if (replyTo === void 0) { replyTo = null; }
        if (cc === void 0) { cc = null; }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.get(this._apiUrl + '?docOutId=' + docOutId + '&entityId=' + entityId + '&objecttableId=' + objecttableId + '&childEntityId=' + childEntityId + '&childEntityObjectTableId=' + childEntityObjectTableId + '&tenant=' + tenant + '&userId=' + userId + '&theIsSendMail=' + theIsSendMail + '&documentTemplateId=' + documentTemplateId + '&subject=' + subject + "&mode=" + mode + "&from=" + from + "&replyTo=" + replyTo + "&cc=" + cc, {
            headers: authHeader,
        })
            .map(function (result) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    HtmlEditorService.prototype.getSentMessageHtmlBody = function (documentId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.get(this._apiUrl + '?documentId=' + documentId + '&tenant=' + tenant, { headers: authHeader, }).map(function (result) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    HtmlEditorService.prototype.sendDocumentHtml = function (sendHtmlFilter) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.post(_this._apiUrl + '/postsendhtmldocument', JSON.stringify(sendHtmlFilter), {
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
    HtmlEditorService.prototype.saveEditedReportToServer = function (filters) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.put(_this._apiUrl + '/putsaveeditedreporttoserver', JSON.stringify(filters), {
                headers: authHeader,
            }).map(function (response) {
                var pmresponse;
                pmresponse = new ServiceResponse_1.ServiceResponse();
                pmresponse.Result = response.json();
                return pmresponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    HtmlEditorService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], HtmlEditorService);
    return HtmlEditorService;
}());
exports.HtmlEditorService = HtmlEditorService;
//# sourceMappingURL=HtmlEditorService.js.map