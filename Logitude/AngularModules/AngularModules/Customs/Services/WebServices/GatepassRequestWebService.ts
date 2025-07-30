import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';


@Injectable()

export class GatepassRequestWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/GatepassRequest';
    }
    
    GetGatepassRequestByMasterCourierId(masterCourierId: string, tenant: number) {
        return defer(() => {
            const authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            let serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            return this._http.get(this._apiUrl + "/GetGatepassRequestByMasterCourierId/?masterCourierId=" + masterCourierId + "&tenant=" + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                let serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );
    }
}
