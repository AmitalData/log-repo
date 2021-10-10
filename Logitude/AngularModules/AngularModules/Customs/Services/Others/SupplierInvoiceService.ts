import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SupplierInvoicePM} from '../../EntityPMs/SupplierInvoicePM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import { SendMultiUpdateRequestParams } from '../../DataContract/RequestParams/SendMultiUpdateRequestParams';

@Injectable()

export class SupplierInvoiceService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationSupplierInvoices';
    }

    GetTotalForeignCurrencyForInvoice(declarationId: string, invoiceCounterKey: number ) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);


        return defer(() => {
            return this._http.get(this._apiUrl + '/GetTotalForeignCurrencyForInvoice?' + 'declarationId=' + declarationId + '&invoiceCounterKey=' + invoiceCounterKey, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));


        });


    }

    GetCheckIfInvoiceNumberExists(declarationId: string, invoiceNumber: string, invoiceCounterKey: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);


        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCheckIfInvoiceNumberExists?' + 'declarationId=' + declarationId + '&invoiceNumber=' + invoiceNumber+ '&invoiceCounterKey=' + invoiceCounterKey, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));


        });


    }

    PostSendMultiUpdate(requestParams: SendMultiUpdateRequestParams) {

        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.post(
                this._apiUrl + '/PostSendMultiUpdate/', JSON.stringify(requestParams), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                    var messString = res;
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = messString;

                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
            ;

        });
    }
}
