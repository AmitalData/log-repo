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
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../Infrastructure/DataContracts/ServiceResponse");
var CounterPM_1 = require("../EntityPMs/CounterPM");
var CounterDefinitionPM_1 = require("../EntityPMs/CounterDefinitionPM");
var TenantSettingPM_1 = require("../../Infrastructure/EntityPMs/TenantSettingPM");
var CountersDomainService = /** @class */ (function () {
    function CountersDomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/CountersDomain';
    }
    CountersDomainService.prototype.GetTenantCounters = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetTenantCounters', { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var _mappedArray = [];
                for (var key in listJason) {
                    var entity;
                    entity = _this.MapJsonToCounterPM(listJason[key]);
                    _mappedArray.push(entity);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CountersDomainService.prototype.GetCounterDefinitions = function (CounterId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return Rx_1.Observable.defer(function () {
            return _this._http.get(_this._apiUrl + '/GetCounterDefinitions?CounterId=' + CounterId, { headers: authHeader }).map(function (response) {
                var listJason = response.json();
                var _mappedArray = [];
                for (var key in listJason) {
                    var entity;
                    entity = _this.MapJsonToCounterDefinitionPM(listJason[key]);
                    _mappedArray.push(entity);
                }
                var serviceResponse;
                serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = _mappedArray;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CountersDomainService.prototype.GetCounterAPIHelper = function (CounterId) {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetCounterAPIHelper?CounterId=' + CounterId;
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var myJsonResult = response.json();
                var mappedResult = _this.MapJsonToCounterAPIHelper(myJsonResult, true);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CountersDomainService.prototype.Post = function (args) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapJsonToCounterAPIHelper(args, false);
            return _this._http.post(_this._apiUrl, JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapJsonToCounterAPIHelper(myJsonResult, true);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    CountersDomainService.prototype.MapJsonToCounterAPIHelper = function (jsonPM, getCallMap, entity) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entity === void 0) { entity = null; }
        if (!entity) {
            entity = new CounterAPIHelper();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "CounterPM") {
                entity.CounterPM = this.MapJsonToCounterPM(jsonPM.CounterPM, getCallMap);
            }
            else if (property === "TenantSettings") {
                entity.TenantSettings = new Array();
                for (var item_Setting in jsonPM.TenantSettings) {
                    var jItem_Setting = jsonPM.TenantSettings[item_Setting];
                    var newItemPM_Setting = this.MapJsonToTenantSettingPM(jItem_Setting, getCallMap);
                    entity.TenantSettings.push(newItemPM_Setting);
                }
            }
            else if (property === "CounterDefinitions") {
                entity.CounterDefinitions = new Array();
                for (var item_Definition in jsonPM.CounterDefinitions) {
                    var jItem_Definition = jsonPM.CounterDefinitions[item_Definition];
                    var newItemPM_Definition = this.MapJsonToCounterDefinitionPM(jItem_Definition, getCallMap);
                    entity.CounterDefinitions.push(newItemPM_Definition);
                }
            }
            else {
                entity[property] = jsonPM[property];
            }
        }
        return entity;
    };
    CountersDomainService.prototype.MapJsonToCounterPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new CounterPM_1.CounterPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
            entityPM.IsDirty = false;
            if (mapParent) {
                entityPM.OldEntityPM = this.clone(entityPM);
            }
            else {
                entityPM.OldEntityPM = null;
            }
        }
        return entityPM;
    };
    CountersDomainService.prototype.MapJsonToTenantSettingPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new TenantSettingPM_1.TenantSettingPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
            entityPM.IsDirty = false;
            if (mapParent) {
                entityPM.OldEntityPM = this.clone(entityPM);
            }
            else {
                entityPM.OldEntityPM = null;
            }
        }
        return entityPM;
    };
    CountersDomainService.prototype.MapJsonToCounterDefinitionPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new CounterDefinitionPM_1.CounterDefinitionPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
            entityPM.IsDirty = false;
            if (mapParent) {
                entityPM.OldEntityPM = this.clone(entityPM);
            }
            else {
                entityPM.OldEntityPM = null;
            }
        }
        return entityPM;
    };
    CountersDomainService.prototype.clone = function (jsonPM) {
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
    CountersDomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], CountersDomainService);
    return CountersDomainService;
}());
exports.CountersDomainService = CountersDomainService;
var CounterAPIHelper = /** @class */ (function () {
    function CounterAPIHelper() {
        this.TenantSettings = [];
        this.CounterDefinitions = [];
    }
    return CounterAPIHelper;
}());
exports.CounterAPIHelper = CounterAPIHelper;
//# sourceMappingURL=CountersDomainService.js.map