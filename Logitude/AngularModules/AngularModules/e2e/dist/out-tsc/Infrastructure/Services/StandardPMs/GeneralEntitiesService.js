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
var EntityPMServiceResponse_1 = require("../../../Infrastructure/DataContracts/EntityPMServiceResponse");
var ClassLevelValidator_1 = require("../../../Infrastructure/Validators/ClassLevelValidator");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var GeneralEntitiesArgs_1 = require("../../DataContracts/GeneralEntitiesArgs");
var GeneralEntitiesService = /** @class */ (function () {
    function GeneralEntitiesService() {
    }
    GeneralEntitiesService.prototype.setServiceArgs = function (serviceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/webfreightdomain';
    };
    //getadvancedqueryfiltersbytenant(tenant: number, userid: string) {
    //    var authHeader = new Headers();
    //    authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
    //    return Rx.Observable.defer(() => {
    //        return this._http.get(this._apiUrl + '/getadvancedqueryfiltersbytenant?' + 'tenant=' + tenant + '&loggedcontactid=' + userid, {
    //            headers: authHeader
    //        }).map(response => {
    //            var pms = response.json();
    //            return pms;
    //        });
    //    }
    //    );
    //}
    GeneralEntitiesService.prototype.insert = function (entities) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("AdvancedQueryFilter", entityPM);
            var response;
            response = new EntityPMServiceResponse_1.EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entities, false);
                return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult;
                        mappedResult = _this.MapJsonToEntityPM(pm, true, entities);
                        response.Result = mappedResult;
                    }
                    return response;
                });
            }
            else {
                response.HasError = true;
                response.ErrorsArray = errorsArray;
                return Rx_1.Observable.of(response);
            }
        });
    };
    GeneralEntitiesService.prototype.update = function (entities) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("AdvancedQueryFilter", entityPM);
            var response;
            response = new EntityPMServiceResponse_1.EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entities, false);
                return _this._http.put(_this._apiUrl + "/PutGeneralEntities", JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult;
                        mappedResult = _this.MapJsonToEntityPM(pm, true, entities);
                        response.Result = mappedResult;
                    }
                    return response;
                });
            }
            else {
                response.HasError = true;
                response.ErrorsArray = errorsArray;
                return Rx_1.Observable.of(response);
            }
        });
    };
    GeneralEntitiesService.prototype.MapJsonToEntityPM = function (jsonPM, getCallMap, entities) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entities === void 0) { entities = null; }
        if (!entities) {
            entities = new GeneralEntitiesArgs_1.GeneralEntitiesArgs();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entities[property] = jsonPM[property];
        }
        //entityPM.IsDirty = false;
        //if (getCallMap) {
        //    entityPM.OldEntityPM = this.clone(entityPM);
        //}
        //else {
        //    entityPM.OldEntityPM = null;
        //}
        return entities;
    };
    GeneralEntitiesService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], GeneralEntitiesService);
    return GeneralEntitiesService;
}());
exports.GeneralEntitiesService = GeneralEntitiesService;
//# sourceMappingURL=GeneralEntitiesService.js.map