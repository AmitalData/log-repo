import { CustomFieldClass } from '../../DataContracts/CustomFieldClass';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ObjectFieldValidationPM } from '../../EntityPMs/ObjectFieldValidationPM';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { Guid } from '../../Utilities/Guid';
import { ObjectFieldPM } from '../../EntityPMs/ObjectFieldPM';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { defer } from 'rxjs';
import { ObjectTablePM } from 'Infrastructure/EntityPMs/ObjectTablePM';
import { PerformanceLogger } from 'Infrastructure/Utilities/PerformanceLogger';

@Injectable()
export class ObjectTablePMExtendedService {
    private _http: HttpClient;
    logitudeURL: string = null;
    private _apiUrl: string;
    baseMetaUrlApi: string = null;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this.logitudeURL = ServiceHelper.GetLogitudeURL();
        this._apiUrl = this.logitudeURL + 'api/ObjectTableExtended';
        this.baseMetaUrlApi = this.logitudeURL + "api/ngMetaData";
    }

    GetObjectTableByName(objectTableName: string, contextTenant: number) {


        return defer(() => {
            return this._http.get(this._apiUrl + '/GetObjectTableByName?' + 'objectTableName=' + objectTableName + '&contextTenant=' + contextTenant, ServiceHelper.GetHttpHeaders()).pipe(
                map((response: HttpResponse<any>) => {
                    var pm = response;

                    var entity: ObjectTablePM;
                    if (pm) {
                        entity = this.MapJsonToEntityPM(pm);
                    }

                    var serviceResponse: ServiceResponse = new ServiceResponse();
                    serviceResponse.Result = entity;
                    return serviceResponse;

                }),

                catchError(ServiceHelper.HandleServiceError));
        });
    }


     MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ObjectTablePM = null) {

         
        if (!entityPM) {
            
            entityPM = new ObjectTablePM();
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

    G
}
