import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { catchError, map } from "rxjs/operators";
import { ServiceResponse } from "src/CargoTracking/DataContracts/ServiceResponse";
import { ServiceHelper } from "src/CargoTracking/Utilities/ServiceHelper";


@Injectable()
export class UserExtendedService {

    private _apiUrl: string;
    public authHeaders = ServiceHelper.GetHeadersWithToken();
    public headers = ServiceHelper.GetHeaders();
    constructor(
        private _http: HttpClient,
        @Inject('BASE_URL') baseUrl: string
    ) {
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/UserExtended';
    }

    
    IsUserAdmin()
    {
        return this._http.get(this._apiUrl + '/GetIsUserAdmin', this.authHeaders).pipe(
            map((response: ServiceResponse) =>
            {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null)
        );
    }
}

