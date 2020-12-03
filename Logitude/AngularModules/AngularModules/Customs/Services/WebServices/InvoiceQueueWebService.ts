import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
 import { map, catchError } from 'rxjs/operators';


@Injectable()

export class InvoiceQueueWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/Urouter';
    }


    GetInvoice(tenant: number, customFileNo: string) {

        var callTime = new Date();

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetInvoice?' + 'tenant=' + tenant + '&customFileNo=' + customFileNo,  ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = response.body;
                        return serviceResponse;
 
                    }),

                    catchError(ServiceHelper.HandleServiceError));
        });
    }

 

  
 }
