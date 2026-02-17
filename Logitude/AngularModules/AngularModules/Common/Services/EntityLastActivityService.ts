import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map';  
import {ServiceArgs} from '../../Infrastructure/DataContracts/ServiceArgs';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {Observable} from 'rxjs/Rx';
@Injectable()

export class EntityLastActivityService {
    private _apiUrl: string;
    private _http: Http;
    private _serviceArgs: ServiceArgs;
    constructor() {

    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/EntityLastActivity';
    }

    AddActivityLog(entityId: string, objectTableId: string, loggedContactId: string, logCode: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetActivityLog?entityId=' + entityId + '&objectTableId=' + objectTableId + '&loggedContactId=' + loggedContactId + '&logCode=' + logCode, {
                headers: authHeader
            }).map(response => {

                var myResult = response.json();

                return myResult;
            });
        });
    }
}
