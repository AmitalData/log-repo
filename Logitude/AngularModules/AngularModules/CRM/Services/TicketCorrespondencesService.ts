import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';

@Injectable()

export class TicketCorrespondencesService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TicketCorrespondences';
    }

    GetCorrespondencesList(entityId: string) {

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetTicketCorrespondences?entityId=' + entityId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists = response;
                return allLists;
            }));
        });
    }
}
