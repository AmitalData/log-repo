
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

import {AutomationPM} from '../../EntityPMs/AutomationPMExtended';
import { AgentSharedLogisticsKey } from '../../EntityPMs/AgentSharedLogisticsKey';



@Injectable()
export class AgentSharedLogisticsKeyPMService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AgentSharedLogisticsKeys';
    }



    GetSingle(key: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + "/GetSingle" + '?key=' + key , { headers: authHeader }).map(response => {

            var result = response.json();

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;




        }).catch(ServiceHelper.HandleServiceError);
    }

    GetSingleByAgentId(agentId: string) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + "/GetSingleByAgentId" + '?agentId=' + agentId + "&tenant=" + SessionInfo.LoggedUserTenant, { headers: authHeader }).map(response => {

            var result = response.json();

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;




        }).catch(ServiceHelper.HandleServiceError);
    }

    

    SendAgentInvitaion(agentId: string, invitedEmail: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


        //return this._http.post(this._apiUrl, JSON.stringify(mappedEntity),
        //    { headers: authHeader }).map((response) => {

        //    });string agentId, string invitedEmail, int tenant
        return this._http.post(this._apiUrl + "/PostAgentSharedLogisticsKeyInvitation" + '?agentId=' + agentId + '&invitedEmail=' + invitedEmail + "&tenant=" + tenant, "", { headers: authHeader }).map(response => {

            var result = response.json();

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();
            pmresponse.Result = result;
            return pmresponse;




        }).catch(ServiceHelper.HandleServiceError);
    }

 
    update(entity: AgentSharedLogisticsKey, updatedByAgentId: string, isAccepted:boolean) {


        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');



            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();



            return this._http.put(this._apiUrl + "?updatedByAgentId=" + updatedByAgentId + '&isAccepted=' + isAccepted , JSON.stringify(entity),
                { headers: authHeader }).map((response) => {

                    serviceResponse.Result = response.json();
                    return serviceResponse;

                }).catch(ServiceHelper.HandleServiceError);

        });
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: AutomationPM = null) {


        if (!entityPM) {

            entityPM = new AutomationPM();
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

