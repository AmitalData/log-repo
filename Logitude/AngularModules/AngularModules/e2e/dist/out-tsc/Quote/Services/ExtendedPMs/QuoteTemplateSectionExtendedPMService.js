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
var QuoteTemplateSectionPM_1 = require("../../EntityPMs/QuoteTemplateSectionPM");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var QuoteTemplateSectionExtendedPMService = /** @class */ (function () {
    function QuoteTemplateSectionExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/QuoteTemplateSectionExtended';
    }
    QuoteTemplateSectionExtendedPMService.prototype.GetQuoteTemplateSectionByQuoteTemplateId = function (quoteTemplateId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplateSectionByQuoteTemplateId/?' + 'quoteTemplateId=' + quoteTemplateId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var quoteTemplateSectionPMLists;
            quoteTemplateSectionPMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                quoteTemplateSectionPMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = quoteTemplateSectionPMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateSectionExtendedPMService.prototype.updateSections = function (quoteTemplateSections) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.put(_this._apiUrl + '/PutQuoteTemplateSections', JSON.stringify(quoteTemplateSections), { headers: authHeader }).map(function (res) {
                var pm = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    // GetDownloadQuoteTemplateSectionPdfFile(string sectionTypeCode, string sectionDocId, string quoteTemplateId, int tenant, string settingId, string quoteId)
    QuoteTemplateSectionExtendedPMService.prototype.DownloadQuoteTemplateSectionPdfFile = function (sectionTypeCode, sectionDocId, quoteTemplateId, settingId, quoteId, userId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetDownloadQuoteTemplateSectionPdfFile/?' + 'sectionTypeCode=' + sectionTypeCode + '&sectionDocId=' + sectionDocId + '&quoteTemplateId=' + quoteTemplateId + '&settingId=' + settingId + '&quoteId=' + quoteId + '&userId=' + userId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateSectionExtendedPMService.prototype.GetQuoteTemplatePdfReport = function (quoteId, quoteTemplateId, userId, isFromLibrary) {
        if (isFromLibrary === void 0) { isFromLibrary = false; }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplatePdfReport/?' + 'quoteId=' + quoteId + '&quoteTemplateId=' + quoteTemplateId + '&userId=' + userId + '&isFromLibrary=' + isFromLibrary, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateSectionExtendedPMService.prototype.GetMakeQuoteTemplateSectionsIncluded = function (quoteId, quotetemplateId, quotetemplatesectionId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetMakeQuoteTemplateSectionsIncluded/?' + 'quoteId=' + quoteId + '&quotetemplateId=' + quotetemplateId + '&quotetemplatesectionId=' + quotetemplatesectionId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateSectionExtendedPMService.prototype.GetMakeQuoteTemplateSectionsExcluded = function (quoteId, quotetemplateId, quotetemplatesectionId, tenant) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetMakeQuoteTemplateSectionsExcluded/?' + 'quoteId=' + quoteId + '&quotetemplateId=' + quotetemplateId + '&quotetemplatesectionId=' + quotetemplatesectionId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    QuoteTemplateSectionExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new QuoteTemplateSectionPM_1.QuoteTemplateSectionPM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    QuoteTemplateSectionExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], QuoteTemplateSectionExtendedPMService);
    return QuoteTemplateSectionExtendedPMService;
}());
exports.QuoteTemplateSectionExtendedPMService = QuoteTemplateSectionExtendedPMService;
//# sourceMappingURL=QuoteTemplateSectionExtendedPMService.js.map