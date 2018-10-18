
import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CustomsRequestsSheetPM } from '../../EntityPMs/CustomsRequestsSheetPM';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { DeclarationPM } from '../../EntityPMs/DeclarationPM';
@Injectable()

export class CustomsRequestSheetExtendedPMService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsRequestSheetExtended';
    }

  
    PostSetCustomsRequestSheetStatus(mappedEntity: CustomsRequestsSheetPM){
    //CancellRequestInProgress(Id: string, Tenant: number) {


        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            //var mappedEntity: CustomsRequestsSheetPM;
            // mappedEntity = this.MapJsonToEntityPM(entityPM, false);

            return Observable.defer(() => {
                return this._http
                    .post(
                    this._apiUrl + '/PostSetCustomsRequestSheetStatus/',
                    JSON.stringify(mappedEntity),
                    { headers: authHeader }
                    )
                    .map(response => {
                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    var requestSheets = response.json();
                    serviceResponse.Result = requestSheets;
                    
                    return serviceResponse;
                    })
                    .catch(ServiceHelper.HandleServiceError);
            }

            );

        }

        );

    }

    
    PostCustomsRequestSheetReQueue(mappedEntity: CustomsRequestsSheetPM) {
        //CancellRequestInProgress(Id: string, Tenant: number) {


        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            //var mappedEntity: CustomsRequestsSheetPM;
            // mappedEntity = this.MapJsonToEntityPM(entityPM, false);

            return Observable.defer(() => {
                return this._http
                    .post(
                    this._apiUrl + '/PostCustomsRequestSheetReQueue/',
                    JSON.stringify(mappedEntity),
                    { headers: authHeader }
                    )
                    .map(response => {
                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        var requestSheets = response.json();
                        serviceResponse.Result = requestSheets;

                        return serviceResponse;
                    })
                    .catch(ServiceHelper.HandleServiceError);
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