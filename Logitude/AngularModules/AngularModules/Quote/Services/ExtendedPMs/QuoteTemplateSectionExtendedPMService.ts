import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


import { defer, of } from 'rxjs';

import {QuoteTemplateSectionPM} from '../../EntityPMs/QuoteTemplateSectionPM';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class QuoteTemplateSectionExtendedPMService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuoteTemplateSectionExtended';
    }



    GetQuoteTemplateSectionByQuoteTemplateId(quoteTemplateId: string, tenant: number) {

        return this._http.get(this._apiUrl + '/GetQuoteTemplateSectionByQuoteTemplateId/?' + 'quoteTemplateId=' + quoteTemplateId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result: any = response;
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
        }), catchError(ServiceHelper.HandleServiceError));
    }



    updateSections(quoteTemplateSections: any) {
        return defer(() => {

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl + '/PutQuoteTemplateSections', JSON.stringify(quoteTemplateSections), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var pm = res;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        } );
    }


   // GetDownloadQuoteTemplateSectionPdfFile(string sectionTypeCode, string sectionDocId, string quoteTemplateId, int tenant, string settingId, string quoteId)
    DownloadQuoteTemplateSectionPdfFile(sectionTypeCode: string, sectionDocId: string, quoteTemplateId: string, settingId: string, quoteId: string, userId:string, tenant: number) {

        return this._http.get(this._apiUrl + '/GetDownloadQuoteTemplateSectionPdfFile/?' + 'sectionTypeCode=' + sectionTypeCode + '&sectionDocId=' + sectionDocId + '&quoteTemplateId=' + quoteTemplateId + '&settingId=' + settingId + '&quoteId=' + quoteId + '&userId=' + userId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
           
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }


    GetQuoteTemplatePdfReport(quoteId: string, quoteTemplateId: string, userId: string, isFromLibrary: boolean = false) {


        return this._http.get(this._apiUrl + '/GetQuoteTemplatePdfReport/?' + 'quoteId=' + quoteId + '&quoteTemplateId=' + quoteTemplateId + '&userId=' + userId + '&isFromLibrary=' + isFromLibrary, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }


 GetMakeQuoteTemplateSectionsIncluded(quoteId: string, quotetemplateId: string, quotetemplatesectionId: string, tenant: number) {

     return this._http.get(this._apiUrl + '/GetMakeQuoteTemplateSectionsIncluded/?' + 'quoteId=' + quoteId + '&quotetemplateId=' + quotetemplateId + '&quotetemplatesectionId=' + quotetemplatesectionId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
         var result = response;
         var pmresponse: ServiceResponse;
         pmresponse = new ServiceResponse();

         pmresponse.Result = result
         return pmresponse;
     }), catchError(ServiceHelper.HandleServiceError));
 }

 GetMakeQuoteTemplateSectionsExcluded(quoteId: string, quotetemplateId: string, quotetemplatesectionId: string, tenant: number) {

     return this._http.get(this._apiUrl + '/GetMakeQuoteTemplateSectionsExcluded/?' + 'quoteId=' + quoteId + '&quotetemplateId=' + quotetemplateId + '&quotetemplatesectionId=' + quotetemplatesectionId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
         var result = response;
         var pmresponse: ServiceResponse;
         pmresponse = new ServiceResponse();

         pmresponse.Result = result
         return pmresponse;
     }), catchError(ServiceHelper.HandleServiceError));
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

