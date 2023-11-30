import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';

@Injectable({
    providedIn: 'root'
})
export class SessionTimeoutServiceService {

    private _apiUrl: string;
    public onExpirToken: Subject<boolean> = new Subject<boolean>();
    lastTimeout;
    constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/';
    }

    RunSessionTimeOut() {
        var url = this._apiUrl + 'CargoSession';
        this._http.get<number>(url, ServiceHelper.GetHeadersWithToken()).subscribe(tokenLifeTime => {
            if (tokenLifeTime != null) {
                if (this.lastTimeout)
                    clearTimeout(this.lastTimeout);
                this.lastTimeout = 0;
                this.lastTimeout = setTimeout(() => this.runOnExpirTokenEvent(), tokenLifeTime);
            }

        });
    }
    runOnExpirTokenEvent() {
        this.onExpirToken.next();
    }
}
