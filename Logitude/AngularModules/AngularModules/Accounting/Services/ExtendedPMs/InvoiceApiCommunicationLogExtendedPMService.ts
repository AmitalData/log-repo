import { Injectable } from '@angular/core';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators'


@Injectable()

export class InvoiceApiCommunicationLogExtendedPMService {

    private _apiUrl: string;
    private httpClient: HttpClient;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InvoiceApiCommunicationLog';
    }

    ReSendCommunication(id: string) {

        return this.httpClient.post(this._apiUrl + '/ReSendCommunication?id=' + id,null, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var result = response;
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;
            }),
            catchError(ServiceHelper.HandleServiceError));


    }

}
