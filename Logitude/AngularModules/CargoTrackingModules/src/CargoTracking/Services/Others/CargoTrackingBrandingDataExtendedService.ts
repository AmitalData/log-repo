import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpClientModule } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { CargoTrackingBrandingData } from '../../DataContracts/CargoTrackingBrandingData';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from './../../Utilities/ServiceHelper';
import { CargoTrackingBrandingDataRequest } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingDataRequest';
import { SessionInfo } from 'src/Infrastructure/Utilities/SessionInfo';


@Injectable()

export class CargoTrackingBrandingDataExtendedService {
    private http: HttpClient;
    private _apiUrl: string;
    private httpHeaders: HttpHeaders;
    constructor(private _http: HttpClient,@Inject('BASE_URL') baseUrl: string) {
        this.httpHeaders = ServiceHelper.GetHeaders();
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/CargoTrackingBranding';
    }

    get(BrandingDataRequest:CargoTrackingBrandingDataRequest) {
        var url = '/PutGetCargoTrackingBrandingData';
        var callUrl = this._apiUrl.concat(url);

        return this._http.put(callUrl,BrandingDataRequest, { headers: this.httpHeaders}).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }
    GetUserDashboardBrandingData(BrandingDataRequest:CargoTrackingBrandingDataRequest) {
        var url = '/PutGetCargoTrackingBrandingDataForPrivateSite';
        var callUrl = this._apiUrl.concat(url);

        return this._http.put(callUrl,BrandingDataRequest, { headers: this.httpHeaders}).pipe(
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

    
    GetLoggedContact(tenant: number) {
        var url = this._apiUrl + '/GetLoggedContact?tenant=' + SessionInfo.LoggedUserTenant;

        var token = SessionInfo.Token || sessionStorage.getItem('Token');

        const authHeader = {
            headers: new HttpHeaders({
                'Access-Control-Allow-Origin': '*',
                'Content-Type': 'application/json',
                'Token': token
            })
        };

        return this._http.get(url, authHeader).pipe(map(response => {
            var result = response;
            return result;
        }), catchError(error => {
            return error;
        }));
    }


}
