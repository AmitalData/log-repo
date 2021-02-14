import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpClientModule } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { BrandingDataService } from './BrandingDataService';
import { HybridLabelsBrandingDataRequest } from '../DataContracts/HybridLabelsBrandingDataRequest';
import { ServiceResponse } from '../DataContracts/ServiceResponse'; 


@Injectable()

export class HybridLabelsBrandingDataService {
    private http: HttpClient;
    private _apiUrl: string;
    private httpHeaders: HttpHeaders;
    constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.httpHeaders = BrandingDataService.GetHeaders();
        this._apiUrl = BrandingDataService.GetAppURL(baseUrl) + 'api/TenantManagmentPrivateLabels';
    }

    GetBrandingData(BrandingDataRequest: HybridLabelsBrandingDataRequest) {
        var url = '/PutGetHybridLabelsBrandingData';
        var callUrl = this._apiUrl.concat(url);

        return this._http.put(callUrl, BrandingDataRequest, { headers: this.httpHeaders }).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }  


}
