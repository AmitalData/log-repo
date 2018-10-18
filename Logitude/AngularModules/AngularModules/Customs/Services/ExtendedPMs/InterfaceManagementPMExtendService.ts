
import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { InterfaceManagementPM } from '../../EntityPMs/InterfaceManagementPM';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { DeclarationPM } from '../../EntityPMs/DeclarationPM';
@Injectable()

export class InterfaceManagementPMExtendService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InterfaceManagementPMExtend';
    }



    GetSingleInterfaceManagementwithDefinition(code: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleInterfaceManagementwithDefinition?code=' + code + '&tenant=' + tenant.toString(), { headers: authHeader }).map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                // serviceResponse.Result = response.json();

                var pm = response.json();

                var entity: InterfaceManagementPM;
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
    PutInterfaceManagementPM(mappedEntity: InterfaceManagementPM) {
        //CancellRequestInProgress(Id: string, Tenant: number) {


        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            //var mappedEntity: InterfaceManagementPM;
            // mappedEntity = this.MapJsonToEntityPM(entityPM, false);

            return Observable.defer(() => {
                return this._http
                    .put(
                    this._apiUrl + '/PutInterfaceManagementPM/',
                    JSON.stringify(mappedEntity),
                    { headers: authHeader }
                    )
                    .map(response => {
                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        var pm = response.json();
                        serviceResponse.Result = pm;

                        return serviceResponse;
                    })
                    .catch(ServiceHelper.HandleServiceError);
            }

            );

        }

        );

    }



    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: InterfaceManagementPM;
        entityPM = new InterfaceManagementPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }



}