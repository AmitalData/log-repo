import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SupplierInvoicePM} from '../../EntityPMs/SupplierInvoicePM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class SupplierInvoiceService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationSupplierInvoices';
    }

    GetTotalForeignCurrencyForInvoice(declarationId: string, invoiceCounterKey: number ) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);


        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTotalForeignCurrencyForInvoice?' + 'declarationId=' + declarationId + '&invoiceCounterKey=' + invoiceCounterKey, { headers: authHeader }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);


        });


    }

    GetCheckIfInvoiceNumberExists(declarationId: string, invoiceNumber: string, invoiceCounterKey: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);


        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCheckIfInvoiceNumberExists?' + 'declarationId=' + declarationId + '&invoiceNumber=' + invoiceNumber+ '&invoiceCounterKey=' + invoiceCounterKey, { headers: authHeader }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);


        });


    }
}