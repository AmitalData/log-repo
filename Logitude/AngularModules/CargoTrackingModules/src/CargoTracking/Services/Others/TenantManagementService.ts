import { HttpClient, HttpResponse } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { catchError, map } from "rxjs/operators";
import { ServiceResponse } from "src/CargoTracking/DataContracts/ServiceResponse";
import { ServiceHelper } from "src/CargoTracking/Utilities/ServiceHelper";
import { TenantManagementPM } from "./TenantManagementPM";
import { defer, throwError } from "rxjs";
import { SessionInfo } from "src/Infrastructure/Utilities/SessionInfo";

@Injectable()
export class TenantManagementService {
    private _apiUrl: string;
    public authHeaders = ServiceHelper.GetHeadersWithToken();
    public headers = ServiceHelper.GetHeaders();
    constructor(
        private _http: HttpClient,
        @Inject('BASE_URL') baseUrl: string
    ) {
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/tenantmanagement';
    }

    getShipmentBuildMonth(): Promise<number> {
        return this._http.get(this._apiUrl + '/GetShipmentBuildMonth', this.authHeaders).toPromise() as Promise<number>;
    }


    get(id: number) {


        var authHeader = new Headers();
        //authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
       
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, { headers: this.headers}).pipe(
                map((response: ServiceResponse) => {
                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse = response;
                    return serviceResponse;
                }),
                catchError(err =>

                    throwError(err?.error?.ErrorMessage)

                ));
       
    }



}
