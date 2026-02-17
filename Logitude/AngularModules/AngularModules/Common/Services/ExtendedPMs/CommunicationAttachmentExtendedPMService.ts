import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';


import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';

import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CommunicationAttachmentPM} from '../../EntityPMs/CommunicationAttachmentPM';

@Injectable() 
export class CommunicationAttachmentExtendedPMService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/CommunicationAttachmentExtended';
    }



    getCommunicationAttachmentsByTenant(tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?tenant=' + tenant , { headers: authHeader }).map(response => {
            var result = response.json();
            var entity: CommunicationAttachmentPM;
            var communicationAttachmentPMLists: CommunicationAttachmentPM[];
            communicationAttachmentPMLists = new Array<CommunicationAttachmentPM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                communicationAttachmentPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = communicationAttachmentPMLists;
            return pmresponse;




        }).catch(ServiceHelper.HandleServiceError);
    }


    getCommunicationAttachmentsByCommunicationLogId(communicationLogId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '?communicationLogId=' + communicationLogId + "&tenant="+ tenant, { headers: authHeader }).map(response => {
            var result = response.json();
            var entity: CommunicationAttachmentPM;
            var communicationAttachmentPMLists: CommunicationAttachmentPM[];
            communicationAttachmentPMLists = new Array<CommunicationAttachmentPM>();


            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                communicationAttachmentPMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = communicationAttachmentPMLists;
            return pmresponse;




        }).catch(ServiceHelper.HandleServiceError);
    }




    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: CommunicationAttachmentPM;
        entityPM = new CommunicationAttachmentPM(null);
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }






}

