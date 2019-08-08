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
var NotificationPMService_1 = require("../StandardPMs/NotificationPMService");
var NotificationWebService = /** @class */ (function () {
    function NotificationWebService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/NotificationWebService';
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/NotificationWebService';
    }
    //NotificationReply
    NotificationWebService.prototype.GetNotificationsByDefinitionCode = function (objectTableId, entityId, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var notificationPMService = new NotificationPMService_1.NotificationPMService();
            return _this._http.get(_this._apiUrl + "/GetNotificationsByDefinitionCode/?objectTableId=" + objectTableId + "&entityId=" + entityId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity;
                        entity = notificationPMService.MapJsonToEntityPM(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    NotificationWebService.prototype.PostSendNotificationReplyRequest = function (entity) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + '/PostSendNotificationReplyRequest/', JSON.stringify(entity), { headers: authHeader }).map(function (res) {
                serviceResponse.Result = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    NotificationWebService.prototype.SetNotificationsStatus = function (Ids, status) {
        var _this = this;
        // Send request
        return Rx_1.Observable.defer(function () {
            // Prepare parameters
            var IdsParameterString = "";
            if (Ids && Ids.length > 0) {
                Ids.forEach(function (el) {
                    IdsParameterString += 'Ids=' + el + '&';
                });
            }
            else {
                console.log("[ERROR] cannot set notification status without Ids!", Ids, status);
                return;
            }
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetSetNotificationsStatus/?" + IdsParameterString + "status=" + status, { headers: authHeader }).map(function (response) {
                //var res = response.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                //serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    NotificationWebService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], NotificationWebService);
    return NotificationWebService;
}());
exports.NotificationWebService = NotificationWebService;
//# sourceMappingURL=NotificationWebService.js.map