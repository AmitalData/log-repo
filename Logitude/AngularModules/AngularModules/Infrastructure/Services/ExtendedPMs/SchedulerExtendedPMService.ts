
import {Injectable} from '@angular/core';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../Validators/ClassLevelValidator';
import {InfraSettings} from '../../Utilities/InfraSettings';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {TasksSchedulerPM} from '../../EntityPMs/TasksSchedulerPM';
import { HttpClient, HttpHeaders, HttpEvent, HttpResponse } from '@angular/common/http';
import { map, catchError, tap } from 'rxjs/operators';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';
import { Body } from '@angular/http/src/body';

@Injectable()

export class SchedulerExtendedPMService {
    private httpClient: HttpClient;
    private apiUrl: string;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SchedulerExtended';
    }

    GetSchedulerHistoryLogs(HistoryId: string) {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Token': ServiceHelper.GetLoggedUserToken()
            })
        };

        var url = this.apiUrl + '/GetSchedulerHistoryLogs?' + 'historyId=' + HistoryId;
        return Observable.defer(() => {
            return this.httpClient.get(url, httpOptions).pipe(
                map(response => {
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),
                catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetSchedulerDetailsById(schedulerId: string) {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Token': ServiceHelper.GetLoggedUserToken()
            })
        };

        var url = this.apiUrl + '/GetSchedulerDetailsById?' + 'schedulerId=' + schedulerId;
        return Observable.defer(() => {
            return this.httpClient.get(url, httpOptions).pipe(
                map(response => {
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = response;

                    return serviceResponse;
                }),
                catchError(ServiceHelper.HandleServiceError));
        });
    }

  	 insert(entityPM: TasksSchedulerPM) {
             const headers: HttpHeaders = new HttpHeaders({
                 'Content-Type': 'application/json',
                 'Token': ServiceHelper.GetLoggedUserToken(),
             });

             var callTime = new Date();
             var url = this.apiUrl;
             return Observable.defer(() => {
                 var validator: ClassLevelValidator;
                 validator = new ClassLevelValidator();
                 var errorsArray = validator.Validate("TasksScheduler", entityPM);
                 var serviceResponse: ServiceResponse;
                 serviceResponse = new ServiceResponse();

                 if (errorsArray.length == 0) {
                     return this.httpClient.post(url, entityPM, {headers, observe:'response'}).pipe(
                         tap((event: HttpEvent<any>) => {
                             if (event instanceof HttpResponse) {
                                 var servertime = event.headers.get('ServerExecutionTime');
                                 PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "TasksScheduler", "SaveChanges", "");
                             }
                         }),
                         map(response => {
                             serviceResponse.Result = response;

                             return serviceResponse;
                         }),
                         catchError(ServiceHelper.HandleServiceError));
                 }
                 else {
                     serviceResponse.HasError = true;
                     serviceResponse.ErrorsArray = errorsArray;

                     return Observable.of(serviceResponse);

                 }
             });
    }

    update(entityPM: TasksSchedulerPM) {
        const headers: HttpHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            'Token': ServiceHelper.GetLoggedUserToken(),
        });

        var callTime = new Date();
        var url = this.apiUrl;
        return Observable.defer(() => {
            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();
            var errorsArray = validator.Validate("TasksScheduler", entityPM);
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            if (errorsArray.length == 0) {
                return this.httpClient.put(url, entityPM, { headers, observe: 'response'}).pipe(
                    tap((event: HttpEvent<any>) => {
                        if (event instanceof HttpResponse) {
                            var servertime = event.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "TasksScheduler", "SaveChanges", "");
                        }
                    }),
                    map(response => {
                        serviceResponse.Result = response;
                        return serviceResponse;
                    }),
                    catchError(ServiceHelper.HandleServiceError));
            }
            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return Observable.of(serviceResponse);
            }
        });
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


}
