import {Injectable} from '@angular/core';
import { HttpClient, HttpEvent, HttpResponse } from '@angular/common/http';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {CustomFieldClass} from '../../../Infrastructure/DataContracts/CustomFieldClass'
import {PerformanceLogger} from '../../../Infrastructure/Utilities/PerformanceLogger';
import {AWBSpecialHandlingCodePM} from '../../EntityPMs/AWBSpecialHandlingCodePM';
import { catchError, map } from 'rxjs/operators';

@Injectable()

export class AWBSpecialHandlingCodeExtendedPMService {
    private _httpClient: HttpClient
    private _apiUrl: string;
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/awbspecialhandlingcodesextended';
    }

    get(id: string) {

        var callTime = new Date();
        return defer(() => {
            return this._httpClient.get(this._apiUrl + '/getsingle?' + 'id=' + id, ServiceHelper.GetHttpHeaders()).pipe(
                map((response) => {

                    //if (response instanceof HttpResponse) {

                        var pm = response;
                        var entity: AWBSpecialHandlingCodePM;
                        if (pm) {
                            entity = this.MapJsonToEntityPM(pm);
                        }

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;

                        //var servertime = response.headers.get('ServerExecutionTime');
                        //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "AWBSpecialHandlingCode", "GetSinglePM", 'id=' + id);

                        return serviceResponse;
                    //}
                })

                , catchError(ServiceHelper.HandleServiceError));
        });
    }

    insert(entityPM: AWBSpecialHandlingCodePM) {
        var callTime = new Date();
        return defer(() => {

            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();
            var errorsArray = validator.Validate("AWBSpecialHandlingCode", entityPM);

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: AWBSpecialHandlingCodePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._httpClient.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(
                    map((response) => {
                        //if (response instanceof HttpResponse) {
                            var pm = response;
                            if (pm) {
                                var mappedResult: AWBSpecialHandlingCodePM;
                                mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }

                            //var servertime = response.headers.get('ServerExecutionTime');
                            //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "AWBSpecialHandlingCode", "SaveChanges", "");
                            return serviceResponse;
                        //}
                    }), catchError(ServiceHelper.HandleServiceError));
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);
            }
        });
    }

    update(entityPM: AWBSpecialHandlingCodePM) {
        var callTime = new Date();
        return defer(() => {

            var validator: ClassLevelValidator;
            validator = new ClassLevelValidator();
            var errorsArray = validator.Validate("AWBSpecialHandlingCode", entityPM);

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: AWBSpecialHandlingCodePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._httpClient.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(
                    map((response) => {
                        //if (response instanceof HttpResponse) {
                            var pm = response;
                            if (pm) {
                                var mappedResult: AWBSpecialHandlingCodePM;
                                mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }

                            //var servertime = response.headers.get('ServerExecutionTime');
                            //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "AWBSpecialHandlingCode", "SaveChanges", "");

                            return serviceResponse;
                        //}
                    }),catchError(ServiceHelper.HandleServiceError));
            }
            else {

                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;
                return of(serviceResponse);
            }
        });
    }
    
    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: AWBSpecialHandlingCodePM = null) {
        if (!entityPM) {

            entityPM = new AWBSpecialHandlingCodePM();
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
        var entityPM: AWBSpecialHandlingCodePM;
        entityPM = new AWBSpecialHandlingCodePM();
        return entityPM;
    }
}
