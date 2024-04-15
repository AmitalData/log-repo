import { ServiceHelper } from '../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class EntityLastActivityService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/EntityLastActivity';
    }

    AddActivityLog(entityId: string, objectTableId: string, loggedContactId: string, logCode: string) {
        var url = this._apiUrl + '/GetActivityLog?entityId=' + entityId + '&objectTableId=' + objectTableId + '&loggedContactId=' + loggedContactId + '&logCode=' + logCode;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myResult = response;

                return myResult;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}
