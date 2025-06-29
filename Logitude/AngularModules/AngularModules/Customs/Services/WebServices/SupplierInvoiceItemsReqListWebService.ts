import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
@Injectable()

export class SupplierInvoiceItemsReqListWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SupplierInvoiceItemsReqListExtended';
    }

    GetProductFileExists(modelCode: string, importerNumber: string, originCountry: string, declarationId : string) {
        return defer(() => {
            let authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            let serviceResponse: ServiceResponse = new ServiceResponse();
            return this._http.get(this._apiUrl + "/GetProductFileExists/?modelCode=" + modelCode + "&importerNumber=" + importerNumber + "&originCountry=" + originCountry + "&declarationId="+declarationId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    getBySiiRequest(declarationId: string, lineNumber: number, invoiceCounterKey: number, invoiceItemLineNumber: number, siiRequestId: string) {
        return defer(() => {
            let authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            let serviceResponse: ServiceResponse = new ServiceResponse();
            return this._http.get(this._apiUrl + "/GetSingle/?declarationid=" + declarationId + "&linenumber=" + lineNumber + "&invoicecounterkey=" + invoiceCounterKey + "&invoiceitemlinenumber=" + invoiceItemLineNumber + "&siirequestid=" + siiRequestId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
        );
    }
}

