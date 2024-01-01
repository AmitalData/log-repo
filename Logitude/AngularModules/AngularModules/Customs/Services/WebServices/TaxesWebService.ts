import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ServiceHelper } from 'Infrastructure/Utilities/ServiceHelper';

@Injectable()
export class TaxesWebService {
    private http: HttpClient = ServiceHelper.HttpClient;
    private apiUrl: string = ServiceHelper.GetLogitudeURL() + 'api/taxes';
    private headers: HttpHeaders = ServiceHelper.GetHttpHeaders().headers;

    constructor() { }

    getTokens(): Promise<string> {
        return this.http.get(this.apiUrl + '/getlinkLogin', { headers: this.headers }).toPromise() as Promise<string>;
    }
}