import { EntityPMServiceResponse } from '../../DataContracts/EntityPMServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { PerformanceLog } from '../../Others/PerformanceLog';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class PerformanceLogService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/performancelogs';
    }

    insert(entity: PerformanceLog) {
        return defer(() => {
            var errorsArray = [];
            var entityServiceResponse: EntityPMServiceResponse;
            entityServiceResponse = new EntityPMServiceResponse();

            if (errorsArray.length == 0) {
                return this._http.post(this._apiUrl, JSON.stringify(entity), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                    var result = response;
                    entityServiceResponse.Result = result;

                    return entityServiceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
            }
            else {
                entityServiceResponse.HasError = true;
                entityServiceResponse.ErrorsArray = errorsArray;

                return of(entityServiceResponse);
            }
        });
    }

    insertLogsList(logs: PerformanceLog[]) {
        return defer(() => {
            var errorsArray = [];
            var entityServiceResponse: EntityPMServiceResponse;
            entityServiceResponse = new EntityPMServiceResponse();

            if (errorsArray.length == 0) {
                var url = this._apiUrl + '/PostLogsList';

                return this._http.post(url, JSON.stringify(logs), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                    var result = response;
                    entityServiceResponse.Result = result;

                    return entityServiceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
            }
            else {
                entityServiceResponse.HasError = true;
                entityServiceResponse.ErrorsArray = errorsArray;

                return of(entityServiceResponse);
            }
        });
    }
}
