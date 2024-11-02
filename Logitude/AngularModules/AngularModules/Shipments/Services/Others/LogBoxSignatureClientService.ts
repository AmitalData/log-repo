import { Injectable } from '@angular/core';
import { defer, of } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { AgentSharedManifestPM } from '../../../Common/EntityPMs/AgentSharedManifestPM';
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';

@Injectable()

export class LogBoxSignatureClientService {
    private _httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/LogBoxSignatureClient';
    }
    
    GetSignRequestReceived(entityPM) {

        var callTime = new Date();
        return defer(() => {
            return this._httpClient.put(this._apiUrl, JSON.stringify(entityPM), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                //var pm = response;

               

                //var entity: AgentSharedManifestPM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}
                 
                var entity = response;
               
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();
                pmresponse.Result = entity;

                return pmresponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetMultiSignRequestReceived(Ids) {
        return defer(() => {

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


            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._httpClient.get(this._apiUrl + "/GetMultiSignRequestReceived/?" + IdsParameterString
                , ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                    //var res = response;

                    var entity = response;

                    var pmresponse: ServiceResponse;
                    pmresponse = new ServiceResponse();
                    pmresponse.Result = entity;

                    return pmresponse;
                }),catchError(ServiceHelper.HandleServiceError));
        }
        ); 
    }
}
