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
var EntityListService = /** @class */ (function () {
    function EntityListService() {
    }
    EntityListService.prototype.getCount = function (objectTableName) {
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ListService";
        var servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getCount());
            });
        });
    };
    EntityListService.prototype.getAll = function (objectTableName) {
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ListService";
        var servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getAll());
            });
        });
    };
    EntityListService.prototype.getByFilters = function (objectTableName, filters, MethodName) {
        if (MethodName === void 0) { MethodName = null; }
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ListService";
        var servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;
        if (MethodName != null) {
            if (MethodName.indexOf('Customs.') > -1) {
                MethodName = MethodName.split('.')[1];
            }
            servicename = MethodName + "ListService";
            servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;
        }
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getByFilters(filters));
            });
        });
    };
    EntityListService.prototype.getByCompactFilters = function (objectTableName, filters, MethodName) {
        if (MethodName === void 0) { MethodName = null; }
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ListService";
        var servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;
        if (MethodName != null) {
            if (MethodName.indexOf('Customs.') > -1) {
                MethodName = MethodName.split('.')[1];
            }
            servicename = MethodName + "ListService";
            servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;
        }
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getByCompactFilters(filters));
            });
        });
    };
    EntityListService.prototype.getCustomByFilters = function (objectTableName, filters, MethodName) {
        if (MethodName === void 0) { MethodName = null; }
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ExtendedListService";
        var servicelink = './' + moduleName + '/Services/ExtendedLists/' + servicename;
        if (MethodName != null) {
            if (MethodName.indexOf('Customs.') > -1) {
                MethodName = MethodName.split('.')[1];
            }
            servicename = MethodName + "ListService";
            servicelink = './' + moduleName + '/Services/ExtendedLists/' + servicename;
        }
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getTenantImportByFilters(filters));
            });
        });
    };
    EntityListService.prototype.getExtendedByFilters = function (objectTableName, filters, MethodName) {
        if (MethodName === void 0) { MethodName = null; }
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ExtendedListService";
        var servicelink = './' + moduleName + '/Services/ExtendedLists/' + servicename;
        if (MethodName != null) {
            if (MethodName.indexOf('Customs.') > -1) {
                MethodName = MethodName.split('.')[1];
            }
            servicename = MethodName + "ListService";
            servicelink = './' + moduleName + '/Services/ExtendedLists/' + servicename;
        }
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getByFilters(filters));
            });
        });
    };
    EntityListService.prototype.getOpenReconciliationsByFilter = function (objectTableName, accountId, filters) {
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ExtendedListService";
        var servicelink = './' + moduleName + '/Services/ExtendedLists/' + servicename;
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getOpenReconciliationsByFilter(accountId, filters));
            });
        });
    };
    EntityListService.prototype.getReconciliationsByFilter = function (objectTableName, accountId, filters) {
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ExtendedListService";
        var servicelink = './' + moduleName + '/Services/ExtendedLists/' + servicename;
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getReconciliationsByFilter(accountId, filters));
            });
        });
    };
    EntityListService.prototype.getExternalReoncilioationsByFilter = function (objectTableName, bankAccountId, filters) {
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ExtendedListService";
        var servicelink = './' + moduleName + '/Services/ExtendedLists/' + servicename;
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getExternalReoncilioationsByFilter(bankAccountId, filters));
            });
        });
    };
    EntityListService.prototype.getMock = function (objectTableName) {
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ExtendedListService";
        var servicelink = './' + moduleName + '/Services/ExtendedLists/' + servicename;
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getMock());
            });
        });
    };
    //GetServicelink(myTableName: string) {
    //    var myResult: string;
    //    if (myTableName != null) {
    //        var R: string = myTableName.toLowerCase();
    //        var myServiceName = myTableName + "ListService";
    //        switch (R) {
    //            case "address":
    //            case "branch":
    //            case "card":
    //            case "chargestype":
    //            case "contact":
    //            case "incoterm":
    //            case "packagetype":
    //            case "port":
    //            case "user":
    //            case "measurement":
    //                {
    //                    myResult = "./Common/Services/StandardLists/" + myServiceName;
    //                    break;
    //                }
    //            case "movetype":
    //            case "prepaidcollect":
    //                {
    //                myResult = "./Infrastructure/Services/StandardLists/" + myServiceName;
    //                break;
    //            }
    //        }
    //    }
    //    return myResult;
    //}
    EntityListService.prototype.getSingleFromCache = function (key, objectTableName, filters) {
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ListService";
        var servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getSingleFromCache(key));
            });
        });
    };
    EntityListService.prototype.getAllFromCache = function (objectTableName, filters) {
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ListService";
        var servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                try {
                    resolve(service.getAllFromCache(filters));
                }
                catch (e) {
                    reject(new Error("service.getAllFromCache is not a function: " + service._apiUrl));
                    //console.log(e);
                }
            });
        });
    };
    EntityListService.prototype.getSingle = function (key, objectTableName, MethodName) {
        if (MethodName === void 0) { MethodName = null; }
        var table = window.ObjectTables.filter(function (d) { return d.Name === objectTableName; })[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ListService";
        var servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;
        if (MethodName != null) {
            servicename = MethodName + "ListService";
            servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;
        }
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getSingle(key));
            });
        });
    };
    EntityListService.prototype.getEntityCopyToCurrentTenant = function (zeroEntityId, tableName) {
        if (tableName == 'Port') {
            var servicelink = './Common/Services/ExtendedLists/PortService';
            return new Promise(function (resolve, reject) {
                SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                    resolve(service.GetPortCopyToCurrentTenant(zeroEntityId));
                });
            });
        }
        if (tableName == 'Carrier') {
            var servicelink = './Common/Services/StandardLists/CarrierListService';
            return new Promise(function (resolve, reject) {
                SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                    resolve(service.GetCarrierCopyToCurrentTenant(zeroEntityId));
                });
            });
        }
    };
    EntityListService.prototype.getDWDimByFilters = function (objectTableName, filters) {
        var servicelink = './Infrastructure/Services/ExtendedPMs/DWQueryBuilderService';
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getByFilters(filters));
            });
        });
    };
    EntityListService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], EntityListService);
    return EntityListService;
}());
exports.EntityListService = EntityListService;
//# sourceMappingURL=EntityListService.js.map