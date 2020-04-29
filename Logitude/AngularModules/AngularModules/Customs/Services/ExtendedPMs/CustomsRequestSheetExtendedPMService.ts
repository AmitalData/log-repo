
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CustomsRequestsSheetPM } from '../../EntityPMs/CustomsRequestsSheetPM';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { DeclarationPM } from '../../EntityPMs/DeclarationPM';
@Injectable()

export class CustomsRequestSheetExtendedPMService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsRequestSheetExtended';
    }

  
    PostSetCustomsRequestSheetStatus(mappedEntity: CustomsRequestsSheetPM){
    //CancellRequestInProgress(Id: string, Tenant: number) {


        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            //var mappedEntity: CustomsRequestsSheetPM;
            // mappedEntity = this.MapJsonToEntityPM(entityPM, false);

            return defer(() => {
                return this._http
                    .post(
                    this._apiUrl + '/PostSetCustomsRequestSheetStatus/',
                    JSON.stringify(mappedEntity),
                    ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    var requestSheets = response;
                    serviceResponse.Result = requestSheets;
                    
                    return serviceResponse;
                    })
                    ,catchError(ServiceHelper.HandleServiceError));
            }

            );

        }

        );

    }

    
    PostCustomsRequestSheetReQueue(mappedEntity: CustomsRequestsSheetPM) {
        //CancellRequestInProgress(Id: string, Tenant: number) {


        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            //var mappedEntity: CustomsRequestsSheetPM;
            // mappedEntity = this.MapJsonToEntityPM(entityPM, false);

            return defer(() => {
                return this._http
                    .post(
                    this._apiUrl + '/PostCustomsRequestSheetReQueue/',
                    JSON.stringify(mappedEntity),
                    ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        var requestSheets = response;
                        serviceResponse.Result = requestSheets;

                        return serviceResponse;
                    })
                    ,catchError(ServiceHelper.HandleServiceError));
            }

            );

        }

        );

    }
    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CustomsRequestsSheetPM;
        entityPM = new CustomsRequestsSheetPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }



}