import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { ServiceHelper } from "src/CargoTracking/Utilities/ServiceHelper";
import { TenantManagementPM } from "./TenantManagementPM";

@Injectable()
export class TenantManagementPMService {
    private _apiUrl: string;
    public authHeaders = ServiceHelper.GetHeadersWithToken();

    constructor(
        private _http: HttpClient, 
        @Inject('BASE_URL') baseUrl: string
    ) {
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl) + 'api/tenantmanagements';
    }

    get(id: number): Promise<TenantManagementPM> {
        return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, this.authHeaders).toPromise() as Promise<TenantManagementPM>;
    }
}
