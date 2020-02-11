import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';


import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse'; 


@Injectable()
export class DocumentTypeMetaDataExtendedService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentTypeMetaData';
    }


 
    GetDocumentTypeMetaDataByDocumentTypeId(documentTypeId: string,  tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?DocumentTypeId=' + documentTypeId + '&tenant=' + tenant, { headers: authHeader}).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetDocumentMetaDataValuesByDocument(tenant: number,documentId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?tenant=' + tenant + '&DocumentId=' + documentId, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetDocumentsFilingMetaDataValueByFilingIdAndCode(documentsFilingId: string, code: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?documentsFilingId=' + documentsFilingId + '&code=' + code, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetDocumentsMetaDataTypeByCode(Code: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?Code=' + Code, { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }
    

}

