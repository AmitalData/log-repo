import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass'
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { ReferantExceptionPM } from '../../EntityPMs/ReferantExceptionPM';

@Injectable()

export class ReferantExceptionExtendedPMService {
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReferantExceptionExtended';
    }

    Delete(declarationid: string, exceptionreasonscode: string) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            return this._http.delete(this._apiUrl + '/Delete/?' + '&declarationid=' + declarationid + '&exceptionreasonscode=' + exceptionreasonscode, ServiceHelper.GetHttpHeaders())
            .pipe(map((response:any) => {
                var myJsonResult = response.body;
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    GetByDecId(declarationId: string) {
        return defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            var _mappedListsArray: Array<ReferantExceptionPM> = [];
            return this._http.get(this._apiUrl + '/GetByDecId/?' + '&declarationid=' + declarationId, ServiceHelper.GetHttpHeaders()).pipe(map((response:ServiceResponse) => {
                var serviceResponse: ServiceResponse = response;
                var _mappedListsArray: Array<ReferantExceptionPM> = [];
                for (var key in serviceResponse) {
                    var entity: ReferantExceptionPM;
                    entity = this.MapJsonToEntityPM(serviceResponse[key]);
                    _mappedListsArray.push(entity);
                }

                serviceResponse.Result = _mappedListsArray;
                return serviceResponse;
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }
    
    MapJsonToEntityPM(jsonPM: any) {
        var entityPM: ReferantExceptionPM;
        entityPM = new ReferantExceptionPM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    }
}
