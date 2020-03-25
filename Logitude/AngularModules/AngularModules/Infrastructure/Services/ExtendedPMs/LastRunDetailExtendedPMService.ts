import {Injectable} from '@angular/core';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {ServiceHelper} from '../../Utilities/ServiceHelper';

import {LastRunDetailPM} from '../../EntityPMs/LastRunDetailPM';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';


@Injectable()

export class LastRunDetailExtendedPMService {
    private httpClient: HttpClient;
    private apiUrl: string;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/lastrundetailsextended/';
    }

    UpdateLastRunDetails(entityPM: LastRunDetailPM, userId: string): Observable<ServiceResponse> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Token': ServiceHelper.GetLoggedUserToken()
            })
        };

        var url = this.apiUrl + "?userid=" + userId;
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();

        return this.httpClient.put(url, entityPM, httpOptions).pipe(
            map(response => {
                serviceResponse.Result = response;

                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }
}
