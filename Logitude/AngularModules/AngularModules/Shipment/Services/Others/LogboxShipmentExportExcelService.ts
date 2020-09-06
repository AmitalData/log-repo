import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { AgentSharedManifestPM } from '../../../Common/EntityPMs/AgentSharedManifestPM';
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';
import { HttpClient, HttpEvent, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


@Injectable()

export class LogboxShipmentExportExcelService {
    private _httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/LogboxShipmentExportExcel';
    }




    GetQueryToExcelData(logboxShipmentExportExcelArgs: any) {

        return defer(() => {
            return this._httpClient.post(this._apiUrl + '/PostGetQueryToExcelData', JSON.stringify(logboxShipmentExportExcelArgs), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
             
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }



}
