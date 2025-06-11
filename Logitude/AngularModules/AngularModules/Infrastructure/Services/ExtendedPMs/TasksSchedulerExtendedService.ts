
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';


@Injectable()

export class TasksSchedulerExtendedService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TasksSchedulerExtended';
    }

    Delete(tasksSchedulerId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.delete(this._apiUrl + '?tasksSchedulerId=' + tasksSchedulerId ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var rerviceResponse: ServiceResponse;
            rerviceResponse = new ServiceResponse();

            rerviceResponse.Result = response;
            return rerviceResponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }
}
