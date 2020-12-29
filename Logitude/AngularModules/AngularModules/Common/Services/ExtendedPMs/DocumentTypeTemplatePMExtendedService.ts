


import {Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {DocumentTypeTemplatePM} from '../../EntityPMs/DocumentTypeTemplatePM';


@Injectable()
export class DocumentTypeTemplatePMExtendedService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeTemplateExtended';
    }


    GetSingleDocumentTypeTemplate(id: string, tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getsingledocumenttypetemplate/?' + 'id=' + id + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result :any = response;
            var entity: DocumentTypeTemplatePM;
            entity = this.MapJsonToEntityPM(result);

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = entity;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetDocumentTypeTemplatesPMForDocumentType(documentTypeId: string, templateType: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '?documentTypeId=' + documentTypeId + '&templateType=' + templateType + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
            var entity: DocumentTypeTemplatePM;
            var DocumentTypeTemplatePMLists: DocumentTypeTemplatePM[];
            DocumentTypeTemplatePMLists = new Array<DocumentTypeTemplatePM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                DocumentTypeTemplatePMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = DocumentTypeTemplatePMLists;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetTemplateBodyByDocumentTemplateId(documentTypeTemplateId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/GetTemplateBodyByDocumentTemplateId?documentTypeTemplateId=' + documentTypeTemplateId + "&tenant=" + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
        
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetTemplateBodyhtmlOrJsonByDocumentTemplateId(documentTyptemplateId: string, tenant: number, isHtml: boolean, pageType: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '?documentTyptemplateId=' + documentTyptemplateId + "&tenant=" + tenant + "&isHtml=" + isHtml + "&pagetype=" + pageType,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
   
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }
  
    //string documentTypeId, int tenant
    getDocumentTypeTemplatesByDocumentTypeIdForAutomations(documentTypeId: string, editorToolCode:string ,tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '/getdocumenttypetemplatesbydocumenttypeidforautomations/?' + 'documentTypeId=' + documentTypeId + '&editorToolCode=' + editorToolCode+ '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
       
            var result :any = response;
            var entity: DocumentTypeTemplatePM;
            var DocumentTypeTemplatePMLists: DocumentTypeTemplatePM[];
            DocumentTypeTemplatePMLists = new Array<DocumentTypeTemplatePM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                DocumentTypeTemplatePMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = DocumentTypeTemplatePMLists;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    SaveDocumentTemplate(filter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.put(this._apiUrl + '/PutSaveDocumentTypeTemplate', JSON.stringify(filter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result :any = response;

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }


    ConvertXmalByteTojosnObject(filter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return defer(() => {
            return this._http.put(this._apiUrl + '/PutConvertXmalByteTojosnObject', JSON.stringify(filter),ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result :any = response;

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }


    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: DocumentTypeTemplatePM;
        entityPM = new DocumentTypeTemplatePM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }

}

