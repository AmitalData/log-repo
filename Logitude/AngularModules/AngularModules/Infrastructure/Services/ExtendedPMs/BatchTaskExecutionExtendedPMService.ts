import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { BIReportPM } from '../../EntityPMs/BIReportPM';
import { of, defer } from 'rxjs';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';
import { ClassLevelValidator } from '../../Validators/ClassLevelValidator';
import { CustomFieldClass } from '../../DataContracts/CustomFieldClass';

@Injectable()
export class BatchTaskExecutionExtendedPMService {
    private httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/BatchTaskExecutionsExtended';
    }
    Cancel(batchTaskExecutionId: string) {
        return this.httpClient.post(this._apiUrl + '/PostCancel?batchTaskExecutionId=' + batchTaskExecutionId, null, ServiceHelper.GetHttpHeaders()).pipe(
            map(response => {
                let serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));
    }
 
}
