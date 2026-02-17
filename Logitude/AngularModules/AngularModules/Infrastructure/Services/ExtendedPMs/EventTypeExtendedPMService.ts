
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Observable';
import {ServiceArgs} from '../../DataContracts/ServiceArgs';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../Validators/ClassLevelValidator';
import {Guid} from '../../Utilities/Guid';
import {InfraSettings} from '../../Utilities/InfraSettings';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {EventTypePM} from '../../EntityPMs/EventTypePM';






@Injectable()
export class EventTypeExtendedPMService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/EventTypeExtended';
    }





    GetEventTypeByCode(code: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl + '/geteventtypebycode/?' + 'code=' + code + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
            var entity: EventTypePM;
            entity = this.MapJsonToEntityPM(result);
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = entity;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }






    GetEventTypesByObjectTable(objectTableId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())

        return this._http.get(this._apiUrl +'/geteventtypesbyobjecttable/?' +  'objectTableId=' + objectTableId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
            var entity: EventTypePM;
            var eventTypePMLists: EventTypePM[];
            eventTypePMLists = new Array<EventTypePM>();

            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                eventTypePMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = eventTypePMLists;
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }



    update(eventTypePMLists: any) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
            authHeader.append('Content-Type', 'application/json');
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
 
            return this._http.put(this._apiUrl, JSON.stringify(eventTypePMLists),
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        return serviceResponse;
                    }).catch(ServiceHelper.HandleServiceError);
        }
        );

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
