import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ReconcileExternalPagePM} from '../../EntityPMs/ReconcileExternalPagePM';
import {ReconcileExternalPageLinePM} from '../../EntityPMs/ReconcileExternalPageLinePM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { ImageParameter } from '../../../Infrastructure/DataContracts/ImageParameter';

@Injectable()

export class ReconcileExternalPageExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReconcileExternalPagesExtended';
    }


    GetBankPageByPageNo(pageNumber: string, bankAccountId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            return Observable.defer(() => {
                return this._http.get(this._apiUrl + '/GetBankPageByPageNo?pageNumber=' + pageNumber + '&bankAccountId=' + bankAccountId, { headers: authHeader })
                    .map(response => {
                        var res = response.json();

                        return res;
                    }).catch(ServiceHelper.HandleServiceError);
            });
        });


    }

    GetPrevPageByPageNo(pageNumber: number, bankAccountId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            return Observable.defer(() => {
                return this._http.get(this._apiUrl + '/GetPrevPageByPageNo?pageNumber=' + pageNumber + '&bankAccountId=' + bankAccountId, { headers: authHeader })
                    .map(response => {
                        var res = response.json();

                        return res;
                    }).catch(ServiceHelper.HandleServiceError);
            });
        });


    }


    LoadBankPages(fileUploadParamerter: ImageParameter) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + '/PostLoadBankPages', JSON.stringify(fileUploadParamerter), {
                headers: authHeader,

            }).map(response => {
                var result = response.json();
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;

            }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }


    GetDraftPage(bankAccountId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            return Observable.defer(() => {
                return this._http.get(this._apiUrl + '/GetDraftPage?bankAccountId=' + bankAccountId, { headers: authHeader })
                    .map(response => {
                        var res = response.json();

                        return res;
                    }).catch(ServiceHelper.HandleServiceError);
            });
        });


    }


    //MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ReconcileExternalPagePM = null) {


    //    if (!entityPM) {

    //        entityPM = new ReconcileExternalPagePM();
    //    }

    //    var jsonPMKeys = Object.keys(jsonPM);

    //    for (var key in jsonPMKeys) {
    //        if (jsonPMKeys[key] === "UIProperties") {

    //            continue;
    //        }
    //        var property = jsonPMKeys[key];
    //        entityPM[property] = jsonPM[property];
    //    }

    //    var oldReconcileExternalPageLines: ReconcileExternalPageLinePM[] = [];
    //    if (entityPM.OldEntityPM && !mapParent) {
    //        oldReconcileExternalPageLines = entityPM.OldEntityPM.ReconcileExternalPageLines;
    //    }


    //    entityPM.ReconcileExternalPageLines = new Array<ReconcileExternalPageLinePM>();
    //    for (var item in jsonPM.ReconcileExternalPageLines) {

    //        var jItem = jsonPM.ReconcileExternalPageLines[item];
    //        if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
    //            continue;
    //        }
    //        var newReconcileExternalPageLinePM: ReconcileExternalPageLinePM;
    //        if (mapParent) {
    //            newReconcileExternalPageLinePM = new ReconcileExternalPageLinePM(entityPM);
    //        }
    //        else {
    //            newReconcileExternalPageLinePM = new ReconcileExternalPageLinePM(null);
    //        }

    //        var pmKeysArray = Object.keys(jItem);
    //        for (var pmKey in pmKeysArray) {

    //            if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
    //                continue;
    //            }
    //            var pmProperty = pmKeysArray[pmKey];
    //            newReconcileExternalPageLinePM[pmProperty] = jItem[pmProperty];
    //        }
    //        newReconcileExternalPageLinePM.IsDirty = false;
    //        if (mapParent) {
    //            newReconcileExternalPageLinePM.OldEntityPM = this.clone(newReconcileExternalPageLinePM);
    //            newReconcileExternalPageLinePM.UniqueKey = Guid.newGuid();
    //            newReconcileExternalPageLinePM.ChangeSetOp = "None";
    //            jItem.ChangeSetOp = "None";

    //        }
    //        else {

    //            if (newReconcileExternalPageLinePM.UniqueKey) {

    //                if (jItem.IsDirty)
    //                    newReconcileExternalPageLinePM.ChangeSetOp = "Update";
    //            }
    //            else {
    //                newReconcileExternalPageLinePM.ChangeSetOp = "Insert";
    //            }

    //            newReconcileExternalPageLinePM.OldEntityPM = null;
    //            newReconcileExternalPageLinePM.EntityParentPM = null;
    //        }


    //        entityPM.ReconcileExternalPageLines.push(newReconcileExternalPageLinePM);
    //    }

    //    if (oldReconcileExternalPageLines) {

    //        for (var itemKey in oldReconcileExternalPageLines) {
    //            if (entityPM.ReconcileExternalPageLines.filter(p => p.UniqueKey === oldReconcileExternalPageLines[itemKey].UniqueKey).length === 0) {

    //                if (oldReconcileExternalPageLines[itemKey]) {
    //                    oldReconcileExternalPageLines[itemKey].ChangeSetOp = "Delete";
    //                    entityPM.ReconcileExternalPageLines.push(oldReconcileExternalPageLines[itemKey]);
    //                }
    //            }
    //        }
    //    }


    //    entityPM.IsDirty = false;

    //    if (mapParent) {
    //        entityPM.OldEntityPM = this.clone(entityPM);
    //        entityPM.OldEntityPM.ReconcileExternalPageLines = [];
    //        for (var m in entityPM.ReconcileExternalPageLines) {
    //            entityPM.OldEntityPM.ReconcileExternalPageLines.push(this.clone(entityPM.ReconcileExternalPageLines[m]));
    //        }

    //    }
    //    else {

    //        entityPM.OldEntityPM = null;
    //    }

    //    return entityPM;
    //}


}
