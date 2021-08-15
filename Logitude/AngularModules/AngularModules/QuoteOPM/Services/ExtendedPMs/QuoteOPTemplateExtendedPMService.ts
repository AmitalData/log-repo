
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass'
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';

import { QuoteOPTemplatePM } from '../../EntityPMs/QuoteOPTemplatePM';

import { QuoteOPTemplateSectionPM } from '../../EntityPMs/QuoteOPTemplateSectionPM';

import { HttpClient, HttpResponse } from '@angular/common/http';

import { catchError, map } from 'rxjs/operators';

@Injectable()

export class QuoteOPTemplateExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    private _httpClient: HttpClient
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuoteOPTemplateExtended';
    }


    insert(entityPM: QuoteOPTemplatePM) {

        var callTime = new Date();
        return defer(() => {

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: QuoteOPTemplatePM;
            mappedEntity = this.MapJsonToEntityPM(entityPM, false);
            return this._httpClient.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders()).pipe(
                map((response: HttpResponse<any>) => {
                    var pm = response.body;
                    if (pm) {
                        var mappedResult: QuoteOPTemplatePM;
                        mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                        serviceResponse.Result = mappedResult;
                    }

                    var servertime = response.headers.get('ServerExecutionTime');
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "QuoteOPTemplate", "SaveChanges", "");

                    return serviceResponse;

                }),

                catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetQuoteOPTemplateLists(queryName: string) {

        return this._httpClient.get(this._apiUrl + '/GetQuoteOPTemplateLists/?' + 'queryName=' + queryName, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = response;
                return pmresponse;
            }),

            catchError(ServiceHelper.HandleServiceError));
    }


    GetTemplateSectionsByQuoteOPTemplateIdAndQuoteId(id: string, quoteId: string, tenant: number, defultQuoteOPTemplate: string, quotationSections: string) {

        return this._http.get(this._apiUrl + '/GetTemplateSectionsByQuoteOPTemplateIdAndQuoteId/?' + 'id=' + id + '&quoteId=' + quoteId + '&tenant=' + tenant + '&defultQuoteOPTemplate=' + defultQuoteOPTemplate + '&quotationSections=' + quotationSections, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }



    GetQuoteOPTemplateListsByQuoteOPTemplateTypeAndTenant(QuoteOPTemplatetype: string, tenant: number) {
         return this._http.get(this._apiUrl + '/GetQuoteOPTemplateListsByQuoteOPTemplateTypeAndTenant/?' + 'QuoteOPTemplatetype=' + QuoteOPTemplatetype + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }




    GetQuoteOPTemplateListsFromLibrary(QuoteOPTemplatetype) {
        return this._http.get(this._apiUrl + '/GetQuoteOPTemplateListsFromLibrary/?QuoteOPTemplatetype=' + QuoteOPTemplatetype, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }





    GetCopyQuoteOPTemplateFromLibrary(QuoteOPTemplateId: string, userId: string) {
        return this._http.get(this._apiUrl + '/GetCopyQuoteOPTemplateFromLibrary/?' + 'QuoteOPTemplateId=' + QuoteOPTemplateId + '&userId=' + userId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }







    GetCopyQuoteOPTemplate(QuoteOPTemplateId: string, copyName: string, userid: string, tenant: number) {

        return this._http.get(this._apiUrl + '/GetCopyQuoteOPTemplate/?' + 'QuoteOPTemplateId=' + QuoteOPTemplateId + '&copyName=' + copyName + '&userid=' + userid + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pm = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = this.MapJsonToEntityPM(pm, true);
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetQuoteOPTemplatePdfReport(quotePMId: string, templateId: string, userId: string) {

        return this._http.get(this._apiUrl + '/GetQuoteOPTemplatePdfReport/?' + 'quoteId=' + quotePMId + '&QuoteOPTemplateId=' + templateId + '&userId=' + userId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetUpdatedQuoteDocumentVersion(quoteId: string, versionNumber: number, QuoteOPTemplateId: string, updatedByUserId: string, tenant: number, isGenerate: boolean = false) {

        return this._http.get(this._apiUrl + '/GetUpdatedQuoteDocumentVersion/?' + 'quoteId=' + quoteId + '&versionNumber=' + versionNumber + '&QuoteOPTemplateId=' + QuoteOPTemplateId + '&updatedByUserId=' + updatedByUserId + '&tenant=' + tenant + '&isGenerate=' + isGenerate, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));

    }
    GetQuoteDocumentVersionsByQuoteId(quoteId: string, tenant: number) {

        return this._http.get(this._apiUrl + '/GetQuoteDocumentVersionsByQuoteId/?' + 'quoteId=' + quoteId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetQuoteCustomerEmailByContactId(contactId: string) {

        return this._http.get(this._apiUrl + '/GetQuoteCustomerEmailByContactId/?' + 'contactId=' + contactId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    UpLoadQuoteDocumentVersionFile(fileData: any, tenant: number) {
  
        return this._http.post(this._apiUrl + '/PostUpLoadQuoteDocumentVersionFile/?' + 'tenant=' + tenant, JSON.stringify(fileData), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
        //PostUpLoadQuoteDocumentVersionFile
        //PostUpLoadQuoteDocumentVersionFile(byte[] fileData, string quoteId, int versionNumber, string fileExtension, string updatedByUserId, int tenant)
    }
    //quotesContext.GetQuoteDocumentVersionsByQuoteIdQuery(this.QuotePM.Id, TenantContext.Current.Id), 

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: QuoteOPTemplatePM = null) {


        if (!entityPM) {

            entityPM = new QuoteOPTemplatePM();
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

        this.MapTemplateSections(entityPM, jsonPM, mapParent); // Call composition tables map methods

        entityPM.IsDirty = false;

        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

            entityPM.OldEntityPM.TemplateSections = [];
            for (var item in entityPM.TemplateSections) {
                var myQuoteOPTemplateSectionPM = entityPM.TemplateSections[item];
                var newQuoteOPTemplateSectionPM: QuoteOPTemplateSectionPM = this.clone(myQuoteOPTemplateSectionPM);


                entityPM.OldEntityPM.TemplateSections.push(newQuoteOPTemplateSectionPM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }

    MapTemplateSections(entityPM: QuoteOPTemplatePM, jsonPM: any, mapParent: boolean = true) {

        entityPM.TemplateSections = new Array<QuoteOPTemplateSectionPM>();
        for (var item in jsonPM.TemplateSections) {

            var jItem = jsonPM.TemplateSections[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newQuoteOPTemplateSectionPM: QuoteOPTemplateSectionPM;
            newQuoteOPTemplateSectionPM = new QuoteOPTemplateSectionPM();

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newQuoteOPTemplateSectionPM[pmProperty] = jItem[pmProperty];
            }
            newQuoteOPTemplateSectionPM.IsDirty = false;
            entityPM.TemplateSections.push(newQuoteOPTemplateSectionPM);
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
        var entityPM: QuoteOPTemplatePM;
        entityPM = new QuoteOPTemplatePM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }


}
