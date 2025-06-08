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

    CreateTraceEvent(tenant: number, entityId: string, tableName: string, eventTypeCode: string, loggedUserEmail: string = '', notes: string = '') {
        return defer(() => {
            return this._http.post(
                this._apiUrl + '/PostTraceEvent', 
                {tenant, entityId, tableName, eventTypeCode, loggedUserEmail, notes}, 
                ServiceHelper.GetHttpHeaders()
            ).pipe(map(() => new ServiceResponse()), catchError(ServiceHelper.HandleServiceError));
        });
    }


    GetLatestTraceEventByEventCode(entityId: string, eventCode: string) {
        
        var url = `${this._apiUrl}/GetLatestTraceEventByEventCode?entityId=${entityId}&eventCode=${eventCode}`;

        return defer(() => {
            let serviceResponse = new ServiceResponse();

            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}

export enum TraceEventTypeCodes { CREATE = 'CREV', UPDATE = 'UPEV' }