import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse'; 
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { defer } from 'rxjs';

@Injectable()
export class ConfirmationNumberDefaultExtendedService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ConfirmationNumberDefaultExtended';
    }

    getAmountForConfirmationNumber(invoiceDate: Date) {
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetAmountForConfirmationNumber/?' + '&invoiceDateStr=' + invoiceDate.toISOString() , ServiceHelper.GetHttpHeaders())
                .pipe(map((response: any) => {
                    var serviceResponse: ServiceResponse;
                    serviceResponse = response;
                    if (serviceResponse.Result) {
                        serviceResponse.Result = Number(serviceResponse.Result);
                    }                       
                     
                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    

   
}
 