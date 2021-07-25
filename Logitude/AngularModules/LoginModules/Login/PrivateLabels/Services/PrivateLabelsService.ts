import { Inject, Injectable } from '@angular/core';
import {Http, Headers, Response} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch'; 
import 'rxjs/add/Observable/throw'; 
import { BrandingDataService } from './BrandingDataService';
import { ServiceResponse } from '../DataContracts/ServiceResponse'; 
import { Observable } from 'rxjs/Observable';

@Injectable()

export class PrivateLabelsService {
    private apiUrl: string;
    public AuthHeader: Headers;
    constructor(private _http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.AuthHeader = new Headers();
        this.AuthHeader.append('Content-Type', 'application/json');
        this.AuthHeader.append('Accept', 'application/json');
        this.apiUrl = BrandingDataService.GetAppURL(baseUrl) + 'api/PrivateLable';
    } 

    GetIsPrivateLableUrl(privatelableurl: string) {
        return this._http.get(this.apiUrl + "/GetIsPrivateLableByLoggedDomain", { headers: this.AuthHeader }).map((response) => {
            
            return response.json();
        }).catch(this.handleError)
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error);
    } 
  }  
 