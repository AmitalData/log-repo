import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CustomsPartnerFtpPM } from '../../EntityPMs/CustomsPartnerFtpPM';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustomsPartnerFtpExtendedPMService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsPartnerFtpExtended';
    }

    //getAll(tenant: number) {
    //    var authHeader = new Headers();
    //    authHeader.append('Token', SessionInfo.Token);

    //    return defer(() => {
    //        return this._http.get(this._apiUrl + '/getAll?tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {


    //            var pmList :any[]= response;

    //            var entityList: CustomsPartnerFtpPM[];
    //            if (pmList) {
    //                pmList.forEach(pm => {
    //                    entityList.push(this.MapJsonToEntityPM(pm));
    //                })
                    
    //            }

    //            var serviceResponse: ServiceResponse;
    //            serviceResponse = new ServiceResponse();
    //            serviceResponse.Result = entityList;
    //            return serviceResponse;

    //        }),catchError(ServiceHelper.HandleServiceError));
    //    });

    //}
    delete(Id: string) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: CustomsPartnerFtpPM;
            return this._http.delete(this._apiUrl + '/Delete/?' + 'Id=' + Id, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var pm = response;
                if (pm) {
                    var mappedResult: CustomsPartnerFtpPM;
                    serviceResponse.Result = mappedResult;
                }

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }
    GetScreenOption(tenant: number) {

        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: CustomsPartnerFtpPM;
            return this._http.get(this._apiUrl + '/GetScreenOption/?' + 'tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var ScreenOption = response;
                if (ScreenOption) {
                    //var mappedResult: CustomsPartnerFtpPM;
                    serviceResponse.Result = ScreenOption;
                }

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CustomsPartnerFtpPM;
        entityPM = new CustomsPartnerFtpPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }



}
