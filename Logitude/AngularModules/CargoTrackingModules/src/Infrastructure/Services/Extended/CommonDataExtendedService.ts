import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';


@Injectable()

export class CommonDataExtendedService {
    private http: HttpClient;
    private _apiUrl: string;
    private httpHeaders: HttpHeaders;
    constructor(private _http: HttpClient,@Inject('BASE_URL') baseUrl: string) {
        this.httpHeaders = ServiceHelper.GetHeaders();
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/commondata';
    }
   
    GetComponayLogo(tenant:number) {
        var url = '/?companyId='+tenant+'&isSmalLogo=false';
        var callUrl = this._apiUrl.concat(url);

        return this._http.get(callUrl, { headers: this.httpHeaders}).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(error => {
                return error;
            }));
    }

   
    
   

}
