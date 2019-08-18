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
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var FVRWebService = /** @class */ (function () {
    function FVRWebService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/FVRWebService';
    }
    FVRWebService.prototype.SendFVR = function (myAirlineId, myFromPortId, myToPortId, myETD, myETA, myVolume, myGrossWeight, myVolumeUnitCode, myGrossWeightUnitCode, myShipmentId, myBookingId, myRecipient) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var args = new FVRServiceArgs();
            args.AirlineId = myAirlineId;
            args.ShipmentId = myShipmentId;
            args.BookingId = myBookingId;
            args.FromPortId = myFromPortId;
            args.ToPortId = myToPortId;
            args.ETD = myETD;
            args.ETA = myETA;
            args.Volume = myVolume;
            args.GrossWeight = myGrossWeight;
            args.VolumeUnitCode = myVolumeUnitCode;
            args.GrossWeightUnitCode = myGrossWeightUnitCode;
            args.Recipient = myRecipient;
            var mappedEntity = _this.MapJsonToFVRServiceArgs(args, false);
            return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    FVRWebService.prototype.SimulateXML = function (xmlString, myShipmentId, myBookingId, isFNA) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetSimulateXML?xmlString=' + xmlString + '&myShipmentId=' + myShipmentId + '&myBookingId=' + myBookingId + '&isFNA=' + isFNA, {
                headers: authHeader
            }).map(function (response) {
                var myJsonResult = response.json();
                var mappedResult = new FVASimulatorResult();
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
    FVRWebService.prototype.GetCopyFlightsSchedulesPorts = function (myResponseIds) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCopyFlightsSchedulesPorts?myResponseIds=' + myResponseIds, {
                headers: authHeader
            }).map(function (response) {
                var listJason = response.json();
                var listMapped = [];
                for (var itemJeson in listJason) {
                    var itemMapped = _this.MapFlightSchedulePort(listJason[itemJeson]);
                    listMapped.push(itemMapped);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = listMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    FVRWebService.prototype.MapJsonToFVRServiceArgs = function (jsonPM, getCallMap, entity) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entity === void 0) { entity = null; }
        if (!entity) {
            entity = new FVRServiceArgs();
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
    FVRWebService.prototype.MapFlightSchedulePort = function (jsonList) {
        var entityPM = new FlightSchedulePort();
        if (jsonList) {
            var jsonListKeys = Object.keys(jsonList);
            for (var key in jsonListKeys) {
                var property = jsonListKeys[key];
                entityPM[property] = jsonList[property];
            }
        }
        return entityPM;
    };
    FVRWebService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], FVRWebService);
    return FVRWebService;
}());
exports.FVRWebService = FVRWebService;
var FVRServiceArgs = /** @class */ (function () {
    function FVRServiceArgs() {
    }
    return FVRServiceArgs;
}());
exports.FVRServiceArgs = FVRServiceArgs;
var FVRResultClass = /** @class */ (function () {
    function FVRResultClass() {
    }
    return FVRResultClass;
}());
exports.FVRResultClass = FVRResultClass;
var FVASimulatorResult = /** @class */ (function () {
    function FVASimulatorResult() {
    }
    return FVASimulatorResult;
}());
exports.FVASimulatorResult = FVASimulatorResult;
var FlightSchedulePort = /** @class */ (function () {
    function FlightSchedulePort() {
    }
    return FlightSchedulePort;
}());
exports.FlightSchedulePort = FlightSchedulePort;
//# sourceMappingURL=FVRWebService.js.map