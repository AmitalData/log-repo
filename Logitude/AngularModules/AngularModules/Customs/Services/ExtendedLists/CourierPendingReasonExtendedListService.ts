import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CourierPendingReasonPM } from '../../EntityPMs/CourierPendingReasonPM';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CourierPendingReasonExtendedListService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CourierPendingReason';
    }

    GetCourierPendingReasonByUnifreightStatus(unifreightStatusCode: string) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCourierPendingReasonByUnifreightStatus/?' + 'unifreightStatusCode=' + unifreightStatusCode, ServiceHelper.GetHttpHeaders()).pipe(map((response:any) => {
                var serviceResponse: ServiceResponse = response;
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
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    DeleteCourierPendingReasonUnifreightStatus(courierPendingReasonList: string) {

        return defer(() => {
            return this._http.delete(this._apiUrl + '/DeleteCourierPendingReasonUnifreightStatus/?' + 'courierPendingReasonCode=' + courierPendingReasonList, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var myJsonResult = response;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
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
