import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';


import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class DocumentsExecutionLogListExtendedService {


    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DocumentsExecutionLogExtended';
    }



    GetDocumentsExecutionLogList(id: string) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/GetDocumentsExecutionLogList/?' + 'id=' + id , { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

}

