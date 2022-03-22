import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectFieldValidationPM } from '../../EntityPMs/ObjectFieldValidationPM';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { ObjectFieldPM } from '../../EntityPMs/ObjectFieldPM';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { defer } from 'rxjs';

@Injectable()
export class ObjectFieldPMExtendedService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ObjectFieldExtended';
    }

    GetObjectFieldByName(objectfieldName: string,querySection:string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetObjectFieldByName?' + 'objectFieldName=' + objectfieldName +'&querySection=' + querySection , ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var lists = response;
                return lists;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    getSingleFromQueries(UniqueCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleQuery?' + 'UniqueCode=' + UniqueCode  , ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var lists = response;
                return lists;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }


    GetEntityAuomationAllowedinAutomationConditionsObjectFieldPMsByEntityTableIds(entityAutomationIds: string, tenant: number) {
        var url = this._apiUrl + '/GetEntityAuomationAllowedinAutomationConditionsObjectFieldPMsByEntityTableIds/?' + 'entityAutomationIds=' + entityAutomationIds + '&tenant=' + tenant;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result: any = response;
            var entity: ObjectFieldPM;
            var objectFieldPMLists: ObjectFieldPM[];
            objectFieldPMLists = new Array<ObjectFieldPM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                objectFieldPMLists.push(entity);
            });
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = objectFieldPMLists;

            return serviceResponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ObjectFieldPM = null) {
        if (!entityPM) {

            entityPM = new ObjectFieldPM();
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

        this.MapObjectFieldValidations(entityPM, jsonPM, mapParent); // Call composition tables map methods

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.ObjectFieldValidations = [];
            for (var item in entityPM.ObjectFieldValidations) {
                var myObjectFieldValidationPM = entityPM.ObjectFieldValidations[item];
                var newObjectFieldValidationPM: ObjectFieldValidationPM = this.clone(myObjectFieldValidationPM);


                entityPM.OldEntityPM.ObjectFieldValidations.push(newObjectFieldValidationPM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;

        return entityPM;
    }

    MapObjectFieldValidations(entityPM: ObjectFieldPM, jsonPM: any, mapParent: boolean = true) {

        var oldObjectFieldValidations: ObjectFieldValidationPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldObjectFieldValidations = entityPM.OldEntityPM.ObjectFieldValidations;
        }

        entityPM.ObjectFieldValidations = new Array<ObjectFieldValidationPM>();
        for (var item in jsonPM.ObjectFieldValidations) {
            var jItem = jsonPM.ObjectFieldValidations[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newObjectFieldValidationPM: ObjectFieldValidationPM;

            if (mapParent) {
                newObjectFieldValidationPM = new ObjectFieldValidationPM(entityPM);
            }
            else {
                newObjectFieldValidationPM = new ObjectFieldValidationPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newObjectFieldValidationPM[pmProperty] = jItem[pmProperty];
            }


            if (mapParent) {
                newObjectFieldValidationPM.UniqueKey = Guid.newGuid();
                newObjectFieldValidationPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newObjectFieldValidationPM.OldEntityPM = this.clone(newObjectFieldValidationPM);
                //file not found! child composition ObjectFieldValidation


            }
            else {
                if (newObjectFieldValidationPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newObjectFieldValidationPM.ChangeSetOp = "Update";
                }
                else {
                    newObjectFieldValidationPM.ChangeSetOp = "Insert";
                }
                //file not found! child composition ObjectFieldValidation

                newObjectFieldValidationPM.OldEntityPM = null;
                newObjectFieldValidationPM.EntityParentPM = null;
            }

            newObjectFieldValidationPM.IsDirty = false;
            entityPM.ObjectFieldValidations.push(newObjectFieldValidationPM);
        }
        if (oldObjectFieldValidations) {

            for (var itemKey in oldObjectFieldValidations) {
                if (entityPM.ObjectFieldValidations.filter(p => p.UniqueKey === oldObjectFieldValidations[itemKey].UniqueKey).length === 0) {

                    if (oldObjectFieldValidations[itemKey]) {
                        //oldObjectFieldValidations[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ObjectFieldValidations.push(oldObjectFieldValidations[itemKey]);
                        var oldItemJson = oldObjectFieldValidations[itemKey];
                        var deletedPM: ObjectFieldValidationPM = new ObjectFieldValidationPM(null);
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

                        //file not found! child composition ObjectFieldValidation
                        deletedPM.OldEntityPM = null;
                        entityPM.ObjectFieldValidations.push(deletedPM);
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
