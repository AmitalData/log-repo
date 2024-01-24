import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';



@Injectable()

export class ExportStorageWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExportStorage';
    }


    GetByCargoKeys(firstCargoID: string, secondCargoID: string, thirdCargoID: string, cargoIdentifierType: string, tenant: number) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetByCargoKeys/?firstCargoID=" + firstCargoID + "&secondCargoID=" + secondCargoID + "&thirdCargoID=" + thirdCargoID + "&cargoIdentifierType=" + cargoIdentifierType + "&tenant=" + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );
    }
}
