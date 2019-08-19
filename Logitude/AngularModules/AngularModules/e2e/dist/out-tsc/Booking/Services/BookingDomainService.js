"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/http");
require("rxjs/add/operator/map");
require("rxjs/add/operator/catch");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var ChartingDataClass_1 = require("../../Infrastructure/DataContracts/Dashboard/ChartingDataClass");
var SessionInfo_1 = require("../../Infrastructure/Utilities/SessionInfo");
var BookingDomainService = /** @class */ (function () {
    function BookingDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/BookingDomain';
    }
    BookingDomainService.prototype.GetBookingsCounts = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetBookingsCounts';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResult = new BookingsDataCounts();
                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    BookingDomainService.prototype.GetRecentBookings = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetRecentBookings';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    BookingDomainService.prototype.GetBookingAnswerPMs = function (bookingId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetBookingAnswerPMs?bookingId=' + bookingId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = allLists;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    BookingDomainService.prototype.GetBookingsDashBoard = function (Tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetBookingsDashBoard?tenant=' + Tenant;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var myList = new Array();
                for (var key in allLists) {
                    var entity;
                    entity = _this.MapJsonToEntityListChartingDataClass(allLists[key]);
                    myList.push(entity);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myList;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    BookingDomainService.prototype.ValidateBookingForSending = function (bookingId, isCancellationSent) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var url = this._apiUrl + '/GetValidateBookingForSending?bookingId=' + bookingId + "&isCancellationSent=" + isCancellationSent;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var myResult = new BookingValidatorResultClass();
                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        myResult[property] = myJsonResult[property];
                    }
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    BookingDomainService.prototype.ValidateBookingMasterFieldExistance = function (entityId, myMasterField, myAirlinePrefixField, myDirectionId, myTransportModeId, isCancelled, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var args = new ValidateShipmentMasterArgs();
            args.BookingId = entityId;
            args.Master = myMasterField;
            args.AirlinePrefix = myAirlinePrefixField;
            args.DirectionId = myDirectionId;
            args.TransportModeId = myTransportModeId;
            args.IsCancelled = isCancelled;
            var mappedEntity = _this.MapJsonToValidateShipmentMasterArgs(args, false);
            return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    BookingDomainService.prototype.MapJsonToValidateShipmentMasterArgs = function (jsonPM, getCallMap, entity) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entity === void 0) { entity = null; }
        if (!entity) {
            entity = new ValidateShipmentMasterArgs();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "UIProperties") {
                continue;
            }
            else {
                entity[property] = jsonPM[property];
            }
        }
        return entity;
    };
    BookingDomainService.prototype.MapJsonToEntityListChartingDataClass = function (jsonList) {
        var entityList;
        entityList = new ChartingDataClass_1.ChartingDataClass();
        var jsonListKeys = Object.keys(jsonList);
        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }
        return entityList;
    };
    return BookingDomainService;
}());
exports.BookingDomainService = BookingDomainService;
var BookingsDataCounts = /** @class */ (function () {
    function BookingsDataCounts() {
    }
    return BookingsDataCounts;
}());
exports.BookingsDataCounts = BookingsDataCounts;
var ValidateShipmentMasterArgs = /** @class */ (function () {
    function ValidateShipmentMasterArgs() {
    }
    return ValidateShipmentMasterArgs;
}());
exports.ValidateShipmentMasterArgs = ValidateShipmentMasterArgs;
var BookingValidatorResultClass = /** @class */ (function () {
    function BookingValidatorResultClass() {
        this.ErrorsList = [];
    }
    return BookingValidatorResultClass;
}());
exports.BookingValidatorResultClass = BookingValidatorResultClass;
//# sourceMappingURL=BookingDomainService.js.map