import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';


import { defer, of } from 'rxjs';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CommunicationLogPM} from '../../EntityPMs/CommunicationLogPM';

@Injectable()
export class CommunicationLogExtendedPMService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() { 


        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CommunicationLogExtended';
    }



    getCommunicationLogPMsByEntityId(entityId: string, tenant: number){
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());


        return this._http.get(this._apiUrl + '/getcommunicationlogpmsbyentityId/?' + 'entityId=' + entityId + '&tenant=' + tenant ,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
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




        }),catchError(ServiceHelper.HandleServiceError));
    }



   getCommunicationLogPMsByEntityIdAndDocumentOutId(entityId: string, documentOutId: string,  tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + '/getcommunicationlogpmsbyentityidanddocumentoutid/?' + 'entityId=' + entityId + '&documentOutId=' + documentOutId +'&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result :any = response;
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




        }),catchError(ServiceHelper.HandleServiceError));
    }




   SendCommunicationLogToQueue(communicationLogId: string, tenant: number) {
       var authHeader = new Headers();
       authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
     
       return this._http.get(this._apiUrl + '/getsendcommunicationlogtoqueue/?' + 'communicationLogId=' + communicationLogId + '&tenant=' + tenant,ServiceHelper.GetHttpHeaders()).pipe(map(response => {
           var result :any = response;

           var pmresponse: ServiceResponse;
           pmresponse = new ServiceResponse();

           pmresponse.Result = result;
           return pmresponse;


       }),catchError(ServiceHelper.HandleServiceError));
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

