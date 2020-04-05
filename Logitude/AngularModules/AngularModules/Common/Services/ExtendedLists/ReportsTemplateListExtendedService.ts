import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Injectable()
export class ReportsTemplateListExtendedService {


    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReportsTemplateExtended';
    }

    getReportsTemplateListsByReportId(reportId: string, reportType:string = "") {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + "/getReportsTemplateListsByReportId" + '?reportId=' + reportId + "&reportType=" + reportType,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = response;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
    }

}

