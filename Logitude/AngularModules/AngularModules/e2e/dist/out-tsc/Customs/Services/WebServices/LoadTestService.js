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
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var LoadTestService = /** @class */ (function () {
    function LoadTestService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CustomsLoadTest';
    }
    LoadTestService.prototype.PostCourierBOLRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostCourierBOLRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LoadTestService.prototype.GetNewCustomFile = function (tenant, ConsigneeId, CustomerId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(_this._apiUrl + '/GetNewCustomFile/?' + '&tenant=' + tenant + '&ConsigneeId=' + ConsigneeId + '&CustomerId=' + CustomerId, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LoadTestService.prototype.GetDeclarationFromFileNo = function (tenant, fileNo, FilingCopy) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(_this._apiUrl + '/GetDeclarationFromFileNo/?' + '&tenant=' + tenant + '&fileNo=' + fileNo + '&FilingCopy=' + FilingCopy, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LoadTestService.prototype.GetTicket = function (tenant, declarationId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(_this._apiUrl + '/GetTicket/?' + '&tenant=' + tenant + '&declarationId=' + declarationId, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    LoadTestService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], LoadTestService);
    return LoadTestService;
}());
exports.LoadTestService = LoadTestService;
//# sourceMappingURL=LoadTestService.js.map