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
var ServiceHelper_1 = require("../../Utilities/ServiceHelper");
var Rx_1 = require("rxjs/Rx");
var DWSubQueryPM_1 = require("../../EntityPMs/DWSubQueryPM");
var DWQueryData_1 = require("../../../Common/DataContracts/DWQueryData");
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var DWSubQueryPMService = /** @class */ (function () {
    function DWSubQueryPMService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/DWSubQuery';
    }
    DWSubQueryPMService.prototype.insertDWQueryData = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("AdvancedQueryFilter", entityPM);
            var response;
            response = new ServiceResponse_1.ServiceResponse();
            if (errorsArray.length == 0) {
                //var mappedEntity: QueryColumnPM;
                //mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                //////////////////////////////////////////////////////
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entityPM.SubQueryData, false);
                entityPM.SubQueryData = mappedEntity;
                var temp = _this.deepClone(entityPM);
                /////////////////////////////////////////////////////
                return _this._http.post(_this._apiUrl, JSON.stringify(temp), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    if (pm) {
                        //var mappedResult: QueryColumnPM;
                        //mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                        response.Result = pm;
                    }
                    return response; //response;
                });
            }
            else {
                return null; //Observable.of(response);
            }
        });
    };
    DWSubQueryPMService.prototype.UpdateDWQueryData = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("AdvancedQueryFilter", entityPM);
            var response;
            response = new ServiceResponse_1.ServiceResponse();
            if (errorsArray.length == 0) {
                //var mappedEntity: QueryColumnPM;
                //mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                //////////////////////////////////////////////////////
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entityPM.SubQueryData, false);
                entityPM.SubQueryData = mappedEntity;
                var temp = _this.deepClone(entityPM);
                /////////////////////////////////////////////////////
                return _this._http.put(_this._apiUrl, JSON.stringify(temp), { headers: authHeader }).map(function (res) {
                    var pm = res.json();
                    if (pm) {
                        //var mappedResult: QueryColumnPM;
                        //mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                        response.Result = pm;
                    }
                    return response; //response;
                });
            }
            else {
                return null; //Observable.of(response);
            }
        });
    };
    DWSubQueryPMService.prototype.deepClone = function (obj, hash) {
        var _this = this;
        if (hash === void 0) { hash = new WeakMap(); }
        // Do not try to clone primitives or functions
        if (Object(obj) !== obj || obj instanceof Function) {
            return obj;
        }
        if (hash.has(obj)) {
            //return hash.get(obj); // Cyclic reference
            return;
        }
        try { // Try to run constructor (without arguments, as we don't know them)
            var result = new obj.constructor();
        }
        catch (e) { // Constructor failed, create object without running the constructor
            result = Object.create(Object.getPrototypeOf(obj));
        }
        // Optional: support for some standard constructors (extend as desired)
        if (obj instanceof Map) {
            Array.from(obj, function (_a) {
                var key = _a[0], val = _a[1];
                return result.set(_this.deepClone(key, hash), _this.deepClone(val, hash));
            });
        }
        else if (obj instanceof Set) {
            Array.from(obj, function (key) { return result.add(_this.deepClone(key, hash)); });
        }
        // Register in hash    
        hash.set(obj, result);
        // Clone and assign enumerable own properties recursively
        return Object.assign.apply(Object, [result].concat(Object.keys(obj).map(function (key) {
            var _a;
            return (_a = {},
                _a[key] = key != "UIProperties" && key != "MyParentClass" && key != "ShowSampleDateCommand" && key != "Items" && key != "TooltipId" && key != "TooltipContentId" && key != "CurrentSession" ? _this.deepClone(obj[key], hash) : true,
                _a);
        })));
    };
    DWSubQueryPMService.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged" || jsonPMKeys[key] === "MyParentClass" || jsonPMKeys[key] === "ShowSampleDateCommand" || jsonPMKeys[key] === "Items" || jsonPMKeys[key] === "TooltipId" || jsonPMKeys[key] === "TooltipContentId" || jsonPMKeys[key] === "CurrentSession") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    DWSubQueryPMService.prototype.insert = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("AdvancedDWQueryFilter", entityPM);
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
    DWSubQueryPMService.prototype.update = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("AdvancedDWQueryFilter", entityPM);
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
    DWSubQueryPMService.prototype.delete = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = []; //validator.Validate("AdvancedDWQueryFilter", entityPM);
            var response;
            response = new EntityPMServiceResponse_1.EntityPMServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entityPM, false);
                return _this._http.delete(_this._apiUrl + '?id=' + entityPM.Id + '&tenant=' + entityPM.Tenant, { headers: authHeader }).map(function (res) {
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
    DWSubQueryPMService.prototype.get = function (id) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getsingle?' + 'id=' + id, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                var entity = new DWQueryData_1.DWQueryData();
                if (pm) {
                    entity.SubQueryData = pm.SubQueryData; //this.MapJsonToEntityPM(pm);
                    entity.Columns = pm.Columns;
                    entity.Filters = pm.Filters;
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                var servertime = response.headers.get('ServerExecutionTime');
                //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "DWObjectField", "GetSinglePM", 'id=' + id);
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DWSubQueryPMService.prototype.getByQueryId = function (Queryid) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        var callTime = new Date();
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/getByQueryId?' + 'id=' + Queryid, {
                headers: authHeader
            }).map(function (response) {
                var pm = response.json();
                var entity = new DWQueryData_1.DWQueryData();
                if (pm) {
                    entity.SubQueryData = pm.SubQueryData; //this.MapJsonToEntityPM(pm);
                    entity.Columns = pm.Columns;
                    entity.Filters = pm.Filters;
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = entity;
                var servertime = response.headers.get('ServerExecutionTime');
                //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "DWObjectField", "GetSinglePM", 'id=' + id);
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    DWSubQueryPMService.prototype.MapJsonToEntityPM = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new DWSubQueryPM_1.DWSubQueryPM();
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
        //if (getCallMap) {
        //    entityPM.OldEntityPM = this.clone(entityPM);
        //}
        //else {
        //    entityPM.OldEntityPM = null;
        //}
        return entityPM;
    };
    DWSubQueryPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], DWSubQueryPMService);
    return DWSubQueryPMService;
}());
exports.DWSubQueryPMService = DWSubQueryPMService;
//# sourceMappingURL=DWSubQueryPMService.js.map