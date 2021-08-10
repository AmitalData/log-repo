import {Injectable, } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import { PerformanceLogger } from '../../../Infrastructure/Utilities/PerformanceLogger';

@Injectable()

export class CargoTrackingMilestoneExtendedService {       
    private _http: HttpClient;
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/cargotrackingmilestoneviews';
    }

    getAll() {
        var callTime = new Date();

		return defer(() => {
			return this._http.get(this._apiUrl + '/getall', ServiceHelper.GetHttpFullHeaders())
				.pipe(
					map((response: HttpResponse<any>) => {

						var allLists = response.body;
						var _mappedListsArray: Array<CargoTrackingMilestoneList> = [];
						if (allLists) {
							for (var key in allLists) {			
								var entity: CargoTrackingMilestoneList = this.MapJsonToEntityList(allLists[key]);
								_mappedListsArray.push(entity);
							}
						}

						var serviceResponse: ServiceResponse = new ServiceResponse(); 
						serviceResponse.Result = _mappedListsArray;  
						serviceResponse.CallTime = callTime;
						var servertime = response.headers.get('ServerExecutionTime');
						PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "CargoTrackingMilestone", "GetAll", ""); 

						return serviceResponse;
					}),

					catchError(ServiceHelper.HandleServiceError));
		});
    }

    update(tenantList) {
        var callTime = new Date();

        return defer(() => {
            return this._http.get(this._apiUrl + '/getall', ServiceHelper.GetHttpFullHeaders())
                .pipe(
                    map((response: HttpResponse<any>) => {

                        var allLists = response.body;
                        var _mappedListsArray: Array<CargoTrackingMilestoneList> = [];
                        if (allLists) {
                            for (var key in allLists) {
                                var entity: CargoTrackingMilestoneList = this.MapJsonToEntityList(allLists[key]);
                                _mappedListsArray.push(entity);
                            }
                        }

                        var serviceResponse: ServiceResponse = new ServiceResponse();
                        serviceResponse.Result = _mappedListsArray;
                        serviceResponse.CallTime = callTime;
                        var servertime = response.headers.get('ServerExecutionTime');
                        PerformanceLogger.InsertPerformanceLog(callTime, new Date(), Number(servertime), "CargoTrackingMilestone", "GetAll", "");

                        return serviceResponse;
                    }),

                    catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityList(jsonList: any) {
        var entityList: CargoTrackingMilestoneList;
        entityList = new CargoTrackingMilestoneList();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }

        return entityList;
    }
}

export class CargoTrackingMilestoneList {
    Code: string;
    EnglishName: string;
    SearchFields: string;
    LocalName: string;
    Inactive: boolean;
}
