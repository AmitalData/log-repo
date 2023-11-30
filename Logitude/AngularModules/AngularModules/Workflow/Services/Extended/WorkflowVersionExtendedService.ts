import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';
import { WorkFlowInstanceActivityList } from '../../EntityLists/WorkFlowInstanceActivityList';
import { WorkFlowInstanceVariableList } from 'Workflow/EntityLists/WorkFlowInstanceVariableList';
import { WorkFlowVersionPM } from 'Workflow/EntityPMs/WorkFlowVersionPM';
import { CustomFieldClass } from 'Infrastructure/DataContracts/CustomFieldClass';

@Injectable()

export class WorkflowVersionExtendedService {
    private _http: HttpClient;
    private _apiUrl: string;

    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/WorkflowVersionExtended';
    }

    putActivate(workFlowVersionId: string) {
        var callTime = new Date();
        
        return defer(() => {
			var serviceResponse: ServiceResponse = new ServiceResponse();

            return this._http.put(this._apiUrl + '/PutActivate?WorkFlowVersionId=' + workFlowVersionId, null,ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var pm = response.body;
                        if (pm) {
                            var mappedResult: WorkFlowVersionPM = this.MapJsonToEntityPM(pm, true);
                            serviceResponse.Result = mappedResult;
                        }

                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "WorkflowVersionExtended", "PutActivate", 'workFlowVersionId=' + workFlowVersionId);

                        return serviceResponse;
                    }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: WorkFlowVersionPM = null) {


        if (!entityPM) {

            entityPM = new WorkFlowVersionPM();
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