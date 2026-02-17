import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

import { ClaimsRelatedEntityPM } from '../../EntityPMs/ClaimsRelatedEntityPM';
import { ClaimsRelatedEntitiesAmountPM } from '../../EntityPMs/ClaimsRelatedEntitiesAmountPM';
import { ClaimsRelatedEntitiesReasonPM } from '../../EntityPMs/ClaimsRelatedEntitiesReasonPM';
import { ClaimsRelatedEntsReasonsExpPM } from '../../EntityPMs/ClaimsRelatedEntsReasonsExpPM';
import { ClaimsRelatedEntsExpDeclarPM } from '../../EntityPMs/ClaimsRelatedEntsExpDeclarPM';
import { ClaimValidator } from '../../Validators/ClaimValidator';

@Injectable()

export class ClaimsRelatedEntityExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/claimsRelatedEntities';
    }

    insert(entityPM: ClaimsRelatedEntityPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();
            var errorsArray = validator.Validate("Customs.ClaimsRelatedEntity", entityPM);

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: ClaimsRelatedEntityPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: ClaimsRelatedEntityPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }
                        return serviceResponse;

                    }).catch(ServiceHelper.HandleServiceError);
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return Observable.of(serviceResponse);
            }
        });
    }

    update(entityPM: ClaimsRelatedEntityPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();
            var errorsArray = validator.Validate("Customs.ClaimsRelatedEntity", entityPM);

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: ClaimsRelatedEntityPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        if (pm) {
                            var mappedResult: ClaimsRelatedEntityPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }


                        return serviceResponse;

                    }).catch(ServiceHelper.HandleServiceError);
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return Observable.of(serviceResponse);

            }
        }

        );

    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ClaimsRelatedEntityPM = null) {

        if (!entityPM) {
            entityPM = new ClaimsRelatedEntityPM(null);
        }
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        this.MapClaimsRelatedEntitiesAmounts(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapClaimsRelatedEntitiesReasons(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapClaimsRelatedEntsExpDeclars(entityPM, jsonPM, mapParent); // Call composition tables map methods

        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.ClaimsRelatedEntitiesAmounts = [];
            for (var k in entityPM.ClaimsRelatedEntitiesAmounts) {
                var myClaimsRelatedEntitiesAmountPM = entityPM.ClaimsRelatedEntitiesAmounts[k];
                var newClaimsRelatedEntitiesAmountPM = this.clone(myClaimsRelatedEntitiesAmountPM);
                entityPM.OldEntityPM.ClaimsRelatedEntitiesAmounts.push(newClaimsRelatedEntitiesAmountPM);
            }

            entityPM.OldEntityPM.ClaimsRelatedEntitiesReasons = [];
            for (var k in entityPM.ClaimsRelatedEntitiesReasons) {
                var myClaimsRelatedEntitiesReasonPM = entityPM.ClaimsRelatedEntitiesReasons[k];
                var newClaimsRelatedEntitiesReasonPM = this.clone(myClaimsRelatedEntitiesReasonPM);

                newClaimsRelatedEntitiesReasonPM.ClaimsRelatedEntsReasonsExps = [];
                for (var k in myClaimsRelatedEntitiesReasonPM.ClaimsRelatedEntsReasonsExps) {
                    var myClaimsRelatedEntsReasonsExpPM = myClaimsRelatedEntitiesReasonPM.ClaimsRelatedEntsReasonsExps[k];
                    var newClaimsRelatedEntsReasonsExpPM = this.clone(myClaimsRelatedEntsReasonsExpPM);
                    newClaimsRelatedEntitiesReasonPM.ClaimsRelatedEntsReasonsExps.push(newClaimsRelatedEntsReasonsExpPM);
                }

                entityPM.OldEntityPM.ClaimsRelatedEntitiesReasons.push(newClaimsRelatedEntitiesReasonPM);
            }

            entityPM.OldEntityPM.ClaimsRelatedEntsExpDeclars = [];
            for (var k in entityPM.ClaimsRelatedEntsExpDeclars) {
                var myClaimsRelatedEntsExpDeclarPM = entityPM.ClaimsRelatedEntsExpDeclars[k];
                var newClaimsRelatedEntsExpDeclarPM = this.clone(myClaimsRelatedEntsExpDeclarPM);
                entityPM.OldEntityPM.ClaimsRelatedEntsExpDeclars.push(newClaimsRelatedEntsExpDeclarPM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }

    MapClaimsRelatedEntitiesAmounts(entityPM: ClaimsRelatedEntityPM, jsonPM: any, mapParent: boolean = true) {

        var oldClaimsRelatedEntitiesAmounts: ClaimsRelatedEntitiesAmountPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldClaimsRelatedEntitiesAmounts = entityPM.OldEntityPM.ClaimsRelatedEntitiesAmounts;
        }

        entityPM.ClaimsRelatedEntitiesAmounts = new Array<ClaimsRelatedEntitiesAmountPM>();
        for (var item in jsonPM.ClaimsRelatedEntitiesAmounts) {
            var jItem = jsonPM.ClaimsRelatedEntitiesAmounts[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newClaimsRelatedEntitiesAmountPM: ClaimsRelatedEntitiesAmountPM;

            if (mapParent) {
                newClaimsRelatedEntitiesAmountPM = new ClaimsRelatedEntitiesAmountPM(entityPM);
            }
            else {
                newClaimsRelatedEntitiesAmountPM = new ClaimsRelatedEntitiesAmountPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newClaimsRelatedEntitiesAmountPM[pmProperty] = jItem[pmProperty];
            }
            newClaimsRelatedEntitiesAmountPM.IsDirty = false;

            if (mapParent) {
                newClaimsRelatedEntitiesAmountPM.UniqueKey = Guid.newGuid();
                newClaimsRelatedEntitiesAmountPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newClaimsRelatedEntitiesAmountPM.OldEntityPM = this.clone(newClaimsRelatedEntitiesAmountPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newClaimsRelatedEntitiesAmountPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newClaimsRelatedEntitiesAmountPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newClaimsRelatedEntitiesAmountPM.ChangeSetOp = "Update";
                    }
                    else {
                        newClaimsRelatedEntitiesAmountPM.ChangeSetOp = "Insert";
                    }
                }

                newClaimsRelatedEntitiesAmountPM.OldEntityPM = null;
                newClaimsRelatedEntitiesAmountPM.EntityParentPM = null;
            }


            entityPM.ClaimsRelatedEntitiesAmounts.push(newClaimsRelatedEntitiesAmountPM);
        }
        if (oldClaimsRelatedEntitiesAmounts) {

            for (var itemKey in oldClaimsRelatedEntitiesAmounts) {
                if (entityPM.ClaimsRelatedEntitiesAmounts.filter(p => p.UniqueKey === oldClaimsRelatedEntitiesAmounts[itemKey].UniqueKey).length === 0) {

                    if (oldClaimsRelatedEntitiesAmounts[itemKey]) {
                        //oldClaimsRelatedEntitiesAmounts[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ClaimsRelatedEntitiesAmounts.push(oldClaimsRelatedEntitiesAmounts[itemKey]);
                        var oldItemJson = oldClaimsRelatedEntitiesAmounts[itemKey];
                        var deletedPM: ClaimsRelatedEntitiesAmountPM = new ClaimsRelatedEntitiesAmountPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.ClaimsRelatedEntitiesAmounts.push(deletedPM);
                    }
                }
            }
        }
    }
    MapClaimsRelatedEntitiesReasons(entityPM: ClaimsRelatedEntityPM, jsonPM: any, mapParent: boolean = true) {

        var oldClaimsRelatedEntitiesReasons: ClaimsRelatedEntitiesReasonPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldClaimsRelatedEntitiesReasons = entityPM.OldEntityPM.ClaimsRelatedEntitiesReasons;
        }

        entityPM.ClaimsRelatedEntitiesReasons = new Array<ClaimsRelatedEntitiesReasonPM>();
        for (var item in jsonPM.ClaimsRelatedEntitiesReasons) {
            var jItem = jsonPM.ClaimsRelatedEntitiesReasons[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newClaimsRelatedEntitiesReasonPM: ClaimsRelatedEntitiesReasonPM;

            if (mapParent) {
                newClaimsRelatedEntitiesReasonPM = new ClaimsRelatedEntitiesReasonPM(entityPM);
            }
            else {
                newClaimsRelatedEntitiesReasonPM = new ClaimsRelatedEntitiesReasonPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newClaimsRelatedEntitiesReasonPM[pmProperty] = jItem[pmProperty];
            }
            newClaimsRelatedEntitiesReasonPM.IsDirty = false;

            if (mapParent) {
                newClaimsRelatedEntitiesReasonPM.UniqueKey = Guid.newGuid();
                newClaimsRelatedEntitiesReasonPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newClaimsRelatedEntitiesReasonPM.OldEntityPM = this.clone(newClaimsRelatedEntitiesReasonPM);


                this.MapClaimsRelatedEntsReasonsExps(newClaimsRelatedEntitiesReasonPM, jItem, mapParent);
                newClaimsRelatedEntitiesReasonPM.OldEntityPM.ClaimsRelatedEntsReasonsExps = [];
                for (var k in newClaimsRelatedEntitiesReasonPM.ClaimsRelatedEntsReasonsExps) {
                    var clonedInside = this.clone(newClaimsRelatedEntitiesReasonPM.ClaimsRelatedEntsReasonsExps[k]);
                    newClaimsRelatedEntitiesReasonPM.OldEntityPM.ClaimsRelatedEntsReasonsExps.push(clonedInside); // clone old ClaimsRelatedEntsReasonsExps//
                }


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newClaimsRelatedEntitiesReasonPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newClaimsRelatedEntitiesReasonPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newClaimsRelatedEntitiesReasonPM.ChangeSetOp = "Update";
                    }
                    else {
                        newClaimsRelatedEntitiesReasonPM.ChangeSetOp = "Insert";
                    }
                }


                this.MapClaimsRelatedEntsReasonsExps(newClaimsRelatedEntitiesReasonPM, jItem, mapParent);

                newClaimsRelatedEntitiesReasonPM.OldEntityPM = null;
                newClaimsRelatedEntitiesReasonPM.EntityParentPM = null;
            }


            entityPM.ClaimsRelatedEntitiesReasons.push(newClaimsRelatedEntitiesReasonPM);
        }
        if (oldClaimsRelatedEntitiesReasons) {

            for (var itemKey in oldClaimsRelatedEntitiesReasons) {
                if (entityPM.ClaimsRelatedEntitiesReasons.filter(p => p.UniqueKey === oldClaimsRelatedEntitiesReasons[itemKey].UniqueKey).length === 0) {

                    if (oldClaimsRelatedEntitiesReasons[itemKey]) {
                        //oldClaimsRelatedEntitiesReasons[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ClaimsRelatedEntitiesReasons.push(oldClaimsRelatedEntitiesReasons[itemKey]);
                        var oldItemJson = oldClaimsRelatedEntitiesReasons[itemKey];
                        var deletedPM: ClaimsRelatedEntitiesReasonPM = new ClaimsRelatedEntitiesReasonPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";



                        this.MapClaimsRelatedEntsReasonsExps(deletedPM, oldItemJson, mapParent);
                        deletedPM.OldEntityPM = null;
                        entityPM.ClaimsRelatedEntitiesReasons.push(deletedPM);
                    }
                }
            }
        }
    }
    MapClaimsRelatedEntsReasonsExps(entityPM: ClaimsRelatedEntitiesReasonPM, jsonPM: any, mapParent: boolean = true) {

        var oldClaimsRelatedEntsReasonsExps: ClaimsRelatedEntsReasonsExpPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldClaimsRelatedEntsReasonsExps = entityPM.OldEntityPM.ClaimsRelatedEntsReasonsExps;
        }

        entityPM.ClaimsRelatedEntsReasonsExps = new Array<ClaimsRelatedEntsReasonsExpPM>();
        for (var item in jsonPM.ClaimsRelatedEntsReasonsExps) {
            var jItem = jsonPM.ClaimsRelatedEntsReasonsExps[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newClaimsRelatedEntsReasonsExpPM: ClaimsRelatedEntsReasonsExpPM;

            if (mapParent) {
                newClaimsRelatedEntsReasonsExpPM = new ClaimsRelatedEntsReasonsExpPM(entityPM);
            }
            else {
                newClaimsRelatedEntsReasonsExpPM = new ClaimsRelatedEntsReasonsExpPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newClaimsRelatedEntsReasonsExpPM[pmProperty] = jItem[pmProperty];
            }
            newClaimsRelatedEntsReasonsExpPM.IsDirty = false;

            if (mapParent) {
                newClaimsRelatedEntsReasonsExpPM.UniqueKey = Guid.newGuid();
                newClaimsRelatedEntsReasonsExpPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newClaimsRelatedEntsReasonsExpPM.OldEntityPM = this.clone(newClaimsRelatedEntsReasonsExpPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newClaimsRelatedEntsReasonsExpPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newClaimsRelatedEntsReasonsExpPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newClaimsRelatedEntsReasonsExpPM.ChangeSetOp = "Update";
                    }
                    else {
                        newClaimsRelatedEntsReasonsExpPM.ChangeSetOp = "Insert";
                    }
                }

                newClaimsRelatedEntsReasonsExpPM.OldEntityPM = null;
                newClaimsRelatedEntsReasonsExpPM.EntityParentPM = null;
            }


            entityPM.ClaimsRelatedEntsReasonsExps.push(newClaimsRelatedEntsReasonsExpPM);
        }
        if (oldClaimsRelatedEntsReasonsExps) {

            for (var itemKey in oldClaimsRelatedEntsReasonsExps) {
                if (entityPM.ClaimsRelatedEntsReasonsExps.filter(p => p.UniqueKey === oldClaimsRelatedEntsReasonsExps[itemKey].UniqueKey).length === 0) {

                    if (oldClaimsRelatedEntsReasonsExps[itemKey]) {
                        //oldClaimsRelatedEntsReasonsExps[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ClaimsRelatedEntsReasonsExps.push(oldClaimsRelatedEntsReasonsExps[itemKey]);
                        var oldItemJson = oldClaimsRelatedEntsReasonsExps[itemKey];
                        var deletedPM: ClaimsRelatedEntsReasonsExpPM = new ClaimsRelatedEntsReasonsExpPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.ClaimsRelatedEntsReasonsExps.push(deletedPM);
                    }
                }
            }
        }
    }
    MapClaimsRelatedEntsExpDeclars(entityPM: ClaimsRelatedEntityPM, jsonPM: any, mapParent: boolean = true) {

        var oldClaimsRelatedEntsExpDeclars: ClaimsRelatedEntsExpDeclarPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldClaimsRelatedEntsExpDeclars = entityPM.OldEntityPM.ClaimsRelatedEntsExpDeclars;
        }

        entityPM.ClaimsRelatedEntsExpDeclars = new Array<ClaimsRelatedEntsExpDeclarPM>();
        for (var item in jsonPM.ClaimsRelatedEntsExpDeclars) {
            var jItem = jsonPM.ClaimsRelatedEntsExpDeclars[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newClaimsRelatedEntsExpDeclarPM: ClaimsRelatedEntsExpDeclarPM;

            if (mapParent) {
                newClaimsRelatedEntsExpDeclarPM = new ClaimsRelatedEntsExpDeclarPM(entityPM);
            }
            else {
                newClaimsRelatedEntsExpDeclarPM = new ClaimsRelatedEntsExpDeclarPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newClaimsRelatedEntsExpDeclarPM[pmProperty] = jItem[pmProperty];
            }
            newClaimsRelatedEntsExpDeclarPM.IsDirty = false;

            if (mapParent) {
                newClaimsRelatedEntsExpDeclarPM.UniqueKey = Guid.newGuid();
                newClaimsRelatedEntsExpDeclarPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newClaimsRelatedEntsExpDeclarPM.OldEntityPM = this.clone(newClaimsRelatedEntsExpDeclarPM);


            }
            else {
                if (entityPM.ChangeSetOp === "Delete") {
                    newClaimsRelatedEntsExpDeclarPM.ChangeSetOp = "Delete";
                }
                else {
                    if (newClaimsRelatedEntsExpDeclarPM.UniqueKey) {

                        if (jItem.IsDirty)
                            newClaimsRelatedEntsExpDeclarPM.ChangeSetOp = "Update";
                    }
                    else {
                        newClaimsRelatedEntsExpDeclarPM.ChangeSetOp = "Insert";
                    }
                }

                newClaimsRelatedEntsExpDeclarPM.OldEntityPM = null;
                newClaimsRelatedEntsExpDeclarPM.EntityParentPM = null;
            }


            entityPM.ClaimsRelatedEntsExpDeclars.push(newClaimsRelatedEntsExpDeclarPM);
        }
        if (oldClaimsRelatedEntsExpDeclars) {

            for (var itemKey in oldClaimsRelatedEntsExpDeclars) {
                if (entityPM.ClaimsRelatedEntsExpDeclars.filter(p => p.UniqueKey === oldClaimsRelatedEntsExpDeclars[itemKey].UniqueKey).length === 0) {

                    if (oldClaimsRelatedEntsExpDeclars[itemKey]) {
                        //oldClaimsRelatedEntsExpDeclars[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ClaimsRelatedEntsExpDeclars.push(oldClaimsRelatedEntsExpDeclars[itemKey]);
                        var oldItemJson = oldClaimsRelatedEntsExpDeclars[itemKey];
                        var deletedPM: ClaimsRelatedEntsExpDeclarPM = new ClaimsRelatedEntsExpDeclarPM(null);
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }


                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.ClaimsRelatedEntsExpDeclars.push(deletedPM);
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

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }


}
