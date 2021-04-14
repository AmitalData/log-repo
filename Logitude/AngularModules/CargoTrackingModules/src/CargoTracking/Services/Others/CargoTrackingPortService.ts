import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';


@Injectable()

export class CargoTrackingPortService {
    private _apiUrl: string;
    constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/CargoTrackingPorts';
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
}

