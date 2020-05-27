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
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var ClassLevelValidator_1 = require("../../../Infrastructure/Validators/ClassLevelValidator");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var ClaimsRelatedEntityPM_1 = require("../../EntityPMs/ClaimsRelatedEntityPM");
var ClaimsRelatedEntitiesAmountPM_1 = require("../../EntityPMs/ClaimsRelatedEntitiesAmountPM");
var ClaimsRelatedEntitiesReasonPM_1 = require("../../EntityPMs/ClaimsRelatedEntitiesReasonPM");
var ClaimsRelatedEntsReasonsExpPM_1 = require("../../EntityPMs/ClaimsRelatedEntsReasonsExpPM");
var ClaimsRelatedEntsExpDeclarPM_1 = require("../../EntityPMs/ClaimsRelatedEntsExpDeclarPM");
var ClaimsRelatedEntityExtendedPMService = /** @class */ (function () {
    function ClaimsRelatedEntityExtendedPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/claimsRelatedEntities';
    }
    ClaimsRelatedEntityExtendedPMService.prototype.insert = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = validator.Validate("Customs.ClaimsRelatedEntity", entityPM);
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
    ClaimsRelatedEntityExtendedPMService.prototype.update = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = validator.Validate("Customs.ClaimsRelatedEntity", entityPM);
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
    ClaimsRelatedEntityExtendedPMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new ClaimsRelatedEntityPM_1.ClaimsRelatedEntityPM(null);
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        this.MapClaimsRelatedEntitiesAmounts(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapClaimsRelatedEntitiesReasons(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapClaimsRelatedEntsExpDeclars(entityPM, jsonPM, mapParent); // Call composition tables map methods
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.ClaimsRelatedEntitiesAmounts = [];
            for (var k in entityPM.ClaimsRelatedEntitiesAmounts) {
                var myClaimsRelatedEntitiesAmountPM = entityPM.ClaimsRelatedEntitiesAmounts[k];
                var newClaimsRelatedEntitiesAmountPM = this.clone(myClaimsRelatedEntitiesAmountPM);
                entityPM.OldEntityPM.ClaimsRelatedEntitiesAmounts.push(newClaimsRelatedEntitiesAmountPM);
            }
            entityPM.OldEntityPM.ClaimsRelatedEntitiesReasons = [];
            for (var k in entityPM.ClaimsRelatedEntitiesReasons) {
                var myClaimsRelatedEntitiesReasonPM = entityPM.ClaimsRelatedEntitiesReasons[k];
                var newClaimsRelatedEntitiesReasonPM = this.clone(myClaimsRelatedEntitiesReasonPM);
                newClaimsRelatedEntitiesReasonPM.ClaimsRelatedEntsReasonsExps = [];
                for (var k in myClaimsRelatedEntitiesReasonPM.ClaimsRelatedEntsReasonsExps) {
                    var myClaimsRelatedEntsReasonsExpPM = myClaimsRelatedEntitiesReasonPM.ClaimsRelatedEntsReasonsExps[k];
                    var newClaimsRelatedEntsReasonsExpPM = this.clone(myClaimsRelatedEntsReasonsExpPM);
                    newClaimsRelatedEntitiesReasonPM.ClaimsRelatedEntsReasonsExps.push(newClaimsRelatedEntsReasonsExpPM);
                }
                entityPM.OldEntityPM.ClaimsRelatedEntitiesReasons.push(newClaimsRelatedEntitiesReasonPM);
            }
            entityPM.OldEntityPM.ClaimsRelatedEntsExpDeclars = [];
            for (var k in entityPM.ClaimsRelatedEntsExpDeclars) {
                var myClaimsRelatedEntsExpDeclarPM = entityPM.ClaimsRelatedEntsExpDeclars[k];
                var newClaimsRelatedEntsExpDeclarPM = this.clone(myClaimsRelatedEntsExpDeclarPM);
                entityPM.OldEntityPM.ClaimsRelatedEntsExpDeclars.push(newClaimsRelatedEntsExpDeclarPM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    ClaimsRelatedEntityExtendedPMService.prototype.MapClaimsRelatedEntitiesAmounts = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldClaimsRelatedEntitiesAmounts = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldClaimsRelatedEntitiesAmounts = entityPM.OldEntityPM.ClaimsRelatedEntitiesAmounts;
        }
        entityPM.ClaimsRelatedEntitiesAmounts = new Array();
        for (var item in jsonPM.ClaimsRelatedEntitiesAmounts) {
            var jItem = jsonPM.ClaimsRelatedEntitiesAmounts[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newClaimsRelatedEntitiesAmountPM;
            if (mapParent) {
                newClaimsRelatedEntitiesAmountPM = new ClaimsRelatedEntitiesAmountPM_1.ClaimsRelatedEntitiesAmountPM(entityPM);
            }
            else {
                newClaimsRelatedEntitiesAmountPM = new ClaimsRelatedEntitiesAmountPM_1.ClaimsRelatedEntitiesAmountPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newClaimsRelatedEntitiesAmountPM[pmProperty] = jItem[pmProperty];
            }
            newClaimsRelatedEntitiesAmountPM.IsDirty = false;
            if (mapParent) {
                newClaimsRelatedEntitiesAmountPM.UniqueKey = Guid_1.Guid.newGuid();
                newClaimsRelatedEntitiesAmountPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newClaimsRelatedEntitiesAmountPM.OldEntityPM = this.clone(newClaimsRelatedEntitiesAmountPM);
            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newClaimsRelatedEntitiesAmountPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newClaimsRelatedEntitiesAmountPM.UniqueKey) {
                        if (jItem.IsDirty)
                            newClaimsRelatedEntitiesAmountPM.ChangeSetOp = "Update";
                    }
                    else {
                        newClaimsRelatedEntitiesAmountPM.ChangeSetOp = "Insert";
                    }
                }
                newClaimsRelatedEntitiesAmountPM.OldEntityPM = null;
                newClaimsRelatedEntitiesAmountPM.EntityParentPM = null;
            }
            entityPM.ClaimsRelatedEntitiesAmounts.push(newClaimsRelatedEntitiesAmountPM);
        }
        if (oldClaimsRelatedEntitiesAmounts) {
            for (var itemKey in oldClaimsRelatedEntitiesAmounts) {
                if (entityPM.ClaimsRelatedEntitiesAmounts.filter(function (p) { return p.UniqueKey === oldClaimsRelatedEntitiesAmounts[itemKey].UniqueKey; }).length === 0) {
                    if (oldClaimsRelatedEntitiesAmounts[itemKey]) {
                        //oldClaimsRelatedEntitiesAmounts[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ClaimsRelatedEntitiesAmounts.push(oldClaimsRelatedEntitiesAmounts[itemKey]);
                        var oldItemJson = oldClaimsRelatedEntitiesAmounts[itemKey];
                        var deletedPM = new ClaimsRelatedEntitiesAmountPM_1.ClaimsRelatedEntitiesAmountPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {
                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }
                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        deletedPM.OldEntityPM = null;
                        entityPM.ClaimsRelatedEntitiesAmounts.push(deletedPM);
                    }
                }
            }
        }
    };
    ClaimsRelatedEntityExtendedPMService.prototype.MapClaimsRelatedEntitiesReasons = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldClaimsRelatedEntitiesReasons = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldClaimsRelatedEntitiesReasons = entityPM.OldEntityPM.ClaimsRelatedEntitiesReasons;
        }
        entityPM.ClaimsRelatedEntitiesReasons = new Array();
        for (var item in jsonPM.ClaimsRelatedEntitiesReasons) {
            var jItem = jsonPM.ClaimsRelatedEntitiesReasons[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newClaimsRelatedEntitiesReasonPM;
            if (mapParent) {
                newClaimsRelatedEntitiesReasonPM = new ClaimsRelatedEntitiesReasonPM_1.ClaimsRelatedEntitiesReasonPM(entityPM);
            }
            else {
                newClaimsRelatedEntitiesReasonPM = new ClaimsRelatedEntitiesReasonPM_1.ClaimsRelatedEntitiesReasonPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newClaimsRelatedEntitiesReasonPM[pmProperty] = jItem[pmProperty];
            }
            newClaimsRelatedEntitiesReasonPM.IsDirty = false;
            if (mapParent) {
                newClaimsRelatedEntitiesReasonPM.UniqueKey = Guid_1.Guid.newGuid();
                newClaimsRelatedEntitiesReasonPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newClaimsRelatedEntitiesReasonPM.OldEntityPM = this.clone(newClaimsRelatedEntitiesReasonPM);
                this.MapClaimsRelatedEntsReasonsExps(newClaimsRelatedEntitiesReasonPM, jItem, mapParent);
                newClaimsRelatedEntitiesReasonPM.OldEntityPM.ClaimsRelatedEntsReasonsExps = [];
                for (var k in newClaimsRelatedEntitiesReasonPM.ClaimsRelatedEntsReasonsExps) {
                    var clonedInside = this.clone(newClaimsRelatedEntitiesReasonPM.ClaimsRelatedEntsReasonsExps[k]);
                    newClaimsRelatedEntitiesReasonPM.OldEntityPM.ClaimsRelatedEntsReasonsExps.push(clonedInside); // clone old ClaimsRelatedEntsReasonsExps//
                }
            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newClaimsRelatedEntitiesReasonPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newClaimsRelatedEntitiesReasonPM.UniqueKey) {
                        if (jItem.IsDirty)
                            newClaimsRelatedEntitiesReasonPM.ChangeSetOp = "Update";
                    }
                    else {
                        newClaimsRelatedEntitiesReasonPM.ChangeSetOp = "Insert";
                    }
                }
                this.MapClaimsRelatedEntsReasonsExps(newClaimsRelatedEntitiesReasonPM, jItem, mapParent);
                newClaimsRelatedEntitiesReasonPM.OldEntityPM = null;
                newClaimsRelatedEntitiesReasonPM.EntityParentPM = null;
            }
            entityPM.ClaimsRelatedEntitiesReasons.push(newClaimsRelatedEntitiesReasonPM);
        }
        if (oldClaimsRelatedEntitiesReasons) {
            for (var itemKey in oldClaimsRelatedEntitiesReasons) {
                if (entityPM.ClaimsRelatedEntitiesReasons.filter(function (p) { return p.UniqueKey === oldClaimsRelatedEntitiesReasons[itemKey].UniqueKey; }).length === 0) {
                    if (oldClaimsRelatedEntitiesReasons[itemKey]) {
                        //oldClaimsRelatedEntitiesReasons[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ClaimsRelatedEntitiesReasons.push(oldClaimsRelatedEntitiesReasons[itemKey]);
                        var oldItemJson = oldClaimsRelatedEntitiesReasons[itemKey];
                        var deletedPM = new ClaimsRelatedEntitiesReasonPM_1.ClaimsRelatedEntitiesReasonPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {
                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }
                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        this.MapClaimsRelatedEntsReasonsExps(deletedPM, oldItemJson, mapParent);
                        deletedPM.OldEntityPM = null;
                        entityPM.ClaimsRelatedEntitiesReasons.push(deletedPM);
                    }
                }
            }
        }
    };
    ClaimsRelatedEntityExtendedPMService.prototype.MapClaimsRelatedEntsReasonsExps = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldClaimsRelatedEntsReasonsExps = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldClaimsRelatedEntsReasonsExps = entityPM.OldEntityPM.ClaimsRelatedEntsReasonsExps;
        }
        entityPM.ClaimsRelatedEntsReasonsExps = new Array();
        for (var item in jsonPM.ClaimsRelatedEntsReasonsExps) {
            var jItem = jsonPM.ClaimsRelatedEntsReasonsExps[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newClaimsRelatedEntsReasonsExpPM;
            if (mapParent) {
                newClaimsRelatedEntsReasonsExpPM = new ClaimsRelatedEntsReasonsExpPM_1.ClaimsRelatedEntsReasonsExpPM(entityPM);
            }
            else {
                newClaimsRelatedEntsReasonsExpPM = new ClaimsRelatedEntsReasonsExpPM_1.ClaimsRelatedEntsReasonsExpPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newClaimsRelatedEntsReasonsExpPM[pmProperty] = jItem[pmProperty];
            }
            newClaimsRelatedEntsReasonsExpPM.IsDirty = false;
            if (mapParent) {
                newClaimsRelatedEntsReasonsExpPM.UniqueKey = Guid_1.Guid.newGuid();
                newClaimsRelatedEntsReasonsExpPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newClaimsRelatedEntsReasonsExpPM.OldEntityPM = this.clone(newClaimsRelatedEntsReasonsExpPM);
            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newClaimsRelatedEntsReasonsExpPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newClaimsRelatedEntsReasonsExpPM.UniqueKey) {
                        if (jItem.IsDirty)
                            newClaimsRelatedEntsReasonsExpPM.ChangeSetOp = "Update";
                    }
                    else {
                        newClaimsRelatedEntsReasonsExpPM.ChangeSetOp = "Insert";
                    }
                }
                newClaimsRelatedEntsReasonsExpPM.OldEntityPM = null;
                newClaimsRelatedEntsReasonsExpPM.EntityParentPM = null;
            }
            entityPM.ClaimsRelatedEntsReasonsExps.push(newClaimsRelatedEntsReasonsExpPM);
        }
        if (oldClaimsRelatedEntsReasonsExps) {
            for (var itemKey in oldClaimsRelatedEntsReasonsExps) {
                if (entityPM.ClaimsRelatedEntsReasonsExps.filter(function (p) { return p.UniqueKey === oldClaimsRelatedEntsReasonsExps[itemKey].UniqueKey; }).length === 0) {
                    if (oldClaimsRelatedEntsReasonsExps[itemKey]) {
                        //oldClaimsRelatedEntsReasonsExps[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ClaimsRelatedEntsReasonsExps.push(oldClaimsRelatedEntsReasonsExps[itemKey]);
                        var oldItemJson = oldClaimsRelatedEntsReasonsExps[itemKey];
                        var deletedPM = new ClaimsRelatedEntsReasonsExpPM_1.ClaimsRelatedEntsReasonsExpPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {
                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }
                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        deletedPM.OldEntityPM = null;
                        entityPM.ClaimsRelatedEntsReasonsExps.push(deletedPM);
                    }
                }
            }
        }
    };
    ClaimsRelatedEntityExtendedPMService.prototype.MapClaimsRelatedEntsExpDeclars = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldClaimsRelatedEntsExpDeclars = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldClaimsRelatedEntsExpDeclars = entityPM.OldEntityPM.ClaimsRelatedEntsExpDeclars;
        }
        entityPM.ClaimsRelatedEntsExpDeclars = new Array();
        for (var item in jsonPM.ClaimsRelatedEntsExpDeclars) {
            var jItem = jsonPM.ClaimsRelatedEntsExpDeclars[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newClaimsRelatedEntsExpDeclarPM;
            if (mapParent) {
                newClaimsRelatedEntsExpDeclarPM = new ClaimsRelatedEntsExpDeclarPM_1.ClaimsRelatedEntsExpDeclarPM(entityPM);
            }
            else {
                newClaimsRelatedEntsExpDeclarPM = new ClaimsRelatedEntsExpDeclarPM_1.ClaimsRelatedEntsExpDeclarPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newClaimsRelatedEntsExpDeclarPM[pmProperty] = jItem[pmProperty];
            }
            newClaimsRelatedEntsExpDeclarPM.IsDirty = false;
            if (mapParent) {
                newClaimsRelatedEntsExpDeclarPM.UniqueKey = Guid_1.Guid.newGuid();
                newClaimsRelatedEntsExpDeclarPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newClaimsRelatedEntsExpDeclarPM.OldEntityPM = this.clone(newClaimsRelatedEntsExpDeclarPM);
            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newClaimsRelatedEntsExpDeclarPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newClaimsRelatedEntsExpDeclarPM.UniqueKey) {
                        if (jItem.IsDirty)
                            newClaimsRelatedEntsExpDeclarPM.ChangeSetOp = "Update";
                    }
                    else {
                        newClaimsRelatedEntsExpDeclarPM.ChangeSetOp = "Insert";
                    }
                }
                newClaimsRelatedEntsExpDeclarPM.OldEntityPM = null;
                newClaimsRelatedEntsExpDeclarPM.EntityParentPM = null;
            }
            entityPM.ClaimsRelatedEntsExpDeclars.push(newClaimsRelatedEntsExpDeclarPM);
        }
        if (oldClaimsRelatedEntsExpDeclars) {
            for (var itemKey in oldClaimsRelatedEntsExpDeclars) {
                if (entityPM.ClaimsRelatedEntsExpDeclars.filter(function (p) { return p.UniqueKey === oldClaimsRelatedEntsExpDeclars[itemKey].UniqueKey; }).length === 0) {
                    if (oldClaimsRelatedEntsExpDeclars[itemKey]) {
                        //oldClaimsRelatedEntsExpDeclars[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ClaimsRelatedEntsExpDeclars.push(oldClaimsRelatedEntsExpDeclars[itemKey]);
                        var oldItemJson = oldClaimsRelatedEntsExpDeclars[itemKey];
                        var deletedPM = new ClaimsRelatedEntsExpDeclarPM_1.ClaimsRelatedEntsExpDeclarPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {
                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }
                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        deletedPM.OldEntityPM = null;
                        entityPM.ClaimsRelatedEntsExpDeclars.push(deletedPM);
                    }
                }
            }
        }
    };
    ClaimsRelatedEntityExtendedPMService.prototype.clone = function (jsonPM) {
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
    ClaimsRelatedEntityExtendedPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], ClaimsRelatedEntityExtendedPMService);
    return ClaimsRelatedEntityExtendedPMService;
}());
exports.ClaimsRelatedEntityExtendedPMService = ClaimsRelatedEntityExtendedPMService;
//# sourceMappingURL=ClaimsRelatedEntityExtendedPMService.js.map