import { ServiceHelper } from './../../Utilities/ServiceHelper';

import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';
import {CargoTrackingShipmentSearchList} from '../../EntityLists/CargoTrackingShipmentSearchList';
import { CargoTrackingShipmentFilters } from 'src/CargoTracking/DataContracts/CargoTrackingShipmentFilters';

@Injectable()
export class CargoTrackingSearchService {
    private _apiUrl: string;
    constructor(private _http: HttpClient , @Inject('BASE_URL') baseUrl: string) {
        // this._apiUrl = "http://localhost:9996/"  + 'api/CargoTrackingSearch';
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl)  + 'api/CargoTrackingSearch';
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
					},catchError(error=>{
						return error;
					})));
		});
	}
    GetUserShipments(pageIndex: number, pageSize: number, shipmentFilters: CargoTrackingShipmentFilters) {
        var authHeaders = ServiceHelper.GetHeaders();

        var urlparameters = '';
		var mykeys = Object.keys(shipmentFilters);
		var addtionalFiltersValues = null;

		for (var i in mykeys) {
			var propName = mykeys[i];
			var propValue = shipmentFilters[propName];

            propValue = encodeURIComponent(propValue);
            urlparameters = urlparameters.concat(propName.concat('=').concat(propValue)).concat('&');

        }


		return defer(() => {
            return this._http.get(this._apiUrl + '/GetUserShipments/?' + urlparameters 
            + '&pageIndex=' + pageIndex
            + '&pageSize=' + pageSize,
             {headers: authHeaders})
				.pipe(
					map((response: HttpResponse<any>) => {

						var list = response;

						return list;
					},catchError(error=>{
						return error;
					})));
		});
	}
    getShipment(SecurityKey: string, tenant: number) {
        var authHeaders = ServiceHelper.GetHeaders();


		return defer(() => {
            return this._http.get(this._apiUrl + '/GetShipment/?' + 'SecurityKey=' + SecurityKey + '&tenant=' + tenant,
             {headers: authHeaders})
				.pipe(
					map((response: HttpResponse<any>) => {

						var list = response;

						return list;
					}));
		});
    }
    GetPublicShipmentReferences(securityKey: string, tenant: number) {
        var authHeaders = ServiceHelper.GetHeaders();


		return defer(() => {
            return this._http.get(this._apiUrl + '/GetShipmentReferences/?' + 'securityKey=' + securityKey + '&tenant=' + tenant,
             {headers: authHeaders})
				.pipe(
					map((response: HttpResponse<any>) => {

						var list = response;

						return list;
					},catchError(error=>{
						return error;
					})));
		});
	}


}

