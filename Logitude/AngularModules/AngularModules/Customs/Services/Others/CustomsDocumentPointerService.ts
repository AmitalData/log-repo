import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SupplierInvoicePM} from '../../EntityPMs/SupplierInvoicePM';

import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

@Injectable()

export class CustomsDocumentPointerService {
    private _http: Http
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CustomsDocumentPointer';
    }

    GetCheckForPointers(declarationId: string, counterKey: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);

      //  var url = this._apiUrl + '/CheckForPointers';

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCheckForPointers?' + 'declarationId=' + declarationId + '&invoiceCounterKey=' + counterKey, { headers: authHeader }).map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.json();
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);

         
        });

   
}
}