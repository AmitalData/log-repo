declare var window: any;
declare var System: any;
import {Injectable} from '@angular/core';
import {SessionLocator} from '../Utilities/SessionLocator';
import { defer, of } from 'rxjs';

@Injectable()

export class EntityPMService {
    constructor() {

    }

    getSingle(objectTableName: string, id: string) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "PMService";
        var servicelink = './' + moduleName + '/Services/StandardPMs/' + servicename;
       
        return new Promise((resolve) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.get(id));
            });
        });
    }

    getNewEntity(objectTableName: string) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "PMService";
        var servicelink = './' + moduleName + '/Services/StandardPMs/' + servicename;

        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.GetNewEntityPM());
            });
        });
    }

    update(objectTableName: string, entityPM: any) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "PMService";
        var servicelink = './' + moduleName + '/Services/StandardPMs/' + servicename;

        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.update(entityPM));
            });
        });
    }

    insert(objectTableName: string, entityPM: any) {
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "PMService";
        var servicelink = './' + moduleName + '/Services/StandardPMs/' + servicename;

        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.insert(entityPM));
            });
        });
    }

}



