
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { InfraSettings } from '../../Utilities/InfraSettings';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';

import { ScreenSectionPM } from '../../EntityPMs/ScreenSectionPM';




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
