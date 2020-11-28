import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpClientModule } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { CargoTrackingBrandingData } from '../../DataContracts/CargoTrackingBrandingData';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from './../../Utilities/ServiceHelper';


@Injectable()

export class CargoTrackingBrandingDataExtendedService {
    private http: HttpClient;
    private _apiUrl: string;
    private httpHeaders: HttpHeaders;
    constructor(private _http: HttpClient,@Inject('BASE_URL') baseUrl: string) {
        this.httpHeaders = ServiceHelper.GetHeaders();
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/CargoTrackingBranding';
    }
   
    get(domain:string) {
        var url = '/GetCargoTrackingBrandingData?domain='+domain;
        var callUrl = this._apiUrl.concat(url);

        return this._http.get(callUrl, { headers: this.httpHeaders}).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }

    GetTenantByDomain(domain:string) {
        var url = '/GetCargoTrackingBrandingTenantByDomain?domain='+domain;
        var callUrl = this._apiUrl.concat(url);

        return this._http.get(callUrl, { headers: this.httpHeaders}).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }
    
   

}
