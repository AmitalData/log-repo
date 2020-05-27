
import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

import {WarehouseReleasePackagePM} from '../../EntityPMs/WarehouseReleasePackagePM';



@Injectable()
export class WarehouseReleasePackagePMExtendedService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/WarehouseReleasePackageExtended';
    }


    GetWarehouseReleasePackagePMListsByWarehouseReleaseId(warehouseReleaseId: string, tenant: number) {
        
        
        return this._http.get(this._apiUrl + "/getWarehouseReleasePackagePMListsByWarehouseReleaseId" + '?warehouseReleaseId=' + warehouseReleaseId +  '&tenant=' + tenant, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var result:any = response;
            var entity: WarehouseReleasePackagePM;
            var warehouseReleasePackagePMLists: WarehouseReleasePackagePM[];
            warehouseReleasePackagePMLists = new Array<WarehouseReleasePackagePM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                warehouseReleasePackagePMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = warehouseReleasePackagePMLists;
            return pmresponse;


        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetWarehouseReleasePackagePMThatNotUsedForAnyEntityLists() {
        
        
        return this._http.get(this._apiUrl + '/GetWarehouseReleasePackagePMThatNotUsedForAnyEntityLists', ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var result:any = response;
            var entity: WarehouseReleasePackagePM;
            var warehouseReleasePackagePMLists: WarehouseReleasePackagePM[];
            warehouseReleasePackagePMLists = new Array<WarehouseReleasePackagePM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                warehouseReleasePackagePMLists.push(entity);
            });
            var pmresponse: ServiceResponse = new ServiceResponse();
            pmresponse.Result = warehouseReleasePackagePMLists;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }


    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: WarehouseReleasePackagePM;
        entityPM = new WarehouseReleasePackagePM(null);
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }


}

