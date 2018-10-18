
import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ClientPM } from '../../EntityPMs/ClientPM';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class ClientExtendedPMService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/Client';
    }

    GetSingleClientPMByCode(code: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleClientPMByCode?code=' + code + '&tenant=' + tenant.toString(), { headers: authHeader }).map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
               // serviceResponse.Result = response.json();

                var pm = response.json();

                var entity: ClientPM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });

    }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: ClientPM;
        entityPM = new ClientPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }




}