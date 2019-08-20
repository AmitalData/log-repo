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
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var SessionInfo_1 = require("../../Utilities/SessionInfo");
var PerformanceLogger_1 = require("../../Utilities/PerformanceLogger");
var BusinessRoleList_1 = require("../../EntityLists/BusinessRoleList");
var BusinessRoleExtendedListService = /** @class */ (function () {
    function BusinessRoleExtendedListService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/businessroleextendedviews';
    }
    BusinessRoleExtendedListService.prototype.getToggleBusinessRoles = function (memberId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetToggleBusinessRoles?memberId=' + memberId, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity;
                        entity = _this.MapJsonToEntityList(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                serviceResponse.CallTime = callTime;
                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger_1.PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "BusinessRole", "GetAllLists", "");
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    BusinessRoleExtendedListService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new BusinessRoleList_1.BusinessRoleList();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    BusinessRoleExtendedListService.CachedData = [];
    BusinessRoleExtendedListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], BusinessRoleExtendedListService);
    return BusinessRoleExtendedListService;
}());
exports.BusinessRoleExtendedListService = BusinessRoleExtendedListService;
//# sourceMappingURL=BusinessRoleExtendedListService.js.map