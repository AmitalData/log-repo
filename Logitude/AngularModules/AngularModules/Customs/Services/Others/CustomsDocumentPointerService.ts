import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SupplierInvoicePM} from '../../EntityPMs/SupplierInvoicePM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustomsDocumentPointerService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsDocumentPointer';
    }

    GetCheckForPointers(declarationId: string, counterKey: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

      //  var url = this._apiUrl + '/CheckForPointers';

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCheckForPointers?' + 'declarationId=' + declarationId + '&invoiceCounterKey=' + counterKey, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));

         
        });

   
}
}