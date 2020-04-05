import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse'; 


@Injectable()
export class DocumentTypeMetaDataExtendedService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeMetaData';
    }


 
    GetDocumentTypeMetaDataByDocumentTypeId(documentTypeId: string,  tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?DocumentTypeId=' + documentTypeId + '&tenant=' + tenant ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetDocumentMetaDataValuesByDocument(tenant: number,documentId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?tenant=' + tenant + '&DocumentId=' + documentId,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetDocumentsMetaDataTypeByCode(Code: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?Code=' + Code,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }
    

}

