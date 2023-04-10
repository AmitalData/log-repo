import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';
import { WorkFlowInstanceActivityList } from '../../EntityLists/WorkFlowInstanceActivityList';
import { WorkFlowInstanceVariableList } from 'Workflow/EntityLists/WorkFlowInstanceVariableList';

@Injectable()

export class WorkflowInstanceExtendedService {
	private _http: HttpClient;
	private _apiUrl: string;

	constructor() {
		this._http = ServiceHelper.HttpClient;
		this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/workflowinstanceextended';
	}

	getActivities(workflowInstanceId: string) {
		var callTime = new Date();
		return defer(() => {
			return this._http.get(this._apiUrl + '/getactivities?workflowinstanceid=' + workflowInstanceId, ServiceHelper.GetHttpFullHeaders())
				.pipe(
					map((response: HttpResponse<any>) => {
						var allLists = response.body;
						var _mappedListsArray: Array<WorkFlowInstanceActivityList> = [];
						if (allLists) {
							for (var key in allLists) {
								var entity: WorkFlowInstanceActivityList = this.MapJsonToActivityEntityList(allLists[key]);
								_mappedListsArray.push(entity);
							}
						}

						var serviceResponse: ServiceResponse = new ServiceResponse();
						serviceResponse.Result = _mappedListsArray;
						serviceResponse.CallTime = callTime;

						var servertime = response.headers.get('ServerExecutionTime');
						PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "WorkflowInstanceExtended", "GetActivities", 'workflowinstanceid=' + workflowInstanceId);

						return serviceResponse;
					}), catchError(ServiceHelper.HandleServiceError));
		});
	}

	getVariables(workflowInstanceId: string) {
		var callTime = new Date();
		return defer(() => {
			return this._http.get(this._apiUrl + '/getvariables?workflowinstanceid=' + workflowInstanceId, ServiceHelper.GetHttpFullHeaders())
				.pipe(
					map((response: HttpResponse<any>) => {
						var allLists = response.body;
						var _mappedListsArray: Array<WorkFlowInstanceVariableList> = [];
						if (allLists) {
							for (var key in allLists) {
								var entity: WorkFlowInstanceVariableList = this.MapJsonToVariableEntityList(allLists[key]);
								_mappedListsArray.push(entity);
							}
						}

						var serviceResponse: ServiceResponse = new ServiceResponse();
						serviceResponse.Result = _mappedListsArray;
						serviceResponse.CallTime = callTime;

						var servertime = response.headers.get('ServerExecutionTime');
						PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "WorkflowInstanceExtended", "GetVariables", 'workflowinstanceid=' + workflowInstanceId);

						return serviceResponse;
					}), catchError(ServiceHelper.HandleServiceError));
		});
	}


	MapJsonToActivityEntityList(jsonList: any) {
		var entityList: WorkFlowInstanceActivityList;
		entityList = new WorkFlowInstanceActivityList();
		var jsonListKeys = Object.keys(jsonList);
		for (var key in jsonListKeys) {
			var property = jsonListKeys[key];
			entityList[property] = jsonList[property];
		}
		return entityList;
	}


	MapJsonToVariableEntityList(jsonList: any) {
		var entityList: WorkFlowInstanceVariableList;
		entityList = new WorkFlowInstanceVariableList();
		var jsonListKeys = Object.keys(jsonList);
		for (var key in jsonListKeys) {
			var property = jsonListKeys[key];
			entityList[property] = jsonList[property];
		}
		return entityList;
	}
}