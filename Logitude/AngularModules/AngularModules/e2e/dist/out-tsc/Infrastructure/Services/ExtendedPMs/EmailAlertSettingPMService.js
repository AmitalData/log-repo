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
var Observable_1 = require("rxjs/Observable");
var ClassLevelValidator_1 = require("../../Validators/ClassLevelValidator");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var EmailAlertSettingPM_1 = require("../../EntityPMs/EmailAlertSettingPM");
var EmailAlertSettingPMService = /** @class */ (function () {
    function EmailAlertSettingPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/EmailAlertSetting';
    }
    EmailAlertSettingPMService.prototype.getAllEmailAlerts = function (tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var serviceResponse;
        serviceResponse = new ServiceResponse_1.ServiceResponse();
        return Observable_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetEmailAlertSettingsByTenant?' + 'tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var mappedAlerts = [];
                var alerts = response.json();
                for (var k in alerts) {
                    var entity;
                    entity = _this.MapJsonToEntityPM(alerts[k]);
                    mappedAlerts.push(entity);
                }
                serviceResponse.Result = mappedAlerts;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    EmailAlertSettingPMService.prototype.updateAllAlerts = function (allAlerts, tenant) {
        var _this = this;
        return Observable_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = [];
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedAlerts = [];
                for (var k in allAlerts) {
                    var entity;
                    entity = _this.MapJsonToEntityPM(allAlerts[k], false);
                    mappedAlerts.push(entity);
                }
                var env = new EmailAlertSettingsEnvelope();
                env.EmailAlerts = [];
                env.EmailAlerts = mappedAlerts;
                var postString;
                postString = JSON.stringify(env);
                console.log(postString);
                return _this._http.put(_this._apiUrl + "?tenant=" + tenant, postString, { headers: authHeader }).map(function (response) {
                    var mappedAlerts = [];
                    var env = response.json();
                    for (var k in env.EmailAlerts) {
                        var entity;
                        entity = _this.MapJsonToEntityPM(env.EmailAlerts[k]);
                        mappedAlerts.push(entity);
                    }
                    serviceResponse.Result = mappedAlerts;
                    return serviceResponse;
                }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
            }
            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;
                return Observable_1.Observable.of(serviceResponse);
            }
        });
    };
    //insert(entityPM: EmailAlertSettingPM) {
    //    return Observable.defer(() => {
    //        var authHeader = new Headers();
    //        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
    //        authHeader.append('Content-Type', 'application/json');
    //        var validator: ClassLevelValidator;
    //        validator = new ClassLevelValidator();
    //        var errorsArray = [];
    //        var response: EntityPMServiceResponse;
    //        response = new EntityPMServiceResponse();
    //        if (errorsArray.length == 0) {
    //            var mappedEntity: EmailAlertSettingPM;
    //            mappedEntity = this.MapJsonToEntityPM(entityPM, false);
    //            return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
    //                { headers: authHeader }).map((res) => {
    //                    var pm = res.json();
    //                    if (pm) {
    //                        var mappedResult: EmailAlertSettingPM;
    //                        mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
    //                        response.Result = mappedResult;
    //                    }
    //                    return response;
    //                }).catch(ServiceHelper.HandleServiceError);
    //        }
    //        else {
    //            response.HasError = true;
    //            response.ErrorsArray = errorsArray;
    //            return Observable.of(response);
    //        }
    //    }
    //    );
    //}
    //update(entityPM: EmailAlertSettingPM) {
    //    return Observable.defer(() => {
    //        var authHeader = new Headers();
    //        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
    //        authHeader.append('Content-Type', 'application/json');
    //        var validator: ClassLevelValidator;
    //        validator = new ClassLevelValidator();
    //        var errorsArray = [];
    //        var serviceResponse: ServiceResponse;
    //        serviceResponse = new ServiceResponse();
    //        if (errorsArray.length == 0) {
    //            var mappedEntity: EmailAlertSettingPM;
    //            mappedEntity = this.MapJsonToEntityPM(entityPM, false);
    //            return this._http.put(this._apiUrl, JSON.stringify(mappedEntity),
    //                { headers: authHeader }).map((res) => {
    //                    var pm = res.json();
    //                    if (pm) {
    //                        var mappedResult: EmailAlertSettingPM;
    //                        mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
    //                        serviceResponse.Result = mappedResult;
    //                    }
    //                    return serviceResponse;
    //                }).catch(ServiceHelper.HandleServiceError);
    //        }
    //        else {
    //            serviceResponse.HasError = true;
    //            serviceResponse.ErrorsArray = errorsArray;
    //            return Observable.of(serviceResponse);
    //        }
    //    }
    //    );
    //}
    EmailAlertSettingPMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new EmailAlertSettingPM_1.EmailAlertSettingPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    EmailAlertSettingPMService.prototype.clone = function (jsonPM) {
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
    EmailAlertSettingPMService.prototype.GetNewEntityPM = function () {
        var entityPM;
        entityPM = new EmailAlertSettingPM_1.EmailAlertSettingPM();
        return entityPM;
    };
    EmailAlertSettingPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], EmailAlertSettingPMService);
    return EmailAlertSettingPMService;
}());
exports.EmailAlertSettingPMService = EmailAlertSettingPMService;
var EmailAlertSettingsEnvelope = /** @class */ (function () {
    function EmailAlertSettingsEnvelope() {
    }
    return EmailAlertSettingsEnvelope;
}());
exports.EmailAlertSettingsEnvelope = EmailAlertSettingsEnvelope;
//# sourceMappingURL=EmailAlertSettingPMService.js.map