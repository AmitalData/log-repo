import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, throwError } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
@Injectable()

export class SIIRequestWebService {
    private _http: HttpClient
    private _apiUrl: string;
    private _apiUrlUser: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SIIRequestExtended';
        this._apiUrlUser = ServiceHelper.GetLogitudeURL() + 'api/Users';
    }


    getByDeclarationId(declarationId: string, id: string) {
        return defer(() => {
            let authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            let serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            return this._http.get(this._apiUrl + "/GetSingle/?declarationId=" + declarationId + "&id=" + id, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                let serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    getSupplierInvoiceItemsForSIIRequest(declarationId: string, siiRequestId: string) {
        return defer(() => {
            let authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            let serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            return this._http.get(this._apiUrl + "/GetSupplierInvoiceItemsForSIIRequest/?declarationId=" + declarationId + "&siiRequestId=" + siiRequestId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                let serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
        );
    }

    postSendSIIRequest(siiRequestId: string, declarationId: string, tenant: number, selectedRows: any[]) {
        return defer(() => {
            let headers = new Headers();
            headers.append('Token', SessionInfo.Token);
            headers.append('Content-Type', 'application/json');

            return this._http.post(
                this._apiUrl + "/PostSendSIIRequest?siiRequestId=" + siiRequestId + "&declarationId=" + declarationId + "&tenant=" + tenant,
                JSON.stringify(selectedRows),
                ServiceHelper.GetHttpHeaders()
            ).pipe(
                map(resp => resp),              
                catchError(err => throwError(err))
            );
        });
    }
}

export class SupplierInvoiceItemsForSIIRequest {
    InvoiceNumber: string;
    InvoiceLineNumber: number;
    InvoiceCounterKey: number;
    ItemCode: string;
    ItemDescription: string;
    ClassificationCode: string;
    TradeAgreementCode: string;
    InvoiceQuantityType: string;
    InvoiceQuantity: string;
    ItemPrice: string;
    ItemPriceCurrencyCode: string;
    OriginCountryCode: string;
    InvoiceQuantityTypeName: string;
    TradeAgreementName: string;
    OriginCountryName: string;
    RequestRequiredStatus: string;
    LineNumber: number;
    HasDemandState: boolean;
    Counter: number;
}
