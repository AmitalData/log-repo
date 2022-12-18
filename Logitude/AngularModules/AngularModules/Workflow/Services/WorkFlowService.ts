import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { WorkFlowVersionList } from '../EntityLists/WorkFlowVersionList';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';

@Injectable()

export class WorkFlowService {
    private _http: HttpClient;
    private _apiUrl: string;
    public static CachedData: WorkFlowPM;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/workflows';
    }

    GetWorkflow(WorkflowId: string) {

        var urlparameters = '/GetSingleWorkflow?id=' + WorkflowId;

        var callUrl = this._apiUrl.concat(urlparameters);
        return this._http.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }
}

