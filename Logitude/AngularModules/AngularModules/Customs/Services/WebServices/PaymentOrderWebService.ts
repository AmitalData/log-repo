import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { GenericRequestParams } from '../../DataContract/RequestParams/GenericRequestParams';
import { PaymentOrderList } from '../../EntityLists/PaymentOrderList';


@Injectable()

export class PaymentOrderWebService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/PaymentOrderWebService';
    }

    PostSendPaymentOrderRequest(entity: GenericRequestParams) {

        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendPaymentOrderRequest/',
                JSON.stringify(entity),
                { headers: authHeader }).map((res) => {

                    serviceResponse.Result = res.json();

                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }

    GetPaymentOrderByPaymentOrderConnection(connectedEntityCode: string, connectedEntityId: string, tenant: number) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetPaymentOrderByPaymentOrderConnection/?ConnectedEntityCode=" + connectedEntityCode + "&connectedEntityId=" + connectedEntityId + "&tenant=" + tenant, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                var _mappedListsArray: Array<PaymentOrderList> = [];
                if (allLists) {
                    for (var key in allLists) {

                        var entity: PaymentOrderList;
                        entity = this.MapJsonToEntityPM(allLists[key]);
                        _mappedListsArray.push(entity);

                    }
                }

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        }
        );
    }

    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: PaymentOrderList;
        entityPM = new PaymentOrderList();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        return entityPM;
    }

}