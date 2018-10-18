import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';


import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import {SessionInfo} from './SessionInfo';




@Injectable()
export class PasswordChangeService {

    //private _http: Http;
    private _apiUrl: string;
    constructor(private _http: Http) {
        //this._http = ServiceHelper.Http;
        this._apiUrl = SessionInfo.GetLogitudeURL() + 'api/PasswordChange';
    }


}




