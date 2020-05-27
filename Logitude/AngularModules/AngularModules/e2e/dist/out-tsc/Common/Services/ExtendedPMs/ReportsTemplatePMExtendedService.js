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
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ReportsTemplatePM_1 = require("../../EntityPMs/ReportsTemplatePM");
var CustomFieldClass_1 = require("../../../Infrastructure/DataContracts/CustomFieldClass");
var ReportsTemplatePMExtendedService = /** @class */ (function () {
    function ReportsTemplatePMExtendedService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ReportsTemplateExtended';
    }
    ReportsTemplatePMExtendedService.prototype.GetReportsTemplatePMsByReportId = function (reportId, reportType) {
        var _this = this;
        if (reportType === void 0) { reportType = ""; }
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetReportsTemplatePMsByReportId" + '?reportId=' + reportId + "&reportType=" + reportType, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var entity;
            var ReportsTemplatePMLists;
            ReportsTemplatePMLists = new Array();
            result.forEach(function (item) {
                entity = _this.MapJsonToEntityPM(item);
                ReportsTemplatePMLists.push(entity);
            });
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = ReportsTemplatePMLists;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    ReportsTemplatePMExtendedService.prototype.GetCopyReportsTemplate = function (reportsTemplateId, userId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetCopyReportsTemplateByReportsTemplateId" + '?reportsTemplateId=' + reportsTemplateId + "&userId=" + userId, { headers: authHeader }).map(function (response) {
            var entity;
            var result = response.json();
            entity = _this.MapJsonToEntityPM(result);
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    ReportsTemplatePMExtendedService.prototype.GetReportTemplateEditorHtmlData = function (reportsTemplateId, version, userId, subject, from, replyTo, cc) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetReportTemplateEditorHtmlData" + '?reportsTemplateId=' + reportsTemplateId + "&version=" + version + "&userId=" + userId + "&subject=" + subject + "&from=" + from + "&replyTo=" + replyTo + "&cc=" + cc, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    ReportsTemplatePMExtendedService.prototype.CreateReportTemplate = function (reportsTemplatePM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var errorsArray = [];
            var response;
            response = new ServiceResponse_1.ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(reportsTemplatePM, false);
                return _this._http.post(_this._apiUrl + '/PostReportTemplate', JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult;
                        mappedResult = _this.MapJsonToEntityPM(pm, true, reportsTemplatePM);
                        response.Result = mappedResult;
                    }
                    return response;
                });
            }
            else {
                response.HasError = true;
                response.ErrorsArray = errorsArray;
                return Rx_1.Observable.of(response);
            }
        });
    };
    ReportsTemplatePMExtendedService.prototype.GetMessageReportsTemplateBodyByReportTemplateIdAndVersion = function (reportsTemplateId, version) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetMessageReportsTemplateBodyByReportTemplateIdAndVersion" + '?reportsTemplateId=' + reportsTemplateId + "&version=" + version, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    ReportsTemplatePMExtendedService.prototype.SaveReportTemplateMessageBody = function (reportsTemplatePM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var errorsArray = [];
            var response;
            response = new ServiceResponse_1.ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(reportsTemplatePM, false);
                return _this._http.put(_this._apiUrl + '/PutSaveReportTemplateMessageBody', JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult;
                        mappedResult = _this.MapJsonToEntityPM(pm, true, reportsTemplatePM);
                        response.Result = mappedResult;
                    }
                    return response;
                }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            }
            else {
                response.HasError = true;
                response.ErrorsArray = errorsArray;
                return Rx_1.Observable.of(response);
            }
        });
    };
    ReportsTemplatePMExtendedService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new ReportsTemplatePM_1.ReportsTemplatePM();
        }
        var customFields = [];
        for (var i = 1; i < 11; i++) {
            customFields.push("Field" + i);
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            if (customFields.indexOf(property) > -1) {
                if (jsonPM[property]) {
                    var customFieldClass = new CustomFieldClass_1.CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
        }
        else {
            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    ReportsTemplatePMExtendedService.prototype.clone = function (jsonPM) {
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
    ReportsTemplatePMExtendedService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ReportsTemplatePMExtendedService);
    return ReportsTemplatePMExtendedService;
}());
exports.ReportsTemplatePMExtendedService = ReportsTemplatePMExtendedService;
//# sourceMappingURL=ReportsTemplatePMExtendedService.js.map