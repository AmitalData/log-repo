import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CompleteStatuses } from 'CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/SIIRequest/SIIRequestTabs/SIIRequestComponent';
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

    getSupplierInvoiceItemsForSIIRequest(declarationId: string) {
        return defer(() => {
            let authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            let serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            return this._http.get(this._apiUrl + "/GetSupplierInvoiceItemsForSIIRequest/?declarationId=" + declarationId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                let serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }
        );
    }
}

export class SupplierInvoiceItemsForSIIRequest {
    InvoiceNumber: string;
    LineNumber: number;
    ItemCode: string;
    ItemName: string;
    ItemDescription: string;
    ClassificationCode: string;
    TradeAgreementCode: string;
    TradeAgreementName: string;
    InvoiceQuantityType: string;
    InvoiceQuantityTypeName: string;
    InvoiceQuantity: string;
    ItemPrice: string;
    ItemPriceCurrencyCode: string;
    OriginCountryCode: string;
    OriginCountryName: string;
    ReqConfirmationTypeCode: string;
    IsCompletedStatus: number;
}
