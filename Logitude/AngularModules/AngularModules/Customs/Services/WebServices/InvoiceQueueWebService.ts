import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from '../../EntityLists/DeclarationList';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CLAIM_2340_ClaimRequestRequestParams } from '../../DataContract/RequestParams/CLAIM_2340_ClaimRequestRequestParams';
import { ContinuousRequestOnClaimFileRequestParams } from '../../DataContract/RequestParams/ContinuousRequestOnClaimFileRequestParams';
import { map, catchError } from 'rxjs/operators';


@Injectable()

export class InvoiceQueueWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/Urouter';
    }


    GetInvoice(id: string) {

        var callTime = new Date();

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetInvoice?' , ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = response;
                        return serviceResponse;
 
                    }),

                    catchError(ServiceHelper.HandleServiceError));
        });
    }

 

  
 }
