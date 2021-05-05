import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass'
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';

import { GLAccountFollowUpDataPM } from '../../EntityPMs/GLAccountFollowUpDataPM';


	export class GLaccountFollowUpDataExtendedPMService
    {
        private _http: HttpClient;
        private _apiUrl: string;
        constructor() {
            this._http = ServiceHelper.HttpClient;
            this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/GLAccountFollowUpDataExtended';
        }


        getByAccountId(accountId: string) {

            var callTime = new Date();

            return defer(() => {
                return this._http.get(this._apiUrl + '/getsingleByAccountId?' + 'accountId=' + accountId, ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {
                            var pm = response.body;

                            var entity: GLAccountFollowUpDataPM;
                            if (pm) {
                                entity = this.MapJsonToEntityPM(pm);
                            }

                            var serviceResponse: ServiceResponse = new ServiceResponse();
                            serviceResponse.Result = entity;

                            var servertime = response.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "GLAccountFollowUpData", "getsingleByAccountId", 'accountId=' + accountId);

                            return serviceResponse;

                        }),

                        catchError(ServiceHelper.HandleServiceError));
            });
        }
        MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: GLAccountFollowUpDataPM = null) {


            if (!entityPM) {

                entityPM = new GLAccountFollowUpDataPM();
                entityPM.DisableMarkAsDirty = true;
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
            entityPM.DisableMarkAsDirty = false;

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

