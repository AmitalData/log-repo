import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class SignUpService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SignUp';
    }

    SendMessageToQueue(signupInfo: any) {
        var url = this._apiUrl + '/PutSendMessageToQueue';
        return defer(() => {
            return this._http.put(url, JSON.stringify(signupInfo), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    CreateTenant(signupInfo: any) {
        var url = this._apiUrl + '/PostCreateTenant';
        return defer(() => {
            return this._http.post(url, JSON.stringify(signupInfo), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
}
