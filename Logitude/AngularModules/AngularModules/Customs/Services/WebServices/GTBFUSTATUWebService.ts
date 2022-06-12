import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';


export class GTBFUSTATUWebService {
    private _apiUrl: string;
    private _http: HttpClient;
    constructor() {
        this._http = ServiceHelper.HttpClient
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/GTBFUSTATUWebService';
    }

    GetAllGTBFUSTATU() {
        var url = this._apiUrl + '/GetAllGTBFUSTATU' ;
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse = new ServiceResponse();
                return serviceResponse.Result;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

}


