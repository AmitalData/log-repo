import { CustomFieldClass } from './../../../Infrastructure/DataContracts/CustomFieldClass';
import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ReconcileExternalPagePM } from '../../EntityPMs/ReconcileExternalPagePM';
import { ReconcileExternalPageLinePM } from '../../EntityPMs/ReconcileExternalPageLinePM';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ImageParameter } from '../../../Infrastructure/DataContracts/ImageParameter';

@Injectable()

export class ReconcileExternalPageExtendedPMService
{
    private _http: Http;
    private _apiUrl: string;
    constructor()
    {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReconcileExternalPagesExtended';
    }


    GetPageByNumber(pageNumber: string, entityId: string,objectTableName: string)
    {

        return Observable.defer(() =>
        {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            return Observable.defer(() =>
            {
                return this._http.get(this._apiUrl + '/GetPageByNumber?pageNumber=' + pageNumber + '&entityId=' + entityId + '&objectTableName=' + objectTableName, { headers: authHeader })
                    .map(response =>
                    {
                        var res = response.json();
                        var pm = res.Result;


                        var entity: ReconcileExternalPagePM;
                        if (pm) {
                            entity = this.MapJsonToEntityPM(pm);
                        }

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;

                        return serviceResponse;
                    }).catch(ServiceHelper.HandleServiceError);
            });
        });


    }


    GetPreviousPageByNumber(pageNumber: number, entityId: string,objectTableName: string)
    {

        return Observable.defer(() =>
        {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            return Observable.defer(() =>
            {
                return this._http.get(this._apiUrl + '/GetPreviousPageByNumber?pageNumber=' + pageNumber + '&entityId=' + entityId + '&objectTableName=' + objectTableName, { headers: authHeader })
                    .map(response =>
                    {
                        var res = response.json();
                        var pm = res.Result;


                        var entity: ReconcileExternalPagePM;
                        if (pm) {
                            entity = this.MapJsonToEntityPM(pm);
                        }

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;

                        return serviceResponse;
                    }).catch(ServiceHelper.HandleServiceError);
            });
        });


    }


    LoadBankPages(fileUploadParamerter: ImageParameter)
    {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() =>
        {
            return this._http.post(this._apiUrl + '/PostLoadBankPages', JSON.stringify(fileUploadParamerter), {
                headers: authHeader,

            }).map(response =>
            {
                var result = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;

            }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }


    CheckLastApprovedBankPageAndReconciledLine(reconcileExternalPageId: string, objectTableName: string)
    {

        return Observable.defer(() =>
        {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            return Observable.defer(() =>
            {
                return this._http.get(this._apiUrl + '/GetCheckLastApprovedBankPageAndReconciledLine?reconcileExternalPageId=' + reconcileExternalPageId+ '&objectTableName=' + objectTableName, { headers: authHeader })
                    .map(response =>
                    {
                        var res = response.json();

                        return res;
                    }).catch(ServiceHelper.HandleServiceError);
            });
        });


    }
    CheckRestorePossibility(reconcileExternalPageId: string)
    {

        return Observable.defer(() =>
        {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            return Observable.defer(() =>
            {
                return this._http.get(this._apiUrl + '/GetCheckRestorePossibility?reconcileExternalPageId=' + reconcileExternalPageId, { headers: authHeader })
                    .map(response =>
                    {
                        var res = response.json();

                        return res;
                    }).catch(ServiceHelper.HandleServiceError);
            });
        });


    }
    GetDraftPage(entityId: string,objectTableName: string)
    {

        return Observable.defer(() =>
        {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            return Observable.defer(() =>
            {
                return this._http.get(this._apiUrl + '/GetDraftPage?entityId=' + entityId + '&objectTableName=' + objectTableName, { headers: authHeader })
                    .map(response =>
                    {
                        var res = response.json();

                        return res;
                    }).catch(ServiceHelper.HandleServiceError);
            });
        });


    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ReconcileExternalPagePM = null)
    {


        if (!entityPM) {

            entityPM = new ReconcileExternalPagePM();
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

        this.MapReconcileExternalPageLines(entityPM, jsonPM, mapParent); // Call composition tables map methods



        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.ReconcileExternalPageLines = [];
            for (var item in entityPM.ReconcileExternalPageLines) {
                var myReconcileExternalPageLinePM = entityPM.ReconcileExternalPageLines[item];
                var newReconcileExternalPageLinePM: ReconcileExternalPageLinePM = this.clone(myReconcileExternalPageLinePM);


                entityPM.OldEntityPM.ReconcileExternalPageLines.push(newReconcileExternalPageLinePM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
    }

    MapReconcileExternalPageLines(entityPM: ReconcileExternalPagePM, jsonPM: any, mapParent: boolean = true)
    {

        var oldReconcileExternalPageLines: ReconcileExternalPageLinePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldReconcileExternalPageLines = entityPM.OldEntityPM.ReconcileExternalPageLines;
        }

        entityPM.ReconcileExternalPageLines = new Array<ReconcileExternalPageLinePM>();
        for (var item in jsonPM.ReconcileExternalPageLines) {
            var jItem = jsonPM.ReconcileExternalPageLines[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newReconcileExternalPageLinePM: ReconcileExternalPageLinePM;

            if (mapParent) {
                newReconcileExternalPageLinePM = new ReconcileExternalPageLinePM(entityPM);
            }
            else {
                newReconcileExternalPageLinePM = new ReconcileExternalPageLinePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newReconcileExternalPageLinePM[pmProperty] = jItem[pmProperty];
            }


            if (mapParent) {
                newReconcileExternalPageLinePM.UniqueKey = Guid.newGuid();
                newReconcileExternalPageLinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newReconcileExternalPageLinePM.OldEntityPM = this.clone(newReconcileExternalPageLinePM);


            }
            else {
                if (newReconcileExternalPageLinePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newReconcileExternalPageLinePM.ChangeSetOp = "Update";
                }
                else {
                    newReconcileExternalPageLinePM.ChangeSetOp = "Insert";
                }

                newReconcileExternalPageLinePM.OldEntityPM = null;
                newReconcileExternalPageLinePM.EntityParentPM = null;
            }

            newReconcileExternalPageLinePM.IsDirty = false;
            entityPM.ReconcileExternalPageLines.push(newReconcileExternalPageLinePM);
        }
        if (oldReconcileExternalPageLines) {

            for (var itemKey in oldReconcileExternalPageLines) {
                if (entityPM.ReconcileExternalPageLines.filter(p => p.UniqueKey === oldReconcileExternalPageLines[itemKey].UniqueKey).length === 0) {

                    if (oldReconcileExternalPageLines[itemKey]) {
                        //oldReconcileExternalPageLines[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ReconcileExternalPageLines.push(oldReconcileExternalPageLines[itemKey]);
                        var oldItemJson = oldReconcileExternalPageLines[itemKey];
                        var deletedPM: ReconcileExternalPageLinePM = new ReconcileExternalPageLinePM(null);
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
                        entityPM.ReconcileExternalPageLines.push(deletedPM);
                    }
                }
            }
        }
    }

    public clone(jsonPM: any)
    {
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
