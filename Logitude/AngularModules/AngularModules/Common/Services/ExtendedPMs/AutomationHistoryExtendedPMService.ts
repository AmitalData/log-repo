
import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

import {AutomationHistoryPM} from '../../EntityPMs/AutomationHistoryPM';



@Injectable()
export class AutomationHistoryExtendedPMService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AutomationHistoryExtended';
    }


    getAutomationHistoryesByAutomationId(automationId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/getautomationhistoryesbyautomationId" + '?automationId=' + automationId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result :any = response;

         var pmresponse: ServiceResponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;




        }),catchError(ServiceHelper.HandleServiceError));
    }


    getAutomationBackupDataByAutomationId(automationId: string, version: number, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/getautomationbackupdatabyautomationid" + '?automationId=' + automationId + '&version=' + version+ '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result :any = response;

            var pmresponse: ServiceResponse = new ServiceResponse();

            pmresponse.Result = result;
            return pmresponse;




        }),catchError(ServiceHelper.HandleServiceError));
    }

    


    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: AutomationHistoryPM;
        entityPM = new AutomationHistoryPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }


}

