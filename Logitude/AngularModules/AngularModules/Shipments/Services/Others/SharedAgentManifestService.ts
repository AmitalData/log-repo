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
import { HttpClient, HttpEvent, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';

@Injectable()

export class SharedAgentManifestService {
    private _httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/SharedAgentManifest';
    }



    getSharedAgentManifestTransLateIdByCode(code: string, tenant: number) {

        var callTime = new Date();
        return defer(() => {
            return this._httpClient.get(this._apiUrl + '/getSharedAgentManifestTransLateIdByCode?' + 'code=' + code + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetCheckIfAnyShipmentHaveMasterNumber(master: string, longMaster: string,  tenant: number) {


        var callTime = new Date();
        return defer(() => {
            return this._httpClient.get(this._apiUrl + '/GetCheckIfAnyShipmentHaveMasterNumber?' + 'master=' + master + '&longMaster=' + longMaster + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }


    GetCheckIfMasterShipmentHaveHouseWithOtherAgent(entityId: string, agentId: string, tenant: number) {

        var callTime = new Date();
        return defer(() => {
            return this._httpClient.get(this._apiUrl + '/GetCheckIfMasterShipmentHaveHouseWithOtherAgent?' + 'entityId=' + entityId + '&agentId=' + agentId + '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    public ShareAgentManifest(shipmentId: string, isUpdateAgent: boolean = false) {


        return defer(() => {
            return this._httpClient.get(this._apiUrl + '/GetSharedAgentManifest?' + 'shipmentId=' + shipmentId + '&isUpdateAgent=' + isUpdateAgent + '&tenant=' + SessionInfo.LoggedUserTenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var pm = response;
                var entity: AgentSharedManifestPM;
                if (pm) {
                    entity = this.MapJsonToEntityPM(pm);
                }

                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = entity;

                return response;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );
    }



    getAgentSharedManifesRefShipmentListsByIds(agentManifestSharedRefListIds: any) {


        return defer(() => {
            return this._httpClient.post(this._apiUrl + '/postagentsharedmanifesrefshipmentListsbyids', JSON.stringify(agentManifestSharedRefListIds), ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
             
                var pmresponse: ServiceResponse;
                pmresponse = new ServiceResponse();

                pmresponse.Result = result;
                return pmresponse;
            }),catchError(ServiceHelper.HandleServiceError));
        }

        );

    }


    getAgentSharedManifestsWorkspaceSummary() {


        return this._httpClient.get(this._apiUrl + '/GetAgentSharedManifestsWorkspaceSummary', ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetIsAgentSharedManifests(agentId: string, entityid: string) {

        var callTime = new Date();
        return defer(() => {
            return this._httpClient.get(this._apiUrl + '/GetIsAgentSharedManifests?' + 'agentId=' + agentId + '&entityid=' + entityid, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    get(id: string) {

        var callTime = new Date();
        return defer(() => {
            return this._httpClient.get(this._apiUrl + '/getsingle?' + 'id=' + id, ServiceHelper.GetHttpHeaders()).pipe(
                map((response) => {
                    //if (response instanceof HttpResponse) {
                        var pm = response;

                        var entity: AgentSharedManifestPM;
                        if (pm) {
                            entity = this.MapJsonToEntityPM(pm);
                        }

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = entity;

                        //var servertime = response.headers.get('ServerExecutionTime');
                        //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "AgentSharedManifest", "GetSinglePM", 'id=' + id);

                        return serviceResponse;
                    //}

                }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    GetAgentSharedManifestsForDashBoard(lastMonths: number, lastDays: number, selectedIndex: number) {

        var callTime = new Date();
        return defer(() => {
            return this._httpClient.get(this._apiUrl + '/GetAgentSharedManifestsForDashBoard?' + 'lastMonths=' + lastMonths + '&lastDays=' + lastDays + '&selectedIndex=' + selectedIndex, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
                var result = response;
                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = result;

                return serviceResponse;

            }),catchError(ServiceHelper.HandleServiceError));
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
