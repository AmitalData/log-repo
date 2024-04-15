import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class TraceEventExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TraceEventExtended';
    }

    PutTraceEventGroup(eventTypeArgs: any) {
        var url = this._apiUrl + '/puttraceeventgroup';

        return defer(() => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(url, JSON.stringify(eventTypeArgs), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                //var pm = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}
