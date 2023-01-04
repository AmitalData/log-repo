import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DashboardPM } from '../../../DashboardModule/EntityPMs/DashboardPM';

@Injectable()

export class DashboardListExtendedService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DashboardListExtended';
    }

    GetDashboardsForDropDown() {
        var url = this._apiUrl + '/GetDashboardsForDropDown';

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myResult = response;
                var myResponse: ServiceResponse = new ServiceResponse();
                myResponse.Result = myResult;
                return myResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}
