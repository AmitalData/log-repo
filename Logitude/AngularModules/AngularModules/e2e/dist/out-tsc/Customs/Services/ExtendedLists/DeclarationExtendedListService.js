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
var DeclarationList_1 = require("../../EntityLists/DeclarationList");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var DeclarationExtendedListService = /** @class */ (function () {
    function DeclarationExtendedListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/Declarartion';
    }
    DeclarationExtendedListService.prototype.GetSingleDeclarationByCustomFileNo = function (customFileNo) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSingleDeclarationByCustomFileNo/?' + 'customFileNo=' + customFileNo, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var declarationList;
                if (serviceResponse.Result) {
                    var entity;
                    declarationList = entity = _this.MapJsonToEntityList(serviceResponse.Result);
                }
                serviceResponse.Result = declarationList;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationExtendedListService.prototype.GetSupplierInvoiceItemsCount = function (declarationId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSupplierInvoiceItemCount/?' + 'declarationId=' + declarationId, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationExtendedListService.prototype.GetSingleDeclarationByNumber = function (declarationByNumber, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetSingleDeclarationByCustomFileNo';
        return Rx_1.Observable.defer(function () {
            return _this._http
                .get(_this._apiUrl + '/GetSingleDeclarationByNumber/?' + 'declarationByNumber=' + declarationByNumber + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var declarationList;
                if (serviceResponse.Result) {
                    var entity;
                    declarationList = entity = _this.MapJsonToEntityList(serviceResponse.Result);
                }
                serviceResponse.Result = declarationList;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationExtendedListService.prototype.GetConsignmentListPMByCustomFileNo = function (customFileNo) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetConsignmentListPMByCustomFileNo';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetConsignmentListPMByCustomFileNo/?' + 'customFileNo=' + customFileNo, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationExtendedListService.prototype.GetCurrenciesCodesForDeclaration = function (declarationId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        //var url = this._apiUrl + '/GetConsignmentListPMByCustomFileNo';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCurrenciesCodesForDeclaration/?' + 'declarationId=' + declarationId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationExtendedListService.prototype.GetSingleDeclarationPMByCargoIdentifiers = function (cargoTypeCode, manifestNumber, secondCargoID, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        //var url = this._apiUrl + '/GetConsignmentListPMByCustomFileNo';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSingleDeclarationPMByCargoIdentifiers/?' + 'cargoTypeCode=' + cargoTypeCode + '&manifestNumber=' + manifestNumber + '&secondCargoID=' + secondCargoID + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationExtendedListService.prototype.PutCopyDeclaration = function (fromDeclarationId, toDeclarationId, tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.put(_this._apiUrl + '/PutCopyDeclaration/?' + 'fromDeclarationId=' + fromDeclarationId + '&toDeclarationId=' + toDeclarationId + '&tenant=' + tenant, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationExtendedListService.prototype.GetDeclarationByCustomFileNoAndCCU = function (customFileNo) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDeclarationByCustomFileNoAndCCU/?' + 'customFileNo=' + customFileNo, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                var declarationList;
                if (serviceResponse.Result) {
                    var entity;
                    declarationList = entity = _this.MapJsonToEntityList(serviceResponse.Result);
                }
                serviceResponse.Result = declarationList;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DeclarationExtendedListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new DeclarationList_1.DeclarationList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    DeclarationExtendedListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], DeclarationExtendedListService);
    return DeclarationExtendedListService;
}());
exports.DeclarationExtendedListService = DeclarationExtendedListService;
//# sourceMappingURL=DeclarationExtendedListService.js.map