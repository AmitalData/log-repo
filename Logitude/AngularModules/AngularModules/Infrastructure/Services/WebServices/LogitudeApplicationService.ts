import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class LogitudeApplicationService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/LogitudeApplication';
    }

    GetCheckIsupgradingSystem() {
        return this._http.get(this._apiUrl, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result = response;
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = result;

            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetCurrenctUserValidity() {
        var url = this._apiUrl + '/GetCurrenctUserValidity?clientEmail=' + SessionInfo.LoggedUserEmail + "&documentToken=" + SessionInfo.DocumentDownloadToken + '&tenant=' + SessionInfo.LoggedUserTenant;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();

                serviceResponse.Result = response

                return serviceResponse;
            }), catchError(ServiceHelper.HandleTimerServiceError));
        });
    }
}
