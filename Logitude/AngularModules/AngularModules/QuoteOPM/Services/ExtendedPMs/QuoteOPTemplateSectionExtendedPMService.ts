import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


import { defer, of } from 'rxjs';

import {QuoteOPTemplateSectionPM} from '../../EntityPMs/QuoteOPTemplateSectionPM';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class QuoteOPTemplateSectionExtendedPMService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuoteOPTemplateSectionExtended';
    }



    GetQuoteOPTemplateSectionByQuoteOPTemplateId(QuoteOPTemplateId: string, tenant: number) {

        return this._http.get(this._apiUrl + '/GetQuoteOPTemplateSectionByQuoteOPTemplateId/?' + 'QuoteOPTemplateId=' + QuoteOPTemplateId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result: any = response;
            var entity: QuoteOPTemplateSectionPM;
            var QuoteOPTemplateSectionPMLists: QuoteOPTemplateSectionPM[];
            QuoteOPTemplateSectionPMLists = new Array<QuoteOPTemplateSectionPM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                QuoteOPTemplateSectionPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = QuoteOPTemplateSectionPMLists;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }



    updateSections(QuoteOPTemplateSections: any) {
        return defer(() => {

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl + '/PutQuoteOPTemplateSections', JSON.stringify(QuoteOPTemplateSections), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var pm = res;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        } );
    }


   // GetDownloadQuoteOPTemplateSectionPdfFile(string sectionTypeCode, string sectionDocId, string QuoteOPTemplateId, int tenant, string settingId, string quoteId)
    DownloadQuoteOPTemplateSectionPdfFile(sectionTypeCode: string, sectionDocId: string, QuoteOPTemplateId: string, settingId: string, quoteId: string, userId:string, tenant: number) {

        return this._http.get(this._apiUrl + '/GetDownloadQuoteOPTemplateSectionPdfFile/?' + 'sectionTypeCode=' + sectionTypeCode + '&sectionDocId=' + sectionDocId + '&QuoteOPTemplateId=' + QuoteOPTemplateId + '&settingId=' + settingId + '&quoteId=' + quoteId + '&userId=' + userId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result = response;
           
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }


    GetQuoteOPTemplatePdfReport(quoteId: string, QuoteOPTemplateId: string, userId: string, isFromLibrary: boolean = false) {


        return this._http.get(this._apiUrl + '/GetQuoteOPTemplatePdfReport/?' + 'quoteId=' + quoteId + '&QuoteOPTemplateId=' + QuoteOPTemplateId + '&userId=' + userId + '&isFromLibrary=' + isFromLibrary, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result = response;
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = result
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }



 GetMakeQuoteOPTemplateSectionsIncluded(quoteId: string, QuoteOPTemplateId: string, QuoteOPTemplatesectionId: string, tenant: number) {

     return this._http.get(this._apiUrl + '/GetMakeQuoteOPTemplateSectionsIncluded/?' + 'quoteId=' + quoteId + '&QuoteOPTemplateId=' + QuoteOPTemplateId + '&QuoteOPTemplatesectionId=' + QuoteOPTemplatesectionId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
         var result = response;
         var pmresponse: ServiceResponse;
         pmresponse = new ServiceResponse();

         pmresponse.Result = result
         return pmresponse;
     }), catchError(ServiceHelper.HandleServiceError));
 }

 GetMakeQuoteOPTemplateSectionsExcluded(quoteId: string, QuoteOPTemplateId: string, QuoteOPTemplatesectionId: string, tenant: number) {

     return this._http.get(this._apiUrl + '/GetMakeQuoteOPTemplateSectionsExcluded/?' + 'quoteId=' + quoteId + '&QuoteOPTemplateId=' + QuoteOPTemplateId + '&QuoteOPTemplatesectionId=' + QuoteOPTemplatesectionId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
         var result = response;
         var pmresponse: ServiceResponse;
         pmresponse = new ServiceResponse();

         pmresponse.Result = result
         return pmresponse;
     }), catchError(ServiceHelper.HandleServiceError));
 }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: QuoteOPTemplateSectionPM;
        entityPM = new QuoteOPTemplateSectionPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }



}

