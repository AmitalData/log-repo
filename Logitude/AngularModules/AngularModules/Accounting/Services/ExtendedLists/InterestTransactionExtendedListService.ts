import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { catchError, map } from 'rxjs/operators';
 
const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Token': ServiceHelper.GetLoggedUserToken()
    })
};

@Injectable()
export class InterestTransactionExtendedListService {
    private _http: Http;
    private httpClient: HttpClient;
    private _apiUrl: string;

    constructor() {
        this._http = ServiceHelper.Http;
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/InterestTransactionViews';
    }


    GetAllInterestTransactionByDate(ReportId: string, InterestCalculationDate: Date) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var serviceResponse: ServiceResponse = new ServiceResponse();
        var url = this._apiUrl + "/GetAllInterestTransactionByDate?ReportId=" + ReportId + "&InterestCalculationDate=" + InterestCalculationDate;
        return this.httpClient.get(url, httpOptions).pipe(
            map(response => {
                serviceResponse.Result = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }
}
