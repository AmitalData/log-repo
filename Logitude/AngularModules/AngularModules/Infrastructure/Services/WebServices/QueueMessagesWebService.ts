import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';

export class QueueMessagesWebService {
    private _apiUrl: string;
    private _http: Http;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/QueueMessagesWebService';
    }

    UpdateTenantManagementStatistics(tenantId: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetUpdateTenantManagementStatistics?tenantId=' + tenantId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {

                var myJsonResult = response.json();
                                
                //var serviceResponse: ServiceResponse;
                //serviceResponse = new ServiceResponse();
                //serviceResponse.Result = myJsonResult;
                //return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
}