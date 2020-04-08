import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

@Injectable()

export class CRMControlsService {
    private _apiUrl: string;
    private _http: HttpClient
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CRMDomain';
    }

    PutCompleteActivity(args: MeetingSummary) {
        return Observable.defer(() => {

            return this._http.put(this._apiUrl + '/PutCompleteActivity', JSON.stringify(args), ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                var myJsonResult = res;
                var myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}

export class MeetingSummary {
    public ActivityId: string;
    public Summary: string;
    public Post: boolean;
}
