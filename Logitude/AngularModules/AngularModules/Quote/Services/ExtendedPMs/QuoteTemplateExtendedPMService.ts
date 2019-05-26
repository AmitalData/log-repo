
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';

import {QuoteTemplatePM} from '../../EntityPMs/QuoteTemplatePM';

import {QuoteTemplateSectionPM} from '../../EntityPMs/QuoteTemplateSectionPM';

@Injectable()

export class QuoteTemplateExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuoteTemplateExtended';
    }

 
    insert(entityPM: QuoteTemplatePM) {

        var callTime = new Date();
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

                var mappedEntity: QuoteTemplatePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
                    { headers: authHeader }).map((response) => {

                        var pm = response.json();
                        if (pm) {
                            var mappedResult: QuoteTemplatePM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }
                        
                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "QuoteTemplate", "SaveChanges", "");


                        return serviceResponse;

                    }).catch(ServiceHelper.HandleServiceError);
         
          
        }

        );
    }


    GetTemplateSectionsByQuoteTemplateIdAndQuoteId(id: string, quoteId: string, tenant: number, defultQuoteTemplate: string, quotationSections: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetTemplateSectionsByQuoteTemplateIdAndQuoteId/?' + 'id=' + id + '&quoteId=' + quoteId + '&tenant=' + tenant + '&defultQuoteTemplate=' + defultQuoteTemplate + '&quotationSections=' + quotationSections, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }
    


    GetQuoteTemplateListsByQuoteTemplateTypeAndTenant(quotetemplatetype: string, tenant : number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplateListsByQuoteTemplateTypeAndTenant/?' + 'quotetemplatetype=' + quotetemplatetype+ '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    
    GetQuoteTemplateLists(queryName: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplateLists/?' + 'queryName=' + queryName , { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetQuoteTemplateListsFromLibrary(quotetemplatetype) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplateListsFromLibrary/?quotetemplatetype=' + quotetemplatetype, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }



    

    GetCopyQuoteTemplateFromLibrary(quoteTemplateId: string,  userId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetCopyQuoteTemplateFromLibrary/?' + 'quoteTemplateId=' + quoteTemplateId +  '&userId=' + userId , { headers: authHeader }).map(response => {
         
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }







    GetCopyQuoteTemplate(quoteTemplateId: string, copyName: string, userid:string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetCopyQuoteTemplate/?' + 'quoteTemplateId=' + quoteTemplateId + '&copyName=' + copyName + '&userid=' + userid + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var pm = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = this.MapJsonToEntityPM(pm, true);
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetQuoteTemplatePdfReport(quotePMId: string, templateId: string, userId: string) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplatePdfReport/?' + 'quoteId=' + quotePMId + '&quoteTemplateId=' + templateId + '&userId=' + userId , { headers: authHeader }).map(response => {
            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetUpdatedQuoteDocumentVersion(quoteId: string, versionNumber: number, quoteTemplateId: string, updatedByUserId: string, tenant: number, isGenerate: boolean = false) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetUpdatedQuoteDocumentVersion/?' + 'quoteId=' + quoteId + '&versionNumber=' + versionNumber + '&quoteTemplateId=' + quoteTemplateId + '&updatedByUserId=' + updatedByUserId + '&tenant=' + tenant + '&isGenerate=' + isGenerate, { headers: authHeader }).map(response => {
            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
        
    }
    GetQuoteDocumentVersionsByQuoteId(quoteId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteDocumentVersionsByQuoteId/?' + 'quoteId=' + quoteId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetQuoteCustomerEmailByContactId(contactId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteCustomerEmailByContactId/?' + 'contactId=' + contactId, { headers: authHeader }).map(response => {
            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    UpLoadQuoteDocumentVersionFile(fileData:any,tenant:number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.post(this._apiUrl + '/PostUpLoadQuoteDocumentVersionFile/?' + 'tenant=' + tenant, JSON.stringify(fileData), { headers: authHeader }).map(response => {
            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
        //PostUpLoadQuoteDocumentVersionFile
        //PostUpLoadQuoteDocumentVersionFile(byte[] fileData, string quoteId, int versionNumber, string fileExtension, string updatedByUserId, int tenant)
    }
    //quotesContext.GetQuoteDocumentVersionsByQuoteIdQuery(this.QuotePM.Id, TenantContext.Current.Id), 

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: QuoteTemplatePM = null) {


        if (!entityPM) {

            entityPM = new QuoteTemplatePM();
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
                var myQuoteTemplateSectionPM = entityPM.TemplateSections[item];
                var newQuoteTemplateSectionPM: QuoteTemplateSectionPM = this.clone(myQuoteTemplateSectionPM);


                entityPM.OldEntityPM.TemplateSections.push(newQuoteTemplateSectionPM);
            }

        }
        else {

            entityPM.OldEntityPM = null;
        }

        return entityPM;
    }

    MapTemplateSections(entityPM: QuoteTemplatePM, jsonPM: any, mapParent: boolean = true) {

        entityPM.TemplateSections = new Array<QuoteTemplateSectionPM>();
        for (var item in jsonPM.TemplateSections) {

            var jItem = jsonPM.TemplateSections[item];
            if (mapParent && (jItem.ChangeSetOp == "Delete" || jItem.ChangeSetOp == 3)) {
                continue;
            }
            var newQuoteTemplateSectionPM: QuoteTemplateSectionPM;
            newQuoteTemplateSectionPM = new QuoteTemplateSectionPM();

            var pmKeysArray = Object.keys(jItem);
            for (var pmKey in pmKeysArray) {

                if ((!mapParent && pmKeysArray[pmKey] === "entityParentPM") || pmKeysArray[pmKey] === "UIProperties" || pmKeysArray[pmKey] === "PropertyChanged") {
                    continue;
                }
                var pmProperty = pmKeysArray[pmKey];
                newQuoteTemplateSectionPM[pmProperty] = jItem[pmProperty];
            }
            newQuoteTemplateSectionPM.IsDirty = false;
            entityPM.TemplateSections.push(newQuoteTemplateSectionPM);
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
        var entityPM: QuoteTemplatePM;
        entityPM = new QuoteTemplatePM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }


}
