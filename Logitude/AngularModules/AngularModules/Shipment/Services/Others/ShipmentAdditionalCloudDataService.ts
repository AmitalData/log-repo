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
export class ShipmentAdditionalCloudDataService {


    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentAdditionalCloudData';
    }
    
    get(id: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getsingle?' + 'id=' + id, {
                headers: authHeader
            }).map(response => {
                var pm = response.json();



                //var entity: AgentSharedManifestPM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = pm;

                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ShipmentAdditionalCloudData", "GetSinglePM", 'id=' + id);

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    getsingledata(id: string) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleData?' + 'id=' + id, {
                headers: authHeader
            }).map(response => {
                var pm = response.json();



                //var entity: AgentSharedManifestPM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = pm;

                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ShipmentAdditionalCloudData", "GetSinglePM", 'id=' + id);

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    update(entityPM: any) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var response: ServiceResponse;
            response = new ServiceResponse();
            //errorsArray = [];
           
            
                var shipString: string;

                shipString = JSON.stringify(entityPM);
                //console.log(shipString);
                return this._http.put(this._apiUrl, shipString,
                    { headers: authHeader }).map((res) => {
                        var pm = res.json();
                        response.Result = pm;
                        return response;

                    }).catch(ServiceHelper.HandleServiceError);
          
        });
    }

    updateUserID(entityPM: any) {
        return Observable.defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
            authHeader.append('Content-Type', 'application/json');
            var response: ServiceResponse;
            response = new ServiceResponse();
            //errorsArray = [];


            var shipString: string;

            shipString = JSON.stringify(entityPM);
            //console.log(shipString);
            return this._http.put(this._apiUrl + '/PutUserId', shipString,
                { headers: authHeader }).map((res) => {
                    var pm = res.json();
                    response.Result = pm;
                    return response;

                }).catch(ServiceHelper.HandleServiceError);

        });
    }

    getSingleWithoutToken(id: string, Tenant: number) {


        var authHeader = new Headers();
        //authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSingleWithoutToken?' + 'securityId=' + id + '&tenant=' + Tenant, {
                headers: authHeader
            }).map(response => {
                var pm = response.json();



                //var entity: AgentSharedManifestPM;
                //if (pm) {
                //    entity = this.MapJsonToEntityPM(pm);
                //}

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = pm;

                var servertime = response.headers.get('ServerExecutionTime');
                //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ShipmentAdditionalCloudData", "GetSinglePM", 'id=' + id);

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    //MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: AgentSharedManifestPM = null) {


    //    if (!entityPM) {

    //        entityPM = new AgentSharedManifestPM();
    //    }

    //    var jsonPMKeys = Object.keys(jsonPM);

    //    for (var key in jsonPMKeys) {
    //        if (jsonPMKeys[key] === "UIProperties") {

    //            continue;
    //        }
    //        var property = jsonPMKeys[key];
    //        entityPM[property] = jsonPM[property];
    //    }


    //    entityPM.IsDirty = false;

    //    if (mapParent) {
    //        entityPM.OldEntityPM = this.clone(entityPM);

    //    }
    //    else {

    //        entityPM.OldEntityPM = null;
    //    }

    //    return entityPM;
    //}


    //public clone(jsonPM: any) {
    //    var entityPM: any;
    //    entityPM = {};

    //    var jsonPMKeys = Object.keys(jsonPM);
    //    for (var key in jsonPMKeys) {

    //        if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
    //            continue;
    //        }

    //        var property = jsonPMKeys[key];
    //        entityPM[property] = jsonPM[property];

    //    }
    //    return entityPM;
    //}



}
