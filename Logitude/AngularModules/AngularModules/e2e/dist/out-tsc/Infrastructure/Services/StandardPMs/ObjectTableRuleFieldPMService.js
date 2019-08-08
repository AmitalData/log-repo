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
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var ClassLevelValidator_1 = require("../../Validators/ClassLevelValidator");
var InfraSettings_1 = require("../../Utilities/InfraSettings");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var ObjectTableRuleFieldPM_1 = require("../../EntityPMs/ObjectTableRuleFieldPM");
var ObjectTableRuleFieldPMService = /** @class */ (function () {
    function ObjectTableRuleFieldPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ObjectTableRuleFields';
    }
    ObjectTableRuleFieldPMService.prototype.get = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
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
    ObjectTableRuleFieldPMService.prototype.insert = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = validator.Validate("ObjectTableRuleField", entityPM);
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entityPM, false);
                return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
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
    ObjectTableRuleFieldPMService.prototype.update = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = validator.Validate("ObjectTableRuleField", entityPM);
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
    ObjectTableRuleFieldPMService.prototype.updateRuleFieldsList = function (ruleFieldsPMList) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            var mappedList = [];
            for (var k in ruleFieldsPMList) {
                var field = ruleFieldsPMList[k];
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(field, false);
                mappedList.push(mappedEntity);
            }
            return _this._http.put(_this._apiUrl + '/PutRuleFieldsList', JSON.stringify(mappedList), { headers: authHeader }).map(function (res) {
                var pm = res.json();
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ObjectTableRuleFieldPMService.prototype.getAllByTenant = function (tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetObjectTableRuleFieldPMsByTenant?' + 'tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var result = response.json();
                var mappedResult = [];
                if (result) {
                    mappedResult = ServiceHelper_1.ServiceHelper.MapJsonToArrayofEntities(result, ObjectTableRuleFieldPM_1.ObjectTableRuleFieldPM);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ObjectTableRuleFieldPMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new ObjectTableRuleFieldPM_1.ObjectTableRuleFieldPM();
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
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    ObjectTableRuleFieldPMService.prototype.clone = function (jsonPM) {
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
    ObjectTableRuleFieldPMService.prototype.GetNewEntityPM = function () {
        var entityPM;
        entityPM = new ObjectTableRuleFieldPM_1.ObjectTableRuleFieldPM();
        entityPM.Tenant = InfraSettings_1.InfraSettings.TenantPM.Id;
        return entityPM;
    };
    ObjectTableRuleFieldPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ObjectTableRuleFieldPMService);
    return ObjectTableRuleFieldPMService;
}());
exports.ObjectTableRuleFieldPMService = ObjectTableRuleFieldPMService;
//# sourceMappingURL=ObjectTableRuleFieldPMService.js.map