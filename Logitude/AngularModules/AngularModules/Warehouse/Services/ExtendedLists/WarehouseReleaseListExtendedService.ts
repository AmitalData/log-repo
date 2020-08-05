
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

@Injectable()
export class WarehouseReleaseListExtendedService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/WarehouseReleaseExtended';
    }

    GetNumberOfConnectedWarehouseReleasesByChildEntityReference(childEntityReference: string) {

        return this._http.get(this._apiUrl + '/GetNumberOfConnectedWarehouseReleasesByChildEntityReference?' + 'childEntityReference=' + childEntityReference, ServiceHelper.GetHttpHeaders()).pipe(map(response => {
            var serviceresponse: ServiceResponse;
            serviceresponse = new ServiceResponse();

            serviceresponse.Result = response;
            return serviceresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }


    getWarehouseReleaseListsByShipmentId(shipmentId: string, tenant: number) {
        
        
        return this._http.get(this._apiUrl + '/GetWarehouseReleaseListsByShipmentId/?' + 'shipmentId=' + shipmentId + '&tenant=' + tenant + '&s=true', ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }

    GetRecentWarehouseReleases() {
        
        
        return this._http.get(this._apiUrl + '/GetRecentWarehouseReleases/', ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }



}

