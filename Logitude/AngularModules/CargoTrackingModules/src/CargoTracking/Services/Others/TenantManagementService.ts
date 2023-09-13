import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { ServiceHelper } from "src/CargoTracking/Utilities/ServiceHelper";

@Injectable()
export class TenantManagementService {
    private _apiUrl: string;
    public authHeaders = ServiceHelper.GetHeadersWithToken();

    constructor(
        private _http: HttpClient, 
        @Inject('BASE_URL') baseUrl: string
    ) {
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/tenantmanagement';
    }

    getShipmentBuildMonth(): Promise<number> {
        return this._http.get(this._apiUrl + '/GetShipmentBuildMonth', this.authHeaders).toPromise() as Promise<number>;
    }
}
