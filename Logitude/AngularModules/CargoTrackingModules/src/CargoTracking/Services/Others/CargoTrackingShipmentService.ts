import { Inject, Injectable } from '@angular/core';
import { HttpClient, JsonpClientBackend} from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';


@Injectable()

export class CargoTrackingShipmentService {
    private _apiUrl: string;
    constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/ShipmentCargoTracking';
    }

    get(id: string) {
        var authHeaders = ServiceHelper.GetHeadersWithToken();

        return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, authHeaders).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }

    GetShipmentPackages(id: string) {
        var authHeaders = ServiceHelper.GetHeadersWithToken();

        return this._http.get(this._apiUrl + '/GetShipmentPackages?' + 'id=' + id, authHeaders).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }

    GetPartnersAddresses(partnersIds: string[]) {
        var authHeaders = ServiceHelper.GetHeadersWithToken();

        var IdsParameterString = "";
        if (partnersIds && partnersIds.length > 0) {
            partnersIds.forEach(el => {
                IdsParameterString += 'partnersIds=' + el + '&';
            });
        }


        return this._http.get(this._apiUrl + '/GetPartnersAddresses?' + IdsParameterString, authHeaders).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }

    GetShipmentCustomsData(shipmentId: string) {
        var authHeaders = ServiceHelper.GetHeadersWithToken();

        return this._http.get(`${this._apiUrl}/GetShipmentCustomsData?shipmentId=${shipmentId}`, authHeaders).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }
}

