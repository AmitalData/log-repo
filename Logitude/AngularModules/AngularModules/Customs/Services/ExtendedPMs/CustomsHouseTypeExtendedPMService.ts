import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CustomsHouseTypePM} from '../../EntityPMs/CustomsHouseTypePM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustomsHouseTypeExtendedPMService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsHouseType';
    }

    GetHouseTypewithAdditional(declarationOfficeCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetHouseTypewithAdditional?declarationOfficeCode=' + declarationOfficeCode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


                var pm = response;

                var entity: CustomsHouseTypePM;
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

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CustomsHouseTypePM;
        entityPM = new CustomsHouseTypePM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }



}
