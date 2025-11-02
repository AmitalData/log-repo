import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


import { defer, of } from 'rxjs';

import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ExcelReportArguments } from 'Common/DataContracts/ExcelReportArguments';


@Injectable()
export class ExcelReportService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ExcelReport';
    }

    getDataProviderFields(reportId: string, reportsTemplateId: string, maxSubLevels?: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
    
        let url = this._apiUrl + "/getDataProviderFields" + 
                  '?reportId=' + reportId + 
                  "&reportsTemplateId=" + reportsTemplateId;
    
        if (maxSubLevels !== undefined) {
            url += "&maxSubLevels=" + maxSubLevels;
        }
    
        return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                var pmresponse: ServiceResponse = new ServiceResponse();
                pmresponse.Result = response;
                return pmresponse;
            }),
            catchError(ServiceHelper.HandleServiceError)
        );
    }
    

    postDataProviderProperties(excelReportArguments: ExcelReportArguments) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.post(this._apiUrl + "/postDataProviderProperties", JSON.stringify(excelReportArguments), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

}

