import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { defer } from 'rxjs';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { catchError, map } from 'rxjs/operators';
import { ChartingDataClass } from '../../../Infrastructure/DataContracts/Dashboard/ChartingDataClass';

@Injectable()

export class DeclarationReferantDataWebService {
    private _http: HttpClient
    private _apiUrl: string;
    constructor() {
        this._http = ServiceHelper.HttpClient;
        this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/DeclarationReferantDataWebService';
    }
    GetQueriesCounts(refId:string, depId:string, transportMode:string) {
        return defer(() => {

            var authHeader = new Headers();
            authHeader.append('Token', SessionInfo.Token);
            authHeader.append('Content-Type', 'application/json');

            var serviceResponse: ServiceResponse;
            serviceResponse = new ServiceResponse();

            return this._http.get(this._apiUrl + "/GetQueriesCounts?refId=" + refId + "&depId=" + depId + "&transportMode=" + transportMode, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var serviceResponse: ServiceResponse = new ServiceResponse();
                serviceResponse.Result = response;
                return serviceResponse;
            }), catchError(ServiceHelper.HandleServiceError));
        }

        );
    }


    GetDeclarationReferantDataDashBoard(Tenant: number) {

        var url = this._apiUrl + '/GetDeclarationReferantDataDashBoard?tenant=' + Tenant;

        return defer(() => {
            return this._http.get(url, ServiceHelper.GetHttpHeaders()).pipe(map(response => {

                var allLists: any = response;
                var myList: Array<ChartingDataClass> = new Array<ChartingDataClass>();
                for (var key in allLists) {
                    var entity: ChartingDataClass;
                    entity = this.MapJsonToEntityListChartingDataClass(allLists[key]);
                    myList.push(entity);
                }


                var serviceResponse: ServiceResponse;
                serviceResponse = new ServiceResponse();
                serviceResponse.Result = myList;
                return serviceResponse;

            }), catchError(ServiceHelper.HandleServiceError));
        });
    }

    MapJsonToEntityListChartingDataClass(jsonList: any) {

        var entityList: ChartingDataClass;
        entityList = new ChartingDataClass();
        var jsonListKeys = Object.keys(jsonList);

        for (var key in jsonListKeys) {
            var property = jsonListKeys[key];
            entityList[property] = jsonList[property];
        }


        return entityList;
    }

}
