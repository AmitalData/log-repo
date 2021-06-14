import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse'; 
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';

@Injectable()
export class ARInvoiceExtendedService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ARInvoice';
    }

    getSingleByInvoiceNumber(number: number) {
        var url = this._apiUrl + '?number=' + number + '&id=';

        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }
}
