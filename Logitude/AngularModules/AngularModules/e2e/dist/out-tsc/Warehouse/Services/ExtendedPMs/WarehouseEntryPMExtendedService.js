"use strict";
/// <reference path="../../../infrastructure/datacontracts/customfieldclass.ts" />
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
var ServiceResponse_1 = require("../../../Infrastructure/DataContracts/ServiceResponse");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var WarehouseEntryPackagePM_1 = require("../../EntityPMs/WarehouseEntryPackagePM");
var WarehouseEntryPM_1 = require("../../EntityPMs/WarehouseEntryPM");
var CustomFieldClass_1 = require("../../../Infrastructure/DataContracts/CustomFieldClass");
var WarehouseEntryPMExtendedService = /** @class */ (function () {
    function WarehouseEntryPMExtendedService() {
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this._apiUrl = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + 'api/WarehouseEntryExtended';
    }
    WarehouseEntryPMExtendedService.prototype.GetWarehouseConnectedEntitiesByEntityId = function (entityId) {
        var authHeader = new http_1.Headers();
        authHeader.append('Token', ServiceHelper_1.ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetWarehouseConnectedEntitiesByEntityId" + '?entityId=' + entityId, { headers: authHeader }).map(function (response) {
            var result = response.json();
            var pmresponse;
            pmresponse = new ServiceResponse_1.ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper_1.ServiceHelper.HandleServiceError);
    };
    WarehouseEntryPMExtendedService.prototype.MapJsonToEntityPM = function (jsonPM, mapParent, entityPM) {
        if (mapParent === void 0) { mapParent = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new WarehouseEntryPM_1.WarehouseEntryPM();
        }
        var customFields = [];
        for (var i = 1; i < 11; i++) {
            customFields.push("Field" + i);
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }
            var property = jsonPMKeys[key];
            if (customFields.indexOf(property) > -1) {
                if (jsonPM[property]) {
                    var customFieldClass = new CustomFieldClass_1.CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }
        }
        this.MapWarehouseEntryPackages(entityPM, jsonPM, mapParent); // Call composition tables map methods
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.WarehouseEntryPackages = [];
            for (var item in entityPM.WarehouseEntryPackages) {
                var myWarehouseEntryPackagePM = entityPM.WarehouseEntryPackages[item];
                var newWarehouseEntryPackagePM = this.clone(myWarehouseEntryPackagePM);
                entityPM.OldEntityPM.WarehouseEntryPackages.push(newWarehouseEntryPackagePM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    WarehouseEntryPMExtendedService.prototype.MapWarehouseEntryPackages = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var oldWarehouseEntryPackages = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldWarehouseEntryPackages = entityPM.OldEntityPM.WarehouseEntryPackages;
        }
        entityPM.WarehouseEntryPackages = new Array();
        for (var item in jsonPM.WarehouseEntryPackages) {
            var jItem = jsonPM.WarehouseEntryPackages[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newWarehouseEntryPackagePM;
            if (mapParent) {
                newWarehouseEntryPackagePM = new WarehouseEntryPackagePM_1.WarehouseEntryPackagePM(entityPM);
            }
            else {
                newWarehouseEntryPackagePM = new WarehouseEntryPackagePM_1.WarehouseEntryPackagePM(null);
            }
            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newWarehouseEntryPackagePM[pmProperty] = jItem[pmProperty];
            }
            if (mapParent) {
                newWarehouseEntryPackagePM.UniqueKey = Guid_1.Guid.newGuid();
                newWarehouseEntryPackagePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newWarehouseEntryPackagePM.OldEntityPM = this.clone(newWarehouseEntryPackagePM);
            }
            else {
                if (newWarehouseEntryPackagePM.UniqueKey) {
                    if (jItem.IsDirty)
                        newWarehouseEntryPackagePM.ChangeSetOp = "Update";
                }
                else {
                    newWarehouseEntryPackagePM.ChangeSetOp = "Insert";
                }
                newWarehouseEntryPackagePM.OldEntityPM = null;
                newWarehouseEntryPackagePM.EntityParentPM = null;
            }
            newWarehouseEntryPackagePM.IsDirty = false;
            entityPM.WarehouseEntryPackages.push(newWarehouseEntryPackagePM);
        }
        if (oldWarehouseEntryPackages) {
            for (var itemKey in oldWarehouseEntryPackages) {
                if (entityPM.WarehouseEntryPackages.filter(function (p) { return p.UniqueKey === oldWarehouseEntryPackages[itemKey].UniqueKey; }).length === 0) {
                    if (oldWarehouseEntryPackages[itemKey]) {
                        //oldWarehouseEntryPackages[itemKey].ChangeSetOp = "Delete";
                        //entityPM.WarehouseEntryPackages.push(oldWarehouseEntryPackages[itemKey]);
                        var oldItemJson = oldWarehouseEntryPackages[itemKey];
                        var deletedPM = new WarehouseEntryPackagePM_1.WarehouseEntryPackagePM(null);
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
                        entityPM.WarehouseEntryPackages.push(deletedPM);
                    }
                }
            }
        }
    };
    WarehouseEntryPMExtendedService.prototype.clone = function (jsonPM) {
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
    WarehouseEntryPMExtendedService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], WarehouseEntryPMExtendedService);
    return WarehouseEntryPMExtendedService;
}());
exports.WarehouseEntryPMExtendedService = WarehouseEntryPMExtendedService;
//# sourceMappingURL=WarehouseEntryPMExtendedService.js.map