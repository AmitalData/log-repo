import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable} from 'rxjs/Observable';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {SessionInfo} from '../../Utilities/SessionInfo';
@Injectable()

export class LogitudeApplicationService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/LogitudeApplication';
    }

    GetCheckIsupgradingSystem() {
        var authHeader = new Headers();
        authHeader.append('Content-Type', 'application/json');
        return this._http.get(this._apiUrl
            , {
                headers: authHeader,
            }).map(response => {
                var result = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;

            });

    }

    GetCurrenctUserValidity() {
        var authHeader = new Headers();
        authHeader.append('Content-Type', 'application/json');
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCurrenctUserValidity?clientEmail=' + SessionInfo.LoggedUserEmail + "&documentToken=" + SessionInfo.DocumentDownloadToken + '&tenant=' + SessionInfo.LoggedUserTenant 
                , {
                    headers: authHeader,
                }).map(res => {

                    var response: ServiceResponse;
                    response = new ServiceResponse();

                    response.Result = res.json();

                    return response;

                }).catch(ServiceHelper.HandleTimerServiceError);
        });

    }
}

