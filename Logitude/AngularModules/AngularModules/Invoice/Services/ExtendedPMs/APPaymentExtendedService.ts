import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse'; 
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { defer, Observable } from 'rxjs';
import { ARInvoiceList } from 'Invoice/EntityLists/ARInvoiceList';

@Injectable()
export class APPaymentExtendedService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/APPaymentExtended';
    }

    updateMulti(ids: string[],masavInterfaceId: string): Observable<ServiceResponse> {
        return this._http.put(this._apiUrl + "/UpdateMulti", 
        {
            ids: ids,
            masavInterfaceId: masavInterfaceId,            
        }
        , ServiceHelper.GetHttpHeaders()).pipe(
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
 