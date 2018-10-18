
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable}     from 'rxjs/Rx';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';

import {WarehouseEntryPackagePM} from '../../EntityPMs/WarehouseEntryPackagePM';



@Injectable()
export class WarehouseEntryPackagePMExtendedService {

    private _http: Http;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.Http;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/WarehouseEntryPackageExtended';
    }


    GetwarehouseEntryPackagePMListByCustomerIdAndWarehouseId(customerId: string , warehouseId: string  , tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/getwarehouseentrypackagepmlistbycustomeridandwarehouseid" + '?customerId=' + customerId + '&warehouseId=' + warehouseId + '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            var entity: WarehouseEntryPackagePM;
            var warehouseEntryPackagePMLists: WarehouseEntryPackagePM[];
            warehouseEntryPackagePMLists = new Array<WarehouseEntryPackagePM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                warehouseEntryPackagePMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = warehouseEntryPackagePMLists;
            return pmresponse;


        }).catch(ServiceHelper.HandleServiceError);
    }

   // string customerId, string warehouseId , string shipmentId
    GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId(shipmentId: string, customerId: string, warehouseId: string,  tenant: number) {
        var authHeader = new Headers();
        authHeader.append('Token', ServiceHelper.GetLoggedUserToken());
        return this._http.get(this._apiUrl + "/GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId" + '?shipmentId=' + shipmentId + '&customerId=' + customerId + '&warehouseId=' + warehouseId+  '&tenant=' + tenant, { headers: authHeader }).map(response => {

            var result = response.json();
            var entity: WarehouseEntryPackagePM;
            var warehouseEntryPackagePMLists: WarehouseEntryPackagePM[];
            warehouseEntryPackagePMLists = new Array<WarehouseEntryPackagePM>();
            result.forEach((item) => {
                entity = this.MapJsonToEntityPM(item);
                warehouseEntryPackagePMLists.push(entity);
            });
            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = warehouseEntryPackagePMLists;
            return pmresponse;


        }).catch(ServiceHelper.HandleServiceError);
    }

    
    MapJsonToEntityPM(jsonPM: any) {

        var entityPM: WarehouseEntryPackagePM;
        entityPM = new WarehouseEntryPackagePM(null);
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }


        entityPM.IsDirty = false;

        return entityPM;
    }


}

