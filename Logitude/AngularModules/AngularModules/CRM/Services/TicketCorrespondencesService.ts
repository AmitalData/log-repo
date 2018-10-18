import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {ServiceArgs} from '../../Infrastructure/DataContracts/ServiceArgs';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';

@Injectable()

export class TicketCorrespondencesService {
    private _apiUrl: string;
    private _http: Http;
    private _serviceArgs: ServiceArgs;
    constructor() {

    }

    setServiceArgs(serviceArgs: ServiceArgs) {
        this._serviceArgs = serviceArgs;
        this._http = serviceArgs.http;

        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TicketCorrespondences';
    }

    GetCorrespondencesList(entityId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetTicketCorrespondences?entityId=' + entityId, {
                headers: authHeader
            }).map(response => {

                var allLists = response.json();
                return allLists;
            });
        });
    }
}