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
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/';
    }

    public GetDigitalLanguages(langCode: string = '') {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + 'DigitalCustomization/GetDigitalPortalLanguages?LanguageCode=' + langCode , ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetDigitalToExcelData(payload: any) {
        return defer(() => {
            return this._http.post(this._apiUrl + "DigitalPortalReport/GetDigitalToExcelData", JSON.stringify(payload), ServiceHelper.GetHttpHeaders())
                .pipe(
                    map((response: any) => {
                    }),
                    catchError(ServiceHelper.HandleServiceError));

        });
    }

}

