
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';

import {ObjectFieldPM} from '../../EntityPMs/ObjectFieldPM';
import {ObjectFieldValidationPM} from '../../EntityPMs/ObjectFieldValidationPM';

@Injectable()

export class ObjectFieldPMExtendedService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ObjectFieldExtended';
    }



    GetEntityAuomationAllowedinAutomationConditionsObjectFieldPMsByEntityTableIds(entityAutomationIds: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetEntityAuomationAllowedinAutomationConditionsObjectFieldPMsByEntityTableIds/?' + 'entityAutomationIds=' + entityAutomationIds + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            var entity: ObjectFieldPM;
            var objectFieldPMLists: ObjectFieldPM[];
            objectFieldPMLists = new Array<ObjectFieldPM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                objectFieldPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = objectFieldPMLists;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
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
