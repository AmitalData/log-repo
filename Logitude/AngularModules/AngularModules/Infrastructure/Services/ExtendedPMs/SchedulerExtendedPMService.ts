import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../Validators/ClassLevelValidator';
import { InfraSettings } from '../../Utilities/InfraSettings';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { TasksSchedulerPM } from '../../EntityPMs/TasksSchedulerPM';
import { HttpClient, HttpHeaders, HttpEvent, HttpResponse } from '@angular/common/http';
import { map, catchError, tap } from 'rxjs/operators';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';
import { CustomFieldClass } from 'Infrastructure/DataContracts/CustomFieldClass';

@Injectable()
export class SchedulerExtendedPMService {
    private httpClient: HttpClient;
    private apiUrl: string;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SchedulerExtended';
    }

    GetSchedulerHistoryLogs(HistoryId: string) {
        var url = this.apiUrl + '/GetSchedulerHistoryLogs?' + 'historyId=' + HistoryId;

        return this.httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = response;

            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetSchedulerDetailsById(schedulerId: string) {
        var url = this.apiUrl + '/GetSchedulerDetailsById?' + 'schedulerId=' + schedulerId;

        return this.httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = response;

            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    insert(entityPM: TasksSchedulerPM) {
        var callTime = new Date();
        var url = this.apiUrl;
        var validator: ClassLevelValidator;
        validator = new ClassLevelValidator();
        var errorsArray = validator.Validate("TasksScheduler", entityPM);
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();

        var mappedEntity: TasksSchedulerPM;
        mappedEntity = this.MapJsonToEntityPM(entityPM, false);

        if (errorsArray.length == 0) {
            return this.httpClient.post(url, mappedEntity, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                if (response instanceof HttpResponse) {
                    serviceResponse.Result = response;
                    var servertime = response.headers.get('ServerExecutionTime');
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "TasksScheduler", "SaveChanges", "");

                    return serviceResponse;
                }
            }), catchError(ServiceHelper.HandleServiceError));
        }
        else {
            serviceResponse.HasError = true;
            serviceResponse.ErrorsArray = errorsArray;

            return of(serviceResponse);
        }
    }

    update(entityPM: TasksSchedulerPM) {
        var callTime = new Date();
        var url = this.apiUrl;
        var validator: ClassLevelValidator;
        validator = new ClassLevelValidator();
        var errorsArray = validator.Validate("TasksScheduler", entityPM);
        var serviceResponse: ServiceResponse;
        serviceResponse = new ServiceResponse();

        var mappedEntity: TasksSchedulerPM;
        mappedEntity = this.MapJsonToEntityPM(entityPM, false);

        if (errorsArray.length == 0) {
            return this.httpClient.put(url, mappedEntity, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                if (response instanceof HttpResponse) {
                    serviceResponse.Result = response;
                    var servertime = response.headers.get('ServerExecutionTime');
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "TasksScheduler", "SaveChanges", "");

                    return serviceResponse;
                }
            }), catchError(ServiceHelper.HandleServiceError));
        }
        else {
            serviceResponse.HasError = true;
            serviceResponse.ErrorsArray = errorsArray;

            return of(serviceResponse);
        }
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: TasksSchedulerPM = null) {


        if (!entityPM) {

            entityPM = new TasksSchedulerPM();
        }

        var customFields: Array<string> = [];
        for (var i = 1; i < 11; i++) {
            customFields.push("Field" + i);
        }
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {

                continue;
            }
            var property = jsonPMKeys[key];

            if (customFields.indexOf(property) > -1) {
                if (jsonPM[property]) {
                    var customFieldClass: CustomFieldClass = new CustomFieldClass(jsonPM[property].Value, jsonPM[property].FieldName, jsonPM[property].TableName);
                    entityPM[property] = customFieldClass;
                }
            }
            else {
                entityPM[property] = jsonPM[property];
            }

        }




        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);

        }
        else {

            entityPM.OldEntityPM = null;
        }
        entityPM.IsDirty = false;
        return entityPM;
    }

 


    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    }

    public GetNewEntityPM() {
        var entityPM: TasksSchedulerPM;
        entityPM = new TasksSchedulerPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }

    isExceedsScheduledTasksLimitPerReport(tenant,createdBy: string, entityId: string) { 
        let url = this.apiUrl + "/GetIsExceedsScheduledTasksLimitPerReport" + '?tenant=' + tenant + "&entityId=" + entityId;

        return this.httpClient.get(url, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = response; 
            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

}
