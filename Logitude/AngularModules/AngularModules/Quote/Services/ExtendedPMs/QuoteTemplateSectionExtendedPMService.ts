import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';


import {Observable}     from 'rxjs/Rx';

import {QuoteTemplateSectionPM} from '../../EntityPMs/QuoteTemplateSectionPM';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class QuoteTemplateSectionExtendedPMService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuoteTemplateSectionExtended';
    }



    GetQuoteTemplateSectionByQuoteTemplateId(quoteTemplateId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplateSectionByQuoteTemplateId/?' + 'quoteTemplateId=' + quoteTemplateId + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            var entity: QuoteTemplateSectionPM;
            var quoteTemplateSectionPMLists: QuoteTemplateSectionPM[];
            quoteTemplateSectionPMLists = new Array<QuoteTemplateSectionPM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                quoteTemplateSectionPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = quoteTemplateSectionPMLists;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }



    updateSections(quoteTemplateSections: any) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl + '/PutQuoteTemplateSections', JSON.stringify(quoteTemplateSections),
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    return serviceResponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        );

    }


   // GetDownloadQuoteTemplateSectionPdfFile(string sectionTypeCode, string sectionDocId, string quoteTemplateId, int tenant, string settingId, string quoteId)
    DownloadQuoteTemplateSectionPdfFile(sectionTypeCode: string, sectionDocId: string, quoteTemplateId: string, settingId: string, quoteId: string, userId:string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetDownloadQuoteTemplateSectionPdfFile/?' + 'sectionTypeCode=' + sectionTypeCode + '&sectionDocId=' + sectionDocId + '&quoteTemplateId=' + quoteTemplateId + '&settingId=' + settingId + '&quoteId=' + quoteId + '&userId=' + userId + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
           
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }


    GetQuoteTemplatePdfReport(quoteId: string, quoteTemplateId: string, userId: string, isFromLibrary: boolean = false) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/GetQuoteTemplatePdfReport/?' + 'quoteId=' + quoteId + '&quoteTemplateId=' + quoteTemplateId + '&userId=' + userId + '&isFromLibrary=' + isFromLibrary , { headers: authHeader }).map(response => {
            var result = response.json();
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }



 GetMakeQuoteTemplateSectionsIncluded(quoteId: string, quotetemplateId: string, quotetemplatesectionId: string, tenant: number) {
     var authHeader = new Headers();
     authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
     return this._http.get(this._apiUrl + '/GetMakeQuoteTemplateSectionsIncluded/?' + 'quoteId=' + quoteId + '&quotetemplateId=' + quotetemplateId + '&quotetemplatesectionId=' + quotetemplatesectionId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
         var result = response.json();
         var pmresponse: ServiceResponse;
         pmresponse = new ServiceResponse();

         pmresponse.Result = result
         return pmresponse;
     }).catch(ServiceHelper.HandleServiceError);
 }

 GetMakeQuoteTemplateSectionsExcluded(quoteId: string, quotetemplateId: string, quotetemplatesectionId: string, tenant: number) {
     var authHeader = new Headers();
     authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
     return this._http.get(this._apiUrl + '/GetMakeQuoteTemplateSectionsExcluded/?' + 'quoteId=' + quoteId + '&quotetemplateId=' + quotetemplateId + '&quotetemplatesectionId=' + quotetemplatesectionId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
         var result = response.json();
         var pmresponse: ServiceResponse;
         pmresponse = new ServiceResponse();

         pmresponse.Result = result
         return pmresponse;
     }).catch(ServiceHelper.HandleServiceError);
 }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: QuoteTemplateSectionPM;
        entityPM = new QuoteTemplateSectionPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }



}

