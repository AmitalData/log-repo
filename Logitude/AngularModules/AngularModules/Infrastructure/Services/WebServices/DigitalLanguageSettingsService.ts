import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer } from 'rxjs';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';

@Injectable()
export class DigitalLanguageSettingsService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DigitalLanguageSettings';
    }

    public GetDigitalLanguages(objectTableId: string = '', screenCode: string = '', profileCode: string = '') {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetDigitalPortalScreens?objectTableId=' + objectTableId + "&screenCode=" + screenCode + "&profileCode=" + profileCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

}

