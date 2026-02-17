import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

@Injectable()

export class CRMControlsService {
    private _apiUrl: string;
    private _http: Http;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CRMDomain';
    }

    PutCompleteActivity(args: MeetingSummary) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            return this._http.put(this._apiUrl + '/PutCompleteActivity', JSON.stringify(args), { headers: authHeader }).map((res) => {
                var myJsonResult = res.json();
                var myResponse = new ServiceResponse();
                myResponse.Result = myJsonResult;
                return myResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }
}

export class MeetingSummary {
    public ActivityId: string;
    public Summary: string;
    public Post: boolean;
}