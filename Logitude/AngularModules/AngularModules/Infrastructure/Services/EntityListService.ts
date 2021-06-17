declare var window: any;
import {Injectable} from '@angular/core';
import {ApiQueryFilters} from '../DataContracts/ApiQueryFilters';
import {ServiceHelper} from '../Utilities/ServiceHelper';
import {SessionLocator} from '../Utilities/SessionLocator';

@Injectable()

export class EntityListService {
    constructor() {

    }

    getCount(objectTableName: string) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ListService";
        var servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;

        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getCount());
            });
        });
    }

    getAll(objectTableName: string) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ListService";
        var servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;

        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getAll());
            });
        });
    }

    getByFilters(objectTableName: string, filters: ApiQueryFilters, MethodName: string = null) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
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

        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getByFilters(filters));
            });
        });
    }


    getByCompactFilters(objectTableName: string, filters: ApiQueryFilters, MethodName: string = null) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
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

        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getByCompactFilters(filters));
            });
        });
    }

    getCustomByFilters(objectTableName: string, filters: ApiQueryFilters, MethodName: string = null) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
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
        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getTenantImportByFilters(filters));
            });
        });
    }

    getExtendedByFilters(objectTableName: string, filters: ApiQueryFilters, MethodName: string = null) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
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
        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getByFilters(filters));
            });
        });
    }

    getOpenReconciliationsByFilter(objectTableName: string, accountId: string, filters: ApiQueryFilters) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ExtendedListService";
        var servicelink = './' + moduleName + '/Services/ExtendedLists/' + servicename;
        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getOpenReconciliationsByFilter(accountId,filters));
            });
        });
    }
    getARPyamentChequesListAsLedgerTransactions(objectTableName: string,  filters: ApiQueryFilters) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ExtendedListService";
        var servicelink = './' + moduleName + '/Services/ExtendedLists/' + servicename;
        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.GetARPyamentChequesListAsLedgerTransactions( filters));
            });
        });
    }

    getReconciliationsByFilter(objectTableName: string, accountId: string, filters: ApiQueryFilters) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ExtendedListService";
        var servicelink = './' + moduleName + '/Services/ExtendedLists/' + servicename;
        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getReconciliationsByFilter(accountId,filters));
            });
        });
    }

    getExternalReoncilioationsByFilter(objectTableName: string, objectTableId: string, entityId: string, filters: ApiQueryFilters)
    {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ExtendedListService";
        var servicelink = './' + moduleName + '/Services/ExtendedLists/' + servicename;
        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getExternalReoncilioationsByFilter(objectTableId, entityId, filters));
            });
        });
    }


    getMock(objectTableName: string) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ExtendedListService";
        var servicelink = './' + moduleName + '/Services/ExtendedLists/' + servicename;

        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getMock());
            });
        });
    }


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

    getSingleFromCache(key: string, objectTableName: string, filters: ApiQueryFilters) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ListService";
        var servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;

        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getSingleFromCache(key));
            });
        });
    }

    getAllFromCache(objectTableName: string, filters: ApiQueryFilters) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "ListService";
        var servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;

        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                try {
                    resolve(service.getAllFromCache(filters));
                } catch (e) {

                    reject(new Error("service.getAllFromCache is not a function: " + service._apiUrl));
                    //console.log(e);
                }
            });
        });
    }

    getSingle(key: string, objectTableName: string, MethodName: string = null) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
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
        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getSingle(key));
            });
        });
    }

    getEntityCopyToCurrentTenant(zeroEntityId: string, tableName:string) {
        if (tableName == 'Port') {
            var servicelink = './Common/Services/ExtendedLists/PortService';

            return new Promise((resolve, reject) => {
                SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                    resolve(service.GetPortCopyToCurrentTenant(zeroEntityId));
                });
            });
        }

        if (tableName == 'Carrier') {
            var servicelink = './Common/Services/StandardLists/CarrierListService';

            return new Promise((resolve, reject) => {
                SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                    resolve(service.GetCarrierCopyToCurrentTenant(zeroEntityId));
                });
            });
        }
    }

    getDWDimByFilters(objectTableName: string, filters: ApiQueryFilters) {

        var servicelink = './Infrastructure/Services/ExtendedPMs/DWQueryBuilderService';


        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getByFilters(filters));
            });
        });
    }
}
