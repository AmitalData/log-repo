/// <reference path="../../../common/entitypms/agentsharedmanifestpm.ts" />
import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { AgentSharedManifestPM } from '../../../Common/EntityPMs/AgentSharedManifestPM';
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';

@Injectable()
export class LogBoxSignatureClientService {


    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/LogBoxSignatureClient';
    }
    
    GetSignRequestReceived(entityPM) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        authHeader.append('Content-Type', 'application/json');
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.put(this._apiUrl, JSON.stringify(entityPM),
                { //.get(this._apiUrl + '/PutSignRequestReceived', {
                headers: authHeader
            }).map(response => {
                //var pm = response.json();

               

                //var entity: AgentSharedManifestPM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}
                 
                var entity = response.json();
               
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = entity;

                return pmresponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetMultiSignRequestReceived(Ids) {
        return Observable.defer(() => {

            // Prepare parameters
            var IdsParameterString = "";
            if (Ids && Ids.length > 0) {
                Ids.forEach(el => {
                    IdsParameterString += 'Ids=' + el + '&';
                });

            } else {
                console.log("[ERROR] cannot Archive Shipments without Ids!", Ids);
                return;
            }

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetMultiSignRequestReceived/?" + IdsParameterString
                , { headers: authHeader }).map(response => {

                    //var res = response.json();

                    var entity = response.json();

                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();
                    pmresponse.Result = entity;

                    return pmresponse;
                }).catch(ServiceHelper.HandleServiceError);
        }
        ); 
    }
}