import {Injectable, } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class DocumentTypeTemplateListExtendedService {


    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeTemplateExtended';
    }




    getDocumentTypeTemplateListsForDocumentType(documentTypeId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getdocumenttypetemplatelistsfordocumenttype/?' + 'documentTypeId=' + documentTypeId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
        //return this._http.get(this._apiUrl + '?documentTypeId=' + documentTypeId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetDocumentTypeTemplatesFromLibraryByDocumentTypeId(tenant: number, documentTypeId: string, isfilter: boolean, mytenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getdocumentTypetemplatesfromlibrarybydocumenttypeid/?'+  'tenant=' + tenant + '&documentTypeId=' + documentTypeId + '&isfilter=' + isfilter + '&mytenant=' + mytenant, { headers: authHeader }).map(response => {
         
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

           

    GetDocumentTypeTemplatesFromLibrary(objecttableid: string, tenant: number, isfilter: boolean, transportModeId: string, shipmentLevelCode: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/getdocumentTypetemplatesfromlibrary/?'+ 'objecttableid=' + objecttableid + '&tenant=' + tenant + '&isfilter=' + isfilter + '&transportModeId=' + transportModeId + '&shipmentLevelCode=' + shipmentLevelCode , { headers: authHeader }).map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }


    CopyDocumentTypeAndDocumentTypTemplate(docmentTypeTemplateId: string, tenant: number) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        var x = 1;
        return this._http.get(this._apiUrl + '/getcopydocumenttypeanddocumenttyptemplate/?'  +  'docmentTypeTemplateId=' + docmentTypeTemplateId + '&tenant=' + tenant , { headers: authHeader }).map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }


}

