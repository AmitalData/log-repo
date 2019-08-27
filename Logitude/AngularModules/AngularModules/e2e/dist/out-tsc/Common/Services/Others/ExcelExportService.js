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
var ExcelExportService = /** @class */ (function () {
    function ExcelExportService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ExcelExport';
    }
    ExcelExportService.prototype.ExportRoleFeaturesToCSVFile = function () {
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ExcelExport';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getexportrolefeaturestocsvfile', { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    ExcelExportService.prototype.ExportFeaturesToCSVFile = function () {
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ExcelExportFeatures';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetExportFeaturesToCSVFile', { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    ExcelExportService.prototype.ImportFeaturePackages = function (parameter) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ExcelExportFeatures';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.post(_this._apiUrl + '/postImportFeaturePackages', JSON.stringify(parameter), {
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
    ExcelExportService.prototype.ImportClockTimeData = function (parameter) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ClockTimeGeneralDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.post(_this._apiUrl + '/postImportingClockTimeData', JSON.stringify(parameter), {
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
    ExcelExportService.prototype.ImportRoleFeatures = function (parameter) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ExcelExport';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.post(_this._apiUrl + '/postimportrolefeatures', JSON.stringify(parameter), {
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
    ExcelExportService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ExcelExportService);
    return ExcelExportService;
}());
exports.ExcelExportService = ExcelExportService;
//# sourceMappingURL=ExcelExportService.js.map