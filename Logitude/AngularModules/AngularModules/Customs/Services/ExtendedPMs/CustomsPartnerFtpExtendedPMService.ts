import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CustomsPartnerFtpPM } from '../../EntityPMs/CustomsPartnerFtpPM';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustomsPartnerFtpExtendedPMService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsPartnerFtpExtended';
    }

    //getAll(tenant: number) {
    //    var authHeader = new Headers();
    //    authHeader.append('Token', SessionInfo.Token);

    //    return Observable.defer(() => {
    //        return this._http.get(this._apiUrl + '/getAll?tenant=' + tenant, { headers: authHeader }).map(response => {


    //            var pmList :any[]= response.json();

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

    //        }).catch(ServiceHelper.HandleServiceError);
    //    });

    //}
    delete(Id: string) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: CustomsPartnerFtpPM;
            return this._http.delete(this._apiUrl + '/Delete/?' + 'Id=' + Id, { headers: authHeader }).map(response => {

                var pm = response.json();
                if (pm) {
                    var mappedResult: CustomsPartnerFtpPM;
                    serviceResponse.Result = mappedResult;
                }

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }
    GetScreenOption(tenant: number) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            var mappedEntity: CustomsPartnerFtpPM;
            return this._http.get(this._apiUrl + '/GetScreenOption/?' + 'tenant=' + tenant, { headers: authHeader }).map(response => {

                var ScreenOption = response.json();
                if (ScreenOption) {
                    //var mappedResult: CustomsPartnerFtpPM;
                    serviceResponse.Result = ScreenOption;
                }

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
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
