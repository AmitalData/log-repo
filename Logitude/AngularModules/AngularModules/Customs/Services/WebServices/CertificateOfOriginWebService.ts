import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CLAIM_2340_ClaimRequestRequestParams } from '../../DataContract/RequestParams/CLAIM_2340_ClaimRequestRequestParams';
import { ContinuousRequestOnClaimFileRequestParams } from '../../DataContract/RequestParams/ContinuousRequestOnClaimFileRequestParams';
import { CertificateOfOriginRequestRequestParams } from 'Customs/DataContract/RequestParams/CertificateOfOriginRequestRequestParams';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';
import { CertificateOfOriginInvoicePM } from 'Customs/EntityPMs/CertificateOfOriginInvoicePM';
import { CertificateOfOriginItemPM } from 'Customs/EntityPMs/CertificateOfOriginItemPM';
import { CustomFieldClass } from 'Infrastructure/DataContracts/CustomFieldClass';
import { Guid } from 'Infrastructure/Utilities/Guid';
import { PerformanceLogger } from 'Infrastructure/Utilities/PerformanceLogger';
import { ClassLevelValidator } from 'Infrastructure/Validators/ClassLevelValidator';


@Injectable()

export class CertificateOfOriginWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CertificateOfOrigin';
    }

    PostCertificateOfOriginRequest(entity: CertificateOfOriginRequestRequestParams) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostCertificateOfOriginRequest/',
                JSON.stringify(entity),
                ServiceHelper.GetHttpHeaders()).pipe(map((res) => {

                    serviceResponse.Result = res;

                    return serviceResponse;

                }), catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    GetCertificateOfOriginByID(declarationId: string, amendmentOriginalDeclartation: string, tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCertificateOfOriginByID/?declarationId=" + declarationId + "&amendmentOriginalDeclartation=" + amendmentOriginalDeclartation + "&tenant=" + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    GetCertificateOfOriginByIDIncludeChildrens(certificateId: string, declarationId: string, tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCertificateOfOriginByIDIncludeChildrens/?certificateId=" + certificateId + "&declarationId=" + declarationId + "&tenant=" + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                var mappedResult: CertificateOfOriginPM = this.MapJsonToEntityPM(response, false);
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    GetCityOfDeclarationByImporterID(importerID: string, tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCityOfDeclarationByImporterID/?importerID=" + importerID + "&tenant=" + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    GetMandatoryFieldsByCooTypeCode(CooTypeCode: string, tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetMandatoryFieldsByCooTypeCode/?CooTypeCode=" + CooTypeCode + "&tenant=" + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );
    }

    delete(certificateOfOriginId: string) {


        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: CertificateOfOriginPM;
            // mappedEntity = this.MapJsonToEntityPM(entityPM, false);

            return this._http.delete(this._apiUrl + '/Delete/?' + 'CertificateOfOriginId=' + certificateOfOriginId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var pm = response;
                if (pm) {
                    var mappedResult: CertificateOfOriginPM;
                    //   mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                    serviceResponse.Result = mappedResult;
                }


                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));

        }

        );

    }
    GetCertificateOfOriginDocumentDeclarationId(declarationId: string, certificateOfOriginId: string) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetCertificateOfOriginDocumentDeclarationId/?declarationId=" + declarationId + "&certificateOfOriginId=" + certificateOfOriginId

                , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = response;
                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
        });
    }


    update(entityPM: CertificateOfOriginPM) {
        const apiUrl = ServiceHelper.GetLogitudeURL() + 'api/certificateoforigins';
        var callTime = new Date();
        return defer(() => {
            var serviceResponse: ServiceResponse = new ServiceResponse();
            var validator: ClassLevelValidator = new ClassLevelValidator();
            var errorsArray = validator.Validate("Customs.CertificateOfOrigin", entityPM);
            if (errorsArray.length == 0) {
                var mappedEntity: CertificateOfOriginPM = this.MapJsonToEntityPM(entityPM, false);
                return this._http.put(apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;
                            if (pm) {

                                entityPM.CertificateOriginInvoiceItems = mappedEntity.CertificateOriginInvoiceItems;
                                entityPM.CertificateOriginItemItems = mappedEntity.CertificateOriginItemItems;
                                var mappedResult: CertificateOfOriginPM = this.MapJsonToEntityPM(pm, false, entityPM);
                                serviceResponse.Result = mappedResult;
                            }
                            var servertime = response.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "CertificateOfOrigin", "SaveChanges", "");
                            return serviceResponse;
                        }),
                        catchError(ServiceHelper.HandleServiceError));
            }

            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;
                return of(serviceResponse);
            }
        });
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: CertificateOfOriginPM = null) {
        if (!entityPM) {
            entityPM = new CertificateOfOriginPM();
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

        this.MapCertificateOriginInvoiceItems(entityPM, jsonPM, mapParent); // Call composition tables map methods
        this.MapCertificateOriginItemItems(entityPM, jsonPM, mapParent); // Call composition tables map methods

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
            entityPM.OldEntityPM.CertificateOriginInvoiceItems = [];
            for (var item in entityPM.CertificateOriginInvoiceItems) {
                var myCertificateOfOriginInvoicePM = entityPM.CertificateOriginInvoiceItems[item];
                var newCertificateOfOriginInvoicePM: CertificateOfOriginInvoicePM = this.clone(myCertificateOfOriginInvoicePM);
                entityPM.OldEntityPM.CertificateOriginInvoiceItems.push(newCertificateOfOriginInvoicePM);
            }

            entityPM.OldEntityPM.CertificateOriginItemItems = [];
            for (var item in entityPM.CertificateOriginItemItems) {
                var myCertificateOfOriginItemPM = entityPM.CertificateOriginItemItems[item];
                var newCertificateOfOriginItemPM: CertificateOfOriginItemPM = this.clone(myCertificateOfOriginItemPM);
                entityPM.OldEntityPM.CertificateOriginItemItems.push(newCertificateOfOriginItemPM);
            }
        }
        else {
            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        entityPM.DisableMarkAsDirty = false;
        return entityPM;
    }

    MapCertificateOriginInvoiceItems(entityPM: CertificateOfOriginPM, jsonPM: any, mapParent: boolean = true) {

        var oldCertificateOriginInvoiceItems: CertificateOfOriginInvoicePM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCertificateOriginInvoiceItems = entityPM.OldEntityPM.CertificateOriginInvoiceItems;
        }

        entityPM.CertificateOriginInvoiceItems = new Array<CertificateOfOriginInvoicePM>();
        for (var item in jsonPM.CertificateOriginInvoiceItems) {
            var jItem = jsonPM.CertificateOriginInvoiceItems[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCertificateOfOriginInvoicePM: CertificateOfOriginInvoicePM;

            if (mapParent) {
                newCertificateOfOriginInvoicePM = new CertificateOfOriginInvoicePM(entityPM);
            }
            else {
                newCertificateOfOriginInvoicePM = new CertificateOfOriginInvoicePM(null);
            }
            newCertificateOfOriginInvoicePM.DisableMarkAsDirty = true;

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCertificateOfOriginInvoicePM[pmProperty] = jItem[pmProperty];
            }


            if (mapParent) {
                newCertificateOfOriginInvoicePM.UniqueKey = Guid.newGuid();
                newCertificateOfOriginInvoicePM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCertificateOfOriginInvoicePM.OldEntityPM = this.clone(newCertificateOfOriginInvoicePM);


            }
            else {
                if (newCertificateOfOriginInvoicePM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCertificateOfOriginInvoicePM.ChangeSetOp = "Update";
                }
                else {
                    newCertificateOfOriginInvoicePM.ChangeSetOp = "Insert";
                }

                newCertificateOfOriginInvoicePM.OldEntityPM = null;
                newCertificateOfOriginInvoicePM.EntityParentPM = null;
            }
            newCertificateOfOriginInvoicePM.DisableMarkAsDirty = false;
            newCertificateOfOriginInvoicePM.IsDirty = false;

            entityPM.CertificateOriginInvoiceItems.push(newCertificateOfOriginInvoicePM);
        }
        // if (oldCertificateOriginInvoiceItems) {

        //     entityPM.CertificateOriginInvoiceItems = new Array<CertificateOfOriginInvoicePM>();
        //     for (var itemKey in oldCertificateOriginInvoiceItems) {
        //         if (entityPM.CertificateOriginInvoiceItems.filter(p => p.UniqueKey === oldCertificateOriginInvoiceItems[itemKey].UniqueKey).length === 0) {

        //             if (oldCertificateOriginInvoiceItems[itemKey]) {
        //                 //oldCertificateOriginInvoiceItems[itemKey].ChangeSetOp = "Delete";
        //                 //entityPM.CertificateOriginInvoiceItems.push(oldCertificateOriginInvoiceItems[itemKey]);
        //                 var oldItemJson = oldCertificateOriginInvoiceItems[itemKey];
        //                 var deletedPM: CertificateOfOriginInvoicePM = new CertificateOfOriginInvoicePM(null);
        //                 deletedPM.DisableMarkAsDirty = true;
        //                 var pmKeys = Object.keys(oldItemJson);
        //                 for (var key in pmKeys) {

        //                     if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
        //                         continue;
        //                     }

        //                     var property = pmKeys[key];
        //                     deletedPM[property] = oldItemJson[property];
        //                 }

        //                 deletedPM.DisableMarkAsDirty = false;
        //                 deletedPM.IsDirty = false;
        //                 deletedPM.ChangeSetOp = "Delete";

        //                 deletedPM.OldEntityPM = null;

        //                 entityPM.CertificateOriginInvoiceItems.push(deletedPM);
        //             }
        //         }
        //     }
        // }
    }

    MapCertificateOriginItemItems(entityPM: CertificateOfOriginPM, jsonPM: any, mapParent: boolean = true) {

        var oldCertificateOriginItemItems: CertificateOfOriginItemPM[] = [];
        if (entityPM.OldEntityPM && !mapParent) {
            oldCertificateOriginItemItems = entityPM.OldEntityPM.CertificateOriginItemItems;
        }

        entityPM.CertificateOriginItemItems = new Array<CertificateOfOriginItemPM>();
        for (var item in jsonPM.CertificateOriginItemItems) {
            var jItem = jsonPM.CertificateOriginItemItems[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newCertificateOfOriginItemPM: CertificateOfOriginItemPM;

            if (mapParent) {
                newCertificateOfOriginItemPM = new CertificateOfOriginItemPM(entityPM);
            }
            else {
                newCertificateOfOriginItemPM = new CertificateOfOriginItemPM(null);
            }
            newCertificateOfOriginItemPM.DisableMarkAsDirty = true;

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {
                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newCertificateOfOriginItemPM[pmProperty] = jItem[pmProperty];
            }


            if (mapParent) {
                newCertificateOfOriginItemPM.UniqueKey = Guid.newGuid();
                newCertificateOfOriginItemPM.ChangeSetOp = "None";
                jItem.ChangeSetOp = "None";
                newCertificateOfOriginItemPM.OldEntityPM = this.clone(newCertificateOfOriginItemPM);


            }
            else {
                if (newCertificateOfOriginItemPM.UniqueKey) {

                    if (jItem.IsDirty)
                        newCertificateOfOriginItemPM.ChangeSetOp = "Update";
                }
                else {
                    newCertificateOfOriginItemPM.ChangeSetOp = "Insert";
                }

                newCertificateOfOriginItemPM.OldEntityPM = null;
                newCertificateOfOriginItemPM.EntityParentPM = null;
            }
            newCertificateOfOriginItemPM.DisableMarkAsDirty = false;
            newCertificateOfOriginItemPM.IsDirty = false;

            entityPM.CertificateOriginItemItems.push(newCertificateOfOriginItemPM);
        }
        // if (oldCertificateOriginItemItems) {

        //     for (var itemKey in oldCertificateOriginItemItems) {
        //         if (entityPM.CertificateOriginItemItems.filter(p => p.UniqueKey === oldCertificateOriginItemItems[itemKey].UniqueKey).length === 0) {

        //             if (oldCertificateOriginItemItems[itemKey]) {
        //                 //oldCertificateOriginItemItems[itemKey].ChangeSetOp = "Delete";
        //                 //entityPM.CertificateOriginItemItems.push(oldCertificateOriginItemItems[itemKey]);
        //                 var oldItemJson = oldCertificateOriginItemItems[itemKey];
        //                 var deletedPM: CertificateOfOriginItemPM = new CertificateOfOriginItemPM(null);
        //                 deletedPM.DisableMarkAsDirty = true;
        //                 var pmKeys = Object.keys(oldItemJson);
        //                 for (var key in pmKeys) {

        //                     if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM" || pmKeys[key] === "PropertyChanged") {
        //                         continue;
        //                     }

        //                     var property = pmKeys[key];
        //                     deletedPM[property] = oldItemJson[property];
        //                 }

        //                 deletedPM.DisableMarkAsDirty = false;
        //                 deletedPM.IsDirty = false;
        //                 deletedPM.ChangeSetOp = "Delete";

        //                 deletedPM.OldEntityPM = null;
        //                 entityPM.CertificateOriginItemItems.push(deletedPM);
        //             }
        //         }
        //     }
        // }
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
