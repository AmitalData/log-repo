import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders, HttpClientModule } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { CargoTrackingBrandingData } from '../../DataContracts/CargoTrackingBrandingData';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';


@Injectable()

export class CargoTrackingBrandingDataExtendedService {
    private http: HttpClient;
    private _apiUrl: string;
    private httpHeaders: HttpHeaders;
    constructor(private _http: HttpClient) {
      //  this._http = ServiceHelper.HttpClient;
        this._apiUrl = this.GetLogitudeURL() + 'api/CargoTracking';
    }
    public  GetLogitudeURL() {

        var logitude_url = location.href.replace('index.html', '');

        if (location.href.indexOf('localhost') > -1) {
            logitude_url = 'http://localhost:9996/';
        }

        else {
            var urlArr = location.href.split("/index.html");
            var url = urlArr[0];
            url = url.replace(url.substring(url.lastIndexOf('/'), url.length), "");
            logitude_url = url + "/";
        }

        return logitude_url;
    }
    get() {
        var url = '/GetCargoTrackingBrandingData';
        var callUrl = this._apiUrl.concat(url);

        return this._http.get(callUrl, {}).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }

   private   GethtpHeaders() {

        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                 'Token':null
            })
        };
       return httpOptions;
    }
   

}
