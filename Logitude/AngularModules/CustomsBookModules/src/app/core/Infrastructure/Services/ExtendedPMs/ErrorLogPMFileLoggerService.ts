// FileLoggerService  1st check is 
//------------------------------------------------------------------------------
import { ClassLevelValidator } from '../../Validators/ClassLevelValidator';
import { HttpClient, HttpEvent, HttpResponse } from '@angular/common/http';
import { CustomFieldClass } from '../../DataContracts/CustomFieldClass';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';
import { InfraSettings } from '../../Utilities/InfraSettings';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { ErrorLogPM } from '../../EntityPMs/ErrorLogPM';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class ErrorLogPMFileLoggerService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/FileLogger';
    }

    get(id: string) {
        var url = this._apiUrl + '/GetSingle?' + 'appSettingKeyValueIsLogUntilDateyyyyMMdd=' + id;
        var callTime = new Date();
        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                if (response instanceof HttpResponse) {
                    var pm = response;
                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = pm;

                    var servertime = response.headers.get('ServerExecutionTime');
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ErrorLog", "GetSinglePM", 'id=' + id);

                    return serviceResponse;
                }
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    insert(entityPM: ErrorLogPM) {

        var callTime = new Date();
        return defer(() => {
            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();
            var errorsArray = validator.Validate("ErrorLog", entityPM);
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            if (errorsArray.length == 0) {
                var mappedEntity: ErrorLogPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                    if (response instanceof HttpResponse) {
                        var pm = response;
                        if (pm) {
                            var mappedResult: ErrorLogPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }
                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ErrorLog", "SaveChanges", "");

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

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ErrorLogPM = null) {
        if (!entityPM) {
            entityPM = new ErrorLogPM();
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
        var entityPM: ErrorLogPM;
        entityPM = new ErrorLogPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }
}
