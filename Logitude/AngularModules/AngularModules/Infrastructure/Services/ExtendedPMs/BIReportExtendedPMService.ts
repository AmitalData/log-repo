import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { BIReportPM } from '../../EntityPMs/BIReportPM';
import { of, defer } from 'rxjs';
import { PerformanceLogger } from '../../Utilities/PerformanceLogger';
import { ClassLevelValidator } from '../../Validators/ClassLevelValidator';
import { CustomFieldClass } from '../../DataContracts/CustomFieldClass';

@Injectable()
export class BIReportExtendedPMService {
    private httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this.httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/BIReportsExtended';
    }

    DoesReportExist(name: string, folderId: string)  {
        var url = this._apiUrl + '/getReportExist?' + 'name=' + name + '&folderId=' + folderId;

        return this.httpClient.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            serviceResponse.Result = response;

            return serviceResponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    UpdateWithoutAGGridXML(entityPM: BIReportPM) {

        var callTime = new Date();

        return defer(() => {

            var serviceResponse: ServiceResponse = new ServiceResponse();
            var validator: ClassLevelValidator = new ClassLevelValidator();
            var errorsArray = validator.Validate("BIReport", entityPM);


            if (errorsArray.length == 0) {

                var mappedEntity: BIReportPM = this.MapJsonToEntityPM(entityPM, false);

                return this.httpClient.put(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {

                            var pm = response.body;
                            if (pm) {
                                var mappedResult: BIReportPM = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }

                            var servertime = response.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "BIReport", "SaveChanges", "");

                            return serviceResponse;
                        }),

                        catchError(ServiceHelper.HandleServiceError));
            }

            else {
                serviceResponse.HasError = true;
                serviceResponse.ErrorsArray = errorsArray;
                return of(serviceResponse);
            }
        });
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: BIReportPM = null) {


        if (!entityPM) {

            entityPM = new BIReportPM();
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
}
