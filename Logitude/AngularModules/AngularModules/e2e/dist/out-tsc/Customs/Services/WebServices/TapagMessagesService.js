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
var DepositPMService_1 = require("../StandardPMs/DepositPMService");
var DeficitPMService_1 = require("../StandardPMs/DeficitPMService");
var DeficitConnFileParagraphTypeListService_1 = require("../StandardLists/DeficitConnFileParagraphTypeListService");
var GuaranteePMService_1 = require("../StandardPMs/GuaranteePMService");
var TapagMessagesService = /** @class */ (function () {
    function TapagMessagesService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/TapagMessages';
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/TapagMessages';
    }
    TapagMessagesService.prototype.PostGuaranteeCertificateRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostGuaranteeCertificateRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TapagMessagesService.prototype.PostFaultQueryRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostFaultQueryRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TapagMessagesService.prototype.PostGuaranteeFileFilterQueryRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostGuaranteeFileFilterQueryRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TapagMessagesService.prototype.GetDeclarationTapagsLists = function (declarationId, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetDeclarationTapagsLists/?declarationId=" + declarationId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(function (response) {
                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TapagMessagesService.prototype.GetDepositPMByPaymentOrderNumberOrTapagId = function (paymentNumber, tapagId, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var depositPMService = new DepositPMService_1.DepositPMService();
            return _this._http.get(_this._apiUrl + "/GetDepositPMByPaymentOrderNumberOrTapagId/?paymentNumber=" + paymentNumber + "&tapagId=" + tapagId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = depositPMService.MapJsonToEntityPM(pm);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TapagMessagesService.prototype.GetDeficitPMByPaymentOrderNumberOrTapagId = function (paymentNumber, tapagId, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var deficitPMService = new DeficitPMService_1.DeficitPMService();
            return _this._http.get(_this._apiUrl + "/GetDeficitPMByPaymentOrderNumberOrTapagId/?paymentNumber=" + paymentNumber + "&tapagId=" + tapagId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = deficitPMService.MapJsonToEntityPM(pm);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TapagMessagesService.prototype.GetSingleTapagList = function (tapagId, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetSingleTapagList/?id=" + tapagId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(function (response) {
                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TapagMessagesService.prototype.GetGuaranteeByTapagId = function (tapagId, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var guaranteePMService = new GuaranteePMService_1.GuaranteePMService();
            return _this._http.get(_this._apiUrl + "/GetGuaranteeByTapagId/?tapagId=" + tapagId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = guaranteePMService.MapJsonToEntityPM(pm);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TapagMessagesService.prototype.GetDeficitConnectedFileParagraphTypeList = function (declarationId, deficitId, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var deficitConnFileParagraphTypeListService = new DeficitConnFileParagraphTypeListService_1.DeficitConnFileParagraphTypeListService();
            return _this._http.get(_this._apiUrl + "/GetDeficitConnectedFileParagraphTypeList/?declarationId=" + declarationId + "&deficitId=" + deficitId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(function (response) {
                //var pm = response.json();
                //var entity: DeficitConnFileParagraphTypeList;
                //if (pm) {
                //    entity = deficitConnFileParagraphTypeListService.MapJsonToEntityList(pm);
                //}
                //var serviceResponse: ServiceResponse = new ServiceResponse();
                //serviceResponse.Result = entity;
                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TapagMessagesService.prototype.PostDeclarationFilterRequestParams = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostDeclarationFilterRequestParams/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TapagMessagesService.prototype.PostBankAccountToRefundQueryRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostBankAccountToRefundQueryRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    TapagMessagesService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], TapagMessagesService);
    return TapagMessagesService;
}());
exports.TapagMessagesService = TapagMessagesService;
//# sourceMappingURL=TapagMessagesService.js.map