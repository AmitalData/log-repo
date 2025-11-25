import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, throwError } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { SIIRequestPM } from '../../EntityPMs/SIIRequestPM';
import { SupplierInvoiceItemsReqListPM } from '../../EntityPMs/SupplierInvoiceItemsReqListPM';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {Guid} from '../../../Infrastructure/Utilities/Guid';

@Injectable()

export class SIIRequestWebService {
    private _http: HttpClient
    private _apiUrl: string;
    private _apiUrlUser: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SIIRequestExtended';
        this._apiUrlUser = ServiceHelper.GetLogitudeURL() + 'api/Users';
    }


    getByDeclarationId(declarationId: string, id: string) {
        return defer(() => {
            let authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            let serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            return this._http.get(this._apiUrl + "/GetSingle/?declarationId=" + declarationId + "&id=" + id, ServiceHelper.GetHttpHeaders()).pipe(
                map((response: HttpResponse<any>) => {
                    var entity: SIIRequestPM;
                    if (response) {
                        entity = this.MapJsonToEntityPM(response);
                    }
                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = entity;
                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    getSupplierInvoiceItemsForSIIRequest(declarationId: string, siiRequestId: string) {
        return defer(() => {
            let authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            let serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            return this._http.get(this._apiUrl + "/GetSupplierInvoiceItemsForSIIRequest/?declarationId=" + declarationId + "&siiRequestId=" + siiRequestId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                let serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    postSendSIIRequest(siiRequestId: string, declarationId: string, tenant: number, selectedRows: any[]) {
        return defer(() => {
            let headers = new Headers();
            headers.append('Token', SessionInfo.Token);
            headers.append('Content-Type', 'application/json');

            return this._http.post(
                this._apiUrl + "/PostSendSIIRequest?siiRequestId=" + siiRequestId + "&declarationId=" + declarationId + "&tenant=" + tenant,
                JSON.stringify(selectedRows),
                ServiceHelper.GetHttpHeaders()
            ).pipe(
                map(resp => resp),
                catchError(err => throwError(err))
            );
        });
    }
    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: SIIRequestPM = null) {


        if (!entityPM) {

            entityPM = new SIIRequestPM();
            entityPM.DisableMarkAsDirty = true;
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

        this.MapSupplierInvoiceItemsReqLists(entityPM, jsonPM, mapParent); // Call composition tables map methods



        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.SupplierInvoiceItemsReqLists = [];
            for (var item in entityPM.SupplierInvoiceItemsReqLists) {
                var mySupplierInvoiceItemsReqListPM = entityPM.SupplierInvoiceItemsReqLists[item];
                var newSupplierInvoiceItemsReqListPM: SupplierInvoiceItemsReqListPM = this.clone(mySupplierInvoiceItemsReqListPM);


                entityPM.OldEntityPM.SupplierInvoiceItemsReqLists.push(newSupplierInvoiceItemsReqListPM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        entityPM.DisableMarkAsDirty = false;

        return entityPM;
    }

    MapSupplierInvoiceItemsReqLists(entityPM: SIIRequestPM, jsonPM: any, mapParent: boolean = true) {

        var oldSupplierInvoiceItemsReqLists: SupplierInvoiceItemsReqListPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldSupplierInvoiceItemsReqLists = entityPM.OldEntityPM.SupplierInvoiceItemsReqLists;
        }

        entityPM.SupplierInvoiceItemsReqLists = new Array<SupplierInvoiceItemsReqListPM>();
        for (var item in jsonPM.SupplierInvoiceItemsReqLists) {
            var jItem = jsonPM.SupplierInvoiceItemsReqLists[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newSupplierInvoiceItemsReqListPM: SupplierInvoiceItemsReqListPM;

            if (mapParent) {
                newSupplierInvoiceItemsReqListPM = new SupplierInvoiceItemsReqListPM(entityPM);
            }
            else {
                newSupplierInvoiceItemsReqListPM = new SupplierInvoiceItemsReqListPM(null);
            }
            newSupplierInvoiceItemsReqListPM.DisableMarkAsDirty = true;

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newSupplierInvoiceItemsReqListPM[pmProperty] = jItem[pmProperty];
            }


            if (mapParent) {
                newSupplierInvoiceItemsReqListPM.UniqueKey = Guid.newGuid();
                newSupplierInvoiceItemsReqListPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newSupplierInvoiceItemsReqListPM.OldEntityPM = this.clone(newSupplierInvoiceItemsReqListPM);


            }
            else {
                if (newSupplierInvoiceItemsReqListPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newSupplierInvoiceItemsReqListPM.ChangeSetOp = "Update";
                }
                else {
                    newSupplierInvoiceItemsReqListPM.ChangeSetOp = "Insert";
                }

                newSupplierInvoiceItemsReqListPM.OldEntityPM = null;
                newSupplierInvoiceItemsReqListPM.EntityParentPM = null;
            }
            newSupplierInvoiceItemsReqListPM.DisableMarkAsDirty = false;
            newSupplierInvoiceItemsReqListPM.IsDirty = false;
            entityPM.SupplierInvoiceItemsReqLists.push(newSupplierInvoiceItemsReqListPM);
        }
        if (oldSupplierInvoiceItemsReqLists) {

            for (var itemKey in oldSupplierInvoiceItemsReqLists) {
                if (entityPM.SupplierInvoiceItemsReqLists.filter(p => p.UniqueKey === oldSupplierInvoiceItemsReqLists[itemKey].UniqueKey).length === 0) {

                    if (oldSupplierInvoiceItemsReqLists[itemKey]) {
                        //oldSupplierInvoiceItemsReqLists[itemKey].ChangeSetOp = "Delete";
                        //entityPM.SupplierInvoiceItemsReqLists.push(oldSupplierInvoiceItemsReqLists[itemKey]);
                        var oldItemJson = oldSupplierInvoiceItemsReqLists[itemKey];
                        var deletedPM: SupplierInvoiceItemsReqListPM = new SupplierInvoiceItemsReqListPM(null);
                        deletedPM.DisableMarkAsDirty = true;
                        var pmKeys = Object.keys(oldItemJson);
                        for (var key in pmKeys) {

                            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
                                continue;
                            }

                            var property = pmKeys[key];
                            deletedPM[property] = oldItemJson[property];
                        }

                        deletedPM.DisableMarkAsDirty = false;
                        deletedPM.IsDirty = false;
                        deletedPM.ChangeSetOp = "Delete";

                        deletedPM.OldEntityPM = null;
                        entityPM.SupplierInvoiceItemsReqLists.push(deletedPM);
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

    public GetNewEntityPM() {
        var entityPM: SIIRequestPM;
        entityPM = new SIIRequestPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }
}

export class SupplierInvoiceItemsForSIIRequest {
    InvoiceNumber: string;
    InvoiceLineNumber: number;
    InvoiceCounterKey: number;
    ItemCode: string;
    ItemDescription: string;
    ClassificationCode: string;
    TradeAgreementCode: string;
    InvoiceQuantityType: string;
    InvoiceQuantity: string;
    ItemPrice: string;
    ItemPriceCurrencyCode: string;
    OriginCountryCode: string;
    InvoiceQuantityTypeName: string;
    TradeAgreementName: string;
    OriginCountryName: string;
    RequestRequiredStatus: string;
    LineNumber: number;
    HasDemandState: boolean;
    Counter: number;
}
