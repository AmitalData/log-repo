import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { WorkerRoleNamePM } from '../../EntityPMs/WorkerRoleNamePM';
import { defer } from 'rxjs';

@Injectable()
export class WorkerRoleNameExtendedPMService {
    private httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/WorkerRoleNamesExtended';
    }

    insert(entityPM: WorkerRoleNamePM) {
        return defer(() => {
            var serviceResponse: ServiceResponse = new ServiceResponse();

            return this.httpClient.post(this._apiUrl, JSON.stringify(entityPM), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                serviceResponse.Result = response;

                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }
}
