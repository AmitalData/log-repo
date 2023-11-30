
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { InfraSettings } from '../../Utilities/InfraSettings';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';

import { ScreenSectionPM } from '../../EntityPMs/ScreenSectionPM';




@Injectable()

export class ScreenSectionExtendedService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ScreenSectionExtended';
    }



    BuildServiceResponse(result:any) {
        var entity: ScreenSectionPM;
        var screenSections: ScreenSectionPM[];
        screenSections = new Array<ScreenSectionPM>();

        result.forEach((item) => {
            entity = this.MapJsonToEntityPM(item);
            screenSections.push(entity);
        });

        var pmresponse: ServiceResponse = new ServiceResponse();
        pmresponse.Result = screenSections;
        return pmresponse;
    }


    GetByScreenCode(screenCode: string) {

        var callTime = new Date();

        return defer(() => {
            return this._http.get(this._apiUrl + '/GetAllByScreenCode?' + 'screenCode=' + screenCode, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any[]>) => {
                        return this.BuildServiceResponse(response.body);
                    }),

                    catchError(ServiceHelper.HandleServiceError));
        });
    }


    update(screenSections: ScreenSectionPM[]) {

        var callTime = new Date();

        return defer(() => {

            var serviceResponse: ServiceResponse = new ServiceResponse();
            return this._http.put(this._apiUrl, JSON.stringify(screenSections), ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any[]>) => {
                        return this.BuildServiceResponse(response.body);
                    }),

                    catchError(ServiceHelper.HandleServiceError));

        });
    }


    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ScreenSectionPM = null) {


        if (!entityPM) {

            entityPM = new ScreenSectionPM();
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

  
}
