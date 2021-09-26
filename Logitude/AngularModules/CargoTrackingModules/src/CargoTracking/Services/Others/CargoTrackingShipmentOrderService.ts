import { Inject, Injectable } from '@angular/core';
import { HttpClient, JsonpClientBackend } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';


@Injectable()

export class CargoTrackingShipmentOrderService {
    private _apiUrl: string;
    public authHeaders = ServiceHelper.GetHeadersWithToken();

    constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/ShipmentOrders';
    }

    get(id: string) {
        return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, this.authHeaders).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }

}

