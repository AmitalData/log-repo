import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { EventTypePM } from '../../EntityPMs/EventTypePM';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class EventTypeExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/EventTypeExtended';
    }

    GetEventTypeByCode(code: string, tenant: number) {
        var url = this._apiUrl + '/geteventtypebycode/?' + 'code=' + code + '&tenant=' + tenant;
        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result = response;
            var entity: EventTypePM;
            entity = this.MapJsonToEntityPM(result);
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = entity;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetEventTypesByObjectTable(objectTableId: string, tenant: number) {
        var url = this._apiUrl + '/geteventtypesbyobjecttable/?' + 'objectTableId=' + objectTableId + '&tenant=' + tenant;

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result: any = response;
            var entity: EventTypePM;
            var eventTypePMLists: EventTypePM[];
            eventTypePMLists = new Array<EventTypePM>();

            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                eventTypePMLists.push(entity);
            });
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            serviceResponse.Result = eventTypePMLists;

            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    update(eventTypePMLists: any) {
        return defer(() => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl, JSON.stringify(eventTypePMLists), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                //var pm = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityPM(jsonPM: any) {
        var entityPM: EventTypePM;
        entityPM = new EventTypePM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;

        return entityPM;
    }
}
