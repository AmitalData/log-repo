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
export class SharedAgentManifestService {


    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SharedAgentManifest';
    }



    getSharedAgentManifestTransLateIdByCode(code: string, tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/getSharedAgentManifestTransLateIdByCode?' + 'code=' + code  + '&tenant=' + tenant, {
                headers: authHeader
            }).map(response => {
                var result = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetCheckIfAnyShipmentHaveMasterNumber(master: string, longMaster: string,  tenant: number) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCheckIfAnyShipmentHaveMasterNumber?' + 'master=' + master + '&longMaster=' + longMaster+ '&tenant=' + tenant, {
                headers: authHeader
            }).map(response => {
                var result = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }


    GetCheckIfMasterShipmentHaveHouseWithOtherAgent(entityId: string, agentId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetCheckIfMasterShipmentHaveHouseWithOtherAgent?' + 'entityId=' + entityId + '&agentId=' + agentId + '&tenant=' + tenant, {
                headers: authHeader
            }).map(response => {
                var result = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    public ShareAgentManifest(shipmentId: string, isUpdateAgent: boolean = false) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetSharedAgentManifest?' + 'shipmentId=' + shipmentId + '&isUpdateAgent=' + isUpdateAgent + '&tenant=' + SessionInfo.LoggedUserTenant, {
                headers: authHeader
            }).map(response => {

                var pm = response.json();
                var entity: AgentSharedManifestPM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;

                return response.json();
            }).catch(ServiceHelper.HandleServiceError);
        }

        );
    }



    getAgentSharedManifesRefShipmentListsByIds(agentManifestSharedRefListIds: any) {

        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        authHeader.append('Content-Type', 'application/json');
        return Observable.defer(() => {
            return this._http.post(this._apiUrl + '/postagentsharedmanifesrefshipmentListsbyids', JSON.stringify(agentManifestSharedRefListIds), {

                headers: authHeader,

            }).map(response => {
                var result = response.json();
             
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;
            }).catch(ServiceHelper.HandleServiceError);
        }

        );

    }


    getAgentSharedManifestsWorkspaceSummary() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());

        return this._http.get(this._apiUrl + '/GetAgentSharedManifestsWorkspaceSummary', { headers: authHeader }).map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response.json();
            return pmresponse;
        }).catch(ServiceHelper.HandleServiceError);
    }

    GetIsAgentSharedManifests(agentId: string, entityid: string) {
        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetIsAgentSharedManifests?' + 'agentId=' + agentId + '&entityid=' + entityid , {
                headers: authHeader
            }).map(response => {
                var result = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
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



                var entity: AgentSharedManifestPM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;

                var servertime = response.headers.get('ServerExecutionTime');
                PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "AgentSharedManifest", "GetSinglePM", 'id=' + id);

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    GetAgentSharedManifestsForDashBoard(lastMonths: number, lastDays: number, selectedIndex: number) {


        var authHeader = new Headers();
        authHeader.append('Token', SessionInfo.Token);
        var callTime = new Date();
        return Observable.defer(() => {
            return this._http.get(this._apiUrl + '/GetAgentSharedManifestsForDashBoard?' + 'lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&selectedIndex=' + selectedIndex, {
                headers: authHeader
            }).map(response => {
                var result = response.json();
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;

                return serviceResponse;

            }).catch(ServiceHelper.HandleServiceError);
        });
    }

    MapJsonToEntityPM(jsonPM: any, mapParent: boolean = true, entityPM: AgentSharedManifestPM = null) {


        if (!entityPM) {

            entityPM = new AgentSharedManifestPM();
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

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM" || jsonPMKeys[key] === "PropertyChanged") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }



}