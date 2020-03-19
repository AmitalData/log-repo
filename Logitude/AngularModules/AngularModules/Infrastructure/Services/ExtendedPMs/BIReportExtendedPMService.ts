import {Injectable} from '@angular/core';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {SessionInfo} from '../../Utilities/SessionInfo';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpEvent, HttpResponse } from '@angular/common/http';
import { map, catchError, tap } from 'rxjs/operators';
@Injectable()

export class BIReportExtendedPMService {
    private httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/BIReportsExtended';
    }

    DoesReportExist(name: string, folderId: string): Observable<ServiceResponse> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Token': ServiceHelper.GetLoggedUserToken()
            })
        };

        var url = this._apiUrl + '/getReportExist?' + 'name=' + name + '&folderId=' + folderId;

        return this.httpClient.get(url, httpOptions).pipe(
            map(response => {
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = response;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }






}
