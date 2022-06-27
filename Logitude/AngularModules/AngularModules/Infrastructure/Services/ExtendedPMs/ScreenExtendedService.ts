
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../Validators/ClassLevelValidator';
import { Guid } from '../../Utilities/Guid';
import { InfraSettings } from '../../Utilities/InfraSettings';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';
import { CustomFieldClass } from '../../DataContracts/CustomFieldClass'

import { ScreenPM } from '../../EntityPMs/ScreenPM';




@Injectable()

export class ScreenExtendedService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ScreenExtended';
    }

    get(id: string) {

        var callTime = new Date();

        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var pm = response.body;

                        var entity: ScreenPM;
                        if (pm) {
                            entity = this.MapJsonToEntityPM(pm);
                        }

                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;

                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Screens", "GetSinglePM", 'id=' + id);

                        return serviceResponse;

                    }),

                    catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetEntityScreens(entityId: string) {
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetEntityScreens?' + 'entityId=' + entityId, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var entities = response.body;


                        return entities;
                    }),
                    catchError(ServiceHelper.HandleServiceError));
        });
    }

    insert(entityPM: ScreenPM) {

        var callTime = new Date();

        return defer(() => {

            var serviceResponse: ServiceResponse = new ServiceResponse();


                var mappedEntity: ScreenPM = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {

                            var pm = response.body;
                            if (pm) {
                                var mappedResult: ScreenPM = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }

                            var servertime = response.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Screens", "SaveChanges", "");

                            return serviceResponse;
                        }),

                        catchError(ServiceHelper.HandleServiceError));

        });
    }


    update(entityPM: ScreenPM) {

        var callTime = new Date();

        return defer(() => {

            var serviceResponse: ServiceResponse = new ServiceResponse();

                var mappedEntity: ScreenPM = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {

                            var pm = response.body;
                            if (pm) {
                                var mappedResult: ScreenPM = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }

                            var servertime = response.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "Screens", "SaveChanges", "");

                            return serviceResponse;
                        }),

                        catchError(ServiceHelper.HandleServiceError));

        });
    }



    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ScreenPM = null) {


        if (!entityPM) {

            entityPM = new ScreenPM();
        }


        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "PropertyChanged") {

                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


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
        var entityPM: ScreenPM;
        entityPM = new ScreenPM();
        entityPM.Tenant = InfraSettings.TenantPM.Id;
        return entityPM;
    }
}
