import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, defer } from 'rxjs';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { catchError, map } from 'rxjs/operators';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';

@Injectable()
export class ShaamWebService {
    private http: HttpClient = ServiceHelper.HttpClient;
    private apiUrl: string = ServiceHelper.GetLogitudeURL() + 'api/ShaamWebService';
    private headers: HttpHeaders = ServiceHelper.GetHttpHeaders().headers;

    constructor() { }

    getTokens(pageSize: number, page: number) {
        const ajax: Observable<TokensResponse> = this.http.get(
            this.apiUrl + '/tokens',
            {
                params: { pageSize: '' + pageSize, page: '' + page },
                headers: this.headers,
            }
        ) as Observable<TokensResponse>;

        return defer(() => {
            return ajax.pipe(map(response => {
                const serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response.tokens;
                serviceResponse.Count = response.count;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    getLinkToCodeForToken(user: string): Observable<string> {
        return this.http.get(
            this.apiUrl + '/linkToCodeForToken',
            {
                params: { user },
                headers: this.headers
            }
        ) as Observable<string>;
    }

    postNewRefreshToken(tenant: number, user: string, code: string): Promise<newRefreshTokenResponse> {
        return this.http.post(
            this.apiUrl + '/newRefreshToken', { tenant, user, code }
        ).toPromise() as Promise<newRefreshTokenResponse>;
    }

    getShaamSettings(): Promise<ShaamSettings> {
        return this.http.get(this.apiUrl + '/settings', { headers: this.headers }).toPromise() as Promise<ShaamSettings>;
    }

    postShaamSettings(key: string, secret: string, isTestEnvironment: boolean, invoiceV2: boolean): Promise<any> {
        return this.http.put(
            this.apiUrl + '/UpdateSettings',
            { clientId: key, secret, isTestEnvironment, invoiceV2 },
            { headers: this.headers }).toPromise();
    }
}

export interface newRefreshTokenResponse {
    message: string
    errorCode: number
    approved: boolean
    status: number
}

interface Token {
    Id: string;
    Tenant: number;
    UpdateDate?: string;
    UserCode: string;
    RefreshExpierDate: string;
    IsActive: boolean;
    RefreshToken: string;
    AccessExpireDate: string;
    AccessToken: string;
    CreateDate: string;
}
interface TokensResponse {
    tokens: Token[];
    count: number;
}

export interface ShaamSettings {
    id: string;
    clientId: string;
    secret: string;
    companyName: string;
    createDate: string;
    updateDate: string;
    tenant: number;
    isTestEnvironment: boolean;
    invoiceV2: boolean;
}
