
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

import {WarehouseReleasePackagePM} from '../../EntityPMs/WarehouseReleasePackagePM';



@Injectable()
export class WarehouseReleasePackagePMExtendedService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/WarehouseReleasePackageExtended';
    }


    GetWarehouseReleasePackagePMListsByWarehouseReleaseId(warehouseReleaseId: string, tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/getWarehouseReleasePackagePMListsByWarehouseReleaseId" + '?warehouseReleaseId=' + warehouseReleaseId +  '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
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


        }).catch(ServiceHelper.HandleServiceError);
    }

    GetWarehouseReleasePackagePMThatNotUsedForAnyEntityLists() {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken())
        return this._http.get(this._apiUrl + '/GetWarehouseReleasePackagePMThatNotUsedForAnyEntityLists', { headers: authHeader }).map(response => {
            var result = response.json();
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
        }).catch(ServiceHelper.HandleServiceError);
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

