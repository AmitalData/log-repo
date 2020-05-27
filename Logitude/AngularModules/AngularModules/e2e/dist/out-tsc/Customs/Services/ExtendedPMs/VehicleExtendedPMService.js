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
var VehiclePM_1 = require("../../EntityPMs/VehiclePM");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var VehicleExtendedPMService = /** @class */ (function () {
    function VehicleExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/Vehicle';
    }
    VehicleExtendedPMService.prototype.GetVehicleByVehicleChassisNumberOrRichbitFileNumber = function (vehicleChassisNumber, richbitFileNumber) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetVehicleByVehicleChassisNumberOrRichbitFileNumber?vehicleChassisNumber=' + vehicleChassisNumber + '&richbitFileNumber=' + richbitFileNumber, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                // serviceResponse.Result = response.json();
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = _this.MapJsonToEntityPM(pm);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    VehicleExtendedPMService.prototype.CheckIfVehicleExistByChassisNumber_old = function (vehicleChassisNumber) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        authHeader.append('Content-Type', 'application/json');
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/CheckIfVehicleExistByChassisNumber/?' + '&vehicleChassisNumber=' + vehicleChassisNumber, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    VehicleExtendedPMService.prototype.CheckIfVehicleExistByChassisNumber = function (vehicleChassisNumber) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http
                .get(_this._apiUrl + '/GetCheckIfVehicleExistByChassisNumber/?' + '&vehicleChassisNumber=' + vehicleChassisNumber, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    VehicleExtendedPMService.prototype.GetDuplicatedVehInSameDeclaration = function (declarationId, vehiclesNumbers, chassissNumbersString) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetDuplicatedVehInSameDeclaration?declarationId=' + declarationId + '&richbitNumbersString=' + vehiclesNumbers + '&chassissNumbersString=' + chassissNumbersString, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                // serviceResponse.Result = response.json();
                var pm = response.json();
                //var entity: VehiclePM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = pm;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    VehicleExtendedPMService.prototype.GetCheckRichbitNumbersError = function (vehiclesNumbers) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCheckRichbitNumbersError?richbitNumbersString=' + vehiclesNumbers, { headers: authHeader }).map(function (response) {
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                // serviceResponse.Result = response.json();
                var pm = response.json();
                //var entity: VehiclePM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = pm;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    VehicleExtendedPMService.prototype.GetVehiclesByRichbitFileNumbers = function (richbitNumbersString) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetVehiclesByRichbitFileNumbers/?' + '&richbitNumbersString=' + richbitNumbersString, { headers: authHeader }).map(function (response) {
                var allLists = response.json();
                var _mappedListsArray = [];
                if (allLists) {
                    for (var key in allLists) {
                        var entity;
                        entity = _this.MapJsonToEntityPM(allLists[key]);
                        _mappedListsArray.push(entity);
                    }
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                var servertime = response.headers.get('ServerExecutionTime');
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    VehicleExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new VehiclePM_1.VehiclePM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    VehicleExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], VehicleExtendedPMService);
    return VehicleExtendedPMService;
}());
exports.VehicleExtendedPMService = VehicleExtendedPMService;
//# sourceMappingURL=VehicleExtendedPMService.js.map