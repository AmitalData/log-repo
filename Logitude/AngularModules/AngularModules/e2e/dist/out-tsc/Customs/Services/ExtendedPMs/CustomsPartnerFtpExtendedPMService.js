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
var CustomsPartnerFtpPM_1 = require("../../EntityPMs/CustomsPartnerFtpPM");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var CustomsPartnerFtpExtendedPMService = /** @class */ (function () {
    function CustomsPartnerFtpExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CustomsPartnerFtpExtended';
    }
    //getAll(tenant: number) {
    //    var authHeader = new Headers();
    //    authHeader.append('Token', SessionInfo.Token);
    //    return Observable.defer(() => {
    //        return this._http.get(this._apiUrl + '/getAll?tenant=' + tenant, { headers: authHeader }).map(response => {
    //            var pmList :any[]= response.json();
    //            var entityList: CustomsPartnerFtpPM[];
    //            if (pmList) {
    //                pmList.forEach(pm => {
    //                    entityList.push(this.MapJsonToEntityPM(pm));
    //                })
    //            }
    //            var serviceResponse: ServiceResponse;
    //            serviceResponse = new ServiceResponse();
    //            serviceResponse.Result = entityList;
    //            return serviceResponse;
    //        }).catch(ServiceHelper.HandleServiceError);
    //    });
    //}
    CustomsPartnerFtpExtendedPMService.prototype.delete = function (Id) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var mappedEntity;
            return _this._http.delete(_this._apiUrl + '/Delete/?' + 'Id=' + Id, { headers: authHeader }).map(function (response) {
                var pm = response.json();
                if (pm) {
                    var mappedResult;
                    serviceResponse.Result = mappedResult;
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CustomsPartnerFtpExtendedPMService.prototype.GetScreenOption = function (tenant) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var mappedEntity;
            return _this._http.get(_this._apiUrl + '/GetScreenOption/?' + 'tenant=' + tenant, { headers: authHeader }).map(function (response) {
                var ScreenOption = response.json();
                if (ScreenOption) {
                    //var mappedResult: CustomsPartnerFtpPM;
                    serviceResponse.Result = ScreenOption;
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CustomsPartnerFtpExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM) {
        var entityPM;
        entityPM = new CustomsPartnerFtpPM_1.CustomsPartnerFtpPM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    CustomsPartnerFtpExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CustomsPartnerFtpExtendedPMService);
    return CustomsPartnerFtpExtendedPMService;
}());
exports.CustomsPartnerFtpExtendedPMService = CustomsPartnerFtpExtendedPMService;
//# sourceMappingURL=CustomsPartnerFtpExtendedPMService.js.map