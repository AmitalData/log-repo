
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { InterfaceManagementPM } from '../../EntityPMs/InterfaceManagementPM';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { DeclarationPM } from '../../EntityPMs/DeclarationPM';
@Injectable()

export class InterfaceManagementPMExtendService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InterfaceManagementPMExtend';
    }



    GetSingleInterfaceManagementwithDefinition(code: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleInterfaceManagementwithDefinition?code=' + code + '&tenant=' + tenant.toString(), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                // serviceResponse.Result = response;

                var pm = response;

                var entity: InterfaceManagementPM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;
                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });

    }
    PutInterfaceManagementPM(mappedEntity: InterfaceManagementPM) {
        //CancellRequestInProgress(Id: string, Tenant: number) {


        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            //var mappedEntity: InterfaceManagementPM;
            // mappedEntity = this.MapJsonToEntityPM(entityPM, false);

            return defer(() => {
                return this._http
                    .put(
                    this._apiUrl + '/PutInterfaceManagementPM/',
                    JSON.stringify(mappedEntity),
                    ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        var pm = response;
                        serviceResponse.Result = pm;

                        return serviceResponse;
                    })
                    ,catchError(ServiceHelper.HandleServiceError));
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