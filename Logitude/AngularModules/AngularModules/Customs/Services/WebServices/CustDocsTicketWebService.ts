import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

import {CustomsDocumentsTicketPM} from '../../EntityPMs/CustomsDocumentsTicketPM';

import {CustomsDocumentPointerPM} from '../../EntityPMs/CustomsDocumentPointerPM';

@Injectable()

export class CustDocsTicketWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustDocsTicketWebService';
    }

    GetCustomsDocumentsTicketsByEntityIdAndChilds(entityId: string, childEntityId1: string, childEntityId2: string, childEntityId3: string, parentEntityCode:string ) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCustomsDocumentsTicketsByEntityIdAndChilds?' + 'entityId=' + entityId + '&childEntityId1=' + childEntityId1 + '&childEntityId2=' + childEntityId2 + '&childEntityId3=' + childEntityId3 + '&parentEntityCode=' + parentEntityCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();

                serviceResponse.Result = response;
                var _mappedListsArray: Array<CustomsDocumentsTicketPM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CustomsDocumentsTicketPM;
                        entity = this.MapJsonToEntityPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                serviceResponse.Result = _mappedListsArray; 
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));


        });


    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: CustomsDocumentsTicketPM = null) {


        if (!entityPM) {

            entityPM = new CustomsDocumentsTicketPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        this.MapCustomsDocumentPointers(entityPM, jsonPM, mapParent); // Call composition tables map methods

        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.CustomsDocumentPointers = [];
            for (var item in entityPM.CustomsDocumentPointers) {
                var myCustomsDocumentPointerPM = entityPM.CustomsDocumentPointers[item];
                var newCustomsDocumentPointerPM: CustomsDocumentPointerPM = this.clone(myCustomsDocumentPointerPM);


                entityPM.OldEntityPM.CustomsDocumentPointers.push(newCustomsDocumentPointerPM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }

    MapCustomsDocumentPointers(entityPM: CustomsDocumentsTicketPM, jsonPM: any, mapParent: boolean = true) {

        var oldCustomsDocumentPointers: CustomsDocumentPointerPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCustomsDocumentPointers = entityPM.OldEntityPM.CustomsDocumentPointers;
        }

        entityPM.CustomsDocumentPointers = new Array<CustomsDocumentPointerPM>();
        for (var item in jsonPM.CustomsDocumentPointers) {
            var jItem = jsonPM.CustomsDocumentPointers[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCustomsDocumentPointerPM: CustomsDocumentPointerPM;

            if (mapParent) {
                newCustomsDocumentPointerPM = new CustomsDocumentPointerPM(entityPM);
            }
            else {
                newCustomsDocumentPointerPM = new CustomsDocumentPointerPM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCustomsDocumentPointerPM[pmProperty] = jItem[pmProperty];
            }
            newCustomsDocumentPointerPM.IsDirty = false;

            if (mapParent) {
                newCustomsDocumentPointerPM.UniqueKey = Guid.newGuid();
                newCustomsDocumentPointerPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCustomsDocumentPointerPM.OldEntityPM = this.clone(newCustomsDocumentPointerPM);


            }
            else {
                if (newCustomsDocumentPointerPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCustomsDocumentPointerPM.ChangeSetOp = "Update";
                }
                else {
                    newCustomsDocumentPointerPM.ChangeSetOp = "Insert";
                }

                newCustomsDocumentPointerPM.OldEntityPM = null;
                newCustomsDocumentPointerPM.EntityParentPM = null;
            }


            entityPM.CustomsDocumentPointers.push(newCustomsDocumentPointerPM);
        }
        if (oldCustomsDocumentPointers) {

            for (var itemKey in oldCustomsDocumentPointers) {
                if (entityPM.CustomsDocumentPointers.filter(p => p.UniqueKey === oldCustomsDocumentPointers[itemKey].UniqueKey).length === 0) {

                    if (oldCustomsDocumentPointers[itemKey]) {
                        //oldCustomsDocumentPointers[itemKey].ChangeSetOp = "Delete";
                        //entityPM.CustomsDocumentPointers.push(oldCustomsDocumentPointers[itemKey]);
                        var oldItemJson = oldCustomsDocumentPointers[itemKey];
                        var deletedPM: CustomsDocumentPointerPM = new CustomsDocumentPointerPM(null);
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
                        entityPM.CustomsDocumentPointers.push(deletedPM);
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