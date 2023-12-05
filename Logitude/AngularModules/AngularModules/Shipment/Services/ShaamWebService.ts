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

    getLinkToCodeForToken(): Observable<string> {
        return this.http.get(
            this.apiUrl + '/linkToCodeForToken',
            { headers: this.headers }
        ) as Observable<string>;
    }
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