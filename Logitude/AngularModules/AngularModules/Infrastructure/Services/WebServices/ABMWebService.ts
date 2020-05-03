import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class ABMWebService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ABMWebService';
    }

    Send(myShipmentId: string) {
        var url = this._apiUrl + '/GetMessageResult?myShipmentId=' + myShipmentId;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var mappedResult: ABMResult = new ABMResult();

                if (myJsonResult) {
                    var jsonListKeys = Object.keys(myJsonResult);
                    for (var key in jsonListKeys) {
                        var property = jsonListKeys[key];
                        mappedResult[property] = myJsonResult[property];
                    }
                }

                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}

export class ABMResult {
    public Id: string;
    public IsValid: boolean;
}
