import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomsDocumentsTicketPM} from '../../EntityPMs/CustomsDocumentsTicketPM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustomsDocumentsTicketsExtendedService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsDocumentsTicketsExtended';
    }

    delete(Id: string) {
    
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: CustomsDocumentsTicketPM;
            // mappedEntity = this.MapJsonToEntityPM(entityPM, false);

            return this._http.delete(this._apiUrl + '/Delete/?' + 'Id=' + Id, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var pm = response;
                if (pm) {
                    var mappedResult: CustomsDocumentsTicketPM;
                    //   mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                    serviceResponse.Result = mappedResult;
                }


                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CustomsDocumentsTicketPM;
        entityPM = new CustomsDocumentsTicketPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }



}