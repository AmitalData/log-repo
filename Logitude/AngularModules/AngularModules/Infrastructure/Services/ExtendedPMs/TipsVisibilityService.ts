import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { TipsVisibilityPM } from '../../EntityPMs/TipsVisibilityPM';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class TipsVisibilityService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TipsVisibility';
    }

    insert(entityPM: TipsVisibilityPM) {

        return defer(() => {
            var errorsArray = [];
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            if (errorsArray.length == 0) {
                var mappedEntity: TipsVisibilityPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                    var pm = response;
                        if (pm) {
                            var mappedResult: TipsVisibilityPM;
                            mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                            serviceResponse.Result = mappedResult;
                        }

                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
            }
            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);
            }
        });
    }

    update(entityPM: TipsVisibilityPM) {
        return defer(() => {
            var errorsArray = [];
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            if (errorsArray.length == 0) {
                var mappedEntity: TipsVisibilityPM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {
                    var pm = response;
                    if (pm) {
                        var mappedResult: TipsVisibilityPM;
                        mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                        serviceResponse.Result = mappedResult;
                    }

                    return serviceResponse;
                }), catchError(ServiceHelper.HandleServiceError));
            }
            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;

                return of(serviceResponse);
            }
        });
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: TipsVisibilityPM = null) {
        if (!entityPM) {
            entityPM = new TipsVisibilityPM();
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
        var entityPM: TipsVisibilityPM;
        entityPM = new TipsVisibilityPM();

        return entityPM;
    }

}
