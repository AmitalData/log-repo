import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../DataContracts/ApiQueryFilters';

export class InboundEmailWebService {
    private _apiUrl: string;
    private _http: Http;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InboundEmailWebService';
    }

    SendInboundEmailAsync(Recepient: string, Tenant: number, Subject: string, Body:string, entityId: string){
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        var url = this._apiUrl + '/GetMessageResult?recepient=' + Recepient + '&tenant=' + Tenant + '&subject=' + Subject + '&body=' + Body + '&entityId=' + entityId;

        return Observable.defer(() => {
            return this._http.get(url, { headers: authHeader }).map(response => {
                var mappedResult = response.json();
                //if (myJsonResult) {
                //    var jsonListKeys = Object.keys(myJsonResult);
                //    for (var key in jsonListKeys) {
                //        var property = jsonListKeys[key];
                //        mappedResult[property] = myJsonResult[property];
                //    }
                //}
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
}

export class InboundEmailResult {
    public Id: string;
    public IsValid: boolean;
}