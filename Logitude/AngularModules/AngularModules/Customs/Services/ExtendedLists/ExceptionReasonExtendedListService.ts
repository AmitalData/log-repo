import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ExceptionReasonPM } from '../../EntityPMs/ExceptionReasonPM';

@Injectable()

export class ExceptionReasonExtendedListService {
    private _http: Http
    private _apiUrl: string;
    private _mainApiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExceptionReason';
        this._mainApiUrl = ServiceHelper.GetLogitudeURL() + 'api/exceptionreasonviews';  

    }

    GetExceptionReasonByUnifreightStatus(unifreightStatusCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetExceptionReasonByUnifreightStatus/?' + 'unifreightStatusCode=' + unifreightStatusCode, { headers: authHeader }).map(response => {
                var serviceResponse: ServiceResponse = response.json();
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
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    DeleteExceptionReasonByUnifreightStatus(exceptionReasonCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.delete(this._apiUrl + '/DeleteExceptionReasonByUnifreightStatus/?' + 'ExceptionReasonCode=' + exceptionReasonCode, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
    get(exceptionReasonCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._mainApiUrl + '/getsingle?' + 'code=' + exceptionReasonCode, {
                headers: authHeader
            }).map(response => {
                var myJsonResult = response.json();
                var entityPM = new ExceptionReasonPM();
                entityPM = this.MapJsonToEntityPM(myJsonResult);
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = entityPM;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
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
