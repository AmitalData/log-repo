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
var ServiceHelper_1 = require("../Utilities/ServiceHelper");
var ServiceResponse_1 = require("../DataContracts/ServiceResponse");
var SessionInfo_1 = require("../Utilities/SessionInfo");
var BusinessProcessDomainService = /** @class */ (function () {
    function BusinessProcessDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/BusinessProcessDomain';
    }
    BusinessProcessDomainService.prototype.GetQueuesWithCounts = function (myFilter) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetQueuesWithCounts?myFilter=' + myFilter;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var myList = new Array();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityListQueueData(allLists[key]);
                    myList.push(entity);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myList;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    BusinessProcessDomainService.prototype.GetTeamsForLoggedUser = function (loggedUserId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetTeamsForLoggedUser?loggedUserId=' + loggedUserId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myResult = response.json();
                var myResponse;
                myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    BusinessProcessDomainService.prototype.MapJsonToEntityListQueueData = function (jsonList) {
        var entityList;
        entityList = new QueueData();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    BusinessProcessDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], BusinessProcessDomainService);
    return BusinessProcessDomainService;
}());
exports.BusinessProcessDomainService = BusinessProcessDomainService;
var QueueData = /** @class */ (function () {
    function QueueData() {
    }
    return QueueData;
}());
exports.QueueData = QueueData;
//# sourceMappingURL=BusinessProcessDomainService.js.map