import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ExceptionReasonPM } from '../../EntityPMs/ExceptionReasonPM';

@Injectable()

export class ExceptionReasonExtendedListService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExceptionReason';
    }

    GetExceptionReasonByUnifreightStatus(unifreightStatusCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetExceptionReasonByUnifreightStatus/?' + 'unifreightStatusCode=' + unifreightStatusCode, ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {
                var serviceResponse: ServiceResponse = response;
                var _mappedListsArray: Array<ExceptionReasonPM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: ExceptionReasonPM;
                        entity = this.MapJsonToEntityPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    DeleteExceptionReasonByUnifreightStatus(exceptionReasonCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.delete(this._apiUrl + '/DeleteExceptionReasonByUnifreightStatus/?' + 'courierPendingReasonList=' + exceptionReasonCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: ExceptionReasonPM;
        entityPM = new ExceptionReasonPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        return entityPM;
    }

}
