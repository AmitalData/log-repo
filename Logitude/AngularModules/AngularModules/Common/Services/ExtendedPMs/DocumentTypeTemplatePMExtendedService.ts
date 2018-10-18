


import {Injectable, } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {DocumentTypeTemplatePM} from '../../EntityPMs/DocumentTypeTemplatePM';


@Injectable()
export class DocumentTypeTemplatePMExtendedService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeTemplateExtended';
    }


    GetSingleDocumentTypeTemplate(id: string, tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getsingledocumenttypetemplate/?' + 'id=' + id + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            var entity: DocumentTypeTemplatePM;
            entity = this.MapJsonToEntityPM(result);

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetDocumentTypeTemplatesPMForDocumentType(documentTypeId: string, templateType: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '?documentTypeId=' + documentTypeId + '&templateType=' + templateType + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
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
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetTemplateBodyByDocumentTemplateId(documentTypeTemplateId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/GetTemplateBodyByDocumentTemplateId?documentTypeTemplateId=' + documentTypeTemplateId + "&tenant=" + tenant, { headers: authHeader }).map(response => {
        
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetTemplateBodyhtmlOrJsonByDocumentTemplateId(documentTyptemplateId: string, tenant: number, isHtml: boolean, pageType: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '?documentTyptemplateId=' + documentTyptemplateId + "&tenant=" + tenant + "&isHtml=" + isHtml + "&pagetype=" + pageType, { headers: authHeader }).map(response => {
   
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }
  
    //string documentTypeId, int tenant
    getDocumentTypeTemplatesByDocumentTypeIdForAutomations(documentTypeId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '/getdocumenttypetemplatesbydocumenttypeidforautomations/?' + 'documentTypeId=' + documentTypeId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
       
            var result = response.json();
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
        }).catch(ServiceHelper.HandleServiceError);
    }

    SaveDocumentTemplate(filter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/PutSaveDocumentTypeTemplate', JSON.stringify(filter), {
                headers: authHeader,

            }).map(response => {
                var result = response.json();

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
                }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }


    ConvertXmalByteTojosnObject(filter: any) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.put(this._apiUrl + '/PutConvertXmalByteTojosnObject', JSON.stringify(filter), {
                headers: authHeader,

            }).map(response => {
                var result = response.json();

                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
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

