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
var IIGGeneralMessagesService = /** @class */ (function () {
    function IIGGeneralMessagesService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/IIGGeneralMessages';
    }
    IIGGeneralMessagesService.prototype.PostMorningMessages = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostMorningMessages/', JSON.stringify(entity), { headers: authHeader })
                .map(function (res) {
                //               serviceResponse.Result = res.json();
                //             return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            ;
        });
    };
    IIGGeneralMessagesService.prototype.PostCourierBOLRequest = function (entity) {
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
    IIGGeneralMessagesService.prototype.PostMasterBOLRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostMasterBOLRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostCreditQueryRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostCreditQueryRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostGoldCreditQueryRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostGoldCreditQueryRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostSpecialActivityRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostSpecialActivityRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostImporterDeclarationRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostImporterDeclarationRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.GetClientProgressBarIndicatorCurrentStage = function (tenant, CustomsRequestsSheetId, BasicResponse) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(_this._apiUrl + '/GetClientProgressBarIndicatorCurrentStage/?' + '&BasicResponse=' + BasicResponse + '&tenant=' + tenant + '&CustomsRequestsSheetId=' + CustomsRequestsSheetId, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostExchangeRatesQuery = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostExchangeRatesQuery/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostCustomItemLegalDemandsQuery = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostCustomItemLegalDemandsQuery/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostUpdateClosedTables = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostUpdateClosedTables/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostFillNotExistedClosedTables = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostFillNotExistedClosedTables/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostVehicleRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostVehicleRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostCustomFileCredit = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostCustomFileCredit/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.GetDefBankForCustomer = function (customerCode, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + '/GetDefBankForCustomer/?' + 'customerCode=' + customerCode + '&tenant=' + tenant.toString(), 
            //JSON.stringify(entity),
            { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostChangingTimeRequestParams = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostChangingTimeRequestParams/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.GetResetDeclarationNumber = function (declarationId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(_this._apiUrl + '/GetResetDeclarationNumber/?' + '&declarationId=' + declarationId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse.Result = response.json();
                serviceResponse.Result = response;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostCargoQueryRequestParams = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostCargoQueryRequestParams/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostMessageRestoreRequestParams = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostMessageRestoreRequestParams/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostBlockListInWarehouseRequestParams = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostBlockListInWarehouseRequestParams/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.GetLOGISUPPACC = function (declarationId, InvoiceCounterKey, LineNumber, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        //GetLOGISUPPACC(string declarationId, int InvoiceCounterKey, int LineNumber, int tenant)
        return Rx_1.Observable.defer(function () {
            return _this._http
                //GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId)
                .get(_this._apiUrl + '/GetLOGISUPPACC/?' +
                '&declarationId=' + declarationId +
                '&InvoiceCounterKey=' + InvoiceCounterKey +
                '&LineNumber=' + LineNumber +
                '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                var myObj = response.json();
                serviceResponse.Result = myObj.MyXML;
                //serviceResponse.Result = (response as any)._body;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostCustomsBookInRequestParams = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostCustomsBookInRequestParams/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostDeficitFileFilterRequestParams = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostDeficitFileFilterRequestParams/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService.prototype.PostTPG_NG_8244_ClaimFileFilterRequestParams = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostTPG_NG_8244_ClaimFileFilterRequestParams/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    IIGGeneralMessagesService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], IIGGeneralMessagesService);
    return IIGGeneralMessagesService;
}());
exports.IIGGeneralMessagesService = IIGGeneralMessagesService;
////////////////////////////////////////////////
var ResultClientProgressBar = /** @class */ (function () {
    function ResultClientProgressBar() {
    }
    return ResultClientProgressBar;
}());
exports.ResultClientProgressBar = ResultClientProgressBar;
////////////////////////////////////////
//# sourceMappingURL=IIGGeneralMessagesService.js.map