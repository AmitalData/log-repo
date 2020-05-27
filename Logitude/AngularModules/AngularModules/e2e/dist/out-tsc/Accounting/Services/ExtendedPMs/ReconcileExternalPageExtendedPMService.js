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
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var ReconcileExternalPageExtendedPMService = /** @class */ (function () {
    function ReconcileExternalPageExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ReconcileExternalPagesExtended';
    }
    ReconcileExternalPageExtendedPMService.prototype.GetBankPageByPageNo = function (pageNumber, bankAccountId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            return Rx_1.Observable.defer(function () {
                return _this._http.get(_this._apiUrl + '/GetBankPageByPageNo?pageNumber=' + pageNumber + '&bankAccountId=' + bankAccountId, { headers: authHeader })
                    .map(function (response) {
                    var res = response.json();
                    return res;
                }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            });
        });
    };
    ReconcileExternalPageExtendedPMService.prototype.GetPrevPageByPageNo = function (pageNumber, bankAccountId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            return Rx_1.Observable.defer(function () {
                return _this._http.get(_this._apiUrl + '/GetPrevPageByPageNo?pageNumber=' + pageNumber + '&bankAccountId=' + bankAccountId, { headers: authHeader })
                    .map(function (response) {
                    var res = response.json();
                    return res;
                }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            });
        });
    };
    ReconcileExternalPageExtendedPMService.prototype.LoadBankPages = function (fileUploadParamerter) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.post(_this._apiUrl + '/PostLoadBankPages', JSON.stringify(fileUploadParamerter), {
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
    ReconcileExternalPageExtendedPMService.prototype.GetDraftPage = function (bankAccountId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            return Rx_1.Observable.defer(function () {
                return _this._http.get(_this._apiUrl + '/GetDraftPage?bankAccountId=' + bankAccountId, { headers: authHeader })
                    .map(function (response) {
                    var res = response.json();
                    return res;
                }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            });
        });
    };
    ReconcileExternalPageExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ReconcileExternalPageExtendedPMService);
    return ReconcileExternalPageExtendedPMService;
}());
exports.ReconcileExternalPageExtendedPMService = ReconcileExternalPageExtendedPMService;
//# sourceMappingURL=ReconcileExternalPageExtendedPMService.js.map