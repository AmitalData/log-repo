import {Injectable} from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../Validators/ClassLevelValidator';
import {Guid} from '../../Utilities/Guid';
import {InfraSettings} from '../../Utilities/InfraSettings';
import {ServiceHelper} from '../../Utilities/ServiceHelper';
import {SessionInfo} from '../../Utilities/SessionInfo';
import {PerformanceLogger} from '../../Utilities/PerformanceLogger';
import {CustomFieldClass} from '../../DataContracts/CustomFieldClass'

import {AnalyzeQueuePM} from '../../EntityPMs/AnalyzeQueuePM';


@Injectable()

export class AnalyzeQueuePMService {
 private _http: HttpClient;
 private _apiUrl: string;
 constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/analyzequeues';      
    }

    get(id: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var pm = response.body;



                        var entity: AnalyzeQueuePM;
                        if (pm) {
                            entity = this.MapJsonToEntityPM(pm);
                        }

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;

                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "AnalyzeQueue", "GetSinglePM", 'id=' + id);

                        return serviceResponse;

                    }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    insert(entityPM: AnalyzeQueuePM) {

        var callTime = new Date();
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var validator: ClassLevelValidator;

            validator = new ClassLevelValidator();

            var errorsArray = validator.Validate("AnalyzeQueue", entityPM);


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();
            if (errorsArray.length == 0) {
                var mappedEntity: AnalyzeQueuePM;
                mappedEntity = this.MapJsonToEntityPM(entityPM, false);

                return this._http.post(this._apiUrl, JSON.stringify(mappedEntity), ServiceHelper.GetHttpFullHeaders())
                    .pipe(
                        map((response: HttpResponse<any>) => {

                            var pm = response.body;
                            if (pm) {
                                var mappedResult: AnalyzeQueuePM;
                                mappedResult = this.MapJsonToEntityPM(pm, true, entityPM);
                                serviceResponse.Result = mappedResult;
                            }


                            var servertime = response.headers.get('ServerExecutionTime');
                            PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "AnalyzeQueue", "SaveChanges", "");


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

    

   

	  MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: AnalyzeQueuePM = null) {

         
        if (!entityPM) {
            
            entityPM = new AnalyzeQueuePM();
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
				
			  if(customFields.indexOf(property) > -1)
                {
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
		    var entityPM: AnalyzeQueuePM;
			entityPM = new AnalyzeQueuePM();
			entityPM.Tenant = InfraSettings.TenantPM.Id;
			return entityPM;
    }
		 

}
