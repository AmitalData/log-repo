import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import { QuoteDashboardArguments } from '../../DataContracts/QuoteDashboardArguments';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

export class DashboardService {

    private _http: Http;
    private _apiUrl: string;

    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QuoteDashboard';
    }

    GetDashboardChartValues(entity: QuoteDashboardArguments) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            
            return this._http.post(this._apiUrl, JSON.stringify(entity),
                    { headers: authHeader }).map((response) => {
                        var allLists = response.json();
                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = allLists;
                        return serviceResponse.Result;
                    }).catch(ServiceHelper.HandleServiceError);
        });
    }
}
