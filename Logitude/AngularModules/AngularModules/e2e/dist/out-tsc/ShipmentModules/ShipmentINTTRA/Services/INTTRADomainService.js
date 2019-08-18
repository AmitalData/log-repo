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
require("rxjs/add/operator/map");
var Rx_1 = require("rxjs/Rx");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var INTTRASettingPM_1 = require("../../../Common/EntityPMs/INTTRASettingPM");
var BranchPMService_1 = require("../../../Common/Services/StandardPMs/BranchPMService");
var INTTRADomainService = /** @class */ (function () {
    function INTTRADomainService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/INTTRADomain';
    }
    INTTRADomainService.prototype.GetINTTRASettings = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetINTTRASettings';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var itemJSON = response.json();
                var itemMapped = _this.MapINTTRASettingsHelper(itemJSON);
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = itemMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    INTTRADomainService.prototype.UpdateINTTRASettings = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapINTTRASettingsHelper(entityPM, false);
            return _this._http.put(_this._apiUrl + '/PutINTTRASettings', JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapINTTRASettingsHelper(myJsonResult, true, entityPM);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    INTTRADomainService.prototype.MapINTTRASettingsHelper = function (jsonPM, getCallMap, entity) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entity === void 0) { entity = null; }
        if (!entity) {
            entity = new INTTRASettingsHelper();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        var myBranchPMService = new BranchPMService_1.BranchPMService();
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "INTTRASetting") {
                if (jsonPM[property]) {
                    entity[property] = this.MapJsonToINTTRASettingPM(jsonPM[property], getCallMap);
                }
            }
            else if (property === "Branches") {
                entity.Branches = new Array();
                for (var item in jsonPM.Branches) {
                    var jItem = jsonPM.Branches[item];
                    var newBranchPM = myBranchPMService.MapJsonToEntityPM(jItem, getCallMap);
                    entity.Branches.push(newBranchPM);
                }
            }
            else if (property === "Items") {
                entity.Items = new Array();
                for (var item in jsonPM.Items) {
                    var jItem = jsonPM.Items[item];
                    var newItemPM = this.MapJsonToINTTRASettingsHelperItem(jItem, getCallMap);
                    entity.Items.push(newItemPM);
                }
            }
            else {
                entity[property] = jsonPM[property];
            }
        }
        return entity;
    };
    INTTRADomainService.prototype.MapJsonToINTTRASettingPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new INTTRASettingPM_1.INTTRASettingPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    INTTRADomainService.prototype.MapJsonToINTTRASettingsHelperItem = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new INTTRASettingsHelperItem();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    INTTRADomainService.prototype.GetINTTRACommunicationSettings = function () {
        var _this = this;
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        var url = this._apiUrl + '/GetINTTRACommunicationSettings';
        return Rx_1.Observable.defer(function () {
            return _this._http.get(url, { headers: authHeader }).map(function (response) {
                var itemJSON = response.json();
                var itemMapped = _this.MapINTTRACommunicationSettingsHelper(itemJSON);
                var serviceResponse = new ServiceResponse_1.ServiceResponse();
                serviceResponse.Result = itemMapped;
                return serviceResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    INTTRADomainService.prototype.UpdateINTTRACommunicationSettings = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var mappedEntity = _this.MapINTTRACommunicationSettingsHelper(entityPM, false);
            return _this._http.put(_this._apiUrl + '/PutINTTRACommunicationSettings', JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
                var myJsonResult = res.json();
                var mappedResult = _this.MapINTTRACommunicationSettingsHelper(myJsonResult, true, entityPM);
                var myResponse = new ServiceResponse_1.ServiceResponse();
                myResponse.Result = mappedResult;
                return myResponse;
            }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
        });
    };
    INTTRADomainService.prototype.MapINTTRACommunicationSettingsHelper = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new INTTRACommunicationSettingsHelper();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            if (property === "UIProperties") {
                continue;
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        return entityPM;
    };
    INTTRADomainService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], INTTRADomainService);
    return INTTRADomainService;
}());
exports.INTTRADomainService = INTTRADomainService;
var INTTRASettingsHelper = /** @class */ (function () {
    function INTTRASettingsHelper() {
    }
    return INTTRASettingsHelper;
}());
exports.INTTRASettingsHelper = INTTRASettingsHelper;
var INTTRASettingsHelperItem = /** @class */ (function () {
    function INTTRASettingsHelperItem() {
    }
    return INTTRASettingsHelperItem;
}());
exports.INTTRASettingsHelperItem = INTTRASettingsHelperItem;
var INTTRACommunicationSettingsHelper = /** @class */ (function () {
    function INTTRACommunicationSettingsHelper() {
    }
    return INTTRACommunicationSettingsHelper;
}());
exports.INTTRACommunicationSettingsHelper = INTTRACommunicationSettingsHelper;
//# sourceMappingURL=INTTRADomainService.js.map