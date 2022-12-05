
import {Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';

@Injectable()
export class WarehouseEntryListExtendedService {

    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/WarehouseEntryExtended';
    }


    GetActiveWarehouseEntriesByShipmentId(shipmentId) {
        return this._http.get(this._apiUrl + '/GetWarehouseEntriesByShipmentId?shipmentId=' + shipmentId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetActiveWarehouseEntriesByWarehouseId(warehouseId) {
        return this._http.get(this._apiUrl + '/GetWarehouseEntriesByWarehouseId?warehouseId=' + warehouseId, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }), catchError(ServiceHelper.HandleServiceError));
    }

    GetRecentWarehouseEntries() {
        
        
        return this._http.get(this._apiUrl + '/GetRecentWarehouseEntries/', ServiceHelper.GetHttpHeaders()).pipe(map(response => {

            var pmresponse: ServiceResponse;
            pmresponse = new ServiceResponse();

            pmresponse.Result = response;
            return pmresponse;
        }),catchError(ServiceHelper.HandleServiceError));
    }





}

