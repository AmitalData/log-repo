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
var ClassLevelValidator_1 = require("../../../Infrastructure/Validators/ClassLevelValidator");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ObjectTableRulePM_1 = require("../../EntityPMs/ObjectTableRulePM");
var RuleConditionFieldPM_1 = require("../../EntityPMs/RuleConditionFieldPM");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ObjectTableRulePMService = /** @class */ (function () {
    function ObjectTableRulePMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/ObjectTableRules';
    }
    ObjectTableRulePMService.prototype.get = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Observable_1.Observable.defer(function () {
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
    ObjectTableRulePMService.prototype.insert = function (entityPM) {
        var _this = this;
        return Observable_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("ObjectTableRule", entityPM);
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
                return Observable_1.Observable.of(serviceResponse);
            }
        });
    };
    ObjectTableRulePMService.prototype.update = function (entityPM) {
        var _this = this;
        return Observable_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("ObjectTableRule", entityPM);
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
                return Observable_1.Observable.of(serviceResponse);
            }
        });
    };
    ObjectTableRulePMService.prototype.getAllByTenant = function (tenant) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Observable_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetObjectTableRulePMsByTenant?' + 'tenant=' + tenant, {
                headers: authHeader
            }).map(function (response) {
                var result = response.json();
                var mappedResult = [];
                if (result) {
                    for (var k in result) {
                        var mappedPM;
                        mappedPM = _this.MapJsonToEntityPM(result[k]);
                        mappedResult.push(mappedPM);
                    }
                    //mappedResult = ServiceHelper.MapJsonToArrayofEntities(result, ObjectTableRulePM);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    ObjectTableRulePMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new ObjectTableRulePM_1.ObjectTableRulePM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        var oldRuleConditionFields = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldRuleConditionFields = entityPM.OldEntityPM.RuleConditionFields;
        }
        entityPM.RuleConditionFields = new Array();
        for (var item in jsonPM.RuleConditionFields) {
            var jItem = jsonPM.RuleConditionFields[item];
            if (mapParent && jItem.ChangeSetOp == "Delete") {
                continue;
            }
            var newRuleConditionFieldPM;
            if (mapParent) {
                newRuleConditionFieldPM = new RuleConditionFieldPM_1.RuleConditionFieldPM(entityPM);
            }
            else {
                newRuleConditionFieldPM = new RuleConditionFieldPM_1.RuleConditionFieldPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newRuleConditionFieldPM[pmProperty] = jItem[pmProperty];
            }
            newRuleConditionFieldPM.IsDirty = false;
            if (mapParent) {
                newRuleConditionFieldPM.OldEntityPM = this.clone(newRuleConditionFieldPM);
                newRuleConditionFieldPM.UniqueKey = Guid_1.Guid.newGuid();
                newRuleConditionFieldPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                if (newRuleConditionFieldPM.UniqueKey) {
                    if (jItem.IsDirty)
                        newRuleConditionFieldPM.ChangeSetOp = "Update";
                }
                else {
                    newRuleConditionFieldPM.ChangeSetOp = "Insert";
                }
                newRuleConditionFieldPM.OldEntityPM = null;
                newRuleConditionFieldPM.EntityParentPM = null;
            }
            entityPM.RuleConditionFields.push(newRuleConditionFieldPM);
        }
        if (oldRuleConditionFields) {
            for (var itemKey in oldRuleConditionFields) {
                if (entityPM.RuleConditionFields.filter(function (p) { return p.UniqueKey === oldRuleConditionFields[itemKey].UniqueKey; }).length === 0) {
                    if (oldRuleConditionFields[itemKey]) {
                        oldRuleConditionFields[itemKey].ChangeSetOp = "Delete";
                        entityPM.RuleConditionFields.push(oldRuleConditionFields[itemKey]);
                    }
                }
            }
        }
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.RuleConditionFields = [];
            for (var m in entityPM.RuleConditionFields) {
                entityPM.OldEntityPM.RuleConditionFields.push(this.clone(entityPM.RuleConditionFields[m]));
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    ObjectTableRulePMService.prototype.clone = function (jsonPM) {
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
    ObjectTableRulePMService.prototype.GetNewEntityPM = function () {
        var entityPM;
        entityPM = new ObjectTableRulePM_1.ObjectTableRulePM();
        entityPM.Tenant = InfraSettings_1.InfraSettings.TenantPM.Id;
        return entityPM;
    };
    ObjectTableRulePMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ObjectTableRulePMService);
    return ObjectTableRulePMService;
}());
exports.ObjectTableRulePMService = ObjectTableRulePMService;
//# sourceMappingURL=ObjectTableRulePMService.js.map