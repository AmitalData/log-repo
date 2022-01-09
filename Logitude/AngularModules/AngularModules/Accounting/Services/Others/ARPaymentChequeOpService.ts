import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';

@Injectable()

export class ARPaymentChequeOperationsService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ARPaymentChequeOp';
    }


    ReturnChequeToCustomer(tenant: number, chequeId: string, paymentId: string)
    {
        var url = this._apiUrl + '/PostReturnChequeToCustomer'+
        `?Tenant=${tenant}&ChequeId=${chequeId}&PaymentId=${paymentId}`;
        return defer(() => {
            return this._http
                .post(url,{}, ServiceHelper.GetHttpHeaders())
                .pipe(map(response => {

                var result = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

}
