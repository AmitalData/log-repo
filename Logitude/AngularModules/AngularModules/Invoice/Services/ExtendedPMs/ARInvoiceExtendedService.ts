import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse'; 
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { defer } from 'rxjs';

@Injectable()
export class ARInvoiceExtendedService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ARInvoiceExtended';
    }

    getInvoiceSequenceStatus(fromDate: Date, toDate: Date) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            return this._http.get(this._apiUrl + '/GetInvoiceSequenceStatus/?' + '&fromDate=' + fromDate.toISOString() + '&toDate=' + toDate.toISOString(), ServiceHelper.GetHttpHeaders())
                .pipe(map((response: any) => {
                    var serviceResponse = new ServiceResponse();
                    serviceResponse.Result = response;
                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
        });
    }

 
}
