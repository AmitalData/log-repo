import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { CustomFieldClass } from '../../../Infrastructure/DataContracts/CustomFieldClass'
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';

import { ReferantExceptionPM } from '../../EntityPMs/ReferantExceptionPM';

@Injectable()

export class ReferantExceptionExtendedPMService {
    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ReferantExceptionExtended';
    }

    Delete(declarationid: string, exceptionreasonscode: string) {
        return Observable.defer(() => {
            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');
            return this._http.delete(this._apiUrl + '/Delete/?' + '&declarationid=' + declarationid + '&exceptionreasonscode=' + exceptionreasonscode, { headers: authHeader }).map(response => {
                var myJsonResult = response.json();
                var serviceResponse = new ServiceResponse();
                serviceResponse.Result = myJsonResult;
                return serviceResponse;
            }).catch(ServiceHelper.HandleServiceError);
        });
    }
}
