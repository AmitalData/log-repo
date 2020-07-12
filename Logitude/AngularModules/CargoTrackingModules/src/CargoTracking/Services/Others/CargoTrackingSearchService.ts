import { ServiceHelper } from './../../Utilities/ServiceHelper';

import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';
import {CargoTrackingShipmentSearchList} from '../../EntityLists/CargoTrackingShipmentSearchList';

@Injectable()
export class CargoTrackingSearchService {
    private _apiUrl: string;
    constructor(private _http: HttpClient) {
        // this._apiUrl = "http://localhost:9996/"  + 'api/CargoTrackingSearch';
        this._apiUrl = ServiceHelper.GetAppURL()  + 'api/CargoTrackingSearch';
    }


	getShipments(searchKey: string, tenant: number) {
        var authHeaders = ServiceHelper.GetHeaders();


		return defer(() => {
            return this._http.get(this._apiUrl + '/GetShipments/?' + 'searchKey=' + searchKey + '&tenant=' + tenant,
             {headers: authHeaders})
				.pipe(
					map((response: HttpResponse<any>) => {

						var list = response;

						return list;
					}));
		});
	}
    getShipment(shipmentId: string, tenant: number) {
        var authHeaders = ServiceHelper.GetHeaders();


		return defer(() => {
            return this._http.get(this._apiUrl + '/GetShipment/?' + 'shipmentId=' + shipmentId + '&tenant=' + tenant,
             {headers: authHeaders})
				.pipe(
					map((response: HttpResponse<any>) => {

						var list = response;

						return list;
					}));
		});
	}


}

