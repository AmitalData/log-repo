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
var Observable_1 = require("rxjs/Observable");
var EntityPMServiceResponse_1 = require("../../DataContracts/EntityPMServiceResponse");
var ClassLevelValidator_1 = require("../../Validators/ClassLevelValidator");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var PerformanceLogService = /** @class */ (function () {
    function PerformanceLogService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/performancelogs';
    }
    PerformanceLogService.prototype.insert = function (entity) {
        var _this = this;
        return Observable_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("ErrorLog", entityPM);
            var response;
            response = new EntityPMServiceResponse_1.EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                return _this._http.post(_this._apiUrl, JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                    var result = res.json();
                    response.Result = result;
                    return response;
                });
            }
            else {
                response.HasError = true;
                response.ErrorsArray = errorsArray;
                return Observable_1.Observable.of(response);
            }
        });
    };
    PerformanceLogService.prototype.insertLogsList = function (logs) {
        var _this = this;
        return Observable_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("ErrorLog", entityPM);
            var response;
            response = new EntityPMServiceResponse_1.EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                return _this._http.post(_this._apiUrl + '/PostLogsList', JSON.stringify(logs), { headers: authHeader }).map(function (res) {
                    var result = res.json();
                    response.Result = result;
                    return response;
                });
            }
            else {
                response.HasError = true;
                response.ErrorsArray = errorsArray;
                return Observable_1.Observable.of(response);
            }
        });
    };
    PerformanceLogService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], PerformanceLogService);
    return PerformanceLogService;
}());
exports.PerformanceLogService = PerformanceLogService;
//# sourceMappingURL=PerformanceLogService.js.map