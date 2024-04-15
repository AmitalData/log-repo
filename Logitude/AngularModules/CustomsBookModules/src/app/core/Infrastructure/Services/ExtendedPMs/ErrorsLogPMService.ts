import { EntityPMServiceResponse } from '../../DataContracts/EntityPMServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { ErrorLogPM } from '../../EntityPMs/ErrorLogPM';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class ErrorsLogPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/errorlogs';
    }

    insert(entityPM: ErrorLogPM) {
        return defer(() => {
            var errorsArray = [];
            var entityPMServiceResponse: EntityPMServiceResponse;
            entityPMServiceResponse = new EntityPMServiceResponse();

            if (errorsArray.length == 0) {
                var mappedEntity: ErrorLogPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                    var pm = response
                    if (pm) {
                        var mappedResult: ErrorLogPM;
                        mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                        entityPMServiceResponse.Result = mappedResult;
                    }

                    return entityPMServiceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
            }
            else {
                entityPMServiceResponse.HasError = true;
                entityPMServiceResponse.ErrorsArray = errorsArray;

                return of(entityPMServiceResponse);
            }
        });
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ErrorLogPM = null) {
        if (!entityPM) {
            entityPM = new ErrorLogPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {

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

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
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

        return entityPM;
    }
}
