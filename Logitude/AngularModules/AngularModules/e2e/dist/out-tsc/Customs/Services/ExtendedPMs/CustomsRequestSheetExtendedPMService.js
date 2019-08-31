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
var CustomsRequestsSheetPM_1 = require("../../EntityPMs/CustomsRequestsSheetPM");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var CustomsRequestSheetExtendedPMService = /** @class */ (function () {
    function CustomsRequestSheetExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CustomsRequestSheetExtended';
    }
    CustomsRequestSheetExtendedPMService.prototype.PostSetCustomsRequestSheetStatus = function (mappedEntity) {
        //CancellRequestInProgress(Id: string, Tenant: number) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            //var mappedEntity: CustomsRequestsSheetPM;
            // mappedEntity = this.MapJsonToEntityPM(entityPM, false);
            return Rx_1.Observable.defer(function () {
                return _this._http
                    .post(_this._apiUrl + '/PostSetCustomsRequestSheetStatus/', JSON.stringify(mappedEntity), { headers: authHeader })
                    .map(function (response) {
                    var serviceResponse = new ServiceResponse_1.ServiceResponse();
                    var requestSheets = response.json();
                    serviceResponse.Result = requestSheets;
                    return serviceResponse;
                })
                    .catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            });
        });
    };
    CustomsRequestSheetExtendedPMService.prototype.PostCustomsRequestSheetReQueue = function (mappedEntity) {
        //CancellRequestInProgress(Id: string, Tenant: number) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            //var mappedEntity: CustomsRequestsSheetPM;
            // mappedEntity = this.MapJsonToEntityPM(entityPM, false);
            return Rx_1.Observable.defer(function () {
                return _this._http
                    .post(_this._apiUrl + '/PostCustomsRequestSheetReQueue/', JSON.stringify(mappedEntity), { headers: authHeader })
                    .map(function (response) {
                    var serviceResponse = new ServiceResponse_1.ServiceResponse();
                    var requestSheets = response.json();
                    serviceResponse.Result = requestSheets;
                    return serviceResponse;
                })
                    .catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            });
        });
    };
    CustomsRequestSheetExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new CustomsRequestsSheetPM_1.CustomsRequestsSheetPM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    CustomsRequestSheetExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CustomsRequestSheetExtendedPMService);
    return CustomsRequestSheetExtendedPMService;
}());
exports.CustomsRequestSheetExtendedPMService = CustomsRequestSheetExtendedPMService;
//# sourceMappingURL=CustomsRequestSheetExtendedPMService.js.map