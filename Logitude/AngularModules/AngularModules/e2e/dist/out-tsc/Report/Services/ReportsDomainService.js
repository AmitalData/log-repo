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
require("rxjs/add/operator/map");
require("rxjs/add/operator/catch");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var ParticipantList_1 = require("../EntityLists/ParticipantList");
var ReportsDomainService = /** @class */ (function () {
    function ReportsDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain';
    }
    ReportsDomainService.prototype.GetActivityStatus = function (currentTenant) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetIQueryableEntityList?tenant=' + currentTenant, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var myList = new Array();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    myList.push(entity);
                }
                return myList;
            });
        });
    };
    ReportsDomainService.prototype.GetBusinessUnitLists = function (currentTenant) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetBusinessUnitLists?tenant=' + currentTenant, {
                headers: authHeader
            }).map(function (response) {
                var myList = response.json();
                return myList;
            });
        });
    };
    ReportsDomainService.prototype.GetAdditionalServicesByTenant = function (currentTenant) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetAdditionalServicesByTenant?tenant=' + currentTenant, {
                headers: authHeader
            }).map(function (response) {
                var myList = response.json();
                return myList;
            });
        });
    };
    ReportsDomainService.prototype.GetProductTypesByTenant = function (currentTenant) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetProductTypesByTenant?tenant=' + currentTenant, {
                headers: authHeader
            }).map(function (response) {
                var myList = response.json();
                return myList;
            });
        });
    };
    ReportsDomainService.prototype.GetLeadSourceLists = function (currentTenant) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetLeadSourceLists?tenant=' + currentTenant, {
                headers: authHeader
            }).map(function (response) {
                var myList = response.json();
                return myList;
            });
        });
    };
    ReportsDomainService.prototype.UploadStaticFile = function (fileUploadParamerter) {
        var _this = this;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ReportsDomain';
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.put(_this._apiUrl + '/putuploadstaticfile', JSON.stringify(fileUploadParamerter), {
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
    ReportsDomainService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new ParticipantList_1.ParticipantList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    ReportsDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ReportsDomainService);
    return ReportsDomainService;
}());
exports.ReportsDomainService = ReportsDomainService;
//# sourceMappingURL=ReportsDomainService.js.map