import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {WorkFlowVersionList} from '../EntityLists/WorkFlowVersionList';

@Injectable()

export class WorkFlowVersionService {
	private _http: HttpClient;
    private _apiUrl: string;   
	public static CachedData: Array<WorkFlowVersionList> = [];
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/workflowversionviews';  
    }

	GetVersionIds(WorkflowId:string) {

        var urlparameters = '/GetByWorkflowId?workflowId=' + WorkflowId;

        var callUrl = this._apiUrl.concat(urlparameters);
        return this._http.get(callUrl, ServiceHelper.GetHttpHeaders()).pipe(
            map((response:ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(ServiceHelper.HandleServiceError));

    }
}

