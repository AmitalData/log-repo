import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { defer } from 'rxjs';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { catchError, map } from 'rxjs/operators';
import { ChartingDataClass } from '../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';

@Injectable()

export class CustomsRequestsSheetWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsRequestsSheetViewsExtended';
    }
    GetStatistics() {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetStatistics" , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );
    }
}
