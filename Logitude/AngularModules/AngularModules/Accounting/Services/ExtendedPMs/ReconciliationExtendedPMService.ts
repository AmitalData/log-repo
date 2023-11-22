import { CustomFieldClass } from './../../../Infrastructure/DataContracts/CustomFieldClass';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ReconciliationPM } from '../../EntityPMs/ReconciliationPM';
import { LedgerTransactionPM } from '../../EntityPMs/LedgerTransactionPM';
import { JournalPM } from '../../EntityPMs/JournalPM';
import { ReconciliationLinePM } from '../../EntityPMs/ReconciliationLinePM';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { RecoCallback } from '../../DataContracts/RecoCallback';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'
import { QueryColumnPM } from 'Infrastructure/EntityPMs/QueryColumnPM';

@Injectable()

export class ReconciliationExtendedPMService {
    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {

        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReconciliationOp';
    }

    insert(entityPM: ReconciliationPM) {
        var mappedEntity: ReconciliationPM;
        mappedEntity = this.MapJsonToEntityPM(entityPM, false);
        return this.httpClient.post(this._apiUrl + '/PostInsertReconciliation', JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(
            map((res: RecoCallback) => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var _callBack: RecoCallback = res;
                if (_callBack) {
                    serviceResponse.Result = _callBack;

                }
                else {
                    console.log("[WARNING!!] no callback for reconciliation!");
                }
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    getCommunicationLog(id: string) {
        return this.httpClient.get(this._apiUrl + '/GetCommunicationLog?id=' + id, ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = res;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }

    UpdateDraftReconciliationTransactions(transactions: LedgerTransactionPM[]) {
        return this.httpClient.put(this._apiUrl + '/PutDraftReconciliationTransactions/', JSON.stringify(transactions), ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = res;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    deleteResetDraftOpenReconciliation(gLAccountId: string) {

        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();

        return this.httpClient.delete(this._apiUrl + '/DeleteResetDraftOpenReconciliation?gLAccountId=' + gLAccountId + '&tenant=' + SessionInfo.LoggedUserTenant, ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                serviceResponse.Result = res;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    getDraftReconciliations(gLAccountId: string) {


        return this.httpClient.get(this._apiUrl + '/GetDraftReconciliations?gLAccountId=' + gLAccountId, ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var transactions = res;

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
            }),
            catchError(ServiceHelper.HandleServiceError));



    }

    CreateJournalReconcile(
        myReconciliationLines: ReconciliationLinePM[],

        TheAccountId: string, AdjustAccountId: string, AccountDate: string, DueDate: string, RefDate: string, Ref1: string, Ref2: string, Ref3: string, Remarks: string) {
        return this.httpClient.post(this._apiUrl + "/PostCreateJournalReconcile?"
            + "&TheAccountId=" + TheAccountId
            + "&AdjustAccountId=" + AdjustAccountId
            + "&AccountDate=" + AccountDate
            + "&DueDate=" + DueDate
            + "&RefDate=" + RefDate
            + "&Ref1=" + Ref1
            + "&Ref2=" + Ref2
            + "&Ref3=" + Ref3
            + "&Remarks=" + Remarks
            , JSON.stringify(myReconciliationLines), ServiceHelper.GetHttpHeaders()).pipe(
                map(res => {
                    var pm = res;
                    if (pm) {
                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        var mappedResult: JournalPM;
                        //mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                        serviceResponse.Result = pm;
                    }



                    return serviceResponse;
                }),
                catchError(ServiceHelper.HandleServiceError));


    }

    CreateSplitJournalReconcile(
        myReconciliationLines: ReconciliationLinePM[],

        TheAccountId: string, AdjustAccountId: string, AccountDate: string, DueDate: string, RefDate: string, Ref1: string, Ref2: string, Ref3: string, Remarks: string) {
        return this.httpClient.post(this._apiUrl + "/PostCreateSplitJournalReconcile?"
            + "&TheAccountId=" + TheAccountId
            + "&AdjustAccountId=" + AdjustAccountId
            + "&AccountDate=" + AccountDate
            + "&DueDate=" + DueDate
            + "&RefDate=" + RefDate
            + "&Ref1=" + Ref1
            + "&Ref2=" + Ref2
            + "&Ref3=" + Ref3
            + "&Remarks=" + Remarks
            , JSON.stringify(myReconciliationLines), ServiceHelper.GetHttpHeaders()).pipe(
                map(res => {
                    if (res) {
                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = res;
                    }
                    return serviceResponse;
                }),
                catchError(ServiceHelper.HandleServiceError));
    }

    getByNumber(number: string) {

        return this.httpClient.get(this._apiUrl + '/GetByNumber?number=' + number, ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var entity = res;



                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }

    GetSingleWithoutLines(id: string) {
        return this.httpClient.get(this._apiUrl + '/GetSingleWithoutLines?id=' + id, ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var pm = res;

                var entity: ReconciliationPM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = entity;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ReconciliationPM = null) {


        if (!entityPM) {

            entityPM = new ReconciliationPM();
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

        this.MapReconciliationLines(entityPM, jsonPM, mapParent); // Call composition tables map methods



        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.ReconciliationLines = [];
            for (var item in entityPM.ReconciliationLines) {
                var myReconciliationLinePM = entityPM.ReconciliationLines[item];
                var newReconciliationLinePM: ReconciliationLinePM = this.clone(myReconciliationLinePM);


                entityPM.OldEntityPM.ReconciliationLines.push(newReconciliationLinePM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
    }

    MapReconciliationLines(entityPM: ReconciliationPM, jsonPM: any, mapParent: boolean = true) {

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
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newReconciliationLinePM[pmProperty] = jItem[pmProperty];
            }


            if (mapParent) {
                newReconciliationLinePM.UniqueKey = Guid.newGuid();
                newReconciliationLinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newReconciliationLinePM.OldEntityPM = this.clone(newReconciliationLinePM);


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

            newReconciliationLinePM.IsDirty = false;
            entityPM.ReconciliationLines.push(newReconciliationLinePM);
        }
        if (oldReconciliationLines) {

            for (var itemKey in oldReconciliationLines) {
                if (entityPM.ReconciliationLines.filter(p => p.UniqueKey === oldReconciliationLines[itemKey].UniqueKey).length === 0) {

                    if (oldReconciliationLines[itemKey]) {
                        //oldReconciliationLines[itemKey].ChangeSetOp = "Delete";
                        //entityPM.ReconciliationLines.push(oldReconciliationLines[itemKey]);
                        var oldItemJson = oldReconciliationLines[itemKey];
                        var deletedPM: ReconciliationLinePM = new ReconciliationLinePM(null);
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
                        entityPM.ReconciliationLines.push(deletedPM);
                    }
                }
            }
        }
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


    PostReconcileExcelData(args: ReconcileExcelDataArgs) {
        return this.httpClient.post(this._apiUrl + '/PostReconcileExcelData?', JSON.stringify(args), ServiceHelper.GetHttpHeaders()).pipe(
            map((res: RecoCallback) => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var _callBack: RecoCallback = res;
                if (_callBack) {
                    serviceResponse.Result = _callBack;
                }
                else {
                    console.log("[WARNING!!] no callback for reconciliation!");
                }
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }

    PostReconcileExtExcelData(args: ReconcileExcelDataArgs) {
        return this.httpClient.post(this._apiUrl + '/PostReconcileExtExcelData?', JSON.stringify(args), ServiceHelper.GetHttpHeaders()).pipe(
            map((res: RecoCallback) => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var _callBack: RecoCallback = res;
                if (_callBack) {
                    serviceResponse.Result = _callBack;
                }
                else {
                    console.log("[WARNING!!] no callback for reconciliation!");
                }
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }
}

export class ReconcileExcelDataArgs {
    Title: string;
    Data: any[] = [];
    QueryColumns: QueryColumnPM[] = [];
    Tenant: number;
}
