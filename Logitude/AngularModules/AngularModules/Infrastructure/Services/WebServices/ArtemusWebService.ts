import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class ArtemusWebService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ArtemusWebService';
    }

    SendAMS_Voyage(shipmentId: string) {
        var url = this._apiUrl + '/GetSendToArtemusVoyageXML?shipmentId=' + shipmentId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;

                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    SendAMS_Bill(shipmentId: string) {
        var url = this._apiUrl + '/GetSendToArtemusBillXML?shipmentId=' + shipmentId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;

                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}
