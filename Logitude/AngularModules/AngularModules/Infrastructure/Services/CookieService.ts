import { HttpClient, HttpHeaders } from '@angular/common/http';
import { SessionLocator } from '../Utilities/SessionLocator';
import { ServiceHelper } from '../Utilities/ServiceHelper';
import { SessionInfo } from '../Utilities/SessionInfo';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { AppTool } from '../Tools';
@Injectable({
  providedIn: 'root'
})
export class CookieService {
    
    logitudeURL: string = null;
    baseUrlApi: string = null;
    baseMetaUrlApi: string = null;
    
    public CurrentTenant: number;
    public AuthHeader;
    public LoggedUserId: string;
    public LoggedUserEmail: string;
    constructor(private _http: HttpClient) {
        this.logitudeURL = AppTool.GetLogitudeURL();
        this.baseUrlApi = this.logitudeURL + "api/";
        this.baseMetaUrlApi = this.logitudeURL + "api/ngMetaData";
    }

    
    GetIsAppServiceData() {
        var url = this.baseUrlApi + "Authentication" + '/GetIsAppServiceData';
         return this._http.get(url).pipe(map(response => {
            return response;
        }), catchError(ServiceHelper.HandleServiceError));
    }
}
