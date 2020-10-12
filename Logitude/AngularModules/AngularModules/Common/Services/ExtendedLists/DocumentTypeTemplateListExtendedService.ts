import {Injectable, } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class DocumentTypeTemplateListExtendedService {


    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeTemplateExtended';
    }




    getDocumentTypeTemplateListsForDocumentType(documentTypeId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getdocumenttypetemplatelistsfordocumenttype/?' + 'documentTypeId=' + documentTypeId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
        //return this._http.get(this._apiUrl + '?documentTypeId=' + documentTypeId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetDocumentTypeTemplatesFromLibraryByDocumentTypeId(tenant: number, documentTypeId: string, isfilter: boolean, mytenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getdocumentTypetemplatesfromlibrarybydocumenttypeid/?'+  'tenant=' + tenant + '&documentTypeId=' + documentTypeId + '&isfilter=' + isfilter + '&mytenant=' + mytenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
         
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

           

    GetDocumentTypeTemplatesFromLibrary(objecttableid: string, tenant: number, isfilter: boolean, transportModeId: string, shipmentLevelCode: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getdocumentTypetemplatesfromlibrary/?'+ 'objecttableid=' + objecttableid + '&tenant=' + tenant + '&isfilter=' + isfilter + '&transportModeId=' + transportModeId + '&shipmentLevelCode=' + shipmentLevelCode ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }


    CopyDocumentTypeAndDocumentTypTemplate(docmentTypeTemplateId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        var x = 1;
        return this._http.get(this._apiUrl + '/getcopydocumenttypeanddocumenttyptemplate/?'  +  'docmentTypeTemplateId=' + docmentTypeTemplateId + '&tenant=' + tenant ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }



    GetDocumentTypeTemplatesDefultAttachments(documentTypeTemplateId: string, objectTableId: string, entityId: string, childEntityId: string) {


        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/GetDocumentTypeTemplatesDefultAttachments/?' + 'documentTypeTemplateId=' + documentTypeTemplateId + '&objectTableId=' + objectTableId + '&entityId=' + entityId + '&childEntityId=' + childEntityId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }







}

