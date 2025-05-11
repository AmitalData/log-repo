import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { SIIRequestPM } from 'Customs/EntityPMs/SIIRequestPM';
@Injectable()

export class SIIRequestWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SIIRequestExtended';
    }

    getByDeclarationId(declarationId: string,id: string) {
        return defer(() => {
            let authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            let serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            return this._http.get(this._apiUrl + "/GetSingle/?declarationId=" + declarationId + "&id=" + id , ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                let serviceResponse: ServiceResponse = new ServiceResponse();
                let mappedResult: SIIRequestPM = new SIIRequestPM();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    getSupplierInvoiceItemsForSIIRequest(declarationId: string) {
        return defer(() => {
            let authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            let serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            return this._http.get(this._apiUrl + "/GetSupplierInvoiceItemsForSIIRequest/?declarationId=" + declarationId , ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                let serviceResponse: ServiceResponse = new ServiceResponse();
                let mappedResult: SIIRequestPM = new SIIRequestPM();
                serviceResponse.Result = mappedResult;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
        );
    }
}
