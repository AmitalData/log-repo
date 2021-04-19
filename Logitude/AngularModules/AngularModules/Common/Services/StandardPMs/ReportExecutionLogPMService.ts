import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse'; 
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { ReportExecutionLogPM } from '../../EntityPMs/ReportExecutionLogPM';
import { ReportExecutionLogPMInitService } from '../../EntityPMInitServices/ReportExecutionLogPMInitService';
 
@Injectable()
export class ReportExecutionLogPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
         //ReportExecutionLogExtended
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/reportexecutionlogviews';
    }
    get(id: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {
                        var pm = response.body;
                        var entity: ReportExecutionLogPM;
                        if (pm) {
                            entity = this.MapJsonToEntityPM(pm);
                            ReportExecutionLogPMInitService.ApplyUIPoperties(entity, false);
                        }
                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;
                        return serviceResponse;
                    }), catchError(ServiceHelper.HandleServiceError));
        });
    }
    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: ReportExecutionLogPM = null) {
        if (!entityPM) {
            entityPM = new ReportExecutionLogPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        if (mapParent) {
            entityPM.OldEntityPM = this.clone(entityPM);
        }
        else {
            entityPM.OldEntityPM = null;
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
}
