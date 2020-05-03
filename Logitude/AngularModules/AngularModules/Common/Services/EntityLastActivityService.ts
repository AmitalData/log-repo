import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {ServiceArgs} from '../../Infrastructure/DataContracts/ServiceArgs';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import { defer, of } from 'rxjs';

@Injectable()

export class EntityLastActivityService {
    private _apiUrl: string;
    private _http: HttpClient;
    private _serviceArgs: ServiceArgs;
    constructor() {

    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/EntityLastActivity';
    }

    AddActivityLog(entityId: string, objectTableId: string, loggedContactId: string, logCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetActivityLog?entityId=' + entityId + '&objectTableId=' + objectTableId + '&loggedContactId=' + loggedContactId + '&logCode=' + logCode,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var myResult = response;

                return myResult;
            }));
        });
    }
}
