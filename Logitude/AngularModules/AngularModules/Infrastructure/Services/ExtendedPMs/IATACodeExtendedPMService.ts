import { HttpClient, HttpEvent, HttpResponse } from '@angular/common/http';
import { ClassLevelValidator } from '../../Validators/ClassLevelValidator';
import { CustomFieldClass } from '../../DataContracts/CustomFieldClass';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { IATACodePM } from '../../EntityPMs/IATACodePM';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';

@Injectable()
export class IATACodeExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/iatacodesextended';
    }

    get(id: string) {
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                if (response instanceof HttpResponse) {
                    var pm = response;
                    var entity: IATACodePM;

                    if (pm) {
                        entity = this.MapJsonToEntityPM(pm);
                    }

                    var serviceResponse: ServiceResponse;
                    serviceResponse = new ServiceResponse();
                    serviceResponse.Result = entity;

                    var servertime = response.headers.get('ServerExecutionTime');
                    PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "IATACode", "GetSinglePM", 'id=' + id);

                    return serviceResponse;
                }
            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    insert(entityPM: IATACodePM) {
        var callTime = new Date();
        return defer(() => {
            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();
            var errorsArray = validator.Validate("IATACode", entityPM);
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            if (errorsArray.length == 0) {
                var mappedEntity: IATACodePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                    if (response instanceof HttpResponse) {
                        var pm = response;
                        if (pm) {
                            var mappedResult: IATACodePM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }

                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "IATACode", "SaveChanges", "");

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

    update(entityPM: IATACodePM) {
        var callTime = new Date();
        return defer(() => {
            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();
            var errorsArray = validator.Validate("IATACode", entityPM);
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            if (errorsArray.length == 0) {
                var mappedEntity: IATACodePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders()).pipe(map((response: HttpEvent<any>) => {
                    if (response instanceof HttpResponse) {
                        var pm = response;
                        if (pm) {
                            var mappedResult: IATACodePM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }

                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "IATACode", "SaveChanges", "");

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

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: IATACodePM = null) {
        if (!entityPM) {
            entityPM = new IATACodePM();
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
        var entityPM: IATACodePM;
        entityPM = new IATACodePM();
        return entityPM;
    }
}
