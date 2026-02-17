import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CourierPendingReasonPM } from '../../EntityPMs/CourierPendingReasonPM';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CourierPendingReasonExtendedListService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CourierPendingReason';
    }

    GetCourierPendingReasonByUnifreightStatus(unifreightStatusCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCourierPendingReasonByUnifreightStatus/?' + 'unifreightStatusCode=' + unifreightStatusCode, { headers: authHeader }).map(response => {
                var serviceResponse: ServiceResponse = response.json();
                var _mappedListsArray: Array<CourierPendingReasonPM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CourierPendingReasonPM;
                        entity = this.MapJsonToEntityPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    DeleteCourierPendingReasonUnifreightStatus(courierPendingReasonCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.delete(this._apiUrl + '/DeleteCourierPendingReasonUnifreightStatus/?' + 'courierPendingReasonCode=' + courierPendingReasonCode, { headers: authHeader }).map(response => {
                var serviceResponse: ServiceResponse = response.json();
                var _mappedListsArray: Array<CourierPendingReasonPM> = [];
                if (serviceResponse.Result) {
                    for (var key in serviceResponse.Result) {

                        var entity: CourierPendingReasonPM;
                        entity = this.MapJsonToEntityPM(serviceResponse.Result[key]);
                        _mappedListsArray.push(entity);
                    }
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CourierPendingReasonPM;
        entityPM = new CourierPendingReasonPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }

        return entityPM;
    }

}
