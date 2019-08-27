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
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var TaxReportPM_1 = require("../../EntityPMs/TaxReportPM");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var CustomFieldClass_1 = require("../../../Infrastructure/DataContracts/CustomFieldClass");
var TaxReportExtendedPMService = /** @class */ (function () {
    function TaxReportExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/TaxReportOp';
    }
    TaxReportExtendedPMService.prototype.DownloadPNC874File = function (taxReportPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var mappedEntity;
            mappedEntity = _this.MapJsonToEntityPM(taxReportPM, false);
            return _this._http.post(_this._apiUrl + "/PostDownloadPNC874File", JSON.stringify(mappedEntity), { headers: authHeader })
                .map(function (res) {
                var result = res.json();
                serviceResponse.Result = result;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TaxReportExtendedPMService.prototype.DownloadPNC874FileInBatch = function (taxReportPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var mappedEntity;
            mappedEntity = _this.MapJsonToEntityPM(taxReportPM, false);
            return _this._http.post(_this._apiUrl + "/PostDownloadPNC874FileInBatch", JSON.stringify(mappedEntity), { headers: authHeader })
                .map(function (res) {
                var result = res.json();
                serviceResponse.Result = result;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TaxReportExtendedPMService.prototype.PostCreateTaxReportInBatch = function (taxReportPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var mappedEntity;
            mappedEntity = _this.MapJsonToEntityPM(taxReportPM, false);
            return _this._http.post(_this._apiUrl + "/PostCreateTaxReportInBatch", JSON.stringify(mappedEntity), { headers: authHeader })
                .map(function (res) {
                var result = res.json();
                serviceResponse.Result = result;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TaxReportExtendedPMService.prototype.GetReportLinesCounter = function (taxReportId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + '/GetLinesCounters?taxReportId=' + taxReportId, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TaxReportExtendedPMService.prototype.getErrorsCount = function (reportId) {
        var _this = this;
        var callTime = new Date();
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetErrorsCount/?' + 'reportId=' + reportId, {
                headers: authHeader
            }).map(function (response) {
                var result = response.json();
                return result;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TaxReportExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new TaxReportPM_1.TaxReportPM();
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
    TaxReportExtendedPMService.prototype.clone = function (jsonPM) {
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
    TaxReportExtendedPMService.prototype.GetNewEntityPM = function () {
        var entityPM;
        entityPM = new TaxReportPM_1.TaxReportPM();
        entityPM.Tenant = InfraSettings_1.InfraSettings.TenantPM.Id;
        return entityPM;
    };
    TaxReportExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], TaxReportExtendedPMService);
    return TaxReportExtendedPMService;
}());
exports.TaxReportExtendedPMService = TaxReportExtendedPMService;
//# sourceMappingURL=TaxReportExtendedPMService.js.map