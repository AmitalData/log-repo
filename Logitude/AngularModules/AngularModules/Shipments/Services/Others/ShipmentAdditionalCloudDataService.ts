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
import { AppTool } from 'Infrastructure/Tools';

@Injectable()

export class ShipmentAdditionalCloudDataService {
    private _httpClient: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._httpClient = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentAdditionalCloudData';
    }
    
    get(id: string) {

        var callTime = new Date();
        return defer(() => {
            return this._httpClient.get(this._apiUrl + '/getsingle?' + 'id=' + id, ServiceHelper.GetHttpHeaders()).pipe(
                map((response) => {
                    //if (response instanceof HttpResponse) {
                        var pm = response;



                        //var entity: AgentSharedManifestPM;
                        //if (pm) {
                        //    entity = this.MapJsonToEntityPM(pm);
                        //}

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = pm;

                        //var servertime = response.headers.get('ServerExecutionTime');
                       // PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ShipmentAdditionalCloudData", "GetSinglePM", 'id=' + id);

                        return serviceResponse;
                    //}
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    getsingledata(id: string) {


        var callTime = new Date();
        return defer(() => {
            return this._httpClient.get(this._apiUrl + '/GetSingleData?' + 'id=' + id, ServiceHelper.GetHttpHeaders()).pipe(
                map((response) => {
                    //if (response instanceof HttpResponse) {
                        var pm = response;



                        //var entity: AgentSharedManifestPM;
                        //if (pm) {
                        //    entity = this.MapJsonToEntityPM(pm);
                        //}

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = pm;

                        //var servertime = response.headers.get('ServerExecutionTime');
                       // PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ShipmentAdditionalCloudData", "GetSinglePM", 'id=' + id);

                        return serviceResponse;
                    //}
            }),catchError(ServiceHelper.HandleServiceError));
        });
    }

    update(entityPM: any) {
        return defer(() => {

            var response: ServiceResponse;
            response = new ServiceResponse();
            //errorsArray = [];
           
            
                var shipString: string;

                shipString = JSON.stringify(entityPM);
            //console.log(shipString);
            return this._httpClient.put(this._apiUrl + '/PutMain', shipString, ServiceHelper.GetHttpHeaders()).pipe(map((res) => {
                        var pm = res;
                        response.Result = pm;
                        return response;

                    }),catchError(ServiceHelper.HandleServiceError));
          
        });
    }

    updateUserID(entityPM: any) {
        return defer(() => {


            var response: ServiceResponse;
            response = new ServiceResponse();
            //errorsArray = [];


            var shipString: string;

            shipString = JSON.stringify(entityPM);
            //console.log(shipString);
            return this._httpClient.put(this._apiUrl + '/PutUserId', shipString, ServiceHelper.GetHttpHeadersWithoutToken()).pipe(map((res) => {
                    var pm = res;
                    response.Result = pm;
                    return response;

                }),catchError(ServiceHelper.HandleServiceError));

        });
    }

    getSingleWithoutToken(id: string, Tenant: number) {


        var callTime = new Date();
        var url = this._apiUrl + '/GetSingleWithoutToken?' + 'securityId=' + id;

        if (!AppTool.IsNullOrUndefined(Tenant)) {
            url += '&tenant=' + Tenant;
        }

        return defer(() => {
            return this._httpClient.get(url).pipe(
                map((response) => {
                    //if (response instanceof HttpResponse) {
                        var pm = response;



                        //var entity: AgentSharedManifestPM;
                        //if (pm) {
                        //    entity = this.MapJsonToEntityPM(pm);
                        //}

                        var serviceResponse: ServiceResponse;
                        serviceResponse = new ServiceResponse();
                        serviceResponse.Result = pm;

                        //var servertime = response.headers.get('ServerExecutionTime');
                        //PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "ShipmentAdditionalCloudData", "GetSinglePM", 'id=' + id);

                        return serviceResponse;
                    //}
                }), catchError(ServiceHelper.HandleServiceError));
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
