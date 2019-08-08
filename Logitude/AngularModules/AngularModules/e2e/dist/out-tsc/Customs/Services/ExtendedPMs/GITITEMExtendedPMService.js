"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ClassLevelValidator_1 = require("../../../Infrastructure/Validators/ClassLevelValidator");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var GITITEMDto_1 = require("../../EntityPMs/Extended/GITITEMDto");
var GITITEMExtendedPMService = /** @class */ (function () {
    function GITITEMExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/GITITEM';
    }
    GITITEMExtendedPMService.prototype.get = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getsingle?' + 'id=' + id, {
                headers: authHeader
            }).map(function (response) {
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
    GITITEMExtendedPMService.prototype.insert546 = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            //if (errorsArray.length == 0) {
            //var mappedEntity: GITITEMDto;
            //    mappedEntity = this.MapJsonToEntityPM(entityPM, false);
            return _this._http.post(_this._apiUrl, JSON.stringify(entityPM), { headers: authHeader }).map(function (res) {
                var pm = res.json();
                if (pm) {
                    var mappedResult;
                    mappedResult = _this.MapJsonToEntityPM(pm, true, entityPM);
                    serviceResponse.Result = mappedResult;
                }
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            //}
            //else {
            //    serviceResponse.HasError = true;
            //    serviceResponse.ErrorsArray = errorsArray;
            //    return Observable.of(serviceResponse);
            //}
        });
    };
    //insert(entityPM: GITITEMDto) {
    //    return Observable.defer(() => {
    //        var authHeader = new Headers();
    //        authHeader.append('Token', SessionInfo.Token);
    //        authHeader.append('Content-Type', 'application/json');
    //        var serviceResponse: ServiceResponse;
    //        serviceResponse = new ServiceResponse();
    //        return this._http
    //            .post(
    //            this._apiUrl + '/PostGITITEMPM',
    //            JSON.stringify(entityPM),
    //            { headers: authHeader })
    //            .map((res) => {
    //                var pm = res.json();
    //                if (pm) {
    //                    var mappedResult: GITITEMDto;
    //                    mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
    //                    serviceResponse.Result = mappedResult;
    //                }
    //                return serviceResponse;
    //            }).catch(ServiceHelper.HandleServiceError);
    //    });
    //}
    GITITEMExtendedPMService.prototype.insert = function (GITITEMDtoList) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            return _this._http
                .post(_this._apiUrl + '/PostGITITEMPMList', JSON.stringify(GITITEMDtoList), { headers: authHeader })
                .map(function (res) {
                //var pm = res.json();
                //if (pm) {
                //    var mappedResult: GITITEMDto[];
                //    mappedResult = this.MapJsonToEntityPM(pm, true, GITITEMDtoList);
                //    serviceResponse.Result = mappedResult;
                //}
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    GITITEMExtendedPMService.prototype.update = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = validator.Validate("Customs.GITITEM", entityPM);
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entityPM, false);
                return _this._http.put(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult;
                        mappedResult = _this.MapJsonToEntityPM(pm, true, entityPM);
                        serviceResponse.Result = mappedResult;
                    }
                    return serviceResponse;
                }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            }
            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;
                return Rx_1.Observable.of(serviceResponse);
            }
        });
    };
    GITITEMExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new GITITEMDto_1.GITITEMDto();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        //entityPM.IsDirty = false;
        //if (mapParent) {
        //    entityPM.OldEntityPM = this.clone(entityPM);
        //}
        //else {
        //    entityPM.OldEntityPM = null;
        //}
        return entityPM;
    };
    return GITITEMExtendedPMService;
}());
exports.GITITEMExtendedPMService = GITITEMExtendedPMService;
//# sourceMappingURL=GITITEMExtendedPMService.js.map