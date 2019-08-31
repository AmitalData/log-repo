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
var ShipmentPM_1 = require("../../../Shipment/EntityPMs/ShipmentPM");
var FSRWebService = /** @class */ (function () {
    function FSRWebService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/FSRWebService';
    }
    FSRWebService.prototype.SendFSR = function (shipmentId, objectTableId, myRecipient) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSendFSR?shipmentId=' + shipmentId + '&objectTableId=' + objectTableId + '&myRecipient=' + myRecipient;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var mappedResult = new FSRResultClass();
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
    FSRWebService.prototype.SendFSRShipment = function (entityPM) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var mappedEntity;
        mappedEntity = this.MapJsonToEntityPM(entityPM, false);
        return Rx_1.Observable.defer(function () {
            return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = new FSRResultClass();
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
    FSRWebService.prototype.SendBookingFSR = function (bookingId, objectTableId, myRecipient) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetSendBookingFSR?bookingId=' + bookingId + '&objectTableId=' + objectTableId + '&myRecipient=' + myRecipient;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var mappedResult = new FSRResultClass();
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
    FSRWebService.prototype.MapJsonToEntityPM = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new ShipmentPM_1.ShipmentPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        if (getCallMap) {
            entityPM.OldEntityPM = this.clone(entityPM);
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    FSRWebService.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    FSRWebService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], FSRWebService);
    return FSRWebService;
}());
exports.FSRWebService = FSRWebService;
var FSRResultClass = /** @class */ (function () {
    function FSRResultClass() {
    }
    return FSRResultClass;
}());
exports.FSRResultClass = FSRResultClass;
//# sourceMappingURL=FSRWebService.js.map