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
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var SessionInfo_1 = require("../../Utilities/SessionInfo");
var CCSWebService = (function () {
    function CCSWebService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CCSWebService';
    }
    CCSWebService.prototype.Send = function (myShipmentId, myRecipient, isSendingCargonaut, isSendingDEXX) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetMessageResult?myShipmentId=' + myShipmentId + '&myRecipient=' + myRecipient + '&isSendingCargonaut=' + isSendingCargonaut + '&isSendingDEXX=' + isSendingDEXX;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var mappedResult = new CCSResult();
                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CCSWebService.prototype.GetFHLsValidation = function (myMasterId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetFHLsValidation?myMasterId=' + myMasterId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityList(allLists[key]);
                    _mappedListsArray.push(entity);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CCSWebService.prototype.GetSendingValidations = function (myShipmentId, myRecipient, isSendingFHLs, isSendingCargonaut, isSendingDEXX, mainCarriageCarrierId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSendingValidations?myShipmentId=' + myShipmentId + '&myRecipient=' + myRecipient + '&isSendingFHLs=' + isSendingFHLs + '&isSendingCargonaut=' + isSendingCargonaut + '&isSendingDEXX=' + isSendingDEXX + '&mainCarriageCarrierId=' + mainCarriageCarrierId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var mappedResult = new AWBResultClass();
                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CCSWebService.prototype.GetAWBPrintingStock = function (myShipmentId, isCargonautSending, isDEXXSending, isConfirmedByUser) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetAWBPrintingStock?myShipmentId=' + myShipmentId + '&isCargonautSending=' + isCargonautSending + '&isDEXXSending=' + isDEXXSending + '&isConfirmedByUser=' + isConfirmedByUser;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var mappedResult = new AWBPrintResult();
                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CCSWebService.prototype.MapJsonToEntityList = function (jsonList) {
        var entityList;
        entityList = new FHLShipmentValidator();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    return CCSWebService;
}());
CCSWebService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [])
], CCSWebService);
exports.CCSWebService = CCSWebService;
var CCSResult = (function () {
    function CCSResult() {
    }
    return CCSResult;
}());
exports.CCSResult = CCSResult;
var AWBResultClass = (function () {
    function AWBResultClass() {
        this.ErrorsList = [];
        this.ValidFHLsDataStringList = [];
    }
    return AWBResultClass;
}());
exports.AWBResultClass = AWBResultClass;
var FHLShipmentValidator = (function () {
    function FHLShipmentValidator() {
        this.FHLErrors = [];
    }
    return FHLShipmentValidator;
}());
exports.FHLShipmentValidator = FHLShipmentValidator;
var AWBPrintResult = (function () {
    function AWBPrintResult() {
    }
    return AWBPrintResult;
}());
exports.AWBPrintResult = AWBPrintResult;
//# sourceMappingURL=CCSWebService.js.map