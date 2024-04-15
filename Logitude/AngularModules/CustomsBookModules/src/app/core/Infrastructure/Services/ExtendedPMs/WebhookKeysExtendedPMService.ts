import {Injectable} from '@angular/core';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../Validators/ClassLevelValidator';
import {Guid} from '../../Utilities/Guid';
import {InfraSettings} from '../../Utilities/InfraSettings';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {SessionInfo} from '../../Utilities/SessionInfo';
import {PerformanceLogger} from '../../Utilities/PerformanceLogger';
import {CustomFieldClass} from '../../DataContracts/CustomFieldClass'

import {WebhookKeysPM} from '../../EntityPMs/WebhookKeysPM';
import { HttpClient, HttpHeaders, HttpEvent, HttpResponse } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { defer, of } from 'rxjs';


@Injectable()

export class WebhookKeysExtendedPMService {
    private httpClient: HttpClient;
    private apiUrl: string;

    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this.apiUrl = ServiceHelper.GetLogitudeURL() + 'api/webhookkeysextended';
    }

    get(id: string) {
        var callTime = new Date();
        var url = this.apiUrl + '/getsingle?' + 'id=' + id;
        return defer(() => {
            return this.httpClient.get(url, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                if (response instanceof HttpResponse) {
                    var pm = response.body;
                    var entity: WebhookKeysPM;
                    if (pm) {
                        entity = this.MapJsonToEntityPM(pm);
                    }
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = entity;

                    var servertime = response.headers.get('ServerExecutionTime');
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "WebhookKeys", "GetSinglePM", 'id=' + id);

                    return serviceResponse;
                }
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    insert(entityPM: WebhookKeysPM) {
        var callTime = new Date();
        return defer(() => {
            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();
            var errorsArray = validator.Validate("WebhookKeys", entityPM);

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: WebhookKeysPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);
                var url = this.apiUrl + '/PostWebhookKeys';
                return this.httpClient.post(url, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                    if (response instanceof HttpResponse) {
                        var pm = response.body;
                        if (pm) {
                            var mappedResult: WebhookKeysPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }
                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "WebhookKeys", "SaveChanges", "");

                        return serviceResponse;

                    }
                }), catchError(ServiceHelper.HandleServiceError));
            }
            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);
            }
        });
    }

    update(entityPM: WebhookKeysPM)  {
        var callTime = new Date();
        return defer(() => {
            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();
            var errorsArray = validator.Validate("WebhookKeys", entityPM);

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: WebhookKeysPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this.httpClient.put(this.apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                    if (response instanceof HttpResponse) {
                        var pm = response.body;
                        if (pm) {
                            var mappedResult: WebhookKeysPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }

                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "WebhookKeys", "SaveChanges", "");


                        return serviceResponse;
                    }
                }), catchError(ServiceHelper.HandleServiceError));
            }
            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);
            }
        });
    }

    PushHookContent(DataToPush: any)  {
        return defer(() => {
            var errorsArray = [];
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            if (errorsArray.length == 0) {
                var url = this.apiUrl + '/PostPushHookContent';
                return this.httpClient.post(url, JSON.stringify(DataToPush), ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                    if (response instanceof HttpResponse) {
                        if (response.status == 200) {
                            serviceResponse.HasError = false;
                            serviceResponse.Result = "Data Pushed Successfully";
                        }
                        else {
                            serviceResponse.HasError = true;
                            serviceResponse.Result = "Data Didn't Pushed Successfully";
                        }

                        return serviceResponse;
                    }
                }), catchError(ServiceHelper.HandleServiceError));
            }
            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);
            }
        });
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: WebhookKeysPM = null) {
        if (!entityPM) {
            entityPM = new WebhookKeysPM();
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
        var entityPM: WebhookKeysPM;
        entityPM = new WebhookKeysPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }
}
