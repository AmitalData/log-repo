import { ServiceHelper } from './../../Utilities/ServiceHelper';

import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer } from 'rxjs';
import { CargoTrackingShipmentFilters } from 'src/CargoTracking/DataContracts/CargoTrackingShipmentFilters';
import { CaptchaParameters } from 'src/CargoTracking/DataContracts/CaptchaParameters';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { SessionInfo } from 'src/Infrastructure/Utilities/SessionInfo';
import { CargoTrackingSearchRequest } from 'src/CargoTracking/DataContracts/CargoTrackingSearchRequest';

@Injectable()
export class CargoTrackingSearchService {
    private _apiUrl: string;
    private httpHeaders: HttpHeaders;
    constructor(private _http: HttpClient , @Inject('BASE_URL') baseUrl: string) {
        // this._apiUrl = "http://localhost:9996/"  + 'api/CargoTrackingSearch';
        this._apiUrl = ServiceHelper.GetAppURL(baseUrl)  + 'api/CargoTrackingSearch';
    }


	getShipments(searchRequest:CargoTrackingSearchRequest) {
        var authHeaders = ServiceHelper.GetHeaders();

        var urlparameters = this.BuildURLParameters(searchRequest);

		return defer(() => {
            return this._http.get(this._apiUrl + urlparameters,
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
    private BuildURLParameters(searchRequest: any)
    {
        var urlparameters = '/GetShipments?';
        var mykeys = Object.keys(searchRequest);
        var addtionalFiltersValues = null;

        for (var i in mykeys) {
            var propName = mykeys[i];
            var propValue = searchRequest[propName];
            var ignoreFilter = ((propName.indexOf("Operator") > 0 && propValue == "Equals") || propName == "AdditionalFilters");

            if (urlparameters != "?") {
                urlparameters = urlparameters.concat('&');
            }

            if (!ignoreFilter) {
                propValue = encodeURIComponent(propValue);
                urlparameters = urlparameters.concat(propName.concat('=').concat(propValue));
            }
        }
        return urlparameters;
    }

    GetUserShipments(pageIndex: number, pageSize: number, shipmentFilters: CargoTrackingShipmentFilters) {
        var authHeaders = ServiceHelper.GetHeadersWithToken();

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
            authHeaders)
				.pipe(
					map((response: HttpResponse<any>) => {

						var list = response;

						return list;
					},catchError(error=>{
						return error;
					})));
		});
    }
    GetUserShipmentsCounter(shipmentFilters: CargoTrackingShipmentFilters) {
        var authHeaders = ServiceHelper.GetHeadersWithToken();
		return defer(() => {
            return this._http.get(this._apiUrl + '/GetUserShipmentsCount/?' + this.ParseFiltersIntoURL(shipmentFilters),
            authHeaders)
				.pipe(
					map((response: HttpResponse<any>) => {
						return response;
					},catchError(error=>{
						return error;
					})));
		});
	}

    GetCaptchaData() {
        return defer(() => {
            return this._http.get(this._apiUrl + '/GetCaptchaData/' ,
                { headers: ServiceHelper.GetHeaders() })
                .pipe(
                    map((response: HttpResponse<any>) => {
                        return response;
                    }, catchError(error => {
                        return error;
                    })));
        });
    }
    PostUserValidation(captchaParameters: CaptchaParameters) {

        var url = '/PutUserValidation/?';
        var callUrl = this._apiUrl.concat(url);
        return this._http.put(callUrl, captchaParameters, { headers: ServiceHelper.GetHeaders() }).pipe(
            map((response: ServiceResponse) => {
                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse = response;
                return serviceResponse;
            }),
            catchError(null));
    }



    private ParseFiltersIntoURL(shipmentFilters: CargoTrackingShipmentFilters)
    {
        var urlparameters = '';
        var keys = Object.keys(shipmentFilters);

        for (var i in keys) {
            var propertyName = keys[i];
            var propertyValue = shipmentFilters[propertyName];
            propertyValue = encodeURIComponent(propertyValue);
            urlparameters = urlparameters.concat(propertyName.concat('=').concat(propertyValue)).concat('&');
        }
        return urlparameters;
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

    getUserShipment(SecurityKey: string, tenant: number) {
        var authHeaders = ServiceHelper.GetHeadersWithToken();


        return defer(() => {
            return this._http.get(this._apiUrl + '/GetUserShipment/?' + 'SecurityKey=' + SecurityKey + '&tenant=' + tenant,
            authHeaders)
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

    TrackSearch(searchKey) {

        // var callUrl = this._apiUrl.concat('/PostSearchTrackAsync/?searchKey=' + searchKey);
        // return this._http.post(callUrl, null, { headers: ServiceHelper.GetHeaders() }).pipe(
        //     map((response: ServiceResponse) => {
        //         var serviceResponse: ServiceResponse = new ServiceResponse();
        //         serviceResponse = response;
        //         return serviceResponse;
        //     }),
        //     catchError(null));
    }



}

