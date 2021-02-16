import { Inject, Injectable } from '@angular/core';
import {Http, Headers, Response} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch'; 
import { BrandingDataService } from './BrandingDataService';
import { HybridLabelsBrandingDataRequest } from '../DataContracts/HybridLabelsBrandingDataRequest';
import { ServiceResponse } from '../DataContracts/ServiceResponse'; 


@Injectable()

export class HybridLabelsBrandingDataService {
    private http: Http;
    private _apiUrl: string;
    private httpHeaders: Headers;
    constructor(private _http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.httpHeaders = BrandingDataService.GetHeaders();
        this._apiUrl = BrandingDataService.GetAppURL(baseUrl) + 'api/TenantManagmentPrivateLabels';
    } 

    GetUserDashboardBrandingData(BrandingDataRequest: HybridLabelsBrandingDataRequest) {
        var url = '/PutGetHybridLabelsBrandingData';
        var callUrl = this._apiUrl.concat(url);

        return this._http.put(callUrl, BrandingDataRequest, { headers: this.httpHeaders }).map((response) => {
            var result: ServiceResponse = response.json();
            return result;
        });
    }
     
    GetBrandingData(BrandingDataRequest: HybridLabelsBrandingDataRequest) {
            var url = '/PutGetHybridLabelsBrandingData';
            var callUrl = this._apiUrl.concat(url);

            return this._http.post(callUrl, BrandingDataRequest , { headers: this.httpHeaders }).map(response => {
                var result = response.json();
                return result;
            });

        } 
    }  


 