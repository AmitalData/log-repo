import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';


import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CommunicationLogPM} from '../../EntityPMs/CommunicationLogPM';

@Injectable()
export class CommunicationLogExtendedPMService {

    private _http: Http;
    private _apiUrl: string;
    constructor() { 


        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CommunicationLogExtended';
    }



    getCommunicationLogPMsByEntityId(entityId: string, tenant: number){
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


        return this._http.get(this._apiUrl + '/getcommunicationlogpmsbyentityId/?' + 'entityId=' + entityId + '&tenant=' + tenant , { headers: authHeader }).map(response => {
            var result = response.json();
            var entity: CommunicationLogPM;
            var communicationLogPMLists: CommunicationLogPM[];
            communicationLogPMLists = new Array<CommunicationLogPM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                communicationLogPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = communicationLogPMLists;
            return pmresponse;




        }).catch(ServiceHelper.HandleServiceError);
    }



   getCommunicationLogPMsByEntityIdAndDocumentOutId(entityId: string, documentOutId: string,  tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getcommunicationlogpmsbyentityidanddocumentoutid/?' + 'entityId=' + entityId + '&documentOutId=' + documentOutId +'&tenant=' + tenant, { headers: authHeader }).map(response => {
            var result = response.json();
            var entity: CommunicationLogPM;
            var communicationLogPMLists: CommunicationLogPM[];
            communicationLogPMLists = new Array<CommunicationLogPM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                communicationLogPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = communicationLogPMLists;
            return pmresponse;




        }).catch(ServiceHelper.HandleServiceError);
    }




   SendCommunicationLogToQueue(communicationLogId: string, tenant: number) {
       var authHeader = new Headers();
       authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
     
       return this._http.get(this._apiUrl + '/getsendcommunicationlogtoqueue/?' + 'communicationLogId=' + communicationLogId + '&tenant=' + tenant, { headers: authHeader }).map(response => {
           var result = response.json();

           var pmresponse: ServiceResponse;
           pmresponse = new ServiceResponse();

           pmresponse.Result = result;
           return pmresponse;


       }).catch(ServiceHelper.HandleServiceError);
   }


    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CommunicationLogPM;
        entityPM = new CommunicationLogPM();
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }






}

