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
var WarehouseReleasePM_1 = require("../../EntityPMs/WarehouseReleasePM");
var WarehouseReleasePackagePM_1 = require("../../EntityPMs/WarehouseReleasePackagePM");
var WarehouseReleasePMExtendedService = /** @class */ (function () {
    function WarehouseReleasePMExtendedService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/WarehouseReleaseExtended';
    }
    WarehouseReleasePMExtendedService.prototype.Insert = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = validator.Validate("WarehouseRelease", entityPM);
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entityPM, false);
                return _this._http.post(_this._apiUrl + '/postwarehousereleasepm', JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
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
    WarehouseReleasePMExtendedService.prototype.CancelRelease = function (entityPM) {
        var _this = this;
        return Rx_1.Observable.defer(function () {
            var authHeader = new http_1.Headers();
            authHeader.append('Token', SessionInfo_1.SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var validator;
            validator = new ClassLevelValidator_1.ClassLevelValidator();
            var errorsArray = validator.Validate("WarehouseRelease", entityPM);
            var serviceResponse;
            serviceResponse = new ServiceResponse_1.ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity;
                mappedEntity = _this.MapJsonToEntityPM(entityPM, false);
                return _this._http.put(_this._apiUrl + '/PutCancelWarehouseReleasePM', JSON.stringify(mappedEntity), { headers: authHeader }).map(function (res) {
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
    WarehouseReleasePMExtendedService.prototype.GetCrossDockWorkspaceSummary = function (transportModeId, directionId) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetCrossDockWorkspaceSummary?' + 'transportModeId=' + transportModeId + '&directionId=' + directionId, { headers: authHeader }).map(function (response) {
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    WarehouseReleasePMExtendedService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new WarehouseReleasePM_1.WarehouseReleasePM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        var oldWarehouseReleasePackages = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldWarehouseReleasePackages = entityPM.OldEntityPM.WarehouseReleasePackages;
        }
        entityPM.WarehouseReleasePackages = new Array();
        for (var item in jsonPM.WarehouseReleasePackages) {
            var jItem = jsonPM.WarehouseReleasePackages[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newWarehouseReleasePackagePM;
            if (mapParent) {
                newWarehouseReleasePackagePM = new WarehouseReleasePackagePM_1.WarehouseReleasePackagePM(entityPM);
            }
            else {
                newWarehouseReleasePackagePM = new WarehouseReleasePackagePM_1.WarehouseReleasePackagePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newWarehouseReleasePackagePM[pmProperty] = jItem[pmProperty];
            }
            newWarehouseReleasePackagePM.IsDirty = false;
            if (mapParent) {
                newWarehouseReleasePackagePM.OldEntityPM = this.clone(newWarehouseReleasePackagePM);
                newWarehouseReleasePackagePM.UniqueKey = Guid_1.Guid.newGuid();
                newWarehouseReleasePackagePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
            }
            else {
                if (newWarehouseReleasePackagePM.UniqueKey) {
                    if (jItem.IsDirty)
                        newWarehouseReleasePackagePM.ChangeSetOp = "Update";
                }
                else {
                    newWarehouseReleasePackagePM.ChangeSetOp = "Insert";
                }
                newWarehouseReleasePackagePM.OldEntityPM = null;
                newWarehouseReleasePackagePM.EntityParentPM = null;
            }
            entityPM.WarehouseReleasePackages.push(newWarehouseReleasePackagePM);
        }
        if (oldWarehouseReleasePackages) {
            for (var itemKey in oldWarehouseReleasePackages) {
                if (entityPM.WarehouseReleasePackages.filter(function (p) { return p.UniqueKey === oldWarehouseReleasePackages[itemKey].UniqueKey; }).length === 0) {
                    if (oldWarehouseReleasePackages[itemKey]) {
                        oldWarehouseReleasePackages[itemKey].ChangeSetOp = "Delete";
                        entityPM.WarehouseReleasePackages.push(oldWarehouseReleasePackages[itemKey]);
                    }
                }
            }
        }
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.WarehouseReleasePackages = [];
            for (var m in entityPM.WarehouseReleasePackages) {
                entityPM.OldEntityPM.WarehouseReleasePackages.push(this.clone(entityPM.WarehouseReleasePackages[m]));
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        return entityPM;
    };
    WarehouseReleasePMExtendedService.prototype.clone = function (jsonPM) {
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
    WarehouseReleasePMExtendedService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], WarehouseReleasePMExtendedService);
    return WarehouseReleasePMExtendedService;
}());
exports.WarehouseReleasePMExtendedService = WarehouseReleasePMExtendedService;
//# sourceMappingURL=WarehouseReleasePMExtendedService.js.map