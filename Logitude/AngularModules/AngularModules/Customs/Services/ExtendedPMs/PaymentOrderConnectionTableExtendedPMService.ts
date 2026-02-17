import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CustomsHouseTypePM } from '../../EntityPMs/CustomsHouseTypePM';

import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class PaymentOrderConnectionTableExtendedPMService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PaymentOrderConnectionTable';
    }

    GetAccountingCustomFileNumbers(paymentOrderId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetAccountingCustomFileNumbers?paymentOrderId=' + paymentOrderId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();

                var _mappedListsArray: Array<string> = [];
                if (serviceResponse.Result) {
                    _mappedListsArray = this.MapJsonToEntityPM(serviceResponse.Result);
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });

    }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: string[] = [];
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[key] = jsonPM[property];
        }

        return entityPM;
    }



}