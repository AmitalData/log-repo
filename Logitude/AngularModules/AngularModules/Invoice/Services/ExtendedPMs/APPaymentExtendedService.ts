import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse'; 
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { defer, Observable } from 'rxjs';
import { ARInvoiceList } from 'Invoice/EntityLists/ARInvoiceList';
import { APPaymentList } from 'Invoice/EntityLists/APPaymentList';

@Injectable()
export class APPaymentExtendedService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/APPaymentExtended';
    }

    updateMulti(apPaymentsList: APPaymentList[]): Observable<ServiceResponse> {
        return this._http.put(this._apiUrl + "/UpdateMulti",apPaymentsList, ServiceHelper.GetHttpHeaders()).pipe(
            map(res => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                var result = res;
                serviceResponse.Result = result;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    } 
}
 