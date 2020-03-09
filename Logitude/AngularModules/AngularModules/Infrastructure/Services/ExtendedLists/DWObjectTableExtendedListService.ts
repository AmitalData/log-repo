import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { BusinessRoleList } from '../../EntityLists/BusinessRoleList';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';

@Injectable()

export class DWObjectTableExtendedListService {
    private httpClient: HttpClient;
    private apiUrl: string;
    public static CachedData: Array<BusinessRoleList> = [];
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/dwobjecttableextended';
    }

    GetFactTablesNames(): Observable<ServiceResponse> {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application-json',
                'Token': ServiceHelper.GetLoggedUserToken()
            })
        };
        var url = this.apiUrl + '/GetFactTablesNames';
        return Observable.defer(() => {
            return this.httpClient.get(url, httpOptions).pipe(
                map(response => {
                    var list = response;

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = list;

                    return serviceResponse;
                }),
                catchError(ServiceHelper.HandleServiceError));
        });
    }
}
