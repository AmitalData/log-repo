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
var EntityPMServiceResponse_1 = require("../../DataContracts/EntityPMServiceResponse");
var ClassLevelValidator_1 = require("../../Validators/ClassLevelValidator");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var ErrorLogPM_1 = require("../../EntityPMs/ErrorLogPM");
var ErrorsLogPMService = /** @class */ (function () {
    function ErrorsLogPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/errorlogs';
    }
    //setServiceArgs(serviceArgs: ServiceArgs) {
    //    this._serviceArgs = serviceArgs;
    //}
    //get(code: string) {
    //    var authHeader = new Headers();
    //    authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
    //    return Observable.defer(() => {
    //        return this._http.get(this._apiUrl + '/getsingle?' + 'code=' + code, {
    //            headers: authHeader
    //        }).map(response => {
    //            var pm = response.json();
    //            var entity: ChargesGroupPM;
    //            if (pm) {
    //                entity = this.MapJsonToEntityPM(pm);
    //            }
    //            return entity;
    //        });
    //    }
    //    );
    //}
    ErrorsLogPMService.prototype.insert = function (entityPM) {
        var _this = this;
        return Observable_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("ErrorLog", entityPM);
            var response;
            response = new EntityPMServiceResponse_1.EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entityPM, false);
                return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult;
                        mappedResult = _this.MapJsonToEntityPM(pm, true, entityPM);
                        response.Result = mappedResult;
                    }
                    return response;
                });
            }
            else {
                response.HasError = true;
                response.ErrorsArray = errorsArray;
                return Observable_1.Observable.of(response);
            }
        });
    };
    ErrorsLogPMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new ErrorLogPM_1.ErrorLogPM();
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
    ErrorsLogPMService.prototype.clone = function (jsonPM) {
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
    ErrorsLogPMService.prototype.GetNewEntityPM = function () {
        var entityPM;
        entityPM = new ErrorLogPM_1.ErrorLogPM();
        return entityPM;
    };
    ErrorsLogPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ErrorsLogPMService);
    return ErrorsLogPMService;
}());
exports.ErrorsLogPMService = ErrorsLogPMService;
//# sourceMappingURL=ErrorsLogPMService.js.map