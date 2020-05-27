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
var SessionLocator_1 = require("../Utilities/SessionLocator");
var EntityPMService = /** @class */ (function () {
    function EntityPMService() {
    }
    EntityPMService.prototype.getSingle = function (objectTableName, id) {
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "PMService";
        var servicelink = './' + moduleName + '/Services/StandardPMs/' + servicename;
        return new Promise(function (resolve) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.get(id));
            });
        });
    };
    EntityPMService.prototype.getNewEntity = function (objectTableName) {
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "PMService";
        var servicelink = './' + moduleName + '/Services/StandardPMs/' + servicename;
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.GetNewEntityPM());
            });
        });
    };
    EntityPMService.prototype.update = function (objectTableName, entityPM) {
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "PMService";
        var servicelink = './' + moduleName + '/Services/StandardPMs/' + servicename;
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.update(entityPM));
            });
        });
    };
    EntityPMService.prototype.insert = function (objectTableName, entityPM) {
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "PMService";
        var servicelink = './' + moduleName + '/Services/StandardPMs/' + servicename;
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.insert(entityPM));
            });
        });
    };
    EntityPMService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], EntityPMService);
    return EntityPMService;
}());
exports.EntityPMService = EntityPMService;
//# sourceMappingURL=EntityPMService.js.map