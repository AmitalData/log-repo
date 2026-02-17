import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable} from 'rxjs/Observable';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse'; 
@Injectable()

export class TermsofUseService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http; 
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TermsofUse';
    }

    GetCheckIfGoToTermUseComponent(tenant: number, userId: string) {

        var s = true;
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return this._http.get(this._apiUrl + '?tenant=' + tenant + '&userId=' + userId
            , {
                headers: authHeader,
            }).map(response => {
           
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = response.json();
                return pmresponse;

            });
    }
}

