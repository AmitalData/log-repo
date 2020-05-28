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
var PhysicalCheckList_1 = require("../../EntityLists/PhysicalCheckList");
var PhysicalCheckWebService = /** @class */ (function () {
    function PhysicalCheckWebService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/PhysicalCheckWebService';
    }
    PhysicalCheckWebService.prototype.GetPhysicalCheckByDeclarationIdLists = function (declarationId, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.get(_this._apiUrl + "/GetPhysicalCheckByDeclarationIdLists/?declarationId=" + declarationId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(function (response) {
                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PhysicalCheckWebService.prototype.PostClosePhysicalCheck = function (physicalCheckId, tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http.post(_this._apiUrl + "/PostClosePhysicalCheck/?physicalCheckId=" + physicalCheckId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(function (response) {
                var res = response.json();
                serviceResponse.Result = res;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    PhysicalCheckWebService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new PhysicalCheckList_1.PhysicalCheckList();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    PhysicalCheckWebService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], PhysicalCheckWebService);
    return PhysicalCheckWebService;
}());
exports.PhysicalCheckWebService = PhysicalCheckWebService;
//# sourceMappingURL=PhysicalCheckWebService.js.map