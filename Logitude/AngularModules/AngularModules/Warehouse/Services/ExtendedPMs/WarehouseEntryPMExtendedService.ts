/// <reference path="../../../infrastructure/datacontracts/customfieldclass.ts" />

import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

import {WarehouseEntryPackagePM} from '../../EntityPMs/WarehouseEntryPackagePM';
import {WarehouseEntryPM} from '../../EntityPMs/WarehouseEntryPM';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass';

@Injectable()
export class WarehouseEntryPMExtendedService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/WarehouseEntryExtended';
    }


    GetWarehouseConnectedEntitiesByEntityId(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetWarehouseConnectedEntitiesByEntityId" + '?entityId=' + entityId , { headers: authHeader }).map(response => {

            var result = response.json();
           
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;


        }).catch(ServiceHelper.HandleServiceError);
    }


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: WarehouseEntryPM = null) {


        if (!entityPM) {

            entityPM = new WarehouseEntryPM();
        }

        var customFields: Array<string> = [];
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
                    var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
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
                var newWarehouseEntryPackagePM: WarehouseEntryPackagePM = this.clone(myWarehouseEntryPackagePM);


                entityPM.OldEntityPM.WarehouseEntryPackages.push(newWarehouseEntryPackagePM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
    }

    MapWarehouseEntryPackages(entityPM: WarehouseEntryPM, jsonPM: any, mapParent: boolean = true) {

        var oldWarehouseEntryPackages: WarehouseEntryPackagePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldWarehouseEntryPackages = entityPM.OldEntityPM.WarehouseEntryPackages;
        }

        entityPM.WarehouseEntryPackages = new Array<WarehouseEntryPackagePM>();
        for (var item in jsonPM.WarehouseEntryPackages) {
            var jItem = jsonPM.WarehouseEntryPackages[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newWarehouseEntryPackagePM: WarehouseEntryPackagePM;

            if (mapParent) {
                newWarehouseEntryPackagePM = new WarehouseEntryPackagePM(entityPM);
            }
            else {
                newWarehouseEntryPackagePM = new WarehouseEntryPackagePM(null);
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
                newWarehouseEntryPackagePM.UniqueKey = Guid.newGuid();
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
                if (entityPM.WarehouseEntryPackages.filter(p => p.UniqueKey === oldWarehouseEntryPackages[itemKey].UniqueKey).length === 0) {

                    if (oldWarehouseEntryPackages[itemKey]) {
                        //oldWarehouseEntryPackages[itemKey].ChangeSetOp = "Delete";
                        //entityPM.WarehouseEntryPackages.push(oldWarehouseEntryPackages[itemKey]);
                        var oldItemJson = oldWarehouseEntryPackages[itemKey];
                        var deletedPM: WarehouseEntryPackagePM = new WarehouseEntryPackagePM(null);
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
    }

    public clone(jsonPM: any) {
        var entityPM: any;
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
    }


}

