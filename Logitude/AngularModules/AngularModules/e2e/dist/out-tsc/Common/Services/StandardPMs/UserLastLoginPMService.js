"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/http");
var Rx_1 = require("rxjs/Rx");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ClassLevelValidator_1 = require("../../../Infrastructure/Validators/ClassLevelValidator");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var UserLastLoginPM_1 = require("../../EntityPMs/UserLastLoginPM");
var UserLastLoginPMService = /** @class */ (function () {
    function UserLastLoginPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/UserLastLogins';
    }
    UserLastLoginPMService.prototype.GetUserLastLogin = function (userId, tenant) {
        var _this = this;
        var url = this._apiUrl + '/GetUserLastLogin?userId=' + userId + '&tenant=' + tenant;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        return this._http.get(url, { headers: authHeader }).map(function (response) {
            var pm = response.json();
            var entity;
            if (pm) {
                entity = _this.MapJsonToEntityPM(pm);
            }
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            serviceResponse.Result = entity;
            return serviceResponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleTimerServiceError);
    };
    UserLastLoginPMService.prototype.update = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("AccountingSetting", entityPM);
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
                }).catch(ServiceHelper_1.ServiceHelper.HandleTimerServiceError);
            }
            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;
                return Rx_1.Observable.of(serviceResponse);
            }
        });
    };
    UserLastLoginPMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new UserLastLoginPM_1.UserLastLoginPM();
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
    UserLastLoginPMService.prototype.clone = function (jsonPM) {
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
    return UserLastLoginPMService;
}());
exports.UserLastLoginPMService = UserLastLoginPMService;
//# sourceMappingURL=UserLastLoginPMService.js.map