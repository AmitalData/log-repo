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
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var Rx_1 = require("rxjs/Rx");
var QueryPM_1 = require("../../EntityPMs/QueryPM");
var SharedUserQueryPM_1 = require("../../EntityPMs/SharedUserQueryPM");
var SessionInfo_1 = require("../../Utilities/SessionInfo");
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var QueriesPMService = /** @class */ (function () {
    function QueriesPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/objectfields';
    }
    QueriesPMService.prototype.setServiceArgs = function (serviceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/queries';
    };
    QueriesPMService.prototype.get = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getsingle?' + 'id=' + id, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                var entity;
                if (pm) {
                    entity = _this.MapJsonToEntityPM(pm);
                }
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    QueriesPMService.prototype.insert = function (entityPM) {
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
                return Rx_1.Observable.of(response);
            }
        });
    };
    QueriesPMService.prototype.update = function (entityPM) {
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
                mappedEntity = _this.MapJsonToEntityPM(entityPM, false);
                return _this._http.put(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
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
                return Rx_1.Observable.of(response);
            }
        });
    };
    QueriesPMService.prototype.delete = function (entityPM, userId) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = [];
            var response;
            response = new EntityPMServiceResponse_1.EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entityPM, false);
                return _this._http.delete(_this._apiUrl + '?id=' + entityPM.Id + '&userId=' + userId, { headers: authHeader }).map(function (res) {
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
                return Rx_1.Observable.of(response);
            }
        });
    };
    QueriesPMService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new QueryPM_1.QueryPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        this.MapSharedUserQueies(entityPM, jsonPM, mapParent);
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.SharedUserQueries = [];
            for (var item in entityPM.SharedUserQueries) {
                var mySharedUserQueryPM = entityPM.SharedUserQueries[item];
                var newSharedUserQueryPM = this.clone(mySharedUserQueryPM);
                entityPM.OldEntityPM.SharedUserQueries.push(newSharedUserQueryPM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    QueriesPMService.prototype.MapSharedUserQueies = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldSharedUserQueries = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSharedUserQueries = entityPM.OldEntityPM.SharedUserQueries;
        }
        entityPM.SharedUserQueries = new Array();
        for (var item in jsonPM.SharedUserQueries) {
            var jItem = jsonPM.SharedUserQueries[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSharedUserQueryPM;
            if (mapParent) {
                newSharedUserQueryPM = new SharedUserQueryPM_1.SharedUserQueryPM(entityPM);
            }
            else {
                newSharedUserQueryPM = new SharedUserQueryPM_1.SharedUserQueryPM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSharedUserQueryPM[pmProperty] = jItem[pmProperty];
            }
            newSharedUserQueryPM.IsDirty = false;
            if (mapParent) {
                newSharedUserQueryPM.UniqueKey = Guid_1.Guid.newGuid();
                newSharedUserQueryPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSharedUserQueryPM.OldEntityPM = this.clone(newSharedUserQueryPM);
            }
            else {
                if (newSharedUserQueryPM.UniqueKey) {
                    if (jItem.IsDirty)
                        newSharedUserQueryPM.ChangeSetOp = "Update";
                }
                else {
                    newSharedUserQueryPM.ChangeSetOp = "Insert";
                }
                newSharedUserQueryPM.OldEntityPM = null;
                newSharedUserQueryPM.EntityParentPM = null;
            }
            newSharedUserQueryPM.IsDirty = false;
            entityPM.SharedUserQueries.push(newSharedUserQueryPM);
        }
        if (oldSharedUserQueries) {
            for (var itemKey in oldSharedUserQueries) {
                if (entityPM.SharedUserQueries.filter(function (p) { return p.UniqueKey === oldSharedUserQueries[itemKey].UniqueKey; }).length === 0) {
                    if (oldSharedUserQueries[itemKey]) {
                        var oldItemJson = oldSharedUserQueries[itemKey];
                        var deletedPM = new SharedUserQueryPM_1.SharedUserQueryPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {
                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }
                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";
                        deletedPM.OldEntityPM = null;
                        entityPM.SharedUserQueries.push(deletedPM);
                    }
                }
            }
        }
    };
    QueriesPMService.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    QueriesPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], QueriesPMService);
    return QueriesPMService;
}());
exports.QueriesPMService = QueriesPMService;
//# sourceMappingURL=QueriesPMService.js.map