
import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass'
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';

import { PaymentChequePM } from '../../EntityPMs/PaymentChequePM';

import { PaymentChequeLinePM } from '../../EntityPMs/PaymentChequeLinePM';

@Injectable()

export class PaymentChequeExtendedPMService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PaymentChequeViews';
    }



    getPaymentChequeByChequeNumber(chequeNumber: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetPaymentChequeByChequeNumber?' + 'ChequeNumber=' + chequeNumber, {
                headers: authHeader
            }).map(response => {
                var pm = response.json();



                var entity: PaymentChequePM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;

              

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }



    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: PaymentChequePM = null) {


        if (!entityPM) {

            entityPM = new PaymentChequePM();
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

        this.MapPaymentChequeLines(entityPM, jsonPM, mapParent); // Call composition tables map methods



        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.PaymentChequeLines = [];
            for (var item in entityPM.PaymentChequeLines) {
                var myPaymentChequeLinePM = entityPM.PaymentChequeLines[item];
                var newPaymentChequeLinePM: PaymentChequeLinePM = this.clone(myPaymentChequeLinePM);


                entityPM.OldEntityPM.PaymentChequeLines.push(newPaymentChequeLinePM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
    }




    MapPaymentChequeLines(entityPM: PaymentChequePM, jsonPM: any, mapParent: boolean = true) {

        var oldPaymentChequeLines: PaymentChequeLinePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldPaymentChequeLines = entityPM.OldEntityPM.PaymentChequeLines;
        }

        entityPM.PaymentChequeLines = new Array<PaymentChequeLinePM>();
        for (var item in jsonPM.PaymentChequeLines) {
            var jItem = jsonPM.PaymentChequeLines[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newPaymentChequeLinePM: PaymentChequeLinePM;

            if (mapParent) {
                newPaymentChequeLinePM = new PaymentChequeLinePM(entityPM);
            }
            else {
                newPaymentChequeLinePM = new PaymentChequeLinePM(null);
            }

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newPaymentChequeLinePM[pmProperty] = jItem[pmProperty];
            }


            if (mapParent) {
                newPaymentChequeLinePM.UniqueKey = Guid.newGuid();
                newPaymentChequeLinePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newPaymentChequeLinePM.OldEntityPM = this.clone(newPaymentChequeLinePM);


            }
            else {
                if (newPaymentChequeLinePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newPaymentChequeLinePM.ChangeSetOp = "Update";
                }
                else {
                    newPaymentChequeLinePM.ChangeSetOp = "Insert";
                }

                newPaymentChequeLinePM.OldEntityPM = null;
                newPaymentChequeLinePM.EntityParentPM = null;
            }

            newPaymentChequeLinePM.IsDirty = false;
            entityPM.PaymentChequeLines.push(newPaymentChequeLinePM);
        }
        if (oldPaymentChequeLines) {

            for (var itemKey in oldPaymentChequeLines) {
                if (entityPM.PaymentChequeLines.filter(p => p.UniqueKey === oldPaymentChequeLines[itemKey].UniqueKey).length === 0) {

                    if (oldPaymentChequeLines[itemKey]) {
                        //oldPaymentChequeLines[itemKey].ChangeSetOp = "Delete";
                        //entityPM.PaymentChequeLines.push(oldPaymentChequeLines[itemKey]);
                        var oldItemJson = oldPaymentChequeLines[itemKey];
                        var deletedPM: PaymentChequeLinePM = new PaymentChequeLinePM(null);
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
                        entityPM.PaymentChequeLines.push(deletedPM);
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
