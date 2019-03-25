import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ReconciliationPM} from '../../EntityPMs/ReconciliationPM';
import {LedgerTransactionPM} from '../../EntityPMs/LedgerTransactionPM';
import {JournalPM} from '../../EntityPMs/JournalPM';
import {ReconciliationLinePM} from '../../EntityPMs/ReconciliationLinePM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { RecoCallback } from '../../DataContracts/RecoCallback';

@Injectable()

export class ReconciliationExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReconciliationOp';
    }

    insert(entityPM: ReconciliationPM) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            //var validator: ClassLevelValidator;

            //validator = new ClassLevelValidator();

            //var errorsArray = validator.Validate("ReconciliationPM", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            //if (errorsArray.length == 0) {
                var mappedEntity: ReconciliationPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((res) =>
                    {
                        var _callBack: RecoCallback = res.json();
                        if(_callBack)
                        {
                            serviceResponse.Result = _callBack;
                            // if(_callBack.isSplitted)
                            // {
                            //     serviceResponse.Result = _callBack;
                            // }
                            // else
                            // {
                            //     // var pm = _callBack.reconciliationPM;
                            //     // if (pm) {
                            //     //     var mappedResult: ReconciliationPM;
                            //     //     //mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            //     //     serviceResponse.Result = pm;
                            //     // }
                            // }
                        }
                        else
                        {
                            console.log("[WARNING!!] no callback for reconciliation!");
                        }
                        return serviceResponse;

                    }).catch(ServiceHelper.HandleServiceError);
            //}
            //else {

            //    serviceResponse.HasError = true;
            //    serviceResponse.ErrorsArray = errorsArray;

            //    return Observable.of(serviceResponse);

            //}
        }

        );
    }

    delsertDraftLedgerTransaction(transactions: LedgerTransactionPM[]) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl + '/PutDelsertDraftLedgerTransaction/', JSON.stringify(transactions), { headers: authHeader })
                .map((res) => {
                    serviceResponse.Result = res.json();
                    return serviceResponse;
                })
                .catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    deleteResetDraftOpenReconciliation(gLAccountId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.delete(this._apiUrl + '/DeleteResetDraftOpenReconciliation?gLAccountId=' + gLAccountId + '&tenant=' + SessionInfo.LoggedUserTenant, { headers: authHeader })
                .map((res) => {
                    serviceResponse.Result = res.json();
                    return serviceResponse;
                })
                    .catch(ServiceHelper.HandleServiceError);
            });
    }

    getDraftReconciliations(gLAccountId: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            return Observable.defer(() => {
                return this._http.get(this._apiUrl + '/GetDraftReconciliations?gLAccountId=' + gLAccountId, { headers: authHeader })
                    .map(response => {
                        var transactions = response.json();

                        var _mappedListsArray: Array<LedgerTransactionPM> = [];
                        if (transactions) {
                            for (var key in transactions) {
                                var entity: LedgerTransactionPM;
                                entity = this.MapJsonToLedgerTransactionPM(transactions[key]);
                                _mappedListsArray.push(entity);
                            }
                        }



                        var serviceResponse = new ServiceResponse();
                        serviceResponse.Result = _mappedListsArray;

                        return serviceResponse;
                    }).catch(ServiceHelper.HandleServiceError);
            });
        });


    }

    CreateJournalReconcile(
        myReconciliationLines: ReconciliationLinePM[],
        TheAccountId: string, AdjustAccountId: string, AccountDate: string, Ref1: string, Ref2: string, Ref3: string, Remarks: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            //var validator: ClassLevelValidator;

            //validator = new ClassLevelValidator();

            //var errorsArray = validator.Validate("ReconciliationPM", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            //if (errorsArray.length == 0) {
            //var mappedEntity: ReconciliationPM[];


            return this._http.post(this._apiUrl + "/PostCreateJournalReconcile?"
                + "&TheAccountId=" + TheAccountId
                + "&AdjustAccountId=" + AdjustAccountId
                +"&AccountDate=" + AccountDate
                +"&Ref1=" + Ref1
                +"&Ref2=" + Ref2
                +"&Ref3=" + Ref3
                +"&Remarks=" + Remarks
                , JSON.stringify(myReconciliationLines),
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    if (pm) {
                        var mappedResult: JournalPM;
                        //mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                        serviceResponse.Result = pm;
                    }



                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
            //}
            //else {

            //    serviceResponse.HasError = true;
            //    serviceResponse.ErrorsArray = errorsArray;

            //    return Observable.of(serviceResponse);

            //}
        }

        );

    }

    getByNumber(number: string){
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');


            return Observable.defer(() => {
                return this._http.get(this._apiUrl + '/GetByNumber?number=' + number, { headers: authHeader })
                    .map(response => {
                        var entity = response.json();



                        var serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;

                        return serviceResponse;
                    }).catch(ServiceHelper.HandleServiceError);
            });
        });
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ReconciliationPM = null) {


        if (!entityPM) {

            entityPM = new ReconciliationPM();
        }

        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        var oldReconciliationLines: ReconciliationLinePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldReconciliationLines = entityPM.OldEntityPM.ReconciliationLines;
        }


        entityPM.ReconciliationLines = new Array<ReconciliationLinePM>();
        for (var item in jsonPM.ReconciliationLines) {

            var jItem = jsonPM.ReconciliationLines[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newReconciliationLinePM: ReconciliationLinePM;
            if (mapParent) {
                newReconciliationLinePM = new ReconciliationLinePM(entityPM);
            }
            else {
                newReconciliationLinePM = new ReconciliationLinePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newReconciliationLinePM[pmProperty] = jItem[pmProperty];
            }
            newReconciliationLinePM.IsDirty = false;
            if (mapParent) {
                newReconciliationLinePM.OldEntityPM = this.clone(newReconciliationLinePM);
                newReconciliationLinePM.UniqueKey = Guid.newGuid();
                newReconciliationLinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";

            }
            else {

                if (newReconciliationLinePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newReconciliationLinePM.ChangeSetOp = "Update";
                }
                else {
                    newReconciliationLinePM.ChangeSetOp = "Insert";
                }

                newReconciliationLinePM.OldEntityPM = null;
                newReconciliationLinePM.EntityParentPM = null;
            }


            entityPM.ReconciliationLines.push(newReconciliationLinePM);
        }

        if (oldReconciliationLines) {

            for (var itemKey in oldReconciliationLines) {
                if (entityPM.ReconciliationLines.filter(p => p.UniqueKey === oldReconciliationLines[itemKey].UniqueKey).length === 0) {

                    if (oldReconciliationLines[itemKey]) {
                        oldReconciliationLines[itemKey].ChangeSetOp = "Delete";
                        entityPM.ReconciliationLines.push(oldReconciliationLines[itemKey]);
                    }
                }
            }
        }


        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.ReconciliationLines = [];
            for (var m in entityPM.ReconciliationLines) {
                entityPM.OldEntityPM.ReconciliationLines.push(this.clone(entityPM.ReconciliationLines[m]));
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }

    MapJsonToLedgerTransactionPM(jsonPM: any, mapParent: boolean = true, entityPM: LedgerTransactionPM = null) {


        if (!entityPM) {

            entityPM = new LedgerTransactionPM();
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
                    //var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    //entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }

        }




        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
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
